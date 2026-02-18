using System;
using System.Drawing;
using System.Windows.Forms;

namespace PostalStampSystem
{
    /// <summary>
    /// Centralized Theme Manager - Tamam forms ko ek professional look dene ke liye
    /// Usage: Form_Load event mein sirf "ThemeManager.ApplyTheme(this);" likho
    /// </summary>
    public static class ThemeManager
    {
        #region Theme Mode Settings

        public enum ThemeMode
        {
            Light,
            Dark
        }

        // Current active theme (default Light)
        private static ThemeMode _currentTheme = ThemeMode.Light;

        /// <summary>
        /// Current theme mode ko get/set karta hai
        /// </summary>
        public static ThemeMode CurrentTheme
        {
            get { return _currentTheme; }
            set
            {
                _currentTheme = value;
                // User preference save karo
                SaveThemePreference(value);
            }
        }

        /// <summary>
        /// Event - jab theme change ho to trigger hota hai
        /// </summary>
        public static event EventHandler ThemeChanged;

        #endregion

        #region Color Scheme - Dynamic (Light/Dark)

        // ─────────────────────────────────────────────────────────
        // LIGHT THEME COLORS
        // ─────────────────────────────────────────────────────────
        private static class LightTheme
        {
            // Primary Colors
            public static readonly Color PrimaryBlue = Color.FromArgb(41, 128, 185);
            public static readonly Color DarkBlue = Color.FromArgb(23, 32, 42);
            public static readonly Color LightBlue = Color.FromArgb(52, 152, 219);

            // Background Colors
            public static readonly Color FormBackground = Color.FromArgb(236, 240, 241);
            public static readonly Color PanelBackground = Color.White;
            public static readonly Color GroupBoxBackground = Color.FromArgb(250, 250, 250);

            // Text Colors
            public static readonly Color PrimaryText = Color.FromArgb(44, 62, 80);
            public static readonly Color SecondaryText = Color.FromArgb(127, 140, 141);
            public static readonly Color LabelText = Color.FromArgb(52, 73, 94);

            // Control Colors
            public static readonly Color ControlBorder = Color.FromArgb(189, 195, 199);
            public static readonly Color ControlFocus = Color.FromArgb(52, 152, 219);
            public static readonly Color DisabledControl = Color.FromArgb(236, 240, 241);
            public static readonly Color TextBoxBackground = Color.White;
            public static readonly Color TextBoxFocus = Color.FromArgb(255, 251, 230);

            // Button Colors
            public static readonly Color ButtonPrimary = Color.FromArgb(41, 128, 185);
            public static readonly Color ButtonSuccess = Color.FromArgb(39, 174, 96);
            public static readonly Color ButtonDanger = Color.FromArgb(231, 76, 60);
            public static readonly Color ButtonWarning = Color.FromArgb(243, 156, 18);
            public static readonly Color ButtonHover = Color.FromArgb(52, 152, 219);

            // DataGrid Colors
            public static readonly Color GridBackground = Color.White;
            public static readonly Color GridHeaderBack = Color.FromArgb(23, 32, 42);
            public static readonly Color GridHeaderFore = Color.White;
            public static readonly Color GridAlternateRow = Color.FromArgb(245, 245, 245);
        }

        // ─────────────────────────────────────────────────────────
        // DARK THEME COLORS
        // ─────────────────────────────────────────────────────────
        private static class DarkTheme
        {
            // Primary Colors
            public static readonly Color PrimaryBlue = Color.FromArgb(52, 152, 219);
            public static readonly Color DarkBlue = Color.FromArgb(236, 240, 241);
            public static readonly Color LightBlue = Color.FromArgb(41, 128, 185);

            // Background Colors
            public static readonly Color FormBackground = Color.FromArgb(33, 37, 41);
            public static readonly Color PanelBackground = Color.FromArgb(52, 58, 64);
            public static readonly Color GroupBoxBackground = Color.FromArgb(44, 47, 51);

            // Text Colors
            public static readonly Color PrimaryText = Color.FromArgb(236, 240, 241);
            public static readonly Color SecondaryText = Color.FromArgb(189, 195, 199);
            public static readonly Color LabelText = Color.FromArgb(220, 221, 222);

            // Control Colors
            public static readonly Color ControlBorder = Color.FromArgb(73, 80, 87);
            public static readonly Color ControlFocus = Color.FromArgb(52, 152, 219);
            public static readonly Color DisabledControl = Color.FromArgb(52, 58, 64);
            public static readonly Color TextBoxBackground = Color.FromArgb(52, 58, 64);
            public static readonly Color TextBoxFocus = Color.FromArgb(64, 68, 75);

