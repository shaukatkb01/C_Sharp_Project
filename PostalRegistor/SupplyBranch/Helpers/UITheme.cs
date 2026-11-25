
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using SupplyBranch.Controls;

namespace SupplyBranch.Helpers
{
    /// <summary>
    /// Application-wide theme adapter for the module forms.
    /// Usage: UITheme.Apply(this) in a form's Load event.
    /// Keeps existing form layouts intact while providing modern, consistent
    /// light/dark styling across controls.
    /// </summary>
    public static class UITheme
    {
        private static readonly Font NormalFont = ThemeFonts.Body;
        private static readonly Font LabelFont = ThemeFonts.Body;
        private static readonly Font ButtonFont = ThemeFonts.Button;
        private static readonly Font GridHeaderFont = ThemeFonts.BodyBold;
        private static readonly Font MenuFont = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
        private static readonly Font GroupBoxFont = new Font("Segoe UI Semibold", 10F, FontStyle.Regular, GraphicsUnit.Point);
        private static readonly Font SubtitleFont = new Font("Segoe UI", 10F, FontStyle.Bold, GraphicsUnit.Point);

        private static readonly Dictionary<string, Font> BoldLabelFonts =
            new Dictionary<string, Font>(StringComparer.Ordinal);

        private enum Role
        {
            None,
            Label,
            LabelSubtitle,
            LabelBalance,
            TextInput,
            Combo,
            Numeric,
            Date,
            Button,
            PanelSurface,
            PanelCustom,
            Group,
            Grid,
            Check,
            Radio,
            RichText,
            CheckedList,
            TabPage,
            Menu,
            Status,
            ToolStripBar
        }

        private sealed class RoleHolder
        {
            public Role Value;
        }

        private sealed class StatusHolder
        {
            public int StatusId;
        }

        private sealed class FormHook
        {
            public EventHandler ThemeChangedHandler;
            public EventHandler ShownHandler;
            public EventHandler DisposedHandler;
        }

        private static readonly ConditionalWeakTable<Control, RoleHolder> Roles =
            new ConditionalWeakTable<Control, RoleHolder>();

        private static readonly ConditionalWeakTable<Control, object> Styled =
            new ConditionalWeakTable<Control, object>();

        private static readonly ConditionalWeakTable<Control, StatusHolder> StatusBoxes =
            new ConditionalWeakTable<Control, StatusHolder>();

        private static readonly ConditionalWeakTable<Form, FormHook> HookedForms =
            new ConditionalWeakTable<Form, FormHook>();

        private static ThemePalette P
        {
            get { return AppTheme.P; }
        }

        private static bool FirstTime(Control c)
        {
            object o;
            if (Styled.TryGetValue(c, out o))
                return false;

            Styled.Add(c, new object());
            return true;
        }

        private static Color C(string html)
        {
            return ColorUtil.FromHexSafe(html, Color.Gray);
        }

        private static Font GetBoldLabelFont(float size, FontStyle existingStyle)
        {
            FontStyle style = existingStyle | FontStyle.Bold;
            string key = size.ToString("0.##") + ":" + ((int)style);

            Font cached;
            if (BoldLabelFonts.TryGetValue(key, out cached))
                return cached;

            cached = new Font("Segoe UI", size, style, GraphicsUnit.Point);
            BoldLabelFonts[key] = cached;
            return cached;
        }

        /// <summary>
        /// Cells the user can type into (used by editable supply grids).
        /// </summary>
        public static Color EditableCellColor
        {
            get { return AppTheme.IsDark ? C("#3A3212") : C("#FEF9C3"); }
        }

        /// <summary>
        /// Read-only / locked grid cells.
        /// </summary>
        public static Color LockedCellColor
        {
            get { return AppTheme.IsDark ? P.SurfaceAlt : C("#EEF0F3"); }
        }

        public static void Apply(Form form)
        {
            if (form == null)
                return;

            ApplyForm(form);

            FormHook existingHook;
            if (HookedForms.TryGetValue(form, out existingHook))
                return;

            FormHook hook = new FormHook();
            hook.ThemeChangedHandler = delegate
            {
                if (form.IsDisposed)
                {
                    AppTheme.ThemeChanged -= hook.ThemeChangedHandler;
                    return;
                }

                ApplyForm(form);
            };

            hook.ShownHandler = delegate
            {
                if (!form.IsDisposed)
                    ApplyScrollbars(form);
            };

            hook.DisposedHandler = delegate
            {
                AppTheme.ThemeChanged -= hook.ThemeChangedHandler;
                form.Shown -= hook.ShownHandler;
                form.Disposed -= hook.DisposedHandler;
            };

            HookedForms.Add(form, hook);
            AppTheme.ThemeChanged += hook.ThemeChangedHandler;
            form.Shown += hook.ShownHandler;
            form.Disposed += hook.DisposedHandler;
        }

