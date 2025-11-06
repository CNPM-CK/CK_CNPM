using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class DanhSachKhachHang : Form
    {
        #region Fields
        //private System.Windows.Forms.Timer searchTimer;

        private Color borderColor = Color.Black;
        private int borderRadius = 12;
        private int borderSize = 2;

        // Layout constants
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;
        private const int SEARCH_HEIGHT = 50;
        private const string PLACEHOLDER_TEXT = "Tìm kiếm khách hàng...";

        private BindingList<KhachHang> dsKhachhang;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";
        private Form currentOpenForm = null;

        public DanhSachKhachHang()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );
        #endregion

        #region Form Load
        private void DanhSachNhanVien_Load(object sender, EventArgs e)
        {
            KhachHangBLL khBLL = new KhachHangBLL();
            dsKhachhang = new BindingList<KhachHang>(khBLL.layDanhSachKH());

            InitializeButtonIcons();
            InitializeSettingMenu();
            CalculateLayout();
        }



        private void RefreshDanhSachKhachHang()
        {
            KhachHangBLL khBLL = new KhachHangBLL();
            dsKhachhang.Clear();
            foreach (var kh in khBLL.layDanhSachKH())
                dsKhachhang.Add(kh);
        }

        private Rectangle editRect;
        private Rectangle deleteRect;


        private void CenterFormOnParent(Form childForm)
        {
            childForm.StartPosition = FormStartPosition.Manual;
            childForm.Location = new Point(
                this.Location.X + (this.Width - childForm.Width) / 2,
                this.Location.Y + (this.Height - childForm.Height) / 2
            );
        }


        private void InitializeSettingMenu()
        {
            ContextMenuStrip settingMenu = new ContextMenuStrip();

            ToolStripMenuItem personalItem = new ToolStripMenuItem("Cài đặt cá nhân");
            personalItem.Click += (s, ev) =>
            {
                //MessageBox.Show("Mở trang cài đặt cá nhân...", "Thông báo");
                // Xóa toàn bộ control cũ trên panel5
                panel5.Controls.Clear();

                // Khởi tạo user control TrangCaNhanUC
                TrangCaNhan trangCaNhanUC = new TrangCaNhan();
                trangCaNhanUC.Dock = DockStyle.Fill;

                // Thêm vào panel5
                panel5.Controls.Add(trangCaNhanUC);
                trangCaNhanUC.BringToFront();
            };

            ToolStripMenuItem logoutItem = new ToolStripMenuItem("Đăng xuất");
            logoutItem.Click += (s, ev) =>
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn đăng xuất không?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    this.Hide();
                    DangNhap loginForm = new DangNhap();
                    loginForm.Show();
                }
            };

            settingMenu.Items.Add(personalItem);
            settingMenu.Items.Add(new ToolStripSeparator());
            settingMenu.Items.Add(logoutItem);

            pictureBoxSetting.Click += (s, ev) =>
            {
                settingMenu.Show(pictureBoxSetting, new Point(0, pictureBoxSetting.Height));
            };
        }

        private void InitializeButtonIcons()
        {
            if (btnDanhsachnv.Image != null)
            {
                btnDanhsachnv.Image = new Bitmap(btnDanhsachnv.Image, new Size(27, 27));
                btnDanhsachnv.ImageAlign = ContentAlignment.MiddleLeft;
                btnDanhsachnv.TextAlign = ContentAlignment.MiddleRight;
                btnDanhsachnv.Padding = new Padding(0, 0, 4, 0);
            }
        }
        #endregion

        #region Layout & Resize
        private void DanhSachNhanVien_Resize(object sender, EventArgs e)
        {
            if (this.ClientSize.Width < 100) return;
            CalculateLayout();
        }

        private void CalculateLayout()
        {
            //int formWidth = this.ClientSize.Width;
            //bool isMaximized = this.WindowState == FormWindowState.Maximized;
            //int btnWidth = isMaximized ? 80 : 66;
            //int btnHeight = isMaximized ? 50 : 40;
            //int btnRadius = isMaximized ? 25 : 20;

            //btnXuatfile.Size = new Size(btnWidth, btnHeight);
            //btnThemuser.Size = new Size(btnWidth, btnHeight);
            //BoGocButton(btnXuatfile, btnRadius);
            //BoGocButton(btnThemuser, btnRadius);

            //btnXuatfile.Left = formWidth - btnWidth - MARGIN;
            //btnThemuser.Left = btnXuatfile.Left - btnWidth - SPACING;

            //pictureFilter.Left = MARGIN;

            //int leftBoundary = pictureFilter.Right + SPACING;
            //int rightBoundary = btnThemuser.Left - SPACING - picturemicro.Width - SPACING;
            //int availableWidth = rightBoundary - leftBoundary;

            //int searchWidth = Math.Max(MIN_SEARCH_WIDTH, Math.Min(availableWidth, MAX_SEARCH_WIDTH));
            //if (searchWidth < MIN_SEARCH_WIDTH)
            //{
            //    searchWidth = Math.Max(150, availableWidth);
            //}

            //containersearch.Left = leftBoundary;
            //containersearch.Width = searchWidth;
            //containersearch.Height = SEARCH_HEIGHT;

            //searchtextbox.Width = searchWidth - (borderSize * 2 + 10);
            //searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);

            //picturemicro.Left = containersearch.Right + SPACING;

            //if (isMaximized)
            //{
            //    btnThemuser.Padding = new Padding(10, 5, 10, 5);
            //    btnXuatfile.Padding = new Padding(10, 5, 10, 5);
            //}
            //else
            //{
            //    btnThemuser.Padding = new Padding(5, 3, 5, 3);
            //    btnXuatfile.Padding = new Padding(5, 3, 5, 3);
            //}

            //containersearch.Invalidate();
        }
        #endregion

        #region Button Styling
        private void BoGocButton(Button btn, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(new Rectangle(0, 0, radius, radius), 180, 90);
            path.AddLine(radius, 0, btn.Width - radius, 0);
            path.AddArc(new Rectangle(btn.Width - radius, 0, radius, radius), -90, 90);
            path.AddLine(btn.Width, radius, btn.Width, btn.Height - radius);
            path.AddArc(new Rectangle(btn.Width - radius, btn.Height - radius, radius, radius), 0, 90);
            path.AddLine(btn.Width - radius, btn.Height, radius, btn.Height);
            path.AddArc(new Rectangle(0, btn.Height - radius, radius, radius), 90, 90);
            path.CloseFigure();
            btn.Region = new Region(path);
        }
        #endregion



        private GraphicsPath CreateRoundedRectPath(RectangleF rect, float radius)
        {
            float effectiveRadius = Math.Min(radius, Math.Min(rect.Width / 2f, rect.Height / 2f));
            float diameter = effectiveRadius * 2f;

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }








        #region Button Events
        private void btnThemuser_Click(object sender, EventArgs e)
        {
            if (currentOpenForm != null && !currentOpenForm.IsDisposed)
            {
                currentOpenForm.BringToFront();
                currentOpenForm.Focus();
                MessageBox.Show("Vui lòng hoàn thành thao tác hiện tại trước khi thực hiện thao tác mới!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ThemKhachHang frmThem = new ThemKhachHang();
            currentOpenForm = frmThem; // Lưu reference

            CenterFormOnParent(frmThem);

            // Xử lý khi form đóng
            frmThem.FormClosed += (s, ev) =>
            {
                currentOpenForm = null; // Xóa reference khi form đóng
            };

            frmThem.SuccesfullyUpdated += (s, ev) => RefreshDanhSachKhachHang();
            frmThem.Show(this);
        }

        private void dgvDanhsachnhanvien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }
        #endregion

        #region Unused Events
        private void DanhSachNhanVien_Click(object sender, EventArgs e) { }
        private void pictureBox5_Click(object sender, EventArgs e) { }
        private void pictureBox4_Click(object sender, EventArgs e) { }
        private void dgvDanhsachnhanvien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void containersearch_Paint_1(object sender, PaintEventArgs e) { }
        private void btnDanhsachnv_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DanhSachKhachHanguc danhSachKhachHanguc = new DanhSachKhachHanguc();
            danhSachKhachHanguc.Dock = DockStyle.Fill;
            panel5.Controls.Add(danhSachKhachHanguc);
            danhSachKhachHanguc.BringToFront();
        }
        private void searchtextbox_TextChanged(object sender, EventArgs e) { }
        #endregion

        private void pictureBoxSetting_Click(object sender, EventArgs e)
        {

        }
    }
}