using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GUI.Helper
{
    public class ModernPanelFlow4Goc : FlowLayoutPanel
    {
        public int BorderRadius { get; set; } = 20;
        public Color BackColorFill { get; set; } = Color.White;
        public Color BorderColor { get; set; } = Color.FromArgb(200, 200, 200);
        public bool DrawTextOnPanel { get; set; } = false;

        private GraphicsPath _path;

        public ModernPanelFlow4Goc()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.White;    
            BackColorFill = Color.White;

            TabStop = false;
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
            _path = BuildPathAllCorners(ClientRectangle, r);   // ✅ bo 4 góc

            Region?.Dispose();
            Region = new Region(_path);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_path == null)
                RebuildPathAndRegion();

            // Nền panel
            using (var brush = new SolidBrush(BackColorFill))
                g.FillPath(brush, _path);

            // Viền panel
            using (var pen = new Pen(BorderColor, 2.2f))
            {
                pen.Alignment = PenAlignment.Inset;
                g.DrawPath(pen, _path);
            }


            // Vẽ text giữa panel (nếu bật)
            if (DrawTextOnPanel && !string.IsNullOrEmpty(Text))
            {
                TextRenderer.DrawText(
                    g,
                    Text,
                    Font,
                    ClientRectangle,
                    ForeColor,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                );
            }

            base.OnPaint(e);
        }

        // 🔽 Bo tròn 4 góc
        private static GraphicsPath BuildPathAllCorners(Rectangle r, int radius)
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

            // Góc trên trái
            path.AddArc(r.Left, r.Top, d, d, 180, 90);
            // Cạnh trên
            path.AddLine(r.Left + radius, r.Top, r.Right - radius, r.Top);

            // Góc trên phải
            path.AddArc(r.Right - d, r.Top, d, d, 270, 90);
            // Cạnh phải
            path.AddLine(r.Right, r.Top + radius, r.Right, r.Bottom - radius);

            // Góc dưới phải
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            // Cạnh dưới
            path.AddLine(r.Right - radius, r.Bottom, r.Left + radius, r.Bottom);

            // Góc dưới trái
            path.AddArc(r.Left, r.Bottom - d, d, d, 90, 90);
            // Cạnh trái
            path.AddLine(r.Left, r.Bottom - radius, r.Left, r.Top + radius);

            path.CloseFigure();
            return path;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _path?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
