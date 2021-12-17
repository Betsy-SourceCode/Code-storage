using System;
using System.Windows.Forms;


namespace GIP_CallWebApi
{
    public partial class LogonForm : Form
    {
        public LogonForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 手动录入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            //var CanShu = HttpHelper.HttpPost("http://api.eos-ts.h3c.com/user/v1.0/account/token", "{ \"account\":\"eos_scmdip_guest\",\"password\":\"tukw0df49ccfizgv\"}");
            //MessageBox.Show(CanShu);
        }
    }
}