        private static void ApplyForm(Form form)
        {
            ThemePalette palette = P;
            form.BackColor = palette.Background;
            form.ForeColor = palette.TextPrimary;
            form.Font = NormalFont;

            ApplyToControls(form, palette);
            ApplyScrollbars(form);
        }

        private static void ApplyScrollbars(Control parent)
        {
            if (parent == null || parent.IsDisposed)
                return;

            if (parent is Form || parent is DataGridView || parent is TextBoxBase || parent is ListBox)
                AppTheme.ApplyScrollbarTheme(parent);

            foreach (Control child in parent.Controls)
                ApplyScrollbars(child);
        }

        private static void ApplyToControls(Control parent, ThemePalette palette)
        {
            foreach (Control control in parent.Controls)
            {
                ApplyControl(control, palette);

                if (control.HasChildren && !(control is DataGridView) && !(control is ToolStrip))
                    ApplyToControls(control, palette);
            }
        }

        private static void ApplyControl(Control control, ThemePalette palette)
        {
            if (control is Label)
            {
                ApplyLabel((Label)control, palette);
            }
            else if (control is TextBox)
            {
                ApplyTextBox((TextBox)control, palette);
            }
            else if (control is ComboBox)
            {
                ApplyComboBox((ComboBox)control, palette);
            }
            else if (control is NumericUpDown)
            {
                ApplyNumeric((NumericUpDown)control, palette);
            }
            else if (control is DateTimePicker)
            {
                DateTimePicker dtp = (DateTimePicker)control;
                dtp.Font = NormalFont;
                dtp.ForeColor = AppTheme.IsDark ? C("#0F172A") : palette.TextPrimary;
                dtp.BackColor = palette.Surface;
            }
            else if (control is Button)
            {
                ApplyButtonDefaults((Button)control, palette);
            }
            else if (control is CheckBox)
            {
                CheckBox chk = (CheckBox)control;
                chk.Font = NormalFont;
                chk.ForeColor = palette.TextPrimary;
                chk.BackColor = Color.Transparent;
                chk.Cursor = Cursors.Hand;
            }
            else if (control is RadioButton)
            {
                RadioButton rb = (RadioButton)control;
                rb.Font = NormalFont;
                rb.ForeColor = palette.TextPrimary;
                rb.BackColor = Color.Transparent;
                rb.Cursor = Cursors.Hand;
            }
            else if (control is DataGridView)
            {
                StyleGrid((DataGridView)control);
            }
            else if (control is GroupBox)
            {
                GroupBox grp = (GroupBox)control;
                grp.Font = GroupBoxFont;
                grp.ForeColor = palette.TextPrimary;
                grp.BackColor = palette.Surface;
            }
            else if (control is TabPage)
            {
                TabPage page = (TabPage)control;
                page.BackColor = palette.Surface;
                page.ForeColor = palette.TextPrimary;
                page.Font = NormalFont;
            }
            else if (control is RichTextBox)
            {
                RichTextBox rtb = (RichTextBox)control;
                rtb.Font = NormalFont;
                rtb.ForeColor = palette.TextPrimary;
                rtb.BackColor = palette.Surface;
                rtb.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is CheckedListBox)
            {
                CheckedListBox clb = (CheckedListBox)control;
                clb.Font = NormalFont;
                clb.ForeColor = palette.TextPrimary;
                clb.BackColor = palette.SurfaceAlt;
                clb.BorderStyle = BorderStyle.FixedSingle;
            }
            else if (control is MenuStrip)
            {
                ApplyMenuStrip((MenuStrip)control, palette);
            }
            else if (control is StatusStrip)
            {
                StatusStrip strip = (StatusStrip)control;
                strip.BackColor = palette.Surface;
                strip.ForeColor = palette.TextPrimary;
                strip.Font = NormalFont;
                strip.SizingGrip = false;
            }
            else if (control is ToolStrip)
            {
                ToolStrip strip = (ToolStrip)control;
                strip.BackColor = palette.Surface;
                strip.ForeColor = palette.TextPrimary;
                strip.Font = NormalFont;
            }
            else if (control is Panel)
            {
                ApplyPanel((Panel)control, palette);
            }
        }

