using System;
using System.Windows.Forms;

namespace FileIndex
{
    internal class ValidationHelper
    {
        // Ye function check karega ke koi control khali toh nahi
        public static bool IsFormValid(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                // control ko Skip karny kay lay
                if (c.Tag != null && c.Tag.ToString() == "Skip")
                {
                    continue; // Isay choro aur aglay control par jao
                }

                // 1. TextBox check karein
                if (c is TextBox && string.IsNullOrWhiteSpace(c.Text))
                {
                    MessageBox.Show("Please fill the " + c.Name.Replace("txt_", "") + " field.");
                    c.Focus();
                    return false;
                }

                // 2. ComboBox check karein
                if (c is ComboBox && ((ComboBox)c).SelectedIndex == -1)
                {
                    MessageBox.Show("Please select an option in " + c.Name.Replace("com_", ""));
                    c.Focus();
                    return false;
                }

                // Agar control ke andar mazeed controls hain (Panel/GroupBox)
                if (c.HasChildren)
                {
                    if (!IsFormValid(c)) return false; // Recursion
                }
            }
            return true;
        }
    }
}