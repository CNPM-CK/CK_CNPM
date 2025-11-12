using BLL;
using DTO;
using GUI.Common;
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
    public partial class TrangChu : Form
    {
        #region Fields

        private Color borderColor = Color.Black;
        private int borderRadius = 12;
        private int borderSize = 2;

        // Layout constants
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;
        private const int SEARCH_HEIGHT = 50;
        private const string PLACEHOLDER_TEXT = "Tìm kiếm nhân viên...";

        private BindingList<NhanVien> dsNhanVien;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";
        private Form currentOpenForm = null;



        public TrangChu()
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
            InitializeButtonIcons();
            InitializeSettingMenu();
            taiAnhDaiDienNguoiDung();
            TrangCaNhan.anhDaiDienDaThayDoi += taiAnhDaiDienNguoiDung;

        }


        private Rectangle editRect;
        private Rectangle deleteRect;

        private void dgvDanhsachnhanvien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

        }

        private void dgvDanhsachnhanvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

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
                panel5.Controls.Clear();
                TrangCaNhan trangCaNhan = new TrangCaNhan();
                trangCaNhan.Dock = DockStyle.Fill;
                panel5.Controls.Add(trangCaNhan);
                trangCaNhan.BringToFront();
            };


            ToolStripMenuItem logoutItem = new ToolStripMenuItem("Đăng xuất");
            logoutItem.Click += (s, ev) =>
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn đăng xuất không?", "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    this.Hide();
                    SessionStore.Current.SignOut();
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
                btnDanhsachnv.Image = new Bitmap(btnDanhsachnv.Image, new Size(30, 30));
                btnDanhsachnv.ImageAlign = ContentAlignment.MiddleLeft;
                btnDanhsachnv.TextAlign = ContentAlignment.MiddleLeft;
                btnDanhsachnv.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnDanhsachnv.Padding = new Padding(5, 0, 0, 0);
            }
            if (btnDanhSachKhachHang.Image != null)
            {
                btnDanhSachKhachHang.Image = new Bitmap(btnDanhSachKhachHang.Image, new Size(30, 30));
                btnDanhSachKhachHang.ImageAlign = ContentAlignment.MiddleLeft;
                btnDanhSachKhachHang.TextAlign = ContentAlignment.MiddleLeft;
                btnDanhSachKhachHang.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnDanhSachKhachHang.Padding = new Padding(5, 0, 0, 0);
            }
            if (btnDanhSachHopDong.Image != null)
            {
                btnDanhSachHopDong.Image = new Bitmap(btnDanhSachHopDong.Image, new Size(30, 30));
                btnDanhSachHopDong.ImageAlign = ContentAlignment.MiddleLeft;
                btnDanhSachHopDong.TextAlign = ContentAlignment.MiddleLeft;
                btnDanhSachHopDong.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnDanhSachHopDong.Padding = new Padding(5, 0, 0, 0);
            }
            if (btnDanhSachDotQT.Image != null)
            {
                btnDanhSachDotQT.Image = new Bitmap(btnDanhSachDotQT.Image, new Size(30, 30));
                btnDanhSachDotQT.ImageAlign = ContentAlignment.MiddleLeft;
                btnDanhSachDotQT.TextAlign = ContentAlignment.MiddleLeft;
                btnDanhSachDotQT.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnDanhSachDotQT.Padding = new Padding(5, 0, 0, 0);
            }
            if (btnDanhSachNenMau.Image != null)
            {
                btnDanhSachNenMau.Image = new Bitmap(btnDanhSachDotQT.Image, new Size(30, 30));
                btnDanhSachNenMau.ImageAlign = ContentAlignment.MiddleLeft;
                btnDanhSachNenMau.TextAlign = ContentAlignment.MiddleLeft;
                btnDanhSachNenMau.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnDanhSachNenMau.Padding = new Padding(5, 0, 0, 0);
            }
            if (btnDanhSachThongSo.Image != null)
            {
                btnDanhSachThongSo.Image = new Bitmap(btnDanhSachDotQT.Image, new Size(30, 30));
                btnDanhSachThongSo.ImageAlign = ContentAlignment.MiddleLeft;
                btnDanhSachThongSo.TextAlign = ContentAlignment.MiddleLeft;
                btnDanhSachThongSo.TextImageRelation = TextImageRelation.ImageBeforeText;
                btnDanhSachThongSo.Padding = new Padding(5, 0, 0, 0);
            }
        }
        #endregion


        #region Unused Events
        private void DanhSachNhanVien_Click(object sender, EventArgs e) { }
        private void pictureBox5_Click(object sender, EventArgs e) { }
        private void pictureBox4_Click(object sender, EventArgs e) { }
        private void dgvDanhsachnhanvien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void containersearch_Paint_1(object sender, PaintEventArgs e) { }
        private void searchtextbox_TextChanged(object sender, EventArgs e) { }
        private void button1_Click(object sender, EventArgs e) { }
        #endregion

        private void btnDanhsachnv_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DSNV_Uc ucDSNV = new DSNV_Uc();
            ucDSNV.Dock = DockStyle.Fill;
            panel5.Controls.Add(ucDSNV);
            ucDSNV.BringToFront();
        }

        private void btnDanhSachKhachHang_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DanhSachKhachHanguc DSKHuc = new DanhSachKhachHanguc();
            DSKHuc.Dock = DockStyle.Fill;
            panel5.Controls.Add(DSKHuc);
            DSKHuc.BringToFront();
        }

        private void btnDanhSachHopDong_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DanhSachHopDonguc DSHDuc = new DanhSachHopDonguc();
            DSHDuc.Dock = DockStyle.Fill;
            panel5.Controls.Add(DSHDuc);
            DSHDuc.BringToFront();
        }

        private void btnDanhSachDotQT_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DanhSachDotQuanTrac DSDQTuc = new DanhSachDotQuanTrac();
            DSDQTuc.Dock = DockStyle.Fill;
            panel5.Controls.Add(DSDQTuc);
            DSDQTuc.BringToFront();
        }

        private void btnDanhSachNenMau_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DanhSachNenMau DSNMuc = new DanhSachNenMau();
            DSNMuc.Dock = DockStyle.Fill;
            panel5.Controls.Add(DSNMuc);
            DSNMuc.BringToFront();
        }

        private void btnDanhSachThongSo_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DanhSachThongSo DSTSuc = new DanhSachThongSo();
            DSTSuc.Dock = DockStyle.Fill;
            panel5.Controls.Add(DSTSuc);
            DSTSuc.BringToFront();
        }

        private void btnDanhSachNhapLieu_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DanhSachDotNhapLieuUC DSDNLuc = new DanhSachDotNhapLieuUC();
            DSDNLuc.Dock = DockStyle.Fill;
            panel5.Controls.Add(DSDNLuc);
            DSDNLuc.BringToFront();
        }

        private void btnDanhSachKetQua_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DanhSachKetQuaUC DSDNLuc = new DanhSachKetQuaUC();
            DSDNLuc.Dock = DockStyle.Fill;
            panel5.Controls.Add(DSDNLuc);
            DSDNLuc.BringToFront();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            TrangThongBao ttb = new TrangThongBao();
            ttb.Dock = DockStyle.Fill;
            panel5.Controls.Add(ttb);
            ttb.BringToFront();
        }

        private void taiAnhDaiDienNguoiDung()
        {
            try
            {
                string email = SessionStore.Current.UserName;
                if (string.IsNullOrEmpty(email))
                    return;

                NhanVienBLL bll = new NhanVienBLL();
                NhanVien nv = bll.layThongTinCaNhan(email);
                if (nv == null || string.IsNullOrEmpty(nv.anhDaiDien))
                {
                    pictureBoxSetting.Image = Properties.Resources.macdinh; // ảnh mặc định
                    return;
                }

                string avatarsFolder = Path.Combine(Application.StartupPath, "Avatars");
                string imgPath = Path.Combine(avatarsFolder, nv.anhDaiDien);

                if (File.Exists(imgPath))
                {
                    // Dùng stream để tránh file lock
                    using (var fs = new FileStream(imgPath, FileMode.Open, FileAccess.Read))
                    {
                        pictureBoxSetting.Image = Image.FromStream(fs);
                        lamTronAnhDaiDien(pictureBoxSetting, 2, Color.White);

                    }
                }
                else
                {
                    pictureBoxSetting.Image = Properties.Resources.macdinh;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải ảnh đại diện: " + ex.Message);
            }
        }
        private void lamTronAnhDaiDien(PictureBox picBox, int borderSize = 2, Color? borderColor = null)
        {
            int diameter = Math.Min(picBox.Width, picBox.Height);
            Bitmap bmp = new Bitmap(diameter, diameter);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);

                // Vẽ hình tròn (không trừ 1 pixel)
                using (GraphicsPath path = new GraphicsPath())
                {
                    path.AddEllipse(0, 0, diameter, diameter);
                    g.SetClip(path);
                    g.DrawImage(picBox.Image, 0, 0, diameter, diameter);
                }

                // Vẽ viền nhẹ
                using (Pen pen = new Pen(borderColor ?? Color.White, borderSize))
                {
                    g.ResetClip();
                    g.DrawEllipse(pen, borderSize / 2f, borderSize / 2f,
                                  diameter - borderSize, diameter - borderSize);
                }
            }

            picBox.Image = bmp;
            picBox.Region = new Region(new Rectangle(0, 0, diameter, diameter));
            picBox.SizeMode = PictureBoxSizeMode.Zoom;
            picBox.BackColor = Color.Transparent;

        }


        private void pictureBoxSetting_Click(object sender, EventArgs e)
        {

        }
    }
}