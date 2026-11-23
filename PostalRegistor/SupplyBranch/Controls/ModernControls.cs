using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using SupplyBranch.Helpers;

namespace SupplyBranch.Controls
{
    // =====================================================================
    //  Shared drawing helpers
    // =====================================================================

    internal static class DrawUtil
    {
        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (radius <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            int d = radius * 2;
            if (d > bounds.Width) d = bounds.Width;
            if (d > bounds.Height) d = bounds.Height;

            path.AddArc(bounds.X, bounds.Y, d, d, 180, 90);
            path.AddArc(bounds.Right - d, bounds.Y, d, d, 270, 90);
            path.AddArc(bounds.Right - d, bounds.Bottom - d, d, d, 0, 90);
            path.AddArc(bounds.X, bounds.Bottom - d, d, d, 90, 90);
            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Resolves the first opaque background color walking up the parent
        /// chain. Used to clear a custom-painted control so no stale text or
        /// rounded-corner artefacts remain behind it.
        /// </summary>
        public static Color ParentBackColor(Control control, ThemePalette palette)
        {
            Control parent = control != null ? control.Parent : null;

            while (parent != null)
            {
                Color color = parent.BackColor;
                if (color.A == 255)
                    return color;

                parent = parent.Parent;
            }

            return palette.Surface;
        }
    }

    // =====================================================================
    //  Themed containers
    // =====================================================================

    public enum PanelRole
    {
        Background,
        Surface,
        SurfaceAlt,
        Sidebar,
        Header,
        Transparent
    }

    public class ThemedPanel : Panel
    {
        private PanelRole _role = PanelRole.Surface;

        public PanelRole Role
        {
            get { return _role; }
            set { _role = value; ApplyRole(); }
        }

        public ThemedPanel()
        {
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);

            ApplyRole();
            AppTheme.ThemeChanged += HandleThemeChanged;
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            ApplyRole();
            Invalidate(true);
        }

