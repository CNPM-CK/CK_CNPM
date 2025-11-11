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
    public partial class DanhSachNhapLieu : Form
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

        //private BindingList<KeHoachDTO> dsKeHoach;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";
        private Form currentOpenForm = null;


        public DanhSachNhapLieu()
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
            // TODO: Thay bằng KeHoachBLL khi có
            // KeHoachBLL khBLL = new KeHoachBLL();
            // dsKeHoach = new BindingList<KeHoach>(khBLL.LayDanhSachKeHoach());

            // Tạm thời dùng dữ liệu mẫu để test

            InitializeContextMenu();
            InitializeButtonIcons();
            //InitializeButtonStyles();
            //InitializeCustomSearchBox();
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
            //if (Properties.Resources.greenlogo == null) return;

            //int dgvWidth = dgvDSKH.Width;
            //int dgvHeight = dgvDSKH.Height;
            //Image watermark = Properties.Resources.greenlogo;

            //int x = (dgvWidth - watermark.Width) / 2;
            //int y = (dgvHeight - watermark.Height) / 2;

            //ColorMatrix matrix = new ColorMatrix();
            //matrix.Matrix33 = 0.3f;
            //ImageAttributes attributes = new ImageAttributes();
            //attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            //e.Graphics.DrawImage(watermark,
            //    new Rectangle(x, y, watermark.Width, watermark.Height),
            //    0, 0, watermark.Width, watermark.Height,
            //    GraphicsUnit.Pixel,
            //    attributes);
        }

        private void InitializeCustomSearchBox()
        {
            //containersearch.BackColor = Color.Transparent;
            //containersearch.Size = new Size(400, SEARCH_HEIGHT);
            //containersearch.BringToFront();

            //searchtextbox.BorderStyle = BorderStyle.None;
            //searchtextbox.BackColor = Color.White;
            //searchtextbox.Font = new Font("Segoe UI", 10F);
            //searchtextbox.ForeColor = Color.Silver;
            //searchtextbox.Text = PLACEHOLDER_TEXT;
            //searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);
            //searchtextbox.Size = new Size(containersearch.Width - (borderSize * 2 + 10), 28);
            //searchtextbox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            //containersearch.Controls.Add(searchtextbox);

            //// Đăng ký events
            //searchtextbox.Enter += searchtextbox_Enter;
            //searchtextbox.Leave += searchtextbox_Leave;
            //searchtextbox.TextChanged += searchtextbox_TextChanged_1;
            //searchtextbox.KeyDown += searchtextbox_KeyDown;
            //containersearch.Paint += containersearch_Paint;
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

        #region Custom Search Box Paint
        private void containersearch_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            //e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            //float offset = borderSize / 2f;
            //RectangleF rect = new RectangleF(
            //    offset,
            //    offset,
            //    containersearch.ClientSize.Width - borderSize,
            //    containersearch.ClientSize.Height - borderSize
            //);

            //using (GraphicsPath path = CreateRoundedRectPath(rect, borderRadius))
            //{
            //    using (SolidBrush brush = new SolidBrush(Color.White))
            //    {
            //        e.Graphics.FillPath(brush, path);
            //    }

            //    using (Pen pen = new Pen(borderColor, borderSize))
            //    {
            //        e.Graphics.DrawPath(pen, path);
            //    }
            //}
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

        private void button1_Click(object sender, EventArgs e)
        {
            //panel5.Controls.Clear();

            //// Tạo UC mới
            //DanhSachThongSoForm ucThongSo = new DanhSachThongSoForm();
            //ucThongSo.Dock = DockStyle.Fill;

            //// Thêm vào panel
            //panel5.Controls.Add(ucThongSo);
            //ucThongSo.BringToFront();
        }

        private void btnNenmau_Click(object sender, EventArgs e)
        {
            //panel5.Controls.Clear();

            //// Tạo UC mới
            //DanhSachNenMauUC ucNenmau = new DanhSachNenMauUC();
            //ucNenmau.Dock = DockStyle.Fill;

            //// Thêm vào panel
            //panel5.Controls.Add(ucNenmau);
            //ucNenmau.BringToFront();
        }

        private void btnDanhsachnv_Click(object sender, EventArgs e)
        {
            panel5.Controls.Clear();
            DanhSachDotNhapLieuUC ucDotquantrac = new DanhSachDotNhapLieuUC();
            ucDotquantrac.Dock = DockStyle.Fill;
            panel5.Controls.Add(ucDotquantrac);
            ucDotquantrac.BringToFront();
        }
    }
}