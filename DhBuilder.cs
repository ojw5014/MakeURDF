using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace OpenJigWare
{
    partial class Ojw
    {
    public enum BuildAction
    {
        None,
        RedMove,      // X축 이동: A
        BlueMove,     // Z축 이동: D
        GreenMove,    // Y축 이동: 복합 3줄
        RedRotate,    // X축 회전: Alpha
        BlueRotate,   // Z축 회전: Theta
        GreenRotate,  // Y축 회전: 복합 3줄
        Motor,        // 모터 선언
        Init,         // 좌표 초기화
        Stl           // STL 모델 추가
    }

    public class DhStep
    {
        public BuildAction Action;
        public float Value;
        public int MotorNumber;
        public string Description;
        public string DhLines;
        public bool IsRaw;       // true = 텍스트에서 직접 임포트 (InjectGroupInfo 건너뜀)

        public override string ToString() { return Description; }
    }

    public class DhBuilder
    {
        private List<DhStep> m_lstSteps = new List<DhStep>();

        // 오버레이: 특정 스텝 뒤에 임시 하이라이트 DH 라인 삽입
        private int m_nOverlayIdx = -1;
        private string m_strOverlay = "";

        public int StepCount { get { return m_lstSteps.Count; } }
        public DhStep GetStep(int index) { return m_lstSteps[index]; }

        public void SetOverlay(int afterStepIdx, string dhLines)
        {
            m_nOverlayIdx = afterStepIdx;
            m_strOverlay = dhLines;
        }

        public void ClearOverlay()
        {
            m_nOverlayIdx = -1;
            m_strOverlay = "";
        }

        public static DhStep CreateStep(BuildAction action, float value, int motorNum = -1)
        {
            DhStep step = new DhStep();
            step.Action = action;
            step.Value = value;
            step.MotorNumber = motorNum;

            int v = (int)Math.Round(value);
            string sv = v.ToString();

            switch (action)
            {
                case BuildAction.RedMove:
                    step.Description = string.Format("빨간(X) {0}mm 이동", sv);
                    step.DhLines = string.Format("[{0},0,0,0],[-1,0,0]", sv);
                    break;
                case BuildAction.BlueMove:
                    step.Description = string.Format("파란(Z) {0}mm 이동", sv);
                    step.DhLines = string.Format("[0,{0},0,0],[-1,0,0]", sv);
                    break;
                case BuildAction.GreenMove:
                    step.Description = string.Format("초록(Y) {0}mm 이동", sv);
                    step.DhLines = string.Format(
                        "[0,0,90,0],[-1,0,0]\r\n[{0},0,0,0],[-1,0,0]\r\n[0,0,-90,0],[-1,0,0]", sv);
                    break;
                case BuildAction.RedRotate:
                    step.Description = string.Format("빨간(X)축 {0}도 회전", sv);
                    step.DhLines = string.Format("[0,0,0,{0}],[-1,0,0]", sv);
                    break;
                case BuildAction.BlueRotate:
                    step.Description = string.Format("파란(Z)축 {0}도 회전", sv);
                    step.DhLines = string.Format("[0,0,{0},0],[-1,0,0]", sv);
                    break;
                case BuildAction.GreenRotate:
                    step.Description = string.Format("초록(Y)축 {0}도 회전", sv);
                    step.DhLines = string.Format(
                        "[0,0,0,90],[-1,0,0]\r\n[0,0,{0},0],[-1,0,0]\r\n[0,0,0,-90],[-1,0,0]", sv);
                    break;
                case BuildAction.Motor:
                    step.Description = string.Format("모터 {0}번 추가", motorNum);
                    step.DhLines = string.Format("[0,0,0,0],[{0},0,0]", motorNum);
                    break;
                case BuildAction.Init:
                    step.Description = "좌표 초기화";
                    step.DhLines = "[0,0,0,0],[-1,0,1]";
                    break;
            }
            return step;
        }

        public static DhStep CreateStlStep(string filename, float offX, float offY, float offZ,
            float pan = 0, float tilt = 0, float swing = 0)
        {
            DhStep step = new DhStep();
            step.Action = BuildAction.Stl;
            step.Value = 0;
            step.MotorNumber = -1;
            step.Description = string.Format("STL: {0}", System.IO.Path.GetFileName(filename));
            step.DhLines = string.Format(CultureInfo.InvariantCulture,
                "@{0},-1,1.0,0,0,{1:F1},{2:F1},{3:F1},{4:F1},{5:F1},{6:F1}",
                filename, offX, offY, offZ, pan, tilt, swing);
            return step;
        }

        /// <summary>
        /// STL DhLines에서 파라미터 파싱.
        /// 형식: @filename,-1,1.0,type,group,offX,offY,offZ,pan,tilt,swing
        /// </summary>
        public static bool ParseStlParams(string dhLines,
            out string filename, out float offX, out float offY, out float offZ,
            out float pan, out float tilt, out float swing)
        {
            filename = ""; offX = offY = offZ = pan = tilt = swing = 0;
            if (string.IsNullOrEmpty(dhLines) || !dhLines.TrimStart().StartsWith("@"))
                return false;
            string[] parts = dhLines.TrimStart('@').Split(',');
            if (parts.Length < 11) return false;
            filename = parts[0].Trim();
            float.TryParse(parts[5].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out offX);
            float.TryParse(parts[6].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out offY);
            float.TryParse(parts[7].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out offZ);
            float.TryParse(parts[8].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out pan);
            float.TryParse(parts[9].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out tilt);
            float.TryParse(parts[10].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out swing);
            return true;
        }

        /// <summary>
        /// STL DhLines 재구성.
        /// </summary>
        public static string BuildStlDhLines(string filename,
            float offX, float offY, float offZ,
            float pan, float tilt, float swing)
        {
            return string.Format(CultureInfo.InvariantCulture,
                "@{0},-1,1.0,0,0,{1:F1},{2:F1},{3:F1},{4:F1},{5:F1},{6:F1}",
                filename, offX, offY, offZ, pan, tilt, swing);
        }

        public void AddStep(DhStep step)
        {
            m_lstSteps.Add(step);
        }

        public DhStep RemoveLastStep()
        {
            if (m_lstSteps.Count == 0) return null;
            DhStep last = m_lstSteps[m_lstSteps.Count - 1];
            m_lstSteps.RemoveAt(m_lstSteps.Count - 1);
            return last;
        }

        public void RemoveStepAt(int index)
        {
            if (index >= 0 && index < m_lstSteps.Count)
                m_lstSteps.RemoveAt(index);
        }

        public void Clear()
        {
            m_lstSteps.Clear();
        }

        /// <summary>
        /// DH 텍스트를 파싱하여 스텝으로 임포트. 주석/빈줄은 건너뜀.
        /// </summary>
        public void ImportFromText(string dhText)
        {
            Clear();
            if (string.IsNullOrEmpty(dhText)) return;

            string[] lines = dhText.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach (string raw in lines)
            {
                string trimmed = raw.Trim();
                if (trimmed.Length == 0 || trimmed.StartsWith("//"))
                    continue;

                DhStep step = new DhStep();
                step.IsRaw = true;
                step.DhLines = trimmed;

                if (trimmed.StartsWith("@"))
                {
                    step.Action = BuildAction.Stl;
                    step.Description = "STL: " + ExtractStlFilename(trimmed);
                }
                else
                {
                    float A, D, Theta, Alpha;
                    int Axis, Dir, Init;
                    ParseDhValues(trimmed, out A, out D, out Theta, out Alpha, out Axis, out Dir, out Init);

                    step.MotorNumber = (Axis >= 0) ? Axis : -1;

                    bool allZero = (A == 0 && D == 0 && Theta == 0 && Alpha == 0);
                    if (Init == 1 && allZero && Axis < 0)
                        step.Action = BuildAction.Init;
                    else if (Axis >= 0 && allZero)
                        step.Action = BuildAction.Motor;
                    else if (A != 0 && D == 0 && Theta == 0 && Alpha == 0 && Axis < 0)
                        step.Action = BuildAction.RedMove;
                    else if (D != 0 && A == 0 && Theta == 0 && Alpha == 0 && Axis < 0)
                        step.Action = BuildAction.BlueMove;
                    else if (Theta != 0 && A == 0 && D == 0 && Alpha == 0 && Axis < 0)
                        step.Action = BuildAction.BlueRotate;
                    else if (Alpha != 0 && A == 0 && D == 0 && Theta == 0 && Axis < 0)
                        step.Action = BuildAction.RedRotate;
                    else
                        step.Action = BuildAction.None;

                    step.Description = BuildImportDescription(A, D, Theta, Alpha, Axis, Dir, Init);
                }

                m_lstSteps.Add(step);
            }
        }

        /// <summary>DH 라인 인덱스 → 스텝 인덱스</summary>
        public int MapDhLineToStep(int dhLineIndex)
        {
            int dhLine = 0;
            for (int i = 0; i < m_lstSteps.Count; i++)
            {
                int lineCount = CountValidDhLines(m_lstSteps[i].DhLines);
                if (dhLineIndex < dhLine + lineCount) return i;
                dhLine += lineCount;
            }
            return -1;
        }

        /// <summary>스텝 인덱스 → 첫 번째 DH 라인 인덱스</summary>
        public int MapStepToDhLine(int stepIndex)
        {
            int dhLine = 0;
            for (int i = 0; i < stepIndex && i < m_lstSteps.Count; i++)
                dhLine += CountValidDhLines(m_lstSteps[i].DhLines);
            return dhLine;
        }

        private static int CountValidDhLines(string dhLines)
        {
            if (string.IsNullOrEmpty(dhLines)) return 1;
            string[] lines = dhLines.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            int count = 0;
            foreach (string l in lines)
            {
                string t = l.Trim();
                if (t.Length > 0 && !t.StartsWith("//")) count++;
            }
            return Math.Max(count, 1);
        }

        private static void ParseDhValues(string line, out float A, out float D,
            out float Theta, out float Alpha, out int Axis, out int Dir, out int Init)
        {
            A = D = Theta = Alpha = 0;
            Axis = -1; Dir = 0; Init = 0;

            // 인라인 주석 제거
            string s = line;
            int commentPos = s.IndexOf("//");
            if (commentPos >= 0) s = s.Substring(0, commentPos);

            s = s.Replace("[", "").Replace("]", "").Trim();
            string[] parts = s.Split(',');

            if (parts.Length >= 4)
            {
                float.TryParse(parts[0].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out A);
                float.TryParse(parts[1].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out D);
                float.TryParse(parts[2].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out Theta);
                float.TryParse(parts[3].Trim(), NumberStyles.Float, CultureInfo.InvariantCulture, out Alpha);
            }
            if (parts.Length >= 7)
            {
                int.TryParse(parts[4].Trim(), out Axis);
                int.TryParse(parts[5].Trim(), out Dir);
                int.TryParse(parts[6].Trim(), out Init);
            }
        }

        private static string ExtractStlFilename(string line)
        {
            string s = line.TrimStart();
            if (s.StartsWith("@["))
                s = s.Substring(2);
            else if (s.StartsWith("@"))
                s = s.Substring(1);
            int comma = s.IndexOf(',');
            if (comma >= 0)
                s = s.Substring(0, comma);
            return s.Trim().TrimEnd(']');
        }

        private static string BuildImportDescription(float A, float D, float Theta, float Alpha,
            int Axis, int Dir, int Init)
        {
            bool hasMotor = (Axis >= 0);
            bool hasInit = (Init == 1);
            bool allZero = (A == 0 && D == 0 && Theta == 0 && Alpha == 0);

            // 단순 패턴 → 한국어 설명 (CreateStep과 동일)
            if (hasInit && allZero && !hasMotor)
                return "좌표 초기화";
            if (hasMotor && allZero)
                return string.Format("모터 {0}번", Axis);
            if (!hasMotor && !hasInit)
            {
                if (A != 0 && D == 0 && Theta == 0 && Alpha == 0)
                    return string.Format("빨간(X) {0}mm 이동", FmtNum(A));
                if (D != 0 && A == 0 && Theta == 0 && Alpha == 0)
                    return string.Format("파란(Z) {0}mm 이동", FmtNum(D));
                if (Theta != 0 && A == 0 && D == 0 && Alpha == 0)
                    return string.Format("파란(Z)축 {0}도 회전", FmtNum(Theta));
                if (Alpha != 0 && A == 0 && D == 0 && Theta == 0)
                    return string.Format("빨간(X)축 {0}도 회전", FmtNum(Alpha));
            }

            // 복합 패턴 → 압축 설명
            StringBuilder sb = new StringBuilder();
            if (hasMotor) sb.AppendFormat("모터{0}", Axis);
            if (hasInit) { if (sb.Length > 0) sb.Append(" "); sb.Append("초기화"); }
            if (A != 0) { if (sb.Length > 0) sb.Append(" "); sb.AppendFormat("A:{0}", FmtNum(A)); }
            if (D != 0) { if (sb.Length > 0) sb.Append(" "); sb.AppendFormat("D:{0}", FmtNum(D)); }
            if (Theta != 0) { if (sb.Length > 0) sb.Append(" "); sb.AppendFormat("θ:{0}", FmtNum(Theta)); }
            if (Alpha != 0) { if (sb.Length > 0) sb.Append(" "); sb.AppendFormat("α:{0}", FmtNum(Alpha)); }
            return sb.Length > 0 ? sb.ToString() : "(원점)";
        }

        private static string FmtNum(float v)
        {
            return (v == (int)v) ? ((int)v).ToString() : v.ToString("F1", CultureInfo.InvariantCulture);
        }

        public string GenerateFullDh()
        {
            StringBuilder sb = new StringBuilder();
            int currentGroup = -1;
            for (int i = 0; i < m_lstSteps.Count; i++)
            {
                var step = m_lstSteps[i];
                if (step.Action == BuildAction.Motor)
                    currentGroup = step.MotorNumber;

                if (step.IsRaw)
                    sb.AppendLine(step.DhLines);
                else
                    sb.AppendLine(InjectGroupInfo(step.DhLines, currentGroup));

                // 오버레이 삽입 (해당 스텝 바로 뒤)
                if (i == m_nOverlayIdx && !string.IsNullOrEmpty(m_strOverlay))
                    sb.AppendLine(m_strOverlay);
            }
            return sb.ToString();
        }

        public string GenerateFullDhWithPreview(DhStep tempStep)
        {
            StringBuilder sb = new StringBuilder();
            int currentGroup = -1;
            for (int i = 0; i < m_lstSteps.Count; i++)
            {
                var step = m_lstSteps[i];
                if (step.Action == BuildAction.Motor)
                    currentGroup = step.MotorNumber;

                if (step.IsRaw)
                    sb.AppendLine(step.DhLines);
                else
                    sb.AppendLine(InjectGroupInfo(step.DhLines, currentGroup));

                if (i == m_nOverlayIdx && !string.IsNullOrEmpty(m_strOverlay))
                    sb.AppendLine(m_strOverlay);
            }
            if (tempStep != null)
            {
                if (tempStep.Action == BuildAction.Motor)
                    currentGroup = tempStep.MotorNumber;
                sb.AppendLine(InjectGroupInfo(tempStep.DhLines, currentGroup));
            }
            return sb.ToString();
        }

        // DH 라인에 PickGroup 정보 주입
        // DH 라인: [A,D,Theta,Alpha],[Axis,Dir,Init] → [A,D,Theta,Alpha],[Axis,Dir,Init,1,groupNum]
        // @ 라인:  @model,-1,1.0,type,num,... → @model,-1,1.0,1,groupNum,...
        private string InjectGroupInfo(string dhLines, int groupNum)
        {
            bool hasAtLine = dhLines.IndexOf('@') >= 0;
            if (groupNum < 0 && !hasAtLine) return dhLines;

            string[] lines = dhLines.Split(new[] { "\r\n" }, StringSplitOptions.None);
            StringBuilder sb = new StringBuilder();
            foreach (string rawLine in lines)
            {
                string line = rawLine.Trim();
                if (line.Length == 0) continue;

                if (line.StartsWith("@"))
                {
                    // @ 라인의 type(idx 3), num(idx 4) 필드에 PickGroup 주입
                    string[] parts = line.Split(',');
                    if (parts.Length >= 5)
                    {
                        int pickGroup = groupNum >= 0 ? groupNum : 1;
                        parts[3] = "1";
                        parts[4] = pickGroup.ToString();
                        line = string.Join(",", parts);
                    }
                }
                else if (groupNum >= 0)
                {
                    int lastBracket = line.LastIndexOf(']');
                    if (lastBracket >= 0)
                        line = line.Substring(0, lastBracket) + string.Format(",1,{0}]", groupNum);
                }

                if (sb.Length > 0) sb.Append("\r\n");
                sb.Append(line);
            }
            return sb.ToString();
        }

        public int GetNextMotorNumber()
        {
            int maxMotor = -1;
            foreach (var step in m_lstSteps)
            {
                if (step.Action == BuildAction.Motor && step.MotorNumber > maxMotor)
                    maxMotor = step.MotorNumber;
            }
            return maxMotor + 1;
        }
    }
    } // partial class Ojw
}
