using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using OpenJigWare;

namespace MakeUrdf
{
    public partial class Main
    {
        // ================================================================
        // Episode Playback — ACT dataset 3D 재생
        // ================================================================

        // --- 그래프 객체 (UI 컨트롤은 Designer.cs에서 정의) ---
        private Ojw.C2D m_C2dQpos, m_C2dTgt;

        // --- 재생 상태 ---
        private string m_strDatasetRoot = "";
        private List<string> m_lstEpisodeDirs = new List<string>();
        private EpisodeData m_Episode = null;
        private int m_nPlayFrame = 0;
        private bool m_bPlaying = false;
        private Timer m_tmrPlayback = null;
        private float m_fPlaySpeed = 1.0f;
        private int m_nPlayIntervalMs = 33; // 기본 ~30fps

        // --- 에피소드 데이터 구조 ---
        private class EpisodeData
        {
            public string Dir;
            public int FrameCount;
            public int Dof;
            public float[][] Qpos;      // [frame][dof]
            public float[][] TgtPos;    // [frame][dof]
            public float[][] Eef;       // [frame][6]
            public string[] WristPaths; // [frame]
            public string[] ExtPaths;   // [frame]
            public float[] TimestampMs; // [frame]
            public int[] MotorIds;     // [dof] 실제 모터 ID
        }

        // ================================================================
        // 초기화
        // ================================================================
        private void InitPlaybackTab()
        {
            // UI 컨트롤은 Designer.cs에서 생성됨 — 여기서는 로직만 초기화

            // 재생 타이머
            m_tmrPlayback = new Timer();
            m_tmrPlayback.Interval = m_nPlayIntervalMs;
            m_tmrPlayback.Tick += tmrPlayback_Tick;

            // 기본 dataset 경로 시도
            string defaultDs = FindDefaultDatasetPath();
            if (!string.IsNullOrEmpty(defaultDs))
            {
                m_strDatasetRoot = defaultDs;
                RefreshEpisodeList();
            }
        }

        private string FindDefaultDatasetPath()
        {
            // MakeUrdf 실행 위치 기준으로 ACT dataset 경로 탐색
            string baseDir = Path.GetDirectoryName(Application.StartupPath);
            string[] candidates = new string[]
            {
                Path.Combine(Application.StartupPath, "dataset"),
                Path.Combine(Application.StartupPath, "..", "dataset"),       // 03/ACT/viewer/ → 03/ACT/dataset/
                Path.Combine(Application.StartupPath, "..", "lerobot_ojw", "03", "ACT", "dataset"),
                Path.Combine(Application.StartupPath, "..", "..", "..", "lerobot_ojw", "03", "ACT", "dataset"),
            };
            foreach (var c in candidates)
            {
                string full = Path.GetFullPath(c);
                if (Directory.Exists(full))
                    return full;
            }
            return "";
        }

        // ================================================================
        // 에피소드 목록
        // ================================================================
        private void RefreshEpisodeList()
        {
            m_lstEpisodeDirs.Clear();
            cmbEpisode.Items.Clear();

            if (string.IsNullOrEmpty(m_strDatasetRoot) || !Directory.Exists(m_strDatasetRoot))
                return;

            // dataset 폴더 내부에 또 dataset이 있을 수 있음
            SearchEpisodes(m_strDatasetRoot);

            foreach (var dir in m_lstEpisodeDirs)
                cmbEpisode.Items.Add(Path.GetFileName(dir));

            btnBrowseDataset.Text = Path.GetFileName(m_strDatasetRoot) + " (" + m_lstEpisodeDirs.Count + ")";

            if (cmbEpisode.Items.Count > 0)
                cmbEpisode.SelectedIndex = 0;
        }

        private void SearchEpisodes(string root)
        {
            try
            {
                foreach (var dir in Directory.GetDirectories(root))
                {
                    string name = Path.GetFileName(dir);
                    if (name.StartsWith("ep_") && File.Exists(Path.Combine(dir, "episode.csv")))
                        m_lstEpisodeDirs.Add(dir);
                    else if (name == "dataset" || name == "data")
                        SearchEpisodes(dir); // 재귀 탐색
                }
            }
            catch { }
        }

