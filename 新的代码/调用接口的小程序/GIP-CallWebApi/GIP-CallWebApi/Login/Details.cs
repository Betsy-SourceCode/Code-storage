using HZH_Controls;
using HZH_Controls.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Test;

namespace Login
{
    public partial class Details : Form
    {
        private static int count = 0; //记录总数据
        public Details()
        {
            InitializeComponent();
        }
        WebStationEntities db = new WebStationEntities();
        private void Details_Load(object sender, EventArgs e)
        {
            //分页控件-设置总页数
            this.ucPagerControl2.PageIndex = 1;
            var DataCount = db.ActionLog2021.ToList().Count;
            if (DataCount % this.ucPagerControl2.PageSize != 0)
            {
                this.ucPagerControl2.PageCount = (DataCount / this.ucPagerControl2.PageSize) + 1;
            }
            else
            {
                this.ucPagerControl2.PageCount = (DataCount / this.ucPagerControl2.PageSize);
            }
            DetailsData(this.ucPagerControl2.PageIndex, this.ucPagerControl2.PageSize);
            List<object> lstSource = new List<object>();
            var List = db.ActionLog2021.ToList();
            var OrderByList = List.OrderBy(a => a.LogID);
            //把总数传给全局变量
            count = this.ucPagerControl2.PageCount;
            var i = 1;
            foreach (var item in OrderByList)
            {
                ActionLog2021 model = new ActionLog2021()
                {
                    LogID = i,
                    UserID = item.UserID,
                    ActionTime = item.ActionTime,
                    TableName = item.TableName,
                    Description = item.Description
                };
                lstSource.Add(model);
                i++;
            }
            //this.ucPagerControl2.DataSource = lstSource;
            //this.ucDataGridView1.ShowCount = 5;
        }
        private void tb_DrawItem(object sender, DrawItemEventArgs e)
        {
            StringFormat sf = new StringFormat();

            #region 头背景
            sf.LineAlignment = StringAlignment.Center;
            sf.Alignment = StringAlignment.Center;
            Rectangle rec = DataList.ClientRectangle;
            //获取背景图片，我的背景图片在项目资源文件中。
            Image backImage = Properties.Resources.tb_Title;
            e.Graphics.DrawImage(backImage, 0, 0, DataList.Width, DataList.ItemSize.Height + 5);
            #endregion
            #region  设置选择的标签的背景
            if (e.Index == DataList.SelectedIndex)
                e.Graphics.DrawImage(Properties.Resources.title, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            else
                e.Graphics.DrawImage(Properties.Resources.tb_Title, e.Bounds.X, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);
            e.Graphics.DrawString(((TabControl)sender).TabPages[e.Index].Text,
            System.Windows.Forms.SystemInformation.MenuFont, new SolidBrush(Color.Black), e.Bounds, sf);
            #endregion
            #region 重写标签名
            ColorConverter colorConverter = new ColorConverter();
            Color cwhite = (Color)colorConverter.ConvertFromString("white");
            SolidBrush white = new SolidBrush(cwhite);
            Rectangle rect0 = DataList.GetTabRect(0);
            Rectangle rect1 = DataList.GetTabRect(1);
            Rectangle rect2 = DataList.GetTabRect(2);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString("操作1", new Font("微软雅黑", 12), white, rect0, stringFormat);
            e.Graphics.DrawString("操作2", new Font("微软雅黑", 12), white, rect1, stringFormat);
            e.Graphics.DrawString("操作3", new Font("微软雅黑", 12), white, rect2, stringFormat);
            e.Graphics.DrawString("操作4", new Font("微软雅黑", 12), white, rect2, stringFormat);
            #endregion
        }

        private void User_TextChanged(object sender, EventArgs e)
        {

        }
        private void ucDataGridView1_Load(object sender, EventArgs e)
        {
            try
            {
                List<DataGridViewColumnEntity> lstCulumns = new List<DataGridViewColumnEntity>();
                //表头
                lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "LogID", HeadText = "编号", Width = 50, WidthType = SizeType.Absolute });
                lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "UserID", HeadText = "姓名", Width = 50, WidthType = SizeType.Absolute });
                lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "ActionTime", HeadText = "年龄", Width = 50, WidthType = SizeType.Absolute });
                //lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "TableName", HeadText = "生日", Width = 500, WidthType = SizeType.Absolute, Format = (a) => { return ((DateTime)a).ToString("yyyy-MM-dd"); } });
                lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "TableName", HeadText = "年龄", Width = 50, WidthType = SizeType.Absolute });
                //lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Description", HeadText = "性别", Width = 50, WidthType = SizeType.Absolute, Format = (a) => { return ((int)a) == 0 ? "女" : "男"; } });
                lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Description", HeadText = "年龄", Width = 500, WidthType = SizeType.Absolute });
                this.ucDataGridView1.Columns = lstCulumns;
                //this.ucDataGridView1.IsShowCheckBox = true;
                DetailsData(this.ucPagerControl2.PageIndex, this.ucPagerControl2.PageSize);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// 数据源
        /// </summary>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        private void DetailsData(int PageIndex, int PageSize)
        {
            //分页
            List<object> lstSource = new List<object>();
            var List = db.ActionLog2021.ToList().OrderBy(a => a.LogID);
            var data = List.Skip((this.ucPagerControl2.PageIndex - 1) * this.ucPagerControl2.PageSize).Take(this.ucPagerControl2.PageSize).ToList<ActionLog2021>();//分页
            foreach (var item in data)
            {
                ActionLog2021 model = new ActionLog2021()
                {
                    LogID = item.LogID,
                    UserID = item.UserID,
                    ActionTime = item.ActionTime,
                    TableName = item.TableName,
                    Description = item.Description
                };
                lstSource.Add(model);
            }
            //更新表格的数据源
            this.ucDataGridView1.DataSource = lstSource;
        }

        private void ucPagerControl2_Click(object sender, EventArgs e)
        {
            DetailsData(this.ucPagerControl2.PageIndex, this.ucPagerControl2.PageSize);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DetailsData(this.ucPagerControl2.PageIndex, this.ucPagerControl2.PageSize);
        }

        private void ucPagerControl2_Load(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Handles the BtnClick event of the page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs" /> instance containing the event data.</param>
        private void page_BtnClick(object sender, EventArgs e)
        {
            int tempPageIndex = (sender as UCBtnExt).BtnText.ToInt();
            DetailsData(int.Parse(this.txtPage.InputText), tempPageIndex);
        }

        //控件重写
        private void btnFirst_BtnClick_1(object sender, EventArgs e)
        {
            //第一页
            this.ucPagerControl2.FirstPage();
            DetailsData(1, this.ucPagerControl2.PageSize);
        }

        private void btnPrevious_BtnClick(object sender, EventArgs e)
        {
            //上一页
            this.ucPagerControl2.PreviousPage();
            DetailsData(this.ucPagerControl2.PageIndex - 1, this.ucPagerControl2.PageSize);
        }

        private void ucBtnExt1_BtnClick(object sender, EventArgs e)
        {
            //下一页
            this.ucPagerControl2.NextPage();
            DetailsData(this.ucPagerControl2.PageIndex + 1, this.ucPagerControl2.PageSize);
        }

        private void btnEnd_BtnClick(object sender, EventArgs e)
        {
            //最后一页
            this.ucPagerControl2.EndPage();
            DetailsData(count, this.ucPagerControl2.PageSize);
        }
        private void btnToPage_BtnClick(object sender, EventArgs e)
        {
            //如果文本框没有值默认为当前页码
            if (this.txtPage.InputText == "")
            {
                //取当前页数
                DetailsData(this.ucPagerControl2.PageIndex, this.ucPagerControl2.PageSize);
            }
            else
            {
                //如果数据的页码大于总页面给出提示
                if (int.Parse(this.txtPage.InputText) > count)
                {
                    MessageBox.Show("您输入的页码大于最大值，请重新输入！");
                }
                else
                {
                    //把文本框的值赋值给当前页码
                    this.ucPagerControl2.PageIndex = int.Parse(this.txtPage.InputText);
                    //获取页码数
                    DetailsData(int.Parse(this.txtPage.InputText), this.ucPagerControl2.PageSize);
                }
            }
            //清空文本框
            this.txtPage.InputText = "";
        }
    }
}
