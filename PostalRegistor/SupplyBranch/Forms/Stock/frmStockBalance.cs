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
    }
}