        private static Role GetRole(Control control)
        {
            RoleHolder holder;
            if (Roles.TryGetValue(control, out holder))
                return holder.Value;

            Role role = Role.None;

            Label lbl = control as Label;
            if (lbl != null)
            {
                string name = (lbl.Name ?? string.Empty).ToLowerInvariant();

                if (name.EndsWith("balance"))
                    role = Role.LabelBalance;
                else if (name == "lblsubtitle")
                    role = Role.LabelSubtitle;
                else
                    role = Role.Label;
            }
            else if (control is TextBox)
            {
                role = Role.TextInput;
            }
            else if (control is ComboBox)
            {
                role = Role.Combo;
            }
            else if (control is NumericUpDown)
            {
                role = Role.Numeric;
            }
            else if (control is Button)
            {
                role = Role.Button;
            }
            else if (control is DataGridView)
            {
                role = Role.Grid;
            }
            else if (control is GroupBox)
            {
                role = Role.Group;
            }
            else if (control is TabPage)
            {
                role = Role.TabPage;
            }
            else if (control is MenuStrip)
            {
                role = Role.Menu;
            }
            else if (control is StatusStrip)
            {
                role = Role.Status;
            }
            else if (control is ToolStrip)
            {
                role = Role.ToolStripBar;
            }
            else if (control is Panel)
            {
                Panel panel = (Panel)control;
                role = IsNeutralSurface(panel.BackColor, panel.Parent)
                    ? Role.PanelSurface
                    : Role.PanelCustom;
            }

            holder = new RoleHolder { Value = role };
            Roles.Add(control, holder);
            return role;
        }

        private static bool IsNeutralSurface(Color color, Control parent)
        {
            if (color == Color.Transparent || color == Color.White || color == SystemColors.Control)
                return true;

            if (parent != null && color == parent.BackColor)
                return true;

            int max = Math.Max(color.R, Math.Max(color.G, color.B));
            int min = Math.Min(color.R, Math.Min(color.G, color.B));

            return color.GetBrightness() > 0.82F && (max - min) < 48;
        }

        private static void ApplyLabel(Label lbl, ThemePalette palette)
        {
            Role role = GetRole(lbl);
            lbl.BackColor = Color.Transparent;

            if (role == Role.LabelSubtitle)
            {
                lbl.Font = SubtitleFont;
                lbl.ForeColor = palette.TextSecondary;
                lbl.AutoSize = true;
                return;
            }

            lbl.ForeColor = (role == Role.LabelBalance) ? palette.Primary : palette.TextPrimary;

            if (lbl.Font != null && lbl.Font.Bold)
            {
                lbl.Font = GetBoldLabelFont(lbl.Font.Size > 0 ? lbl.Font.Size : 9.5F, lbl.Font.Style);
            }
            else
            {
                lbl.Font = LabelFont;
            }
        }

        private static void ApplyTextBox(TextBox txt, ThemePalette palette)
        {
            txt.Font = NormalFont;
            txt.BorderStyle = BorderStyle.FixedSingle;

            StatusHolder status;
            if (StatusBoxes.TryGetValue(txt, out status))
            {
                ApplyStatusColors(txt, status.StatusId, palette);
                return;
            }

            RefreshTextBox(txt, palette);

            if (!FirstTime(txt))
                return;

            txt.ReadOnlyChanged += delegate { RefreshTextBox(txt, P); };
            txt.EnabledChanged += delegate { RefreshTextBox(txt, P); };
            txt.Enter += delegate { RefreshTextBox(txt, P); };
            txt.Leave += delegate { RefreshTextBox(txt, P); };
        }

        private static void RefreshTextBox(TextBox txt, ThemePalette palette)
        {
            StatusHolder status;
            if (StatusBoxes.TryGetValue(txt, out status))
                return;

            if (!txt.Enabled || txt.ReadOnly)
            {
                txt.BackColor = palette.SurfaceAlt;
                txt.ForeColor = palette.TextMuted;
            }
            else if (txt.Focused)
            {
                txt.BackColor = palette.Surface;
                txt.ForeColor = palette.TextPrimary;
            }
            else
            {
                txt.BackColor = palette.SurfaceAlt;
                txt.ForeColor = palette.TextPrimary;
            }
        }

