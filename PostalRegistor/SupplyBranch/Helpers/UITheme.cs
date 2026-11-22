using System;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace SupplyBranch.Helpers
{
    /// <summary>
    /// Application-wide modern theme (same palette as frmLogin).
    /// Usage: UITheme.Apply(this) in a form's Load event.
    /// Only colors, fonts and flat styles are changed - never sizes, positions or docking.
    /// </summary>
    public static class UITheme
    {
        //==================================================
        // Main Colors (same palette as frmLogin)
        //==================================================

        private static readonly Color Primary = ColorTranslator.FromHtml("#2563EB");
        private static readonly Color PrimaryHover = ColorTranslator.FromHtml("#1D4ED8");
        private static readonly Color PrimaryPressed = ColorTranslator.FromHtml("#1E40AF");
        private static readonly Color Success = ColorTranslator.FromHtml("#16A34A");
        private static readonly Color SuccessHover = ColorTranslator.FromHtml("#15803D");
        private static readonly Color Danger = ColorTranslator.FromHtml("#DC2626");
        private static readonly Color DangerHover = ColorTranslator.FromHtml("#B91C1C");
        private static readonly Color Slate = ColorTranslator.FromHtml("#334155");
        private static readonly Color SlateHover = ColorTranslator.FromHtml("#1E293B");
        private static readonly Color Dark = ColorTranslator.FromHtml("#0F172A");

        private static readonly Color FormBackColor = ColorTranslator.FromHtml("#F1F5F9");
        private static readonly Color PanelBackColor = Color.White;
        // Input fields are tinted light blue so empty boxes stand out from the white cards.
        // Typing (focus) turns the field white; read-only fields are neutral grey.
        private static readonly Color TextBackColor = ColorTranslator.FromHtml("#E8F0FE");
        private static readonly Color FocusBackColor = Color.White;
        private static readonly Color ReadOnlyBack = ColorTranslator.FromHtml("#EEF0F3");
        private static readonly Color ReadOnlyFore = ColorTranslator.FromHtml("#475569");
        private static readonly Color TextForeColor = ColorTranslator.FromHtml("#1E293B");
        private static readonly Color BorderColor = ColorTranslator.FromHtml("#CBD5E1");
        private static readonly Color GridLineColor = ColorTranslator.FromHtml("#E2E8F0");
        private static readonly Color HeaderColor = Dark;
        private static readonly Color RowAlt = ColorTranslator.FromHtml("#F8FAFC");
        private static readonly Color RowSelect = ColorTranslator.FromHtml("#DBEAFE");
        private static readonly Color DisabledBack = ColorTranslator.FromHtml("#E2E8F0");
        private static readonly Color DisabledFore = ColorTranslator.FromHtml("#94A3B8");
        private static readonly Color MenuBackColor = Dark;


        //==================================================
        // Fonts
        //==================================================

        private static readonly Font NormalFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        private static readonly Font LabelFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        private static readonly Font ButtonFont = new Font("Segoe UI Semibold", 9.5F);
        private static readonly Font GridHeaderFont = new Font("Segoe UI Semibold", 9.5F);
        private static readonly Font MenuFont = new Font("Segoe UI", 10F, FontStyle.Regular);
        private static readonly Font GroupBoxFont = new Font("Segoe UI Semibold", 10F);


        //==================================================
        // Tracks controls that already have event handlers attached,
        // so calling Apply() twice never attaches handlers twice
        //==================================================

        private static readonly ConditionalWeakTable<Control, object> Styled =
            new ConditionalWeakTable<Control, object>();

        // Text boxes styled as status badges - their colors must not be reset on focus changes
        private static readonly ConditionalWeakTable<Control, object> StatusBoxes =
            new ConditionalWeakTable<Control, object>();

        private static bool FirstTime(Control c)
        {
            object o;
            if (Styled.TryGetValue(c, out o))
                return false;

            Styled.Add(c, new object());
            return true;
        }


        //==================================================
        // Modern Menu Renderer
        //==================================================

        private class ModernMenuRenderer : ToolStripProfessionalRenderer
        {
            public ModernMenuRenderer()
                : base(new ModernColorTable())
            {
            }
        }

        private class ModernColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected
            {
                get { return Primary; }
            }

            public override Color MenuItemSelectedGradientBegin
            {
                get { return Primary; }
            }

            public override Color MenuItemSelectedGradientEnd
            {
                get { return PrimaryHover; }
            }

            public override Color MenuItemBorder
            {
                get { return Primary; }
            }

            public override Color MenuBorder
            {
                get { return BorderColor; }
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
        // Apply Theme
        //==================================================

        public static void Apply(Form form)
        {
            if (form == null)
                return;

            form.BackColor = FormBackColor;
            form.ForeColor = TextForeColor;
            form.Font = NormalFont;

            ApplyToControls(form);
        }


        //==================================================
        // Apply recursively to all controls
        //==================================================

        private static void ApplyToControls(Control parent)
        {
            foreach (Control control in parent.Controls)
            {
                if (control.Name == "lblSubTitle" && control is Label lblSubTitle)
                {
                    // Subtitle specific properties
                    lblSubTitle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
                    lblSubTitle.ForeColor = ColorTranslator.FromHtml("#64748B");
                    lblSubTitle.BackColor = Color.Transparent;
                    lblSubTitle.AutoSize = true;
                }
                else
                {
                    ApplyControl(control);
                }

                // DataGridView internals are not child controls we style
                if (control.HasChildren && !(control is DataGridView))
                {
                    ApplyToControls(control);
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
                ApplyLabel((Label)control);
            }

            // ---------------------------------------------
            // TextBox
            // ---------------------------------------------
            else if (control is TextBox)
            {
                ApplyTextBox((TextBox)control);
            }

            // ---------------------------------------------
            // ComboBox
            // ---------------------------------------------
            else if (control is ComboBox)
            {
                ApplyComboBox((ComboBox)control);
            }

            // ---------------------------------------------
            // Button
            // ---------------------------------------------
            else if (control is Button)
            {
                ApplyButtonDefaults((Button)control);
            }

            // ---------------------------------------------
            // MenuStrip
            // ---------------------------------------------
            else if (control is MenuStrip)
            {
                ApplyMenuStrip((MenuStrip)control);
            }

            // ---------------------------------------------
            // CheckBox
            // ---------------------------------------------
            else if (control is CheckBox)
            {
                CheckBox chk = (CheckBox)control;
                chk.Font = NormalFont;
                chk.ForeColor = TextForeColor;
                chk.BackColor = Color.Transparent;
                chk.Cursor = Cursors.Hand;
            }

            // ---------------------------------------------
            // RadioButton
            // ---------------------------------------------
            else if (control is RadioButton)
            {
                RadioButton rb = (RadioButton)control;
                rb.Font = NormalFont;
                rb.ForeColor = TextForeColor;
                rb.BackColor = Color.Transparent;
                rb.Cursor = Cursors.Hand;
            }

            // ---------------------------------------------
            // DateTimePicker
            // ---------------------------------------------
            else if (control is DateTimePicker)
            {
                DateTimePicker dtp = (DateTimePicker)control;
                dtp.Font = NormalFont;
                dtp.ForeColor = TextForeColor;
                dtp.BackColor = Color.White;
                dtp.Format = DateTimePickerFormat.Short;
            }

            // ---------------------------------------------
            // NumericUpDown
            // ---------------------------------------------
            else if (control is NumericUpDown)
            {
                ApplyNumeric((NumericUpDown)control);
            }

            // ---------------------------------------------
            // DataGridView
            // ---------------------------------------------
            else if (control is DataGridView)
            {
                StyleGrid((DataGridView)control);
            }

            // ---------------------------------------------
            // GroupBox
            // ---------------------------------------------
            else if (control is GroupBox)
            {
                GroupBox grp = (GroupBox)control;
                grp.Font = GroupBoxFont;
                grp.ForeColor = TextForeColor;
                grp.BackColor = PanelBackColor;
            }

            // ---------------------------------------------
            // Panel (also FlowLayoutPanel / TableLayoutPanel)
            // Panels with an intentional custom color are left alone.
            // ---------------------------------------------
            else if (control is Panel)
            {
                Panel panel = (Panel)control;
                panel.ForeColor = TextForeColor;

                if (panel.Parent != null &&
                    (panel.BackColor == panel.Parent.BackColor ||
                     panel.BackColor == SystemColors.Control))
                {
                    panel.BackColor = PanelBackColor;
                }
            }

            // ---------------------------------------------
            // TabControl
            // ---------------------------------------------
            else if (control is TabControl)
            {
                control.Font = NormalFont;
            }

            // ---------------------------------------------
            // TabPage
            // ---------------------------------------------
            else if (control is TabPage)
            {
                TabPage page = (TabPage)control;
                page.BackColor = PanelBackColor;
                page.Font = NormalFont;
            }
        }


        //==================================================
        // Label
        //==================================================

        private static void ApplyLabel(Label lbl)
        {
            lbl.BackColor = Color.Transparent;
            lbl.ForeColor = TextForeColor;

            string name = (lbl.Name ?? "").ToLowerInvariant();

            // Bold labels (titles, balance "cards") keep their own size and
            // weight so small stat boxes don't overflow - only the typeface changes.
            if (lbl.Font != null && lbl.Font.Bold)
            {
                lbl.Font = new Font("Segoe UI", lbl.Font.Size, lbl.Font.Style);
            }
            else
            {
                lbl.Font = LabelFont;
            }

            if (name.EndsWith("balance"))
            {
                lbl.ForeColor = Primary;
            }
        }


        //==================================================
        // TextBox
        //==================================================

        private static void ApplyTextBox(TextBox txt)
        {
            txt.Font = NormalFont;
            txt.ForeColor = TextForeColor;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Margin = new Padding(3);

            Action refresh = delegate
            {
                // Status badges (StyleStatusBox) keep their own colors
                object marker;
                if (StatusBoxes.TryGetValue(txt, out marker))
                    return;

                if (txt.ReadOnly || !txt.Enabled)
                {
                    txt.BackColor = ReadOnlyBack;
                    txt.ForeColor = ReadOnlyFore;
                }
                else
                {
                    txt.BackColor = TextBackColor;
                    txt.ForeColor = TextForeColor;
                }
            };

            if (FirstTime(txt))
            {
                txt.ReadOnlyChanged += delegate { refresh(); };
                txt.EnabledChanged += delegate { refresh(); };

                // White while typing so the active field is obvious
                txt.Enter += delegate
                {
                    if (!txt.ReadOnly && txt.Enabled)
                        txt.BackColor = FocusBackColor;
                };
                txt.Leave += delegate { refresh(); };
            }

            refresh();
        }


        //==================================================
        // ComboBox
        //==================================================

        private static void ApplyComboBox(ComboBox cmb)
        {
            cmb.Font = NormalFont;
            cmb.FlatStyle = FlatStyle.Flat;

            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.IntegralHeight = true;
            cmb.Margin = new Padding(3);

            // 1. AutoCompleteSource ko PEHLE set karein (Error se bachne ke liye)
            cmb.AutoCompleteSource = AutoCompleteSource.ListItems;
            // 2. AutoCompleteMode ko BAAD me set karein
            cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            Action refresh = delegate
            {
                if (cmb.Enabled)
                {
                    cmb.BackColor = TextBackColor;
                    cmb.ForeColor = TextForeColor;
                }
                else
                {
                    cmb.BackColor = ReadOnlyBack;
                    cmb.ForeColor = ReadOnlyFore;
                }
            };

            refresh();

            if (!FirstTime(cmb))
                return;

            cmb.EnabledChanged += delegate { refresh(); };

            // White while the dropdown has focus, tinted otherwise
            cmb.Enter += delegate
            {
                if (cmb.Enabled)
                    cmb.BackColor = FocusBackColor;
            };
            cmb.Leave += delegate { refresh(); };

            // Dropdown width auto-adjust logic
            cmb.DropDown += (s, e) =>
            {
                ComboBox cb = s as ComboBox;
                if (cb == null || cb.Items.Count == 0) return;

                int maxWidth = cb.Width;
                int scrollBarWidth = SystemInformation.VerticalScrollBarWidth;

                using (Graphics g = cb.CreateGraphics())
                {
                    foreach (var item in cb.Items)
                    {
                        string text = cb.GetItemText(item);
                        int itemWidth = (int)g.MeasureString(text, cb.Font).Width + scrollBarWidth + 10;
                        if (itemWidth > maxWidth)
                        {
                            maxWidth = itemWidth;
                        }
                    }
                }
                cb.DropDownWidth = maxWidth;
            };
        }


        //==================================================
        // NumericUpDown
        //==================================================

        private static void ApplyNumeric(NumericUpDown num)
        {
            num.Font = NormalFont;
            num.ForeColor = TextForeColor;
            num.BackColor = TextBackColor;

            if (!FirstTime(num))
                return;

            num.Enter += delegate { num.BackColor = FocusBackColor; };
            num.Leave += delegate { num.BackColor = TextBackColor; };
        }


        //==================================================
        // MenuStrip
        //==================================================

        private static void ApplyMenuStrip(MenuStrip menuStrip)
        {
            menuStrip.Font = MenuFont;

            menuStrip.BackColor = MenuBackColor;
            menuStrip.ForeColor = Color.White;

            menuStrip.RenderMode = ToolStripRenderMode.Professional;
            menuStrip.Renderer = new ModernMenuRenderer();

            menuStrip.Padding = new Padding(6, 3, 6, 3);

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

            item.Padding = new Padding(8, 5, 8, 5);

            foreach (ToolStripItem child in item.DropDownItems)
            {
                if (child is ToolStripMenuItem subItem)
                {
                    subItem.Font = MenuFont;
                    subItem.ForeColor = TextForeColor;
                    subItem.BackColor = Color.White;
                    subItem.Padding = new Padding(10, 5, 10, 5);

                    ApplyMenuItem(subItem);

                    // ApplyMenuItem sets white text for top-level items; sub items stay dark
                    subItem.ForeColor = TextForeColor;
                    subItem.BackColor = Color.White;
                }
            }
        }


        //==================================================
        // DataGridView  (public so forms can re-style a grid created later)
        //==================================================

        public static void StyleGrid(DataGridView dgv)
        {
            if (dgv == null)
                return;

            dgv.Font = NormalFont;
            dgv.BackgroundColor = FormBackColor;
            dgv.BorderStyle = BorderStyle.None;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = GridLineColor;

            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Header
            dgv.ColumnHeadersDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = HeaderColor,
                    ForeColor = Color.White,
                    Font = GridHeaderFont,
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    SelectionBackColor = HeaderColor,
                    SelectionForeColor = Color.White,
                    Padding = new Padding(8, 6, 8, 6)
                };

            // Rows
            dgv.DefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = Color.White,
                    ForeColor = TextForeColor,
                    SelectionBackColor = RowSelect,
                    SelectionForeColor = TextForeColor,
                    Font = NormalFont,
                    Padding = new Padding(8, 6, 8, 6)
                };

            // Alternating rows
            dgv.AlternatingRowsDefaultCellStyle =
                new DataGridViewCellStyle
                {
                    BackColor = RowAlt,
                    ForeColor = TextForeColor,
                    SelectionBackColor = RowSelect,
                    SelectionForeColor = TextForeColor
                };

            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.MultiSelect = false;
            dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgv.RowTemplate.Height = 31;
            dgv.ColumnHeadersHeight = 34;
        }


        //==================================================
        // Status badge: Draft = amber, Approved = blue,
        // Issued = green, Cancelled = red
        //==================================================

        public static void StyleStatusBox(TextBox txt, int statusId)
        {
            if (txt == null)
                return;

            object marker;
            if (!StatusBoxes.TryGetValue(txt, out marker))
                StatusBoxes.Add(txt, new object());

            Color back, fore;

            switch (statusId)
            {
                case 1: back = ColorTranslator.FromHtml("#FEF3C7"); fore = ColorTranslator.FromHtml("#92400E"); break;
                case 2: back = ColorTranslator.FromHtml("#DBEAFE"); fore = ColorTranslator.FromHtml("#1E40AF"); break;
                case 3: back = ColorTranslator.FromHtml("#DCFCE7"); fore = ColorTranslator.FromHtml("#166534"); break;
                case 4: back = ColorTranslator.FromHtml("#FEE2E2"); fore = ColorTranslator.FromHtml("#991B1B"); break;
                default: back = ReadOnlyBack; fore = TextForeColor; break;
            }

            txt.BackColor = back;
            txt.ForeColor = fore;
            txt.Font = ButtonFont;
            txt.TextAlign = HorizontalAlignment.Center;
        }


        //==================================================
        // Buttons
        // Color is chosen from the button's name:
        //   Close / Cancel / Refresh / Exit / Clear / Reset  -> white with border
        //   Approve                                          -> green
        //   Delete / Remove                                  -> red
        //   Assign / Print / Report / Performa / Export      -> slate
        //   everything else (Save, Issue, Add, Search ...)   -> blue
        //==================================================

        private static void ApplyButtonDefaults(Button btn)
        {
            if (btn == null) return;

            string n = (btn.Name ?? "").ToLowerInvariant();

            Color back, hover, pressed, fore = Color.White;
            bool secondary = false;

            if (n.Contains("close") || n.Contains("cancel") || n.Contains("exit") ||
                n.Contains("refresh") || n.Contains("clear") || n.Contains("reset"))
            {
                secondary = true;
                back = Color.White;
                hover = FormBackColor;
                pressed = DisabledBack;
                fore = TextForeColor;
            }
            else if (n.Contains("approve"))
            {
                back = Success;
                hover = SuccessHover;
                pressed = SuccessHover;
            }
            else if (n.Contains("delete") || n.Contains("remove"))
            {
                back = Danger;
                hover = DangerHover;
                pressed = DangerHover;
            }
            else if (n.Contains("assign") || n.Contains("print") || n.Contains("report") ||
                     n.Contains("performa") || n.Contains("export"))
            {
                back = Slate;
                hover = SlateHover;
                pressed = SlateHover;
            }
            else
            {
                back = Primary;
                hover = PrimaryHover;
                pressed = PrimaryPressed;
            }

            btn.Font = ButtonFont;
            btn.FlatStyle = FlatStyle.Flat;
            btn.UseVisualStyleBackColor = false;
            btn.Padding = new Padding(10, 5, 10, 5);

            btn.FlatAppearance.BorderSize = secondary ? 1 : 0;
            btn.FlatAppearance.BorderColor = BorderColor;
            btn.FlatAppearance.MouseOverBackColor = hover;
            btn.FlatAppearance.MouseDownBackColor = pressed;

            Action refresh = delegate
            {
                if (btn.Enabled)
                {
                    btn.BackColor = back;
                    btn.ForeColor = fore;
                    btn.Cursor = Cursors.Hand;
                }
                else
                {
                    btn.BackColor = DisabledBack;
                    btn.ForeColor = DisabledFore;
                    btn.Cursor = Cursors.Default;
                }
            };

            // Buttons such as Issue / Save are enabled & disabled by code - keep the look in sync
            if (FirstTime(btn))
            {
                btn.EnabledChanged += delegate { refresh(); };
            }

            refresh();
        }
    }
}