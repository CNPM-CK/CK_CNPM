using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BLL.Speech;
using GUI.Helper;

namespace GUI.Forms
{
    public partial class DSKQUC : UserControl
    {
        private KetQuaBLL ketQuaBLL = new KetQuaBLL();

        // Voice search
        private VoiceRecorder _recorder;
        private WhisperService _whisper;
        private string _wavPath;
        private bool _ready = false;

        // Search
        private List<DTO_KetQuaHeader> danhSachGoc = new List<DTO_KetQuaHeader>();
        private bool isPlaceholder = true;
        private const string PLACEHOLDER_TEXT = "Tìm kiếm kết quả...";

        // Search box styling  
        private Color borderColor = Color.Black;
        private int borderRadius = 12;
        private int borderSize = 2;

        // Layout constants - GIỐNG DSNV_Uc
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;
        private const int SEARCH_HEIGHT = 50;

        // Phân trang
        private int trangHienTai = 1;
        private int kichThuocTrang = 15;
        private int tongSoBanGhi = 0;
        private int tongSoTrang = 0;

        public DSKQUC()
        {
            InitializeComponent();
            this.Load += DSKQUC_Load;
            this.Resize += DSKQUC_Resize;
            dgvDanhsachketqua.CellDoubleClick += dgvDanhsachketqua_CellDoubleClick;
            picturemicro.Click += BtnMic_Click;
            btnTruoc.Click += btnTruoc_Click;
            btnSau.Click += btnSau_Click;

            // Thêm sự kiện cho button1
            if (button1 != null)
            {
                button1.Click += Button1_Click;
            }
        }

        private async void DSKQUC_Load(object sender, EventArgs e)
        {
            // QUAN TRỌNG: Setup DataGridView TRƯỚC
            SetupDataGridView();

            // Khởi tạo search box
            InitializeCustomSearchBox();

            // Khởi tạo button styles
            InitializeButtonStyles();

            // Load data với phân trang
            LoadDanhSachKetQua();

            // Set column widths
            dgvDanhsachketqua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            SetColumnWidths();

            // Tính toán layout ban đầu
            CalculateLayout();

            // ========== KHỞI TẠO VOICE SEARCH ==========
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string modelPath = Path.Combine(baseDir, "Model", "ggml-tiny.bin");
            _wavPath = Path.Combine(baseDir, "TempAudio", "search_kq.wav");

            _recorder = new VoiceRecorder(_wavPath);
            string appId = "ga825cbd";
            string apiKey = "55774f42c55202232e1b4d8ebfc314c5";
            string apiSecret = "c1cb5fc788d78ec4b808e8cc4beb4a3d";

            var iatService = new IATService(appId, apiKey, apiSecret);
            _whisper = new WhisperService(modelPath, iatService);

            picturemicro.Enabled = false;

            try
            {
                await _whisper.InitAsync();
                _ready = true;
                picturemicro.Enabled = true;
            }
            catch (Exception ex)
            {
                picturemicro.Enabled = false;
            }
        }

        // ========== KHỞI TẠO BUTTON STYLES ==========
        private void InitializeButtonStyles()
        {
            if (button1 != null)
            {
                button1.Size = new Size(66, 40);
                BoGocButton(button1, 20);
            }

            BoGocButton(btnTruoc, 20);
            BoGocButton(btnSau, 20);
        }

        // ========== BUTTON STYLING ==========
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

        // ========== LAYOUT & RESIZE - GIỐNG DSNV_Uc ==========
        private void DSKQUC_Resize(object sender, EventArgs e)
        {
            if (this.ClientSize.Width < 100) return;
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

            // Cập nhật button1 nếu tồn tại
            if (button1 != null)
            {
                button1.Size = new Size(btnWidth, btnHeight);
                BoGocButton(button1, btnRadius);
            }

            // Lấy parent panel của các controls (giả sử là panel6 hoặc tương tự)
            Control btnParent = button1?.Parent;

            if (btnParent != null && btnParent != this)
            {
                int parentWidth = btnParent.Width;

                // Đặt button1 ở góc phải cùng
                if (button1 != null)
                {
                    button1.Left = parentWidth - btnWidth - MARGIN;
                    button1.Top = 10;
                }
            }
            else if (button1 != null)
            {
                button1.Left = formWidth - btnWidth - MARGIN;
                button1.Top = 10;
            }

            // Đặt pictureFilter ở góc trái
            if (pictureFilter != null)
            {
                pictureFilter.Left = MARGIN;
                pictureFilter.Top = 10;
            }

            // Tính toán vị trí cho containersearch và picturemicro
            int leftBoundary = pictureFilter != null ? pictureFilter.Right + SPACING : MARGIN;
            int rightBoundary = button1 != null ?
                button1.Left - SPACING - (picturemicro != null ? picturemicro.Width : 0) - SPACING :
                formWidth - MARGIN;

            int availableWidth = rightBoundary - leftBoundary;

            // Tính width cho search box
            int searchWidth = Math.Max(MIN_SEARCH_WIDTH, Math.Min(availableWidth, MAX_SEARCH_WIDTH));
            if (searchWidth < MIN_SEARCH_WIDTH)
            {
                searchWidth = Math.Max(150, availableWidth);
            }

            // Đặt containersearch
            if (containersearch != null)
            {
                containersearch.Left = leftBoundary;
                containersearch.Width = searchWidth;
                containersearch.Height = SEARCH_HEIGHT;
                containersearch.Top = 10;
            }

            // Cập nhật searchtextbox
            if (searchtextbox != null && containersearch != null)
            {
                searchtextbox.Width = searchWidth - (borderSize * 2 + 10);
                searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);
            }

