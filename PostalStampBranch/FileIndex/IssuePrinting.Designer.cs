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
            label2 = new Label();
            cmb_Issue_No = new ComboBox();
            btn_Distribution = new Button();
            btn_Invoices = new Button();
            btn_Tags = new Button();
            btn_DGCircular = new Button();
            label1 = new Label();
            cmb_Signature = new ComboBox();
            txt_salient = new TextBox();
            btn_DGQty = new Button();
            btn_Foundation = new Button();
            btn_Printorder = new Button();
            btn_Promtprinter = new Button();
            txt_DgRefer = new TextBox();
            txt_StmOption = new TextBox();
            txt_Joborder = new TextBox();
            txt_SouvOption = new TextBox();
            txt_JoborderSouv = new TextBox();
            label3 = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label5 = new Label();
            picker_DilverDate = new DateTimePicker();
            checkBox1 = new CheckBox();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 245);
            label2.Name = "label2";
            label2.Size = new Size(79, 21);
            label2.TabIndex = 158;
            label2.Text = "Issue No:-";
            // 
            // cmb_Issue_No
            // 
            cmb_Issue_No.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_Issue_No.FormattingEnabled = true;
            cmb_Issue_No.Location = new Point(128, 242);
            cmb_Issue_No.Name = "cmb_Issue_No";
            cmb_Issue_No.Size = new Size(128, 29);
            cmb_Issue_No.TabIndex = 0;
            // 
            // btn_Distribution
            // 
            btn_Distribution.Location = new Point(27, 482);
            btn_Distribution.Name = "btn_Distribution";
            btn_Distribution.Size = new Size(167, 51);
            btn_Distribution.TabIndex = 8;
            btn_Distribution.Text = "Print Distribution";
            btn_Distribution.UseVisualStyleBackColor = true;
            btn_Distribution.Click += btn_Distribution_Click;
            // 
            // btn_Invoices
            // 
            btn_Invoices.Location = new Point(27, 414);
            btn_Invoices.Name = "btn_Invoices";
            btn_Invoices.Size = new Size(167, 51);
            btn_Invoices.TabIndex = 7;
            btn_Invoices.Text = "Print Invoices";
            btn_Invoices.UseVisualStyleBackColor = true;
            btn_Invoices.Click += btn_Invoices_Click;
            // 
            // btn_Tags
            // 
            btn_Tags.Location = new Point(27, 561);
            btn_Tags.Name = "btn_Tags";
            btn_Tags.Size = new Size(167, 51);
            btn_Tags.TabIndex = 9;
            btn_Tags.Text = "Print Tags";
            btn_Tags.UseVisualStyleBackColor = true;
            btn_Tags.Click += btn_Tags_Click;
            // 
            // btn_DGCircular
            // 
            btn_DGCircular.Location = new Point(212, 414);
            btn_DGCircular.Name = "btn_DGCircular";
            btn_DGCircular.Size = new Size(167, 51);
            btn_DGCircular.TabIndex = 10;
            btn_DGCircular.Text = "Print DG Circular";
            btn_DGCircular.UseVisualStyleBackColor = true;
            btn_DGCircular.Click += btn_DGCircular_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(267, 245);
            label1.Name = "label1";
            label1.Size = new Size(79, 21);
            label1.TabIndex = 164;
            label1.Text = "Issue No:-";
            // 
            // cmb_Signature
            // 
            cmb_Signature.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb_Signature.FormattingEnabled = true;
            cmb_Signature.Location = new Point(361, 242);
            cmb_Signature.Name = "cmb_Signature";
            cmb_Signature.Size = new Size(128, 29);
            cmb_Signature.TabIndex = 1;
            // 
            // txt_salient
            // 
            txt_salient.Font = new Font("Courier New", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            txt_salient.Location = new Point(27, 669);
            txt_salient.Multiline = true;
            txt_salient.Name = "txt_salient";
            txt_salient.ScrollBars = ScrollBars.Horizontal;
            txt_salient.Size = new Size(1108, 344);
            txt_salient.TabIndex = 15;
            // 
            // btn_DGQty
            // 
            btn_DGQty.Location = new Point(212, 482);
            btn_DGQty.Name = "btn_DGQty";
            btn_DGQty.Size = new Size(167, 51);
            btn_DGQty.TabIndex = 11;
            btn_DGQty.Text = "Print Quantity DG Circular";
            btn_DGQty.UseVisualStyleBackColor = true;
            btn_DGQty.Click += btn_DGQty_Click;
            // 
            // btn_Foundation
            // 
            btn_Foundation.Location = new Point(395, 550);
            btn_Foundation.Name = "btn_Foundation";
            btn_Foundation.Size = new Size(167, 62);
            btn_Foundation.TabIndex = 14;
            btn_Foundation.Text = "Foundation PrintOrder";
            btn_Foundation.UseVisualStyleBackColor = true;
            btn_Foundation.Click += btn_Foundation_Click;
            // 
            // btn_Printorder
            // 
            btn_Printorder.Location = new Point(395, 414);
            btn_Printorder.Name = "btn_Printorder";
            btn_Printorder.Size = new Size(167, 51);
            btn_Printorder.TabIndex = 12;
            btn_Printorder.Text = "NSPC PrintOrder";
            btn_Printorder.UseVisualStyleBackColor = true;
            btn_Printorder.Click += btn_Printorder_Click;
            // 
            // btn_Promtprinter
            // 
            btn_Promtprinter.Location = new Point(395, 482);
            btn_Promtprinter.Name = "btn_Promtprinter";
            btn_Promtprinter.Size = new Size(167, 51);
            btn_Promtprinter.TabIndex = 13;
            btn_Promtprinter.Text = "PromtPrinter PrintOrder";
            btn_Promtprinter.UseVisualStyleBackColor = true;
            btn_Promtprinter.Click += btn_Promtprinter_Click;
            // 
            // txt_DgRefer
            // 
            txt_DgRefer.Location = new Point(27, 345);
            txt_DgRefer.Name = "txt_DgRefer";
            txt_DgRefer.PlaceholderText = "DG office letter Refrence";
            txt_DgRefer.Size = new Size(885, 29);
            txt_DgRefer.TabIndex = 2;
            txt_DgRefer.Text = "Director General Pakistan Post Islamabad letter No./Email";
            // 
            // txt_StmOption
            // 
            txt_StmOption.Location = new Point(27, 290);
            txt_StmOption.Name = "txt_StmOption";
            txt_StmOption.PlaceholderText = "Stamp option";
            txt_StmOption.Size = new Size(170, 29);
            txt_StmOption.TabIndex = 3;
            // 
            // txt_Joborder
            // 
            txt_Joborder.Location = new Point(362, 290);
            txt_Joborder.Name = "txt_Joborder";
            txt_Joborder.PlaceholderText = "Joborder Stamp";
            txt_Joborder.Size = new Size(272, 29);
            txt_Joborder.TabIndex = 5;
            // 
            // txt_SouvOption
            // 
            txt_SouvOption.Location = new Point(203, 290);
            txt_SouvOption.Name = "txt_SouvOption";
            txt_SouvOption.PlaceholderText = "Souvenir Option";
            txt_SouvOption.Size = new Size(153, 29);
            txt_SouvOption.TabIndex = 4;
            // 
            // txt_JoborderSouv
            // 
            txt_JoborderSouv.Location = new Point(640, 290);
            txt_JoborderSouv.Name = "txt_JoborderSouv";
            txt_JoborderSouv.PlaceholderText = "Souvenir Joborder ";
            txt_JoborderSouv.Size = new Size(272, 29);
            txt_JoborderSouv.TabIndex = 6;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(27, 645);
            label3.Name = "label3";
            label3.Size = new Size(79, 21);
            label3.TabIndex = 165;
            label3.Text = "Issue No:-";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(34, 167, 240);
            flowLayoutPanel1.Location = new Point(335, 115);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(398, 3);
            flowLayoutPanel1.TabIndex = 166;
            // 
            // label5
            // 
            label5.BackColor = Color.Transparent;
            label5.Font = new Font("Segoe UI", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.ForeColor = Color.FromArgb(0, 165, 255);
            label5.Location = new Point(335, 9);
            label5.Name = "label5";
            label5.Size = new Size(398, 87);
            label5.TabIndex = 167;
            label5.Text = "Printing Job";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picker_DilverDate
            // 
            picker_DilverDate.Enabled = false;
            picker_DilverDate.Format = DateTimePickerFormat.Short;
            picker_DilverDate.Location = new Point(506, 239);
            picker_DilverDate.Name = "picker_DilverDate";
            picker_DilverDate.Size = new Size(191, 29);
            picker_DilverDate.TabIndex = 169;
            picker_DilverDate.Value = new DateTime(2026, 1, 1, 0, 0, 0, 0);
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(506, 208);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(102, 25);
            checkBox1.TabIndex = 170;
            checkBox1.Text = "DilverDate";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // IssuePrinting
            // 
            AutoScaleDimensions = new SizeF(9F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1525, 1060);
            Controls.Add(checkBox1);
            Controls.Add(picker_DilverDate);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(label5);
            Controls.Add(label3);
            Controls.Add(txt_JoborderSouv);
            Controls.Add(txt_SouvOption);
            Controls.Add(txt_Joborder);
            Controls.Add(txt_StmOption);
            Controls.Add(txt_DgRefer);
            Controls.Add(btn_Foundation);
            Controls.Add(btn_Printorder);
            Controls.Add(btn_Promtprinter);
            Controls.Add(btn_DGQty);
            Controls.Add(txt_salient);
            Controls.Add(label1);
            Controls.Add(cmb_Signature);
            Controls.Add(btn_DGCircular);
            Controls.Add(btn_Tags);
            Controls.Add(btn_Invoices);
            Controls.Add(btn_Distribution);
            Controls.Add(label2);
            Controls.Add(cmb_Issue_No);
            Name = "IssuePrinting";
            Text = "SupplyDate";
            Load += IssuePrinting_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label2;
        private ComboBox cmb_Issue_No;
        private Button btn_Distribution;
        private Button btn_Invoices;
        private Button btn_Tags;
        private Button btn_DGCircular;
        private Label label1;
        private ComboBox cmb_Signature;
        private TextBox txt_salient;
        private Button btn_DGQty;
        private Button btn_Foundation;
        private Button btn_Printorder;
        private Button btn_Promtprinter;
        private TextBox txt_DgRefer;
        private TextBox txt_StmOption;
        private TextBox txt_Joborder;
        private TextBox txt_SouvOption;
        private TextBox txt_JoborderSouv;
        private Label label3;
        private FlowLayoutPanel flowLayoutPanel1;
        private Label label5;
        private DateTimePicker picker_DilverDate;
        private CheckBox checkBox1;
    }
}