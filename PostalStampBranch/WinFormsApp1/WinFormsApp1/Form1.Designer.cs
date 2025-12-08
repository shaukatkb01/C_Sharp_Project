namespace WinFormsApp1
{
    partial class LoginF
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
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            user_nL = new Label();
            passwordL = new Label();
            RegitorB = new Button();
            LoginB = new Button();
            LoginTb = new TabControl();
            tabPage1 = new TabPage();
            RegistorTb = new TabPage();
            LoginTb.SuspendLayout();
            tabPage1.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(187, 13);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "User Name";
            textBox1.Size = new Size(222, 29);
            textBox1.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(187, 67);
            textBox2.Name = "textBox2";
            textBox2.PasswordChar = '*';
            textBox2.PlaceholderText = "Password";
            textBox2.Size = new Size(222, 29);
            textBox2.TabIndex = 1;
            // 
            // user_nL
            // 
            user_nL.AutoSize = true;
            user_nL.Location = new Point(10, 21);
            user_nL.Name = "user_nL";
            user_nL.Size = new Size(88, 21);
            user_nL.TabIndex = 2;
            user_nL.Text = "User Name";
            // 
            // passwordL
            // 
            passwordL.AutoSize = true;
            passwordL.Location = new Point(10, 75);
            passwordL.Name = "passwordL";
            passwordL.Size = new Size(76, 21);
            passwordL.TabIndex = 3;
            passwordL.Text = "Password";
            // 
            // RegitorB
            // 
            RegitorB.Location = new Point(296, 162);
            RegitorB.Name = "RegitorB";
            RegitorB.Size = new Size(109, 28);
            RegitorB.TabIndex = 5;
            RegitorB.Text = "Regitor";
            RegitorB.UseVisualStyleBackColor = true;
            // 
            // LoginB
            // 
            LoginB.Location = new Point(140, 162);
            LoginB.Name = "LoginB";
            LoginB.Size = new Size(109, 28);
            LoginB.TabIndex = 4;
            LoginB.Text = "Login";
            LoginB.UseVisualStyleBackColor = true;
            LoginB.Click += button2_Click;
            // 
            // LoginTb
            // 
            LoginTb.Controls.Add(tabPage1);
            LoginTb.Controls.Add(RegistorTb);
            LoginTb.Location = new Point(2, 12);
            LoginTb.Name = "LoginTb";
            LoginTb.SelectedIndex = 0;
            LoginTb.Size = new Size(799, 438);
            LoginTb.TabIndex = 6;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = SystemColors.ControlLight;
            tabPage1.Controls.Add(LoginB);
            tabPage1.Controls.Add(textBox1);
            tabPage1.Controls.Add(RegitorB);
            tabPage1.Controls.Add(textBox2);
            tabPage1.Controls.Add(passwordL);
            tabPage1.Controls.Add(user_nL);
            tabPage1.ImeMode = ImeMode.NoControl;
            tabPage1.Location = new Point(4, 30);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(791, 404);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Login";
            // 
            // RegistorTb
            // 
            RegistorTb.BackColor = SystemColors.ControlLight;
            RegistorTb.Location = new Point(4, 30);
            RegistorTb.Name = "RegistorTb";
            RegistorTb.Padding = new Padding(3);
            RegistorTb.Size = new Size(791, 404);
            RegistorTb.TabIndex = 1;
            RegistorTb.Text = "Registor";
            // 
            // LoginF
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLight;
            ClientSize = new Size(800, 450);
            Controls.Add(LoginTb);
            Name = "LoginF";
            Text = "Login";
            LoginTb.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TextBox textBox1;
        private TextBox textBox2;
        private Label user_nL;
        private Label passwordL;
        private Button RegitorB;
        private Button LoginB;
        private TabControl LoginTb;
        private TabPage tabPage1;
        private TabPage RegistorTb;
    }
}
