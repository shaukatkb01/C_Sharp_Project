namespace FileIndex
{
    partial class IssuePrinting
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
            flowLayoutPanel1 = new FlowLayoutPanel();
            label5 = new Label();
            panel1 = new Panel();
            panel2 = new Panel();
            checkBox1 = new CheckBox();
            picker_DilverDate = new DateTimePicker();
            label3 = new Label();
            txt_JoborderSouv = new TextBox();
            txt_SouvOption = new TextBox();
            txt_Joborder = new TextBox();
            txt_StmOption = new TextBox();
            txt_DgRefer = new TextBox();
            btn_Foundation = new Button();
            btn_Printorder = new Button();
            btn_Promtprinter = new Button();
            btn_DGQty = new Button();
            txt_salient = new TextBox();
            label1 = new Label();
            cmb_Signature = new ComboBox();
            btn_DGCircular = new Button();
            btn_Tags = new Button();
            btn_Invoices = new Button();
            btn_Distribution = new Button();
            label2 = new Label();
            cmb_Issue_No = new ComboBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(379, 106);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(398, 3);
            flowLayoutPanel1.TabIndex = 166;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(0, 165, 255);
            label5.Location = new Point(379, 0);
            label5.Name = "label5";
            label5.Size = new Size(398, 87);
            label5.TabIndex = 167;
            label5.Text = "Printing Job";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            panel1.Controls.Add(label5);
            panel1.Controls.Add(flowLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1156, 198);
            panel1.TabIndex = 171;
            // 
            // panel2
            // 
            panel2.AutoScroll = true;
            panel2.Controls.Add(checkBox1);
            panel2.Controls.Add(picker_DilverDate);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(txt_JoborderSouv);
            panel2.Controls.Add(txt_SouvOption);
            panel2.Controls.Add(txt_Joborder);
            panel2.Controls.Add(txt_StmOption);
            panel2.Controls.Add(txt_DgRefer);
            panel2.Controls.Add(btn_Foundation);
            panel2.Controls.Add(btn_Printorder);
            panel2.Controls.Add(btn_Promtprinter);
            panel2.Controls.Add(btn_DGQty);
            panel2.Controls.Add(txt_salient);
            panel2.Controls.Add(label1);
            panel2.Controls.Add(cmb_Signature);
            panel2.Controls.Add(btn_DGCircular);
            panel2.Controls.Add(btn_Tags);
            panel2.Controls.Add(btn_Invoices);
            panel2.Controls.Add(btn_Distribution);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(cmb_Issue_No);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 198);
            panel2.Name = "panel2";
            panel2.Size = new Size(1156, 838);
            panel2.TabIndex = 172;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.ForeColor = Color.White;
            checkBox1.Location = new Point(300, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(106, 25);
            checkBox1.TabIndex = 191;
            checkBox1.Text = "Dilver Date";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // picker_DilverDate
            // 
            picker_DilverDate.Enabled = false;
            picker_DilverDate.Format = DateTimePickerFormat.Short;
            picker_DilverDate.Location = new Point(289, 38);
            picker_DilverDate.Name = "picker_DilverDate";
            picker_DilverDate.Size = new Size(126, 29);
            picker_DilverDate.TabIndex = 3;
            picker_DilverDate.Value = new DateTime(2026, 1, 1, 0, 0, 0, 0);
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 455);
            label3.Name = "label3";
            label3.Size = new Size(79, 21);
            label3.TabIndex = 189;
            label3.Text = "Issue No:-";
            // 
            // txt_JoborderSouv
            // 
            txt_JoborderSouv.Location = new Point(622, 89);
            txt_JoborderSouv.Name = "txt_JoborderSouv";
            txt_JoborderSouv.PlaceholderText = "Souvenir Joborder ";
            txt_JoborderSouv.Size = new Size(272, 29);
            txt_JoborderSouv.TabIndex = 7;
            txt_JoborderSouv.Tag = "souveControll";
            // 
            // txt_SouvOption
            // 
            txt_SouvOption.Location = new Point(185, 89);
            txt_SouvOption.Name = "txt_SouvOption";
            txt_SouvOption.PlaceholderText = "Souvenir Option";
            txt_SouvOption.Size = new Size(153, 29);
            txt_SouvOption.TabIndex = 5;
            txt_SouvOption.Tag = "souveControll";
            // 
            // txt_Joborder
            // 
            txt_Joborder.Location = new Point(344, 89);
            txt_Joborder.Name = "txt_Joborder";
            txt_Joborder.PlaceholderText = "Joborder Stamp";
            txt_Joborder.Size = new Size(272, 29);
            txt_Joborder.TabIndex = 6;
            // 
            // txt_StmOption
            // 
            txt_StmOption.Location = new Point(9, 89);
            txt_StmOption.Name = "txt_StmOption";
            txt_StmOption.PlaceholderText = "Stamp option";
            txt_StmOption.Size = new Size(170, 29);
            txt_StmOption.TabIndex = 4;
            // 
            // txt_DgRefer
            // 
            txt_DgRefer.Location = new Point(9, 144);
            txt_DgRefer.Name = "txt_DgRefer";
            txt_DgRefer.PlaceholderText = "DG office letter Refrence";
            txt_DgRefer.Size = new Size(885, 29);
            txt_DgRefer.TabIndex = 8;
            txt_DgRefer.Text = "Director General Pakistan Post Islamabad letter No./Email";
            // 
            // btn_Foundation
            // 
            btn_Foundation.Location = new Point(199, 390);
            btn_Foundation.Name = "btn_Foundation";
            btn_Foundation.Size = new Size(167, 62);
            btn_Foundation.TabIndex = 16;
            btn_Foundation.Text = "Foundation PrintOrder";
            btn_Foundation.UseVisualStyleBackColor = true;
            btn_Foundation.Click += btn_Foundation_Click;
            // 
            // btn_Printorder
            // 
            btn_Printorder.Location = new Point(199, 260);
            btn_Printorder.Name = "btn_Printorder";
            btn_Printorder.Size = new Size(167, 62);
            btn_Printorder.TabIndex = 12;
            btn_Printorder.Text = "NSPC PrintOrder";
            btn_Printorder.UseVisualStyleBackColor = true;
            btn_Printorder.Click += btn_Printorder_Click;
            // 
            // btn_Promtprinter
            // 
            btn_Promtprinter.Location = new Point(199, 325);
            btn_Promtprinter.Name = "btn_Promtprinter";
            btn_Promtprinter.Size = new Size(167, 62);
            btn_Promtprinter.TabIndex = 14;
            btn_Promtprinter.Text = "PromtPrinter PrintOrder";
            btn_Promtprinter.UseVisualStyleBackColor = true;
            btn_Promtprinter.Click += btn_Promtprinter_Click;
            // 
            // btn_DGQty
            // 
            btn_DGQty.Location = new Point(199, 195);
            btn_DGQty.Name = "btn_DGQty";
            btn_DGQty.Size = new Size(167, 62);
            btn_DGQty.TabIndex = 10;
            btn_DGQty.Text = "Print Quantity DG Circular";
            btn_DGQty.UseVisualStyleBackColor = true;
            btn_DGQty.Click += btn_DGQty_Click;
            // 
            // txt_salient
            // 
            txt_salient.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_salient.Location = new Point(12, 479);
            txt_salient.Multiline = true;
            txt_salient.Name = "txt_salient";
            txt_salient.ScrollBars = ScrollBars.Horizontal;
            txt_salient.Size = new Size(1114, 344);
            txt_salient.TabIndex = 17;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(168, 4);
            label1.Name = "label1";
            label1.Size = new Size(77, 21);
            label1.TabIndex = 188;
            label1.Text = "Signature";
            // 
            // cmb_Signature
            // 
            cmb_Signature.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_Signature.FormattingEnabled = true;
            cmb_Signature.Location = new Point(149, 38);
            cmb_Signature.Name = "cmb_Signature";
            cmb_Signature.Size = new Size(128, 29);
            cmb_Signature.TabIndex = 2;
            // 
            // btn_DGCircular
            // 
            btn_DGCircular.Location = new Point(12, 390);
            btn_DGCircular.Name = "btn_DGCircular";
            btn_DGCircular.Size = new Size(167, 62);
            btn_DGCircular.TabIndex = 15;
            btn_DGCircular.Text = "Print DG Circular";
            btn_DGCircular.UseVisualStyleBackColor = true;
            btn_DGCircular.Click += btn_DGCircular_Click;
            // 
            // btn_Tags
            // 
            btn_Tags.Location = new Point(12, 325);
            btn_Tags.Name = "btn_Tags";
            btn_Tags.Size = new Size(167, 62);
            btn_Tags.TabIndex = 13;
            btn_Tags.Text = "Print Tags";
            btn_Tags.UseVisualStyleBackColor = true;
            btn_Tags.Click += btn_Tags_Click;
            // 
            // btn_Invoices
            // 
            btn_Invoices.Location = new Point(12, 195);
            btn_Invoices.Name = "btn_Invoices";
            btn_Invoices.Size = new Size(167, 62);
            btn_Invoices.TabIndex = 9;
            btn_Invoices.Text = "Print Invoices";
            btn_Invoices.UseVisualStyleBackColor = true;
            btn_Invoices.Click += btn_Invoices_Click;
            // 
            // btn_Distribution
            // 
            btn_Distribution.Location = new Point(12, 260);
            btn_Distribution.Name = "btn_Distribution";
            btn_Distribution.Size = new Size(167, 62);
            btn_Distribution.TabIndex = 11;
            btn_Distribution.Text = "Print Distribution";
            btn_Distribution.UseVisualStyleBackColor = true;
            btn_Distribution.Click += btn_Distribution_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 4);
            label2.Name = "label2";
            label2.Size = new Size(79, 21);
            label2.TabIndex = 187;
            label2.Text = "Issue No:-";
            // 
            // cmb_Issue_No
            // 
            cmb_Issue_No.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_Issue_No.FormattingEnabled = true;
            cmb_Issue_No.Location = new Point(9, 38);
            cmb_Issue_No.Name = "cmb_Issue_No";
            cmb_Issue_No.Size = new Size(128, 29);
            cmb_Issue_No.TabIndex = 1;
            cmb_Issue_No.SelectedIndexChanged += cmb_Issue_No_SelectedIndexChanged;
            // 
            // IssuePrinting
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1156, 1036);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "IssuePrinting";
            Text = "SupplyDate";
            Load += IssuePrinting_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label5;
        private Panel panel1;
        private Panel panel2;
        private CheckBox checkBox1;
        private DateTimePicker picker_DilverDate;
        private Label label3;
        private TextBox txt_JoborderSouv;
        private TextBox txt_SouvOption;
        private TextBox txt_Joborder;
        private TextBox txt_StmOption;
        private TextBox txt_DgRefer;
        private Button btn_Foundation;
        private Button btn_Printorder;
        private Button btn_Promtprinter;
        private Button btn_DGQty;
        private TextBox txt_salient;
        private Label label1;
        private ComboBox cmb_Signature;
        private Button btn_DGCircular;
        private Button btn_Tags;
        private Button btn_Invoices;
        private Button btn_Distribution;
        private Label label2;
        private ComboBox cmb_Issue_No;
    }
}