            // Button Colors
            public static readonly Color ButtonPrimary = Color.FromArgb(0, 123, 255);
            public static readonly Color ButtonSuccess = Color.FromArgb(40, 167, 69);
            public static readonly Color ButtonDanger = Color.FromArgb(220, 53, 69);
            public static readonly Color ButtonWarning = Color.FromArgb(255, 193, 7);
            public static readonly Color ButtonHover = Color.FromArgb(23, 162, 184);

            // DataGrid Colors
            public static readonly Color GridBackground = Color.FromArgb(52, 58, 64);
            public static readonly Color GridHeaderBack = Color.FromArgb(73, 80, 87);
            public static readonly Color GridHeaderFore = Color.FromArgb(236, 240, 241);
            public static readonly Color GridAlternateRow = Color.FromArgb(44, 47, 51);
        }

        // ─────────────────────────────────────────────────────────
        // DYNAMIC COLOR PROPERTIES (Auto-switch based on theme)
        // ─────────────────────────────────────────────────────────
        public static Color PrimaryBlue => _currentTheme == ThemeMode.Light ? LightTheme.PrimaryBlue : DarkTheme.PrimaryBlue;
        public static Color DarkBlue => _currentTheme == ThemeMode.Light ? LightTheme.DarkBlue : DarkTheme.DarkBlue;
        public static Color LightBlue => _currentTheme == ThemeMode.Light ? LightTheme.LightBlue : DarkTheme.LightBlue;
        public static Color FormBackground => _currentTheme == ThemeMode.Light ? LightTheme.FormBackground : DarkTheme.FormBackground;
        public static Color PanelBackground => _currentTheme == ThemeMode.Light ? LightTheme.PanelBackground : DarkTheme.PanelBackground;
        public static Color GroupBoxBackground => _currentTheme == ThemeMode.Light ? LightTheme.GroupBoxBackground : DarkTheme.GroupBoxBackground;
        public static Color PrimaryText => _currentTheme == ThemeMode.Light ? LightTheme.PrimaryText : DarkTheme.PrimaryText;
        public static Color SecondaryText => _currentTheme == ThemeMode.Light ? LightTheme.SecondaryText : DarkTheme.SecondaryText;
        public static Color LabelText => _currentTheme == ThemeMode.Light ? LightTheme.LabelText : DarkTheme.LabelText;
        public static Color ControlBorder => _currentTheme == ThemeMode.Light ? LightTheme.ControlBorder : DarkTheme.ControlBorder;
        public static Color ControlFocus => _currentTheme == ThemeMode.Light ? LightTheme.ControlFocus : DarkTheme.ControlFocus;
        public static Color DisabledControl => _currentTheme == ThemeMode.Light ? LightTheme.DisabledControl : DarkTheme.DisabledControl;
        public static Color TextBoxBackground => _currentTheme == ThemeMode.Light ? LightTheme.TextBoxBackground : DarkTheme.TextBoxBackground;
        public static Color TextBoxFocus => _currentTheme == ThemeMode.Light ? LightTheme.TextBoxFocus : DarkTheme.TextBoxFocus;
        public static Color ButtonPrimary => _currentTheme == ThemeMode.Light ? LightTheme.ButtonPrimary : DarkTheme.ButtonPrimary;
        public static Color ButtonSuccess => _currentTheme == ThemeMode.Light ? LightTheme.ButtonSuccess : DarkTheme.ButtonSuccess;
        public static Color ButtonDanger => _currentTheme == ThemeMode.Light ? LightTheme.ButtonDanger : DarkTheme.ButtonDanger;
        public static Color ButtonWarning => _currentTheme == ThemeMode.Light ? LightTheme.ButtonWarning : DarkTheme.ButtonWarning;
        public static Color ButtonHover => _currentTheme == ThemeMode.Light ? LightTheme.ButtonHover : DarkTheme.ButtonHover;
        public static Color GridBackground => _currentTheme == ThemeMode.Light ? LightTheme.GridBackground : DarkTheme.GridBackground;
        public static Color GridHeaderBack => _currentTheme == ThemeMode.Light ? LightTheme.GridHeaderBack : DarkTheme.GridHeaderBack;
        public static Color GridHeaderFore => _currentTheme == ThemeMode.Light ? LightTheme.GridHeaderFore : DarkTheme.GridHeaderFore;
        public static Color GridAlternateRow => _currentTheme == ThemeMode.Light ? LightTheme.GridAlternateRow : DarkTheme.GridAlternateRow;

        #endregion

        #region Font Settings

        public static readonly Font FormTitleFont = new Font("Segoe UI", 14F, FontStyle.Bold);
        public static readonly Font LabelFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font TextBoxFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font ButtonFont = new Font("Segoe UI", 9.5F, FontStyle.Bold);
        public static readonly Font GroupBoxFont = new Font("Segoe UI", 10F, FontStyle.Bold);
        public static readonly Font DataGridFont = new Font("Segoe UI", 9F, FontStyle.Regular);

