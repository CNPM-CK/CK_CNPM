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
    public partial class DanhSachKeHoach : Form
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
        private const string PLACEHOLDER_TEXT = "Tìm kiếm...";

        private BindingList<KeHoach> dsKeHoach;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";
        private Form currentOpenForm = null;

        public DanhSachKeHoach()
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

            InitializeContextMenu();
            InitializeButtonIcons();
            InitializeSettingMenu();
            InitializeDataGridView();
            CalculateLayout();
        }

        

        private void InitializeDataGridView()
        {
        }

        private void RefreshDanhSachKeHoach()
        {
        }

        private Rectangle editRect;
        private Rectangle deleteRect;

        private void dgvDSKH_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
           
        }

        private void dgvDSKH_CellClick(object sender, DataGridViewCellEventArgs e)
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

        private void dgvDSKH_Paint(object sender, PaintEventArgs e)
        {
        }


        private void InitializeContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem pdfItem = new ToolStripMenuItem("Xuất PDF");
            pdfItem.Click += (s, ev) => { MessageBox.Show("Xuất PDF..."); };

            ToolStripMenuItem excelItem = new ToolStripMenuItem("Xuất Excel");
            excelItem.Click += (s, ev) => { MessageBox.Show("Xuất Excel..."); };

            menu.Items.Add(pdfItem);
            menu.Items.Add(excelItem);

            //btnXuatfile.Click += (s, ev) =>
            //{
            //    menu.Show(btnXuatfile, new Point(0, btnXuatfile.Height));
            //};
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
                btnDanhsachnv.TextAlign = ContentAlignment.MiddleCenter;
                btnDanhsachnv.Padding = new Padding(0, 0, 5, 0);
            }

            if (btnDanhsachthongso.Image != null)
            {
                btnDanhsachthongso.Image = new Bitmap(btnDanhsachthongso.Image, new Size(30, 30));
                btnDanhsachthongso.ImageAlign = ContentAlignment.MiddleLeft;
                btnDanhsachthongso.TextAlign = ContentAlignment.MiddleCenter;
                btnDanhsachthongso.Padding = new Padding(0, 0, 5, 0);
            }

            if (btnNenmau.Image != null)
            {
                btnNenmau.Image = new Bitmap(btnNenmau.Image, new Size(30, 30));
                btnNenmau.ImageAlign = ContentAlignment.MiddleLeft;
                btnNenmau.TextAlign = ContentAlignment.MiddleCenter;
                btnNenmau.Padding = new Padding(0, 0, 5, 0);
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

        #region Custom Search Box Paint
        private void containersearch_Paint(object sender, PaintEventArgs e)
        {
        }

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
        #endregion

        #region TextBox Events
        private void searchtextbox_Enter(object sender, EventArgs e)
        {
            
        }

        private void searchtextbox_Leave(object sender, EventArgs e)
        {
            
        }

        private void searchtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            
        }

        private void searchtextbox_TextChanged_1(object sender, EventArgs e)
        {
           
        }

        
        #endregion

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

            KeHoachQuanTrac frmThem = new KeHoachQuanTrac();
            currentOpenForm = frmThem;
            CenterFormOnParent(frmThem);
            frmThem.FormClosed += (s, ev) => { currentOpenForm = null; };
            frmThem.ShowDialog(this);

        }

        private void dgvDSKH_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }

        private void dgvDSKH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();

            // Tạo UC mới
            DanhSachThongSo ucThongSo = new DanhSachThongSo();
            ucThongSo.Dock = DockStyle.Fill;

            // Thêm vào panel
            panel5.Controls.Add(ucThongSo);
            ucThongSo.BringToFront();
        }

        private void btnNenmau_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();

            // Tạo UC mới
            DanhSachNenMau ucNenmau = new DanhSachNenMau();
            ucNenmau.Dock = DockStyle.Fill;

            // Thêm vào panel
            panel5.Controls.Add(ucNenmau);
            ucNenmau.BringToFront();
        }

        private void btnDanhsachnv_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DanhSachDotQuanTrac ucDotquantrac = new DanhSachDotQuanTrac();
            ucDotquantrac.Dock = DockStyle.Fill;
            panel5.Controls.Add(ucDotquantrac);
            ucDotquantrac.BringToFront();
        }
    }
}
#endregion