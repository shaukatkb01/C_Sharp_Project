using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace SupplyBranch.Helpers
{
    /// <summary>
    /// Application-wide theme mode.
    /// </summary>
    public enum AppThemeMode
    {
        Light = 0,
        Dark = 1
    }

    /// <summary>
    /// A complete set of colors for one theme (light or dark).
    /// </summary>
    public sealed class ThemePalette
    {
        public Color Background { get; set; }
        public Color Surface { get; set; }
        public Color SurfaceAlt { get; set; }
        public Color Border { get; set; }

        public Color TextPrimary { get; set; }
        public Color TextSecondary { get; set; }
        public Color TextMuted { get; set; }

        public Color Primary { get; set; }
        public Color PrimaryHover { get; set; }
        public Color PrimaryPressed { get; set; }
        public Color OnPrimary { get; set; }

        public Color Success { get; set; }
        public Color Warning { get; set; }
        public Color Danger { get; set; }

        public Color Sidebar { get; set; }
        public Color SidebarText { get; set; }
        public Color SidebarMuted { get; set; }
        public Color SidebarHover { get; set; }
        public Color SidebarActive { get; set; }

        public Color Header { get; set; }

        public Color GridHeader { get; set; }
        public Color GridHeaderText { get; set; }
        public Color GridRow { get; set; }
        public Color GridRowAlt { get; set; }
        public Color GridSelection { get; set; }
        public Color GridSelectionText { get; set; }
        public Color GridLine { get; set; }
    }

    /// <summary>
    /// Central theme manager. Reads/writes the user's choice and notifies all
    /// theme-aware controls when the mode changes.
    /// </summary>
    public static class AppTheme
    {
        private static readonly ThemePalette LightPalette = CreateLight();
        private static readonly ThemePalette DarkPalette = CreateDark();

        private static AppThemeMode _mode = LoadMode();

        public static ThemePalette Light { get { return LightPalette; } }
        public static ThemePalette Dark { get { return DarkPalette; } }
        public static AppThemeMode Mode { get { return _mode; } }
        public static bool IsDark { get { return _mode == AppThemeMode.Dark; } }
        public static ThemePalette P { get { return IsDark ? DarkPalette : LightPalette; } }

        /// <summary>Raised after the theme mode changes.</summary>
        public static event EventHandler ThemeChanged;

        public static void Toggle()
        {
            SetMode(IsDark ? AppThemeMode.Light : AppThemeMode.Dark);
        }

        public static void SetMode(AppThemeMode mode, bool persist = true)
        {
            if (_mode == mode)
                return;

            _mode = mode;

            if (persist)
                SaveMode(mode);

            EventHandler handler = ThemeChanged;
            if (handler != null)
                handler(null, EventArgs.Empty);
        }

        /// <summary>Applies the base form colors for the active theme.</summary>
        public static void ApplyBase(Form form)
        {
            if (form == null)
                return;

            form.BackColor = P.Background;
            form.ForeColor = P.TextPrimary;
            form.Font = ThemeFonts.Body;
        }

        [DllImport("uxtheme.dll", CharSet = CharSet.Unicode)]
        private static extern int SetWindowTheme(IntPtr hWnd, string pszSubAppName, string pszSubIdList);

        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWnd, EnumChildProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        private static extern int GetClassName(IntPtr hWnd, System.Text.StringBuilder lpClassName, int nMaxCount);

        private delegate bool EnumChildProc(IntPtr hWnd, IntPtr lParam);

        /// <summary>
        /// Gives native scrollbars a dark look in dark mode (and restores the
        /// default look in light mode). Safe to call repeatedly.
        /// </summary>
        public static void ApplyScrollbarTheme(Control control)
        {
            ApplyScrollbarTheme(control, IsDark);
        }

        /// <summary>
        /// Same as <see cref="ApplyScrollbarTheme(Control)"/> but with an explicit
        /// mode, for dark surfaces that stay dark in both themes (e.g. the sidebar).
        /// </summary>
        public static void ApplyScrollbarTheme(Control control, bool dark)
        {
            if (control == null || !control.IsHandleCreated)
                return;

            try
            {
                string theme = dark ? "DarkMode_Explorer" : null;

                SetWindowTheme(control.Handle, theme, null);

                // The scrollbar itself is a separate child window, so theme it too.
                EnumChildWindows(control.Handle, delegate(IntPtr child, IntPtr param)
                {
                    System.Text.StringBuilder builder = new System.Text.StringBuilder(64);
                    GetClassName(child, builder, builder.Capacity);

                    if (builder.ToString().IndexOf("ScrollBar", StringComparison.OrdinalIgnoreCase) >= 0)
                        SetWindowTheme(child, theme, null);

                    return true;
                }, IntPtr.Zero);
            }
            catch
            {
                // Not supported on older Windows versions: ignore.
            }
        }

        private static ThemePalette CreateLight()
        {
            return new ThemePalette
            {
                Background = C("#F3F6FB"),
                Surface = C("#FFFFFF"),
                SurfaceAlt = C("#F6F8FC"),
                Border = C("#E2E8F0"),

                TextPrimary = C("#0F172A"),
                TextSecondary = C("#475569"),
                TextMuted = C("#6B7A90"),

                Primary = C("#2563EB"),
                PrimaryHover = C("#1D4ED8"),
                PrimaryPressed = C("#1E40AF"),
                OnPrimary = C("#FFFFFF"),

                Success = C("#16A34A"),
                Warning = C("#D97706"),
                Danger = C("#DC2626"),

                Sidebar = C("#0B1526"),
                SidebarText = C("#DCE6F5"),
                SidebarMuted = C("#7F8FA8"),
                SidebarHover = C("#16233A"),
                SidebarActive = C("#2563EB"),

                Header = C("#FFFFFF"),

                GridHeader = C("#F1F5F9"),
                GridHeaderText = C("#334155"),
                GridRow = C("#FFFFFF"),
                GridRowAlt = C("#F8FAFC"),
                GridSelection = C("#DBEAFE"),
                GridSelectionText = C("#1E3A8A"),
                GridLine = C("#E8EDF5")
            };
        }

        private static ThemePalette CreateDark()
        {
            return new ThemePalette
            {
                Background = C("#0A1120"),
                Surface = C("#111B2E"),
                SurfaceAlt = C("#17233A"),
                Border = C("#22314C"),

                TextPrimary = C("#E9EFFA"),
                TextSecondary = C("#AEBBD0"),
                TextMuted = C("#7D8DA6"),

                Primary = C("#3B82F6"),
                PrimaryHover = C("#2F74E8"),
                PrimaryPressed = C("#2563EB"),
                OnPrimary = C("#FFFFFF"),

                Success = C("#22C55E"),
                Warning = C("#F59E0B"),
                Danger = C("#EF4444"),

                Sidebar = C("#070D19"),
                SidebarText = C("#D5E0F0"),
                SidebarMuted = C("#73839C"),
                SidebarHover = C("#111D31"),
                SidebarActive = C("#2563EB"),

                Header = C("#0D1728"),

                GridHeader = C("#17233A"),
                GridHeaderText = C("#C9D5E8"),
                GridRow = C("#111B2E"),
                GridRowAlt = C("#0E1729"),
                GridSelection = C("#1E3A8A"),
                GridSelectionText = C("#EAF1FF"),
                GridLine = C("#22314C")
            };
        }

        private static Color C(string hex)
        {
            return ColorTranslator.FromHtml(hex);
        }

        private static string ThemeFilePath
        {
            get
            {
                string folder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "SupplyBranch");

                Directory.CreateDirectory(folder);
                return Path.Combine(folder, "ui-theme.txt");
            }
        }

        private static AppThemeMode LoadMode()
        {
            try
            {
                string path = ThemeFilePath;
                if (File.Exists(path))
                {
                    string value = File.ReadAllText(path).Trim();
                    if (string.Equals(value, "Dark", StringComparison.OrdinalIgnoreCase))
                        return AppThemeMode.Dark;
                }
            }
            catch
            {
                // Ignore: fall back to light theme.
            }

            return AppThemeMode.Light;
        }

        private static void SaveMode(AppThemeMode mode)
        {
            try
            {
                File.WriteAllText(ThemeFilePath, mode.ToString());
            }
            catch
            {
                // Ignore: theme still applies for this session.
            }
        }
    }

    /// <summary>Shared fonts used by the modern UI.</summary>
    public static class ThemeFonts
    {
        public static readonly Font H1 = new Font("Segoe UI Semibold", 21F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font H2 = new Font("Segoe UI Semibold", 15.5F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font H3 = new Font("Segoe UI Semibold", 12F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font Body = new Font("Segoe UI", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font BodyLarge = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font BodyBold = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font Small = new Font("Segoe UI", 8.5F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font Tiny = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font Button = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font Metric = new Font("Segoe UI Semibold", 22F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font SectionLabel = new Font("Segoe UI Semibold", 8F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font Icon = new Font("Segoe MDL2 Assets", 11F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font IconSmall = new Font("Segoe MDL2 Assets", 9F, FontStyle.Regular, GraphicsUnit.Point);
        public static readonly Font IconLarge = new Font("Segoe MDL2 Assets", 17F, FontStyle.Regular, GraphicsUnit.Point);
    }

    /// <summary>Segoe MDL2 Assets glyphs used across the UI.</summary>
    public static class ThemeIcons
    {
        public const string Home = "\uE80F";
        public const string Dashboard = "\uE80F";
        public const string People = "\uE716";
        public const string User = "\uE77B";
        public const string Settings = "\uE713";
        public const string Office = "\uE80F";
        public const string Map = "\uE707";
        public const string Document = "\uE8A5";
        public const string Page = "\uE7C3";
        public const string Package = "\uE7B8";
        public const string Library = "\uE8F1";
        public const string Add = "\uE710";
        public const string Edit = "\uE70F";
        public const string Delete = "\uE74D";
        public const string Save = "\uE74E";
        public const string Search = "\uE721";
        public const string Refresh = "\uE72C";
        public const string Print = "\uE749";
        public const string Lock = "\uE72E";
        public const string Unlock = "\uE785";
        public const string Key = "\uE8D7";
        public const string Warning = "\uE7BA";
        public const string Info = "\uE946";
        public const string Help = "\uE897";
        public const string Clock = "\uE823";
        public const string Calendar = "\uE787";
        public const string Sun = "\uE706";
        public const string Moon = "\uE708";
        public const string Check = "\uE73E";
        public const string Cancel = "\uE711";
        public const string ChevronRight = "\uE76C";
        public const string ChevronDown = "\uE70D";
        public const string Power = "\uE7E8";
        public const string Download = "\uE896";
        public const string Keyboard = "\uE765";
        public const string Chart = "\uE9D2";
        public const string Money = "\uE8EF";
        public const string Switch = "\uE8AB";
    }

    /// <summary>Small color helpers.</summary>
    public static class ColorUtil
    {
        public static Color Blend(Color from, Color to, float amount)
        {
            amount = Math.Max(0F, Math.Min(1F, amount));
            int r = (int)(from.R + (to.R - from.R) * amount);
            int g = (int)(from.G + (to.G - from.G) * amount);
            int b = (int)(from.B + (to.B - from.B) * amount);
            int a = (int)(from.A + (to.A - from.A) * amount);
            return Color.FromArgb(a, r, g, b);
        }

        public static Color Darken(Color color, float amount)
        {
            return Blend(color, Color.Black, amount);
        }

        public static Color Lighten(Color color, float amount)
        {
            return Blend(color, Color.White, amount);
        }

        public static Color WithAlpha(Color color, int alpha)
        {
            return Color.FromArgb(Math.Max(0, Math.Min(255, alpha)), color.R, color.G, color.B);
        }
    }
}