        #endregion

        #region Main Theme Application Method

        /// <summary>
        /// Form par complete theme apply karta hai
        /// Usage: ThemeManager.ApplyTheme(this);
        /// </summary>
        public static void ApplyTheme(Form form)
        {
            if (form == null) return;

            // Form ki basic settings
            form.BackColor = FormBackground;
            form.Font = LabelFont;

            // Recursively sabhi controls par theme apply karo
            ApplyThemeToControls(form.Controls);
        }

        /// <summary>
        /// Recursively har control par theme apply karta hai
        /// </summary>
        private static void ApplyThemeToControls(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                // Control type ke mutabiq styling
                switch (control)
                {
                    case TextBox textBox:
                        StyleTextBox(textBox);
                        break;

                    case ComboBox comboBox:
                        StyleComboBox(comboBox);
                        break;

                    case Button button:
                        StyleButton(button);
                        break;

                    case Label label:
                        StyleLabel(label);
                        break;

                    case GroupBox groupBox:
                        StyleGroupBox(groupBox);
                        break;

                    case Panel panel:
                        StylePanel(panel);
                        break;

                    case DataGridView dgv:
                        StyleDataGridView(dgv);
                        break;

                    case DateTimePicker dtp:
                        StyleDateTimePicker(dtp);
                        break;

                    case CheckBox checkBox:
                        StyleCheckBox(checkBox);
                        break;

                    case RadioButton radioButton:
                        StyleRadioButton(radioButton);
                        break;

                    case NumericUpDown numeric:
                        StyleNumericUpDown(numeric);
                        break;
                }

                // Agar control ke andar aur controls hain (like Panel, GroupBox)
                if (control.HasChildren)
                {
                    ApplyThemeToControls(control.Controls);
                }
            }
        }

        #endregion

        #region Individual Control Styling Methods

        private static void StyleTextBox(TextBox txt)
        {
            txt.Font = TextBoxFont;
            txt.BackColor = TextBoxBackground;
            txt.ForeColor = PrimaryText;
            txt.BorderStyle = BorderStyle.FixedSingle;

            // Focus events ke liye
            txt.Enter += (s, e) =>
            {
                txt.BackColor = TextBoxFocus;
            };
            txt.Leave += (s, e) =>
            {
                txt.BackColor = TextBoxBackground;
            };
        }

        private static void StyleComboBox(ComboBox cmb)
        {
            cmb.Font = TextBoxFont;
            cmb.BackColor = TextBoxBackground;
            cmb.ForeColor = PrimaryText;
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.DropDownStyle = ComboBoxStyle.DropDownList; // Typing prevent karne ke liye
        }

        private static void StyleButton(Button btn)
        {
            btn.Font = ButtonFont;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Cursor = Cursors.Hand;
            btn.FlatAppearance.BorderSize = 0;
            btn.Height = 35; // Standard height

            // Button text ke mutabiq color (smart detection)
            string btnText = btn.Text.ToLower();

            if (btnText.Contains("save") || btnText.Contains("submit") || btnText.Contains("💾"))
            {
                btn.BackColor = ButtonSuccess;
            }
            else if (btnText.Contains("delete") || btnText.Contains("remove") || btnText.Contains("🗑"))
            {
                btn.BackColor = ButtonDanger;
            }
            else if (btnText.Contains("print") || btnText.Contains("🖨"))
            {
                btn.BackColor = ButtonWarning;
            }
            else if (btnText.Contains("search") || btnText.Contains("🔍"))
            {
                btn.BackColor = PrimaryBlue;
            }
            else
            {
                btn.BackColor = PrimaryBlue; // Default
            }

            btn.ForeColor = Color.White;

            // Hover effect
            btn.MouseEnter += (s, e) =>
            {
                btn.BackColor = ButtonHover;
            };
            btn.MouseLeave += (s, e) =>
            {
                // Original color restore
                if (btnText.Contains("save") || btnText.Contains("submit"))
                    btn.BackColor = ButtonSuccess;
                else if (btnText.Contains("delete") || btnText.Contains("remove"))
                    btn.BackColor = ButtonDanger;
                else if (btnText.Contains("print"))
                    btn.BackColor = ButtonWarning;
                else
                    btn.BackColor = PrimaryBlue;
            };
        }

        private static void StyleLabel(Label lbl)
        {
            lbl.Font = LabelFont;
            lbl.ForeColor = LabelText;
            lbl.BackColor = Color.Transparent;
        }

        private static void StyleGroupBox(GroupBox grp)
        {
            grp.Font = GroupBoxFont;
            grp.ForeColor = DarkBlue;
            grp.BackColor = GroupBoxBackground;
            grp.FlatStyle = FlatStyle.Flat;
        }

