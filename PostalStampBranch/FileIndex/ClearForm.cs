using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileIndex
{
    internal class ClearForm
    {
        public static void ClearAllControls(Control parent)

        {
            foreach (Control c in parent.Controls)
            {
                // 1. Agar TextBox hai toh khali kar do
                if (c is TextBox)
                {
                    ((TextBox)c).Clear();
                }
                // 2. Agar ComboBox hai toh selection khatam kar do
                else if (c is ComboBox)
                {
                    ((ComboBox)c).SelectedIndex = -1;
                }
                // 3. Agar NumericUpDown hai toh value 0 kar do
                else if (c is NumericUpDown)
                {
                    ((NumericUpDown)c).Value = 0;
                }
                // 4. Agar CheckBox hai toh uncheck kar do
                else if (c is CheckBox)
                {
                    ((CheckBox)c).Checked = false;
                }
                // 5. Agar DateTimePicker hai toh aaj ki date set kar do
                else if (c is DateTimePicker)
                {
                    ((DateTimePicker)c).Value = DateTime.Now;
                }

                // SAB SE ZAROORI: Agar controls kisi Panel ya GroupBox ke andar hain
                if (c.HasChildren)
                {
                    ClearAllControls(c); // Ye khud ko dobara call karega (Recursion)
                }
            }
        }
    }
}
