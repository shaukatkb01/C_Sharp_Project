using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SupplyBranch.Helpers
{
    internal class FormStyleHelper
    {
        /// <summary>
        /// WinForms Modern UI Engine & Style Applicator.
        /// Usage in Form_Load:
        ///   WinFormsModernizer.Apply(this);
        /// </summary>
        public static class WinFormsModernizer
        {
            // ==========================================
            // THEME COLORS & CONFIGURATION
            // Edit these values to customize the look and feel.
            // ==========================================
            public static class Theme
            {
                public static Color AccentColor = ColorTranslator.FromHtml("#3E7CB1");
                public static Color BackColor = ColorTranslator.FromHtml("#12181F");
                public static Color SurfaceColor = ColorTranslator.FromHtml("#1C2530");
                public static Color TextColor = ColorTranslator.FromHtml("#EDEFF2");
                public static Color SecondaryTextColor = ColorTranslator.FromHtml("#8A94A3");
                public static Color BorderColor = ColorTranslator.FromHtml("#2A3542");
                public static Font MainFont = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                public static int CornerRadius = 8;
                public static bool IsDarkTheme = true;

                // Feature toggles
                public static bool EnableFormFadeIn = true;
                public static bool EnableDwmRoundedCorners = true;
                public static bool EnableHoverAnimations = true;
                public static bool EnableFocusGlow = true;
            }

            // ==========================================
            // WIN32 NATIVE DWM API (Windows 11 Rounded Corners & Dark Titlebar)
            // ==========================================
            [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, SetLastError = true)]
            private static extern int DwmSetWindowAttribute(IntPtr hwnd, int attribute, ref int pvAttribute, int cbAttribute);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)] string lParam);

            private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
            private const int DWMWA_WINDOW_CORNER_PREFERENCE = 33;
            private const int EM_SETCUEBANNER = 0x1501;

            public enum DWM_WINDOW_CORNER_PREFERENCE
            {
                DWMSC_DEFAULT = 0,
                DWMSC_DONOTROUND = 1,
                DWMSC_ROUND = 2,
                DWMSC_ROUNDSMALL = 3
            }

            /// <summary>
            /// Call this inside Form_Load to modernize all controls on the form automatically.
            /// </summary>
            /// <param name="form">The target form (usually "this").</param>
            public static void Apply(Form form)
            {
                if (form == null) return;

                // 1. Enable double buffering to prevent UI flicker
                EnableDoubleBuffering(form);

                // 2. Apply base form styling
                form.BackColor = Theme.BackColor;
                form.ForeColor = Theme.TextColor;
                form.Font = Theme.MainFont;

                // 3. Apply Windows 11 native dark titlebar & rounded corners
                ApplyNativeWindowAttributes(form);

                // 4. Recursively style all child controls
                StyleControlRecursive(form);

                // 5. Smooth fade-in load animation
                if (Theme.EnableFormFadeIn)
                {
                    AnimateFormFadeIn(form);
                }
            }

            /// <summary>
            /// Recursively enables double buffering to eliminate render flicker.
            /// </summary>
            public static void EnableDoubleBuffering(Control control)
            {
                typeof(Control).InvokeMember(
                    "DoubleBuffered",
                    BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty,
                    null,
                    control,
                    new object[] { true });

                foreach (Control child in control.Controls)
                {
                    EnableDoubleBuffering(child);
                }
            }

            /// <summary>
            /// Applies Windows 11 DWM rounded window corners and immersive dark-mode titlebar.
            /// </summary>
            private static void ApplyNativeWindowAttributes(Form form)
            {
                try
                {
                    if (Environment.OSVersion.Version.Major >= 10)
                    {
                        int darkMode = Theme.IsDarkTheme ? 1 : 0;
                        DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, ref darkMode, sizeof(int));

                        if (Theme.EnableDwmRoundedCorners)
                        {
                            int cornerPref = (int)DWM_WINDOW_CORNER_PREFERENCE.DWMSC_ROUND;
                            DwmSetWindowAttribute(form.Handle, DWMWA_WINDOW_CORNER_PREFERENCE, ref cornerPref, sizeof(int));
                        }
                    }
                }
                catch
                {
                    // Silently fall back on older Windows versions where these DWM attributes aren't supported
                }
            }

            /// <summary>
            /// Walks the control tree and applies modern styles based on control type.
            /// </summary>
            private static void StyleControlRecursive(Control parent)
            {
                foreach (Control ctrl in parent.Controls)
                {
                    ctrl.Font = Theme.MainFont;

                    if (ctrl is Button btn)
                        StyleButton(btn);
                    else if (ctrl is TextBox txt)
                        StyleTextBox(txt);
                    else if (ctrl is DataGridView dgv)
                        StyleDataGridView(dgv);
                    else if (ctrl is ComboBox combo)
                        StyleComboBox(combo);
                    else if (ctrl is CheckBox chk)
                        StyleCheckBox(chk);
                    else if (ctrl is RadioButton rad)
                        StyleRadioButton(rad);
                    else if (ctrl is TabControl tab)
                        StyleTabControl(tab);
                    else if (ctrl is ProgressBar pbar)
                        StyleProgressBar(pbar);
                    else if (ctrl is Panel || ctrl is GroupBox)
                    {
                        ctrl.BackColor = Theme.SurfaceColor;
                        ctrl.ForeColor = Theme.TextColor;
                    }

                    // Recurse into children (panels, group boxes, etc.), but not into DataGridView cells
                    if (ctrl.HasChildren && !(ctrl is DataGridView))
                    {
                        StyleControlRecursive(ctrl);
                    }
                }
            }

            // ==========================================
            // CONTROL-SPECIFIC STYLERS & ANIMATIONS
            // ==========================================

            private static void StyleButton(Button btn)
            {
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
                btn.BackColor = Theme.AccentColor;
                btn.ForeColor = Color.White;
                btn.Cursor = Cursors.Hand;
                btn.Padding = new Padding(10, 5, 10, 5);

                if (Theme.CornerRadius > 0)
                {
                    ApplyRoundedRegion(btn, Theme.CornerRadius);
                    btn.SizeChanged += (s, e) => ApplyRoundedRegion(btn, Theme.CornerRadius);
                }

                if (Theme.EnableHoverAnimations)
                {
                    Color origColor = btn.BackColor;
                    Color hoverColor = ControlPaint.Light(origColor, 0.15f);

                    btn.MouseEnter += (s, e) => { btn.BackColor = hoverColor; };
                    btn.MouseLeave += (s, e) => { btn.BackColor = origColor; };
                }
            }

            private static void StyleTextBox(TextBox txt)
            {
                txt.BorderStyle = BorderStyle.FixedSingle;
                txt.BackColor = Theme.SurfaceColor;
                txt.ForeColor = Theme.TextColor;

                if (Theme.EnableFocusGlow)
                {
                    Color normalColor = Theme.SurfaceColor;
                    Color focusColor = ControlPaint.Light(Theme.SurfaceColor, 0.15f);

                    txt.GotFocus += (s, e) => { txt.BackColor = focusColor; };
                    txt.LostFocus += (s, e) => { txt.BackColor = normalColor; };
                }
            }

            private static void StyleDataGridView(DataGridView dgv)
            {
                dgv.EnableHeadersVisualStyles = false;
                dgv.BorderStyle = BorderStyle.None;
                dgv.BackgroundColor = Theme.SurfaceColor;
                dgv.GridColor = Theme.BorderColor;

                // Header style
                dgv.ColumnHeadersDefaultCellStyle.BackColor = Theme.AccentColor;
                dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                dgv.ColumnHeadersDefaultCellStyle.Font = new Font(Theme.MainFont, FontStyle.Bold);
                dgv.ColumnHeadersHeight = 38;

                // Row style
                dgv.DefaultCellStyle.BackColor = Theme.SurfaceColor;
                dgv.DefaultCellStyle.ForeColor = Theme.TextColor;
                dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(60, Theme.AccentColor);
                dgv.DefaultCellStyle.SelectionForeColor = Theme.TextColor;
                dgv.RowTemplate.Height = 32;
                dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }

            private static void StyleComboBox(ComboBox combo)
            {
                combo.FlatStyle = FlatStyle.Flat;
                combo.BackColor = Theme.SurfaceColor;
                combo.ForeColor = Theme.TextColor;
            }

            private static void StyleCheckBox(CheckBox chk)
            {
                chk.FlatStyle = FlatStyle.Flat;
                chk.ForeColor = Theme.TextColor;
                chk.Cursor = Cursors.Hand;
            }

            private static void StyleRadioButton(RadioButton rad)
            {
                rad.FlatStyle = FlatStyle.Flat;
                rad.ForeColor = Theme.TextColor;
                rad.Cursor = Cursors.Hand;
            }

            private static void StyleTabControl(TabControl tab)
            {
                tab.DrawMode = TabDrawMode.OwnerDrawFixed;
                tab.SizeMode = TabSizeMode.Fixed;
                tab.ItemSize = new Size(120, 36);

                tab.DrawItem += (sender, e) =>
                {
                    Graphics g = e.Graphics;
                    Rectangle tabBounds = tab.GetTabRect(e.Index);
                    bool isSelected = tab.SelectedIndex == e.Index;

                    using (Brush bgBrush = new SolidBrush(isSelected ? Theme.SurfaceColor : Theme.BackColor))
                    using (Brush textBrush = new SolidBrush(isSelected ? Theme.AccentColor : Theme.SecondaryTextColor))
                    {
                        g.FillRectangle(bgBrush, tabBounds);

                        // Draw accent underline for the selected tab
                        if (isSelected)
                        {
                            Rectangle underline = new Rectangle(tabBounds.X, tabBounds.Bottom - 3, tabBounds.Width, 3);
                            using (Brush accentBrush = new SolidBrush(Theme.AccentColor))
                            {
                                g.FillRectangle(accentBrush, underline);
                            }
                        }

                        StringFormat sf = new StringFormat
                        {
                            Alignment = StringAlignment.Center,
                            LineAlignment = StringAlignment.Center
                        };

                        g.DrawString(tab.TabPages[e.Index].Text, Theme.MainFont, textBrush, tabBounds, sf);
                    }
                };
            }

            private static void StyleProgressBar(ProgressBar pbar)
            {
                pbar.ForeColor = Theme.AccentColor;
                pbar.BackColor = Theme.SurfaceColor;
            }

            // ==========================================
            // UTILITY HELPER METHODS
            // ==========================================

            public static void SetPlaceholderText(TextBox txt, string placeholder)
            {
                try
                {
                    SendMessage(txt.Handle, EM_SETCUEBANNER, 0, placeholder);
                }
                catch
                {
                    // Ignore if the handle isn't created yet
                }
            }

            private static void ApplyRoundedRegion(Control control, int radius)
            {
                if (control.Width <= 0 || control.Height <= 0) return;

                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddArc(0, 0, radius, radius, 180, 90);
                    path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
                    path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
                    path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
                    path.CloseAllFigures();
                    control.Region = new Region(path);
                }
            }

            /// <summary>
            /// Smooth fade-in animation played when the form first opens.
            /// </summary>
            private static void AnimateFormFadeIn(Form form)
            {
                form.Opacity = 0.0;
                Timer timer = new Timer { Interval = 15 };
                timer.Tick += (s, e) =>
                {
                    if (form.Opacity < 1.0)
                    {
                        form.Opacity = Math.Min(1.0, form.Opacity + 0.05);
                    }
                    else
                    {
                        form.Opacity = 1.0;
                        timer.Stop();
                        timer.Dispose();
                    }
                };
                timer.Start();
            }
        }
    }
}