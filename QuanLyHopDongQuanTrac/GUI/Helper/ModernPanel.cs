using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI.Helper
{
    public class ModernPanel : Panel
    {
        public int BorderRadius { get; set; } = 20;
        public Color BackColorFill { get; set; } = Color.White;  // màu nền chính
        public Color BorderColor { get; set; } = Color.FromArgb(200, 200, 200);
        public bool DrawTextOnPanel { get; set; } = false;

        private GraphicsPath _path;

        public ModernPanel()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            TabStop = false;
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Parent == null) { base.OnPaintBackground(e); return; }

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            var state = g.Save();
            try
            {
                g.TranslateTransform(-Left, -Top);
                using (var pea = new PaintEventArgs(g, Parent.ClientRectangle))
                {
                    InvokePaintBackground(Parent, pea);
                    InvokePaint(Parent, pea);
                }
            }
            finally { g.Restore(state); }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            RebuildPathAndRegion();
        }

        private void RebuildPathAndRegion()
        {
            _path?.Dispose();
            int r = Math.Min(BorderRadius, Math.Min(Width, Height) / 2);
            _path = BuildPathBottomCornersOnly(ClientRectangle, r);
            Region?.Dispose();
            Region = new Region(_path);
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_path == null) RebuildPathAndRegion();

            using (var brush = new SolidBrush(BackColorFill))
                g.FillPath(brush, _path);

            using (var pen = new Pen(BorderColor, 1.2f))
                g.DrawPath(pen, _path);

            if (DrawTextOnPanel && !string.IsNullOrEmpty(Text))
            {
                TextRenderer.DrawText(g, Text, Font, ClientRectangle, ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }

            base.OnPaint(e);
        }

        private static GraphicsPath BuildPathBottomCornersOnly(Rectangle r, int radius)
        {
            r = Rectangle.Inflate(r, -1, -1);
            int d = Math.Max(0, radius * 2);
            var path = new GraphicsPath();

            if (d <= 0)
            {
                path.AddRectangle(r);
                return path;
            }

            path.StartFigure();
            path.AddLine(r.Left, r.Top, r.Right, r.Top);
            path.AddLine(r.Right, r.Top, r.Right, r.Bottom - radius);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddLine(r.Right - radius, r.Bottom, r.Left + radius, r.Bottom);
            path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            path.AddLine(r.Left, r.Bottom - radius, r.Left, r.Top);
            path.CloseFigure();

            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) _path?.Dispose();
            base.Dispose(disposing);
        }
    }
}
