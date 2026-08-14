using System;
using System.Drawing;
using System.Windows.Forms;

namespace SupplyBranch.Helpers
{
    public static class UITheme
    {
        //==================================================
        // Modern Menu Renderer
        //==================================================

        private class ModernMenuRenderer :
            ToolStripProfessionalRenderer
        {
            public ModernMenuRenderer()
                : base(new ModernColorTable())
            {
            }
        }

        private class ModernColorTable :
            ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Color.FromArgb(91, 76, 170); }
            }

            public override Color MenuItemSelectedGradientBegin
            {
                get { return Color.FromArgb(91, 76, 170); }
            }

            public override Color MenuItemSelectedGradientEnd
            {
                get { return Color.FromArgb(110, 94, 195); }
            }

            public override Color MenuBorder
            {
                get { return Color.FromArgb(205, 209, 218); }
            }

            public override Color ToolStripDropDownBackground
            {
                get { return Color.White; }
            }

            public override Color ImageMarginGradientBegin
            {
                get { return Color.White; }
            }

            public override Color ImageMarginGradientMiddle
            {
                get { return Color.White; }
            }

            public override Color ImageMarginGradientEnd
            {
                get { return Color.White; }
            }
        }


        //==================================================
        // Main Colors
        //==================================================

        private static readonly Color FormBackColor =
            Color.FromArgb(245, 247, 250);

        private static readonly Color PanelBackColor =
            Color.White;

        // Slightly off-white so empty controls remain visible
        private static readonly Color TextBackColor =
            Color.FromArgb(252, 253, 255);

        private static readonly Color TextForeColor =
            Color.FromArgb(35, 40, 50);

        // Normal border - clearly visible
        private static readonly Color BorderColor =
            Color.FromArgb(175, 181, 192);

        // Focus border
        private static readonly Color FocusBorderColor =
            Color.FromArgb(91, 76, 170);

        // Header
        private static readonly Color HeaderColor =
            Color.FromArgb(75, 63, 145);

        // Main button
        private static readonly Color ButtonColor =
            Color.FromArgb(91, 76, 170);

        private static readonly Color ButtonHoverColor =
            Color.FromArgb(110, 94, 195);

        private static readonly Color ButtonPressedColor =
            Color.FromArgb(72, 59, 145);

        // Menu
        private static readonly Color MenuBackColor =
            Color.FromArgb(55, 48, 95);


        //==================================================
        // Fonts
        //==================================================

        private static readonly Font NormalFont =
            new Font("Segoe UI", 9.5F, FontStyle.Regular);

        private static readonly Font LabelFont =
            new Font("Segoe UI", 9.5F, FontStyle.Regular);

        private static readonly Font ButtonFont =
            new Font("Segoe UI", 9.5F, FontStyle.Bold);

        private static readonly Font GridHeaderFont =
            new Font("Segoe UI", 9.5F, FontStyle.Bold);

        private static readonly Font MenuFont =
            new Font("Segoe UI", 10F, FontStyle.Regular);

        private static readonly Font GroupBoxFont =
            new Font("Segoe UI", 10F, FontStyle.Bold);


        //==================================================
        // Apply Theme
        //==================================================

        public static void Apply(Form form)
        {
            if (form == null)
                return;

            form.BackColor = FormBackColor;
            form.Font = NormalFont;

            ApplyToControls(form);
        }


        //==================================================
        // Apply recursively to all controls
        //==================================================

        //private static void ApplyToControls(Control parent)
        //{
        //    foreach (Control control in parent.Controls)
        //    {
        //        ApplyControl(control);

        //        if (control.HasChildren)
        //        {
        //            ApplyToControls(control);
        //        }
        //    }
        //}

        private static void ApplyToControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control.Name == "lblSubTitle" && control is Label lblSubTitle)
                {
                    // Subtitle specific properties
                    lblSubTitle.Font = new Font(
                        "Segoe UI",
                        10F,
                        FontStyle.Bold);

                    lblSubTitle.ForeColor =
                        Color.FromArgb(100, 105, 115);

                    lblSubTitle.BackColor =
                        Color.Transparent;

                    lblSubTitle.AutoSize = true;
                }
                else
                {
                    ApplyControl(control);
                }

                if (control.HasChildren)
                {
                    ApplyToControls(control);
                }
            }
        }

        //==================================================
        // MenuStrip
        //==================================================

        private static void ApplyMenuStrip(MenuStrip menuStrip)
        {
            menuStrip.Font = MenuFont;

            menuStrip.BackColor = MenuBackColor;
            menuStrip.ForeColor = Color.White;

            menuStrip.RenderMode =
                ToolStripRenderMode.Professional;

            menuStrip.Renderer =
                new ModernMenuRenderer();

            menuStrip.Padding =
                new Padding(6, 3, 6, 3);

            foreach (ToolStripItem item in menuStrip.Items)
            {
                if (item is ToolStripMenuItem menuItem)
                {
                    ApplyMenuItem(menuItem);
                }
            }
        }


        private static void ApplyMenuItem(ToolStripMenuItem item)
        {
            item.Font = MenuFont;

            item.ForeColor = Color.White;
            item.BackColor = MenuBackColor;

            item.Padding =
                new Padding(8, 5, 8, 5);

            foreach (ToolStripItem child in item.DropDownItems)
            {
                if (child is ToolStripMenuItem subItem)
                {
                    subItem.Font = MenuFont;

                    subItem.ForeColor =
                        Color.FromArgb(35, 40, 50);

                    subItem.BackColor =
                        Color.White;

                    subItem.Padding =
                        new Padding(10, 5, 10, 5);

                    ApplyMenuItem(subItem);
                }
            }
        }


        //==================================================
        // Individual Control Styling
        //==================================================

        private static void ApplyControl(Control control)
        {
            // ---------------------------------------------
            // Label
            // ---------------------------------------------

            if (control is Label)
            {
                control.Font = LabelFont;
                control.ForeColor = TextForeColor;
                control.BackColor = Color.Transparent;
            }


            // ---------------------------------------------
            // TextBox
            // ---------------------------------------------

            else if (control is TextBox)
            {
                TextBox txt = (TextBox)control;

                txt.Font = NormalFont;

                txt.ForeColor =
                    TextForeColor;

                // Clearly visible empty background
                txt.BackColor =
                    TextBackColor;

                txt.BorderStyle =
                    BorderStyle.FixedSingle;

                txt.Margin =
                    new Padding(3);

                // Make empty textbox visually identifiable
                AddTextBoxFocusEffect(txt);
            }


            // ---------------------------------------------
            // ComboBox
            // ---------------------------------------------

            else if (control is ComboBox)
            {
                ComboBox cmb = (ComboBox)control;

                cmb.Font = NormalFont;

                cmb.ForeColor =
                    TextForeColor;

                // White background so empty combo is visible
                cmb.BackColor =
                    Color.White;

                // Standard gives a much clearer boundary
                cmb.FlatStyle =
                    FlatStyle.Standard;

                cmb.DropDownStyle =
                    ComboBoxStyle.DropDownList;

                cmb.IntegralHeight = true;

                cmb.Margin =
                    new Padding(3);

                AddComboBoxFocusEffect(cmb);
            }


            // ---------------------------------------------
            // Button
            // ---------------------------------------------

            else if (control is Button)
            {
                Button btn = (Button)control;

                btn.Font =
                    ButtonFont;

                btn.ForeColor =
                    Color.White;

                btn.BackColor =
                    ButtonColor;

                btn.FlatStyle =
                    FlatStyle.Flat;

                btn.FlatAppearance.BorderSize =
                    0;

                btn.FlatAppearance.MouseOverBackColor =
                    ButtonHoverColor;

                btn.FlatAppearance.MouseDownBackColor =
                    ButtonPressedColor;

                btn.Cursor =
                    Cursors.Hand;

                btn.Padding =
                    new Padding(10, 5, 10, 5);

                AddButtonHover(btn);
            }


            // ---------------------------------------------
            // MenuStrip
            // ---------------------------------------------

            else if (control is MenuStrip)
            {
                ApplyMenuStrip(
                    (MenuStrip)control);
            }


            // ---------------------------------------------
            // CheckBox
            // ---------------------------------------------

            else if (control is CheckBox)
            {
                CheckBox chk =
                    (CheckBox)control;

                chk.Font =
                    NormalFont;

                chk.ForeColor =
                    TextForeColor;

                chk.BackColor =
                    Color.Transparent;
            }


            // ---------------------------------------------
            // RadioButton
            // ---------------------------------------------

            else if (control is RadioButton)
            {
                RadioButton rb =
                    (RadioButton)control;

                rb.Font =
                    NormalFont;

                rb.ForeColor =
                    TextForeColor;

                rb.BackColor =
                    Color.Transparent;
            }


            // ---------------------------------------------
            // DateTimePicker
            // ---------------------------------------------

            else if (control is DateTimePicker)
            {
                DateTimePicker dtp =
                    (DateTimePicker)control;

                dtp.Font =
                    NormalFont;

                dtp.ForeColor =
                    TextForeColor;

                dtp.BackColor =
                    Color.White;

                dtp.Format =
                    DateTimePickerFormat.Short;

                AddDateTimePickerFocusEffect(dtp);
            }


            // ---------------------------------------------
            // NumericUpDown
            // ---------------------------------------------

            else if (control is NumericUpDown)
            {
                NumericUpDown num =
                    (NumericUpDown)control;

                num.Font =
                    NormalFont;

                num.ForeColor =
                    TextForeColor;

                num.BackColor =
                    TextBackColor;

                AddNumericFocusEffect(num);
            }


            // ---------------------------------------------
            // DataGridView
            // ---------------------------------------------

            else if (control is DataGridView)
            {
                ApplyDataGridView(
                    (DataGridView)control);
            }


            // ---------------------------------------------
            // GroupBox
            // ---------------------------------------------

            else if (control is GroupBox)
            {
                GroupBox grp =
                    (GroupBox)control;

                grp.Font =
                    GroupBoxFont;

                grp.ForeColor =
                    HeaderColor;

                grp.BackColor =
                    PanelBackColor;
            }


            // ---------------------------------------------
            // Panel
            // ---------------------------------------------

            else if (control is Panel)
            {
                Panel panel =
                    (Panel)control;

                panel.BackColor =
                    PanelBackColor;
            }


            // ---------------------------------------------
            // TabControl
            // ---------------------------------------------

            else if (control is TabControl)
            {
                TabControl tab =
                    (TabControl)control;

                tab.Font =
                    NormalFont;
            }


            // ---------------------------------------------
            // TabPage
            // ---------------------------------------------

            else if (control is TabPage)
            {
                TabPage page =
                    (TabPage)control;

                page.BackColor =
                    FormBackColor;

                page.Font =
                    NormalFont;
            }
        }


        //==================================================
        // TextBox Focus Effect
        //==================================================

        private static void AddTextBoxFocusEffect(
            TextBox txt)
        {
            EventHandler enterHandler =
                (s, e) =>
                {
                    txt.BackColor =
                        Color.White;

                    txt.ForeColor =
                        TextForeColor;
                };

            EventHandler leaveHandler =
                (s, e) =>
                {
                    txt.BackColor =
                        TextBackColor;
                };

            txt.Enter += enterHandler;
            txt.Leave += leaveHandler;
        }


        //==================================================
        // ComboBox Focus Effect
        //==================================================

        private static void AddComboBoxFocusEffect(
            ComboBox cmb)
        {
            EventHandler enterHandler =
                (s, e) =>
                {
                    cmb.BackColor =
                        Color.White;

                    cmb.ForeColor =
                        TextForeColor;
                };

            EventHandler leaveHandler =
                (s, e) =>
                {
                    cmb.BackColor =
                        Color.White;
                };

            cmb.Enter += enterHandler;
            cmb.Leave += leaveHandler;
        }


        //==================================================
        // DateTimePicker Focus Effect
        //==================================================

        private static void AddDateTimePickerFocusEffect(
            DateTimePicker dtp)
        {
            EventHandler enterHandler =
                (s, e) =>
                {
                    dtp.BackColor =
                        Color.White;
                };

            EventHandler leaveHandler =
                (s, e) =>
                {
                    dtp.BackColor =
                        Color.White;
                };

            dtp.Enter += enterHandler;
            dtp.Leave += leaveHandler;
        }


        //==================================================
        // NumericUpDown Focus Effect
        //==================================================

        private static void AddNumericFocusEffect(
            NumericUpDown num)
        {
            EventHandler enterHandler =
                (s, e) =>
                {
                    num.BackColor =
                        Color.White;
                };

            EventHandler leaveHandler =
                (s, e) =>
                {
                    num.BackColor =
                        TextBackColor;
                };

            num.Enter += enterHandler;
            num.Leave += leaveHandler;
        }


        //==================================================
        // DataGridView
        //==================================================

        private static void ApplyDataGridView(
            DataGridView dgv)
        {
            dgv.Font =
                NormalFont;

            dgv.BackgroundColor =
                Color.White;

            dgv.BorderStyle =
                BorderStyle.None;

            dgv.CellBorderStyle =
                DataGridViewCellBorderStyle.SingleHorizontal;

            dgv.GridColor =
                Color.FromArgb(225, 228, 234);

            dgv.EnableHeadersVisualStyles =
                false;


            // ---------------------------------------------
            // Header
            // ---------------------------------------------

            dgv.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        HeaderColor,

                    ForeColor =
                        Color.White,

                    Font =
                        GridHeaderFont,

                    Alignment =
                        DataGridViewContentAlignment.MiddleCenter,

                    SelectionBackColor =
                        HeaderColor,

                    SelectionForeColor =
                        Color.White,

                    Padding =
                        new Padding(5, 6, 5, 6)
                };


            // ---------------------------------------------
            // Rows
            // ---------------------------------------------

            dgv.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.White,

                    ForeColor =
                        TextForeColor,

                    SelectionBackColor =
                        Color.FromArgb(225, 222, 245),

                    SelectionForeColor =
                        TextForeColor,

                    Font =
                        NormalFont,

                    Padding =
                        new Padding(5, 4, 5, 4)
                };


            // ---------------------------------------------
            // Alternating rows
            // ---------------------------------------------

            dgv.AlternatingRowsDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor =
                        Color.FromArgb(248, 249, 252),

                    ForeColor =
                        TextForeColor,

                    SelectionBackColor =
                        Color.FromArgb(225, 222, 245),

                    SelectionForeColor =
                        TextForeColor
                };


            dgv.RowHeadersVisible =
                false;

            dgv.AllowUserToAddRows =
                false;

            dgv.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;

            dgv.MultiSelect =
                false;

            dgv.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.None;

            dgv.RowTemplate.Height =
                31;

            dgv.ColumnHeadersHeight =
                34;
        }


        //==================================================
        // Button Hover Effect
        //==================================================

        private static void AddButtonHover(
            Button btn)
        {
            btn.MouseEnter += (s, e) =>
            {
                if (btn.Enabled)
                {
                    btn.BackColor =
                        ButtonHoverColor;
                }
            };

            btn.MouseLeave += (s, e) =>
            {
                if (btn.Enabled)
                {
                    btn.BackColor =
                        ButtonColor;
                }
            };

            btn.MouseDown += (s, e) =>
            {
                if (btn.Enabled)
                {
                    btn.BackColor =
                        ButtonPressedColor;
                }
            };

            btn.MouseUp += (s, e) =>
            {
                if (btn.Enabled)
                {
                    btn.BackColor =
                        ButtonHoverColor;
                }
            };
        }


    }
}