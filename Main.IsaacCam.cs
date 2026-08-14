// ================================================================
// Isaac 카메라 스트리밍 뷰어 — UI 껍데기
//   수신 기능은 전부 DLL: Ojw.CStream (AForge MJPEG/JPEG 수신기).
//   서버는 isaac_runner.py 의 MJPEG 서버 (기본 http://127.0.0.1:8081,
//   ?action=stream&cam=<이름>). Isaac 실행 중이어야 영상이 나온다.
// ================================================================
using System;
using System.Drawing;
using System.Windows.Forms;
using OpenJigWare;

namespace MakeUrdf
{
    public partial class Main
    {
        /// <summary>Apply DH 직후 호출 — Isaac 러너가 떠 있으면 새 URDF 를 조용히
        /// 재-Export 하고 RELOAD 를 보내 기구/카메라 변경을 즉시 반영한다.
        /// 시뮬 미실행(200ms 내 미접속)이면 조용히 무시. MuJoCo/PyBullet 러너는
        /// RELOAD 명령을 모르는 명령으로 무시한다.</summary>
        private void TrySimReloadAfterApply()
        {
            try
            {
                using (var tc = new System.Net.Sockets.TcpClient())
                {
                    var ar = tc.BeginConnect("127.0.0.1", 4000, null, null);
                    if (ar.AsyncWaitHandle.WaitOne(200, false) == false) return;
                    tc.EndConnect(ar);
                    // URDF + USDA 모두 재생성 — 러너가 URDF 모드든 USD 모드든 새 파일을 읽게.
                    // (URDF 만 갱신하면 USD 모드 러너가 낡은 robot_stl.usda 를 재참조해
                    //  "리스폰은 되는데 내용이 그대로" 가 된다 — 2026-08-14 실사고)
                    btnExportUsd_Click(null, EventArgs.Empty);
                    byte[] abyData = System.Text.Encoding.UTF8.GetBytes("RELOAD;");
                    tc.GetStream().Write(abyData, 0, abyData.Length);
                    Ojw.printf(txtLogHistory, "[Sim] Apply DH → URDF 재-Export + RELOAD 전송 (핫 리로드)\r\n");
                }
            }
            catch { }
        }

        private void btnIsaacCam_Click(object sender, EventArgs e)
        {
            const int _W = 640, _H = 480;
            Form frm = new Form();
            frm.Text = "Isaac 카메라 (Ojw.CStream ← isaac_runner MJPEG :8081)";
            frm.ClientSize = new Size(_W + 20, _H + 60);
            frm.StartPosition = FormStartPosition.CenterParent;
            frm.FormBorderStyle = FormBorderStyle.FixedSingle;
            frm.MaximizeBox = false;

            Panel pnView = new Panel();
            pnView.SetBounds(10, 10, _W, _H);
            pnView.BackColor = Color.Black;
            frm.Controls.Add(pnView);

            TextBox txtUrl = new TextBox();
            txtUrl.SetBounds(10, _H + 22, 330, 25);
            txtUrl.Text = "http://127.0.0.1:8081/?action=stream&cam=camera0";
            frm.Controls.Add(txtUrl);

            Ojw.CStream cs = new Ojw.CStream();
            Action<string> fnStart = delegate(string strCam)
            {
                if (strCam != null)
                {
                    // cam= 쿼리만 교체 (포트/호스트는 사용자가 바꾼 값 유지)
                    string strUrl = txtUrl.Text;
                    int nQ = strUrl.IndexOf("&cam=");
                    if (nQ >= 0) strUrl = strUrl.Substring(0, nQ);
                    txtUrl.Text = strUrl + "&cam=" + strCam;
                }
                if (cs.IsStreaming) cs.Stop();
                if (cs.Start_MJpeg(pnView, txtUrl.Text, 0, 0, _W, _H) == false)
                    MessageBox.Show("스트림 열기 실패 — Isaac 이 실행 중인지, 로그의\r\n" +
                                    "[MJPEG] 시작 줄과 포트를 확인하십시오.");
            };

            Button btnCam0 = new Button();
            btnCam0.SetBounds(346, _H + 20, 92, 28);
            btnCam0.Text = "camera0 손목";
            btnCam0.Click += delegate { fnStart("camera0"); };
            frm.Controls.Add(btnCam0);

            Button btnCam1 = new Button();
            btnCam1.SetBounds(442, _H + 20, 92, 28);
            btnCam1.Text = "camera1 천장";
            btnCam1.Click += delegate { fnStart("camera1"); };
            frm.Controls.Add(btnCam1);

            Button btnGo = new Button();
            btnGo.SetBounds(538, _H + 20, 56, 28);
            btnGo.Text = "연결";
            btnGo.Click += delegate { fnStart(null); };
            frm.Controls.Add(btnGo);

            Button btnStop = new Button();
            btnStop.SetBounds(598, _H + 20, 52, 28);
            btnStop.Text = "정지";
            btnStop.Click += delegate { try { if (cs.IsStreaming) cs.Stop(); } catch { } };
            frm.Controls.Add(btnStop);

            frm.FormClosing += delegate { try { if (cs.IsStreaming) cs.Stop(); } catch { } };
            frm.Show(this);

            // 열자마자 기본 카메라 연결 시도 (Isaac 미실행이면 안내만 뜸)
            fnStart("camera0");
        }
    }
}
