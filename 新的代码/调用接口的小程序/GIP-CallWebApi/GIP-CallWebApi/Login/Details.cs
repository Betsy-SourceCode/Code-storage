using GIP_CallWebApi;
using HZH_Controls;
using HZH_Controls.Controls;
using HZH_Controls.Forms;
using Login.Properties;
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
using Test.UC;
using WinformControlLibraryExtension;

namespace Login
{
    public partial class Details : Form
    {
        public Details()
        {
            InitializeComponent();
        }
        WebStationEntities db = new WebStationEntities();
        gipwip_R1Entities gipwip_R1 = new gipwip_R1Entities();
        List<object> listSource = new List<object>();
        public int i = 0;
        private void Details_Load(object sender, EventArgs e)
        {
            // tabcontrol设置为可以自定义绘制标签内容
            this.DataList.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tb_DrawItem);
            #region 接口页签控件初始化
            this.Factory_processes_ucComboxGrid.Refresh(); //重置下拉框控件
            this.Factory_reworks_ucComboxGrid.Refresh(); //重置下拉框控件
            this.Factory_sns_ucComboxGrid.Refresh(); //重置下拉框控件
            this.Factory_processes_dateExt.DatePicker.DateValue = DateTime.Now;
            this.Factory_reworks_dateExt.DatePicker.DateValue = DateTime.Now;
            this.Factory_sns_dateExt.DatePicker.DateValue = DateTime.Now;
            this.Factory_processes_dateExt.DatePicker.MaxValue = DateTime.Now;
            this.Factory_reworks_dateExt.DatePicker.MaxValue = DateTime.Now;
            this.Factory_sns_dateExt.DatePicker.MaxValue = DateTime.Now;
            ComboxGridDataSource(1, this.Factory_processes_dateExt.DatePicker.DateValue.ToString());
            ComboxGridDataSource(2, this.Factory_reworks_dateExt.DatePicker.DateValue.ToString());
            ComboxGridDataSource(3, this.Factory_sns_dateExt.DatePicker.DateValue.ToString());
            #endregion
            #region 表格页签
            //分页控件-设置总页数
            this.ucPagerControl.PageIndex = 1;
            var DataCount = gipwip_R1.Log_ErrorData.OrderByDescending(a => a.Error_Date).Count();
            if (DataCount % this.ucPagerControl.PageSize != 0)
            {
                this.ucPagerControl.PageCount = (DataCount / this.ucPagerControl.PageSize) + 1;
            }
            else
            {
                this.ucPagerControl.PageCount = (DataCount / this.ucPagerControl.PageSize);
            }
            initDataSource();
            this.ucPagerControl.DataSource = this.listSource;
            //委托事件添加具体事件
            this.ucPagerControl.ShowSourceChanged += m_page_ShowSourceChanged;
            #endregion
        }
        #region 窗口样式布局
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
            Rectangle rect3 = DataList.GetTabRect(3);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            e.Graphics.DrawString("操作1", new Font("微软雅黑", 12), white, rect0, stringFormat);
            e.Graphics.DrawString("操作2", new Font("微软雅黑", 12), white, rect1, stringFormat);
            e.Graphics.DrawString("操作3", new Font("微软雅黑", 12), white, rect2, stringFormat);
            e.Graphics.DrawString("操作4", new Font("微软雅黑", 12), white, rect3, stringFormat);
            #endregion
        }
        #endregion
        #region 接口页签
        /// <summary>
        /// 接口页签-下拉框列表数据源
        /// </summary>
        /// <param name="typeid">1-factory_processes，2-factory_reworks,3-Factory_sns</param>
        /// <param name="date"></param>
        private void ComboxGridDataSource(int typeid, string date)
        {
            try
            {
                //表头
                List<DataGridViewColumnEntity> lstCulumns = new List<DataGridViewColumnEntity>();
                lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Name", HeadText = "工单号", Width = 600, WidthType = SizeType.Absolute });
                if (typeid == 1)
                {
                    this.Factory_processes_ucComboxGrid.GridColumns = lstCulumns;
                }
                if (typeid == 2)
                {
                    this.Factory_reworks_ucComboxGrid.GridColumns = lstCulumns;
                }
                if (typeid == 3)
                {
                    this.Factory_sns_ucComboxGrid.GridColumns = lstCulumns;
                }
                //this.ucComboxGrid1.UCDataGridView.IsShowCheckBox = true;
                var data = new List<Receipt>();
                List<object> lstSource = new List<object>();
                var List = SqlStoredProcedure.GetGDnumberList(date);
                data = List;
                //默认添加一个全部
                Receipt receipt = new Receipt();
                receipt.Name = "All";
                lstSource.Add(receipt);
                foreach (var item in data)
                {
                    if (data.Count < 0)
                    {
                        break;
                    }
                    else
                    {
                        Receipt model = new Receipt()
                        {
                            Name = item.ERPMO
                        };
                        lstSource.Add(model);
                    }

                }
                if (typeid == 1)
                {
                    this.Factory_processes_ucComboxGrid.GridDataSource = lstSource;
                    this.Factory_processes_ucComboxGrid.ResetText();
                }
                if (typeid == 2)
                {
                    this.Factory_reworks_ucComboxGrid.GridDataSource = lstSource;
                    this.Factory_reworks_ucComboxGrid.ResetText();
                }
                if (typeid == 3)
                {
                    this.Factory_sns_ucComboxGrid.GridDataSource = lstSource;
                    this.Factory_sns_ucComboxGrid.ResetText();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        /// <summary>
        /// Factory_processes页签下拉列表鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Factory_processes_ucComboxGrid_MouseClick(object sender, System.EventArgs e)
        {
            this.Factory_processes_ucComboxGrid.GridDataSource = null;
            ComboxGridDataSource(1, this.Factory_processes_dateExt.DatePicker.DateValue.ToString());
        }

        /// <summary>
        /// Factory_reworks页签下拉列表鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Factory_reworks_ucComboxGrid_MouseClick(object sender, MouseEventArgs e)
        {
            this.Factory_reworks_ucComboxGrid.GridDataSource = null;
            ComboxGridDataSource(2, this.Factory_reworks_dateExt.DatePicker.DateValue.ToString());
        }

        /// <summary>
        /// Factory_sns页签下拉列表鼠标点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Factory_sns_ucComboxGrid_MouseClick(object sender, MouseEventArgs e)
        {
            this.Factory_sns_ucComboxGrid.GridDataSource = null;
            ComboxGridDataSource(3, this.Factory_sns_dateExt.DatePicker.DateValue.ToString());
        }

        private void ucComboxGrid_MouseCaptureChanged(object sender, EventArgs e)
        {
            //ComboxGridDataSource(this.Factory_processes_dateExt.DatePicker.DateValue.ToString());
            //获取时间控件的值调用sql形成表格数据
        }
        /// <summary>
        /// Factory_processes页签按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Factory_processes_skinButton_Click(object sender, EventArgs e)
        {
            //HZH_Controls.Forms.FrmTips.ShowTipsSuccess(this.FindForm(), "你点击了");
            if (this.Factory_processes_ucComboxGrid.TextValue != "点我看工单号列表")
            {
                //调用Factory_processes接口
                MaskingExt.Show(this, new MaskingExt.MaskingSettings() { TextOrientation = MaskingExt.MaskingTextOrientations.Right });
                UseInterface(1, "http://api.eos-ts.h3c.com/odm-api/v1.0/factory/process");
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择工单号或者您选择的日期当天没有工单号请重新选择！");
            }
        }

        /// <summary>
        /// Factory_reworks页签按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Factory_reworks_skinButton_Click(object sender, EventArgs e)
        {
            if (this.Factory_reworks_ucComboxGrid.TextValue != "点我看工单号列表")
            {
                //调用Factory_processes接口
                MaskingExt.Show(this, new MaskingExt.MaskingSettings() { TextOrientation = MaskingExt.MaskingTextOrientations.Right });
                UseInterface(2, "http://api.eos-ts.h3c.com/odm-api/v1.0/factory/rework");
            }
            else
            {
                DevExpress.XtraEditors.XtraMessageBox.Show("请选择工单号或者您选择的日期当天没有工单号请重新选择！");
            }
        }
        /// <summary>
        /// Factory_sns页签按钮点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Factory_sns_skinButton_Click(object sender, EventArgs e)
        {
            DevExpress.XtraEditors.XtraMessageBox.Show("暂未开发");
        }

        /// <summary>
        /// 接口页签-调用接口
        /// </summary>
        /// <param name="typeid">1-factory_processes，2-factory_reworks</param>
        /// <param name="url"></param>
        private void UseInterface(int typeid, string url)
        {
            var ERPMO = this.Factory_processes_ucComboxGrid.TextValue; //下拉框值
            var date = this.Factory_processes_dateExt.DatePicker.DateValue.ToString(); //时间控件
            DateTime dt = DateTime.Parse(date);
            //如果是全部就添加当天整个工单号(循环调用接口)
            string TextValue = "";
            if (typeid == 1)
            {
                TextValue = this.Factory_processes_ucComboxGrid.TextValue;
            }
            if (typeid == 2)
            {
                TextValue = this.Factory_reworks_ucComboxGrid.TextValue;
            }
            if (TextValue == "All")
            {
                //查当天所有工单号
                List<Receipt> ERPMOList = SqlStoredProcedure.GetGDnumberList(this.Factory_processes_dateExt.DatePicker.DateValue.ToString());
                string Message = "";
                foreach (var item in ERPMOList)
                {
                    //通过日期和工单号查出列表集合
                    ERPMO = item.ERPMO.ToString();
                    var reult = UseInterfaceMethod(true, typeid, url, ERPMO, dt);
                    if (reult == 2) //返回值 0-工单号没数据 1-失败，2-成功
                    {
                        Message = "执行成功！";
                    }
                    else
                    {
                        if (reult == 1)
                        {
                            Message = "工单号数据出现异常，请查看错误数据表格！";
                        }
                    }

                }
                DevExpress.XtraEditors.XtraMessageBox.Show(Message);
            }
            else
            {
                var reult = UseInterfaceMethod(false, 1, url, ERPMO, dt);
                //只有选择单条数据的时候判断工单号是否有数据
                if (reult == 2) //返回值 0-工单号没数据 1-失败，2-成功
                {
                    DevExpress.XtraEditors.XtraMessageBox.Show("执行成功！");
                }
                else
                {
                    if (reult == 0)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("执行失败，您选择的工单号暂时无数据，请重新选择！");
                    }
                    if (reult == 1)
                    {
                        DevExpress.XtraEditors.XtraMessageBox.Show("工单号数据出现异常，请查看错误数据表格！");
                    }
                }

            }
            MaskingExt.Clear();
            #region 数据测试
            //流水号
            //var Transaction_id = "ODM-" + "POW012" + "-" + time + number + "";
            //Factory_reworks数据集合
            //var Factory_reworks_List = "[{\"factory_code\":\"POW012\"," +
            //           "\"item_code\":\"9801A1G2\"," +
            //           "\"item_type\":\"PSR380-24A\"," +
            //           "\"sn\":\"GHL75258214800315\"," +
            //           "\"fault_date\":\"2021-11-28 01:45:09.173\"," +
            //           "\"fault_appearance\":\"功率效率\"," +
            //           "\"appearance_link\":\"FATE\"," +
            //           "\"fault_type\":\"电性能\"," +
            //           "\"fault_case\":\"R167连锡\"," +
            //           "\"cause_type\":\"PROCESS(生产线作业)\"," +
            //           "\"cause_subcategory\":\"插件/执锡不良\"," +
            //           "\"root_cause\":\"PROCESS(生产线作业)\"," +
            //            "\"defect_location\":\"R167\"," +
            //           "\"repair_date\":\"2021-11-30 21:52:57.197\"}]";
            //var JKfactory_reworks = HttpHelper.HttpPost(2, url + "/odm-api/v1.0/factory/rework", "{\"transaction_id\":\"" + Transaction_id + "\",\"factory_reworks\":" + Factory_reworks_List + "}");
            #endregion
        }

        /// <summary>
        /// 接口页签-获得接口参数调用接口
        /// </summary>
        /// <param name="typeid">1-factory_processes，2-factory_reworks</param>
        /// <param name="url"></param>
        /// <param name="ERPMO"></param>
        private int UseInterfaceMethod(bool IsSelectAll, int typeid, string url, string ERPMO, DateTime dt) //返回值 0-工单号没数据 1-失败，2-成功
        {
            try
            {
                var DataSetList = ""; //数据集合
                string time = dt.ToString("yyyyMMddHHmmssfff"); //时间控件的值格式化为时间戳格式（17位）
                int flag = 2;  //默认成功
                //随机五位数
                Random rnd = new Random();
                List<DataSet> list = SqlStoredProcedure.ByDateAndERPMOGetListSql(typeid, dt.ToString("yyyy-MM-dd"), ERPMO); //日期年月日 
                if (IsSelectAll == false && list.Count == 0)  //只有选择单条数据才判断
                {
                    flag = 0;
                    return flag;
                }
                foreach (var dataSet in list)
                {
                    int number = rnd.Next(10000, 100000);
                    //流水号
                    var Transaction_id = "ODM-" + dataSet.厂商代码 + "-" + time + number + "";
                    //Factory_processes接口参数
                    if (typeid == 1)
                    {
                        DataSetList += "{\"transaction_id\":\"" + Transaction_id +
                               "\",\"factory_processes\":[{\"factory_code\":\"" + dataSet.厂商代码 + "\"," +
                                "\"item_code\":\"" + dataSet.编码 + "\"," +
                                "\"sn\":\"" + dataSet.SN条码 + "\"," +
                                "\"order_number\":\"" + dataSet.Po订单号 + "\"," +
                                "\"mo_number\":\"" + dataSet.任务令 + "\"," +
                                "\"factory\":\"" + dataSet.工厂 + "\"," +
                                "\"line_code\":\"" + dataSet.线别 + "\"," +
                                "\"class_code\":\"" + dataSet.班次 + "\"," +
                                "\"is_maintenance\":\"" + dataSet.是否维修品 + "\"," +
                                "\"section_code\":\"" + dataSet.工序名称 + "\"," +
                                "\"process_result\":\"" + dataSet.加工结果 + "\"," +
                                "\"start_time\":\"" + dataSet.开始时间 + "\"}]";
                    }
                    //Factory_reworks接口参数
                    if (typeid == 2)
                    {
                        DataSetList += "{\"transaction_id\":\"" + Transaction_id +
                                       "\",\"factory_reworks\":[{\"factory_code\":\"" + dataSet.厂商代码 + "\"," +
                                        "\"item_code\":\"" + dataSet.编码 + "\"," +
                                        "\"item_type\":\"" + dataSet.型号 + "\"," +
                                        "\"sn\":\"" + dataSet.SN条码 + "\"," +
                                        "\"fault_date\":\"" + dataSet.故障日期 + "\"," +
                                        "\"fault_appearance\":\"" + dataSet.不良现象 + "\"," +
                                        "\"appearance_link\":\"" + dataSet.出现环节 + "\"," +
                                        "\"fault_type\":\"" + dataSet.不良分类 + "\"," +
                                        "\"fault_case\":\"" + dataSet.不良原因分析 + "\"," +
                                        "\"cause_type\":\"" + dataSet.原因分类 + "\"," +
                                        "\"cause_subcategory\":\"" + dataSet.原因小类 + "\"," +
                                        "\"root_cause\":\"" + dataSet.根因 + "\"," +
                                        "\"defect_location\":\"" + dataSet.不良器件位置 + "\"," +
                                        "\"repair_date\":\"" + dataSet.修理日期 + "\"}]";
                    }
                    DataSetList += "}";
                    DataSetList += ",";
                    if (i >= 200 || i >= list.Count)
                    {
                        DataSetList = DataSetList.TrimEnd(',');
                        //接口返回结果
                        var JKResult = HttpHelper.HttpPost(2, url, DataSetList);
                        Receipt receipt = HttpHelper.DeserializeJsonToObject<Receipt>(JKResult);
                        ////接口返回新增成功
                        if (receipt.code != 1100)
                        {
                            //数据新增失败，写入数据表里:记录时间和工单号
                            SqlStoredProcedure.AddLog_ErrorData(dt, ERPMO, receipt.msg);
                            flag = 1;

                        }
                        else
                        {
                            //接口调用成功
                            //DevExpress.XtraEditors.XtraMessageBox.Show(receipt.msg);
                        }
                        i = 1;
                    }
                    else
                    {
                        i++;
                    }
                }
                return flag;
            }
            catch (Exception ex)
            {
                //数据新增失败，写入数据表里:记录时间和工单号
                SqlStoredProcedure.AddLog_ErrorData(dt, ERPMO, ex.Message);
                return 1;
                throw;
            }
        }
        #endregion
        #region 表格页签
        /// <summary>
        /// 给委托事件添加具体事件：给表格赋值
        /// </summary>
        /// <param name="currentSource"></param>
        private void m_page_ShowSourceChanged(object currentSource)
        {
            this.ucDataGridView.DataSource = currentSource;
        }
        /// <summary>
        /// 表格页签-初始化表格数据源
        /// </summary>
        private void initDataSource()
        {
            this.listSource.Clear();
            //错误数据表倒序显示
            var data = gipwip_R1.Log_ErrorData.OrderByDescending(a => a.Error_Date).ToList();
            int i = 1;
            foreach (var item in data)
            {
                Log_ErrorData model = new Log_ErrorData()
                {
                    Error_Number = i,
                    Error_Data = item.Error_Data,
                    Error_Reason = item.Error_Reason,
                    Error_Date = item.Error_Date
                };
                this.listSource.Add(model);
                i++;
            }
        }
        /// <summary>
        /// 表格页签—列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucDataGridView1_Load(object sender, EventArgs e)
        {
            try
            {
                List<DataGridViewColumnEntity> lstCulumns = new List<DataGridViewColumnEntity>();
                //表头
                lstCulumns.Add(new DataGridViewColumnEntity() { Width = 35, WidthType = SizeType.Absolute, CustomCellType = typeof(UCTestGridTable_CustomCellIcon) });
                lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Error_Number", HeadText = "编号", Width = 50, WidthType = SizeType.Absolute });
                lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Error_Data", HeadText = "数据", Width = 450, WidthType = SizeType.Absolute });
                lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Error_Reason", HeadText = "错误原因", Width = 200, WidthType = SizeType.Absolute });
                //lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "TableName", HeadText = "生日", Width = 500, WidthType = SizeType.Absolute, Format = (a) => { return ((DateTime)a).ToString("yyyy-MM-dd"); } });
                //lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Description", HeadText = "性别", Width = 50, WidthType = SizeType.Absolute, Format = (a) => { return ((int)a) == 0 ? "女" : "男"; } });
                lstCulumns.Add(new DataGridViewColumnEntity() { DataField = "Error_Date", HeadText = "产生时间", Width = 100, WidthType = SizeType.Absolute });
                lstCulumns.Add(new DataGridViewColumnEntity() { Width = 155, WidthType = SizeType.Absolute, CustomCellType = typeof(UCTestGridTable_CustomCell) });
                lstCulumns.Add(new DataGridViewColumnEntity() { Width = 50, WidthType = SizeType.Absolute, CustomCellType = typeof(UCTestGridTable_CustomCellIcon) });
                this.ucDataGridView.Columns = lstCulumns;
                //给分页控件传数据源,通过委托事件m_page_ShowSourceChanged初始化表格
                this.ucPagerControl.DataSource = this.listSource;
                //this.ucDataGridView1.IsShowCheckBox = true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void m_page_ShowSourceChangeds(object currentSource)
        {
            this.ucDataGridView.DataSource = currentSource;
        }
        /// <summary>
        /// 数据行单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ucDataGridView1_ItemClick(object sender, DataGridViewEventArgs e)
        {
            //显示一个提示框
            //HZH_Controls.Forms.FrmAnchorTips.ShowTips(button1, "测试提示信息\nBOTTOM", AnchorTipsLocation.BOTTOM);
            //DataGridView dgv = sender as DataGridView;
            //string msg = string.Format("单元格所在的行{0}，单元格所在的列{1}，单元格所在的值{2}", dgv.CurrentCell.RowIndex, dgv.CurrentCell.ColumnIndex, dgv.CurrentCell.Value);
            //MessageBox.Show(msg);
            //string msg1 = string.Format("单元格所在的行{0}，单元格所在的列{1}，单元格所在的值{2}", e.RowIndex, e.ColumnIndex, dataGridView1[e.ColumnIndex, e.RowIndex].Value);
            //MessageBox.Show(msg1);
            //委托事件添加具体事件
            var aa = ucDataGridView.SelectRows.ToString();
            DevExpress.XtraEditors.XtraMessageBox.Show(aa);
        }
        #endregion

    }
}
