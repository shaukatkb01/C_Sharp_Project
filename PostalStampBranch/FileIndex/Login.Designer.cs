

namespace FileIndex
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            userTxt = new TextBox();
            label1 = new Label();
            label2 = new Label();
            ShowPasswordlbl = new Label();
            passwordTxt = new TextBox();
            loginBtn = new Button();
            label3 = new Label();
            button1 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            showPasswordCheck = new CheckBox();
            linkLabel1 = new LinkLabel();
            pictureBox3 = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // userTxt
            // 
            userTxt.BorderStyle = BorderStyle.None;
            userTxt.Font = new Font("Segoe UI", 14F);
            userTxt.Location = new Point(107, 138);
            userTxt.Name = "userTxt";
            userTxt.PlaceholderText = "UserName";
            userTxt.Size = new Size(267, 25);
            userTxt.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(5, 142);
            label1.Name = "label1";
            label1.Size = new Size(84, 21);
            label1.TabIndex = 0;
            label1.Text = "UserName";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(5, 188);
            label2.Name = "label2";
            label2.Size = new Size(76, 21);
            label2.TabIndex = 1;
            label2.Text = "Password";
            // 
            // ShowPasswordlbl
            // 
            ShowPasswordlbl.AutoSize = true;
            ShowPasswordlbl.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            ShowPasswordlbl.ForeColor = Color.Gray;
            ShowPasswordlbl.Location = new Point(115, 399);
            ShowPasswordlbl.Name = "ShowPasswordlbl";
            ShowPasswordlbl.Size = new Size(143, 17);
            ShowPasswordlbl.TabIndex = 2;
            ShowPasswordlbl.Text = "Don't have an account?";
            // 
            // passwordTxt
            // 
            passwordTxt.Location = new Point(107, 180);
            passwordTxt.Name = "passwordTxt";
            passwordTxt.PlaceholderText = "Password";
            passwordTxt.Size = new Size(267, 29);
            passwordTxt.TabIndex = 2;
            passwordTxt.UseSystemPasswordChar = true;
            // 
            // loginBtn
            // 
            loginBtn.BackColor = Color.FromArgb(0, 165, 255);
            loginBtn.Cursor = Cursors.Hand;
            loginBtn.FlatAppearance.BorderSize = 0;
            loginBtn.FlatStyle = FlatStyle.Flat;
            loginBtn.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            loginBtn.ForeColor = Color.White;
            loginBtn.Location = new Point(35, 291);
            loginBtn.Name = "loginBtn";
            loginBtn.Size = new Size(141, 52);
            loginBtn.TabIndex = 4;
            loginBtn.Tag = "3";
            loginBtn.Text = "Login";
            loginBtn.UseVisualStyleBackColor = false;
            loginBtn.Click += loginBtn_Click;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(0, 165, 255);
            label3.Location = new Point(0, 21);
            label3.Name = "label3";
            label3.Size = new Size(398, 46);
            label3.TabIndex = 0;
            label3.Text = "WELCOME BACK";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(0, 165, 255);
            button1.Cursor = Cursors.Hand;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Location = new Point(210, 291);
            button1.Name = "button1";
            button1.Size = new Size(141, 52);
            button1.TabIndex = 5;
            button1.Tag = "4";
            button1.Text = "Foreget Password?";
            button1.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(51, 79);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(300, 3);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(343, 138);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(31, 24);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 8;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(343, 180);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(31, 29);
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 9;
            pictureBox2.TabStop = false;
            // 
            // showPasswordCheck
            // 
            showPasswordCheck.AutoSize = true;
            showPasswordCheck.Checked = true;
            showPasswordCheck.CheckState = CheckState.Indeterminate;
            showPasswordCheck.ForeColor = Color.White;
            showPasswordCheck.Location = new Point(107, 233);
            showPasswordCheck.Name = "showPasswordCheck";
            showPasswordCheck.Size = new Size(138, 25);
            showPasswordCheck.TabIndex = 3;
            showPasswordCheck.Text = "Show Password";
            showPasswordCheck.UseVisualStyleBackColor = true;
            showPasswordCheck.CheckedChanged += showPasswordCheck_CheckedChanged;
            // 
            // linkLabel1
            // 
            linkLabel1.ActiveLinkColor = Color.White;
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Segoe UI", 9.75F, FontStyle.Underline, GraphicsUnit.Point, 0);
            linkLabel1.LinkColor = Color.FromArgb(0, 165, 255);
            linkLabel1.Location = new Point(264, 399);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(57, 17);
            linkLabel1.TabIndex = 6;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Registor";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // pictureBox3
            // 
            pictureBox3.Cursor = Cursors.Hand;
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(343, 12);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(46, 35);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 10;
            pictureBox3.TabStop = false;
            pictureBox3.Click += pictureBox3_Click;
            // 
            // Login
            // 
            AcceptButton = loginBtn;
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(44, 62, 80);
            ClientSize = new Size(400, 500);
            Controls.Add(pictureBox3);
            Controls.Add(linkLabel1);
            Controls.Add(showPasswordCheck);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(button1);
            Controls.Add(label3);
            Controls.Add(loginBtn);
            Controls.Add(passwordTxt);
            Controls.Add(userTxt);
            Controls.Add(ShowPasswordlbl);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.None;
            MinimizeBox = false;
            Name = "Login";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            Load += Login_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label ShowPasswordlbl;
        private TextBox userTxt;
        private TextBox passwordTxt;
        private Button loginBtn;
        private Label label3;
        private Button button1;
        private FlowLayoutPanel flowLayoutPanel1;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private CheckBox showPasswordCheck;
        private LinkLabel linkLabel1;
        private PictureBox pictureBox3;
    }
}