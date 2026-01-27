namespace FileIndex
{
    partial class frmDashboard
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
            btnAdd = new Button();
            txtTaskDescription = new TextBox();
            dgvTasks = new DataGridView();
            panel1 = new Panel();
            button1 = new Button();
            cmb_Task = new ComboBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dgvTasks).BeginInit();
            panel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnAdd
            // 
            btnAdd.Location = new Point(641, 3);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(156, 32);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            btnAdd.Click += btnAdd_Click;
            // 
            // txtTaskDescription
            // 
            txtTaskDescription.Location = new Point(12, 0);
            txtTaskDescription.Name = "txtTaskDescription";
            txtTaskDescription.Size = new Size(623, 29);
            txtTaskDescription.TabIndex = 4;
            // 
            // dgvTasks
            // 
            dgvTasks.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvTasks.Location = new Point(3, 3);
            dgvTasks.Name = "dgvTasks";
            dgvTasks.Size = new Size(797, 347);
            dgvTasks.TabIndex = 3;
            dgvTasks.CellContentClick += dgvTasks_CellContentClick;
            dgvTasks.CellDoubleClick += dgvTasks_CellDoubleClick;
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.Controls.Add(button1);
            panel1.Controls.Add(cmb_Task);
            panel1.Controls.Add(btnAdd);
            panel1.Controls.Add(txtTaskDescription);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 100);
            panel1.TabIndex = 6;
            // 
            // button1
            // 
            button1.Location = new Point(641, 41);
            button1.Name = "button1";
            button1.Size = new Size(156, 32);
            button1.TabIndex = 7;
            button1.Text = "Delet";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // cmb_Task
            // 
            cmb_Task.FormattingEnabled = true;
            cmb_Task.Location = new Point(12, 41);
            cmb_Task.Name = "cmb_Task";
            cmb_Task.Size = new Size(623, 29);
            cmb_Task.TabIndex = 6;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(dgvTasks);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(0, 100);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(800, 350);
            flowLayoutPanel1.TabIndex = 7;
            // 
            // frmDashboard
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(panel1);
            Name = "frmDashboard";
            Text = "frmDashboard";
            Load += frmDashboard_Load;
            ((System.ComponentModel.ISupportInitialize)dgvTasks).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnAdd;
        private TextBox txtTaskDescription;
        private DataGridView dgvTasks;
        private Panel panel1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button button1;
        private ComboBox cmb_Task;
    }
}