            // Đặt picturemicro
            if (picturemicro != null && containersearch != null)
            {
                picturemicro.Left = containersearch.Right + SPACING;
                picturemicro.Top = 10;
            }

            // Cập nhật padding cho button1
            if (button1 != null)
            {
                if (isMaximized)
                {
                    button1.Padding = new Padding(10, 5, 10, 5);
                }
                else
                {
                    button1.Padding = new Padding(5, 3, 5, 3);
                }
            }

            // Invalidate để vẽ lại
            if (containersearch != null)
            {
                containersearch.Invalidate();
            }
        }

        // ========== BUTTON1 CLICK EVENT ==========
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                QuanLyHopDongChuKy formBieuDo = new QuanLyHopDongChuKy();
                formBieuDo.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở form biểu đồ: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetColumnWidths()
        {
            if (dgvDanhsachketqua.Columns["STT"] != null)
                dgvDanhsachketqua.Columns["STT"].Width = 60;
            if (dgvDanhsachketqua.Columns["TenCongTy"] != null)
                dgvDanhsachketqua.Columns["TenCongTy"].Width = 280;
            if (dgvDanhsachketqua.Columns["DotQuanTrac"] != null)
                dgvDanhsachketqua.Columns["DotQuanTrac"].Width = 280;
            if (dgvDanhsachketqua.Columns["NgayTao"] != null)
                dgvDanhsachketqua.Columns["NgayTao"].Width = 125;
            if (dgvDanhsachketqua.Columns["NgayTraKQ"] != null)
                dgvDanhsachketqua.Columns["NgayTraKQ"].Width = 125;
            if (dgvDanhsachketqua.Columns["TenNhanVien"] != null)
                dgvDanhsachketqua.Columns["TenNhanVien"].Width = 200;
            if (dgvDanhsachketqua.Columns["TrangThai"] != null)
                dgvDanhsachketqua.Columns["TrangThai"].Width = 140;
        }

        private void SetupDataGridView()
        {
            dgvDanhsachketqua.Columns.Clear();
            dgvDanhsachketqua.AutoGenerateColumns = false;
            dgvDanhsachketqua.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDanhsachketqua.MultiSelect = false;
            dgvDanhsachketqua.ReadOnly = true;
            dgvDanhsachketqua.AllowUserToAddRows = false;
            dgvDanhsachketqua.AllowUserToDeleteRows = false;
            dgvDanhsachketqua.RowHeadersVisible = false;
            dgvDanhsachketqua.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            dgvDanhsachketqua.AllowUserToResizeRows = false;
            dgvDanhsachketqua.BackgroundColor = Color.White;
            dgvDanhsachketqua.BorderStyle = BorderStyle.None;
            dgvDanhsachketqua.EnableHeadersVisualStyles = false;
            dgvDanhsachketqua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            int headerHeight = 40;
            int rowHeight = 45;
            dgvDanhsachketqua.ColumnHeadersHeight = headerHeight;
            dgvDanhsachketqua.RowTemplate.Height = rowHeight;

            dgvDanhsachketqua.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = Color.FromArgb(200, 200, 200),
                Padding = new Padding(5),
                WrapMode = DataGridViewTriState.False
            };

