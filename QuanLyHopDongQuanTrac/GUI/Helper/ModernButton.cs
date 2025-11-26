using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Helper
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    public class ModernButton : Button
    {
        public int BorderRadius { get; set; } = 20;
        public Color BackColorNormal { get; set; } = Color.FromArgb(52, 152, 219);
        public Color BackColorHover { get; set; } = Color.FromArgb(41, 128, 185);
        public Color BorderColor { get; set; } = Color.FromArgb(180, 180, 180);

        private bool isHover;
        private GraphicsPath _path;   // cache path để dùng cho vẽ + hit-test

        public ModernButton()
        {
            // Owner-draw, mượt, redraw khi resize, nền trong suốt
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.SupportsTransparentBackColor, true);

            BackColor = Color.Transparent;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            UseVisualStyleBackColor = false;
            TabStop = true;
        }

        protected override bool ShowFocusCues => false;

        // Vẽ nền parent vào vùng nút để "giả trong suốt" (loại ô vuông nền)
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            if (Parent == null) { base.OnPaintBackground(e); return; }

            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            // dịch hệ trục để vẽ nền parent vào đúng vị trí của button
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
            RebuildPathAndRegion();     // cập nhật Region -> hitbox theo bo tròn
        }

        private void RebuildPathAndRegion()
        {
            _path?.Dispose();
            _path = BuildRoundPath(ClientRectangle, Math.Min(BorderRadius, Math.Min(Width, Height) / 2));
            // Region quyết định cả vẽ & hit-test: click ngoài vùng bo sẽ không nhận
            Region?.Dispose();
            Region = new Region(_path);
            Invalidate();
        }

        [Browsable(true)]
        [Category("Appearance")]
        public Image ButtonImage { get; set; }

        [Browsable(true)]
        [Category("Appearance")]
        public int ImageTextPadding { get; set; } = 6;

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            if (_path == null)
                RebuildPathAndRegion();

            // --- Vẽ nền ---
            using (var brush = new SolidBrush(isHover ? BackColorHover : BackColorNormal))
                g.FillPath(brush, _path);

            // --- Vẽ viền ---
            using (var pen = new Pen(BorderColor, 1.2f))
                g.DrawPath(pen, _path);

            Rectangle textRect = ClientRectangle;

            // --- Vẽ ảnh (nếu có) ---
            if (ButtonImage != null)
            {
                // Vùng tối đa cho icon (chừa 10px mỗi bên)
                int maxImgWidth = Width - 20;
                int maxImgHeight = Height - 10;

                // Tỉ lệ scale
                float ratioW = (float)maxImgWidth / ButtonImage.Width;
                float ratioH = (float)maxImgHeight / ButtonImage.Height;
                float scale = Math.Min(ratioW, ratioH);

                int drawW = (int)(ButtonImage.Width * scale);
                int drawH = (int)(ButtonImage.Height * scale);

                int imgX = 10; // hoặc (Width - drawW) / 2 nếu muốn icon ở giữa
                int imgY = (Height - drawH) / 2;

                g.DrawImage(ButtonImage, new Rectangle(imgX, imgY, drawW, drawH));

                int xText = imgX + drawW + ImageTextPadding;
                textRect = new Rectangle(
                    xText,
                    0,
                    Width - xText - 10,
                    Height
                );
            }


            // --- Vẽ chữ ---
            if (!string.IsNullOrEmpty(Text))
            {
                TextFormatFlags flags =
                    TextFormatFlags.HorizontalCenter |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.EndEllipsis;

                TextRenderer.DrawText(
                    g,
                    Text,
                    Font,
                    textRect,
                    ForeColor,
                    flags
                );
            }
        }




        protected override void OnMouseEnter(EventArgs e) { isHover = true; Invalidate(); base.OnMouseEnter(e); }
        protected override void OnMouseLeave(EventArgs e) { isHover = false; Invalidate(); base.OnMouseLeave(e); }

        // Phòng trường hợp Region chưa kịp cập nhật: chặn click ngoài bo tròn
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (_path != null && !_path.IsVisible(e.Location)) return;
            base.OnMouseDown(e);
        }

        private static GraphicsPath BuildRoundPath(Rectangle r, int radius)
        {
            // trừ 1px để viền không bị cắt
            r = Rectangle.Inflate(r, -1, -1);
            int d = Math.Max(0, radius * 2);
            var path = new GraphicsPath();
            if (d <= 0) { path.AddRectangle(r); return path; }

            path.StartFigure();
            path.AddArc(r.X, r.Y, d, d, 180, 90);
            path.AddArc(r.Right - d, r.Y, d, d, 270, 90);
            path.AddArc(r.Right - d, r.Bottom - d, d, d, 0, 90);
            path.AddArc(r.X, r.Bottom - d, d, d, 90, 90);
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
