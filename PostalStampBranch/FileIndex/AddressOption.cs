using PostalStampSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileIndex
{
    public partial class AddressOption : Form
    {
        public string SelectedOption { get; set; } = "";
        public AddressOption()
        {
            InitializeComponent();
        }


        private void AddressOption_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (radio1.Checked) SelectedOption = "WithNameAddress";
            else if (radio2.Checked) SelectedOption = "OnlyAddress";
            else if (radio3.Checked) SelectedOption = "Tag";

            this.DialogResult = DialogResult.OK; // Form band ho jayega aur 'OK' return karega
            this.Close();
        }
    }
}