            dgvDanhsachketqua.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = Color.Black,
                SelectionBackColor = Color.FromArgb(200, 200, 200),
                SelectionForeColor = Color.Black,
                Padding = new Padding(8, 0, 8, 0),
                WrapMode = DataGridViewTriState.False
            };

            dgvDanhsachketqua.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                SelectionBackColor = Color.FromArgb(200, 200, 200),
                SelectionForeColor = Color.Black,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.Black
            };

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STT",
                HeaderText = "STT",
                Width = 50,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenCongTy",
                HeaderText = "Tên Công Ty",
                DataPropertyName = "TenKhachHang",
                Width = 280,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DotQuanTrac",
                HeaderText = "Tên Đợt Quan Trắc",
                DataPropertyName = "DotQuanTrac",
                Width = 300,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgayTao",
                HeaderText = "Ngày Tạo",
                DataPropertyName = "NgayTao",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy",
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.White,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgayTraKQ",
                HeaderText = "Ngày Trả KQ",
                DataPropertyName = "NgayTraKQ",
                Width = 110,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy",
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.White,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenNhanVien",
                HeaderText = "Người Lập",
                DataPropertyName = "TenNhanVien",
                Width = 200,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng Thái",
                DataPropertyName = "TrangThai",
                Width = 140,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                    Padding = new Padding(8, 0, 8, 0),
                    ForeColor = Color.Black
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GhiChu",
                HeaderText = "Ghi Chú",
                DataPropertyName = "GhiChu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 150,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 10, 0),
                    WrapMode = DataGridViewTriState.False,
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(108, 117, 125)
                }
            });
        }

        // ========== PHÂN TRANG ==========
        public void LoadDanhSachKetQua()
        {
            try
            {
                if (tongSoBanGhi == 0)
                {
                    tongSoBanGhi = ketQuaBLL.demTongSoKetQua();
                    tongSoTrang = (int)Math.Ceiling((double)tongSoBanGhi / kichThuocTrang);
                }

                var list = ketQuaBLL.layDanhSachKetQua_PhanTrang(trangHienTai, kichThuocTrang);
                danhSachGoc = list;

                dgvDanhsachketqua.Rows.Clear();

                int sttBatDau = (trangHienTai - 1) * kichThuocTrang;
                int stt = 0;

                foreach (var item in list)
                {
                    stt++;
                    int rowIndex = dgvDanhsachketqua.Rows.Add();
                    var row = dgvDanhsachketqua.Rows[rowIndex];

                    row.Cells["STT"].Value = sttBatDau + stt;
                    row.Cells["TenCongTy"].Value = item.TenKhachHang ?? "";
                    row.Cells["DotQuanTrac"].Value = item.DotQuanTrac ?? "";
                    row.Cells["NgayTao"].Value = item.NgayTao;
                    row.Cells["NgayTraKQ"].Value = item.NgayTraKQ;
                    row.Cells["TenNhanVien"].Value = item.TenNhanVien ?? "";
                    row.Cells["TrangThai"].Value = item.TrangThai;
                    row.Cells["GhiChu"].Value = item.GhiChu ?? "";
                    row.Tag = item.MaKQ;
                }

                FormatDataGridView();
                dgvDanhsachketqua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                SetColumnWidths();

                soTrang.Text = $"Trang {trangHienTai}/{tongSoTrang}";

                btnTruoc.Enabled = trangHienTai > 1;
                btnSau.Enabled = trangHienTai < tongSoTrang;

                if (list.Count == 0 && tongSoBanGhi == 0)
                {
                    MessageBox.Show("Chưa có kết quả quan trắc nào!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách kết quả: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            if (trangHienTai > 1)
            {
                trangHienTai--;
                LoadDanhSachKetQua();
            }
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            if (trangHienTai < tongSoTrang)
            {
                trangHienTai++;
                LoadDanhSachKetQua();
            }
        }

        private void FormatDataGridView()
        {
            foreach (DataGridViewRow row in dgvDanhsachketqua.Rows)
            {
                if (row.Cells["TrangThai"].Value != null)
                {
                    string trangThai = row.Cells["TrangThai"].Value.ToString().Trim();

                    if (trangThai.Equals("Đã xác nhận", StringComparison.OrdinalIgnoreCase))
                    {
                        row.Cells["TrangThai"].Value = "✓ Đã xác nhận";
                        row.Cells["TrangThai"].Style.BackColor = Color.White;
                        row.Cells["TrangThai"].Style.ForeColor = Color.Black;
                        row.Cells["TrangThai"].Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
                    }
                    else
                    {
                        row.Cells["TrangThai"].Value = "○ Chờ xác nhận";
                        row.Cells["TrangThai"].Style.BackColor = Color.White;
                        row.Cells["TrangThai"].Style.ForeColor = Color.Black;
                        row.Cells["TrangThai"].Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Regular);
                    }
                }
                row.DefaultCellStyle.SelectionBackColor = Color.LightGray;
                row.DefaultCellStyle.SelectionForeColor = Color.Black;
            }
        }

        private void dgvDanhsachketqua_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                try
                {
                    string maKQ = dgvDanhsachketqua.Rows[e.RowIndex].Tag?.ToString();

                    if (string.IsNullOrEmpty(maKQ))
                    {
                        MessageBox.Show("Không tìm thấy mã kết quả!",
                            "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    ChiTietKetQua formChiTiet = new ChiTietKetQua(maKQ);
                    DialogResult result = formChiTiet.ShowDialog();

                    if (result == DialogResult.OK)
                    {
                        LoadDanhSachKetQua();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi mở chi tiết: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void dgvDanhsachketqua_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void dgvDanhsachketqua_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                if (Properties.Resources.greenlogo == null) return;

                int dgvWidth = dgvDanhsachketqua.Width;
                int dgvHeight = dgvDanhsachketqua.Height;
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
            catch { }
        }

        private void panel6_Paint_2(object sender, PaintEventArgs e)
        {
        }

        // ========== CUSTOM SEARCH BOX ==========
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

            searchtextbox.Enter += searchtextbox_Enter;
            searchtextbox.Leave += searchtextbox_Leave;
            searchtextbox.TextChanged += searchtextbox_TextChanged;
            searchtextbox.KeyDown += searchtextbox_KeyDown;
            containersearch.Paint += containersearch_Paint;
        }

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

                // Reset về phân trang
                trangHienTai = 1;
                tongSoBanGhi = 0;
                LoadDanhSachKetQua();
            }
        }

        private void searchtextbox_TextChanged(object sender, EventArgs e)
        {
            if (isPlaceholder) return;
            PerformSearch();
        }

        private void searchtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (dgvDanhsachketqua.Rows.Count > 0)
                {
                    dgvDanhsachketqua.ClearSelection();
                    dgvDanhsachketqua.Rows[0].Selected = true;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                searchtextbox.Clear();

                // Reset về phân trang
                trangHienTai = 1;
                tongSoBanGhi = 0;
                LoadDanhSachKetQua();
            }
        }

        private void PerformSearch()
        {
            string keyword = searchtextbox.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(keyword))
            {
                // Reset về phân trang
                trangHienTai = 1;
                tongSoBanGhi = 0;
                LoadDanhSachKetQua();
                return;
            }

            try
            {
                // Lấy toàn bộ dữ liệu để search
                var allData = ketQuaBLL.LayDanhSachKetQuaMoi();

                var filtered = allData.Where(item =>
                    (item.TenKhachHang ?? "").ToLower().Contains(keyword) ||
                    (item.DotQuanTrac ?? "").ToLower().Contains(keyword) ||
                    (item.TenNhanVien ?? "").ToLower().Contains(keyword) ||
                    (item.TrangThai ?? "").ToLower().Contains(keyword) ||
                    (item.GhiChu ?? "").ToLower().Contains(keyword)
                ).ToList();

                dgvDanhsachketqua.Rows.Clear();
                int stt = 0;
                foreach (var item in filtered)
                {
                    stt++;
                    int rowIndex = dgvDanhsachketqua.Rows.Add();
                    var row = dgvDanhsachketqua.Rows[rowIndex];
                    row.Cells["STT"].Value = stt;
                    row.Cells["TenCongTy"].Value = item.TenKhachHang ?? "";
                    row.Cells["DotQuanTrac"].Value = item.DotQuanTrac ?? "";
                    row.Cells["NgayTao"].Value = item.NgayTao;
                    row.Cells["NgayTraKQ"].Value = item.NgayTraKQ;
                    row.Cells["TenNhanVien"].Value = item.TenNhanVien ?? "";
                    row.Cells["TrangThai"].Value = item.TrangThai;
                    row.Cells["GhiChu"].Value = item.GhiChu ?? "";
                    row.Tag = item.MaKQ;
                }
                FormatDataGridView();

                soTrang.Text = $"Tìm thấy {filtered.Count} kết quả";

                btnTruoc.Enabled = false;
                btnSau.Enabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ========== VOICE SEARCH ==========
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
                await System.Threading.Tasks.Task.Delay(300);

                try
                {
                    string text;
                    try { text = await _whisper.TranscribeIFlytekAsync(_wavPath); }
                    catch { text = await _whisper.TranscribeAsync(_wavPath); }

                    if (string.IsNullOrWhiteSpace(text))
                    {
                        MessageBox.Show("Không nghe được nội dung");
                        return;
                    }

                    isPlaceholder = false;
                    searchtextbox.ForeColor = Color.FromArgb(64, 64, 64);
                    searchtextbox.Text = text.Trim();
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