
namespace Login
{
    partial class Details
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Details));
            this.webStationDataSet = new Login.WebStationDataSet();
            this.selectCustomerAllBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.selectCustomerAllTableAdapter = new Login.WebStationDataSetTableAdapters.SelectCustomerAllTableAdapter();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.ucPagerControl2 = new HZH_Controls.Controls.UCPagerControl2();
            this.ucDataGridView1 = new HZH_Controls.Controls.UCDataGridView();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label7 = new System.Windows.Forms.Label();
            this.skinButton3 = new CCWin.SkinControl.SkinButton();
            this.skinPanel3 = new CCWin.SkinControl.SkinPanel();
            this.skinAlphaWaterTextBox2 = new CCWin.SkinControl.SkinAlphaWaterTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dateExt3 = new WinformControlLibraryExtension.DateExt();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label4 = new System.Windows.Forms.Label();
            this.skinButton2 = new CCWin.SkinControl.SkinButton();
            this.skinPanel2 = new CCWin.SkinControl.SkinPanel();
            this.skinAlphaWaterTextBox1 = new CCWin.SkinControl.SkinAlphaWaterTextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dateExt2 = new WinformControlLibraryExtension.DateExt();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.skinPanel1 = new CCWin.SkinControl.SkinPanel();
            this.Password = new CCWin.SkinControl.SkinAlphaWaterTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateExt1 = new WinformControlLibraryExtension.DateExt();
            this.DataList = new System.Windows.Forms.TabControl();
            this.btnFirst = new HZH_Controls.Controls.UCBtnExt();
            this.btnPrevious = new HZH_Controls.Controls.UCBtnExt();
            this.ucBtnExt1 = new HZH_Controls.Controls.UCBtnExt();
            this.btnEnd = new HZH_Controls.Controls.UCBtnExt();
            this.txtPage = new HZH_Controls.Controls.UCTextBoxEx();
            this.btnToPage = new HZH_Controls.Controls.UCBtnExt();
            ((System.ComponentModel.ISupportInitialize)(this.webStationDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectCustomerAllBindingSource)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.DataList.SuspendLayout();
            this.SuspendLayout();
            // 
            // webStationDataSet
            // 
            this.webStationDataSet.DataSetName = "WebStationDataSet";
            this.webStationDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // selectCustomerAllBindingSource
            // 
            this.selectCustomerAllBindingSource.DataMember = "SelectCustomerAll";
            this.selectCustomerAllBindingSource.DataSource = this.webStationDataSet;
            // 
            // selectCustomerAllTableAdapter
            // 
            this.selectCustomerAllTableAdapter.ClearBeforeFill = true;
            // 
            // tabPage4
            // 
            this.tabPage4.BackgroundImage = global::Login.Properties.Resources.bg;
            this.tabPage4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPage4.Controls.Add(this.btnToPage);
            this.tabPage4.Controls.Add(this.txtPage);
            this.tabPage4.Controls.Add(this.btnEnd);
            this.tabPage4.Controls.Add(this.ucBtnExt1);
            this.tabPage4.Controls.Add(this.btnPrevious);
            this.tabPage4.Controls.Add(this.btnFirst);
            this.tabPage4.Controls.Add(this.ucDataGridView1);
            this.tabPage4.Controls.Add(this.ucPagerControl2);
            this.tabPage4.Location = new System.Drawing.Point(4, 44);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(873, 510);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // ucPagerControl2
            // 
            this.ucPagerControl2.BackColor = System.Drawing.Color.White;
            this.ucPagerControl2.DataSource = ((System.Collections.Generic.List<object>)(resources.GetObject("ucPagerControl2.DataSource")));
            this.ucPagerControl2.Location = new System.Drawing.Point(37, 462);
            this.ucPagerControl2.Name = "ucPagerControl2";
            this.ucPagerControl2.PageCount = 0;
            this.ucPagerControl2.PageIndex = 1;
            this.ucPagerControl2.PageModel = HZH_Controls.Controls.PageModel.Soure;
            this.ucPagerControl2.PageSize = 15;
            this.ucPagerControl2.Size = new System.Drawing.Size(798, 41);
            this.ucPagerControl2.StartIndex = 0;
            this.ucPagerControl2.TabIndex = 4;
            this.ucPagerControl2.Load += new System.EventHandler(this.ucPagerControl2_Load);
            // 
            // ucDataGridView1
            // 
            this.ucDataGridView1.BackColor = System.Drawing.Color.White;
            this.ucDataGridView1.Columns = null;
            this.ucDataGridView1.DataSource = null;
            this.ucDataGridView1.HeadFont = new System.Drawing.Font("微软雅黑", 12F);
            this.ucDataGridView1.HeadHeight = 40;
            this.ucDataGridView1.HeadPadingLeft = 0;
            this.ucDataGridView1.HeadTextColor = System.Drawing.Color.Black;
            this.ucDataGridView1.IsShowCheckBox = false;
            this.ucDataGridView1.IsShowHead = true;
            this.ucDataGridView1.Location = new System.Drawing.Point(37, 0);
            this.ucDataGridView1.Name = "ucDataGridView1";
            this.ucDataGridView1.Padding = new System.Windows.Forms.Padding(0, 40, 0, 0);
            this.ucDataGridView1.RowHeight = 41;
            this.ucDataGridView1.RowType = typeof(HZH_Controls.Controls.UCDataGridViewRow);
            this.ucDataGridView1.Size = new System.Drawing.Size(798, 456);
            this.ucDataGridView1.TabIndex = 0;
            this.ucDataGridView1.Load += new System.EventHandler(this.ucDataGridView1_Load);
            // 
            // tabPage3
            // 
            this.tabPage3.BackgroundImage = global::Login.Properties.Resources.LoginView;
            this.tabPage3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabPage3.Controls.Add(this.label7);
            this.tabPage3.Controls.Add(this.skinButton3);
            this.tabPage3.Controls.Add(this.skinPanel3);
            this.tabPage3.Controls.Add(this.skinAlphaWaterTextBox2);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.dateExt3);
            this.tabPage3.Location = new System.Drawing.Point(4, 44);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(873, 510);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(376, 139);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(146, 27);
            this.label7.TabIndex = 41;
            this.label7.Text = "Factory_sns";
            // 
            // skinButton3
            // 
            this.skinButton3.BackColor = System.Drawing.Color.Transparent;
            this.skinButton3.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(159)))), ((int)(((byte)(176)))));
            this.skinButton3.BorderColor = System.Drawing.Color.Transparent;
            this.skinButton3.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton3.DownBack = null;
            this.skinButton3.DownBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(60)))), ((int)(((byte)(84)))));
            this.skinButton3.FadeGlow = false;
            this.skinButton3.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.skinButton3.ForeColor = System.Drawing.Color.White;
            this.skinButton3.GlowColor = System.Drawing.Color.Black;
            this.skinButton3.Location = new System.Drawing.Point(369, 329);
            this.skinButton3.Margin = new System.Windows.Forms.Padding(10);
            this.skinButton3.MouseBack = null;
            this.skinButton3.MouseBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(139)))), ((int)(((byte)(146)))));
            this.skinButton3.Name = "skinButton3";
            this.skinButton3.NormlBack = null;
            this.skinButton3.Radius = 20;
            this.skinButton3.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinButton3.Size = new System.Drawing.Size(122, 41);
            this.skinButton3.TabIndex = 40;
            this.skinButton3.Text = "执 行";
            this.skinButton3.UseVisualStyleBackColor = false;
            // 
            // skinPanel3
            // 
            this.skinPanel3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinPanel3.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel3.DownBack = null;
            this.skinPanel3.Location = new System.Drawing.Point(371, 287);
            this.skinPanel3.MouseBack = null;
            this.skinPanel3.Name = "skinPanel3";
            this.skinPanel3.NormlBack = null;
            this.skinPanel3.Size = new System.Drawing.Size(215, 1);
            this.skinPanel3.TabIndex = 39;
            // 
            // skinAlphaWaterTextBox2
            // 
            this.skinAlphaWaterTextBox2.BackAlpha = 10;
            this.skinAlphaWaterTextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skinAlphaWaterTextBox2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinAlphaWaterTextBox2.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skinAlphaWaterTextBox2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(60)))), ((int)(((byte)(84)))));
            this.skinAlphaWaterTextBox2.Location = new System.Drawing.Point(371, 267);
            this.skinAlphaWaterTextBox2.Name = "skinAlphaWaterTextBox2";
            this.skinAlphaWaterTextBox2.Size = new System.Drawing.Size(215, 17);
            this.skinAlphaWaterTextBox2.TabIndex = 38;
            this.skinAlphaWaterTextBox2.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinAlphaWaterTextBox2.WaterFont = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinAlphaWaterTextBox2.WaterText = "请输入工单号";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(285, 270);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(68, 17);
            this.label8.TabIndex = 37;
            this.label8.Text = "工单号：";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(283, 221);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 17);
            this.label9.TabIndex = 36;
            this.label9.Text = "入库日期：";
            // 
            // dateExt3
            // 
            this.dateExt3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            // 
            // 
            // 
            this.dateExt3.DatePicker.ActivateColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dateExt3.DatePicker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt3.DatePicker.BottomBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt3.DatePicker.BottomBarBtnForeNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt3.DatePicker.BottomBarClockDotColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dateExt3.DatePicker.DateBackDisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dateExt3.DatePicker.DateBackNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt3.DatePicker.DateForeDisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.dateExt3.DatePicker.DateForeFutureColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(206)))), ((int)(((byte)(235)))));
            this.dateExt3.DatePicker.DateForePastColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(206)))), ((int)(((byte)(235)))));
            this.dateExt3.DatePicker.DateForeSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt3.DatePicker.DateTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.dateExt3.DatePicker.Location = new System.Drawing.Point(0, 0);
            this.dateExt3.DatePicker.Name = "";
            this.dateExt3.DatePicker.TabIndex = 0;
            this.dateExt3.DatePicker.TimeForeSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt3.DatePicker.TopBarBtnForeNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt3.DateTextAlign = WinformControlLibraryExtension.DateExt.DataTextAligns.Left;
            this.dateExt3.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dateExt3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.dateExt3.Location = new System.Drawing.Point(369, 217);
            this.dateExt3.Name = "dateExt3";
            this.dateExt3.Size = new System.Drawing.Size(221, 24);
            this.dateExt3.TabIndex = 35;
            // 
            // tabPage2
            // 
            this.tabPage2.BackgroundImage = global::Login.Properties.Resources.LoginView;
            this.tabPage2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.skinButton2);
            this.tabPage2.Controls.Add(this.skinPanel2);
            this.tabPage2.Controls.Add(this.skinAlphaWaterTextBox1);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.dateExt2);
            this.tabPage2.Location = new System.Drawing.Point(4, 44);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(873, 510);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(324, 141);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(201, 27);
            this.label4.TabIndex = 34;
            this.label4.Text = "Factory_reworks";
            // 
            // skinButton2
            // 
            this.skinButton2.BackColor = System.Drawing.Color.Transparent;
            this.skinButton2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(159)))), ((int)(((byte)(176)))));
            this.skinButton2.BorderColor = System.Drawing.Color.Transparent;
            this.skinButton2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton2.DownBack = null;
            this.skinButton2.DownBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(60)))), ((int)(((byte)(84)))));
            this.skinButton2.FadeGlow = false;
            this.skinButton2.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.skinButton2.ForeColor = System.Drawing.Color.White;
            this.skinButton2.GlowColor = System.Drawing.Color.Black;
            this.skinButton2.Location = new System.Drawing.Point(369, 329);
            this.skinButton2.Margin = new System.Windows.Forms.Padding(10);
            this.skinButton2.MouseBack = null;
            this.skinButton2.MouseBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(139)))), ((int)(((byte)(146)))));
            this.skinButton2.Name = "skinButton2";
            this.skinButton2.NormlBack = null;
            this.skinButton2.Radius = 20;
            this.skinButton2.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinButton2.Size = new System.Drawing.Size(122, 41);
            this.skinButton2.TabIndex = 33;
            this.skinButton2.Text = "执 行";
            this.skinButton2.UseVisualStyleBackColor = false;
            // 
            // skinPanel2
            // 
            this.skinPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinPanel2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel2.DownBack = null;
            this.skinPanel2.Location = new System.Drawing.Point(371, 287);
            this.skinPanel2.MouseBack = null;
            this.skinPanel2.Name = "skinPanel2";
            this.skinPanel2.NormlBack = null;
            this.skinPanel2.Size = new System.Drawing.Size(215, 1);
            this.skinPanel2.TabIndex = 32;
            // 
            // skinAlphaWaterTextBox1
            // 
            this.skinAlphaWaterTextBox1.BackAlpha = 10;
            this.skinAlphaWaterTextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skinAlphaWaterTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.skinAlphaWaterTextBox1.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.skinAlphaWaterTextBox1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(60)))), ((int)(((byte)(84)))));
            this.skinAlphaWaterTextBox1.Location = new System.Drawing.Point(371, 267);
            this.skinAlphaWaterTextBox1.Name = "skinAlphaWaterTextBox1";
            this.skinAlphaWaterTextBox1.Size = new System.Drawing.Size(215, 17);
            this.skinAlphaWaterTextBox1.TabIndex = 31;
            this.skinAlphaWaterTextBox1.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinAlphaWaterTextBox1.WaterFont = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinAlphaWaterTextBox1.WaterText = "请输入工单号";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.label5.Location = new System.Drawing.Point(285, 270);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 17);
            this.label5.TabIndex = 30;
            this.label5.Text = "工单号：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(283, 221);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 17);
            this.label6.TabIndex = 29;
            this.label6.Text = "入库日期：";
            // 
            // dateExt2
            // 
            this.dateExt2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            // 
            // 
            // 
            this.dateExt2.DatePicker.ActivateColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dateExt2.DatePicker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt2.DatePicker.BottomBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt2.DatePicker.BottomBarBtnForeNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt2.DatePicker.BottomBarClockDotColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dateExt2.DatePicker.DateBackDisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dateExt2.DatePicker.DateBackNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt2.DatePicker.DateForeDisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.dateExt2.DatePicker.DateForeFutureColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(206)))), ((int)(((byte)(235)))));
            this.dateExt2.DatePicker.DateForePastColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(206)))), ((int)(((byte)(235)))));
            this.dateExt2.DatePicker.DateForeSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt2.DatePicker.DateTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.dateExt2.DatePicker.Location = new System.Drawing.Point(0, 0);
            this.dateExt2.DatePicker.Name = "";
            this.dateExt2.DatePicker.TabIndex = 0;
            this.dateExt2.DatePicker.TimeForeSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt2.DatePicker.TopBarBtnForeNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt2.DateTextAlign = WinformControlLibraryExtension.DateExt.DataTextAligns.Left;
            this.dateExt2.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dateExt2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.dateExt2.Location = new System.Drawing.Point(369, 217);
            this.dateExt2.Name = "dateExt2";
            this.dateExt2.Size = new System.Drawing.Size(221, 24);
            this.dateExt2.TabIndex = 28;
            // 
            // tabPage1
            // 
            this.tabPage1.BackgroundImage = global::Login.Properties.Resources.LoginView;
            this.tabPage1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.skinButton1);
            this.tabPage1.Controls.Add(this.skinPanel1);
            this.tabPage1.Controls.Add(this.Password);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.dateExt1);
            this.tabPage1.Location = new System.Drawing.Point(4, 44);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(873, 510);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(339, 125);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(219, 27);
            this.label3.TabIndex = 27;
            this.label3.Text = "Factory_processes";
            // 
            // skinButton1
            // 
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(69)))), ((int)(((byte)(159)))), ((int)(((byte)(176)))));
            this.skinButton1.BorderColor = System.Drawing.Color.Transparent;
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.DownBack = null;
            this.skinButton1.DownBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(60)))), ((int)(((byte)(84)))));
            this.skinButton1.FadeGlow = false;
            this.skinButton1.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Bold);
            this.skinButton1.ForeColor = System.Drawing.Color.White;
            this.skinButton1.GlowColor = System.Drawing.Color.Black;
            this.skinButton1.Location = new System.Drawing.Point(384, 313);
            this.skinButton1.Margin = new System.Windows.Forms.Padding(10);
            this.skinButton1.MouseBack = null;
            this.skinButton1.MouseBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(49)))), ((int)(((byte)(139)))), ((int)(((byte)(146)))));
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = null;
            this.skinButton1.Radius = 20;
            this.skinButton1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinButton1.Size = new System.Drawing.Size(122, 41);
            this.skinButton1.TabIndex = 26;
            this.skinButton1.Text = "执 行";
            this.skinButton1.UseVisualStyleBackColor = false;
            // 
            // skinPanel1
            // 
            this.skinPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.skinPanel1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinPanel1.DownBack = null;
            this.skinPanel1.Location = new System.Drawing.Point(386, 271);
            this.skinPanel1.MouseBack = null;
            this.skinPanel1.Name = "skinPanel1";
            this.skinPanel1.NormlBack = null;
            this.skinPanel1.Size = new System.Drawing.Size(215, 1);
            this.skinPanel1.TabIndex = 23;
            // 
            // Password
            // 
            this.Password.BackAlpha = 10;
            this.Password.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Password.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.Password.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Password.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(38)))), ((int)(((byte)(60)))), ((int)(((byte)(84)))));
            this.Password.Location = new System.Drawing.Point(386, 251);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(215, 17);
            this.Password.TabIndex = 22;
            this.Password.WaterColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(127)))), ((int)(((byte)(127)))));
            this.Password.WaterFont = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Password.WaterText = "请输入工单号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(300, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "工单号：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Tahoma", 10.5F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(298, 205);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "入库日期：";
            // 
            // dateExt1
            // 
            this.dateExt1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            // 
            // 
            // 
            this.dateExt1.DatePicker.ActivateColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.dateExt1.DatePicker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt1.DatePicker.BottomBarBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt1.DatePicker.BottomBarBtnForeNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt1.DatePicker.BottomBarClockDotColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.dateExt1.DatePicker.DateBackDisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.dateExt1.DatePicker.DateBackNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt1.DatePicker.DateForeDisabledColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.dateExt1.DatePicker.DateForeFutureColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(206)))), ((int)(((byte)(235)))));
            this.dateExt1.DatePicker.DateForePastColor = System.Drawing.Color.FromArgb(((int)(((byte)(135)))), ((int)(((byte)(206)))), ((int)(((byte)(235)))));
            this.dateExt1.DatePicker.DateForeSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt1.DatePicker.DateTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.dateExt1.DatePicker.Location = new System.Drawing.Point(0, 0);
            this.dateExt1.DatePicker.Name = "";
            this.dateExt1.DatePicker.TabIndex = 0;
            this.dateExt1.DatePicker.TimeForeSelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt1.DatePicker.TopBarBtnForeNormalColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dateExt1.DateTextAlign = WinformControlLibraryExtension.DateExt.DataTextAligns.Left;
            this.dateExt1.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.dateExt1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(105)))), ((int)(((byte)(105)))), ((int)(((byte)(105)))));
            this.dateExt1.Location = new System.Drawing.Point(384, 201);
            this.dateExt1.Name = "dateExt1";
            this.dateExt1.Size = new System.Drawing.Size(221, 24);
            this.dateExt1.TabIndex = 0;
            // 
            // DataList
            // 
            this.DataList.Controls.Add(this.tabPage1);
            this.DataList.Controls.Add(this.tabPage2);
            this.DataList.Controls.Add(this.tabPage3);
            this.DataList.Controls.Add(this.tabPage4);
            this.DataList.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.DataList.ItemSize = new System.Drawing.Size(150, 40);
            this.DataList.Location = new System.Drawing.Point(-4, 0);
            this.DataList.Name = "DataList";
            this.DataList.SelectedIndex = 0;
            this.DataList.Size = new System.Drawing.Size(881, 558);
            this.DataList.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.DataList.TabIndex = 0;
            // 
            // btnFirst
            // 
            this.btnFirst.BackColor = System.Drawing.Color.White;
            this.btnFirst.BtnBackColor = System.Drawing.Color.White;
            this.btnFirst.BtnFont = new System.Drawing.Font("微软雅黑", 9F);
            this.btnFirst.BtnForeColor = System.Drawing.Color.Gray;
            this.btnFirst.BtnText = "<<";
            this.btnFirst.ConerRadius = 5;
            this.btnFirst.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFirst.EnabledMouseEffect = false;
            this.btnFirst.FillColor = System.Drawing.Color.White;
            this.btnFirst.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnFirst.IsRadius = true;
            this.btnFirst.IsShowRect = true;
            this.btnFirst.IsShowTips = false;
            this.btnFirst.Location = new System.Drawing.Point(110, 467);
            this.btnFirst.Margin = new System.Windows.Forms.Padding(5);
            this.btnFirst.Name = "btnFirst";
            this.btnFirst.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnFirst.RectWidth = 1;
            this.btnFirst.Size = new System.Drawing.Size(30, 30);
            this.btnFirst.TabIndex = 6;
            this.btnFirst.TabStop = false;
            this.btnFirst.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnFirst.TipsText = "";
            this.btnFirst.BtnClick += new System.EventHandler(this.btnFirst_BtnClick_1);
            // 
            // btnPrevious
            // 
            this.btnPrevious.BackColor = System.Drawing.Color.White;
            this.btnPrevious.BtnBackColor = System.Drawing.Color.White;
            this.btnPrevious.BtnFont = new System.Drawing.Font("微软雅黑", 9F);
            this.btnPrevious.BtnForeColor = System.Drawing.Color.Gray;
            this.btnPrevious.BtnText = "<";
            this.btnPrevious.ConerRadius = 5;
            this.btnPrevious.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPrevious.EnabledMouseEffect = false;
            this.btnPrevious.FillColor = System.Drawing.Color.White;
            this.btnPrevious.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnPrevious.IsRadius = true;
            this.btnPrevious.IsShowRect = true;
            this.btnPrevious.IsShowTips = false;
            this.btnPrevious.Location = new System.Drawing.Point(150, 467);
            this.btnPrevious.Margin = new System.Windows.Forms.Padding(5);
            this.btnPrevious.Name = "btnPrevious";
            this.btnPrevious.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnPrevious.RectWidth = 1;
            this.btnPrevious.Size = new System.Drawing.Size(30, 30);
            this.btnPrevious.TabIndex = 7;
            this.btnPrevious.TabStop = false;
            this.btnPrevious.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnPrevious.TipsText = "";
            this.btnPrevious.BtnClick += new System.EventHandler(this.btnPrevious_BtnClick);
            // 
            // ucBtnExt1
            // 
            this.ucBtnExt1.BackColor = System.Drawing.Color.White;
            this.ucBtnExt1.BtnBackColor = System.Drawing.Color.White;
            this.ucBtnExt1.BtnFont = new System.Drawing.Font("微软雅黑", 9F);
            this.ucBtnExt1.BtnForeColor = System.Drawing.Color.Gray;
            this.ucBtnExt1.BtnText = ">";
            this.ucBtnExt1.ConerRadius = 5;
            this.ucBtnExt1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ucBtnExt1.EnabledMouseEffect = false;
            this.ucBtnExt1.FillColor = System.Drawing.Color.White;
            this.ucBtnExt1.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ucBtnExt1.IsRadius = true;
            this.ucBtnExt1.IsShowRect = true;
            this.ucBtnExt1.IsShowTips = false;
            this.ucBtnExt1.Location = new System.Drawing.Point(550, 467);
            this.ucBtnExt1.Margin = new System.Windows.Forms.Padding(5);
            this.ucBtnExt1.Name = "ucBtnExt1";
            this.ucBtnExt1.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.ucBtnExt1.RectWidth = 1;
            this.ucBtnExt1.Size = new System.Drawing.Size(30, 30);
            this.ucBtnExt1.TabIndex = 14;
            this.ucBtnExt1.TabStop = false;
            this.ucBtnExt1.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.ucBtnExt1.TipsText = "";
            this.ucBtnExt1.BtnClick += new System.EventHandler(this.ucBtnExt1_BtnClick);
            // 
            // btnEnd
            // 
            this.btnEnd.BackColor = System.Drawing.Color.White;
            this.btnEnd.BtnBackColor = System.Drawing.Color.White;
            this.btnEnd.BtnFont = new System.Drawing.Font("微软雅黑", 9F);
            this.btnEnd.BtnForeColor = System.Drawing.Color.Gray;
            this.btnEnd.BtnText = ">>";
            this.btnEnd.ConerRadius = 5;
            this.btnEnd.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnEnd.EnabledMouseEffect = false;
            this.btnEnd.FillColor = System.Drawing.Color.White;
            this.btnEnd.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnEnd.IsRadius = true;
            this.btnEnd.IsShowRect = true;
            this.btnEnd.IsShowTips = false;
            this.btnEnd.Location = new System.Drawing.Point(590, 467);
            this.btnEnd.Margin = new System.Windows.Forms.Padding(5);
            this.btnEnd.Name = "btnEnd";
            this.btnEnd.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnEnd.RectWidth = 1;
            this.btnEnd.Size = new System.Drawing.Size(30, 30);
            this.btnEnd.TabIndex = 15;
            this.btnEnd.TabStop = false;
            this.btnEnd.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnEnd.TipsText = "";
            this.btnEnd.BtnClick += new System.EventHandler(this.btnEnd_BtnClick);
            // 
            // txtPage
            // 
            this.txtPage.BackColor = System.Drawing.Color.White;
            this.txtPage.ConerRadius = 5;
            this.txtPage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtPage.DecLength = 2;
            this.txtPage.FillColor = System.Drawing.Color.Empty;
            this.txtPage.FocusBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(77)))), ((int)(((byte)(59)))));
            this.txtPage.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPage.ForeColor = System.Drawing.Color.Gray;
            this.txtPage.InputText = "";
            this.txtPage.InputType = HZH_Controls.TextInputType.PositiveInteger;
            this.txtPage.IsFocusColor = true;
            this.txtPage.IsRadius = true;
            this.txtPage.IsShowClearBtn = false;
            this.txtPage.IsShowKeyboard = false;
            this.txtPage.IsShowRect = true;
            this.txtPage.IsShowSearchBtn = false;
            this.txtPage.KeyBoardType = HZH_Controls.Controls.KeyBoardType.UCKeyBorderAll_EN;
            this.txtPage.Location = new System.Drawing.Point(629, 467);
            this.txtPage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtPage.MaxValue = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.txtPage.MinValue = new decimal(new int[] {
            1000000,
            0,
            0,
            -2147483648});
            this.txtPage.Name = "txtPage";
            this.txtPage.Padding = new System.Windows.Forms.Padding(5);
            this.txtPage.PasswordChar = '\0';
            this.txtPage.PromptColor = System.Drawing.Color.Silver;
            this.txtPage.PromptFont = new System.Drawing.Font("微软雅黑", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtPage.PromptText = "页码";
            this.txtPage.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txtPage.RectWidth = 1;
            this.txtPage.RegexPattern = "";
            this.txtPage.Size = new System.Drawing.Size(62, 30);
            this.txtPage.TabIndex = 16;
            // 
            // btnToPage
            // 
            this.btnToPage.BackColor = System.Drawing.Color.White;
            this.btnToPage.BtnBackColor = System.Drawing.Color.White;
            this.btnToPage.BtnFont = new System.Drawing.Font("微软雅黑", 11F);
            this.btnToPage.BtnForeColor = System.Drawing.Color.Gray;
            this.btnToPage.BtnText = "跳转";
            this.btnToPage.ConerRadius = 5;
            this.btnToPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnToPage.EnabledMouseEffect = false;
            this.btnToPage.FillColor = System.Drawing.Color.White;
            this.btnToPage.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.btnToPage.IsRadius = true;
            this.btnToPage.IsShowRect = true;
            this.btnToPage.IsShowTips = false;
            this.btnToPage.Location = new System.Drawing.Point(700, 467);
            this.btnToPage.Margin = new System.Windows.Forms.Padding(5);
            this.btnToPage.Name = "btnToPage";
            this.btnToPage.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(223)))), ((int)(((byte)(223)))), ((int)(((byte)(223)))));
            this.btnToPage.RectWidth = 1;
            this.btnToPage.Size = new System.Drawing.Size(62, 30);
            this.btnToPage.TabIndex = 17;
            this.btnToPage.TabStop = false;
            this.btnToPage.TipsColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(30)))), ((int)(((byte)(99)))));
            this.btnToPage.TipsText = "";
            this.btnToPage.BtnClick += new System.EventHandler(this.btnToPage_BtnClick);
            // 
            // Details
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(878, 559);
            this.Controls.Add(this.DataList);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Details";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Details_Load);
            ((System.ComponentModel.ISupportInitialize)(this.webStationDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.selectCustomerAllBindingSource)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.DataList.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private WebStationDataSet webStationDataSet;
        private System.Windows.Forms.BindingSource selectCustomerAllBindingSource;
        private WebStationDataSetTableAdapters.SelectCustomerAllTableAdapter selectCustomerAllTableAdapter;
        private System.Windows.Forms.TabPage tabPage4;
        private HZH_Controls.Controls.UCDataGridView ucDataGridView1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label7;
        private CCWin.SkinControl.SkinButton skinButton3;
        private CCWin.SkinControl.SkinPanel skinPanel3;
        private CCWin.SkinControl.SkinAlphaWaterTextBox skinAlphaWaterTextBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private WinformControlLibraryExtension.DateExt dateExt3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label4;
        private CCWin.SkinControl.SkinButton skinButton2;
        private CCWin.SkinControl.SkinPanel skinPanel2;
        private CCWin.SkinControl.SkinAlphaWaterTextBox skinAlphaWaterTextBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private WinformControlLibraryExtension.DateExt dateExt2;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label3;
        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.SkinPanel skinPanel1;
        private CCWin.SkinControl.SkinAlphaWaterTextBox Password;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private WinformControlLibraryExtension.DateExt dateExt1;
        private System.Windows.Forms.TabControl DataList;
        private HZH_Controls.Controls.UCPagerControl2 ucPagerControl2;
        private HZH_Controls.Controls.UCBtnExt btnFirst;
        private HZH_Controls.Controls.UCBtnExt btnPrevious;
        private HZH_Controls.Controls.UCBtnExt ucBtnExt1;
        private HZH_Controls.Controls.UCBtnExt btnEnd;
        private HZH_Controls.Controls.UCTextBoxEx txtPage;
        private HZH_Controls.Controls.UCBtnExt btnToPage;
    }
}