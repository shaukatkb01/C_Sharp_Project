using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SupplyBranch.DAL;
using SupplyBranch.Helpers;

namespace SupplyBranch.Forms.Stock
{
    public partial class frmStockBalance : Form
    {
        private StockDAL stockDAL = new StockDAL();
        private DataTable dtStockPrint;
        private int printRowIndex = 0;
        private int rowIndex = 0;





        private void LoadStockBalance()
        {
            try
            {
                dgvStockBalance.AutoGenerateColumns = false;

                dgvStockBalance.DataSource =
                    stockDAL.GetStockBalance();

                dgvStockBalance.ClearSelection();
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Unable to load stock balance.\n\n" + ex.Message,
                    "Stock Balance",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        public frmStockBalance()
        {
            InitializeComponent();
        }

        private void frmStockBalance_Load(object sender, EventArgs e)
        {
            LoadStockBalance();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStockBalance();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            dtStockPrint = stockDAL.GetCurrentStockPosition();

            if (dtStockPrint == null || dtStockPrint.Rows.Count == 0)
            {
                MessageBox.Show("Print ke liye data majood nahi hai.", "No Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            printRowIndex = 0;
            printPreviewDialog1.Document = printDocument1;
            printPreviewDialog1.ShowDialog();
        }

        private void printDocument1_PrintPage_1(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int x = 40;
            int y = 90;
            int cellHeight = 25;

            int[] colWidths = { 150, 100, 100, 100, 110, 110 };
            string[] headers = { "Category Name", "Denom (Rs)", "Boxes", "Packets", "Sheets", "Stamps" };

            // Header Title
            using (Font titleFont = new Font("Segoe UI", 14, FontStyle.Bold))
            {
                e.Graphics.DrawString("CENTRAL STAMP STORE - STOCK BALANCE POSITION", titleFont, Brushes.Navy, new PointF(120, 30));
            }

            using (Font subFont = new Font("Segoe UI", 9, FontStyle.Regular))
            {
                e.Graphics.DrawString("Date: " + DateTime.Now.ToString("dd-MMM-yyyy hh:mm tt"), subFont, Brushes.Black, new PointF(40, 65));
            }

            using (Font headerFont = new Font("Segoe UI", 9, FontStyle.Bold))
            using (Font cellFont = new Font("Segoe UI", 9, FontStyle.Regular))
            using (Pen pen = new Pen(Color.Gray, 1))
            {
                // Table Headers
                x = 40;
                for (int i = 0; i < headers.Length; i++)
                {
                    e.Graphics.FillRectangle(Brushes.LightGray, x, y, colWidths[i], cellHeight);
                    e.Graphics.DrawRectangle(pen, x, y, colWidths[i], cellHeight);
                    e.Graphics.DrawString(headers[i], headerFont, Brushes.Black, new RectangleF(x + 3, y + 4, colWidths[i] - 3, cellHeight));
                    x += colWidths[i];
                }

                y += cellHeight;

                // Table Rows
                while (printRowIndex < dtStockPrint.Rows.Count)
                {
                    DataRow row = dtStockPrint.Rows[printRowIndex];
                    x = 40;

                    string category = row["CategoryName"].ToString();
                    string denom = row["DenominationValue"].ToString();
                    string box = row["BalanceBoxQty"].ToString();
                    string packet = row["BalancePacketQty"].ToString();
                    string sheet = row["BalanceSheetQty"].ToString();
                    string stamp = row["BalanceStampQty"].ToString();

                    string[] rowData = { category, denom, box, packet, sheet, stamp };

                    for (int i = 0; i < rowData.Length; i++)
                    {
                        e.Graphics.DrawRectangle(pen, x, y, colWidths[i], cellHeight);
                        e.Graphics.DrawString(rowData[i], cellFont, Brushes.Black, new RectangleF(x + 3, y + 4, colWidths[i] - 3, cellHeight));
                        x += colWidths[i];
                    }

                    y += cellHeight;

                    if (y + cellHeight > e.MarginBounds.Bottom)
                    {
                        printRowIndex++;
                        e.HasMorePages = true;
                        return;
                    }

                    printRowIndex++;
                }

                e.HasMorePages = false;
            }
        }

        private void printDocument1_BeginPrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            printRowIndex = 0;
        }
    }
}
