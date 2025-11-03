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
    public partial class DanhSachNhanVien : Form
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



        public DanhSachNhanVien()
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
        }

       

        //private void taiDanhSachNhanVien()
        //{
        //    NhanVienBLL nvBLL = new NhanVienBLL();
        //    dsNhanVien.Clear();
        //    foreach (var nv in nvBLL.layDanhSachNhanVien())
        //        dsNhanVien.Add(nv);
        //}

        private Rectangle editRect;
        private Rectangle deleteRect;

        private void dgvDanhsachnhanvien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
          
        }

        private void dgvDanhsachnhanvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
        }

        //private void HandleEdit(DataGridViewRow row)
        //{
        //    if (currentOpenForm != null && !currentOpenForm.IsDisposed)
        //    {
        //        currentOpenForm.BringToFront();
        //        currentOpenForm.Focus();
        //        MessageBox.Show("Vui lòng hoàn thành thao tác hiện tại trước khi thực hiện thao tác mới!",
        //            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        //        return;
        //    }

        //    NhanVien nv = new NhanVien
        //    {
        //        maNV = row.Cells["maNV"].Value.ToString(),
        //        maPhong = row.Cells["maPhong"].Value?.ToString(),
        //        hoTen = row.Cells["hoTen"].Value?.ToString(),
        //        ngaySinh = row.Cells["ngaySinh"].Value != null ? Convert.ToDateTime(row.Cells["ngaySinh"].Value) : DateTime.MinValue,
        //        gioiTinh = row.Cells["gioiTinh"].Value?.ToString(),
        //        diaChi = row.Cells["diaChi"].Value?.ToString(),
        //        soDienThoai = row.Cells["soDienThoai"].Value?.ToString(),
        //        email = row.Cells["email"].Value?.ToString()
        //    };

        //    SuaNhanVien frmSua = new SuaNhanVien(nv);
        //    currentOpenForm = frmSua;
        //    CenterFormOnParent(frmSua);

        //    frmSua.FormClosed += (s, ev) =>
        //    {
        //        currentOpenForm = null; // Xóa reference khi form đóng
        //    };

        //    frmSua.SuccesfullyUpdated += (s, ev) => RefreshDanhSachNhanVien();
        //    frmSua.Show(this);
        //}

        //private void HandleDelete(DataGridViewRow row)
        //{
        //    string maNV = row.Cells["maNV"].Value.ToString();
        //    string hoTen = row.Cells["hoTen"].Value?.ToString();

        //    DialogResult result = MessageBox.Show(
        //        $"Bạn có chắc chắn muốn xóa nhân viên '{hoTen}' (Mã: {maNV}) không?",
        //        "Xác nhận xóa",
        //        MessageBoxButtons.YesNo,
        //        MessageBoxIcon.Question
        //    );

        //    if (result == DialogResult.Yes)
        //    {
        //        try
        //        {
        //            NhanVienBLL nvBLL = new NhanVienBLL();
        //            nvBLL.xoaNhanVien(maNV);

        //            MessageBox.Show("Đã xóa nhân viên thành công!", "Thông báo",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            RefreshDanhSachNhanVien();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show("Có lỗi xảy ra khi xóa nhân viên: " + ex.Message,
        //                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}

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
                MessageBox.Show("Mở trang cài đặt cá nhân...", "Thông báo");
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
                btnDanhsachnv.Image = new Bitmap(btnDanhsachnv.Image, new Size(30, 30));
                btnDanhsachnv.ImageAlign = ContentAlignment.MiddleLeft;
                btnDanhsachnv.TextAlign = ContentAlignment.MiddleRight;
                btnDanhsachnv.Padding = new Padding(0, 0, 2, 0);
            }
            if (btnHopdong.Image != null)
            {
                btnHopdong.Image = new Bitmap(btnHopdong.Image, new Size(30, 30));
                btnHopdong.ImageAlign = ContentAlignment.MiddleLeft;
                btnHopdong.TextAlign = ContentAlignment.MiddleRight;
                btnHopdong.Padding = new Padding(0, 0, 2, 0);
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
        private void button1_Click(object sender, EventArgs e){ }
        #endregion

        private void btnDanhsachnv_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DSNV_Uc ucDSNV = new DSNV_Uc();
            ucDSNV.Dock = DockStyle.Fill;
            panel5.Controls.Add(ucDSNV);
            ucDSNV.BringToFront();
        }

       
    }
}