// ================================================================
// 축 상태 칸 — 각 Move/Rotate 버튼 아래의 "현재 값" 입력칸 (2026-09-11)
//
// ★아래쪽 Joint/DH 편집 패널과 내용은 겹치지만 역할이 다르다:
//     아래 패널 = **명령**   — 값을 적고 Apply 를 눌러 줄을 다시 쓴다
//     이 칸     = **명령 + 현재 상태** — 선택한 물체의 지금 값이 늘 떠 있고,
//                 옆의 ±버튼을 누르면 그 값이 따라 바뀌며, 직접 고쳐 넣을 수도 있다
//
// 해석(줄 → 값, 값 → 줄)은 **전부 DLL** 이다:
//     Ojw.DhBuilder.ReadLineState / SetLineValue / SetDhMeta
//   여기서는 칸에 넣고 빼는 일만 한다. 앱이 따로 파싱하면 "버튼이 올린 값"과
//   "칸에 보이는 값"이 어긋난다 — 그래서 커밋 경로도 ±버튼과 같은
//   ApplySelectedLineDelta 를 그대로 탄다(절대값은 델타로 환산해서 넘긴다).
//
// @STL 줄도 동일하게 동작한다 (버튼이 이미 @ 에 적용되는 것과 같은 대응):
//     X이동=offX  Y이동=offY  Z이동=offZ  X회전=tilt  Y회전=pan  Z회전=swing
// ================================================================
using System;
using System.Globalization;
using System.Windows.Forms;
using OpenJigWare;
using BuildAction = OpenJigWare.Ojw.BuildAction;
using DhBuilder = OpenJigWare.Ojw.DhBuilder;

namespace MakeUrdf
{
    public partial class Main
    {
        /// <summary>칸을 코드로 채우는 중 — Leave/CheckedChanged 가 되받아 다시 쓰는 것을 막는다.</summary>
        private bool m_bSyncingState = false;

        /// <summary>Dir 콤보 항목 (CLAUDE.md 의 Dir 규약 그대로).</summary>
        private static readonly string[] DIR_ITEMS = {
            "0 정방향 Z회전", "1 역방향 Z회전",
            "2 정방향 Z이동", "3 역방향 Z이동",
            "4 정방향 바퀴",  "5 역방향 바퀴",
        };

        private void InitAxisStatePanel()
        {
            cmbStateDir.Items.Clear();
            cmbStateDir.Items.AddRange(DIR_ITEMS);
            ClearStateFields();
        }

        /// <summary>텍스트박스 → 그 칸이 담당하는 축.</summary>
        private BuildAction StateActionOf(object sender)
        {
            if (sender == txtStateXMove) return BuildAction.RedMove;
            if (sender == txtStateYMove) return BuildAction.GreenMove;
            if (sender == txtStateZMove) return BuildAction.BlueMove;
            if (sender == txtStateXRotate) return BuildAction.RedRotate;
            if (sender == txtStateYRotate) return BuildAction.GreenRotate;
            if (sender == txtStateZRotate) return BuildAction.BlueRotate;
            return BuildAction.None;
        }

        private static string FmtState(float v)
        {
            if (v == (float)Math.Round(v) && Math.Abs(v) < 1e7f)
                return ((int)Math.Round(v)).ToString(CultureInfo.InvariantCulture);
            return v.ToString("0.######", CultureInfo.InvariantCulture);
        }

        /// <summary>선택 줄의 현재 상태를 칸에 채운다. 선택이 없거나 값 없는 축은 비운다.</summary>
        private void PopulateStateFields(int textLine)
        {
            if (txtStateXMove == null) return;          // Designer 초기화 전
            bool bPrev = m_bSyncingState;
            m_bSyncingState = true;
            try
            {
                string[] lines = txtDhParams.Lines;
                Ojw.DhBuilder.DhLineState st = DhBuilder.ReadLineState(lines, textLine);

                SetAxisBox(txtStateXMove, st, BuildAction.RedMove);
                SetAxisBox(txtStateYMove, st, BuildAction.GreenMove);
                SetAxisBox(txtStateZMove, st, BuildAction.BlueMove);
                SetAxisBox(txtStateXRotate, st, BuildAction.RedRotate);
                SetAxisBox(txtStateYRotate, st, BuildAction.GreenRotate);
                SetAxisBox(txtStateZRotate, st, BuildAction.BlueRotate);

                // joint / init / dir — DH 줄만 의미가 있다
                bool meta = st.Ok && st.HasMeta;
                txtStateJoint.Enabled = meta;
                chkStateInit.Enabled = meta;
                cmbStateDir.Enabled = meta;
                txtStateJoint.Text = meta ? st.Joint.ToString(CultureInfo.InvariantCulture) : "";
                chkStateInit.Checked = meta && st.Init == 1;
                cmbStateDir.SelectedIndex =
                    (meta && st.Dir >= 0 && st.Dir < DIR_ITEMS.Length) ? st.Dir : -1;

                lblStateNote.Text = !st.Ok ? st.Note
                                  : st.IsStl ? "@STL"
                                  : st.Note;          // 모터/수식/초기화 줄이면 사유, 아니면 빈 문자열
            }
            finally { m_bSyncingState = bPrev; }
        }