        private void ApplyRole()
        {
            ThemePalette p = AppTheme.P;

            switch (_role)
            {
                case PanelRole.Background:
                    BackColor = p.Background;
                    break;
                case PanelRole.Surface:
                    BackColor = p.Surface;
                    break;
                case PanelRole.SurfaceAlt:
                    BackColor = p.SurfaceAlt;
                    break;
                case PanelRole.Sidebar:
                    BackColor = p.Sidebar;
                    break;
                case PanelRole.Header:
                    BackColor = p.Header;
                    break;
                case PanelRole.Transparent:
                    BackColor = Color.Transparent;
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                AppTheme.ThemeChanged -= HandleThemeChanged;

            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// A panel that hosts a single tall child and scrolls it with the mouse
    /// wheel, drawing its own slim scrollbar so the sidebar stays consistent
    /// in both themes (native scrollbars ignore the dark sidebar styling).
    /// </summary>
    public class ScrollHostPanel : Panel, IMessageFilter
    {
        private const int WM_MOUSEWHEEL = 0x020A;
        private const int GutterWidth = 10;
        private const int ThumbWidth = 4;

        private PanelRole _role = PanelRole.Sidebar;
        private Control _content;
        private int _offset;
        private bool _dragging;
        private int _dragStartY;
        private int _dragStartOffset;
        private bool _filterAdded;
        private bool _inLayout;

        public PanelRole Role
        {
            get { return _role; }
            set { _role = value; ApplyRole(); }
        }

        public Color ThumbColor { get; set; }

        public ScrollHostPanel()
        {
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);

            ApplyRole();
            ThumbColor = ColorUtil.Blend(AppTheme.P.Sidebar, AppTheme.P.SidebarText, 0.26F);
            AppTheme.ThemeChanged += HandleThemeChanged;
        }

        public void SetContent(Control content)
        {
            if (_content != null)
                Controls.Remove(_content);

            _content = content;

            if (_content != null)
            {
                _content.Dock = DockStyle.None;
                _content.Left = 0;
                _content.Top = 0;
                Controls.Add(_content);
            }

            Relayout();
        }

        /// <summary>Re-measures the hosted content. Call after adding rows.</summary>
        public void Relayout()
        {
            if (_content == null || _inLayout || IsDisposed)
                return;

            _inLayout = true;

            try
            {
                int width = Math.Max(1, ClientSize.Width - GutterWidth);
                _content.Width = width;

                int preferred = _content.GetPreferredSize(new Size(width, 0)).Height;
                if (preferred > 0)
                    _content.Height = preferred;

                ClampOffset();
                _content.Top = _offset;
            }
            finally
            {
                _inLayout = false;
            }

            Invalidate();
        }

        public void ScrollToTop()
        {
            _offset = 0;
            if (_content != null)
                _content.Top = 0;
            Invalidate();
        }

        private int MaxOffset
        {
            get
            {
                if (_content == null)
                    return 0;

                return Math.Min(0, ClientSize.Height - _content.Height);
            }
        }

        private void ClampOffset()
        {
            int max = MaxOffset;
            if (_offset < max) _offset = max;
            if (_offset > 0) _offset = 0;
        }

        private void ScrollBy(int delta)
        {
            if (MaxOffset >= 0)
                return;

            _offset -= delta;
            ClampOffset();

            if (_content != null)
                _content.Top = _offset;

            Invalidate();
        }

        private Rectangle GetThumbRect()
        {
            if (_content == null)
                return Rectangle.Empty;

            int scrollable = -MaxOffset;
            if (scrollable <= 0)
                return Rectangle.Empty;

            int trackTop = 6;
            int trackHeight = ClientSize.Height - 12;
            if (trackHeight <= 0)
                return Rectangle.Empty;

            int thumbHeight = Math.Max(30, (int)(trackHeight * (double)ClientSize.Height / _content.Height));
            if (thumbHeight > trackHeight)
                thumbHeight = trackHeight;

            int thumbTop = trackTop + (int)((trackHeight - thumbHeight) * (double)(-_offset) / scrollable);

            return new Rectangle(ClientSize.Width - ThumbWidth - 3, thumbTop, ThumbWidth, thumbHeight);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Relayout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Rectangle thumb = GetThumbRect();
            if (thumb.IsEmpty)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (GraphicsPath path = DrawUtil.RoundedRect(thumb, ThumbWidth / 2))
            using (SolidBrush brush = new SolidBrush(ThumbColor))
            {
                e.Graphics.FillPath(brush, path);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            Rectangle thumb = GetThumbRect();
            if (thumb.IsEmpty)
                return;

            if (thumb.Contains(e.Location))
            {
                _dragging = true;
                _dragStartY = e.Y;
                _dragStartOffset = _offset;
                Capture = true;
            }
            else if (e.X >= ClientSize.Width - GutterWidth)
            {
                int trackTop = 6;
                int trackHeight = ClientSize.Height - 12;
                int thumbHeight = thumb.Height;
                int span = Math.Max(1, trackHeight - thumbHeight);
                int target = e.Y - trackTop - thumbHeight / 2;
                int scrollable = -MaxOffset;

                _offset = -(int)(Math.Max(0, Math.Min(span, target)) * (double)scrollable / span);
                ClampOffset();

                if (_content != null)
                    _content.Top = _offset;

                Invalidate();
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (!_dragging)
            {
                Rectangle thumb = GetThumbRect();
                Cursor = !thumb.IsEmpty && thumb.Contains(e.Location)
                    ? Cursors.Hand
                    : Cursors.Default;
                return;
            }

            Rectangle t = GetThumbRect();
            if (t.IsEmpty)
                return;

            int trackHeight = ClientSize.Height - 12;
            int span = Math.Max(1, trackHeight - t.Height);
            int scrollable = -MaxOffset;

            _offset = _dragStartOffset - (int)((e.Y - _dragStartY) * (double)scrollable / span);
            ClampOffset();

            if (_content != null)
                _content.Top = _offset;

            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _dragging = false;
            Capture = false;
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            ScrollBy(e.Delta);
        }

        public bool PreFilterMessage(ref Message m)
        {
            if (m.Msg != WM_MOUSEWHEEL || _content == null || MaxOffset >= 0 || !IsHandleCreated)
                return false;

            if (!RectangleToScreen(ClientRectangle).Contains(Cursor.Position))
                return false;

            int delta = (short)((long)m.WParam >> 16);
            ScrollBy(delta);
            return true;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (!_filterAdded)
            {
                Application.AddMessageFilter(this);
                _filterAdded = true;
            }

            Relayout();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (_filterAdded)
            {
                Application.RemoveMessageFilter(this);
                _filterAdded = false;
            }

            base.OnHandleDestroyed(e);
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            ApplyRole();
            ThumbColor = ColorUtil.Blend(AppTheme.P.Sidebar, AppTheme.P.SidebarText, 0.26F);
            Invalidate();
        }

        private void ApplyRole()
        {
            ThemePalette p = AppTheme.P;

            switch (_role)
            {
                case PanelRole.Background:
                    BackColor = p.Background;
                    break;
                case PanelRole.Surface:
                    BackColor = p.Surface;
                    break;
                case PanelRole.SurfaceAlt:
                    BackColor = p.SurfaceAlt;
                    break;
                case PanelRole.Sidebar:
                    BackColor = p.Sidebar;
                    break;
                case PanelRole.Header:
                    BackColor = p.Header;
                    break;
                case PanelRole.Transparent:
                    BackColor = Color.Transparent;
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_filterAdded)
                {
                    Application.RemoveMessageFilter(this);
                    _filterAdded = false;
                }

                AppTheme.ThemeChanged -= HandleThemeChanged;
            }

            base.Dispose(disposing);
        }
    }

    public class ThemedTable : TableLayoutPanel
    {
        private PanelRole _role = PanelRole.Surface;

        public PanelRole Role
        {
            get { return _role; }
            set { _role = value; ApplyRole(); }
        }

        public ThemedTable()
        {
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);

            ApplyRole();
            AppTheme.ThemeChanged += HandleThemeChanged;
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            ApplyRole();
            Invalidate(true);
        }

        private void ApplyRole()
        {
            ThemePalette p = AppTheme.P;

            switch (_role)
            {
                case PanelRole.Background:
                    BackColor = p.Background;
                    break;
                case PanelRole.Surface:
                    BackColor = p.Surface;
                    break;
                case PanelRole.SurfaceAlt:
                    BackColor = p.SurfaceAlt;
                    break;
                case PanelRole.Sidebar:
                    BackColor = p.Sidebar;
                    break;
                case PanelRole.Header:
                    BackColor = p.Header;
                    break;
                case PanelRole.Transparent:
                    BackColor = Color.Transparent;
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                AppTheme.ThemeChanged -= HandleThemeChanged;

            base.Dispose(disposing);
        }
    }

    /// <summary>A card with rounded corners and a subtle 1px border.</summary>
    public class CardPanel : Panel
    {
        private int _cornerRadius = 14;
        private int _borderWidth = 1;
        private bool _useAltSurface;

        public bool UseAltSurface
        {
            get { return _useAltSurface; }
            set
            {
                _useAltSurface = value;
                BackColor = SurfaceColor;
                Invalidate();
            }
        }

        private Color SurfaceColor
        {
            get { return _useAltSurface ? AppTheme.P.SurfaceAlt : AppTheme.P.Surface; }
        }

        public int CornerRadius
        {
            get { return _cornerRadius; }
            set
            {
                _cornerRadius = Math.Max(0, value);
                UpdateRegion();
                Invalidate();
            }
        }

        public int BorderWidth
        {
            get { return _borderWidth; }
            set
            {
                _borderWidth = Math.Max(0, value);
                Invalidate();
            }
        }

        public CardPanel()
        {
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint,
                true);

            BackColor = AppTheme.P.Surface;
            Padding = new Padding(16);
            AppTheme.ThemeChanged += HandleThemeChanged;
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            BackColor = SurfaceColor;
            UpdateRegion();
            Invalidate(true);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
        }

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0)
                return;

            using (GraphicsPath path = DrawUtil.RoundedRect(
                new Rectangle(0, 0, Width, Height), _cornerRadius))
            {
                Region old = Region;
                Region = new Region(path);
                if (old != null)
                    old.Dispose();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_borderWidth <= 0)
                return;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = DrawUtil.RoundedRect(r, _cornerRadius))
            using (Pen pen = new Pen(AppTheme.P.Border, _borderWidth))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                AppTheme.ThemeChanged -= HandleThemeChanged;