        private static void ApplyComboBox(ComboBox cmb, ThemePalette palette)
        {
            cmb.Font = NormalFont;
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.DropDownStyle = ComboBoxStyle.DropDownList;
            cmb.IntegralHeight = true;
            cmb.Margin = new Padding(3);
            cmb.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmb.AutoCompleteMode = AutoCompleteMode.SuggestAppend;

            RefreshCombo(cmb, palette);

            if (!FirstTime(cmb))
                return;

            cmb.EnabledChanged += delegate { RefreshCombo(cmb, P); };
            cmb.Enter += delegate
            {
                if (cmb.Enabled)
                    cmb.BackColor = P.Surface;
            };
            cmb.Leave += delegate { RefreshCombo(cmb, P); };

            cmb.DropDown += delegate
            {
                if (cmb.Items.Count == 0)
                    return;

                int maxWidth = cmb.Width;
                int scrollBarWidth = SystemInformation.VerticalScrollBarWidth;

                using (Graphics g = cmb.CreateGraphics())
                {
                    foreach (object item in cmb.Items)
                    {
                        string text = cmb.GetItemText(item);
                        int itemWidth = (int)g.MeasureString(text, cmb.Font).Width + scrollBarWidth + 10;
                        if (itemWidth > maxWidth)
                            maxWidth = itemWidth;
                    }
                }

                cmb.DropDownWidth = maxWidth;
            };
        }

        private static void RefreshCombo(ComboBox cmb, ThemePalette palette)
        {
            cmb.BackColor = palette.SurfaceAlt;
            cmb.ForeColor = cmb.Enabled ? palette.TextPrimary : palette.TextMuted;
        }

        private static void ApplyNumeric(NumericUpDown num, ThemePalette palette)
        {
            num.Font = NormalFont;
            num.ForeColor = palette.TextPrimary;
            num.BackColor = palette.SurfaceAlt;

            if (!FirstTime(num))
                return;

            num.Enter += delegate { num.BackColor = P.Surface; };
            num.Leave += delegate { num.BackColor = P.SurfaceAlt; };
        }

        private class ThemedMenuRenderer : ToolStripProfessionalRenderer
        {
            public ThemedMenuRenderer()
                : base(new ThemedColorTable())
            {
            }
        }

        private class ThemedColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected { get { return P.Primary; } }
            public override Color MenuItemSelectedGradientBegin { get { return P.Primary; } }
            public override Color MenuItemSelectedGradientEnd { get { return P.PrimaryHover; } }
            public override Color MenuItemBorder { get { return P.Primary; } }
            public override Color MenuBorder { get { return P.Border; } }
            public override Color ToolStripDropDownBackground { get { return P.Surface; } }
            public override Color ImageMarginGradientBegin { get { return P.Surface; } }
            public override Color ImageMarginGradientMiddle { get { return P.Surface; } }
            public override Color ImageMarginGradientEnd { get { return P.Surface; } }
        }

        private static void ApplyMenuStrip(MenuStrip menuStrip, ThemePalette palette)
        {
            menuStrip.Font = MenuFont;
            menuStrip.BackColor = palette.Sidebar;
            menuStrip.ForeColor = palette.SidebarText;
            menuStrip.RenderMode = ToolStripRenderMode.Professional;
            menuStrip.Renderer = new ThemedMenuRenderer();
            menuStrip.Padding = new Padding(6, 3, 6, 3);

            foreach (ToolStripItem item in menuStrip.Items)
            {
                ToolStripMenuItem menuItem = item as ToolStripMenuItem;
                if (menuItem != null)
                    ApplyMenuItem(menuItem, true, palette);
            }
        }

        private static void ApplyMenuItem(ToolStripMenuItem item, bool topLevel, ThemePalette palette)
        {
            item.Font = MenuFont;
            item.ForeColor = topLevel ? palette.SidebarText : palette.TextPrimary;
            item.BackColor = topLevel ? palette.Sidebar : palette.Surface;
            item.Padding = topLevel ? new Padding(8, 5, 8, 5) : new Padding(10, 5, 10, 5);

            foreach (ToolStripItem child in item.DropDownItems)
            {
                ToolStripMenuItem subItem = child as ToolStripMenuItem;
                if (subItem != null)
                    ApplyMenuItem(subItem, false, palette);
            }
        }

        /// <summary>
        /// Aligns DataGridView styling with GridStyler so forms and custom grids share one look.
        /// </summary>
        public static void StyleGrid(DataGridView dgv)
        {
            if (dgv == null)
                return;

            GridStyler.Apply(dgv);
        }

        public static void StyleStatusBox(TextBox txt, int statusId)
        {
            if (txt == null)
                return;

            StatusHolder holder;
            if (!StatusBoxes.TryGetValue(txt, out holder))
            {
                holder = new StatusHolder();
                StatusBoxes.Add(txt, holder);
            }

            holder.StatusId = statusId;
            ApplyStatusColors(txt, statusId, P);
        }