        private void SetAxisBox(TextBox tb, Ojw.DhBuilder.DhLineState st, BuildAction act)
        {
            bool has = st.Ok && st.HasValue(act);
            tb.Text = has ? FmtState(st.ValueOf(act)) : "";
            tb.Enabled = st.Ok;
            // Y 축은 DH 줄에서 합성 3줄의 값 운반 줄일 때만 값이 있다.
            // 값이 없어도 입력은 허용한다 — 숫자를 넣으면 ±버튼과 똑같이 합성 3줄이 삽입된다.
            tb.BackColor = has ? System.Drawing.SystemColors.Window
                               : System.Drawing.Color.FromArgb(245, 245, 245);
        }

        private void ClearStateFields()
        {
            if (txtStateXMove == null) return;
            bool bPrev = m_bSyncingState;
            m_bSyncingState = true;
            try
            {
                txtStateXMove.Text = txtStateYMove.Text = txtStateZMove.Text = "";
                txtStateXRotate.Text = txtStateYRotate.Text = txtStateZRotate.Text = "";
                txtStateJoint.Text = "";
                chkStateInit.Checked = false;
                cmbStateDir.SelectedIndex = -1;
                lblStateNote.Text = "";
            }
            finally { m_bSyncingState = bPrev; }
        }

        // ── 입력 확정 ────────────────────────────────────────────────

        private void StateField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter) return;
            e.SuppressKeyPress = true;      // 비프음 방지
            e.Handled = true;
            CommitStateField(sender);
        }

        private void StateField_Leave(object sender, EventArgs e)
        {
            CommitStateField(sender);
        }

        private void CommitStateField(object sender)
        {
            if (m_bSyncingState) return;
            if (m_nSelectedTextLine < 0) return;
            if (sender == txtStateJoint) { CommitStateMeta(); return; }

            TextBox tb = sender as TextBox;
            BuildAction act = StateActionOf(sender);
            if (tb == null || act == BuildAction.None) return;

            string s = tb.Text.Trim();
            if (s.Length == 0) { PopulateStateFields(m_nSelectedTextLine); return; }

            float want;
            if (!float.TryParse(s, NumberStyles.Float, CultureInfo.InvariantCulture, out want))
            {
                lblStatus.Text = "숫자를 입력하세요";
                PopulateStateFields(m_nSelectedTextLine);   // 되돌림
                return;
            }

            string[] lines = txtDhParams.Lines;
            Ojw.DhBuilder.DhLineState st = DhBuilder.ReadLineState(lines, m_nSelectedTextLine);
            float cur = st.Ok ? st.ValueOf(act) : 0f;
            float delta = want - cur;
            if (Math.Abs(delta) < 1e-4f) { PopulateStateFields(m_nSelectedTextLine); return; }

            // ★±버튼과 완전히 같은 경로 — 절대값을 델타로 바꿔 넘긴다.
            //   값을 못 넣는 줄(모터/수식/초기화)이면 거기서 새 줄을 끼워 준다.
            if (!ApplySelectedLineDelta(act, delta))
                PopulateStateFields(m_nSelectedTextLine);
        }

        /// <summary>joint / dir / init 확정 — DH 줄의 두 번째 [ ] 그룹만 다시 쓴다.</summary>
        private void StateMeta_Changed(object sender, EventArgs e)
        {
            if (m_bSyncingState) return;
            CommitStateMeta();
        }

        private void CommitStateMeta()
        {
            if (m_bSyncingState) return;
            int sel = m_nSelectedTextLine;
            if (sel < 0) return;
            string[] lines = txtDhParams.Lines;
            if (sel >= lines.Length) return;

            int joint;
            if (!int.TryParse(txtStateJoint.Text.Trim(), NumberStyles.Integer,
                              CultureInfo.InvariantCulture, out joint)) joint = -1;
            int dir = (cmbStateDir.SelectedIndex >= 0) ? cmbStateDir.SelectedIndex : 0;
            int init = chkStateInit.Checked ? 1 : 0;

            string newLine;
            if (!DhBuilder.SetDhMeta(lines[sel], joint, dir, init, out newLine))
            { PopulateStateFields(sel); return; }
            if (newLine == lines[sel]) return;

            lines[sel] = newLine;
            txtDhParams.Lines = lines;
            ResetDhTextFormatting();
            MakeAll(txtDhParams.Text);
            m_Builder.ImportFromText(txtDhParams.Text);

            int newDh = MapTextLineToDhIndex(sel);
            RefreshHistory(m_Builder.MapDhLineToStep(newDh));

            bool bPrev = m_bSyncingSelection;
            m_bSyncingSelection = true;
            try
            {
                m_nSelectedTextLine = sel;
                m_nSelectedDhLine = newDh;
                if (m_C3d != null)
                {
                    if (m_bFaceSnapMode || m_bMergeMode) m_C3d.User_SetPick(-1);
                    else m_C3d.User_SetPick(newDh);
                    m_C3d.User_ShowDhBall(m_bShowArrow ? newDh : -1);
                }
                HighlightDhTextLine(sel);
                string[] cur = txtDhParams.Lines;
                if (sel < cur.Length)
                {
                    PopulateDhEditFields(sel, cur[sel].Trim());
                    pnDhEdit.Visible = true;
                }
            }
            finally { m_bSyncingSelection = bPrev; }

            lblStatus.Text = String.Format("Line {0}: joint={1} dir={2} init={3}",
                sel + 1, joint, dir, init);
        }
    }
}
