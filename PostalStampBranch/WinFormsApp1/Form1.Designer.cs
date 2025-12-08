namespace WinFormsApp1
{
    partial class LoginForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            user_logintab = new TabControl();
            logingtab = new TabPage();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            login_from_closebtn = new Button();
            user_loginbtn = new Button();
            login_passwordlb = new Label();
            user_namelb = new Label();
            login_user_passwordtxt = new TextBox();
            login_user_nametxt = new TextBox();
            user_registortab = new TabPage();
            imageList1 = new ImageList(components);
            user_logintab.SuspendLayout();
            logingtab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // user_logintab
            // 
            user_logintab.Controls.Add(logingtab);
            user_logintab.Controls.Add(user_registortab);
            user_logintab.Location = new Point(0, 4);
            user_logintab.Name = "user_logintab";
            user_logintab.SelectedIndex = 0;
            user_logintab.Size = new Size(799, 446);
            user_logintab.TabIndex = 0;
            // 
            // logingtab
            // 
            logingtab.Controls.Add(label1);
            logingtab.Controls.Add(pictureBox1);
            logingtab.Controls.Add(login_from_closebtn);
            logingtab.Controls.Add(user_loginbtn);
            logingtab.Controls.Add(login_passwordlb);
            logingtab.Controls.Add(user_namelb);
            logingtab.Controls.Add(login_user_passwordtxt);
            logingtab.Controls.Add(login_user_nametxt);
            logingtab.Location = new Point(4, 30);
            logingtab.Name = "logingtab";
            logingtab.Padding = new Padding(3);
            logingtab.Size = new Size(791, 412);
            logingtab.TabIndex = 0;
            logingtab.Text = "Login";
            logingtab.Click += logintab_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 21.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(208, 78);
            label1.Name = "label1";
            label1.Size = new Size(153, 40);
            label1.TabIndex = 7;
            label1.Text = "User Login";
            label1.Click += label1_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.LoginScreen;
            pictureBox1.InitialImage = Properties.Resources.LoginScreen;
            pictureBox1.Location = new Point(569, 187);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(158, 131);
            pictureBox1.TabIndex = 6;
            pictureBox1.TabStop = false;
            // 
            // login_from_closebtn
            // 
            login_from_closebtn.Location = new Point(300, 305);
            login_from_closebtn.Name = "login_from_closebtn";
            login_from_closebtn.Size = new Size(120, 27);
            login_from_closebtn.TabIndex = 5;
            login_from_closebtn.Text = "Close";
            login_from_closebtn.UseVisualStyleBackColor = true;
            login_from_closebtn.Click += login_from_closebtn_Click;
            // 
            // user_loginbtn
            // 
            user_loginbtn.Location = new Point(139, 305);
            user_loginbtn.Name = "user_loginbtn";
            user_loginbtn.Size = new Size(120, 27);
            user_loginbtn.TabIndex = 4;
            user_loginbtn.Text = "Login";
            user_loginbtn.UseVisualStyleBackColor = true;
            // 
            // login_passwordlb
            // 
            login_passwordlb.AutoSize = true;
            login_passwordlb.Location = new Point(48, 233);
            login_passwordlb.Name = "login_passwordlb";
            login_passwordlb.Size = new Size(79, 21);
            login_passwordlb.TabIndex = 3;
            login_passwordlb.Text = "Password:";
            // 
            // user_namelb
            // 
            user_namelb.AutoSize = true;
            user_namelb.Location = new Point(48, 166);
            user_namelb.Name = "user_namelb";
            user_namelb.Size = new Size(87, 21);
            user_namelb.TabIndex = 2;
            user_namelb.Text = "UserName:";
            // 
            // login_user_passwordtxt
            // 
            login_user_passwordtxt.Location = new Point(139, 230);
            login_user_passwordtxt.Name = "login_user_passwordtxt";
            login_user_passwordtxt.PasswordChar = '*';
            login_user_passwordtxt.Size = new Size(334, 29);
            login_user_passwordtxt.TabIndex = 1;
            // 
            // login_user_nametxt
            // 
            login_user_nametxt.Location = new Point(139, 166);
            login_user_nametxt.Name = "login_user_nametxt";
            login_user_nametxt.Size = new Size(334, 29);
            login_user_nametxt.TabIndex = 0;
            // 
            // user_registortab
            // 
            user_registortab.Location = new Point(4, 30);
            user_registortab.Name = "user_registortab";
            user_registortab.Padding = new Padding(3);
            user_registortab.Size = new Size(791, 387);
            user_registortab.TabIndex = 1;
            user_registortab.Text = "Registor";
            user_registortab.UseVisualStyleBackColor = true;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(16, 16);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // LoginForm
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(user_logintab);
            Name = "LoginForm";
            Text = "Login";
            user_logintab.ResumeLayout(false);
            logingtab.ResumeLayout(false);
            logingtab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl user_logintab;
        private TabPage logingtab;
        private Label login_passwordlb;
        private Label user_namelb;
        private TextBox login_user_passwordtxt;
        private TextBox login_user_nametxt;
        private TabPage user_registortab;
        private Button login_from_closebtn;
        private Button user_loginbtn;
        private PictureBox pictureBox1;
        private ImageList imageList1;
        private Label label1;

        // Add this method to the partial class to resolve CS0103 for 'logintab_Click'
        private void logintab_Click(object sender, EventArgs e)
        {
            // You can add any logic here if needed, or leave it empty if not required.
        }
    }
}