        // ================================================================
        // 에피소드 로딩
        // ================================================================
        private EpisodeData LoadEpisode(string epDir)
        {
            string csvPath = Path.Combine(epDir, "episode.csv");
            if (!File.Exists(csvPath)) return null;

            // DOF 감지: robot_config.txt 또는 기본값
            int dof = DetectDofForPlayback(epDir);

            // CSV 컬럼 레이아웃 감지
            bool inDetail = false;
            int eefStart, tgtStart, minCols;

            // episode_meta.json이 있으면 사용
            string metaPath = Path.Combine(epDir, "episode_meta.json");
            if (File.Exists(metaPath))
            {
                // meta가 있으면 간소 CSV 형식 (2*N+10)
                inDetail = false;
            }

            if (inDetail)
            {
                minCols = 10 * dof + 10;
                eefStart = 3 + 5 * dof;
                tgtStart = 3 + 5 * dof + 6;
            }
            else
            {
                minCols = 2 * dof + 10;
                eefStart = 3 + dof;
                tgtStart = 3 + dof + 6;
            }

            var qposList = new List<float[]>();
            var tgtList = new List<float[]>();
            var eefList = new List<float[]>();
            var wristList = new List<string>();
            var extList = new List<string>();
            var tsList = new List<float>();

            try
            {
                using (var reader = new StreamReader(csvPath, System.Text.Encoding.UTF8))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] cols = line.Split(',');
                        if (cols.Length < minCols) continue;

                        // 첫 번째 컬럼이 숫자인지 확인 (헤더 건너뛰기)
                        int frameIdx;
                        if (!int.TryParse(cols[0].Trim(), out frameIdx)) continue;

                        // qpos
                        float[] qpos = new float[dof];
                        for (int j = 0; j < dof; j++)
                            float.TryParse(cols[3 + j].Trim(), out qpos[j]);
                        qposList.Add(qpos);

                        // eef
                        float[] eef = new float[6];
                        for (int j = 0; j < 6; j++)
                            float.TryParse(cols[eefStart + j].Trim(), out eef[j]);
                        eefList.Add(eef);

                        // tgt_pos
                        float[] tgt = new float[dof];
                        for (int j = 0; j < dof; j++)
                            float.TryParse(cols[tgtStart + j].Trim(), out tgt[j]);
                        tgtList.Add(tgt);

                        // 이미지 경로
                        wristList.Add(cols[1].Trim());
                        extList.Add(cols[2].Trim());

                        // timestamp (마지막 열)
                        float ts;
                        float.TryParse(cols[cols.Length - 1].Trim(), out ts);
                        tsList.Add(ts);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("에피소드 로드 실패: " + ex.Message);
                return null;
            }

            if (qposList.Count == 0) return null;

            return new EpisodeData
            {
                Dir = epDir,
                FrameCount = qposList.Count,
                Dof = dof,
                Qpos = qposList.ToArray(),
                TgtPos = tgtList.ToArray(),
                Eef = eefList.ToArray(),
                WristPaths = wristList.ToArray(),
                ExtPaths = extList.ToArray(),
                TimestampMs = tsList.ToArray(),
                MotorIds = GetPlaybackMotorIds(dof, epDir)
            };
        }

