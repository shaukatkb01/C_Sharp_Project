namespace SupplyBranch.Forms.Transactions
{
    partial class frmSaleReturn
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlModeBar = new System.Windows.Forms.Panel();
            this.btnModeEntry = new System.Windows.Forms.Button();
            this.btnModeList = new System.Windows.Forms.Button();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.pnlCurrentStock = new System.Windows.Forms.Panel();
            this.pnlStamp = new System.Windows.Forms.Panel();
            this.lblStampBalance = new System.Windows.Forms.Label();
            this.lblStampTitle = new System.Windows.Forms.Label();
            this.pnlSheet = new System.Windows.Forms.Panel();
            this.lblSheetBalance = new System.Windows.Forms.Label();
            this.lblSheetTitle = new System.Windows.Forms.Label();
            this.pnlPacket = new System.Windows.Forms.Panel();
            this.lblPacketBalance = new System.Windows.Forms.Label();
            this.lblPacketTitle = new System.Windows.Forms.Label();
            this.pnlBox = new System.Windows.Forms.Panel();
            this.lblBoxBalance = new System.Windows.Forms.Label();
            this.lblBoxTitle = new System.Windows.Forms.Label();
            this.lblStockItem = new System.Windows.Forms.Label();
            this.lblStockTitle = new System.Windows.Forms.Label();
            this.pnlStockEntry = new System.Windows.Forms.Panel();
            this.fpnlButton = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSaveDraft = new System.Windows.Forms.Button();
            this.btnPostReturn = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnSearchReturns = new System.Windows.Forms.Button();
            this.btnOpenReturn = new System.Windows.Forms.Button();
            this.btnPrintReturn = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.labelRemarks = new System.Windows.Forms.Label();
            this.txtSupplyDate = new System.Windows.Forms.TextBox();
            this.labelSupplyDate = new System.Windows.Forms.Label();
            this.txtInvoiceNo = new System.Windows.Forms.TextBox();
            this.labelInvoiceNo = new System.Windows.Forms.Label();
            this.txtOfficeName = new System.Windows.Forms.TextBox();
            this.labelOffice = new System.Windows.Forms.Label();
            this.cmbIssuedSupply = new SupplyBranch.Controls.ModernComboBox();
            this.labelIssuedSupply = new System.Windows.Forms.Label();
            this.txtReturnStatus = new System.Windows.Forms.TextBox();
            this.labelStatus = new System.Windows.Forms.Label();
            this.txtFinancialYear = new System.Windows.Forms.TextBox();
            this.labelFY = new System.Windows.Forms.Label();
            this.dtReturnDate = new System.Windows.Forms.DateTimePicker();
            this.labelReturnDate = new System.Windows.Forms.Label();
            this.txtReturnNo = new System.Windows.Forms.TextBox();
            this.labelReturnNo = new System.Windows.Forms.Label();
            this.labelSearchReturnNo = new System.Windows.Forms.Label();
            this.txtSearchReturnNo = new System.Windows.Forms.TextBox();
            this.labelFilterStatus = new System.Windows.Forms.Label();
            this.cmbFilterStatus = new SupplyBranch.Controls.ModernComboBox();
            this.labelFilterFrom = new System.Windows.Forms.Label();
            this.dtFilterFrom = new System.Windows.Forms.DateTimePicker();
            this.labelFilterTo = new System.Windows.Forms.Label();
            this.dtFilterTo = new System.Windows.Forms.DateTimePicker();
            this.dgvReturnDetail = new System.Windows.Forms.DataGridView();
            this.pnlModeBar.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.pnlCurrentStock.SuspendLayout();
            this.pnlStamp.SuspendLayout();
            this.pnlSheet.SuspendLayout();
            this.pnlPacket.SuspendLayout();
            this.pnlBox.SuspendLayout();
            this.fpnlButton.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnDetail)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlModeBar
            // 
            this.pnlModeBar.Controls.Add(this.btnModeEntry);
            this.pnlModeBar.Controls.Add(this.btnModeList);
            this.pnlModeBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlModeBar.Location = new System.Drawing.Point(0, 0);
            this.pnlModeBar.Name = "pnlModeBar";
            this.pnlModeBar.Padding = new System.Windows.Forms.Padding(12, 6, 12, 6);
            this.pnlModeBar.Size = new System.Drawing.Size(1400, 44);
            this.pnlModeBar.TabIndex = 2;
            // 
            // btnModeEntry
            // 
            this.btnModeEntry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModeEntry.Location = new System.Drawing.Point(14, 7);
            this.btnModeEntry.Name = "btnModeEntry";
            this.btnModeEntry.Size = new System.Drawing.Size(165, 30);
            this.btnModeEntry.Text = "Sale Return Entry";
            this.btnModeEntry.Click += new System.EventHandler(this.btnModeEntry_Click);
            // 
            // btnModeList
            // 
            this.btnModeList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModeList.Location = new System.Drawing.Point(186, 7);
            this.btnModeList.Name = "btnModeList";
            this.btnModeList.Size = new System.Drawing.Size(165, 30);
            this.btnModeList.Text = "Sale Return List";
            this.btnModeList.Click += new System.EventHandler(this.btnModeList_Click);
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.pnlCurrentStock);
            this.pnlTop.Controls.Add(this.pnlStockEntry);
            this.pnlTop.Controls.Add(this.fpnlButton);
            this.pnlTop.Controls.Add(this.txtRemarks);
            this.pnlTop.Controls.Add(this.labelRemarks);
            this.pnlTop.Controls.Add(this.txtSupplyDate);
            this.pnlTop.Controls.Add(this.labelSupplyDate);
            this.pnlTop.Controls.Add(this.txtInvoiceNo);
            this.pnlTop.Controls.Add(this.labelInvoiceNo);
            this.pnlTop.Controls.Add(this.txtOfficeName);
            this.pnlTop.Controls.Add(this.labelOffice);
            this.pnlTop.Controls.Add(this.cmbIssuedSupply);
            this.pnlTop.Controls.Add(this.labelIssuedSupply);
            this.pnlTop.Controls.Add(this.txtReturnStatus);
            this.pnlTop.Controls.Add(this.labelStatus);
            this.pnlTop.Controls.Add(this.txtFinancialYear);
            this.pnlTop.Controls.Add(this.labelFY);
            this.pnlTop.Controls.Add(this.dtReturnDate);
            this.pnlTop.Controls.Add(this.labelReturnDate);
            this.pnlTop.Controls.Add(this.txtReturnNo);
            this.pnlTop.Controls.Add(this.labelReturnNo);
            this.pnlTop.Controls.Add(this.labelSearchReturnNo);
            this.pnlTop.Controls.Add(this.txtSearchReturnNo);
            this.pnlTop.Controls.Add(this.labelFilterStatus);
            this.pnlTop.Controls.Add(this.cmbFilterStatus);
            this.pnlTop.Controls.Add(this.labelFilterFrom);
            this.pnlTop.Controls.Add(this.dtFilterFrom);
            this.pnlTop.Controls.Add(this.labelFilterTo);
            this.pnlTop.Controls.Add(this.dtFilterTo);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 44);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1400, 255);
            this.pnlTop.TabIndex = 0;
            // 
            // Entry Controls
            // 
            this.labelReturnNo.AutoSize = true;
            this.labelReturnNo.Location = new System.Drawing.Point(14, 16);
            this.labelReturnNo.Name = "labelReturnNo";
            this.labelReturnNo.Size = new System.Drawing.Size(58, 13);
            this.labelReturnNo.Text = "Return No";

            this.txtReturnNo.Location = new System.Drawing.Point(115, 13);
            this.txtReturnNo.Name = "txtReturnNo";
            this.txtReturnNo.Size = new System.Drawing.Size(210, 20);

            this.labelReturnDate.AutoSize = true;
            this.labelReturnDate.Location = new System.Drawing.Point(14, 48);
            this.labelReturnDate.Name = "labelReturnDate";
            this.labelReturnDate.Size = new System.Drawing.Size(67, 13);
            this.labelReturnDate.Text = "Return Date";

            this.dtReturnDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtReturnDate.Location = new System.Drawing.Point(115, 45);
            this.dtReturnDate.Name = "dtReturnDate";
            this.dtReturnDate.Size = new System.Drawing.Size(210, 20);

            this.labelFY.AutoSize = true;
            this.labelFY.Location = new System.Drawing.Point(14, 80);
            this.labelFY.Name = "labelFY";
            this.labelFY.Size = new System.Drawing.Size(74, 13);
            this.labelFY.Text = "Financial Year";

            this.txtFinancialYear.Location = new System.Drawing.Point(115, 77);
            this.txtFinancialYear.Name = "txtFinancialYear";
            this.txtFinancialYear.Size = new System.Drawing.Size(210, 20);

            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(14, 112);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(37, 13);
            this.labelStatus.Text = "Status";

            this.txtReturnStatus.Location = new System.Drawing.Point(115, 109);
            this.txtReturnStatus.Name = "txtReturnStatus";
            this.txtReturnStatus.Size = new System.Drawing.Size(210, 20);

            this.labelIssuedSupply.AutoSize = true;
            this.labelIssuedSupply.Location = new System.Drawing.Point(350, 16);
            this.labelIssuedSupply.Name = "labelIssuedSupply";
            this.labelIssuedSupply.Size = new System.Drawing.Size(77, 13);
            this.labelIssuedSupply.Text = "Issued Supply";

            this.cmbIssuedSupply.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbIssuedSupply.FormattingEnabled = true;
            this.cmbIssuedSupply.Location = new System.Drawing.Point(440, 13);
            this.cmbIssuedSupply.Name = "cmbIssuedSupply";
            this.cmbIssuedSupply.Size = new System.Drawing.Size(230, 21);
            this.cmbIssuedSupply.SelectedIndexChanged += new System.EventHandler(this.cmbIssuedSupply_SelectedIndexChanged);

            this.labelOffice.AutoSize = true;
            this.labelOffice.Location = new System.Drawing.Point(350, 48);
            this.labelOffice.Name = "labelOffice";
            this.labelOffice.Size = new System.Drawing.Size(35, 13);
            this.labelOffice.Text = "Office";

            this.txtOfficeName.Location = new System.Drawing.Point(440, 45);
            this.txtOfficeName.Name = "txtOfficeName";
            this.txtOfficeName.Size = new System.Drawing.Size(230, 20);

            this.labelInvoiceNo.AutoSize = true;
            this.labelInvoiceNo.Location = new System.Drawing.Point(350, 80);
            this.labelInvoiceNo.Name = "labelInvoiceNo";
            this.labelInvoiceNo.Size = new System.Drawing.Size(62, 13);
            this.labelInvoiceNo.Text = "Invoice No.";

            this.txtInvoiceNo.Location = new System.Drawing.Point(440, 77);
            this.txtInvoiceNo.Name = "txtInvoiceNo";
            this.txtInvoiceNo.Size = new System.Drawing.Size(230, 20);

            this.labelSupplyDate.AutoSize = true;
            this.labelSupplyDate.Location = new System.Drawing.Point(350, 112);
            this.labelSupplyDate.Name = "labelSupplyDate";
            this.labelSupplyDate.Size = new System.Drawing.Size(65, 13);
            this.labelSupplyDate.Text = "Supply Date";

            this.txtSupplyDate.Location = new System.Drawing.Point(440, 109);
            this.txtSupplyDate.Name = "txtSupplyDate";
            this.txtSupplyDate.Size = new System.Drawing.Size(230, 20);

            this.labelRemarks.AutoSize = true;
            this.labelRemarks.Location = new System.Drawing.Point(14, 144);
            this.labelRemarks.Name = "labelRemarks";
            this.labelRemarks.Size = new System.Drawing.Size(49, 13);
            this.labelRemarks.Text = "Remarks";

            this.txtRemarks.Location = new System.Drawing.Point(115, 141);
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.Size = new System.Drawing.Size(555, 20);
            // 
            // List Filter Controls
            // 
            this.labelSearchReturnNo.AutoSize = true;
            this.labelSearchReturnNo.Location = new System.Drawing.Point(14, 16);
            this.labelSearchReturnNo.Name = "labelSearchReturnNo";
            this.labelSearchReturnNo.Size = new System.Drawing.Size(58, 13);
            this.labelSearchReturnNo.Text = "Return No";
            this.labelSearchReturnNo.Visible = false;

            this.txtSearchReturnNo.Location = new System.Drawing.Point(115, 13);
            this.txtSearchReturnNo.Name = "txtSearchReturnNo";
            this.txtSearchReturnNo.Size = new System.Drawing.Size(210, 20);
            this.txtSearchReturnNo.Visible = false;

            this.labelFilterStatus.AutoSize = true;
            this.labelFilterStatus.Location = new System.Drawing.Point(14, 48);
            this.labelFilterStatus.Name = "labelFilterStatus";
            this.labelFilterStatus.Size = new System.Drawing.Size(37, 13);
            this.labelFilterStatus.Text = "Status";
            this.labelFilterStatus.Visible = false;

            this.cmbFilterStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilterStatus.FormattingEnabled = true;
            this.cmbFilterStatus.Location = new System.Drawing.Point(115, 45);
            this.cmbFilterStatus.Name = "cmbFilterStatus";
            this.cmbFilterStatus.Size = new System.Drawing.Size(210, 21);
            this.cmbFilterStatus.Visible = false;

            this.labelFilterFrom.AutoSize = true;
            this.labelFilterFrom.Location = new System.Drawing.Point(350, 16);
            this.labelFilterFrom.Name = "labelFilterFrom";
            this.labelFilterFrom.Size = new System.Drawing.Size(62, 13);
            this.labelFilterFrom.Text = "From Date";
            this.labelFilterFrom.Visible = false;

            this.dtFilterFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFilterFrom.Location = new System.Drawing.Point(440, 13);
            this.dtFilterFrom.Name = "dtFilterFrom";
            this.dtFilterFrom.Size = new System.Drawing.Size(230, 20);
            this.dtFilterFrom.Visible = false;

            this.labelFilterTo.AutoSize = true;
            this.labelFilterTo.Location = new System.Drawing.Point(350, 48);
            this.labelFilterTo.Name = "labelFilterTo";
            this.labelFilterTo.Size = new System.Drawing.Size(52, 13);
            this.labelFilterTo.Text = "To Date";
            this.labelFilterTo.Visible = false;

            this.dtFilterTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFilterTo.Location = new System.Drawing.Point(440, 45);
            this.dtFilterTo.Name = "dtFilterTo";
            this.dtFilterTo.Size = new System.Drawing.Size(230, 20);
            this.dtFilterTo.Visible = false;
            // 
            // fpnlButton
            // 
            this.fpnlButton.Controls.Add(this.btnSaveDraft);
            this.fpnlButton.Controls.Add(this.btnPostReturn);
            this.fpnlButton.Controls.Add(this.btnRefresh);
            this.fpnlButton.Controls.Add(this.btnSearchReturns);
            this.fpnlButton.Controls.Add(this.btnOpenReturn);
            this.fpnlButton.Controls.Add(this.btnPrintReturn);
            this.fpnlButton.Controls.Add(this.btnClose);
            this.fpnlButton.Location = new System.Drawing.Point(14, 180);
            this.fpnlButton.Name = "fpnlButton";
            this.fpnlButton.Size = new System.Drawing.Size(660, 48);

            this.btnSaveDraft.Location = new System.Drawing.Point(3, 4);
            this.btnSaveDraft.Name = "btnSaveDraft";
            this.btnSaveDraft.Size = new System.Drawing.Size(130, 36);
            this.btnSaveDraft.Text = "Save Draft";
            this.btnSaveDraft.Click += new System.EventHandler(this.btnSaveDraft_Click);

            this.btnPostReturn.Location = new System.Drawing.Point(139, 4);
            this.btnPostReturn.Name = "btnPostReturn";
            this.btnPostReturn.Size = new System.Drawing.Size(150, 36);
            this.btnPostReturn.Text = "Post Return (Stock IN)";
            this.btnPostReturn.Click += new System.EventHandler(this.btnPostReturn_Click);

            this.btnRefresh.Location = new System.Drawing.Point(295, 4);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(105, 36);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);

            this.btnSearchReturns.Location = new System.Drawing.Point(3, 4);
            this.btnSearchReturns.Name = "btnSearchReturns";
            this.btnSearchReturns.Size = new System.Drawing.Size(115, 36);
            this.btnSearchReturns.Text = "Search";
            this.btnSearchReturns.Visible = false;
            this.btnSearchReturns.Click += new System.EventHandler(this.btnSearchReturns_Click);

            this.btnOpenReturn.Location = new System.Drawing.Point(124, 4);
            this.btnOpenReturn.Name = "btnOpenReturn";
            this.btnOpenReturn.Size = new System.Drawing.Size(120, 36);
            this.btnOpenReturn.Text = "Open / View";
            this.btnOpenReturn.Visible = false;
            this.btnOpenReturn.Click += new System.EventHandler(this.btnOpenReturn_Click);

            this.btnPrintReturn.Location = new System.Drawing.Point(250, 4);
            this.btnPrintReturn.Name = "btnPrintReturn";
            this.btnPrintReturn.Size = new System.Drawing.Size(135, 36);
            this.btnPrintReturn.Text = "Print Return";
            this.btnPrintReturn.Visible = false;
            this.btnPrintReturn.Click += new System.EventHandler(this.btnPrintReturn_Click);

            this.btnClose.Location = new System.Drawing.Point(406, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(110, 36);
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pnlStockEntry
            // 
            this.pnlStockEntry.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStockEntry.Location = new System.Drawing.Point(690, 12);
            this.pnlStockEntry.Name = "pnlStockEntry";
            this.pnlStockEntry.Size = new System.Drawing.Size(390, 225);
            // 
            // pnlCurrentStock
            // 
            this.pnlCurrentStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlCurrentStock.Controls.Add(this.pnlStamp);
            this.pnlCurrentStock.Controls.Add(this.pnlSheet);
            this.pnlCurrentStock.Controls.Add(this.pnlPacket);
            this.pnlCurrentStock.Controls.Add(this.pnlBox);
            this.pnlCurrentStock.Controls.Add(this.lblStockItem);
            this.pnlCurrentStock.Controls.Add(this.lblStockTitle);
            this.pnlCurrentStock.Location = new System.Drawing.Point(1092, 12);
            this.pnlCurrentStock.Name = "pnlCurrentStock";
            this.pnlCurrentStock.Size = new System.Drawing.Size(292, 225);

            this.lblStockTitle.AutoSize = true;
            this.lblStockTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.lblStockTitle.Location = new System.Drawing.Point(12, 10);
            this.lblStockTitle.Name = "lblStockTitle";
            this.lblStockTitle.Size = new System.Drawing.Size(138, 17);
            this.lblStockTitle.Text = "CURRENT STOCK";

            this.lblStockItem.AutoSize = true;
            this.lblStockItem.Location = new System.Drawing.Point(12, 34);
            this.lblStockItem.Name = "lblStockItem";
            this.lblStockItem.Size = new System.Drawing.Size(113, 13);
            this.lblStockItem.Text = "Select an item to view";

            this.pnlBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlBox.Controls.Add(this.lblBoxBalance);
            this.pnlBox.Controls.Add(this.lblBoxTitle);
            this.pnlBox.Location = new System.Drawing.Point(12, 65);
            this.pnlBox.Name = "pnlBox";
            this.pnlBox.Size = new System.Drawing.Size(60, 65);

            this.lblBoxTitle.AutoSize = true;
            this.lblBoxTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold);
            this.lblBoxTitle.Location = new System.Drawing.Point(14, 8);
            this.lblBoxTitle.Name = "lblBoxTitle";
            this.lblBoxTitle.Size = new System.Drawing.Size(28, 13);
            this.lblBoxTitle.Text = "BOX";

            this.lblBoxBalance.AutoSize = true;
            this.lblBoxBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblBoxBalance.Location = new System.Drawing.Point(20, 30);
            this.lblBoxBalance.Name = "lblBoxBalance";
            this.lblBoxBalance.Size = new System.Drawing.Size(15, 15);
            this.lblBoxBalance.Text = "0";

            this.pnlPacket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlPacket.Controls.Add(this.lblPacketBalance);
            this.pnlPacket.Controls.Add(this.lblPacketTitle);
            this.pnlPacket.Location = new System.Drawing.Point(80, 65);
            this.pnlPacket.Name = "pnlPacket";
            this.pnlPacket.Size = new System.Drawing.Size(62, 65);

            this.lblPacketTitle.AutoSize = true;
            this.lblPacketTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold);
            this.lblPacketTitle.Location = new System.Drawing.Point(8, 8);
            this.lblPacketTitle.Name = "lblPacketTitle";
            this.lblPacketTitle.Size = new System.Drawing.Size(44, 13);
            this.lblPacketTitle.Text = "PACKET";

            this.lblPacketBalance.AutoSize = true;
            this.lblPacketBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblPacketBalance.Location = new System.Drawing.Point(22, 30);
            this.lblPacketBalance.Name = "lblPacketBalance";
            this.lblPacketBalance.Size = new System.Drawing.Size(15, 15);
            this.lblPacketBalance.Text = "0";

            this.pnlSheet.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSheet.Controls.Add(this.lblSheetBalance);
            this.pnlSheet.Controls.Add(this.lblSheetTitle);
            this.pnlSheet.Location = new System.Drawing.Point(150, 65);
            this.pnlSheet.Name = "pnlSheet";
            this.pnlSheet.Size = new System.Drawing.Size(60, 65);

            this.lblSheetTitle.AutoSize = true;
            this.lblSheetTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold);
            this.lblSheetTitle.Location = new System.Drawing.Point(10, 8);
            this.lblSheetTitle.Name = "lblSheetTitle";
            this.lblSheetTitle.Size = new System.Drawing.Size(37, 13);
            this.lblSheetTitle.Text = "SHEET";

            this.lblSheetBalance.AutoSize = true;
            this.lblSheetBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblSheetBalance.Location = new System.Drawing.Point(20, 30);
            this.lblSheetBalance.Name = "lblSheetBalance";
            this.lblSheetBalance.Size = new System.Drawing.Size(15, 15);
            this.lblSheetBalance.Text = "0";

            this.pnlStamp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlStamp.Controls.Add(this.lblStampBalance);
            this.pnlStamp.Controls.Add(this.lblStampTitle);
            this.pnlStamp.Location = new System.Drawing.Point(218, 65);
            this.pnlStamp.Name = "pnlStamp";
            this.pnlStamp.Size = new System.Drawing.Size(60, 65);

            this.lblStampTitle.AutoSize = true;
            this.lblStampTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Bold);
            this.lblStampTitle.Location = new System.Drawing.Point(9, 8);
            this.lblStampTitle.Name = "lblStampTitle";
            this.lblStampTitle.Size = new System.Drawing.Size(40, 13);
            this.lblStampTitle.Text = "STAMP";

            this.lblStampBalance.AutoSize = true;
            this.lblStampBalance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblStampBalance.Location = new System.Drawing.Point(20, 30);
            this.lblStampBalance.Name = "lblStampBalance";
            this.lblStampBalance.Size = new System.Drawing.Size(15, 15);
            this.lblStampBalance.Text = "0";
            // 
            // dgvReturnDetail
            // 
            this.dgvReturnDetail.AllowUserToAddRows = false;
            this.dgvReturnDetail.AllowUserToDeleteRows = false;
            this.dgvReturnDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReturnDetail.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvReturnDetail.Location = new System.Drawing.Point(0, 299);
            this.dgvReturnDetail.MultiSelect = false;
            this.dgvReturnDetail.Name = "dgvReturnDetail";
            this.dgvReturnDetail.RowHeadersVisible = false;
            this.dgvReturnDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvReturnDetail.Size = new System.Drawing.Size(1400, 421);
            this.dgvReturnDetail.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReturnDetail_CellDoubleClick);
            this.dgvReturnDetail.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvReturnDetail_CellEndEdit);
            this.dgvReturnDetail.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvReturnDetail_CellFormatting);
            this.dgvReturnDetail.CurrentCellChanged += new System.EventHandler(this.dgvReturnDetail_CurrentCellChanged);
            this.dgvReturnDetail.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvReturnDetail_EditingControlShowing);
            // 
            // frmSaleReturn
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1400, 720);
            this.Controls.Add(this.dgvReturnDetail);
            this.Controls.Add(this.pnlTop);
            this.Controls.Add(this.pnlModeBar);
            this.Name = "frmSaleReturn";
            this.Text = "Sale Return (Stock IN)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmSaleReturn_Load);
            this.pnlModeBar.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlCurrentStock.ResumeLayout(false);
            this.pnlCurrentStock.PerformLayout();
            this.pnlStamp.ResumeLayout(false);
            this.pnlStamp.PerformLayout();
            this.pnlSheet.ResumeLayout(false);
            this.pnlSheet.PerformLayout();
            this.pnlPacket.ResumeLayout(false);
            this.pnlPacket.PerformLayout();
            this.pnlBox.ResumeLayout(false);
            this.pnlBox.PerformLayout();
            this.fpnlButton.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvReturnDetail)).EndInit();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel pnlModeBar;
        private System.Windows.Forms.Button btnModeEntry;
        private System.Windows.Forms.Button btnModeList;
        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Panel pnlCurrentStock;
        private System.Windows.Forms.Panel pnlStamp;
        private System.Windows.Forms.Label lblStampBalance;
        private System.Windows.Forms.Label lblStampTitle;
        private System.Windows.Forms.Panel pnlSheet;
        private System.Windows.Forms.Label lblSheetBalance;
        private System.Windows.Forms.Label lblSheetTitle;
        private System.Windows.Forms.Panel pnlPacket;
        private System.Windows.Forms.Label lblPacketBalance;
        private System.Windows.Forms.Label lblPacketTitle;
        private System.Windows.Forms.Panel pnlBox;
        private System.Windows.Forms.Label lblBoxBalance;
        private System.Windows.Forms.Label lblBoxTitle;
        private System.Windows.Forms.Label lblStockItem;
        private System.Windows.Forms.Label lblStockTitle;
        private System.Windows.Forms.Panel pnlStockEntry;
        private System.Windows.Forms.FlowLayoutPanel fpnlButton;
        private System.Windows.Forms.Button btnSaveDraft;
        private System.Windows.Forms.Button btnPostReturn;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button btnSearchReturns;
        private System.Windows.Forms.Button btnOpenReturn;
        private System.Windows.Forms.Button btnPrintReturn;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label labelRemarks;
        private System.Windows.Forms.TextBox txtSupplyDate;
        private System.Windows.Forms.Label labelSupplyDate;
        private System.Windows.Forms.TextBox txtInvoiceNo;
        private System.Windows.Forms.Label labelInvoiceNo;
        private System.Windows.Forms.TextBox txtOfficeName;
        private System.Windows.Forms.Label labelOffice;
        private System.Windows.Forms.ComboBox cmbIssuedSupply;
        private System.Windows.Forms.Label labelIssuedSupply;
        private System.Windows.Forms.TextBox txtReturnStatus;
        private System.Windows.Forms.Label labelStatus;
        private System.Windows.Forms.TextBox txtFinancialYear;
        private System.Windows.Forms.Label labelFY;
        private System.Windows.Forms.DateTimePicker dtReturnDate;
        private System.Windows.Forms.Label labelReturnDate;
        private System.Windows.Forms.TextBox txtReturnNo;
        private System.Windows.Forms.Label labelReturnNo;
        private System.Windows.Forms.Label labelSearchReturnNo;
        private System.Windows.Forms.TextBox txtSearchReturnNo;
        private System.Windows.Forms.Label labelFilterStatus;
        private System.Windows.Forms.ComboBox cmbFilterStatus;
        private System.Windows.Forms.Label labelFilterFrom;
        private System.Windows.Forms.DateTimePicker dtFilterFrom;
        private System.Windows.Forms.Label labelFilterTo;
        private System.Windows.Forms.DateTimePicker dtFilterTo;
        private System.Windows.Forms.DataGridView dgvReturnDetail;
    }
}