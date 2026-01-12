namespace FileIndex
{
    partial class Register
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Register));
            flowLayoutPanel1 = new FlowLayoutPanel();
            label3 = new Label();
            textFullName = new TextBox();
            texUserName = new TextBox();
            textEmail = new TextBox();
            textPassword = new TextBox();
            textConfPassword = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            pictureBox1 = new PictureBox();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
            checkRegisterAgree = new CheckBox();
            btnRegister = new Button();
            label7 = new Label();
            linkLabel1 = new LinkLabel();
            CloseBtn = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(51, 77);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(300, 3);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            label3.BackColor = Color.Transparent;
            label3.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.ForeColor = Color.FromArgb(0, 165, 255);
            label3.Location = new Point(0, 19);
            label3.Name = "label3";
            label3.Size = new Size(398, 46);
            label3.TabIndex = 0;
            label3.Text = "REGISTER NOW";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            label3.Click += label3_Click;
            // 
            // textFullName
            // 
            textFullName.Location = new Point(183, 156);
            textFullName.Name = "textFullName";
            textFullName.PlaceholderText = "Full Name";
            textFullName.Size = new Size(205, 29);
            textFullName.TabIndex = 1;
            // 
            // texUserName
            // 
            texUserName.Location = new Point(183, 204);
            texUserName.Name = "texUserName";
            texUserName.PlaceholderText = "UserName";
            texUserName.Size = new Size(205, 29);
            texUserName.TabIndex = 2;
            // 
            // textEmail
            // 
            textEmail.Location = new Point(183, 250);
            textEmail.Name = "textEmail";
            textEmail.PlaceholderText = "Email";
            textEmail.Size = new Size(205, 29);
            textEmail.TabIndex = 3;
            // 
            // textPassword
            // 
            textPassword.Location = new Point(183, 295);
            textPassword.Name = "textPassword";
            textPassword.PlaceholderText = "Password";
            textPassword.Size = new Size(205, 29);
            textPassword.TabIndex = 4;
            textPassword.UseSystemPasswordChar = true;
            // 
            // textConfPassword
            // 
            textConfPassword.Location = new Point(183, 340);
            textConfPassword.Name = "textConfPassword";
            textConfPassword.PlaceholderText = "Confirm Password";
            textConfPassword.Size = new Size(205, 29);
            textConfPassword.TabIndex = 5;
            textConfPassword.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = Color.White;
            label1.Location = new Point(40, 164);
            label1.Name = "label1";
            label1.Size = new Size(81, 21);
            label1.TabIndex = 0;
            label1.Text = "Full Name";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.ForeColor = Color.White;
            label2.Location = new Point(40, 207);
            label2.Name = "label2";
            label2.Size = new Size(88, 21);
            label2.TabIndex = 0;
            label2.Text = "User Name";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(40, 258);
            label4.Name = "label4";
            label4.Size = new Size(48, 21);
            label4.TabIndex = 0;
            label4.Text = "Email";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.White;
            label5.Location = new Point(40, 303);
            label5.Name = "label5";
            label5.Size = new Size(76, 21);
            label5.TabIndex = 0;
            label5.Text = "Password";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = Color.White;
            label6.Location = new Point(40, 348);
            label6.Name = "label6";
            label6.Size = new Size(137, 21);
            label6.TabIndex = 0;
            label6.Text = "Confirm Password";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(357, 156);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(31, 29);
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(357, 204);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(31, 29);
            pictureBox2.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox2.TabIndex = 14;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(357, 250);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(31, 29);
            pictureBox3.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox3.TabIndex = 15;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(357, 295);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(31, 29);
            pictureBox4.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox4.TabIndex = 16;
            pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(357, 340);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(31, 29);
            pictureBox5.SizeMode = PictureBoxSizeMode.CenterImage;
            pictureBox5.TabIndex = 17;
            pictureBox5.TabStop = false;
            // 
            // checkRegisterAgree
            // 
            checkRegisterAgree.AutoSize = true;
            checkRegisterAgree.ForeColor = Color.White;
            checkRegisterAgree.Location = new Point(40, 401);
            checkRegisterAgree.Name = "checkRegisterAgree";
            checkRegisterAgree.Size = new Size(261, 25);
            checkRegisterAgree.TabIndex = 6;
            checkRegisterAgree.Text = "I agree to the Terms && Conditions";
            checkRegisterAgree.UseVisualStyleBackColor = true;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.FromArgb(0, 165, 255);
            btnRegister.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnRegister.ForeColor = Color.White;
            btnRegister.Location = new Point(40, 450);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(169, 46);
            btnRegister.TabIndex = 7;
            btnRegister.Text = "Register";
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.Click += button1_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.ForeColor = Color.White;
            label7.Location = new Point(40, 533);
            label7.Name = "label7";
            label7.Size = new Size(186, 21);
            label7.TabIndex = 18;
            label7.Text = "Already have an account?";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.LinkColor = Color.FromArgb(0, 165, 255);
            linkLabel1.Location = new Point(232, 533);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(49, 21);
            linkLabel1.TabIndex = 19;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "Login";
            linkLabel1.LinkClicked += linkLabel1_LinkClicked;
            // 
            // CloseBtn
            // 
            CloseBtn.BackColor = Color.FromArgb(0, 165, 255);
            CloseBtn.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            CloseBtn.ForeColor = Color.White;
            CloseBtn.Location = new Point(215, 450);
            CloseBtn.Name = "CloseBtn";
            CloseBtn.Size = new Size(169, 46);
            CloseBtn.TabIndex = 20;
            CloseBtn.Text = "Close";
            CloseBtn.UseVisualStyleBackColor = false;
            CloseBtn.Click += CloseBtn_Click;
            // 
            // Register
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(44, 62, 80);
            ClientSize = new Size(400, 600);
            Controls.Add(CloseBtn);
            Controls.Add(linkLabel1);
            Controls.Add(label7);
            Controls.Add(btnRegister);
            Controls.Add(checkRegisterAgree);
            Controls.Add(pictureBox5);
            Controls.Add(pictureBox4);
            Controls.Add(pictureBox3);
            Controls.Add(pictureBox2);
            Controls.Add(pictureBox1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textConfPassword);
            Controls.Add(textPassword);
            Controls.Add(textEmail);
            Controls.Add(texUserName);
            Controls.Add(textFullName);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label3);
            FormBorderStyle = FormBorderStyle.None;
            Name = "Register";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Registor";
            Load += Register_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private Label label3;
        private TextBox textFullName;
        private TextBox texUserName;
        private TextBox textEmail;
        private TextBox textPassword;
        private TextBox textConfPassword;
        private Label label1;
        private Label label2;
        private Label label4;
        private Label label5;
        private Label label6;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
        private CheckBox checkRegisterAgree;
        private Button btnRegister;
        private Label label7;
        private LinkLabel linkLabel1;
        public Button CloseBtn;
    }
}