using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class ThemNhanVien : Form
    {
        public ThemNhanVien()
        {
            InitializeComponent();
        }


        #region Custom TextBox cho Form Nhân viên
        private Panel CreateEmployeeTextBox(string placeholder, int width, int height)
        {

            int borderSize = 2;
            int borderRadius = 12; // bo góc nè
            Color borderColor = Color.FromArgb(0, 152, 70); // xanh đồng bộ
            Color placeholderColor = Color.Silver;
            Color textColor = Color.FromArgb(64, 64, 64);

            Panel container = new Panel();
            container.Size = new Size(width, height);
            container.BackColor = Color.White;
            container.Padding = new Padding(borderSize);

            // Vẽ border bo tròn
            container.Paint += (s, e) =>
            {
                e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                using (GraphicsPath path = new GraphicsPath())
                {
                    int arc = borderRadius * 2;
                    path.AddArc(0, 0, arc, arc, 180, 90); // Góc trái trên
                    path.AddArc(container.Width - arc - 1, 0, arc, arc, 270, 90); // Góc phải trên
                    path.AddArc(container.Width - arc - 1, container.Height - arc - 1, arc, arc, 0, 90); // Góc phải dưới
                    path.AddArc(0, container.Height - arc - 1, arc, arc, 90, 90); // Góc trái dưới
                    path.CloseFigure();

                    using (Pen pen = new Pen(borderColor, borderSize))
                    {
                        e.Graphics.DrawPath(pen, path);
                    }
                }
            };

            // Tạo TextBox
            TextBox txt = new TextBox();
            txt.BorderStyle = BorderStyle.None;
            txt.Font = new Font("Segoe UI", 10F);
            txt.ForeColor = placeholderColor;
            txt.Text = placeholder;
            txt.Location = new Point(borderRadius / 2, (height - txt.Height) / 2);
            txt.Width = width - borderRadius;
            txt.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            // Placeholder logic
            txt.Enter += (s, e) =>
            {
                if (txt.Text == placeholder && txt.ForeColor == placeholderColor)
                {
                    txt.Text = "";
                    txt.ForeColor = textColor;
                }
            };
            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = placeholderColor;
                }
            };

            container.Controls.Add(txt);
            return container;
        }
        #endregion



        private void ThemNhanVien_Load(object sender, EventArgs e)
        {
            // TextBox Email
            Panel txtEmail = CreateEmployeeTextBox("Nhập email", 300, 35);
            txtEmail.Location = new Point(30, 80);
            panel3.Controls.Add(txtEmail);

            Panel txtHoTen = CreateEmployeeTextBox("Nhập họ tên nhân viên", 300, 35);
            txtHoTen.Location = new Point(30, 30);
            panel3.Controls.Add(txtHoTen);


            Panel txtPhone = CreateEmployeeTextBox("Nhập số điện thoại", 300, 35);
            txtPhone.Location = new Point(30, 130);
            panel3.Controls.Add(txtPhone);

        }
    }
}
