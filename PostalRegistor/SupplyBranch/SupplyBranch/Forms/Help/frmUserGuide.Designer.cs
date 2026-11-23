namespace SupplyBranch.Forms.Help
{
    partial class frmUserGuide
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
            this.rtbGuide = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // rtbGuide
            // 
            this.rtbGuide.BackColor = System.Drawing.Color.White;
            this.rtbGuide.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbGuide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbGuide.Font = new System.Drawing.Font("Segoe UI", 10.25F);
            this.rtbGuide.Location = new System.Drawing.Point(0, 0);
            this.rtbGuide.Name = "rtbGuide";
            this.rtbGuide.ReadOnly = true;
            this.rtbGuide.Size = new System.Drawing.Size(784, 560);
            this.rtbGuide.TabIndex = 0;
            this.rtbGuide.Text = "";
            // 
            // frmUserGuide
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 560);
            this.Controls.Add(this.rtbGuide);
            this.Name = "frmUserGuide";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Supply Branch - User Guide";
            this.Load += new System.EventHandler(this.frmUserGuide_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox rtbGuide;
    }
}