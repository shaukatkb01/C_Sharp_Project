namespace FileIndex
{
    partial class Main
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            pnl_Top = new Panel();
            btn_task = new Button();
            button1 = new Button();
            btn_logout = new Button();
            txt_userName = new Label();
            pnl_Sidebar = new Panel();
            btn_Address = new Button();
            btn_Admin = new Button();
            btn_InvoiceWork = new Button();
            btn_PhiletalicSupply = new Button();
            btn_Issue = new Button();
            btn_Com = new Button();
            btn_File = new Button();
            sqlCommandBuilder1 = new Microsoft.Data.SqlClient.SqlCommandBuilder();
            pnl_Main = new Panel();
            pnl_Loading = new Panel();
            label45 = new Label();
            tabControl1 = new TabControl();
            tab_1 = new TabPage();
            tab_2 = new TabPage();
            tab_3 = new TabPage();
            tab_4 = new TabPage();
            Dashboard = new TabPage();
            tab_6 = new TabPage();
            tab_7 = new TabPage();
            timer1 = new System.Windows.Forms.Timer(components);
            sqlCommandBuilder2 = new Microsoft.Data.SqlClient.SqlCommandBuilder();
            pnl_Top.SuspendLayout();
            pnl_Sidebar.SuspendLayout();
            pnl_Main.SuspendLayout();
            pnl_Loading.SuspendLayout();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // pnl_Top
            // 
            pnl_Top.BackColor = Color.SkyBlue;
            pnl_Top.Controls.Add(btn_task);
            pnl_Top.Controls.Add(button1);
            pnl_Top.Controls.Add(btn_logout);
            pnl_Top.Controls.Add(txt_userName);
            pnl_Top.Dock = DockStyle.Top;
            pnl_Top.Location = new Point(0, 0);
            pnl_Top.Margin = new Padding(2, 2, 2, 2);
            pnl_Top.Name = "pnl_Top";
            pnl_Top.Size = new Size(966, 39);
            pnl_Top.TabIndex = 14;
            // 
            // btn_task
            // 
            btn_task.Location = new Point(702, 0);
            btn_task.Margin = new Padding(2, 2, 2, 2);
            btn_task.Name = "btn_task";
            btn_task.Size = new Size(104, 39);
            btn_task.TabIndex = 3;
            btn_task.Text = "Task";
            btn_task.UseVisualStyleBackColor = true;
            btn_task.Click += btn_task_Click;
            // 
            // button1
            // 
            button1.Location = new Point(507, 0);
            button1.Margin = new Padding(2, 2, 2, 2);
            button1.Name = "button1";
            button1.Size = new Size(171, 26);
            button1.TabIndex = 2;
            button1.Text = "data import from access";
            button1.UseVisualStyleBackColor = true;
            button1.Visible = false;
            button1.Click += button1_Click;
            // 
            // btn_logout
            // 
            btn_logout.Dock = DockStyle.Right;
            btn_logout.Image = (Image)resources.GetObject("btn_logout.Image");
            btn_logout.ImageAlign = ContentAlignment.MiddleLeft;
            btn_logout.Location = new Point(850, 0);
            btn_logout.Margin = new Padding(2, 2, 2, 2);
            btn_logout.Name = "btn_logout";
            btn_logout.Size = new Size(116, 39);
            btn_logout.TabIndex = 1;
            btn_logout.Text = "Logout";
            btn_logout.UseVisualStyleBackColor = true;
            btn_logout.Click += btn_logout_Click;
            // 
            // txt_userName
            // 
            txt_userName.AutoSize = true;
            txt_userName.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            txt_userName.Location = new Point(46, 5);
            txt_userName.Margin = new Padding(2, 0, 2, 0);
            txt_userName.Name = "txt_userName";
            txt_userName.Size = new Size(0, 21);
            txt_userName.TabIndex = 0;
            // 
            // pnl_Sidebar
            // 
            pnl_Sidebar.BackColor = Color.LightBlue;
            pnl_Sidebar.Controls.Add(btn_Address);
            pnl_Sidebar.Controls.Add(btn_Admin);
            pnl_Sidebar.Controls.Add(btn_InvoiceWork);
            pnl_Sidebar.Controls.Add(btn_PhiletalicSupply);
            pnl_Sidebar.Controls.Add(btn_Issue);
            pnl_Sidebar.Controls.Add(btn_Com);
            pnl_Sidebar.Controls.Add(btn_File);
            pnl_Sidebar.Location = new Point(0, 44);
            pnl_Sidebar.Margin = new Padding(2, 2, 2, 2);
            pnl_Sidebar.Name = "pnl_Sidebar";
            pnl_Sidebar.Size = new Size(33, 500);
            pnl_Sidebar.TabIndex = 17;
            // 
            // btn_Address
            // 
            btn_Address.Dock = DockStyle.Top;
            btn_Address.FlatAppearance.BorderSize = 0;
            btn_Address.FlatStyle = FlatStyle.Flat;
            btn_Address.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Address.ForeColor = Color.White;
            btn_Address.Image = (Image)resources.GetObject("btn_Address.Image");
            btn_Address.ImageAlign = ContentAlignment.MiddleLeft;
            btn_Address.Location = new Point(0, 216);
            btn_Address.Margin = new Padding(2, 2, 2, 2);
            btn_Address.Name = "btn_Address";
            btn_Address.Size = new Size(33, 36);
            btn_Address.TabIndex = 6;
            btn_Address.UseVisualStyleBackColor = true;
            btn_Address.Click += btn_Address_Click;
            // 
            // btn_Admin
            // 
            btn_Admin.Dock = DockStyle.Top;
            btn_Admin.FlatAppearance.BorderSize = 0;
            btn_Admin.FlatStyle = FlatStyle.Flat;
            btn_Admin.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Admin.ForeColor = Color.White;
            btn_Admin.Image = (Image)resources.GetObject("btn_Admin.Image");
            btn_Admin.ImageAlign = ContentAlignment.MiddleLeft;
            btn_Admin.Location = new Point(0, 180);
            btn_Admin.Margin = new Padding(2, 2, 2, 2);
            btn_Admin.Name = "btn_Admin";
            btn_Admin.Size = new Size(33, 36);
            btn_Admin.TabIndex = 5;
            btn_Admin.UseVisualStyleBackColor = true;
            btn_Admin.Click += btn_Admin_Click;
            // 
            // btn_InvoiceWork
            // 
            btn_InvoiceWork.Dock = DockStyle.Top;
            btn_InvoiceWork.FlatAppearance.BorderSize = 0;
            btn_InvoiceWork.FlatStyle = FlatStyle.Flat;
            btn_InvoiceWork.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_InvoiceWork.ForeColor = Color.White;
            btn_InvoiceWork.Image = (Image)resources.GetObject("btn_InvoiceWork.Image");
            btn_InvoiceWork.ImageAlign = ContentAlignment.MiddleLeft;
            btn_InvoiceWork.Location = new Point(0, 144);
            btn_InvoiceWork.Margin = new Padding(2, 2, 2, 2);
            btn_InvoiceWork.Name = "btn_InvoiceWork";
            btn_InvoiceWork.Size = new Size(33, 36);
            btn_InvoiceWork.TabIndex = 4;
            btn_InvoiceWork.UseVisualStyleBackColor = true;
            btn_InvoiceWork.Click += btn_InvoiceWork_Click;
            // 
            // btn_PhiletalicSupply
            // 
            btn_PhiletalicSupply.Dock = DockStyle.Top;
            btn_PhiletalicSupply.FlatAppearance.BorderSize = 0;
            btn_PhiletalicSupply.FlatStyle = FlatStyle.Flat;
            btn_PhiletalicSupply.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_PhiletalicSupply.ForeColor = Color.White;
            btn_PhiletalicSupply.Image = (Image)resources.GetObject("btn_PhiletalicSupply.Image");
            btn_PhiletalicSupply.ImageAlign = ContentAlignment.MiddleLeft;
            btn_PhiletalicSupply.Location = new Point(0, 108);
            btn_PhiletalicSupply.Margin = new Padding(2, 2, 2, 2);
            btn_PhiletalicSupply.Name = "btn_PhiletalicSupply";
            btn_PhiletalicSupply.Size = new Size(33, 36);
            btn_PhiletalicSupply.TabIndex = 3;
            btn_PhiletalicSupply.UseVisualStyleBackColor = true;
            btn_PhiletalicSupply.Click += btn_PhiletalicSupply_Click;
            // 
            // btn_Issue
            // 
            btn_Issue.Dock = DockStyle.Top;
            btn_Issue.FlatAppearance.BorderSize = 0;
            btn_Issue.FlatStyle = FlatStyle.Flat;
            btn_Issue.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Issue.ForeColor = Color.White;
            btn_Issue.Image = (Image)resources.GetObject("btn_Issue.Image");
            btn_Issue.ImageAlign = ContentAlignment.MiddleLeft;
            btn_Issue.Location = new Point(0, 72);
            btn_Issue.Margin = new Padding(2, 2, 2, 2);
            btn_Issue.Name = "btn_Issue";
            btn_Issue.Size = new Size(33, 36);
            btn_Issue.TabIndex = 2;
            btn_Issue.UseVisualStyleBackColor = true;
            btn_Issue.Click += btn_Issue_Click;
            // 
            // btn_Com
            // 
            btn_Com.Dock = DockStyle.Top;
            btn_Com.FlatAppearance.BorderSize = 0;
            btn_Com.FlatStyle = FlatStyle.Flat;
            btn_Com.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_Com.ForeColor = Color.White;
            btn_Com.Image = (Image)resources.GetObject("btn_Com.Image");
            btn_Com.ImageAlign = ContentAlignment.MiddleLeft;
            btn_Com.Location = new Point(0, 36);
            btn_Com.Margin = new Padding(2, 2, 2, 2);
            btn_Com.Name = "btn_Com";
            btn_Com.Size = new Size(33, 36);
            btn_Com.TabIndex = 1;
            btn_Com.UseVisualStyleBackColor = true;
            btn_Com.Click += btn_Com_Click;
            // 
            // btn_File
            // 
            btn_File.Dock = DockStyle.Top;
            btn_File.FlatAppearance.BorderSize = 0;
            btn_File.FlatStyle = FlatStyle.Flat;
            btn_File.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btn_File.ForeColor = Color.White;
            btn_File.Image = (Image)resources.GetObject("btn_File.Image");
            btn_File.ImageAlign = ContentAlignment.MiddleLeft;
            btn_File.Location = new Point(0, 0);
            btn_File.Margin = new Padding(2, 2, 2, 2);
            btn_File.Name = "btn_File";
            btn_File.Size = new Size(33, 36);
            btn_File.TabIndex = 0;
            btn_File.UseVisualStyleBackColor = true;
            btn_File.Click += btn_File_Click;
            // 
            // pnl_Main
            // 
            pnl_Main.BackColor = Color.AliceBlue;
            pnl_Main.Controls.Add(pnl_Loading);
            pnl_Main.Controls.Add(tabControl1);
            pnl_Main.Dock = DockStyle.Fill;
            pnl_Main.Location = new Point(0, 0);
            pnl_Main.Margin = new Padding(8, 2, 2, 2);
            pnl_Main.Name = "pnl_Main";
            pnl_Main.Padding = new Padding(39, 50, 0, 0);
            pnl_Main.Size = new Size(966, 544);
            pnl_Main.TabIndex = 15;
            // 
            // pnl_Loading
            // 
            pnl_Loading.BackColor = Color.LightGray;
            pnl_Loading.Controls.Add(label45);
            pnl_Loading.Dock = DockStyle.Fill;
            pnl_Loading.Location = new Point(39, 50);
            pnl_Loading.Margin = new Padding(2, 2, 2, 2);
            pnl_Loading.Name = "pnl_Loading";
            pnl_Loading.Size = new Size(927, 494);
            pnl_Loading.TabIndex = 423;
            pnl_Loading.Visible = false;
            // 
            // label45
            // 
            label45.AutoSize = true;
            label45.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label45.Location = new Point(367, 29);
            label45.Margin = new Padding(2, 0, 2, 0);
            label45.Name = "label45";
            label45.Size = new Size(477, 45);
            label45.TabIndex = 0;
            label45.Text = "Please Wait... Loading Records";
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top;
            tabControl1.Controls.Add(tab_1);
            tabControl1.Controls.Add(tab_2);
            tabControl1.Controls.Add(tab_3);
            tabControl1.Controls.Add(tab_4);
            tabControl1.Controls.Add(Dashboard);
            tabControl1.Controls.Add(tab_6);
            tabControl1.Controls.Add(tab_7);
            tabControl1.ItemSize = new Size(200, 50);
            tabControl1.Location = new Point(79, 90);
            tabControl1.Margin = new Padding(16, 2, 2, 2);
            tabControl1.Multiline = true;
            tabControl1.Name = "tabControl1";
            tabControl1.Padding = new Point(5, 3);
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(913, 711);
            tabControl1.SizeMode = TabSizeMode.FillToRight;
            tabControl1.TabIndex = 0;
            // 
            // tab_1
            // 
            tab_1.AutoScroll = true;
            tab_1.Location = new Point(4, 54);
            tab_1.Margin = new Padding(2, 2, 2, 2);
            tab_1.Name = "tab_1";
            tab_1.Padding = new Padding(2, 2, 2, 2);
            tab_1.Size = new Size(905, 653);
            tab_1.TabIndex = 0;
            tab_1.Text = "tab_1";
            tab_1.UseVisualStyleBackColor = true;
            // 
            // tab_2
            // 
            tab_2.AutoScroll = true;
            tab_2.Location = new Point(4, 54);
            tab_2.Margin = new Padding(2, 2, 2, 2);
            tab_2.Name = "tab_2";
            tab_2.Padding = new Padding(2, 2, 2, 2);
            tab_2.Size = new Size(905, 653);
            tab_2.TabIndex = 1;
            tab_2.Text = "tab_2";
            tab_2.UseVisualStyleBackColor = true;
            // 
            // tab_3
            // 
            tab_3.AutoScroll = true;
            tab_3.Location = new Point(4, 54);
            tab_3.Margin = new Padding(2, 2, 2, 2);
            tab_3.Name = "tab_3";
            tab_3.Size = new Size(905, 653);
            tab_3.TabIndex = 2;
            tab_3.Text = "tab_3";
            tab_3.UseVisualStyleBackColor = true;
            // 
            // tab_4
            // 
            tab_4.AutoScroll = true;
            tab_4.Location = new Point(4, 54);
            tab_4.Margin = new Padding(2, 2, 2, 2);
            tab_4.Name = "tab_4";
            tab_4.Padding = new Padding(2, 2, 2, 2);
            tab_4.Size = new Size(905, 653);
            tab_4.TabIndex = 3;
            tab_4.Text = "tab_4";
            tab_4.UseVisualStyleBackColor = true;
            // 
            // Dashboard
            // 
            Dashboard.AutoScroll = true;
            Dashboard.Location = new Point(4, 54);
            Dashboard.Margin = new Padding(2, 2, 2, 2);
            Dashboard.Name = "Dashboard";
            Dashboard.Padding = new Padding(2, 2, 2, 2);
            Dashboard.Size = new Size(905, 653);
            Dashboard.TabIndex = 4;
            Dashboard.Text = "Task";
            Dashboard.UseVisualStyleBackColor = true;
            // 
            // tab_6
            // 
            tab_6.AutoScroll = true;
            tab_6.Location = new Point(4, 54);
            tab_6.Margin = new Padding(2, 2, 2, 2);
            tab_6.Name = "tab_6";
            tab_6.Size = new Size(905, 653);
            tab_6.TabIndex = 5;
            tab_6.Text = "tab_6";
            tab_6.UseVisualStyleBackColor = true;
            // 
            // tab_7
            // 
            tab_7.AutoScroll = true;
            tab_7.Location = new Point(4, 54);
            tab_7.Margin = new Padding(2, 2, 2, 2);
            tab_7.Name = "tab_7";
            tab_7.Size = new Size(905, 653);
            tab_7.TabIndex = 6;
            tab_7.Text = "tab_7";
            tab_7.UseVisualStyleBackColor = true;
            // 
            // timer1
            // 
            timer1.Enabled = true;
            timer1.Interval = 1;
            timer1.Tick += timer1_Tick;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            BackColor = Color.RoyalBlue;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(983, 500);
            Controls.Add(pnl_Top);
            Controls.Add(pnl_Sidebar);
            Controls.Add(pnl_Main);
            DoubleBuffered = true;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Margin = new Padding(2, 2, 2, 2);
            Name = "Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Main";
            WindowState = FormWindowState.Maximized;
            FormClosed += Main_FormClosed;
            Load += Main_Load;
            pnl_Top.ResumeLayout(false);
            pnl_Top.PerformLayout();
            pnl_Sidebar.ResumeLayout(false);
            pnl_Main.ResumeLayout(false);
            pnl_Loading.ResumeLayout(false);
            pnl_Loading.PerformLayout();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Panel pnl_Top;
        private Panel pnl_Sidebar;
        private Button btn_InvoiceWork;
        private Button btn_PhiletalicSupply;
        private Button btn_Issue;
        private Button btn_Com;
        private Button btn_File;
        private Microsoft.Data.SqlClient.SqlCommandBuilder sqlCommandBuilder1;
        private dtTable.dsPhilatelic dsPhilatelic1;
        private Panel pnl_Main;
        private TabControl tabControl1;
        private TabPage tab_1;
        private TabPage tab_2;
        private TabPage tab_3;
        private System.Windows.Forms.Timer timer1;
        private TabPage tab_4;
        private Button btn_Admin;
        private Label txt_userName;
        private TabPage Dashboard;
        private Microsoft.Data.SqlClient.SqlCommandBuilder sqlCommandBuilder2;
        private Button btn_Address;
        private TabPage tab_6;
        private TabPage tab_7;
        private Button btn_logout;
        private Button button1;
        private Button btn_task;
        private Panel pnl_Loading;
        private Label label45;
    }
}