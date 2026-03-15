using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace OpenJigWare
{
    partial class Ojw
    {
    public class StlBoundingBox
    {
        public float MinX, MinY, MinZ;
        public float MaxX, MaxY, MaxZ;

        // 삼각형 정점 데이터 (9개씩: v0x,v0y,v0z, v1x,v1y,v1z, v2x,v2y,v2z)
        public List<float> Vertices = new List<float>();

        public float CenterX { get { return (MinX + MaxX) / 2; } }
        public float CenterY { get { return (MinY + MaxY) / 2; } }
        public float CenterZ { get { return (MinZ + MaxZ) / 2; } }

        /// <summary>
        /// 바운딩 박스의 6면 중심점 반환
        /// face: 0=+X, 1=-X, 2=+Y, 3=-Y, 4=+Z, 5=-Z
        /// </summary>
        public void GetFaceCenter(int face, out float x, out float y, out float z)
        {
            x = CenterX; y = CenterY; z = CenterZ;
            switch (face)
            {
                case 0: x = MaxX; break;  // +X
                case 1: x = MinX; break;  // -X
                case 2: y = MaxY; break;  // +Y
                case 3: y = MinY; break;  // -Y
                case 4: z = MaxZ; break;  // +Z
                case 5: z = MinZ; break;  // -Z
            }
        }

        public string GetFaceName(int face)
        {
            switch (face)
            {
                case 0: return "+X";
                case 1: return "-X";
                case 2: return "+Y";
                case 3: return "-Y";
                case 4: return "+Z";
                case 5: return "-Z";
                default: return "";
            }
        }

        public bool IsValid
        {
            get { return MinX <= MaxX && MinY <= MaxY && MinZ <= MaxZ; }
        }

        /// <summary>
        /// 지정된 바운딩 박스 면에 있는 삼각형 정점들의 중심(centroid) 계산.
        /// 같은 2D 평면에 존재하는 도형의 실제 기하학적 중심을 반환.
        /// face: 0=+X, 1=-X, 2=+Y, 3=-Y, 4=+Z, 5=-Z
        /// </summary>
        public bool GetFaceGeometryCentroid(int face, out float cx, out float cy, out float cz)
        {
            cx = cy = cz = 0;

            if (Vertices.Count < 9) return false;

            // 바운딩 박스 크기 기반 허용 오차 (0.1%)
            float dimX = MaxX - MinX;
            float dimY = MaxY - MinY;
            float dimZ = MaxZ - MinZ;
            float maxDim = Math.Max(dimX, Math.Max(dimY, dimZ));
            if (maxDim <= 0) return false;
            float tolerance = maxDim * 0.001f;
            if (tolerance < 0.001f) tolerance = 0.001f;

            // 면의 기준값과 축 인덱스
            float faceVal;
            int axisIdx; // 0=X, 1=Y, 2=Z
            switch (face)
            {
                case 0: faceVal = MaxX; axisIdx = 0; break;
                case 1: faceVal = MinX; axisIdx = 0; break;
                case 2: faceVal = MaxY; axisIdx = 1; break;
                case 3: faceVal = MinY; axisIdx = 1; break;
                case 4: faceVal = MaxZ; axisIdx = 2; break;
                case 5: faceVal = MinZ; axisIdx = 2; break;
                default: return false;
            }

            float sumX = 0, sumY = 0, sumZ = 0;
            int count = 0;
            int numTris = Vertices.Count / 9;

            for (int i = 0; i < numTris; i++)
            {
                int baseIdx = i * 9;

                // 3개 정점 모두 해당 면 평면 위에 있는지 확인
                bool allOnFace = true;
                for (int v = 0; v < 3 && allOnFace; v++)
                {
                    float val = Vertices[baseIdx + v * 3 + axisIdx];
                    allOnFace = Math.Abs(val - faceVal) <= tolerance;
                }

                if (allOnFace)
                {
                    for (int v = 0; v < 3; v++)
                    {
                        sumX += Vertices[baseIdx + v * 3];
                        sumY += Vertices[baseIdx + v * 3 + 1];
                        sumZ += Vertices[baseIdx + v * 3 + 2];
                        count++;
                    }
                }
            }

            if (count > 0)
            {
                cx = sumX / count;
                cy = sumY / count;
                cz = sumZ / count;
                return true;
            }

            return false;
        }
    }

    public static class StlParser
    {
        public static StlBoundingBox Parse(string filePath)
        {
            // FileShare.ReadWrite → C3d가 파일을 잡고 있어도 읽기 가능
            byte[] data;
            using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                data = new byte[fs.Length];
                fs.Read(data, 0, data.Length);
            }

            // ASCII 형식 판별: "solid"로 시작하고 "facet" 포함
            if (data.Length > 84)
            {
                string sample = Encoding.ASCII.GetString(data, 0, Math.Min(1024, data.Length));
                if (sample.TrimStart().StartsWith("solid") && sample.Contains("facet"))
                    return ParseAscii(Encoding.ASCII.GetString(data));
            }

            return ParseBinary(data);
        }

        private static StlBoundingBox ParseBinary(byte[] data)
        {
            var bb = new StlBoundingBox();
            bb.MinX = bb.MinY = bb.MinZ = float.MaxValue;
            bb.MaxX = bb.MaxY = bb.MaxZ = float.MinValue;

            if (data.Length < 84) return bb;

            int triCount = BitConverter.ToInt32(data, 80);
            int offset = 84;

            for (int i = 0; i < triCount && offset + 50 <= data.Length; i++)
            {
                offset += 12; // 법선 벡터 건너뛰기
                for (int v = 0; v < 3; v++)
                {
                    float x = BitConverter.ToSingle(data, offset); offset += 4;
                    float y = BitConverter.ToSingle(data, offset); offset += 4;
                    float z = BitConverter.ToSingle(data, offset); offset += 4;
                    bb.Vertices.Add(x);
                    bb.Vertices.Add(y);
                    bb.Vertices.Add(z);
                    UpdateBB(bb, x, y, z);
                }
                offset += 2; // attribute byte count
            }

            return bb;
        }

        private static StlBoundingBox ParseAscii(string text)
        {
            var bb = new StlBoundingBox();
            bb.MinX = bb.MinY = bb.MinZ = float.MaxValue;
            bb.MaxX = bb.MaxY = bb.MaxZ = float.MinValue;

            string[] lines = text.Split('\n');
            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();
                if (!line.StartsWith("vertex")) continue;

                string[] parts = line.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 4)
                {
                    float x, y, z;
                    if (float.TryParse(parts[1], NumberStyles.Float, CultureInfo.InvariantCulture, out x) &&
                        float.TryParse(parts[2], NumberStyles.Float, CultureInfo.InvariantCulture, out y) &&
                        float.TryParse(parts[3], NumberStyles.Float, CultureInfo.InvariantCulture, out z))
                    {
                        bb.Vertices.Add(x);
                        bb.Vertices.Add(y);
                        bb.Vertices.Add(z);
                        UpdateBB(bb, x, y, z);
                    }
                }
            }

            return bb;
        }

        /// <summary>
        /// 지정된 면의 삼각형들만 추출하여 별도 바이너리 STL 파일로 저장.
        /// 반환값: 추출된 삼각형 개수 (0이면 해당 면에 삼각형 없음)
        /// </summary>
        public static int WriteFaceStl(StlBoundingBox bbox, int face, string outputPath)
        {
            if (bbox.Vertices.Count < 9) return 0;

            float dimX = bbox.MaxX - bbox.MinX;
            float dimY = bbox.MaxY - bbox.MinY;
            float dimZ = bbox.MaxZ - bbox.MinZ;
            float maxDim = Math.Max(dimX, Math.Max(dimY, dimZ));
            if (maxDim <= 0) return 0;
            float tolerance = maxDim * 0.001f;
            if (tolerance < 0.001f) tolerance = 0.001f;

            float faceVal;
            int axisIdx;
            switch (face)
            {
                case 0: faceVal = bbox.MaxX; axisIdx = 0; break;
                case 1: faceVal = bbox.MinX; axisIdx = 0; break;
                case 2: faceVal = bbox.MaxY; axisIdx = 1; break;
                case 3: faceVal = bbox.MinY; axisIdx = 1; break;
                case 4: faceVal = bbox.MaxZ; axisIdx = 2; break;
                case 5: faceVal = bbox.MinZ; axisIdx = 2; break;
                default: return 0;
            }

            // 면 위의 삼각형 인덱스 수집
            var faceTriIndices = new List<int>();
            int numTris = bbox.Vertices.Count / 9;
            for (int i = 0; i < numTris; i++)
            {
                int baseIdx = i * 9;
                bool allOnFace = true;
                for (int v = 0; v < 3 && allOnFace; v++)
                {
                    float val = bbox.Vertices[baseIdx + v * 3 + axisIdx];
                    allOnFace = Math.Abs(val - faceVal) <= tolerance;
                }
                if (allOnFace)
                    faceTriIndices.Add(i);
            }

            if (faceTriIndices.Count == 0) return 0;

            // 바이너리 STL 쓰기
            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(new byte[80]); // header
                bw.Write(faceTriIndices.Count);
                foreach (int idx in faceTriIndices)
                {
                    int baseIdx = idx * 9;
                    bw.Write(0f); bw.Write(0f); bw.Write(0f); // normal
                    for (int vi = 0; vi < 9; vi++)
                        bw.Write(bbox.Vertices[baseIdx + vi]);
                    bw.Write((ushort)0); // attribute
                }
            }
            return faceTriIndices.Count;
        }

        /// <summary>
        /// 지정된 면 위의 삼각형 인덱스 목록 반환
        /// </summary>
        public static List<int> GetFaceTriangleIndices(StlBoundingBox bbox, int face)
        {
            var result = new List<int>();
            if (bbox.Vertices.Count < 9) return result;

            float dimX = bbox.MaxX - bbox.MinX;
            float dimY = bbox.MaxY - bbox.MinY;
            float dimZ = bbox.MaxZ - bbox.MinZ;
            float maxDim = Math.Max(dimX, Math.Max(dimY, dimZ));
            if (maxDim <= 0) return result;
            float tolerance = maxDim * 0.001f;
            if (tolerance < 0.001f) tolerance = 0.001f;

            float faceVal;
            int axisIdx;
            switch (face)
            {
                case 0: faceVal = bbox.MaxX; axisIdx = 0; break;
                case 1: faceVal = bbox.MinX; axisIdx = 0; break;
                case 2: faceVal = bbox.MaxY; axisIdx = 1; break;
                case 3: faceVal = bbox.MinY; axisIdx = 1; break;
                case 4: faceVal = bbox.MaxZ; axisIdx = 2; break;
                case 5: faceVal = bbox.MinZ; axisIdx = 2; break;
                default: return result;
            }

            int numTris = bbox.Vertices.Count / 9;
            for (int i = 0; i < numTris; i++)
            {
                int baseIdx = i * 9;
                bool allOnFace = true;
                for (int v = 0; v < 3 && allOnFace; v++)
                {
                    float val = bbox.Vertices[baseIdx + v * 3 + axisIdx];
                    allOnFace = Math.Abs(val - faceVal) <= tolerance;
                }
                if (allOnFace)
                    result.Add(i);
            }
            return result;
        }

        /// <summary>
        /// 면 위의 삼각형들을 연결된 그룹(클러스터)으로 분류.
        /// 꼭짓점을 공유하는 삼각형들은 같은 클러스터로 묶임.
        /// 각 클러스터 = 원본 삼각형 인덱스 리스트.
        /// </summary>
        public static List<List<int>> ClusterFaceTriangles(StlBoundingBox bbox, int face)
        {
            List<int> faceTriIndices = GetFaceTriangleIndices(bbox, face);
            if (faceTriIndices.Count == 0) return new List<List<int>>();

            float maxDim = Math.Max(bbox.MaxX - bbox.MinX,
                            Math.Max(bbox.MaxY - bbox.MinY, bbox.MaxZ - bbox.MinZ));
            float snapGrid = maxDim * 0.0005f;
            if (snapGrid < 0.0005f) snapGrid = 0.0005f;

            int n = faceTriIndices.Count;
            int[] parent = new int[n];
            for (int i = 0; i < n; i++) parent[i] = i;

            // vertex hash → 처음 등장한 클러스터 내 인덱스
            var vertexToIdx = new Dictionary<long, int>();

            for (int ci = 0; ci < n; ci++)
            {
                int triIdx = faceTriIndices[ci];
                int baseIdx = triIdx * 9;
                for (int v = 0; v < 3; v++)
                {
                    float x = bbox.Vertices[baseIdx + v * 3];
                    float y = bbox.Vertices[baseIdx + v * 3 + 1];
                    float z = bbox.Vertices[baseIdx + v * 3 + 2];
                    long key = SnapKey(x, y, z, snapGrid);

                    int existing;
                    if (vertexToIdx.TryGetValue(key, out existing))
                        UF_Union(parent, ci, existing);
                    else
                        vertexToIdx[key] = ci;
                }
            }

            // 루트별 그룹화
            var groups = new Dictionary<int, List<int>>();
            for (int ci = 0; ci < n; ci++)
            {
                int root = UF_Find(parent, ci);
                if (!groups.ContainsKey(root))
                    groups[root] = new List<int>();
                groups[root].Add(faceTriIndices[ci]);
            }
            return new List<List<int>>(groups.Values);
        }

        private static long SnapKey(float x, float y, float z, float grid)
        {
            long ix = (long)Math.Round(x / grid);
            long iy = (long)Math.Round(y / grid);
            long iz = (long)Math.Round(z / grid);
            return (ix * 1000003L + iy) * 1000003L + iz;
        }

        private static int UF_Find(int[] p, int i)
        {
            while (p[i] != i) { p[i] = p[p[i]]; i = p[i]; }
            return i;
        }

        private static void UF_Union(int[] p, int a, int b)
        {
            int ra = UF_Find(p, a), rb = UF_Find(p, b);
            if (ra != rb) p[ra] = rb;
        }

        /// <summary>
        /// 특정 삼각형 인덱스 목록만 바이너리 STL로 저장
        /// </summary>
        public static int WriteClusterStl(StlBoundingBox bbox, List<int> triIndices, string outputPath)
        {
            if (triIndices == null || triIndices.Count == 0) return 0;

            using (var fs = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            using (var bw = new BinaryWriter(fs))
            {
                bw.Write(new byte[80]);
                bw.Write(triIndices.Count);
                foreach (int idx in triIndices)
                {
                    int baseIdx = idx * 9;
                    bw.Write(0f); bw.Write(0f); bw.Write(0f);
                    for (int vi = 0; vi < 9; vi++)
                        bw.Write(bbox.Vertices[baseIdx + vi]);
                    bw.Write((ushort)0);
                }
            }
            return triIndices.Count;
        }

        /// <summary>
        /// 삼각형 인덱스 목록의 정점 중심(centroid) 계산
        /// </summary>
        public static void GetClusterCentroid(StlBoundingBox bbox, List<int> triIndices,
            out float cx, out float cy, out float cz)
        {
            cx = cy = cz = 0;
            if (triIndices == null || triIndices.Count == 0) return;

            int count = 0;
            foreach (int triIdx in triIndices)
            {
                int baseIdx = triIdx * 9;
                for (int v = 0; v < 3; v++)
                {
                    cx += bbox.Vertices[baseIdx + v * 3];
                    cy += bbox.Vertices[baseIdx + v * 3 + 1];
                    cz += bbox.Vertices[baseIdx + v * 3 + 2];
                    count++;
                }
            }
            if (count > 0) { cx /= count; cy /= count; cz /= count; }
        }

        private static void UpdateBB(StlBoundingBox bb, float x, float y, float z)
        {
            if (x < bb.MinX) bb.MinX = x;
            if (x > bb.MaxX) bb.MaxX = x;
            if (y < bb.MinY) bb.MinY = y;
            if (y > bb.MaxY) bb.MaxY = y;
            if (z < bb.MinZ) bb.MinZ = z;
            if (z > bb.MaxZ) bb.MaxZ = z;
        }

        // =====================================================
        // 원형 윤곽선(Circular Contour) 감지
        // 바운딩박스 면의 경계선에서 원형 특징(구멍, 보스 등)을 찾음
        // =====================================================

        /// <summary>
        /// 바운딩박스 면의 원형 윤곽선 정보
        /// </summary>
        public class CircularContour
        {
            public int Face;                    // 바운딩박스 면 (0-5)
            public float CenterX, CenterY, CenterZ; // 3D 중심
            public float Radius;                // 평균 반지름
            public float Circularity;           // 원형도 (0~1, 높을수록 원에 가까움)
            public List<int> RimTriIndices;     // 윤곽선 인접 삼각형 인덱스

            public override string ToString()
            {
                return string.Format("Face{0} R={1:F1} C={2:F2} ({3}tri)",
                    Face, Radius, Circularity, RimTriIndices != null ? RimTriIndices.Count : 0);
            }
        }

        /// <summary>
        /// 모든 바운딩박스 면에서 원형 윤곽선(구멍, 보스 등)을 감지.
        /// 면의 삼각형 경계선을 추적하여 폐곡선을 만들고 원형도를 계산.
        /// </summary>
        public static List<CircularContour> FindCircularContours(StlBoundingBox bbox)
        {
            var result = new List<CircularContour>();
            if (bbox.Vertices.Count < 9) return result;

            float maxDim = Math.Max(bbox.MaxX - bbox.MinX,
                            Math.Max(bbox.MaxY - bbox.MinY, bbox.MaxZ - bbox.MinZ));
            if (maxDim <= 0) return result;
            float snapGrid = maxDim * 0.0005f;
            if (snapGrid < 0.0005f) snapGrid = 0.0005f;

            for (int face = 0; face < 6; face++)
            {
                var faceTris = GetFaceTriangleIndices(bbox, face);
                if (faceTris.Count == 0) continue;

                // 면의 법선축과 접선축
                int normalAxis;
                switch (face)
                {
                    case 0: case 1: normalAxis = 0; break;
                    case 2: case 3: normalAxis = 1; break;
                    default: normalAxis = 2; break;
                }
                int ax1 = (normalAxis + 1) % 3;
                int ax2 = (normalAxis + 2) % 3;

                // 정점 키 → 3D 위치, 에지 카운트
                var vertexPos = new Dictionary<long, float[]>();
                var edgeCnt = new Dictionary<long, Dictionary<long, int>>();

                foreach (int triIdx in faceTris)
                {
                    int b = triIdx * 9;
                    long[] keys = new long[3];
                    for (int v = 0; v < 3; v++)
                    {
                        float x = bbox.Vertices[b + v * 3];
                        float y = bbox.Vertices[b + v * 3 + 1];
                        float z = bbox.Vertices[b + v * 3 + 2];
                        keys[v] = SnapKey(x, y, z, snapGrid);
                        if (!vertexPos.ContainsKey(keys[v]))
                            vertexPos[keys[v]] = new float[] { x, y, z };
                    }

                    for (int e = 0; e < 3; e++)
                    {
                        long ka = Math.Min(keys[e], keys[(e + 1) % 3]);
                        long kb = Math.Max(keys[e], keys[(e + 1) % 3]);
                        if (ka == kb) continue;

                        if (!edgeCnt.ContainsKey(ka))
                            edgeCnt[ka] = new Dictionary<long, int>();
                        int cnt;
                        edgeCnt[ka].TryGetValue(kb, out cnt);
                        edgeCnt[ka][kb] = cnt + 1;
                    }
                }

                // 경계 에지 (count==1): 인접 그래프 생성
                var adj = new Dictionary<long, List<long>>();
                foreach (var outer in edgeCnt)
                {
                    long ka = outer.Key;
                    foreach (var inner in outer.Value)
                    {
                        if (inner.Value != 1) continue;
                        long kb = inner.Key;
                        if (!adj.ContainsKey(ka)) adj[ka] = new List<long>();
                        if (!adj.ContainsKey(kb)) adj[kb] = new List<long>();
                        adj[ka].Add(kb);
                        adj[kb].Add(ka);
                    }
                }

                // 폐곡선 추적
                var visited = new HashSet<long>();
                foreach (long startV in adj.Keys)
                {
                    if (visited.Contains(startV)) continue;

                    var loop = new List<long>();
                    long cur = startV;
                    long prev = -1;
                    bool closed = false;

                    while (true)
                    {
                        visited.Add(cur);
                        loop.Add(cur);

                        long next = -1;
                        foreach (long nb in adj[cur])
                        {
                            if (nb != prev && !visited.Contains(nb))
                            {
                                next = nb;
                                break;
                            }
                        }

                        if (next < 0)
                        {
                            // 시작점으로 돌아갈 수 있으면 폐곡선
                            if (loop.Count >= 3 && adj[cur].Contains(startV))
                                closed = true;
                            break;
                        }
                        prev = cur;
                        cur = next;
                    }

                    if (!closed || loop.Count < 6) continue;

                    // 3D 중심 계산
                    float cx = 0, cy = 0, cz = 0;
                    foreach (long key in loop)
                    {
                        float[] p = vertexPos[key];
                        cx += p[0]; cy += p[1]; cz += p[2];
                    }
                    cx /= loop.Count; cy /= loop.Count; cz /= loop.Count;

                    // 2D 평면 위의 반지름/원형도 계산
                    float[] dists = new float[loop.Count];
                    float meanR = 0;
                    float cAx1 = 0, cAx2 = 0;
                    foreach (long key in loop)
                    {
                        float[] p = vertexPos[key];
                        cAx1 += p[ax1]; cAx2 += p[ax2];
                    }
                    cAx1 /= loop.Count; cAx2 /= loop.Count;

                    for (int i = 0; i < loop.Count; i++)
                    {
                        float[] p = vertexPos[loop[i]];
                        float d1 = p[ax1] - cAx1;
                        float d2 = p[ax2] - cAx2;
                        dists[i] = (float)Math.Sqrt(d1 * d1 + d2 * d2);
                        meanR += dists[i];
                    }
                    meanR /= loop.Count;

                    if (meanR < snapGrid * 2) continue; // 너무 작은 윤곽선 제외

                    float variance = 0;
                    for (int i = 0; i < loop.Count; i++)
                    {
                        float diff = dists[i] - meanR;
                        variance += diff * diff;
                    }
                    variance /= loop.Count;
                    float stddev = (float)Math.Sqrt(variance);
                    float circularity = (meanR > 0) ? 1.0f - stddev / meanR : 0;

                    // 윤곽선 인접 삼각형 수집 (경계 에지를 가진 삼각형)
                    var loopVerts = new HashSet<long>(loop);
                    var rimTris = new List<int>();
                    foreach (int triIdx in faceTris)
                    {
                        int b = triIdx * 9;
                        int onLoop = 0;
                        for (int v = 0; v < 3; v++)
                        {
                            float x = bbox.Vertices[b + v * 3];
                            float y = bbox.Vertices[b + v * 3 + 1];
                            float z = bbox.Vertices[b + v * 3 + 2];
                            long key = SnapKey(x, y, z, snapGrid);
                            if (loopVerts.Contains(key)) onLoop++;
                        }
                        if (onLoop >= 2) rimTris.Add(triIdx);
                    }

                    var contour = new CircularContour();
                    contour.Face = face;
                    contour.CenterX = cx;
                    contour.CenterY = cy;
                    contour.CenterZ = cz;
                    contour.Radius = meanR;
                    contour.Circularity = circularity;
                    contour.RimTriIndices = rimTris;
                    result.Add(contour);
                }
            }

            return result;
        }
        // =====================================================
        // Fusion 360 스타일 면 선택: 레이캐스트 + 법선 기반 면 확장
        // =====================================================

        /// <summary>
        /// Möller-Trumbore 레이-삼각형 교차 검사.
        /// 히트 시 거리 t >= 0 반환, 미스 시 -1 반환.
        /// </summary>
        public static float RayTriangleIntersect(
            float ox, float oy, float oz,
            float dx, float dy, float dz,
            float v0x, float v0y, float v0z,
            float v1x, float v1y, float v1z,
            float v2x, float v2y, float v2z)
        {
            const float EPSILON = 1e-7f;
            float e1x = v1x - v0x, e1y = v1y - v0y, e1z = v1z - v0z;
            float e2x = v2x - v0x, e2y = v2y - v0y, e2z = v2z - v0z;

            float hx = dy * e2z - dz * e2y;
            float hy = dz * e2x - dx * e2z;
            float hz = dx * e2y - dy * e2x;

            float a = e1x * hx + e1y * hy + e1z * hz;
            if (a > -EPSILON && a < EPSILON) return -1;

            float f = 1.0f / a;
            float sx = ox - v0x, sy = oy - v0y, sz = oz - v0z;
            float u = f * (sx * hx + sy * hy + sz * hz);
            if (u < 0 || u > 1) return -1;

            float qx = sy * e1z - sz * e1y;
            float qy = sz * e1x - sx * e1z;
            float qz = sx * e1y - sy * e1x;

            float v = f * (dx * qx + dy * qy + dz * qz);
            if (v < 0 || u + v > 1) return -1;

            float t = f * (e2x * qx + e2y * qy + e2z * qz);
            return t >= 0 ? t : -1;
        }

        /// <summary>
        /// 메시의 모든 삼각형에 대해 레이캐스트. 가장 가까운 히트 삼각형 인덱스 반환 (-1 = 미스).
        /// </summary>
        public static int RaycastMesh(StlBoundingBox bbox,
            float ox, float oy, float oz,
            float dx, float dy, float dz)
        {
            int numTris = bbox.Vertices.Count / 9;
            int bestIdx = -1;
            float bestT = float.MaxValue;

            for (int i = 0; i < numTris; i++)
            {
                int b = i * 9;
                float t = RayTriangleIntersect(ox, oy, oz, dx, dy, dz,
                    bbox.Vertices[b], bbox.Vertices[b + 1], bbox.Vertices[b + 2],
                    bbox.Vertices[b + 3], bbox.Vertices[b + 4], bbox.Vertices[b + 5],
                    bbox.Vertices[b + 6], bbox.Vertices[b + 7], bbox.Vertices[b + 8]);
                if (t >= 0 && t < bestT)
                {
                    bestT = t;
                    bestIdx = i;
                }
            }
            return bestIdx;
        }

        /// <summary>
        /// 삼각형의 법선 벡터 계산 (정규화됨)
        /// </summary>
        public static void GetTriangleNormal(StlBoundingBox bbox, int triIdx,
            out float nx, out float ny, out float nz)
        {
            int b = triIdx * 9;
            float e1x = bbox.Vertices[b + 3] - bbox.Vertices[b];
            float e1y = bbox.Vertices[b + 4] - bbox.Vertices[b + 1];
            float e1z = bbox.Vertices[b + 5] - bbox.Vertices[b + 2];
            float e2x = bbox.Vertices[b + 6] - bbox.Vertices[b];
            float e2y = bbox.Vertices[b + 7] - bbox.Vertices[b + 1];
            float e2z = bbox.Vertices[b + 8] - bbox.Vertices[b + 2];

            nx = e1y * e2z - e1z * e2y;
            ny = e1z * e2x - e1x * e2z;
            nz = e1x * e2y - e1y * e2x;

            float len = (float)Math.Sqrt(nx * nx + ny * ny + nz * nz);
            if (len > 1e-10f) { nx /= len; ny /= len; nz /= len; }
            else { nx = ny = nz = 0; }
        }

        /// <summary>
        /// 히트된 삼각형에서 시작하여 동일 면(coplanar) 삼각형들을 BFS로 탐색.
        /// 에지 공유 + 법선 유사(threshold 이내) 조건으로 면 확장.
        /// Fusion 360 스타일 면 선택.
        /// </summary>
        public static List<int> FloodFillFace(StlBoundingBox bbox, int startTriIdx, float normalToleranceDeg = 15f)
        {
            int numTris = bbox.Vertices.Count / 9;
            if (startTriIdx < 0 || startTriIdx >= numTris)
                return new List<int>();

            float maxDim = Math.Max(bbox.MaxX - bbox.MinX,
                            Math.Max(bbox.MaxY - bbox.MinY, bbox.MaxZ - bbox.MinZ));
            float snapGrid = maxDim * 0.0005f;
            if (snapGrid < 0.0005f) snapGrid = 0.0005f;

            // 각 삼각형의 꼭짓점 키 계산
            long[][] triVertKeys = new long[numTris][];
            for (int i = 0; i < numTris; i++)
            {
                int b = i * 9;
                triVertKeys[i] = new long[3];
                for (int v = 0; v < 3; v++)
                    triVertKeys[i][v] = SnapKey(bbox.Vertices[b + v * 3],
                        bbox.Vertices[b + v * 3 + 1], bbox.Vertices[b + v * 3 + 2], snapGrid);
            }

            // 에지 → 삼각형 인덱스 맵 (nested dict으로 overflow 방지)
            var edgeMap = new Dictionary<long, Dictionary<long, List<int>>>();
            for (int i = 0; i < numTris; i++)
            {
                for (int e = 0; e < 3; e++)
                {
                    long ka = Math.Min(triVertKeys[i][e], triVertKeys[i][(e + 1) % 3]);
                    long kb = Math.Max(triVertKeys[i][e], triVertKeys[i][(e + 1) % 3]);
                    if (ka == kb) continue;

                    Dictionary<long, List<int>> inner;
                    if (!edgeMap.TryGetValue(ka, out inner))
                    {
                        inner = new Dictionary<long, List<int>>();
                        edgeMap[ka] = inner;
                    }
                    List<int> tris;
                    if (!inner.TryGetValue(kb, out tris))
                    {
                        tris = new List<int>();
                        inner[kb] = tris;
                    }
                    tris.Add(i);
                }
            }

            // 시작 삼각형의 법선
            float snx, sny, snz;
            GetTriangleNormal(bbox, startTriIdx, out snx, out sny, out snz);

            float cosThreshold = (float)Math.Cos(normalToleranceDeg * Math.PI / 180.0);

            // BFS
            var result = new List<int>();
            var visited = new HashSet<int>();
            var queue = new Queue<int>();

            queue.Enqueue(startTriIdx);
            visited.Add(startTriIdx);

            while (queue.Count > 0)
            {
                int curr = queue.Dequeue();
                result.Add(curr);

                for (int e = 0; e < 3; e++)
                {
                    long ka = Math.Min(triVertKeys[curr][e], triVertKeys[curr][(e + 1) % 3]);
                    long kb = Math.Max(triVertKeys[curr][e], triVertKeys[curr][(e + 1) % 3]);
                    if (ka == kb) continue;

                    Dictionary<long, List<int>> inner;
                    if (!edgeMap.TryGetValue(ka, out inner)) continue;
                    List<int> neighbors;
                    if (!inner.TryGetValue(kb, out neighbors)) continue;

                    foreach (int nb in neighbors)
                    {
                        if (nb == curr || visited.Contains(nb)) continue;

                        float nnx, nny, nnz;
                        GetTriangleNormal(bbox, nb, out nnx, out nny, out nnz);
                        float dot = snx * nnx + sny * nny + snz * nnz;

                        if (dot >= cosThreshold)
                        {
                            visited.Add(nb);
                            queue.Enqueue(nb);
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 면 그룹 내 내부 경계 루프(구멍)에 인접한 삼각형만 분리.
        /// 클릭한 삼각형이 내부 경계에 인접하면 해당 구멍 루프의 인접 삼각형만 반환.
        /// 아니면 원본 faceTris를 그대로 반환.
        /// </summary>
        public static List<int> FilterHoleNearbyTris(StlBoundingBox bbox, List<int> faceTris,
            int clickedTriIdx, float normalToleranceDeg = 15f)
        {
            if (faceTris.Count < 6) return faceTris;

            float maxDim = Math.Max(bbox.MaxX - bbox.MinX,
                            Math.Max(bbox.MaxY - bbox.MinY, bbox.MaxZ - bbox.MinZ));
            float snapGrid = maxDim * 0.0005f;
            if (snapGrid < 0.0005f) snapGrid = 0.0005f;

            // 면 그룹 내 삼각형 집합
            var faceSet = new HashSet<int>(faceTris);

            // 각 삼각형의 꼭짓점 키
            int numTris = bbox.Vertices.Count / 9;
            var triVertKeys = new Dictionary<int, long[]>();
            foreach (int tri in faceTris)
            {
                int b = tri * 9;
                long[] keys = new long[3];
                for (int v = 0; v < 3; v++)
                    keys[v] = SnapKey(bbox.Vertices[b + v * 3],
                        bbox.Vertices[b + v * 3 + 1], bbox.Vertices[b + v * 3 + 2], snapGrid);
                triVertKeys[tri] = keys;
            }

            // 전체 메시의 에지 → 삼각형 맵 (내부 경계 판별용)
            var globalEdgeMap = new Dictionary<long, Dictionary<long, List<int>>>();
            for (int i = 0; i < numTris; i++)
            {
                int b = i * 9;
                long[] keys = new long[3];
                for (int v = 0; v < 3; v++)
                    keys[v] = SnapKey(bbox.Vertices[b + v * 3],
                        bbox.Vertices[b + v * 3 + 1], bbox.Vertices[b + v * 3 + 2], snapGrid);
                for (int e = 0; e < 3; e++)
                {
                    long ka = Math.Min(keys[e], keys[(e + 1) % 3]);
                    long kb = Math.Max(keys[e], keys[(e + 1) % 3]);
                    if (ka == kb) continue;
                    Dictionary<long, List<int>> inner;
                    if (!globalEdgeMap.TryGetValue(ka, out inner))
                    { inner = new Dictionary<long, List<int>>(); globalEdgeMap[ka] = inner; }
                    List<int> tris;
                    if (!inner.TryGetValue(kb, out tris))
                    { tris = new List<int>(); inner[kb] = tris; }
                    tris.Add(i);
                }
            }

            // 면 그룹 내에서 내부 경계 에지 찾기:
            // 면 그룹 내 1회만 등장하는 에지 중에서 전체 메시에서는 2회 등장하는 에지 = 내부 경계 (구멍 경계)
            var faceEdgeCnt = new Dictionary<long, Dictionary<long, int>>();
            foreach (int tri in faceTris)
            {
                long[] keys = triVertKeys[tri];
                for (int e = 0; e < 3; e++)
                {
                    long ka = Math.Min(keys[e], keys[(e + 1) % 3]);
                    long kb = Math.Max(keys[e], keys[(e + 1) % 3]);
                    if (ka == kb) continue;
                    Dictionary<long, int> ci;
                    if (!faceEdgeCnt.TryGetValue(ka, out ci))
                    { ci = new Dictionary<long, int>(); faceEdgeCnt[ka] = ci; }
                    int cnt; ci.TryGetValue(kb, out cnt); ci[kb] = cnt + 1;
                }
            }

            // 내부 경계 에지에 인접한 정점 키 수집
            var innerBoundaryVertKeys = new HashSet<long>();
            foreach (var outer in faceEdgeCnt)
            {
                long ka = outer.Key;
                foreach (var inner in outer.Value)
                {
                    if (inner.Value != 1) continue; // 면 내에서 1회 등장 = 경계 에지
                    long kb = inner.Key;
                    // 전체 메시에서 이 에지가 2회 등장하면 내부 경계 (다른 면과 공유)
                    Dictionary<long, List<int>> ge;
                    if (globalEdgeMap.TryGetValue(ka, out ge))
                    {
                        List<int> geTris;
                        if (ge.TryGetValue(kb, out geTris) && geTris.Count >= 2)
                        {
                            innerBoundaryVertKeys.Add(ka);
                            innerBoundaryVertKeys.Add(kb);
                        }
                    }
                }
            }

            if (innerBoundaryVertKeys.Count == 0) return faceTris; // 내부 경계 없음

            // 클릭한 삼각형이 내부 경계에 인접한지 확인
            if (!triVertKeys.ContainsKey(clickedTriIdx)) return faceTris;
            long[] clickKeys = triVertKeys[clickedTriIdx];
            bool clickNearHole = false;
            for (int v = 0; v < 3; v++)
            {
                if (innerBoundaryVertKeys.Contains(clickKeys[v]))
                { clickNearHole = true; break; }
            }
            if (!clickNearHole) return faceTris; // 클릭이 구멍 근처가 아님

            // 내부 경계에 인접한 삼각형만 BFS로 수집
            // 시작: 클릭한 삼각형에서, 내부 경계 정점을 공유하는 삼각형들만 탐색
            var holeTris = new List<int>();
            var visitedTris = new HashSet<int>();
            var queue = new Queue<int>();
            queue.Enqueue(clickedTriIdx);
            visitedTris.Add(clickedTriIdx);

            while (queue.Count > 0)
            {
                int curr = queue.Dequeue();
                // 이 삼각형이 내부 경계 정점을 포함하는지 확인
                long[] curKeys = triVertKeys[curr];
                bool touchesBoundary = false;
                for (int v = 0; v < 3; v++)
                {
                    if (innerBoundaryVertKeys.Contains(curKeys[v]))
                    { touchesBoundary = true; break; }
                }
                if (!touchesBoundary) continue; // 경계와 무관한 삼각형은 건너뜀

                holeTris.Add(curr);

                // 인접 삼각형 탐색 (에지 공유)
                for (int e = 0; e < 3; e++)
                {
                    long ka = Math.Min(curKeys[e], curKeys[(e + 1) % 3]);
                    long kb = Math.Max(curKeys[e], curKeys[(e + 1) % 3]);
                    if (ka == kb) continue;
                    Dictionary<long, List<int>> inner;
                    if (!globalEdgeMap.TryGetValue(ka, out inner)) continue;
                    List<int> neighbors;
                    if (!inner.TryGetValue(kb, out neighbors)) continue;
                    foreach (int nb in neighbors)
                    {
                        if (visitedTris.Contains(nb) || !faceSet.Contains(nb)) continue;
                        visitedTris.Add(nb);
                        queue.Enqueue(nb);
                    }
                }
            }

            return holeTris.Count >= 3 ? holeTris : faceTris;
        }

        /// <summary>
        /// 면 그룹의 경계 에지(외곽선) 꼭짓점 추출.
        /// face 내 삼각형에서 1회만 등장하는 에지 = 경계.
        /// 반환: float[6] 배열 리스트 (x0,y0,z0, x1,y1,z1)
        /// </summary>
        public static List<float[]> GetBoundaryEdges(StlBoundingBox bbox, List<int> faceTris)
        {
            float maxDim = Math.Max(bbox.MaxX - bbox.MinX,
                            Math.Max(bbox.MaxY - bbox.MinY, bbox.MaxZ - bbox.MinZ));
            float snapGrid = maxDim * 0.0005f;
            if (snapGrid < 0.0005f) snapGrid = 0.0005f;

            var edgeCount = new Dictionary<long, Dictionary<long, int>>();
            var edgeVerts = new Dictionary<long, Dictionary<long, float[]>>();

            foreach (int triIdx in faceTris)
            {
                int b = triIdx * 9;
                long[] vk = new long[3];
                float[][] vp = new float[3][];
                for (int v = 0; v < 3; v++)
                {
                    vp[v] = new float[] {
                        bbox.Vertices[b + v * 3],
                        bbox.Vertices[b + v * 3 + 1],
                        bbox.Vertices[b + v * 3 + 2]
                    };
                    vk[v] = SnapKey(vp[v][0], vp[v][1], vp[v][2], snapGrid);
                }

                for (int e = 0; e < 3; e++)
                {
                    long ka = Math.Min(vk[e], vk[(e + 1) % 3]);
                    long kb = Math.Max(vk[e], vk[(e + 1) % 3]);
                    if (ka == kb) continue;

                    Dictionary<long, int> ci;
                    if (!edgeCount.TryGetValue(ka, out ci))
                    { ci = new Dictionary<long, int>(); edgeCount[ka] = ci; }
                    int cnt; ci.TryGetValue(kb, out cnt); ci[kb] = cnt + 1;

                    Dictionary<long, float[]> vi;
                    if (!edgeVerts.TryGetValue(ka, out vi))
                    { vi = new Dictionary<long, float[]>(); edgeVerts[ka] = vi; }
                    if (!vi.ContainsKey(kb))
                        vi[kb] = new float[] {
                            vp[e][0], vp[e][1], vp[e][2],
                            vp[(e + 1) % 3][0], vp[(e + 1) % 3][1], vp[(e + 1) % 3][2]
                        };
                }
            }

            var result = new List<float[]>();
            foreach (var outer in edgeCount)
                foreach (var inner in outer.Value)
                    if (inner.Value == 1)
                        result.Add(edgeVerts[outer.Key][inner.Key]);
            return result;
        }

        /// <summary>
        /// 경계 에지를 얇은 삼각형 스트립으로 STL 파일 작성.
        /// 법선 방향으로 살짝 들어올려 z-fighting 방지.
        /// </summary>
        public static int WriteEdgeHighlightStl(StlBoundingBox bbox, List<int> faceTris,
            string filePath, float faceNx, float faceNy, float faceNz)
        {
            var edges = GetBoundaryEdges(bbox, faceTris);
            if (edges.Count == 0) return 0;

            float maxDim = Math.Max(bbox.MaxX - bbox.MinX,
                            Math.Max(bbox.MaxY - bbox.MinY, bbox.MaxZ - bbox.MinZ));
            float thickness = maxDim * 0.004f;
            float lift = maxDim * 0.002f;

            using (var bw = new System.IO.BinaryWriter(System.IO.File.Create(filePath)))
            {
                bw.Write(new byte[80]);
                bw.Write((uint)(edges.Count * 2));

                foreach (var edge in edges)
                {
                    float x0 = edge[0] + faceNx * lift;
                    float y0 = edge[1] + faceNy * lift;
                    float z0 = edge[2] + faceNz * lift;
                    float x1 = edge[3] + faceNx * lift;
                    float y1 = edge[4] + faceNy * lift;
                    float z1 = edge[5] + faceNz * lift;

                    float dx = x1 - x0, dy = y1 - y0, dz = z1 - z0;
                    float px = dy * faceNz - dz * faceNy;
                    float py = dz * faceNx - dx * faceNz;
                    float pz = dx * faceNy - dy * faceNx;
                    float plen = (float)Math.Sqrt(px * px + py * py + pz * pz);
                    if (plen > 1e-10f)
                    { px = px / plen * thickness; py = py / plen * thickness; pz = pz / plen * thickness; }

                    bw.Write(faceNx); bw.Write(faceNy); bw.Write(faceNz);
                    bw.Write(x0); bw.Write(y0); bw.Write(z0);
                    bw.Write(x1); bw.Write(y1); bw.Write(z1);
                    bw.Write(x0 + px); bw.Write(y0 + py); bw.Write(z0 + pz);
                    bw.Write((ushort)0);

                    bw.Write(faceNx); bw.Write(faceNy); bw.Write(faceNz);
                    bw.Write(x1); bw.Write(y1); bw.Write(z1);
                    bw.Write(x1 + px); bw.Write(y1 + py); bw.Write(z1 + pz);
                    bw.Write(x0 + px); bw.Write(y0 + py); bw.Write(z0 + pz);
                    bw.Write((ushort)0);
                }
            }
            return edges.Count;
        }

        /// <summary>
        /// 선택된 면(faceTris)의 경계선에서 원형 특징(구멍, 보스 등)을 찾음.
        /// 경계 에지를 폐곡선으로 추적하고 원형도 계산.
        /// 원형도가 가장 높은 특징의 중심 좌표 반환 (없으면 null).
        /// </summary>
        public static float[] FindCircularFeatureOnFace(StlBoundingBox bbox, List<int> faceTris,
            float minCircularity = 0.7f)
        {
            if (faceTris.Count < 3) return null;

            float maxDim = Math.Max(bbox.MaxX - bbox.MinX,
                            Math.Max(bbox.MaxY - bbox.MinY, bbox.MaxZ - bbox.MinZ));
            float snapGrid = maxDim * 0.0005f;
            if (snapGrid < 0.0005f) snapGrid = 0.0005f;

            // 정점 키 → 3D 위치, 에지 카운트
            var vertexPos = new Dictionary<long, float[]>();
            var edgeCnt = new Dictionary<long, Dictionary<long, int>>();

            foreach (int triIdx in faceTris)
            {
                int b = triIdx * 9;
                long[] keys = new long[3];
                for (int v = 0; v < 3; v++)
                {
                    float x = bbox.Vertices[b + v * 3];
                    float y = bbox.Vertices[b + v * 3 + 1];
                    float z = bbox.Vertices[b + v * 3 + 2];
                    keys[v] = SnapKey(x, y, z, snapGrid);
                    if (!vertexPos.ContainsKey(keys[v]))
                        vertexPos[keys[v]] = new float[] { x, y, z };
                }

                for (int e = 0; e < 3; e++)
                {
                    long ka = Math.Min(keys[e], keys[(e + 1) % 3]);
                    long kb = Math.Max(keys[e], keys[(e + 1) % 3]);
                    if (ka == kb) continue;

                    Dictionary<long, int> ci;
                    if (!edgeCnt.TryGetValue(ka, out ci))
                    { ci = new Dictionary<long, int>(); edgeCnt[ka] = ci; }
                    int cnt; ci.TryGetValue(kb, out cnt); ci[kb] = cnt + 1;
                }
            }

            // 경계 에지 (count==1): 인접 그래프 생성
            var adj = new Dictionary<long, List<long>>();
            foreach (var outer in edgeCnt)
            {
                long ka = outer.Key;
                foreach (var inner in outer.Value)
                {
                    if (inner.Value != 1) continue;
                    long kb = inner.Key;
                    if (!adj.ContainsKey(ka)) adj[ka] = new List<long>();
                    if (!adj.ContainsKey(kb)) adj[kb] = new List<long>();
                    adj[ka].Add(kb);
                    adj[kb].Add(ka);
                }
            }

            // 폐곡선 추적 → 원형도 계산
            var visited = new HashSet<long>();
            float bestCircularity = 0;
            float[] bestCenter = null;

            foreach (long startV in adj.Keys)
            {
                if (visited.Contains(startV)) continue;

                var loop = new List<long>();
                long cur = startV;
                long prev = -1;
                bool closed = false;

                while (true)
                {
                    visited.Add(cur);
                    loop.Add(cur);

                    long next = -1;
                    foreach (long nb in adj[cur])
                    {
                        if (nb != prev && !visited.Contains(nb))
                        {
                            next = nb;
                            break;
                        }
                    }

                    if (next < 0)
                    {
                        if (loop.Count >= 6 && adj[cur].Contains(startV))
                            closed = true;
                        break;
                    }
                    prev = cur;
                    cur = next;
                }

                if (!closed || loop.Count < 6) continue;

                // 3D 중심 계산
                float cx = 0, cy = 0, cz = 0;
                foreach (long key in loop)
                {
                    float[] p = vertexPos[key];
                    cx += p[0]; cy += p[1]; cz += p[2];
                }
                cx /= loop.Count; cy /= loop.Count; cz /= loop.Count;

                // 3D 거리로 원형도 계산
                float meanR = 0;
                float[] dists = new float[loop.Count];
                for (int i = 0; i < loop.Count; i++)
                {
                    float[] p = vertexPos[loop[i]];
                    float dx = p[0] - cx, dy = p[1] - cy, dz = p[2] - cz;
                    dists[i] = (float)Math.Sqrt(dx * dx + dy * dy + dz * dz);
                    meanR += dists[i];
                }
                meanR /= loop.Count;

                if (meanR < snapGrid * 2) continue;

                float variance = 0;
                for (int i = 0; i < loop.Count; i++)
                {
                    float diff = dists[i] - meanR;
                    variance += diff * diff;
                }
                variance /= loop.Count;
                float stddev = (float)Math.Sqrt(variance);
                float circularity = (meanR > 0) ? 1.0f - stddev / meanR : 0;

                if (circularity >= minCircularity && circularity > bestCircularity)
                {
                    bestCircularity = circularity;
                    bestCenter = new float[] { cx, cy, cz };
                }
            }

            return bestCenter;
        }
    }
    } // partial class Ojw
}
