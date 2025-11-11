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
    public partial class DanhSachHopDonguc : UserControl
    {
        private readonly bool _isPhongKinhDoanh = SessionStore.Current.MaPhong == "P001";
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
        private const string PLACEHOLDER_TEXT = "Tìm kiếm hợp đồng...";

        private BindingList<HopDongDTO> dsHopDong;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";
        private Form currentOpenForm = null;

        private Rectangle editRect;
        private Rectangle deleteRect;

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

        #region Constructor
        public DanhSachHopDonguc()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Load dữ liệu khi UserControl được khởi tạo
            this.Load += DanhSachKhachHanguc_Load;
        }
        #endregion

        #region UserControl Load
        private void DanhSachKhachHanguc_Load(object sender, EventArgs e)
        {
            InitializeButtonIcons();
            InitializeButtonStyles();
            InitializeCustomSearchBox();
            InitializeDataGridView();
            CalculateLayout();
        }

        private void InitializeDataGridView()
        {
            dgvdanhsachHopDong.AutoGenerateColumns = false;
            dgvdanhsachHopDong.Columns.Clear();
            dgvdanhsachHopDong.AllowUserToAddRows = false;
            dgvdanhsachHopDong.ReadOnly = true;
            dgvdanhsachHopDong.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvdanhsachHopDong.MultiSelect = false;
            dgvdanhsachHopDong.RowTemplate.Height = 50;

            // Font settings
            dgvdanhsachHopDong.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            dgvdanhsachHopDong.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvdanhsachHopDong.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvdanhsachHopDong.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F);

            // Header styling
            dgvdanhsachHopDong.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 152, 70);
            dgvdanhsachHopDong.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvdanhsachHopDong.EnableHeadersVisualStyles = false;

            // Cell styling
            dgvdanhsachHopDong.DefaultCellStyle.BackColor = Color.White;
            dgvdanhsachHopDong.DefaultCellStyle.ForeColor = Color.Black;
            dgvdanhsachHopDong.DefaultCellStyle.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dgvdanhsachHopDong.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvdanhsachHopDong.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdanhsachHopDong.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvdanhsachHopDong.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "maHD", HeaderText = "Mã hợp đồng", Name = "maHD" },
                new DataGridViewTextBoxColumn { DataPropertyName = "maKH", HeaderText = "Mã khách hàng", Name = "maKH" },
                new DataGridViewTextBoxColumn { DataPropertyName = "ngayKy", HeaderText = "Ngày ký", Name = "ngayKy" },
                new DataGridViewTextBoxColumn { DataPropertyName = "ngayKetThucHD", HeaderText = "Ngày kết thúc hợp đồng", Name = "ngayKetThucHD" },
                new DataGridViewTextBoxColumn { DataPropertyName = "trangThai", HeaderText = "Trạng thái", Name = "trangThai" },
                new DataGridViewTextBoxColumn { DataPropertyName = "tanSuatQuanTrac", HeaderText = "Tần suất quan trắc", Name = "tanSuatQuanTrac" },
                new DataGridViewTextBoxColumn { DataPropertyName = "soHD", HeaderText = "Số hợp đồng", Name = "soHD" }
            });
            if (_isPhongKinhDoanh)
            {
                DataGridViewImageColumn thaoTacCol = new DataGridViewImageColumn
                {
                    Name = "ThaoTac",
                    HeaderText = "Thao tác",
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                dgvdanhsachHopDong.Columns.Add(thaoTacCol);
            }
            dgvdanhsachHopDong.CellFormatting += dgvdanhsachHopDong_CellFormatting;
            dgvdanhsachHopDong.CellPainting += dgvdanhsachHopDong_CellPainting;
            dgvdanhsachHopDong.CellClick += dgvdanhsachHopDong_CellClick;
            dgvdanhsachHopDong.Paint += dgvdanhsachHopDong_Paint;

            dgvdanhsachHopDong.DataSource = dsHopDong;
            dgvdanhsachHopDong.ReadOnly = true;
            taiTrangKhachHang();
        }

        private void lamMoiDanhSachKhachHang()
        {
            totalRecords = 0;
            taiTrangKhachHang();
        }
        #endregion

        #region DataGridView Events
        private void dgvdanhsachHopDong_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (!_isPhongKinhDoanh) return;
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvdanhsachHopDong.Columns["ThaoTac"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                int iconWidth = 24;
                int iconHeight = 24;
                int spacing = 10;
                int totalWidth = (iconWidth * 2) + spacing;

                int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;
                int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

                editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
                e.Graphics.DrawImage(Properties.Resources.edit, editRect);

                deleteRect = new Rectangle(startX + iconWidth + spacing, startY, iconWidth, iconHeight);
                e.Graphics.DrawImage(Properties.Resources.trash_can, deleteRect);

                e.Handled = true;
            }
        }

        private void dgvdanhsachHopDong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isPhongKinhDoanh) return;
            if (e.RowIndex < 0 || e.ColumnIndex != dgvdanhsachHopDong.Columns["ThaoTac"].Index)
                return;

            var clickPoint = dgvdanhsachHopDong.PointToClient(Cursor.Position);
            DataGridViewRow row = dgvdanhsachHopDong.Rows[e.RowIndex];

            if (row.Cells["maHD"].Value == null) return;

            if (editRect.Contains(clickPoint))
            {
                HandleEdit(row);
            }
            else if (deleteRect.Contains(clickPoint))
            {
                HandleDelete(row);
            }
        }

        private void HandleEdit(DataGridViewRow row)
        {
            HopDongDTO hd = new HopDongDTO
            {
                maHD = row.Cells["maHD"].Value.ToString(),
                maKH = row.Cells["maKH"].Value?.ToString(),
                ngayKy = row.Cells["ngayKy"].Value != null ? Convert.ToDateTime(row.Cells["ngayKy"].Value) : DateTime.MinValue,
                ngayKetThucHD = row.Cells["ngayKetThucHD"].Value != null ? Convert.ToDateTime(row.Cells["ngayKetThucHD"].Value) : DateTime.MinValue,
                trangThai = row.Cells["trangThai"].Value?.ToString(),
                tanSuatQuanTrac = row.Cells["tanSuatQuanTrac"].Value?.ToString(),
                soHD = row.Cells["soHD"].Value?.ToString(),
            };

            SuaHopDongForm frmSua = new SuaHopDongForm(hd);
            CenterFormOnParent(frmSua);
            frmSua.SuccesfullyUpdated += (s, ev) => lamMoiDanhSachKhachHang();
            frmSua.Show(this);
        }

        private void HandleDelete(DataGridViewRow row)
        {
            if (currentOpenForm != null && !currentOpenForm.IsDisposed)
            {
                currentOpenForm.BringToFront();
                currentOpenForm.Focus();
                MessageBox.Show("Vui lòng hoàn thành thao tác hiện tại trước khi thực hiện thao tác mới!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string maHD = row.Cells["maHD"].Value.ToString();

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa hợp đồng (Mã: {maHD}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                //try
                //{
                //    KhachHangBLL khBLL = new KhachHangBLL();
                //    khBLL.xoaKhachHang(maKH);

                //    MessageBox.Show("Đã xóa khách hàng thành công!", "Thông báo",
                //        MessageBoxButtons.OK, MessageBoxIcon.Information);

                //    lamMoiDanhSachKhachHang();
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show("Có lỗi xảy ra khi xóa khách hàng: " + ex.Message,
                //        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //}
            }
        }

        private void CenterFormOnParent(Form childForm)
        {
            // Lấy parent form của UserControl
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                childForm.StartPosition = FormStartPosition.Manual;
                childForm.Location = new Point(
                    parentForm.Location.X + (parentForm.Width - childForm.Width) / 2,
                    parentForm.Location.Y + (parentForm.Height - childForm.Height) / 2
                );
            }
            else
            {
                childForm.StartPosition = FormStartPosition.CenterScreen;
            }
        }

        private void dgvdanhsachHopDong_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Resources.greenlogo == null) return;

            int dgvWidth = dgvdanhsachHopDong.Width;
            int dgvHeight = dgvdanhsachHopDong.Height;
            Image watermark = Properties.Resources.greenlogo;

            int x = (dgvWidth - watermark.Width) / 2;
            int y = (dgvHeight - watermark.Height) / 2;

            ColorMatrix matrix = new ColorMatrix();
            matrix.Matrix33 = 0.3f;
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            e.Graphics.DrawImage(watermark,
                new Rectangle(x, y, watermark.Width, watermark.Height),
                0, 0, watermark.Width, watermark.Height,
                GraphicsUnit.Pixel,
                attributes);
        }

        private void dgvdanhsachHopDong_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
        }
        #endregion

        #region Search Box Initialization
        private void InitializeCustomSearchBox()
        {
            containersearch.BackColor = Color.Transparent;
            containersearch.Size = new Size(400, SEARCH_HEIGHT);
            containersearch.BringToFront();

            searchtextbox.BorderStyle = BorderStyle.None;
            searchtextbox.BackColor = Color.White;
            searchtextbox.Font = new Font("Segoe UI", 10F);
            searchtextbox.ForeColor = Color.Silver;
            searchtextbox.Text = PLACEHOLDER_TEXT;
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);
            searchtextbox.Size = new Size(containersearch.Width - (borderSize * 2 + 10), 28);
            searchtextbox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            containersearch.Controls.Add(searchtextbox);

            searchtextbox.Enter += searchtextbox_Enter;
            searchtextbox.Leave += searchtextbox_Leave;
            searchtextbox.TextChanged += searchtextbox_TextChanged_1;
            searchtextbox.KeyDown += searchtextbox_KeyDown;
            containersearch.Paint += containersearch_Paint;
        }
        #endregion

        #region Button Initialization
        private void InitializeButtonIcons()
        {

            if (btnThemuser.Image != null)
            {
                btnThemuser.Image = new Bitmap(btnThemuser.Image, new Size(24, 24));
            }

            if (btnXuatfile.Image != null)
            {
                btnXuatfile.Image = new Bitmap(btnXuatfile.Image, new Size(24, 24));
            }
        }

        private void InitializeButtonStyles()
        {
            btnThemuser.Visible = _isPhongKinhDoanh;
            btnXuatfile.Visible = _isPhongKinhDoanh;
            btnThemuser.Size = new Size(66, 40);
            btnXuatfile.Size = new Size(66, 40);

            BoGocButton(btnThemuser, 20);
            BoGocButton(btnXuatfile, 20);
            BoGocButton(btnTruoc, 20);
            BoGocButton(btnSau, 20);
            btnThemuser.Click += btnThemuser_Click;
        }
        #endregion

        #region Layout & Resize
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.Width < 100) return;
            CalculateLayout();
        }

        private void CalculateLayout()
        {
            int formWidth = this.Width;

            // Kiểm tra xem parent form có đang maximized không
            Form parentForm = this.FindForm();
            bool isMaximized = parentForm != null && parentForm.WindowState == FormWindowState.Maximized;

            int btnWidth = isMaximized ? 80 : 66;
            int btnHeight = isMaximized ? 50 : 40;
            int btnRadius = isMaximized ? 25 : 20;

            btnXuatfile.Size = new Size(btnWidth, btnHeight);
            btnThemuser.Size = new Size(btnWidth, btnHeight);
            BoGocButton(btnXuatfile, btnRadius);
            BoGocButton(btnThemuser, btnRadius);

            Control btnParent = btnXuatfile.Parent; // panel6

            if (btnParent != null && btnParent != this)
            {
                int parentWidth = btnParent.Width;
                btnXuatfile.Left = parentWidth - btnWidth - MARGIN;
                btnThemuser.Left = btnXuatfile.Left - btnWidth - SPACING;
            }
            else
            {
                btnXuatfile.Left = formWidth - btnWidth - MARGIN;
                btnThemuser.Left = btnXuatfile.Left - btnWidth - SPACING;
            }

            // Đặt vị trí dọc của button trong panel6
            int topPosition = 10;
            btnXuatfile.Top = topPosition;
            btnThemuser.Top = topPosition;

            if (panel7 != null && panel6 != null)
            {
                panel7.Top = panel6.Bottom + 15;
            }

            pictureFilter.Left = MARGIN;

            int leftBoundary = pictureFilter.Right + SPACING;
            int rightBoundary = btnThemuser.Left - SPACING - picturemicro.Width - SPACING;
            int availableWidth = rightBoundary - leftBoundary;

            int searchWidth = Math.Max(MIN_SEARCH_WIDTH, Math.Min(availableWidth, MAX_SEARCH_WIDTH));
            if (searchWidth < MIN_SEARCH_WIDTH)
            {
                searchWidth = Math.Max(150, availableWidth);
            }

            containersearch.Left = leftBoundary;
            containersearch.Width = searchWidth;
            containersearch.Height = SEARCH_HEIGHT;

            searchtextbox.Width = searchWidth - (borderSize * 2 + 10);
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);

            picturemicro.Left = containersearch.Right + SPACING;

            if (isMaximized)
            {
                btnThemuser.Padding = new Padding(10, 5, 10, 5);
                btnXuatfile.Padding = new Padding(10, 5, 10, 5);
            }
            else
            {
                btnThemuser.Padding = new Padding(5, 3, 5, 3);
                btnXuatfile.Padding = new Padding(5, 3, 5, 3);
            }

            containersearch.Invalidate();
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
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float offset = borderSize / 2f;
            RectangleF rect = new RectangleF(
                offset,
                offset,
                containersearch.ClientSize.Width - borderSize,
                containersearch.ClientSize.Height - borderSize
            );

            using (GraphicsPath path = CreateRoundedRectPath(rect, borderRadius))
            {
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillPath(brush, path);
                }

                using (Pen pen = new Pen(borderColor, borderSize))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
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

        #region TextBox Events - Search Functionality
        private void searchtextbox_Enter(object sender, EventArgs e)
        {
            if (isPlaceholder)
            {
                isPlaceholder = false;
                searchtextbox.Text = "";
                searchtextbox.ForeColor = Color.FromArgb(64, 64, 64);
            }
        }

        private void searchtextbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchtextbox.Text))
            {
                isPlaceholder = true;
                searchtextbox.Text = PLACEHOLDER_TEXT;
                searchtextbox.ForeColor = Color.Silver;
                dgvdanhsachHopDong.DataSource = dsHopDong;
                lastSearchKeyword = "";
            }
        }

        private void searchtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (dgvdanhsachHopDong.Rows.Count > 0)
                {
                    dgvdanhsachHopDong.ClearSelection();
                    dgvdanhsachHopDong.Rows[0].Selected = true;
                    dgvdanhsachHopDong.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                searchtextbox.Clear();
                dgvdanhsachHopDong.DataSource = dsHopDong;
                lastSearchKeyword = "";
            }
        }

        private void searchtextbox_TextChanged_1(object sender, EventArgs e)
        {
            if (isPlaceholder)
                return;

            string currentKeyword = searchtextbox.Text.Trim().ToLower();
            if (currentKeyword == lastSearchKeyword)
                return;

            lastSearchKeyword = currentKeyword;
            PerformSearch();
        }

        private void PerformSearch()
        {
            //string keyword = searchtextbox.Text?.Trim().ToLower() ?? "";
            //if (string.IsNullOrEmpty(keyword))
            //{
            //    dgvdanhsachHopDong.DataSource = dsHopDong;
            //    return;
            //}

            //var filtered = dsHopDong
            //    .Where(kh =>
            //        (kh.tenDoanhNghiep ?? "").ToLower().Contains(keyword) ||
            //        (kh.nguoiDaiDien ?? "").ToLower().Contains(keyword) ||
            //        (kh.kyHieuDN ?? "").ToLower().Contains(keyword) ||
            //        (kh.soDienThoaiKH ?? "").Contains(keyword) ||
            //        (kh.diaChi ?? "").ToLower().Contains(keyword) ||
            //        kh.maKH.ToString().ToLower().Contains(keyword)
            //    )
            //    .ToList();

            //dgvdanhsachHopDong.DataSource = new BindingList<HopDongDTO>(filtered);
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

            ThemHopDongForm frmThem = new ThemHopDongForm();
            currentOpenForm = frmThem;

            CenterFormOnParent(frmThem);

            frmThem.FormClosed += (s, ev) =>
            {
                currentOpenForm = null;
            };

            frmThem.SuccesfullyUpdated += (s, ev) => lamMoiDanhSachKhachHang();
            frmThem.Show();
        }
        #endregion

        private void panel6_Paint(object sender, PaintEventArgs e) { }

        private void dgvdanhsachHopDong_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        int currentPage = 1;
        int pageSize = 15;
        int totalRecords = 0;
        int totalPages = 0;
        private void taiTrangKhachHang()
        {
            var bll = new KhachHangBLL();

            // 🔹 Tính tổng số trang (chỉ cần 1 lần khi load form)
            if (totalRecords == 0)
            {
                totalRecords = bll.demTongSoKhachHang();
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            }

            HopDongBLL hdBLL = new HopDongBLL();
            var data = hdBLL.LayDanhSachHD();
            
            dsHopDong = new BindingList<HopDongDTO>(data);

            dgvdanhsachHopDong.DataSource = dsHopDong;
            //dgvdanhsachHopDong.DataSource = data;

            // 🔹 Cập nhật label trang
            soTrang.Text = $"Trang {currentPage}/{totalPages}";

            // 🔹 Disable nút nếu đang ở biên
            btnTruoc.Enabled = currentPage > 1;
            btnSau.Enabled = currentPage < totalPages;
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                taiTrangKhachHang();
            }
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            currentPage++;
            taiTrangKhachHang();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void picturemicro_Click(object sender, EventArgs e)
        {

        }
    }
}