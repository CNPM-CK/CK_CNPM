using BLL;
using BLL.Speech;
using DTO;
using GUI.Common;
using GUI.Helper;
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
    public partial class DanhSachKhachHanguc : UserControl
    {
        private VoiceRecorder _recorder;
        private WhisperService _whisper;
        private string _wavPath;
        private bool _ready = false;
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
        private const string PLACEHOLDER_TEXT = "Tìm kiếm khách hàng...";

        private BindingList<KhachHang> dsKhachhang;
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
        public DanhSachKhachHanguc()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Load dữ liệu khi UserControl được khởi tạo
            this.Load += DanhSachKhachHanguc_Load;
        }
        #endregion

        #region UserControl Load
        private async void DanhSachKhachHanguc_Load(object sender, EventArgs e)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string modelPath = Path.Combine(baseDir, "Model", "ggml-tiny.bin");
            _wavPath = Path.Combine(baseDir, "TempAudio", "search.wav");

            _recorder = new VoiceRecorder(_wavPath);
            var iatService = IATService.TryCreateFromConfiguration();

            _whisper = new WhisperService(modelPath, iatService);

            picturemicro.Enabled = false;
            picturemicro.Text = "Đang tải model...";

            try
            {
                await _whisper.InitAsync();
                _ready = true;
                picturemicro.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Lỗi khởi tạo Whisper:\n\n" + ex.ToString(),
                    "Whisper Init Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
            }
            InitializeButtonIcons();
            InitializeButtonStyles();
            InitializeCustomSearchBox();
            InitializeDataGridView();
            CalculateLayout();
        }

        private void InitializeDataGridView()
        {
            dgvDanhsachnhanvien.AutoGenerateColumns = false;
            dgvDanhsachnhanvien.Columns.Clear();
            dgvDanhsachnhanvien.AllowUserToAddRows = false;
            dgvDanhsachnhanvien.ReadOnly = true;
            dgvDanhsachnhanvien.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhsachnhanvien.MultiSelect = false;
            dgvDanhsachnhanvien.RowTemplate.Height = 50;

            // Font settings
            dgvDanhsachnhanvien.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            dgvDanhsachnhanvien.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvDanhsachnhanvien.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvDanhsachnhanvien.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F);

            // Header styling
            dgvDanhsachnhanvien.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 152, 70);
            dgvDanhsachnhanvien.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDanhsachnhanvien.EnableHeadersVisualStyles = false;

            // Cell styling
            dgvDanhsachnhanvien.DefaultCellStyle.BackColor = Color.White;
            dgvDanhsachnhanvien.DefaultCellStyle.ForeColor = Color.Black;
            dgvDanhsachnhanvien.DefaultCellStyle.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dgvDanhsachnhanvien.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDanhsachnhanvien.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDanhsachnhanvien.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvDanhsachnhanvien.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "maKH", HeaderText = "Mã khách hàng", Name = "maKH" },
                new DataGridViewTextBoxColumn { DataPropertyName = "tenDoanhNghiep", HeaderText = "Tên doanh nghiệp", Name = "tenDoanhNghiep" },
                new DataGridViewTextBoxColumn { DataPropertyName = "kyHieuDN", HeaderText = "Ký hiệu DN", Name = "kyHieuDN" },
                new DataGridViewTextBoxColumn { DataPropertyName = "nguoiDaiDien", HeaderText = "Người đại diện", Name = "nguoiDaiDien" },
                new DataGridViewTextBoxColumn { DataPropertyName = "soDienThoaiKH", HeaderText = "Số điện thoại", Name = "soDienThoaiKH" },
                new DataGridViewTextBoxColumn { DataPropertyName = "maSoThue", HeaderText = "Mã số thuế", Name = "maSoThue" },
                new DataGridViewTextBoxColumn { DataPropertyName = "emailNguoiDaiDien", HeaderText = "Email người đại diện", Name = "emailNguoiDaiDien" },
                new DataGridViewTextBoxColumn { DataPropertyName = "emailDoanhNghiep", HeaderText = "Email doanh nghiệp", Name = "emailDoanhNghiep" },
                new DataGridViewTextBoxColumn { DataPropertyName = "tenTrangThai", HeaderText = "Trạng thái", Name = "tenTrangThai" },
                new DataGridViewTextBoxColumn { DataPropertyName = "diaChi", HeaderText = "Địa chỉ", Name = "diaChi" }
            });
            if (_isPhongKinhDoanh)
            {
                DataGridViewImageColumn thaoTacCol = new DataGridViewImageColumn
                {
                    Name = "ThaoTac",
                    HeaderText = "Thao tác",
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                dgvDanhsachnhanvien.Columns.Add(thaoTacCol);
            }
            // Gán data và event
            dgvDanhsachnhanvien.CellPainting += dgvDanhsachnhanvien_CellPainting;
            dgvDanhsachnhanvien.CellClick += dgvDanhsachnhanvien_CellClick;
            dgvDanhsachnhanvien.Paint += dgvDanhsachnhanvien_Paint;

            taiTrangKhachHang();
        }

        private void lamMoiDanhSachKhachHang()
        {
            //totalRecords = 0;

            //// Reset search box về trạng thái placeholder
            //if (!string.IsNullOrWhiteSpace(searchtextbox.Text) && searchtextbox.Text != PLACEHOLDER_TEXT)
            //{
            //    isPlaceholder = true;
            //    searchtextbox.Text = PLACEHOLDER_TEXT;
            //    searchtextbox.ForeColor = Color.Silver;
            //    lastSearchKeyword = "";
            //}
            //taiTrangKhachHang();
            // Reset search box về trạng thái placeholder
            if (!string.IsNullOrWhiteSpace(searchtextbox.Text) && searchtextbox.Text != PLACEHOLDER_TEXT)
            {
                isPlaceholder = true;
                searchtextbox.Text = PLACEHOLDER_TEXT;
                searchtextbox.ForeColor = Color.Silver;
                lastSearchKeyword = "";
            }

            // ✅ Reset bộ lọc
            huyBoLoc();
        }
        #endregion

        #region DataGridView Events
        private void dgvDanhsachnhanvien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (!_isPhongKinhDoanh) return;
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvDanhsachnhanvien.Columns["ThaoTac"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                int iconWidth = 24, iconHeight = 24, spacing = 10;
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

        private void dgvDanhsachnhanvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isPhongKinhDoanh) return;
            if (e.RowIndex < 0 || e.ColumnIndex != dgvDanhsachnhanvien.Columns["ThaoTac"].Index)
                return;

            var clickPoint = dgvDanhsachnhanvien.PointToClient(Cursor.Position);
            DataGridViewRow row = dgvDanhsachnhanvien.Rows[e.RowIndex];

            if (editRect.Contains(clickPoint))
                HandleEdit(row);
            else if (deleteRect.Contains(clickPoint))
                HandleDelete(row);
        }

        private void HandleEdit(DataGridViewRow row)
        {
            if (currentOpenForm != null && !currentOpenForm.IsDisposed)
            {
                currentOpenForm.BringToFront();
                currentOpenForm.Focus();
                MessageBox.Show("Vui lòng hoàn thành thao tác hiện tại trước khi thực hiện thao tác mới!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            KhachHang kh = new KhachHang
            {
                maKH = row.Cells["maKH"].Value.ToString(),
                soDienThoaiKH = row.Cells["soDienThoaiKH"].Value?.ToString(),
                tenDoanhNghiep = row.Cells["tenDoanhNghiep"].Value?.ToString(),
                nguoiDaiDien = row.Cells["nguoiDaiDien"].Value?.ToString(),
                diaChi = row.Cells["diaChi"].Value?.ToString(),
                kyHieuDN = row.Cells["kyHieuDN"].Value?.ToString(),
                maSoThue = row.Cells["maSoThue"].Value?.ToString(),
                emailDoanhNghiep = row.Cells["emailDoanhNghiep"].Value?.ToString(),
                emailNguoiDaiDien = row.Cells["emailNguoiDaiDien"].Value?.ToString(),
                trangThai = row.Cells["tenTrangThai"].Value?.ToString() == "Đang hợp tác" ? 1 :
            row.Cells["tenTrangThai"].Value?.ToString() == "Ngừng hợp tác" ? 2 : 1
            };

            ThemKhachHang frmSua = new ThemKhachHang();
            frmSua.isEditMode = true;
            frmSua.KhachHangHienTai = kh;

            currentOpenForm = frmSua;
            CenterFormOnParent(frmSua);

            frmSua.FormClosed += (s, ev) =>
            {
                currentOpenForm = null;
            };

            frmSua.SuccesfullyUpdated += (s, ev) => lamMoiDanhSachKhachHang();
            if (frmSua.ShowDialog() == DialogResult.OK)
            {
                lamMoiDanhSachKhachHang();
            }
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

            string maKH = row.Cells["maKH"].Value.ToString();
            string tenDN = row.Cells["tenDoanhNghiep"].Value?.ToString();

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa khách hàng '{tenDN}' (Mã: {maKH}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    KhachHangBLL khBLL = new KhachHangBLL();
                    khBLL.xoaKhachHang(maKH);

                    MessageBox.Show("Đã xóa khách hàng thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    lamMoiDanhSachKhachHang();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xóa khách hàng: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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

        private void dgvDanhsachnhanvien_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Resources.greenlogo == null) return;

            int dgvWidth = dgvDanhsachnhanvien.Width;
            int dgvHeight = dgvDanhsachnhanvien.Height;
            Image watermark = Properties.Resources.greenlogo;

            int x = (dgvWidth - watermark.Width) / 2;
            int y = (dgvHeight - watermark.Height) / 2;

            ColorMatrix matrix = new ColorMatrix();
            matrix.Matrix33 = 0.08f;
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            e.Graphics.DrawImage(watermark,
                new Rectangle(x, y, watermark.Width, watermark.Height),
                0, 0, watermark.Width, watermark.Height,
                GraphicsUnit.Pixel,
                attributes);
        }

        private void dgvDanhsachnhanvien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
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
            
        }

        private void InitializeButtonStyles()
        {
            btnThemuser.Visible = _isPhongKinhDoanh;
            btnThemuser.Size = new Size(66, 40);

            BoGocButton(btnThemuser, 20);
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
            if (btnThemuser == null || containersearch == null) return;

            int formWidth = this.Width;

            Form parentForm = this.FindForm();
            bool isMaximized = parentForm != null && parentForm.WindowState == FormWindowState.Maximized;

            // ✅ Kích thước button dựa vào trạng thái maximize
            int btnWidth = isMaximized ? 80 : 66;
            int btnHeight = isMaximized ? 50 : 40;
            int btnRadius = isMaximized ? 25 : 20;
            int topOffset = 10;

            // ✅ Resize và bo góc button
            btnThemuser.Size = new Size(btnWidth, btnHeight);
            BoGocButton(btnThemuser, btnRadius);
            btnThemuser.Top = topOffset;

            // ✅ Đặt button ở góc phải với margin
            Control btnParent = btnThemuser.Parent; // Thường là panel6
            if (btnParent != null && btnParent != this)
            {
                int parentWidth = btnParent.Width;
                btnThemuser.Left = parentWidth - btnWidth - MARGIN;
            }
            else
            {
                btnThemuser.Left = formWidth - btnWidth - MARGIN;
            }

            // ✅ Tính toán vị trí cho search box
            int leftBoundary = pictureFilter != null ? pictureFilter.Right + SPACING : MARGIN;
            int rightBoundary = btnThemuser.Left - SPACING;

            // ✅ Nếu có micro icon, trừ thêm khoảng cách
            if (picturemicro != null)
            {
                rightBoundary -= (picturemicro.Width + SPACING);
            }

            int availableWidth = rightBoundary - leftBoundary;

            // ✅ Tính width cho search box
            int searchWidth = Math.Max(MIN_SEARCH_WIDTH, Math.Min(availableWidth, MAX_SEARCH_WIDTH));
            if (searchWidth < MIN_SEARCH_WIDTH)
            {
                searchWidth = Math.Max(150, availableWidth);
            }

            // ✅ Đặt vị trí pictureFilter
            if (pictureFilter != null)
            {
                pictureFilter.Left = MARGIN;
            }

            // ✅ Đặt vị trí và kích thước search box
            containersearch.Left = leftBoundary;
            containersearch.Width = searchWidth;
            containersearch.Height = SEARCH_HEIGHT;

            // ✅ Resize textbox bên trong
            searchtextbox.Width = searchWidth - (borderSize * 2 + 10);
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);

            // ✅ Đặt micro icon sau search box
            if (picturemicro != null)
            {
                picturemicro.Left = containersearch.Right + SPACING;
            }

            // ✅ Padding button dựa vào trạng thái maximize
            if (isMaximized)
            {
                btnThemuser.Padding = new Padding(10, 5, 10, 5);
            }
            else
            {
                btnThemuser.Padding = new Padding(5, 3, 5, 3);
            }

            // ✅ Refresh vẽ lại search box
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
                dgvDanhsachnhanvien.DataSource = dsKhachhang;
                lastSearchKeyword = "";
            }
        }

        private void searchtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (dgvDanhsachnhanvien.Rows.Count > 0)
                {
                    dgvDanhsachnhanvien.ClearSelection();
                    dgvDanhsachnhanvien.Rows[0].Selected = true;
                    dgvDanhsachnhanvien.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                searchtextbox.Clear();
                dgvDanhsachnhanvien.DataSource = dsKhachhang;
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


        //private void PerformSearch()
        //{
        //    string keyword = searchtextbox.Text?.Trim().ToLower() ?? "";

        //    if (string.IsNullOrEmpty(keyword))
        //    {
        //        // Reset về phân trang
        //        currentPage = 1;
        //        totalRecords = 0;
        //        taiTrangKhachHang();
        //        return;
        //    }

        //    // ✅ TÌM KIẾM TRÊN TOÀN BỘ DATABASE
        //    try
        //    {
        //        KhachHangBLL bll = new KhachHangBLL();
        //        var allData = bll.layDanhSachKH(); // ✅ Cần thêm method này vào BLL

        //        var filtered = allData
        //            .Where(kh =>
        //                (kh.tenDoanhNghiep ?? "").ToLower().Contains(keyword) ||
        //                (kh.nguoiDaiDien ?? "").ToLower().Contains(keyword) ||
        //                (kh.kyHieuDN ?? "").ToLower().Contains(keyword) ||
        //                (kh.soDienThoaiKH ?? "").Contains(keyword) ||
        //                (kh.diaChi ?? "").ToLower().Contains(keyword) ||
        //                kh.maKH.ToString().ToLower().Contains(keyword)
        //            )
        //            .ToList();

        //        dgvDanhsachnhanvien.DataSource = new BindingList<KhachHang>(filtered);

        //        // Hiển thị số kết quả
        //        soTrang.Text = $"Tìm thấy {filtered.Count} kết quả";

        //        // Disable nút phân trang khi search
        //        btnTruoc.Enabled = false;
        //        btnSau.Enabled = false;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi",
        //            MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}


        private void PerformSearch()
        {
            string keyword = searchtextbox.Text?.Trim().ToLower() ?? "";

            if (string.IsNullOrEmpty(keyword))
            {
                // ✅ Nếu xóa keyword
                if (isFiltering)
                {
                    // Đang lọc → áp dụng lại bộ lọc
                    apDungBoLoc(filterTrangThai);
                }
                else
                {
                    // Không lọc → load lại phân trang
                    currentPage = 1;
                    totalRecords = 0;
                    taiTrangKhachHang();
                }
                return;
            }

            try
            {
                KhachHangBLL bll = new KhachHangBLL();

                // ✅ Lấy dữ liệu nguồn để tìm
                List<KhachHang> searchSource;

                if (isFiltering)
                {
                    // Đang lọc → tìm trong kết quả đã lọc
                    searchSource = dsKhachhang.ToList();
                }
                else
                {
                    // Không lọc → tìm trong toàn bộ database
                    searchSource = bll.layDanhSachKH();
                }

                var filtered = searchSource
                    .Where(kh =>
                        (kh.tenDoanhNghiep ?? "").ToLower().Contains(keyword) ||
                        (kh.nguoiDaiDien ?? "").ToLower().Contains(keyword) ||
                        (kh.kyHieuDN ?? "").ToLower().Contains(keyword) ||
                        (kh.soDienThoaiKH ?? "").Contains(keyword) ||
                        (kh.diaChi ?? "").ToLower().Contains(keyword) ||
                        kh.maKH.ToString().ToLower().Contains(keyword)
                    )
                    .ToList();

                dgvDanhsachnhanvien.DataSource = null;
                dsKhachhang = new BindingList<KhachHang>(filtered);
                dgvDanhsachnhanvien.DataSource = dsKhachhang;

                soTrang.Text = $"Tìm thấy {filtered.Count} kết quả";
                btnTruoc.Enabled = false;
                btnSau.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            ThemKhachHang frmThem = new ThemKhachHang();
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

        private void dgvDanhsachnhanvien_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

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


            var data = bll.layDanhSachKH_PhanTrang(currentPage, pageSize);
            dsKhachhang = new BindingList<KhachHang>(data);

            dgvDanhsachnhanvien.DataSource = dsKhachhang;
            //dgvDanhsachnhanvien.DataSource = data;

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

        private async void BtnMic_Click(object sender, EventArgs e)
        {
            if (!_ready) return;

            if (!_recorder.IsRecording)
            {
                try
                {
                    _recorder.Start();
                    picturemicro.Image = Properties.Resources.microphone_hoatdong;
                    searchtextbox.ForeColor = Color.Silver;
                    searchtextbox.Text = "Đang nghe...";
                    isPlaceholder = true;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể ghi âm: " + ex.Message);
                }
            }

            else
            {
                picturemicro.Enabled = false;
                picturemicro.Image = Properties.Resources.microphone;

                _recorder.Stop();
                await Task.Delay(300);

                try
                {
                    string text;
                    try
                    {
                        text = await _whisper.TranscribeIFlytekAsync(_wavPath);

                    }
                    catch
                    {
                        text = await _whisper.TranscribeAsync(_wavPath);
                    }

                    if (string.IsNullOrWhiteSpace(text))
                    {
                        MessageBox.Show("Không nghe được nội dung");
                        return;
                    }

                    // Đảm bảo bỏ trạng thái placeholder
                    isPlaceholder = false;
                    searchtextbox.ForeColor = Color.FromArgb(64, 64, 64);

                    searchtextbox.Text = text.Trim();
                    searchtextbox.SelectionStart = searchtextbox.Text.Length;

                    // Cho tự động lọc luôn nếu muốn
                    PerformSearch();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi nhận dạng giọng nói: " + ex.Message);
                }
                finally
                {
                    picturemicro.Enabled = true;
                }

            }

        }


        private string filterTrangThai = null;
        private bool isFiltering = false;


        private void pictureFilter_Click(object sender, EventArgs e)
        {
            LocTrangThaiKhachHang filter = new LocTrangThaiKhachHang();
            if (filter.ShowDialog() == DialogResult.OK)
            {
                apDungBoLoc(filter.SelectedTrangThai);
            }
        }
        private void apDungBoLoc(string trangThai)
        {
            //// Bắt đầu từ danh sách đầy đủ
            //var result = dsKhachhang.AsEnumerable();

            //// 3) Lọc trạng thái
            //if (!string.IsNullOrEmpty(trangThai))
            //    result = result.Where(nv => nv.trangThai.ToString() == trangThai);

            //// Kết quả cuối
            //dgvDanhsachnhanvien.DataSource = result.ToList();
            try
            {
                // Lưu điều kiện lọc
                filterTrangThai = trangThai;
                isFiltering = !string.IsNullOrEmpty(trangThai);

                if (!isFiltering)
                {
                    // Không có bộ lọc → load lại phân trang
                    currentPage = 1;
                    totalRecords = 0;
                    taiTrangKhachHang();
                    return;
                }

                // ✅ Lấy TOÀN BỘ dữ liệu từ BLL
                KhachHangBLL bll = new KhachHangBLL();
                var allData = bll.layDanhSachKH();

                // ✅ Lọc theo trạng thái
                var filtered = allData.Where(kh => kh.trangThai.ToString() == trangThai).ToList();

                // ✅ Gán vào DataGridView
                dgvDanhsachnhanvien.DataSource = null;
                dsKhachhang = new BindingList<KhachHang>(filtered);
                dgvDanhsachnhanvien.DataSource = dsKhachhang;

                // ✅ Vô hiệu hóa phân trang khi lọc
                btnTruoc.Enabled = false;
                btnSau.Enabled = false;
                //soTrang.Text = $"Tìm thấy {filtered.Count} kết quả";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi áp dụng bộ lọc: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void huyBoLoc()
        {
            filterTrangThai = null;
            isFiltering = false;
            currentPage = 1;
            totalRecords = 0;
            taiTrangKhachHang();
        }
    }
}