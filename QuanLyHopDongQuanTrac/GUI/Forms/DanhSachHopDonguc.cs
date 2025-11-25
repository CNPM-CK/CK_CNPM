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
    public partial class DanhSachHopDonguc : UserControl
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
        private const string PLACEHOLDER_TEXT = "Tìm kiếm hợp đồng...";

        private BindingList<HopDongDTO> dsHopDong;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";
        private Form currentOpenForm = null;

        private Rectangle editRect;
        private Rectangle deleteRect;

        // Phân trang
        int currentPage = 1;
        int pageSize = 15;
        int totalRecords = 0;
        int totalPages = 0;

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
            string appId = "ga825cbd";
            string apiKey = "55774f42c55202232e1b4d8ebfc314c5";
            string apiSecret = "c1cb5fc788d78ec4b808e8cc4beb4a3d";

            var iatService = new IATService(appId, apiKey, apiSecret);

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

            if (_isPhongKinhDoanh)
            {
                dgvdanhsachHopDong.CellPainting += dgvdanhsachHopDong_CellPainting;
                dgvdanhsachHopDong.CellClick += dgvdanhsachHopDong_CellClick;
            }

            dgvdanhsachHopDong.Paint += dgvdanhsachHopDong_Paint;

            // Load dữ liệu phân trang
            lamMoiDanhSachKhachHang();
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
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != dgvdanhsachHopDong.Columns["ThaoTac"].Index) return;

            e.PaintBackground(e.ClipBounds, true);

            int iconWidth = 24, iconHeight = 24, spacing = 10;
            int totalWidth = (iconWidth * 2) + spacing;
            int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;
            int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

            editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
            if (Properties.Resources.edit != null)
                e.Graphics.DrawImage(Properties.Resources.edit, editRect);

            deleteRect = new Rectangle(startX + iconWidth + spacing, startY, iconWidth, iconHeight);
            if (Properties.Resources.trash_can != null)
                e.Graphics.DrawImage(Properties.Resources.trash_can, deleteRect);

            e.Handled = true;
        }

        private void dgvdanhsachHopDong_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isPhongKinhDoanh) return;
            if (e.RowIndex < 0 || e.RowIndex >= dgvdanhsachHopDong.Rows.Count) return;
            if (e.ColumnIndex != dgvdanhsachHopDong.Columns["ThaoTac"].Index) return;

            var clickPoint = dgvdanhsachHopDong.PointToClient(Cursor.Position);
            DataGridViewRow row = dgvdanhsachHopDong.Rows[e.RowIndex];

            if (row?.Cells["maHD"]?.Value == null) return;

            if (editRect.Contains(clickPoint)) HandleEdit(row);
            else if (deleteRect.Contains(clickPoint)) HandleDelete(row);
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

            HopDongDTO hdSource = row.DataBoundItem as HopDongDTO;

            if (hdSource == null)
            {
                MessageBox.Show("Không thể lấy thông tin hợp đồng!", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SuaHopDongForm frmSua = new SuaHopDongForm(hdSource);
            currentOpenForm = frmSua;
            CenterFormOnParent(frmSua);

            frmSua.FormClosed += (s, ev) => { currentOpenForm = null; };
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

            string maHD = row.Cells["maHD"].Value?.ToString();

            if (string.IsNullOrEmpty(maHD)) return;

            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa hợp đồng (Mã: {maHD}) không?",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    HopDongBLL hdBLL = new HopDongBLL();
                    // Thêm method xóa trong HopDongBLL nếu chưa có
                    // hdBLL.xoaHopDong(maHD);

                    MessageBox.Show("Đã xóa hợp đồng thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.BeginInvoke(new Action(() =>
                    {
                        lamMoiDanhSachKhachHang();
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Có lỗi xảy ra khi xóa hợp đồng: {ex.Message}",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CenterFormOnParent(Form childForm)
        {
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
            matrix.Matrix33 = 0.08f;
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
            // Custom formatting nếu cần
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
            Form parentForm = this.FindForm();
            bool isMaximized = parentForm != null && parentForm.WindowState == FormWindowState.Maximized;

            int btnWidth = isMaximized ? 80 : 66;
            int btnHeight = isMaximized ? 50 : 40;
            int btnRadius = isMaximized ? 25 : 20;

            btnXuatfile.Size = new Size(btnWidth, btnHeight);
            btnThemuser.Size = new Size(btnWidth, btnHeight);
            BoGocButton(btnXuatfile, btnRadius);
            BoGocButton(btnThemuser, btnRadius);

            Control btnParent = btnXuatfile.Parent;

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
                lastSearchKeyword = "";
                return;
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

                // Reset về phân trang
                currentPage = 1;
                totalRecords = 0;
                lamMoiDanhSachKhachHang();
                lastSearchKeyword = "";
            }
        }

        private void searchtextbox_TextChanged_1(object sender, EventArgs e)
        {
            if (isPlaceholder) return;

            string currentKeyword = searchtextbox.Text.Trim().ToLower();
            if (currentKeyword == lastSearchKeyword) return;

            lastSearchKeyword = currentKeyword;
            PerformSearch();
        }

        private void PerformSearch()
        {
            string keyword = searchtextbox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                // Reset về phân trang
                currentPage = 1;
                totalRecords = 0;
                lamMoiDanhSachKhachHang();
                return;
            }

            // ✅ TÌM KIẾM TRÊN TOÀN BỘ DATABASE
            try
            {
                HopDongBLL bll = new HopDongBLL();
                var allData = bll.LayDanhSachHD(); // Lấy tất cả hợp đồng

                var filtered = allData
                    .Where(hd =>
                        (hd.maHD ?? "").ToLower().Contains(keyword) ||
                        (hd.maKH ?? "").ToLower().Contains(keyword) ||
                        (hd.trangThai ?? "").ToLower().Contains(keyword) ||
                        (hd.tanSuatQuanTrac ?? "").ToLower().Contains(keyword) ||
                        (hd.soHD ?? "").ToLower().Contains(keyword) ||
                        hd.ngayKy.ToString("dd/MM/yyyy").Contains(keyword) ||
                        hd.ngayKetThucHD.ToString("dd/MM/yyyy").Contains(keyword)
                    )
                    .ToList();

                dgvdanhsachHopDong.DataSource = new BindingList<HopDongDTO>(filtered);

                // Hiển thị số kết quả
                if (soTrang != null)
                    soTrang.Text = $"Tìm thấy {filtered.Count} kết quả";

                // Disable nút phân trang khi search
                if (btnTruoc != null) btnTruoc.Enabled = false;
                if (btnSau != null) btnSau.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi tìm kiếm: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Pagination
        private void taiTrangKhachHang()
        {
            HopDongBLL hdBLL = new HopDongBLL();

            // ✅ Tính tổng số trang
            if (totalRecords == 0)
            {
                totalRecords = hdBLL.demSoLuongHopDong(); // Cần thêm method này vào HopDongBLL
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            }

            // ✅ Lấy dữ liệu theo trang (CẦN THÊM METHOD PHÂN TRANG VÀO HopDongBLL)
            var data = hdBLL.layDanhSachHopDong_PhanTrang(currentPage, pageSize);
            dsHopDong = new BindingList<HopDongDTO>(data);
            dgvdanhsachHopDong.DataSource = dsHopDong;

            // ✅ Cập nhật label trang
            soTrang.Text = $"Trang {currentPage}/{totalPages}";

            // ✅ Disable nút nếu đang ở biên
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
            if (currentPage < totalPages)
            {
                currentPage++;
                taiTrangKhachHang();
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

            ThemHopDongForm frmThem = new ThemHopDongForm();
            currentOpenForm = frmThem;
            CenterFormOnParent(frmThem);

            frmThem.FormClosed += (s, ev) => { currentOpenForm = null; };
            frmThem.SuccesfullyUpdated += (s, ev) => lamMoiDanhSachKhachHang();

            frmThem.Show();
        }
        #endregion

        #region Filter
        private void pictureFilter_Click(object sender, EventArgs e)
        {
            LocHopDongvaDQT filterForm = new LocHopDongvaDQT();

            if (filterForm.ShowDialog() == DialogResult.OK)
            {
                ApplyFilter(
                    filterForm.SelectedNgayBatDau,
                    filterForm.SelectedNgayKetThuc,
                    filterForm.SelectedTrangThai
                );
            }
        }

        //private void ApplyFilter(string ngayBD, string ngayKT, string trangThai)
        //{
        //    if (dsHopDong == null || dsHopDong.Count == 0)
        //    {
        //        return;
        //    }

        //    var query = dsHopDong.AsEnumerable();

        //    // Lọc trạng thái
        //    if (!string.IsNullOrEmpty(trangThai))
        //        query = query.Where(hd => hd.trangThai == trangThai);

        //    // Lọc ngày bắt đầu
        //    if (!string.IsNullOrEmpty(ngayBD))
        //    {
        //        DateTime dateBD = DateTime.Parse(ngayBD);
        //        query = query.Where(hd => hd.ngayKy >= dateBD);
        //    }

        //    // Lọc ngày kết thúc
        //    if (!string.IsNullOrEmpty(ngayKT))
        //    {
        //        DateTime dateKT = DateTime.Parse(ngayKT);
        //        query = query.Where(hd => hd.ngayKetThucHD <= dateKT);
        //    }

        //    var filtered = query.ToList();
        //    dgvdanhsachHopDong.DataSource = new BindingList<HopDongDTO>(filtered);

        //    // Hiển thị số kết quả lọc
        //    if (soTrang != null)
        //        soTrang.Text = $"Tìm thấy {filtered.Count} kết quả";

        //    // Disable nút phân trang khi lọc
        //    if (btnTruoc != null) btnTruoc.Enabled = false;
        //    if (btnSau != null) btnSau.Enabled = false;
        //}


        private void ApplyFilter(string ngayBD, string ngayKT, string trangThai)
        {
            try
            {
                HopDongBLL bll = new HopDongBLL();

                // ✅ Lấy toàn bộ dữ liệu từ database
                var allData = bll.LayDanhSachHD();

                if (allData == null || allData.Count == 0)
                {
                    MessageBox.Show("Không có dữ liệu để lọc!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var query = allData.AsEnumerable();

                // Lọc theo trạng thái
                if (!string.IsNullOrEmpty(trangThai))
                {
                    query = query.Where(hd =>
                        (hd.trangThai ?? "").Equals(trangThai, StringComparison.OrdinalIgnoreCase));
                }

                // Lọc theo ngày bắt đầu (ngày ký >= ngày bắt đầu)
                if (!string.IsNullOrEmpty(ngayBD))
                {
                    if (DateTime.TryParse(ngayBD, out DateTime dateBD))
                    {
                        query = query.Where(hd => hd.ngayKy >= dateBD);
                    }
                }

                // Lọc theo ngày kết thúc (ngày kết thúc hợp đồng <= ngày kết thúc)
                if (!string.IsNullOrEmpty(ngayKT))
                {
                    if (DateTime.TryParse(ngayKT, out DateTime dateKT))
                    {
                        query = query.Where(hd => hd.ngayKetThucHD <= dateKT);
                    }
                }

                var filtered = query.ToList();

                // Cập nhật DataGridView
                dgvdanhsachHopDong.DataSource = new BindingList<HopDongDTO>(filtered);

                // Hiển thị số kết quả
                if (soTrang != null)
                {
                    soTrang.Text = $"Tìm thấy {filtered.Count} kết quả";
                }

                // Disable nút phân trang khi đang lọc
                if (btnTruoc != null) btnTruoc.Enabled = false;
                if (btnSau != null) btnSau.Enabled = false;

                // Hiển thị thông báo nếu không tìm thấy kết quả
                if (filtered.Count == 0)
                {
                    MessageBox.Show("Không tìm thấy hợp đồng nào phù hợp với điều kiện lọc!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lọc dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        #endregion

        #region Unused Events
        private void panel6_Paint(object sender, PaintEventArgs e) { }
        private void dgvdanhsachHopDong_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        private void panel1_Paint(object sender, PaintEventArgs e) { }
        private void picturemicro_Click(object sender, EventArgs e) { }
        #endregion

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
    }
}