        private static void StylePanel(Panel pnl)
        {
            pnl.BackColor = PanelBackground;
            pnl.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void StyleDataGridView(DataGridView dgv)
        {
            dgv.Font = DataGridFont;
            dgv.BackgroundColor = GridBackground;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Header styling
            dgv.ColumnHeadersDefaultCellStyle.BackColor = GridHeaderBack;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = GridHeaderFore;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = GridHeaderBack;
            dgv.ColumnHeadersHeight = 40;

            // Row styling
            dgv.DefaultCellStyle.BackColor = GridBackground;
            dgv.DefaultCellStyle.ForeColor = PrimaryText;
            dgv.DefaultCellStyle.SelectionBackColor = LightBlue;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.RowTemplate.Height = 35;

            // Alternating row color
            dgv.AlternatingRowsDefaultCellStyle.BackColor = GridAlternateRow;

            dgv.EnableHeadersVisualStyles = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;
        }

        private static void StyleDateTimePicker(DateTimePicker dtp)
        {
            dtp.Font = TextBoxFont;
            dtp.CalendarForeColor = PrimaryText;
            dtp.CalendarMonthBackground = TextBoxBackground;
            dtp.Format = DateTimePickerFormat.Short;
        }

        private static void StyleCheckBox(CheckBox chk)
        {
            chk.Font = LabelFont;
            chk.ForeColor = LabelText;
            chk.BackColor = Color.Transparent;
        }

        private static void StyleRadioButton(RadioButton rdb)
        {
            rdb.Font = LabelFont;
            rdb.ForeColor = LabelText;
            rdb.BackColor = Color.Transparent;
        }

        private static void StyleNumericUpDown(NumericUpDown numeric)
        {
            numeric.Font = TextBoxFont;
            numeric.BackColor = TextBoxBackground;
            numeric.ForeColor = PrimaryText;
            numeric.BorderStyle = BorderStyle.FixedSingle;
        }

        #endregion

        #region Theme Management Methods

        /// <summary>
        /// Theme change karta hai aur sabhi open forms ko update karta hai
        /// </summary>
        public static void ChangeTheme(ThemeMode newTheme)
        {
            if (_currentTheme == newTheme) return; // Already same theme

            _currentTheme = newTheme;
            SaveThemePreference(newTheme);

            // Sabhi open forms ko refresh karo
            RefreshAllOpenForms();

            // Event trigger karo
            ThemeChanged?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Sabhi open forms ko refresh karta hai (theme apply karta hai)
        /// </summary>
        private static void RefreshAllOpenForms()
        {
            foreach (Form form in Application.OpenForms)
            {
                ApplyTheme(form);
                form.Refresh();
            }
        }

        /// <summary>
        /// User ki theme preference save karta hai (registry/settings mein)
        /// </summary>
        private static void SaveThemePreference(ThemeMode theme)
        {
            try
            {
                Microsoft.Win32.Registry.SetValue(
                    @"HKEY_CURRENT_USER\Software\PostalStampSystem",
                    "Theme",
                    theme.ToString()
                );
            }
            catch
            {
                // Registry access fail ho to koi masla nahi
            }
        }

        /// <summary>
        /// Saved theme preference load karta hai
        /// </summary>
        public static void LoadThemePreference()
        {
            try
            {
                object value = Microsoft.Win32.Registry.GetValue(
                    @"HKEY_CURRENT_USER\Software\PostalStampSystem",
                    "Theme",
                    "Light"
                );

                if (value != null && Enum.TryParse(value.ToString(), out ThemeMode savedTheme))
                {
                    _currentTheme = savedTheme;
                }
            }
            catch
            {
                _currentTheme = ThemeMode.Light; // Default
            }
        }

        #endregion

        #region Helper Methods - Manual Styling (Optional)

        /// <summary>
        /// Specific button ko manually color do
        /// </summary>
        public static void SetButtonColor(Button btn, string colorType)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.ForeColor = Color.White;
            btn.Cursor = Cursors.Hand;

            switch (colorType.ToLower())
            {
                case "success":
                case "save":
                    btn.BackColor = ButtonSuccess;
                    break;
                case "danger":
                case "delete":
                    btn.BackColor = ButtonDanger;
                    break;
                case "warning":
                case "print":
                    btn.BackColor = ButtonWarning;
                    break;
                default:
                    btn.BackColor = ButtonPrimary;
                    break;
            }
        }

        /// <summary>
        /// Title label ke liye special styling
        /// </summary>
        public static void StyleAsTitle(Label lbl)
        {
            lbl.Font = FormTitleFont;
            lbl.ForeColor = DarkBlue;
            lbl.BackColor = Color.Transparent;
        }

        #endregion
    }
}
