namespace FileIndex
{
    partial class UserRoles
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
            btn_Assign = new Button();
            DataGridView = new DataGridView();
            cmb_User = new ComboBox();
            flowLayoutPanel2 = new FlowLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            cmb_Roles = new ComboBox();
            panel2 = new Panel();
            btn_delet = new Button();
            panel1 = new Panel();
            ((System.ComponentModel.ISupportInitialize)DataGridView).BeginInit();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // btn_Assign
            // 
            btn_Assign.Location = new Point(280, 114);
            btn_Assign.Name = "btn_Assign";
            btn_Assign.Size = new Size(124, 44);
            btn_Assign.TabIndex = 153;
            btn_Assign.Text = "Save";
            btn_Assign.UseVisualStyleBackColor = true;
            btn_Assign.Click += btn_Assign_Click;
            // 
            // DataGridView
            // 
            DataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            DataGridView.Dock = DockStyle.Fill;
            DataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
            DataGridView.Location = new Point(0, 0);
            DataGridView.Name = "DataGridView";
            DataGridView.Size = new Size(800, 232);
            DataGridView.TabIndex = 151;
            // 
            // cmb_User
            // 
            cmb_User.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_User.FormattingEnabled = true;
            cmb_User.Location = new Point(104, 124);
            cmb_User.Name = "cmb_User";
            cmb_User.Size = new Size(128, 29);
            cmb_User.TabIndex = 152;
           
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel2.Location = new Point(226, 76);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(300, 3);
            flowLayoutPanel2.TabIndex = 154;
            // 
            // label1
            // 
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.FromArgb(0, 165, 255);
            label1.Location = new Point(170, 21);
            label1.Name = "label1";
            label1.Size = new Size(398, 51);
            label1.TabIndex = 155;
            label1.Text = "Assign Users Role";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 132);
            label2.Name = "label2";
            label2.Size = new Size(88, 21);
            label2.TabIndex = 156;
            label2.Text = "User Name";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 178);
            label3.Name = "label3";
            label3.Size = new Size(77, 21);
            label3.TabIndex = 158;
            label3.Text = "User Role";
            // 
            // cmb_Roles
            // 
            cmb_Roles.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_Roles.FormattingEnabled = true;
            cmb_Roles.Location = new Point(104, 170);
            cmb_Roles.Name = "cmb_Roles";
            cmb_Roles.Size = new Size(128, 29);
            cmb_Roles.TabIndex = 157;
            // 
            // panel2
            // 
            panel2.Controls.Add(btn_delet);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(flowLayoutPanel2);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(cmb_Roles);
            panel2.Controls.Add(btn_Assign);
            panel2.Controls.Add(cmb_User);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 220);
            panel2.TabIndex = 160;
            // 
            // btn_delet
            // 
            btn_delet.Location = new Point(280, 164);
            btn_delet.Name = "btn_delet";
            btn_delet.Size = new Size(124, 44);
            btn_delet.TabIndex = 159;
            btn_delet.Text = "Delet";
            btn_delet.UseVisualStyleBackColor = true;
            btn_delet.Click += btn_delet_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(DataGridView);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 220);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 232);
            panel1.TabIndex = 161;
            // 
            // UserRoles
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Controls.Add(panel2);
            Name = "UserRoles";
            Text = "UserRoles";
            Load += UserRoles_Load;
            ((System.ComponentModel.ISupportInitialize)DataGridView).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btn_Assign;
        private DataGridView DataGridView;
        private ComboBox cmb_User;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label1;
        private Label label2;
        private Label label3;
        private ComboBox cmb_Roles;
        private Panel panel2;
        private Button btn_delet;
        private Panel panel1;
    }
}