        private static void ApplyStatusColors(TextBox txt, int statusId, ThemePalette palette)
        {
            Color back, fore;

            if (AppTheme.IsDark)
            {
                switch (statusId)
                {
                    case 1: back = C("#3A2A08"); fore = C("#FBBF24"); break;
                    case 2: back = C("#10294D"); fore = C("#93C5FD"); break;
                    case 3: back = C("#0E2E1B"); fore = C("#86EFAC"); break;
                    case 4: back = C("#3A1518"); fore = C("#FCA5A5"); break;
                    default: back = palette.SurfaceAlt; fore = palette.TextPrimary; break;
                }
            }
            else
            {
                switch (statusId)
                {
                    case 1: back = C("#FEF3C7"); fore = C("#92400E"); break;
                    case 2: back = C("#DBEAFE"); fore = C("#1E40AF"); break;
                    case 3: back = C("#DCFCE7"); fore = C("#166534"); break;
                    case 4: back = C("#FEE2E2"); fore = C("#991B1B"); break;
                    default: back = palette.Background; fore = palette.TextPrimary; break;
                }
            }

            txt.BackColor = back;
            txt.ForeColor = fore;
            txt.Font = ButtonFont;
            txt.TextAlign = HorizontalAlignment.Center;
        }

        private static void ApplyPanel(Panel panel, ThemePalette palette)
        {
            Role role = GetRole(panel);
            panel.ForeColor = palette.TextPrimary;

            if (role == Role.PanelSurface)
                panel.BackColor = palette.Surface;
        }

        private static void ApplyButtonDefaults(Button btn, ThemePalette palette)
        {
            if (btn == null)
                return;

            string n = (btn.Name ?? string.Empty).ToLowerInvariant();
            string t = (btn.Text ?? string.Empty).ToLowerInvariant();

            Color back, hover, pressed, fore = palette.OnPrimary;
            bool secondary = false;

            if (n.Contains("close") || n.Contains("cancel") || n.Contains("exit") ||
                n.Contains("refresh") || n.Contains("clear") || n.Contains("reset") ||
                t.Contains("close") || t.Contains("cancel") || t.Contains("exit") ||
                t.Contains("refresh") || t.Contains("clear") || t.Contains("reset"))
            {
                secondary = true;
                back = palette.SurfaceAlt;
                hover = palette.Surface;
                pressed = palette.Border;
                fore = palette.TextPrimary;
            }
            else if (n.Contains("approve"))
            {
                back = palette.Success;
                hover = ColorUtil.Darken(palette.Success, 0.12F);
                pressed = ColorUtil.Darken(palette.Success, 0.22F);
            }
            else if (n.Contains("delete") || n.Contains("remove"))
            {
                back = palette.Danger;
                hover = ColorUtil.Darken(palette.Danger, 0.12F);
                pressed = ColorUtil.Darken(palette.Danger, 0.22F);
            }
            else if (n.Contains("assign") || n.Contains("print") || n.Contains("report") ||
                     n.Contains("performa") || n.Contains("export"))
            {
                back = AppTheme.IsDark ? C("#3B4A63") : C("#334155");
                hover = AppTheme.IsDark ? C("#465878") : C("#1E293B");
                pressed = AppTheme.IsDark ? C("#2C384C") : C("#0F172A");
            }
            else
            {
                back = palette.Primary;
                hover = palette.PrimaryHover;
                pressed = palette.PrimaryPressed;
            }

            btn.Font = ButtonFont;
            btn.FlatStyle = FlatStyle.Flat;
            btn.UseVisualStyleBackColor = false;
            btn.Padding = new Padding(10, 5, 10, 5);

            btn.FlatAppearance.BorderSize = secondary ? 1 : 0;
            btn.FlatAppearance.BorderColor = palette.Border;
            btn.FlatAppearance.MouseOverBackColor = hover;
            btn.FlatAppearance.MouseDownBackColor = pressed;

            if (btn.Enabled)
            {
                btn.BackColor = back;
                btn.ForeColor = fore;
                btn.Cursor = Cursors.Hand;
            }
            else
            {
                btn.BackColor = palette.SurfaceAlt;
                btn.ForeColor = palette.TextMuted;
                btn.Cursor = Cursors.Default;
            }

            if (FirstTime(btn))
            {
                btn.EnabledChanged += delegate { UpdateButtonEnabledState(btn, back, fore, P); };
            }
        }

        private static void UpdateButtonEnabledState(Button btn, Color enabledBack, Color enabledFore, ThemePalette palette)
        {
            if (btn.IsDisposed)
                return;

            if (btn.Enabled)
            {
                btn.BackColor = enabledBack;
                btn.ForeColor = enabledFore;
                btn.Cursor = Cursors.Hand;
            }
            else
            {
                btn.BackColor = palette.SurfaceAlt;
                btn.ForeColor = palette.TextMuted;
                btn.Cursor = Cursors.Default;
            }
        }
    }
}
