public static class UIHelper
{
    public static void ApplyTheme(Control parent)
    {
        foreach (Control c in parent.Controls)
        {
            // 1. TextBoxes, Numeric, ComboBox ke liye logic
            if (c is TextBox || c is NumericUpDown || c is ComboBox)
            {
                c.Enter += (s, ev) =>
                {
                    c.BackColor = Color.LightCyan;
                    if (c is NumericUpDown num) num.Select(0, num.Text.Length);
                    else if (c is TextBox txt) txt.SelectAll();
                };

                c.Leave += (s, ev) =>
                {
                    bool hasData = false;
                    if (c is TextBox && !string.IsNullOrWhiteSpace(c.Text)) hasData = true;
                    if (c is NumericUpDown num && num.Value > 0) hasData = true;
                    if (c is ComboBox combo && combo.SelectedIndex > -1) hasData = true;

                    c.BackColor = hasData ? Color.Khaki : Color.White;
                };
            }

            // 2. Buttons ke liye Special Design
            if (c is Button btn)
            {
                btn.BackColor = Color.FromArgb(0, 165, 255);
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.Cursor = Cursors.Hand;

                // Mouse upar aaye toh rang badle (Hover effect)
                btn.MouseEnter += (s, ev) => { btn.BackColor = Color.FromArgb(0, 120, 215); };

                // Mouse bahar jaye toh wapas asli rang
                btn.MouseLeave += (s, ev) => { btn.BackColor = Color.FromArgb(0, 165, 255); };
            }

            // Labels ko white karna (Dark background par nazar aane ke liye)
            if (c is Label lbl)
            {
                lbl.ForeColor = Color.White;
            }

            if (c.HasChildren)
            {
                ApplyTheme(c); // Recursion
            }
        }
    }

    public static void SetFormTheme(Form frm)
    {// #ADD8E6 173, 216, 230 44, 62, 80
        frm.BackColor = Color.FromArgb(44, 62, 80);
        frm.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        frm.StartPosition = FormStartPosition.CenterScreen;
    }
}