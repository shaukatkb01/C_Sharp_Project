namespace SupplyBranch.Forms
{
    partial class frmDashBoard
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
            this.cmb_Task = new System.Windows.Forms.ComboBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dgvTasks = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.dgvSuppliesBottom = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliesBottom)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmb_Task
            // 
            this.cmb_Task.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmb_Task.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_Task.FormattingEnabled = true;
            this.cmb_Task.Location = new System.Drawing.Point(3, 29);
            this.cmb_Task.Name = "cmb_Task";
            this.cmb_Task.Size = new System.Drawing.Size(713, 21);
            this.cmb_Task.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Location = new System.Drawing.Point(722, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 29);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "Add";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click_1);
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(722, 47);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 29);
            this.button1.TabIndex = 2;
            this.button1.Text = "Delete";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // dgvTasks
            // 
            this.dgvTasks.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTasks.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTasks.Location = new System.Drawing.Point(0, 0);
            this.dgvTasks.Name = "dgvTasks";
            this.dgvTasks.ReadOnly = true;
            this.dgvTasks.Size = new System.Drawing.Size(800, 174);
            this.dgvTasks.TabIndex = 3;
            this.dgvTasks.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTasks_CellContentClick);
            this.dgvTasks.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTasks_CellContentDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.cmb_Task);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(800, 91);
            this.panel1.TabIndex = 4;
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 91);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.dgvTasks);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.dgvSuppliesBottom);
            this.splitContainerMain.Size = new System.Drawing.Size(800, 359);
            this.splitContainerMain.SplitterDistance = 180;
            this.splitContainerMain.TabIndex = 5;

            // 
            // dgvSuppliesBottom
            // 
            this.dgvSuppliesBottom.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSuppliesBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSuppliesBottom.Location = new System.Drawing.Point(0, 0);
            this.dgvSuppliesBottom.Name = "dgvSuppliesBottom";
            this.dgvSuppliesBottom.ReadOnly = true;
            this.dgvSuppliesBottom.Size = new System.Drawing.Size(800, 175);
            this.dgvSuppliesBottom.TabIndex = 4;
            // 
            // frmDashBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.panel1);
            this.Name = "frmDashBoard";
            this.Text = "Task Dashboard";
            this.Load += new System.EventHandler(this.frmDashBoard_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTasks)).EndInit();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSuppliesBottom)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cmb_Task;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dgvTasks;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.DataGridView dgvSuppliesBottom;
    }
}