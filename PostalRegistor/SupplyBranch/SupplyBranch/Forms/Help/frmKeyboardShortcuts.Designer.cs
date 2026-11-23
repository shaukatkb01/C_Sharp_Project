namespace SupplyBranch.Forms.Help
{
    partial class frmKeyboardShortcuts
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
            this.rtbShortcuts = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbShortcuts
            // 
            this.rtbShortcuts.BackColor = System.Drawing.Color.White;
            this.rtbShortcuts.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbShortcuts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbShortcuts.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.rtbShortcuts.Location = new System.Drawing.Point(0, 0);
            this.rtbShortcuts.Name = "rtbShortcuts";
            this.rtbShortcuts.ReadOnly = true;
            this.rtbShortcuts.Size = new System.Drawing.Size(800, 450);
            this.rtbShortcuts.TabIndex = 0;
            this.rtbShortcuts.Text = "";
            // 
            // frmKeyboardShortcuts
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rtbShortcuts);
            this.Name = "frmKeyboardShortcuts";
            this.Text = "frmKeyboardShortcuts";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbShortcuts;
    }
}