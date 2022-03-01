using GIP_CallWebApi;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Login
{
    public partial class LoginView : DevExpress.XtraEditors.XtraForm
    {
        //token
        public static string token;
        public LoginView()
        {
            InitializeComponent();
            #region 解决闪烁问题
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲
            #endregion
        }
        #region 解决闪烁问题
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0014) // 禁掉清除背景消息
                return;
            base.WndProc(ref m);
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
        #endregion

        private void LoginView_Load(object sender, EventArgs e)
        {

        }

        private void User_MouseEnter(object sender, EventArgs e)
        {
            skinPanel2.BackColor = Color.FromArgb(69, 159, 176);
        }

        private void User_MouseLeave(object sender, EventArgs e)
        {
            skinPanel2.BackColor = Color.FromArgb(127, 127, 127);
        }

        private void Password_MouseHover(object sender, EventArgs e)
        {

        }

        private void Password_MouseEnter(object sender, EventArgs e)
        {
            skinPanel1.BackColor = Color.FromArgb(69, 159, 176);
        }

        private void Password_MouseLeave(object sender, EventArgs e)
        {
            skinPanel1.BackColor = Color.FromArgb(127, 127, 127);
        }

        private void Login_Click(object sender, EventArgs e)
        {
            //改成手动登录
            //获得账号和密码文本框的值
            var User = this.User.Text;
            var Password = this.Password.Text;
            var url = "http://api.eos-ts.h3c.com/user/v1.0/account/token";
            //var LoginResult = HttpHelper.HttpPost(1, url, "{ \"account\":\"" + User + "\",\"password\":\"" + Password + "\"}");
            //if (LoginResult != "")
            //{
            //    //取token值赋值给全局变量
            //    token = LoginResult;
            //    //跳转到详情页
            //    Details details = new Details();
            //    details.Show();
            //    //this.Hide();
            //}
            //else
            //{
            //    DevExpress.XtraEditors.XtraMessageBox.Show("登录失败，账号或者密码错误请检查后重新尝试登录！");
            //}

        }

        private void Exit_Click(object sender, EventArgs e)
        {
            //退出程序
            Application.Exit();
        }
    }
}
