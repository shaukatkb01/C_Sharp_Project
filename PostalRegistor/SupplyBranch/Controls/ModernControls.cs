
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
    internal static class DrawUtil
    {
        public static GraphicsPath RoundedRect(Rectangle bounds, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (bounds.Width <= 0 || bounds.Height <= 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

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

        public static void ApplyRoleColor(Control control, PanelRole role, ThemePalette palette)
        {
            if (control == null || palette == null)
                return;

            switch (role)
            {
                case PanelRole.Background:
                    control.BackColor = palette.Background;
                    break;
                case PanelRole.Surface:
                    control.BackColor = palette.Surface;
                    break;
                case PanelRole.SurfaceAlt:
                    control.BackColor = palette.SurfaceAlt;
                    break;
                case PanelRole.Sidebar:
                    control.BackColor = palette.Sidebar;
                    break;
                case PanelRole.Header:
                    control.BackColor = palette.Header;
                    break;
                case PanelRole.Transparent:
                    control.BackColor = Color.Transparent;
                    break;
            }
        }
    }

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
            DrawUtil.ApplyRoleColor(this, _role, AppTheme.P);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                AppTheme.ThemeChanged -= HandleThemeChanged;

            base.Dispose(disposing);
        }
    }

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

            _offset += delta;
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
            if (m.Msg != WM_MOUSEWHEEL || _content == null || MaxOffset >= 0 || !IsHandleCreated || !Visible)
                return false;

            Form ownerForm = FindForm();
            if (ownerForm != null && (ownerForm.IsDisposed || !ownerForm.Visible || Form.ActiveForm != ownerForm))
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
            DrawUtil.ApplyRoleColor(this, _role, AppTheme.P);
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
            DrawUtil.ApplyRoleColor(this, _role, AppTheme.P);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                AppTheme.ThemeChanged -= HandleThemeChanged;

            base.Dispose(disposing);
        }
    }

    /// <summary>A card with rounded corners, subtle border, and optional hover accent.</summary>
    public class CardPanel : Panel
    {
        private int _cornerRadius = 14;
        private int _borderWidth = 1;
        private bool _useAltSurface;
        private bool _hoverHighlight;
        private bool _hovered;

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

        public bool HoverHighlight
        {
            get { return _hoverHighlight; }
            set { _hoverHighlight = value; Invalidate(); }
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

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (_hoverHighlight)
            {
                _hovered = true;
                Invalidate();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (_hovered)
            {
                _hovered = false;
                Invalidate();
            }
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

            ThemePalette p = AppTheme.P;
            Color borderColor = (_hoverHighlight && _hovered) ? p.Primary : p.Border;

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle r = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = DrawUtil.RoundedRect(r, _cornerRadius))
            using (Pen pen = new Pen(borderColor, _borderWidth))
            {
                e.Graphics.DrawPath(pen, path);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                AppTheme.ThemeChanged -= HandleThemeChanged;
                if (Region != null)
                {
                    Region.Dispose();
                    Region = null;
                }
            }

            base.Dispose(disposing);
        }
    }

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

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
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
                    using (Pen pen = new Pen(border, Focused && Enabled ? 1.8F : 1F))
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
            border = (Focused && Enabled) ? p.Primary : Color.Transparent;

            switch (_variant)
            {
                case ButtonVariant.Secondary:
                    back = p.Surface;
                    fore = p.TextPrimary;
                    if (!Focused)
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

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
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

                if (Focused && Enabled && !IsActive)
                {
                    using (Pen focusPen = new Pen(p.Primary, 1.5F))
                        e.Graphics.DrawPath(focusPen, path);
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
            HoverHighlight = true;

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

    /// <summary>Modern input box with optional icon, clear action, password toggle, and error state.</summary>
    public class InputBox : Panel
    {
        private readonly TextBox _textBox;
        private readonly Label _iconLabel;
        private readonly Label _revealLabel;
        private readonly Label _clearLabel;
        private readonly Label _placeholderLabel;

        private bool _focused;
        private bool _revealed;
        private bool _hasError;
        private bool _showClear;
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

        public bool HasError
        {
            get { return _hasError; }
            set { _hasError = value; Invalidate(); }
        }

        public bool ShowClearButton
        {
            get { return _showClear; }
            set
            {
                _showClear = value;
                UpdateActionButtons();
                LayoutChildren();
            }
        }

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
                UpdateActionButtons();
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
            set
            {
                _textBox.ReadOnly = value;
                SyncColors();
                UpdateActionButtons();
                LayoutChildren();
            }
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
                if (_hasError && _textBox.TextLength > 0)
                    _hasError = false;

                UpdatePlaceholderVisibility();
                UpdateActionButtons();
                LayoutChildren();
                Invalidate();

                EventHandler handler = ValueChanged;
                if (handler != null)
                    handler(this, EventArgs.Empty);
            };

            _textBox.Enter += (s, e) =>
            {
                _focused = true;
                SyncColors();
                UpdatePlaceholderVisibility();
                Invalidate();
            };

            _textBox.Leave += (s, e) =>
            {
                _focused = false;
                SyncColors();
                UpdatePlaceholderVisibility();
                Invalidate();
            };

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

            _clearLabel = new Label
            {
                AutoSize = false,
                Size = new Size(22, 22),
                Text = ThemeIcons.Cancel,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = ThemeFonts.IconSmall,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Visible = false,
                ForeColor = AppTheme.P.TextMuted
            };
            _clearLabel.Click += (s, e) =>
            {
                _textBox.Clear();
                _textBox.Focus();
            };

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
            Controls.Add(_clearLabel);
            Controls.Add(_revealLabel);
            Controls.Add(_placeholderLabel);
            _placeholderLabel.BringToFront();

            AppTheme.ThemeChanged += HandleThemeChanged;
        }

        private void UpdateActionButtons()
        {
            if (_clearLabel != null)
                _clearLabel.Visible = _showClear && Enabled && !_textBox.ReadOnly && _textBox.TextLength > 0;

            if (_revealLabel != null)
                _revealLabel.Visible = _showReveal;
        }

        private void UpdatePlaceholderVisibility()
        {
            if (_placeholderLabel == null || _textBox == null)
                return;

            _placeholderLabel.Visible =
                !_focused && _textBox.TextLength == 0 && !string.IsNullOrEmpty(_placeholder);
        }

        private void SyncColors()
        {
            ThemePalette p = AppTheme.P;
            Color boxBg = (!Enabled || _textBox.ReadOnly) ? p.SurfaceAlt : p.Surface;
            Color boxFg = (!Enabled) ? p.TextMuted : p.TextPrimary;

            BackColor = boxBg;
            _textBox.BackColor = boxBg;
            _textBox.ForeColor = boxFg;
            _iconLabel.ForeColor = _hasError ? p.Danger : (_focused ? p.Primary : p.TextMuted);
            _revealLabel.ForeColor = _revealed ? p.Primary : p.TextMuted;
            _clearLabel.ForeColor = p.TextMuted;
            _placeholderLabel.ForeColor = p.TextMuted;
        }

        private void HandleThemeChanged(object sender, EventArgs e)
        {
            SyncColors();
            UpdateRegion();
            Invalidate();
        }

        private void ToggleReveal()
        {
            _revealed = !_revealed;
            _textBox.UseSystemPasswordChar = !_revealed;
            SyncColors();
            _textBox.Focus();
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            _textBox.Enabled = Enabled;
            SyncColors();
            UpdateActionButtons();
            LayoutChildren();
            Invalidate();
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
            if (_textBox == null || _iconLabel == null || _revealLabel == null || _clearLabel == null)
                return;

            int iconWidth = string.IsNullOrEmpty(_iconGlyph) ? 0 : 40;
            int rightCursor = Width - 8;

            _iconLabel.Visible = iconWidth > 0;
            _iconLabel.Location = new Point(14, (Height - _iconLabel.Height) / 2);

            if (_revealLabel.Visible)
            {
                _revealLabel.Location = new Point(rightCursor - _revealLabel.Width, (Height - _revealLabel.Height) / 2);
                rightCursor = _revealLabel.Left - 4;
            }

            if (_clearLabel.Visible)
            {
                _clearLabel.Location = new Point(rightCursor - _clearLabel.Width, (Height - _clearLabel.Height) / 2);
                rightCursor = _clearLabel.Left - 4;
            }

            int textLeft = 14 + iconWidth;
            int textWidth = Math.Max(20, rightCursor - textLeft - 4);

            _textBox.Location = new Point(textLeft, (Height - _textBox.Height) / 2);
            _textBox.Width = textWidth;

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

            ThemePalette p = AppTheme.P;
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            Color borderColor = _hasError ? p.Danger : (_focused ? p.Primary : p.Border);
            float borderWidth = (_hasError || _focused) ? 1.8F : 1F;

            Rectangle bounds = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = DrawUtil.RoundedRect(bounds, 10))
            using (Pen pen = new Pen(borderColor, borderWidth))
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
            {
                AppTheme.ThemeChanged -= HandleThemeChanged;
                if (Region != null)
                {
                    Region.Dispose();
                    Region = null;
                }
            }

            base.Dispose(disposing);
        }
    }

    public class ModernComboBox : ComboBox
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct COMBOBOXINFO
        {
            public int cbSize;
            public RECT rcItem;
            public RECT rcButton;
            public int stateButton;
            public IntPtr hwndCombo;
            public IntPtr hwndItem;
            public IntPtr hwndList;
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        [DllImport("user32.dll")]
        private static extern bool GetComboBoxInfo(IntPtr hwnd, ref COMBOBOXINFO info);

        public ModernComboBox()
        {
            DrawMode = DrawMode.OwnerDrawFixed;
            FlatStyle = FlatStyle.Flat;
            IntegralHeight = true;
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            ThemePalette p = AppTheme.P;

            bool editing = (e.State & DrawItemState.ComboBoxEdit) == DrawItemState.ComboBoxEdit;
            bool selected = !editing && (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color back = editing
                ? BackColor
                : (selected ? p.GridSelection : p.Surface);

            Color fore = editing
                ? ForeColor
                : (selected ? p.GridSelectionText : p.TextPrimary);

            using (SolidBrush brush = new SolidBrush(back))
                e.Graphics.FillRectangle(brush, e.Bounds);

            if (e.Index < 0 || e.Index >= Items.Count)
                return;

            string text = GetItemText(Items[e.Index]);

            TextRenderer.DrawText(
                e.Graphics,
                text,
                Font,
                e.Bounds,
                fore,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x000F && IsHandleCreated)
                PaintArrow();
        }

        private void PaintArrow()
        {
            COMBOBOXINFO info = new COMBOBOXINFO();
            info.cbSize = Marshal.SizeOf(typeof(COMBOBOXINFO));

            if (!GetComboBoxInfo(Handle, ref info))
                return;

            Rectangle rect = new Rectangle(
                info.rcButton.Left,
                info.rcButton.Top,
                Math.Max(0, info.rcButton.Right - info.rcButton.Left),
                Math.Max(0, info.rcButton.Bottom - info.rcButton.Top));

            if (rect.Width <= 0 || rect.Height <= 0)
                return;

            ThemePalette p = AppTheme.P;

            try
            {
                using (Graphics g = Graphics.FromHwnd(Handle))
                using (SolidBrush back = new SolidBrush(BackColor))
                {
                    g.FillRectangle(back, rect);

                    int cx = rect.Left + rect.Width / 2;
                    int cy = rect.Top + rect.Height / 2;

                    using (Pen pen = new Pen(Enabled ? p.TextMuted : p.Border, 1.6F))
                    {
                        pen.StartCap = LineCap.Round;
                        pen.EndCap = LineCap.Round;
                        pen.LineJoin = LineJoin.Round;

                        g.SmoothingMode = SmoothingMode.AntiAlias;
                        g.DrawLines(pen, new[]
                        {
                            new Point(cx - 4, cy - 2),
                            new Point(cx, cy + 2),
                            new Point(cx + 4, cy - 2)
                        });
                    }

                    Rectangle client = ClientRectangle;
                    if (client.Width > 2 && client.Height > 2)
                    {
                        using (Pen border = new Pen(p.Border))
                            g.DrawRectangle(border, 0, 0, client.Width - 1, client.Height - 1);
                    }
                }
            }
            catch
            {
                // Painting the overlay is best-effort only.
            }
        }
    }

    public class ModernCheckedListBox : CheckedListBox
    {
        public ModernCheckedListBox()
        {
            BorderStyle = BorderStyle.FixedSingle;
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == 0x000F && IsHandleCreated)
                PaintChecks();
        }

        private void PaintChecks()
        {
            if (Items.Count == 0)
                return;

            ThemePalette p = AppTheme.P;

            try
            {
                using (Graphics g = Graphics.FromHwnd(Handle))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.SetClip(ClientRectangle);

                    using (SolidBrush onFill = new SolidBrush(p.Primary))
                    using (SolidBrush offFill = new SolidBrush(p.SurfaceAlt))
                    using (Pen onBorder = new Pen(p.Primary))
                    using (Pen offBorder = new Pen(p.TextMuted))
                    using (Pen tick = new Pen(p.OnPrimary, 1.8F))
                    {
                        tick.StartCap = LineCap.Round;
                        tick.EndCap = LineCap.Round;
                        tick.LineJoin = LineJoin.Round;

                        for (int i = 0; i < Items.Count; i++)
                        {
                            Rectangle row = GetItemRectangle(i);

                            if (row.Bottom <= 0 || row.Top >= ClientSize.Height)
                                continue;

                            const int size = 13;
                            Rectangle box = new Rectangle(2, row.Top + (row.Height - size) / 2, size, size);

                            bool isChecked = GetItemChecked(i);

                            g.FillRectangle(isChecked ? (Brush)onFill : offFill, box);
                            g.DrawRectangle(isChecked ? onBorder : offBorder, box);

                            if (!isChecked)
                                continue;

                            g.DrawLines(tick, new[]
                            {
                                new Point(box.Left + 3, box.Top + 7),
                                new Point(box.Left + 6, box.Top + 10),
                                new Point(box.Left + 11, box.Top + 4)
                            });
                        }
                    }
                }
            }
            catch
            {
                // Overdrawing is best-effort only.
            }
        }
    }

    public class ModernCheckBox : CheckBox
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintBox(e.Graphics);
        }

        private void PaintBox(Graphics g)
        {
            ThemePalette p = AppTheme.P;
            const int size = 13;
            Rectangle box = new Rectangle(1, (Height - size) / 2, size, size);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush fill = new SolidBrush(Checked ? p.Primary : p.SurfaceAlt))
                g.FillRectangle(fill, box);

            using (Pen border = new Pen(Checked ? p.Primary : p.TextMuted))
                g.DrawRectangle(border, box);

            if (!Checked)
                return;

            using (Pen tick = new Pen(p.OnPrimary, 1.8F))
            {
                tick.StartCap = LineCap.Round;
                tick.EndCap = LineCap.Round;
                tick.LineJoin = LineJoin.Round;

                g.DrawLines(tick, new[]
                {
                    new Point(box.Left + 3, box.Top + 7),
                    new Point(box.Left + 6, box.Top + 10),
                    new Point(box.Left + 11, box.Top + 4)
                });
            }
        }
    }

    public class ModernRadioButton : RadioButton
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            PaintCircle(e.Graphics);
        }

        private void PaintCircle(Graphics g)
        {
            ThemePalette p = AppTheme.P;
            const int size = 13;
            Rectangle box = new Rectangle(1, (Height - size) / 2, size, size);

            g.SmoothingMode = SmoothingMode.AntiAlias;

            using (SolidBrush fill = new SolidBrush(p.SurfaceAlt))
                g.FillEllipse(fill, box);

            using (Pen border = new Pen(Checked ? p.Primary : p.TextMuted, 1.4F))
                g.DrawEllipse(border, box);

            if (!Checked)
                return;

            Rectangle dot = new Rectangle(box.X + 4, box.Y + 4, box.Width - 8, box.Height - 8);

            using (SolidBrush brush = new SolidBrush(p.Primary))
                g.FillEllipse(brush, dot);
        }
    }

    public class ModernNumericUpDown : NumericUpDown
    {
        private Control _buttons;
        private EventHandler _themeHandler;

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            AttachButtons();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            DetachButtons();
            base.OnHandleDestroyed(e);
        }

        private void AttachButtons()
        {
            if (_buttons != null)
                return;

            foreach (Control child in Controls)
            {
                if (child.GetType().Name == "UpDownButtons")
                {
                    _buttons = child;
                    break;
                }
            }

            if (_buttons == null)
                return;

            _buttons.Paint += PaintButtons;

            _themeHandler = delegate { _buttons.Invalidate(); };
            AppTheme.ThemeChanged += _themeHandler;
        }

        private void DetachButtons()
        {
            if (_buttons == null)
                return;

            _buttons.Paint -= PaintButtons;

            if (_themeHandler != null)
            {
                AppTheme.ThemeChanged -= _themeHandler;
                _themeHandler = null;
            }

            _buttons = null;
        }

        private void PaintButtons(object sender, PaintEventArgs e)
        {
            Control buttons = (Control)sender;
            ThemePalette p = AppTheme.P;
            Graphics g = e.Graphics;

            g.Clear(p.SurfaceAlt);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int width = buttons.Width;
            int height = buttons.Height;
            int half = height / 2;

            using (Pen line = new Pen(p.Border))
            {
                g.DrawLine(line, 1, half, width, half);
                g.DrawRectangle(line, 0, 0, width - 1, height - 1);
            }

            using (Pen arrow = new Pen(Enabled ? p.TextMuted : p.Border, 1.5F))
            {
                arrow.StartCap = LineCap.Round;
                arrow.EndCap = LineCap.Round;

                int cx = width / 2;

                int topY = half / 2;
                g.DrawLines(arrow, new[]
                {
                    new Point(cx - 3, topY + 2),
                    new Point(cx, topY - 1),
                    new Point(cx + 3, topY + 2)
                });

                int bottomY = half + (height - half) / 2;
                g.DrawLines(arrow, new[]
                {
                    new Point(cx - 3, bottomY - 2),
                    new Point(cx, bottomY + 1),
                    new Point(cx + 3, bottomY - 2)
                });
            }
        }
    }

    public static class ControlStyler
    {
        private static readonly ConditionalWeakTable<ComboBox, EventHandler> ComboHandlers =
            new ConditionalWeakTable<ComboBox, EventHandler>();

        public static void StyleComboBox(ComboBox combo)
        {
            if (combo == null || combo is ModernComboBox)
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

            if (e.Index >= 0 && e.Index < combo.Items.Count)
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

    public static class ScrollBarStyler
    {
        private static readonly ConditionalWeakTable<ScrollBar, object> Hooked =
            new ConditionalWeakTable<ScrollBar, object>();

        private static readonly ConditionalWeakTable<Control, object> Pending =
            new ConditionalWeakTable<Control, object>();

        public static void Attach(Control parent)
        {
            if (parent == null)
                return;

            if (!parent.IsHandleCreated)
            {
                object marker;
                if (!Pending.TryGetValue(parent, out marker))
                {
                    Pending.Add(parent, new object());
                    Control target = parent;
                    parent.HandleCreated += delegate { Attach(target); };
                }

                return;
            }

            foreach (Control child in parent.Controls)
            {
                ScrollBar bar = child as ScrollBar;
                if (bar == null)
                    continue;

                object marker;
                if (Hooked.TryGetValue(bar, out marker))
                    continue;

                Hooked.Add(bar, new object());
                bar.Paint += PaintBar;
            }
        }

        public static void Refresh(Control parent)
        {
            if (parent == null)
                return;

            foreach (Control child in parent.Controls)
            {
                ScrollBar bar = child as ScrollBar;
                if (bar != null)
                    bar.Invalidate();
            }
        }

        private static void PaintBar(object sender, PaintEventArgs e)
        {
            ScrollBar bar = (ScrollBar)sender;
            ThemePalette p = AppTheme.P;
            Graphics g = e.Graphics;

            bool vertical = bar is VScrollBar;
            int width = bar.ClientSize.Width;
            int height = bar.ClientSize.Height;

            if (width <= 0 || height <= 0)
                return;

            Color track = p.SurfaceAlt;
            Color thumb = AppTheme.IsDark
                ? Color.FromArgb(58, 76, 107)
                : Color.FromArgb(195, 206, 223);
            Color arrowColor = bar.Enabled ? p.TextMuted : p.Border;

            g.Clear(track);
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int arrow = Math.Min(SystemInformation.VerticalScrollBarWidth, vertical ? height / 4 : width / 4);
            if (arrow < 8)
                arrow = 8;

            int min = bar.Minimum;
            int max = bar.Maximum;
            int large = Math.Max(1, bar.LargeChange);
            int range = Math.Max(1, max - min + 1);

            double ratio = (double)large / range;
            double position = range > large
                ? (double)(bar.Value - min) / (range - large)
                : 0;

            if (position < 0) position = 0;
            if (position > 1) position = 1;

            if (vertical)
            {
                int trackTop = arrow;
                int trackHeight = height - arrow * 2;

                if (trackHeight < 4)
                    return;

                int thumbHeight = Math.Max(28, (int)(trackHeight * ratio));
                if (thumbHeight > trackHeight)
                    thumbHeight = trackHeight;

                int thumbTop = trackTop + (int)((trackHeight - thumbHeight) * position);

                using (SolidBrush brush = new SolidBrush(thumb))
                using (GraphicsPath path = DrawUtil.RoundedRect(
                    new Rectangle(3, thumbTop + 1, width - 6, thumbHeight - 2), 4))
                    g.FillPath(brush, path);

                using (Pen pen = new Pen(arrowColor, 1.5F))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;

                    int cx = width / 2;

                    int topY = arrow / 2;
                    g.DrawLines(pen, new[]
                    {
                        new Point(cx - 3, topY + 2),
                        new Point(cx, topY - 1),
                        new Point(cx + 3, topY + 2)
                    });

                    int bottomY = height - arrow / 2;
                    g.DrawLines(pen, new[]
                    {
                        new Point(cx - 3, bottomY - 2),
                        new Point(cx, bottomY + 1),
                        new Point(cx + 3, bottomY - 2)
                    });
                }
            }
            else
            {
                int trackLeft = arrow;
                int trackWidth = width - arrow * 2;

                if (trackWidth < 4)
                    return;

                int thumbWidth = Math.Max(28, (int)(trackWidth * ratio));
                if (thumbWidth > trackWidth)
                    thumbWidth = trackWidth;

                int thumbLeft = trackLeft + (int)((trackWidth - thumbWidth) * position);

                using (SolidBrush brush = new SolidBrush(thumb))
                using (GraphicsPath path = DrawUtil.RoundedRect(
                    new Rectangle(thumbLeft + 1, 3, thumbWidth - 2, height - 6), 4))
                    g.FillPath(brush, path);

                using (Pen pen = new Pen(arrowColor, 1.5F))
                {
                    pen.StartCap = LineCap.Round;
                    pen.EndCap = LineCap.Round;

                    int cy = height / 2;

                    int leftX = arrow / 2;
                    g.DrawLines(pen, new[]
                    {
                        new Point(leftX + 2, cy - 3),
                        new Point(leftX - 1, cy),
                        new Point(leftX + 2, cy + 3)
                    });

                    int rightX = width - arrow / 2;
                    g.DrawLines(pen, new[]
                    {
                        new Point(rightX - 2, cy - 3),
                        new Point(rightX + 1, cy),
                        new Point(rightX - 2, cy + 3)
                    });
                }
            }

            using (Pen edge = new Pen(p.Border))
            {
                if (vertical)
                    g.DrawLine(edge, 0, 0, 0, height);
                else
                    g.DrawLine(edge, 0, 0, width, 0);
            }
        }
    }

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
            grid.ColumnHeadersHeight = 36;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersDefaultCellStyle.BackColor = p.GridHeader;
            grid.ColumnHeadersDefaultCellStyle.ForeColor = p.GridHeaderText;
            grid.ColumnHeadersDefaultCellStyle.SelectionBackColor = p.GridHeader;
            grid.ColumnHeadersDefaultCellStyle.SelectionForeColor = p.GridHeaderText;
            grid.ColumnHeadersDefaultCellStyle.Font = ThemeFonts.BodyBold;
            grid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            grid.DefaultCellStyle.BackColor = p.GridRow;
            grid.DefaultCellStyle.ForeColor = p.TextPrimary;
            grid.DefaultCellStyle.SelectionBackColor = p.GridSelection;
            grid.DefaultCellStyle.SelectionForeColor = p.GridSelectionText;
            grid.DefaultCellStyle.Font = ThemeFonts.Body;
            grid.DefaultCellStyle.Padding = new Padding(8, 0, 8, 0);

            grid.AlternatingRowsDefaultCellStyle.BackColor = p.GridRowAlt;
            grid.RowTemplate.Height = 33;

            AppTheme.ApplyScrollbarTheme(grid);
            ScrollBarStyler.Attach(grid);
            ScrollBarStyler.Refresh(grid);
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