        private int DetectDofForPlayback(string epDir)
        {
            // 1) robot_config.txt 탐색 (에피소드 상위 디렉토리들)
            string dir = epDir;
            for (int i = 0; i < 5; i++)
            {
                dir = Path.GetDirectoryName(dir);
                if (dir == null) break;
                string cfg = Path.Combine(dir, "robot_config.txt");
                if (File.Exists(cfg))
                {
                    int d = DetectDofFromConfig(cfg);
                    if (d > 0) return d;
                }
            }

            // 2) CSV 첫 행에서 컬럼 수로 추정
            string csv = Path.Combine(epDir, "episode.csv");
            try
            {
                using (var sr = new StreamReader(csv))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] cols = line.Split(',');
                        int dummy;
                        if (!int.TryParse(cols[0].Trim(), out dummy)) continue;
                        // 간소 형식: 2*N+10 -> N = (cols-10)/2
                        int n = (cols.Length - 10) / 2;
                        if (n >= 1 && n <= 20) return n;
                        break;
                    }
                }
            }
            catch { }

            return 6; // 기본값
        }

        private int DetectDofFromConfig(string path)
        {
            int[] ids = ParseMotorIdsFromConfig(path);
            return (ids != null) ? ids.Length : 0;
        }

        /// <summary>
        /// robot_config.txt의 [MOTORS] 섹션에서 실제 모터 ID 배열을 파싱
        /// </summary>
        private int[] ParseMotorIdsFromConfig(string path)
        {
            try
            {
                string section = null;
                foreach (var line in File.ReadAllLines(path, System.Text.Encoding.UTF8))
                {
                    string s = line.Trim();
                    if (string.IsNullOrEmpty(s) || s.StartsWith("#")) continue;
                    if (s.StartsWith("[") && s.EndsWith("]") && !s.Contains(","))
                    {
                        section = s.Substring(1, s.Length - 2);
                        continue;
                    }
                    if (section == "MOTORS")
                    {
                        string[] parts = s.Split(',');
                        var ids = new List<int>();
                        foreach (var p in parts)
                        {
                            int v;
                            if (int.TryParse(p.Trim(), out v))
                                ids.Add(v);
                        }
                        if (ids.Count > 0) return ids.ToArray();
                    }
                }
            }
            catch { }
            return null;
        }

        // ================================================================
        // C2D 그래프 초기화 & 실시간 그리기
        // ================================================================
        private static readonly Color[] s_GraphPalette = new Color[]
        {
            Color.FromArgb(255, 80, 80),   // 빨강
            Color.FromArgb(80, 255, 80),   // 녹색
            Color.FromArgb(80, 120, 255),  // 파랑
            Color.FromArgb(255, 255, 80),  // 노랑
            Color.FromArgb(255, 80, 255),  // 자홍
            Color.FromArgb(80, 255, 255),  // 시안
            Color.FromArgb(255, 160, 80),  // 주황
            Color.FromArgb(160, 80, 255),  // 보라
        };

        private void InitC2dGraphs()
        {
            // 기존 C2D 해제
            if (m_C2dQpos != null) { m_C2dQpos.Destroy(); m_C2dQpos = null; }
            if (m_C2dTgt != null) { m_C2dTgt.Destroy(); m_C2dTgt = null; }

            m_C2dQpos = new Ojw.C2D();
            m_C2dQpos.Create(picGraphQpos);
            m_C2dQpos.SetAngleX(0); m_C2dQpos.SetAngleY(0); m_C2dQpos.SetAngleZ(0);
            m_C2dQpos.SetScale(1.0);
            m_C2dQpos.SetBackColor(Color.FromArgb(30, 30, 30));

            m_C2dTgt = new Ojw.C2D();
            m_C2dTgt.Create(picGraphTgt);
            m_C2dTgt.SetAngleX(0); m_C2dTgt.SetAngleY(0); m_C2dTgt.SetAngleZ(0);
            m_C2dTgt.SetScale(1.0);
            m_C2dTgt.SetBackColor(Color.FromArgb(30, 30, 30));
        }

        /// <summary>
        /// 얇은 선으로 전체 데이터를 미리 그리고, 굵은 선으로 현재 프레임까지 덮어 그린다.
        /// </summary>
        private void DrawGraphsRealtime(int currentFrame)
        {
            if (m_Episode == null || m_C2dQpos == null) return;

            int dof = m_Episode.Dof;
            int totalFrames = m_Episode.FrameCount;
            if (totalFrames < 2) return;

            int w = picGraphQpos.Width;
            int h = picGraphQpos.Height;
            double halfW = w / 2.0;
            double halfH = h / 2.0;

            double xScale = (double)w / totalFrames;
            double yScale = halfH / 200.0;

            int[] motorIds = m_Episode.MotorIds;

            // --- qpos ---
            DrawOneGraph(m_C2dQpos, m_Episode.Qpos, dof, totalFrames, currentFrame,
                         halfW, halfH, xScale, yScale, motorIds);

            // --- tgt ---
            DrawOneGraph(m_C2dTgt, m_Episode.TgtPos, dof, totalFrames, currentFrame,
                         halfW, halfH, xScale, yScale, motorIds);
        }

        private void DrawOneGraph(Ojw.C2D c2d, float[][] data, int dof, int totalFrames,
                                  int currentFrame, double halfW, double halfH,
                                  double xScale, double yScale, int[] motorIds)
        {
            c2d.Clear();

            // 데이터의 min/max를 구해서 Y축 중심과 스케일을 자동 설정
            double dataMin = double.MaxValue, dataMax = double.MinValue;
            for (int i = 0; i < totalFrames; i++)
                for (int j = 0; j < dof; j++)
                {
                    if (data[i][j] < dataMin) dataMin = data[i][j];
                    if (data[i][j] > dataMax) dataMax = data[i][j];
                }
            double dataCenter = (dataMin + dataMax) / 2.0;
            double dataRange = dataMax - dataMin;
            if (dataRange < 10.0) dataRange = 10.0; // 최소 범위
            double autoYScale = (halfH * 2.0 * 0.85) / dataRange; // 85% 채우기

            // 1단계: 전체 데이터를 얇은 선(1px)으로 어둡게 미리 그리기
            c2d.SetPenWidth(1.0f);
            for (int j = 0; j < dof; j++)
            {
                Color c = s_GraphPalette[j % s_GraphPalette.Length];
                // 어두운 버전 (밝기 40%)
                Color dim = Color.FromArgb(c.R * 2 / 5, c.G * 2 / 5, c.B * 2 / 5);
                c2d.SetColor(dim);
                for (int i = 1; i < totalFrames; i++)
                {
                    double x1 = (i - 1) * xScale - halfW;
                    double x2 = i * xScale - halfW;
                    double y1 = (data[i - 1][j] - dataCenter) * autoYScale;
                    double y2 = (data[i][j] - dataCenter) * autoYScale;
                    c2d.Line(x1, y1, 0, x2, y2, 0);
                }
            }

            // 2단계: 현재 프레임까지 굵은 선(2px)으로 밝게 덮어 그리기
            c2d.SetPenWidth(2.0f);
            for (int j = 0; j < dof; j++)
            {
                Color c = s_GraphPalette[j % s_GraphPalette.Length];
                c2d.SetColor(c);
                for (int i = 1; i <= currentFrame && i < totalFrames; i++)
                {
                    double x1 = (i - 1) * xScale - halfW;
                    double x2 = i * xScale - halfW;
                    double y1 = (data[i - 1][j] - dataCenter) * autoYScale;
                    double y2 = (data[i][j] - dataCenter) * autoYScale;
                    c2d.Line(x1, y1, 0, x2, y2, 0);
                }
            }

            // 3단계: 현재 위치 커서 (세로 흰선)
            c2d.SetPenWidth(1.0f);
            c2d.SetColor(Color.White);
            double cursorX = currentFrame * xScale - halfW;
            c2d.Line(cursorX, -halfH, 0, cursorX, halfH, 0);

            // 4단계: 범례 (우상단에 DOF별 색상+이름)
            for (int j = 0; j < dof; j++)
            {
                Color c = s_GraphPalette[j % s_GraphPalette.Length];
                double lx = halfW - 50;
                double ly = halfH - 12 - j * 12;
                // 색상 선
                c2d.SetColor(c);
                c2d.SetPenWidth(2.0f);
                c2d.Line(lx, ly, 0, lx + 12, ly, 0);
                // 텍스트
                string label = (motorIds != null && j < motorIds.Length) ? "ID " + motorIds[j].ToString() : "J" + j.ToString();
                c2d.TextC(c, 7, label, lx + 16, ly, 0, false);
            }
            c2d.SetPenWidth(1.0f);

            c2d.Flush();
        }

        // ================================================================
        // 프레임 적용 (3D + 이미지 + 그래프 커서)
        // ================================================================
        private void ApplyFrame(int frame)
        {
            if (m_Episode == null || m_C3d == null) return;
            if (frame < 0 || frame >= m_Episode.FrameCount) return;

            m_nPlayFrame = frame;

            // 1) 3D 로봇에 관절각 적용
            int dof = m_Episode.Dof;
            int[] motorIds = GetPlaybackMotorIds(dof);
            for (int j = 0; j < dof; j++)
            {
                if (motorIds[j] >= 0)
                    m_C3d.SetData(motorIds[j], m_Episode.Qpos[frame][j]);
            }

            // 2) 카메라 이미지 표시
            LoadFrameImage(picWrist, Path.Combine(m_Episode.Dir, m_Episode.WristPaths[frame]));
            LoadFrameImage(picExternal, Path.Combine(m_Episode.Dir, m_Episode.ExtPaths[frame]));

            // 3) 실시간 그래프
            DrawGraphsRealtime(frame);
            RedrawPopupGraph();

            // 4) UI 업데이트
            if (trkFrame.Maximum >= frame)
                trkFrame.Value = frame;
            float ts = m_Episode.TimestampMs[frame];
            lblFrameInfo.Text = String.Format("Frame: {0}/{1}  t={2:F0}ms", frame, m_Episode.FrameCount - 1, ts);
        }

        private int[] GetPlaybackMotorIds(int dof, string epDir = null)
        {
            // 1) 3D 모델의 DH 파라미터에서 모터 ID 읽기 (테스트 탭에서 로드된 것)
            try
            {
                int[] kinIds = m_C3d.CalcInv_MotorIDs(0);
                if (kinIds != null && kinIds.Length >= dof)
                {
                    int[] result = new int[dof];
                    Array.Copy(kinIds, result, dof);
                    return result;
                }
            }
            catch { }

            // 2) robot_config.txt에서 실제 모터 ID 읽기
            string searchDir = epDir ?? (m_Episode != null ? m_Episode.Dir : null);
            if (searchDir != null)
            {
                string dir = searchDir;
                for (int i = 0; i < 5; i++)
                {
                    dir = Path.GetDirectoryName(dir);
                    if (dir == null) break;
                    string cfg = Path.Combine(dir, "robot_config.txt");
                    if (File.Exists(cfg))
                    {
                        int[] cfgIds = ParseMotorIdsFromConfig(cfg);
                        if (cfgIds != null && cfgIds.Length >= dof)
                        {
                            int[] result = new int[dof];
                            Array.Copy(cfgIds, result, dof);
                            return result;
                        }
                    }
                }
            }

            // 3) 기본값 0,1,2,...
            int[] ids = new int[dof];
            for (int i = 0; i < dof; i++)
                ids[i] = i;
            return ids;
        }

        private void LoadFrameImage(PictureBox pic, string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    // 파일 락 방지: 스트림으로 로드
                    using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                    {
                        var oldImg = pic.Image;
                        pic.Image = Image.FromStream(fs);
                        if (oldImg != null) oldImg.Dispose();
                    }
                }
            }
            catch { }
        }

        // ================================================================
        // 이벤트 핸들러
        // ================================================================
        private void btnBrowseDataset_Click(object sender, EventArgs e)
        {
            using (var dlg = new FolderBrowserDialog())
            {
                dlg.Description = "ACT dataset 폴더를 선택하세요";
                if (!string.IsNullOrEmpty(m_strDatasetRoot))
                    dlg.SelectedPath = m_strDatasetRoot;
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    m_strDatasetRoot = dlg.SelectedPath;
                    RefreshEpisodeList();
                }
            }
        }

        private void cmbEpisode_SelectedIndexChanged(object sender, EventArgs e)
        {
            StopPlayback();

            int idx = cmbEpisode.SelectedIndex;
            if (idx < 0 || idx >= m_lstEpisodeDirs.Count) return;

            m_Episode = LoadEpisode(m_lstEpisodeDirs[idx]);
            if (m_Episode == null)
            {
                lblFrameInfo.Text = "로드 실패";
                return;
            }

            trkFrame.Minimum = 0;
            trkFrame.Maximum = Math.Max(0, m_Episode.FrameCount - 1);
            trkFrame.Value = 0;

            InitC2dGraphs();
            ApplyFrame(0);

            lblFrameInfo.Text = String.Format("로드 완료: {0} frames, DOF={1}", m_Episode.FrameCount, m_Episode.Dof);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (m_Episode == null) return;
            m_bPlaying = true;
            m_tmrPlayback.Interval = Math.Max(1, (int)(m_nPlayIntervalMs / m_fPlaySpeed));
            m_tmrPlayback.Start();
            btnPlay.Enabled = false;
            btnPause.Enabled = true;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            m_bPlaying = false;
            m_tmrPlayback.Stop();
            btnPlay.Enabled = true;
            btnPause.Enabled = false;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            StopPlayback();
            m_nPlayFrame = 0;
            if (m_Episode != null)
                ApplyFrame(0);
        }

        private void StopPlayback()
        {
            m_bPlaying = false;
            if (m_tmrPlayback != null)
                m_tmrPlayback.Stop();
            btnPlay.Enabled = true;
            btnPause.Enabled = false;
        }

        private void trkFrame_Scroll(object sender, EventArgs e)
        {
            if (m_Episode == null) return;
            ApplyFrame(trkFrame.Value);
        }

        private void trkSpeed_Scroll(object sender, EventArgs e)
        {
            m_fPlaySpeed = trkSpeed.Value / 10.0f;
            lblPlaySpeed.Text = String.Format("{0:F1}x", m_fPlaySpeed);
            if (m_bPlaying)
                m_tmrPlayback.Interval = Math.Max(1, (int)(m_nPlayIntervalMs / m_fPlaySpeed));
        }

        private void tmrPlayback_Tick(object sender, EventArgs e)
        {
            if (m_Episode == null || !m_bPlaying) return;

            m_nPlayFrame++;
            if (m_nPlayFrame >= m_Episode.FrameCount)
            {
                m_nPlayFrame = 0; // 루프 재생
            }

            ApplyFrame(m_nPlayFrame);
        }
        // ================================================================
        // 그래프 팝업 (더블클릭으로 큰 창에서 보기)
        // ================================================================
        private Form m_frmPopupGraph = null;
        private PictureBox m_picPopupGraph = null;
        private Ojw.C2D m_C2dPopup = null;
        private bool m_bPopupIsQpos = true; // true=qpos, false=tgt

        private void picGraphQpos_DoubleClick(object sender, EventArgs e)
        {
            m_bPopupIsQpos = true;
            ShowPopupGraph("qpos");
        }

        private void picGraphTgt_DoubleClick(object sender, EventArgs e)
        {
            m_bPopupIsQpos = false;
            ShowPopupGraph("tgt_pos");
        }

        private void ShowPopupGraph(string title)
        {
            if (m_Episode == null) return;

            // 기존 팝업이 열려있으면 닫기
            if (m_frmPopupGraph != null && !m_frmPopupGraph.IsDisposed)
            {
                m_frmPopupGraph.Close();
                m_frmPopupGraph = null;
            }

            m_frmPopupGraph = new Form();
            m_frmPopupGraph.Text = title + " Graph";
            m_frmPopupGraph.Size = new Size(800, 400);
            m_frmPopupGraph.StartPosition = FormStartPosition.CenterParent;
            m_frmPopupGraph.BackColor = Color.FromArgb(30, 30, 30);

            m_picPopupGraph = new PictureBox();
            m_picPopupGraph.Dock = DockStyle.Fill;
            m_picPopupGraph.BackColor = Color.FromArgb(30, 30, 30);
            m_frmPopupGraph.Controls.Add(m_picPopupGraph);

            m_frmPopupGraph.Shown += (s, ev) =>
            {
                m_C2dPopup = new Ojw.C2D();
                m_C2dPopup.Create(m_picPopupGraph);
                m_C2dPopup.SetAngleX(0); m_C2dPopup.SetAngleY(0); m_C2dPopup.SetAngleZ(0);
                m_C2dPopup.SetScale(1.0);
                m_C2dPopup.SetBackColor(Color.FromArgb(30, 30, 30));
                RedrawPopupGraph();
            };

            m_frmPopupGraph.Resize += (s, ev) => RedrawPopupGraph();

            m_frmPopupGraph.FormClosed += (s, ev) =>
            {
                if (m_C2dPopup != null) { m_C2dPopup.Destroy(); m_C2dPopup = null; }
                m_picPopupGraph = null;
                m_frmPopupGraph = null;
            };

            m_frmPopupGraph.Show(this);
        }

        private void RedrawPopupGraph()
        {
            if (m_C2dPopup == null || m_Episode == null) return;
            if (m_picPopupGraph == null || m_picPopupGraph.Width < 10) return;

            int dof = m_Episode.Dof;
            int totalFrames = m_Episode.FrameCount;
            if (totalFrames < 2) return;

            int currentFrame = trkFrame.Value;
            int w = m_picPopupGraph.Width;
            int h = m_picPopupGraph.Height;
            double halfW = w / 2.0;
            double halfH = h / 2.0;
            double xScale = (double)w / totalFrames;
            double yScale = halfH / 200.0;

            float[][] data = m_bPopupIsQpos ? m_Episode.Qpos : m_Episode.TgtPos;
            DrawOneGraph(m_C2dPopup, data, dof, totalFrames, currentFrame,
                         halfW, halfH, xScale, yScale, m_Episode.MotorIds);
        }
    }
}