            base.Dispose(disposing);
        }
    }

    // =====================================================================
    //  Themed labels
    // =====================================================================

    public enum LabelRole
    {
        Primary,
        Secondary,
        Muted,
        Accent,
        OnAccent,
        Danger,
        Success,
        Warning,
        SidebarText,
        SidebarMuted
    }

    public class ThemedLabel : Label
    {
        private LabelRole _role = LabelRole.Primary;

        public LabelRole Role
        {
            get { return _role; }
            set { _role = value; ApplyRole(); }
        }

        public ThemedLabel()
        {
            BackColor = Color.Transparent;
            Font = ThemeFonts.Body;
            UseMnemonic = false;
            ApplyRole();
            AppTheme.ThemeChanged += HandleThemeChanged;
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            ApplyRole();
        }

        private void ApplyRole()
        {
            ThemePalette p = AppTheme.P;

            switch (_role)
            {
                case LabelRole.Primary:
                    ForeColor = p.TextPrimary;
                    break;
                case LabelRole.Secondary:
                    ForeColor = p.TextSecondary;
                    break;
                case LabelRole.Muted:
                    ForeColor = p.TextMuted;
                    break;
                case LabelRole.Accent:
                    ForeColor = p.Primary;
                    break;
                case LabelRole.OnAccent:
                    ForeColor = p.OnPrimary;
                    break;
                case LabelRole.Danger:
                    ForeColor = p.Danger;
                    break;
                case LabelRole.Success:
                    ForeColor = p.Success;
                    break;
                case LabelRole.Warning:
                    ForeColor = p.Warning;
                    break;
                case LabelRole.SidebarText:
                    ForeColor = p.SidebarText;
                    break;
                case LabelRole.SidebarMuted:
                    ForeColor = p.SidebarMuted;
                    break;
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                AppTheme.ThemeChanged -= HandleThemeChanged;

            base.Dispose(disposing);
        }
    }

    // =====================================================================
    //  Buttons
    // =====================================================================

    public enum ButtonVariant
    {
        Primary,
        Secondary,
        Ghost,
        Subtle,
        Danger,
        Success,
        Sidebar
    }

    public class ModernButton : Button
    {
        private ButtonVariant _variant = ButtonVariant.Primary;
        private bool _hover;
        private bool _pressed;

        public ButtonVariant Variant
        {
            get { return _variant; }
            set { _variant = value; Invalidate(); }
        }

        public string IconGlyph { get; set; }

        public int CornerRadius { get; set; }

        public ModernButton()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Opaque |
                ControlStyles.SupportsTransparentBackColor,
                true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            BackColor = Color.Transparent;
            ForeColor = Color.White;
            Font = ThemeFonts.Button;
            Cursor = Cursors.Hand;
            Height = 38;
            CornerRadius = 8;

            AppTheme.ThemeChanged += HandleThemeChanged;
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hover = false;
            _pressed = false;
            Invalidate();
        }

        protected override void OnMouseDown(MouseEventArgs mevent)
        {
            base.OnMouseDown(mevent);
            _pressed = true;
            Invalidate();
        }

        protected override void OnMouseUp(MouseEventArgs mevent)
        {
            base.OnMouseUp(mevent);
            _pressed = false;
            Invalidate();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ThemePalette p = AppTheme.P;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Always clear the full client area first so no stale glyphs or
            // rounded-corner remnants stay behind this button.
            using (SolidBrush clear = new SolidBrush(DrawUtil.ParentBackColor(this, p)))
            {
                e.Graphics.FillRectangle(clear, ClientRectangle);
            }

            Color back;
            Color fore;
            Color border;
            ResolveColors(p, out back, out fore, out border);

            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = DrawUtil.RoundedRect(bounds, CornerRadius))
            {
                if (back.A > 0)
                {
                    using (SolidBrush brush = new SolidBrush(back))
                        e.Graphics.FillPath(brush, path);
                }

                if (border.A > 0)
                {
                    using (Pen pen = new Pen(border, 1F))
                        e.Graphics.DrawPath(pen, path);
                }
            }

            string text = Text ?? string.Empty;
            string glyph = IconGlyph ?? string.Empty;

            if (string.IsNullOrEmpty(glyph))
            {
                TextRenderer.DrawText(
                    e.Graphics, text, Font, ClientRectangle, fore,
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPrefix |
                    TextFormatFlags.EndEllipsis);
                return;
            }

            Size iconSize = TextRenderer.MeasureText(glyph, ThemeFonts.Icon);
            Size textSize = string.IsNullOrEmpty(text)
                ? Size.Empty
                : TextRenderer.MeasureText(text, Font);

            int gap = string.IsNullOrEmpty(text) ? 0 : 8;
            int total = iconSize.Width + gap + textSize.Width;
            int startX = Math.Max(2, (Width - total) / 2);

            Rectangle iconRect = new Rectangle(startX, 0, iconSize.Width, Height);
            TextRenderer.DrawText(
                e.Graphics, glyph, ThemeFonts.Icon, iconRect, fore,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);

            if (!string.IsNullOrEmpty(text))
            {
                Rectangle textRect = new Rectangle(
                    startX + iconSize.Width + gap, 0, textSize.Width + 4, Height);

                TextRenderer.DrawText(
                    e.Graphics, text, Font, textRect, fore,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
            }
        }

        private void ResolveColors(ThemePalette p, out Color back, out Color fore, out Color border)
        {
            border = Color.Transparent;

            switch (_variant)
            {
                case ButtonVariant.Secondary:
                    back = p.Surface;
                    fore = p.TextPrimary;
                    border = p.Border;
                    break;
                case ButtonVariant.Ghost:
                    back = Color.Transparent;
                    fore = p.TextSecondary;
                    break;
                case ButtonVariant.Subtle:
                    back = p.SurfaceAlt;
                    fore = p.TextPrimary;
                    break;
                case ButtonVariant.Danger:
                    back = p.Danger;
                    fore = Color.White;
                    break;
                case ButtonVariant.Success:
                    back = p.Success;
                    fore = Color.White;
                    break;
                case ButtonVariant.Sidebar:
                    back = Color.Transparent;
                    fore = p.SidebarText;
                    break;
                default:
                    back = p.Primary;
                    fore = p.OnPrimary;
                    break;
            }

            if (!Enabled)
            {
                back = p.SurfaceAlt;
                fore = p.TextMuted;
                border = p.Border;
                return;
            }

            if (_pressed)
            {
                if (back.A > 0)
                    back = ColorUtil.Darken(back, 0.14F);
            }
            else if (_hover)
            {
                switch (_variant)
                {
                    case ButtonVariant.Primary:
                        back = p.PrimaryHover;
                        break;
                    case ButtonVariant.Danger:
                        back = ColorUtil.Darken(p.Danger, 0.1F);
                        break;
                    case ButtonVariant.Success:
                        back = ColorUtil.Darken(p.Success, 0.1F);
                        break;
                    case ButtonVariant.Ghost:
                        back = p.SurfaceAlt;
                        break;
                    case ButtonVariant.Secondary:
                        back = p.SurfaceAlt;
                        break;
                    case ButtonVariant.Subtle:
                        back = p.Border;
                        break;
                    case ButtonVariant.Sidebar:
                        back = p.SidebarHover;
                        break;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                AppTheme.ThemeChanged -= HandleThemeChanged;

            base.Dispose(disposing);
        }
    }

    /// <summary>A left-aligned navigation button used in the sidebar.</summary>
    public class SidebarButton : Button
    {
        private bool _hover;

        public string IconGlyph { get; set; }

        public bool IsActive { get; set; }

        public bool AdminOnly { get; set; }

        public Type NavFormType { get; set; }

        public SidebarButton()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.Opaque |
                ControlStyles.SupportsTransparentBackColor,
                true);

            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            BackColor = Color.Transparent;
            Font = ThemeFonts.Body;
            Cursor = Cursors.Hand;
            Height = 38;
            TextAlign = ContentAlignment.MiddleLeft;
            Padding = new Padding(44, 0, 10, 0);

            AppTheme.ThemeChanged += HandleThemeChanged;
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            _hover = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            _hover = false;
            Invalidate();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Cursor = Enabled ? Cursors.Hand : Cursors.Default;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            ThemePalette p = AppTheme.P;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Clear the whole row first (prevents text ghosting when the
            // navigation panel scrolls or the theme changes).
            using (SolidBrush clear = new SolidBrush(DrawUtil.ParentBackColor(this, p)))
            {
                e.Graphics.FillRectangle(clear, ClientRectangle);
            }

            Color back = Color.Transparent;
            Color fore = Enabled ? p.SidebarText : p.SidebarMuted;

            if (IsActive && Enabled)
            {
                back = p.SidebarActive;
                fore = Color.White;
            }
            else if (_hover && Enabled)
            {
                back = p.SidebarHover;
                fore = Color.White;
            }

            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = DrawUtil.RoundedRect(bounds, 8))
            {
                if (back.A > 0)
                {
                    using (SolidBrush brush = new SolidBrush(back))
                        e.Graphics.FillPath(brush, path);
                }
            }

            string glyph = IconGlyph ?? string.Empty;
            if (!string.IsNullOrEmpty(glyph))
            {
                Rectangle iconRect = new Rectangle(15, 0, 20, Height);
                TextRenderer.DrawText(
                    e.Graphics, glyph, ThemeFonts.Icon, iconRect, fore,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
            }

            Rectangle textRect = new Rectangle(44, 0, Math.Max(10, Width - 52), Height);
            TextRenderer.DrawText(
                e.Graphics, Text ?? string.Empty, Font, textRect, fore,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                AppTheme.ThemeChanged -= HandleThemeChanged;

            base.Dispose(disposing);
        }
    }

    // =====================================================================
    //  Icon badge + KPI card
    // =====================================================================

    public class IconBadge : Control
    {
        private string _glyph = string.Empty;
        private Color _accent = Color.FromArgb(37, 99, 235);
        private int _radius = 11;

        public string Glyph
        {
            get { return _glyph; }
            set { _glyph = value; Invalidate(); }
        }

        public Color Accent
        {
            get { return _accent; }
            set { _accent = value; Invalidate(); }
        }

        public int Radius
        {
            get { return _radius; }
            set { _radius = Math.Max(0, value); Invalidate(); }
        }

        public IconBadge()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw,
                true);

            Size = new Size(44, 44);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = DrawUtil.RoundedRect(bounds, _radius))
            using (SolidBrush brush = new SolidBrush(_accent))
            {
                e.Graphics.FillPath(brush, path);
            }

            TextRenderer.DrawText(
                e.Graphics, _glyph, ThemeFonts.IconLarge, ClientRectangle, Color.White,
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.NoPrefix);
        }
    }

    /// <summary>KPI / statistics card used on the dashboard.</summary>
    public class StatCard : CardPanel
    {
        private readonly IconBadge _badge;
        private readonly ThemedLabel _value;
        private readonly ThemedLabel _title;
        private readonly ThemedLabel _hint;

        public Label ValueLabel { get { return _value; } }

        public StatCard(string title, string hint, string glyph, Color accent)
        {
            Padding = new Padding(18);
            Height = 112;

            _badge = new IconBadge
            {
                Glyph = glyph,
                Accent = accent,
                Location = new Point(18, 22),
                Size = new Size(44, 44)
            };

            _title = new ThemedLabel
            {
                Text = title,
                Role = LabelRole.Muted,
                Font = ThemeFonts.Small,
                AutoSize = true,
                Location = new Point(76, 22)
            };

            _value = new ThemedLabel
            {
                Text = "0",
                Role = LabelRole.Primary,
                Font = ThemeFonts.Metric,
                AutoSize = true,
                Location = new Point(74, 40)
            };

            _hint = new ThemedLabel
            {
                Text = hint,
                Role = LabelRole.Muted,
                Font = ThemeFonts.Tiny,
                AutoSize = true,
                Location = new Point(76, 78)
            };

            Controls.Add(_badge);
            Controls.Add(_title);
            Controls.Add(_value);
            Controls.Add(_hint);
        }
    }

    // =====================================================================
    //  Modern input field
    // =====================================================================

    public class InputBox : Panel
    {
        private readonly TextBox _textBox;
        private readonly Label _iconLabel;
        private readonly Label _revealLabel;
        private readonly Label _placeholderLabel;

        private bool _focused;
        private bool _revealed;
        private string _placeholder = string.Empty;
        private string _iconGlyph = string.Empty;
        private bool _showReveal;

        public event EventHandler ValueChanged;

        public string Value
        {
            get { return _textBox.Text; }
            set { _textBox.Text = value ?? string.Empty; }
        }

        public TextBox InnerTextBox { get { return _textBox; } }

        public string Placeholder
        {
            get { return _placeholder; }
            set
            {
                _placeholder = value ?? string.Empty;
                if (_placeholderLabel != null)
                {
                    _placeholderLabel.Text = _placeholder;
                    UpdatePlaceholderVisibility();
                }
            }
        }

        public string IconGlyph
        {
            get { return _iconGlyph; }
            set
            {
                _iconGlyph = value ?? string.Empty;
                _iconLabel.Text = _iconGlyph;
                LayoutChildren();
            }
        }

        public bool ShowReveal
        {
            get { return _showReveal; }
            set
            {
                _showReveal = value;
                _revealLabel.Visible = value;
                LayoutChildren();
            }
        }

        public bool UseSystemPasswordChar
        {
            get { return _textBox.UseSystemPasswordChar; }
            set
            {
                _textBox.UseSystemPasswordChar = value;
                if (!value)
                    _revealed = true;
            }
        }

        public bool ReadOnly
        {
            get { return _textBox.ReadOnly; }
            set { _textBox.ReadOnly = value; }
        }

        public InputBox()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.SupportsTransparentBackColor,
                true);

            Height = 46;
            BackColor = AppTheme.P.Surface;

            _iconLabel = new Label
            {
                AutoSize = false,
                Size = new Size(20, 20),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = ThemeFonts.Icon,
                BackColor = Color.Transparent,
                ForeColor = AppTheme.P.TextMuted
            };

            _textBox = new TextBox
            {
                BorderStyle = BorderStyle.None,
                Font = ThemeFonts.BodyLarge,
                BackColor = AppTheme.P.Surface,
                ForeColor = AppTheme.P.TextPrimary
            };

            _textBox.TextChanged += (s, e) =>
            {
                UpdatePlaceholderVisibility();

                EventHandler handler = ValueChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            };

            _textBox.Enter += (s, e) => { _focused = true; UpdatePlaceholderVisibility(); Invalidate(); };
            _textBox.Leave += (s, e) => { _focused = false; UpdatePlaceholderVisibility(); Invalidate(); };

            _placeholderLabel = new Label
            {
                AutoSize = false,
                UseMnemonic = false,
                Text = string.Empty,
                Font = ThemeFonts.BodyLarge,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                ForeColor = AppTheme.P.TextMuted,
                Cursor = Cursors.IBeam
            };
            _placeholderLabel.Click += (s, e) => _textBox.Focus();

            _revealLabel = new Label
            {
                AutoSize = false,
                Size = new Size(24, 24),
                Text = "\uE7B3",
                TextAlign = ContentAlignment.MiddleCenter,
                Font = ThemeFonts.Icon,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Visible = false,
                ForeColor = AppTheme.P.TextMuted
            };

            _revealLabel.Click += (s, e) => ToggleReveal();

            Controls.Add(_iconLabel);
            Controls.Add(_textBox);
            Controls.Add(_revealLabel);
            Controls.Add(_placeholderLabel);
            _placeholderLabel.BringToFront();

            AppTheme.ThemeChanged += HandleThemeChanged;
        }

        private void UpdatePlaceholderVisibility()
        {
            if (_placeholderLabel == null || _textBox == null)
                return;

            _placeholderLabel.Visible =
                !_focused && _textBox.TextLength == 0 && !string.IsNullOrEmpty(_placeholder);
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            BackColor = AppTheme.P.Surface;
            _textBox.BackColor = AppTheme.P.Surface;
            _textBox.ForeColor = AppTheme.P.TextPrimary;
            _iconLabel.ForeColor = _focused ? AppTheme.P.Primary : AppTheme.P.TextMuted;
            _revealLabel.ForeColor = AppTheme.P.TextMuted;
            _placeholderLabel.ForeColor = AppTheme.P.TextMuted;
            UpdateRegion();
            Invalidate();
        }

        private void ToggleReveal()
        {
            _revealed = !_revealed;
            _textBox.UseSystemPasswordChar = !_revealed;
            _revealLabel.ForeColor = _revealed ? AppTheme.P.Primary : AppTheme.P.TextMuted;
            _textBox.Focus();
        }

        private void ApplyCueBanner()
        {
            // Placeholder is drawn by a themed label instead of the native cue
            // banner so it renders consistently in both themes and in bitmaps.
            UpdatePlaceholderVisibility();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateRegion();
            LayoutChildren();
        }

        private void UpdateRegion()
        {
            if (Width <= 0 || Height <= 0)
                return;

            using (GraphicsPath path = DrawUtil.RoundedRect(
                new Rectangle(0, 0, Width, Height), 10))
            {
                Region old = Region;
                Region = new Region(path);
                if (old != null)
                    old.Dispose();
            }
        }

        private void LayoutChildren()
        {
            if (_textBox == null || _iconLabel == null || _revealLabel == null)
                return;

            int iconWidth = string.IsNullOrEmpty(_iconGlyph) ? 0 : 40;
            int revealWidth = _showReveal ? 32 : 12;

            _iconLabel.Visible = iconWidth > 0;
            _iconLabel.Location = new Point(14, (Height - _iconLabel.Height) / 2);

            int textLeft = 14 + iconWidth;
            int textWidth = Math.Max(20, Width - textLeft - revealWidth - 4);

            _textBox.Location = new Point(textLeft, (Height - _textBox.Height) / 2);
            _textBox.Width = textWidth;

            _revealLabel.Location = new Point(Width - _revealLabel.Width - 8,
                (Height - _revealLabel.Height) / 2);

            if (_placeholderLabel != null)
            {
                _placeholderLabel.Location = new Point(textLeft, (Height - _textBox.Height) / 2);
                _placeholderLabel.Size = new Size(textWidth, _textBox.Height);
            }

            UpdatePlaceholderVisibility();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = DrawUtil.RoundedRect(bounds, 10))
            using (Pen pen = new Pen(_focused ? AppTheme.P.Primary : AppTheme.P.Border,
                _focused ? 1.8F : 1F))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _textBox.Focus();
        }

        public void FocusInput()
        {
            _textBox.Focus();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                AppTheme.ThemeChanged -= HandleThemeChanged;

            base.Dispose(disposing);
        }
    }

    // =====================================================================
    //  Standard-control theming helpers
    // =====================================================================

    public static class ControlStyler
    {
        private static readonly ConditionalWeakTable<ComboBox, EventHandler> ComboHandlers =
            new ConditionalWeakTable<ComboBox, EventHandler>();

        public static void StyleComboBox(ComboBox combo)
        {
            if (combo == null)
                return;

            EventHandler existing;
            if (!ComboHandlers.TryGetValue(combo, out existing))
            {
                EventHandler handler = (s, e) => ApplyCombo(combo);
                ComboHandlers.Add(combo, handler);
                AppTheme.ThemeChanged += handler;

                combo.Disposed += (s, e) => AppTheme.ThemeChanged -= handler;
                combo.DrawMode = DrawMode.OwnerDrawFixed;
                combo.ItemHeight = 28;
                combo.IntegralHeight = false;
                combo.DrawItem += DrawComboItem;
            }

            ApplyCombo(combo);
        }

        private static void ApplyCombo(ComboBox combo)
        {
            ThemePalette p = AppTheme.P;
            combo.BackColor = p.SurfaceAlt;
            combo.ForeColor = p.TextPrimary;
            combo.Font = ThemeFonts.Body;
            combo.FlatStyle = FlatStyle.Flat;
            combo.Refresh();
        }

        private static void DrawComboItem(object sender, DrawItemEventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            if (combo == null)
                return;

            ThemePalette p = AppTheme.P;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color back = selected ? p.GridSelection : p.Surface;
            Color fore = selected ? p.GridSelectionText : p.TextPrimary;

            using (SolidBrush brush = new SolidBrush(back))
                e.Graphics.FillRectangle(brush, e.Bounds);

            if (e.Index >= 0)
            {
                string text = combo.GetItemText(combo.Items[e.Index]);

                TextRenderer.DrawText(
                    e.Graphics, text, combo.Font,
                    new Rectangle(e.Bounds.X + 8, e.Bounds.Y, e.Bounds.Width - 8, e.Bounds.Height),
                    fore,
                    TextFormatFlags.Left | TextFormatFlags.VerticalCenter |
                    TextFormatFlags.NoPrefix | TextFormatFlags.EndEllipsis);
            }
        }
    }

    /// <summary>Applies a modern, theme-aware look to any DataGridView.</summary>
    public static class GridStyler
    {
        private static readonly ConditionalWeakTable<DataGridView, EventHandler> Handlers =
            new ConditionalWeakTable<DataGridView, EventHandler>();

        public static void Apply(DataGridView grid)
        {
            if (grid == null)
                return;

            EventHandler existing;
            if (!Handlers.TryGetValue(grid, out existing))
            {
                EventHandler handler = (s, e) => Style(grid);
                Handlers.Add(grid, handler);
                AppTheme.ThemeChanged += handler;

                grid.Disposed += (s, e) => AppTheme.ThemeChanged -= handler;
                grid.HandleCreated += (s, e) => AppTheme.ApplyScrollbarTheme(grid);
                EnableDoubleBuffer(grid);
            }

            Style(grid);
        }

        private static void Style(DataGridView grid)
        {
            ThemePalette p = AppTheme.P;

            grid.BackgroundColor = p.Surface;
            grid.BorderStyle = BorderStyle.None;
            grid.GridColor = p.GridLine;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.EnableHeadersVisualStyles = false;
            grid.RowHeadersVisible = false;
            grid.AllowUserToResizeRows = false;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;

            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            grid.ColumnHeadersHeight = 40;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersDefaultCellStyle.BackColor = p.GridHeader;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = p.GridHeaderText;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = p.GridHeader;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = p.GridHeaderText;
            grid.ColumnHeadersDefaultCellStyle.Font = ThemeFonts.BodyBold;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 0, 6, 0);

            grid.DefaultCellStyle.BackColor = p.GridRow;
            grid.DefaultCellStyle.ForeColor = p.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = p.GridSelection;
            grid.DefaultCellStyle.SelectionForeColor = p.GridSelectionText;
            grid.DefaultCellStyle.Font = ThemeFonts.Body;
            grid.DefaultCellStyle.Padding = new Padding(6, 0, 6, 0);

            grid.AlternatingRowsDefaultCellStyle.BackColor = p.GridRowAlt;
            grid.RowTemplate.Height = 38;

            AppTheme.ApplyScrollbarTheme(grid);
            grid.Refresh();
        }

        private static void EnableDoubleBuffer(DataGridView grid)
        {
            try
            {
                PropertyInfo property = typeof(DataGridView).GetProperty(
                    "DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);

                if (property != null)
                    property.SetValue(grid, true, null);
            }
            catch
            {
                // Non-critical: only affects smooth scrolling.
            }
        }
    }
}
