using BLL;
using DTO;
using GUI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class DanhSachKetQuaUC : UserControl
    {
        private readonly bool _isPhongKetQua = SessionStore.Current.MaPhong == "P005";
        #region Fields
        private readonly KetQuaBLL ketQuaBLL = new KetQuaBLL();

        private Color borderColor = Color.Black;
        private int borderRadius = 12;
        private int borderSize = 2;

        // Layout constants
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;
        private const int SEARCH_HEIGHT = 50;
        private const string PLACEHOLDER_TEXT = "Tìm kiếm kết quả...";

        // Phân trang (nếu cần mở rộng sau)
        private int currentPage = 1;
        private int pageSize = 15;
        private int totalRecords = 0;
        private int totalPages = 0;

        // Search
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";

        // Mở form con (chi tiết)
        private Form currentOpenForm = null;

        // Vùng vẽ thao tác (nếu cần thêm icon sau)
        private Rectangle editRect;
        private Rectangle deleteRect;
        #endregion

        #region Constructor
        public DanhSachKetQuaUC()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            // Load khi UC được tạo
            this.Load += DanhSachKetQuaUC_Load;
            // Dùng override OnResize để tính layout mượt hơn
        }
        #endregion

        #region Load
        private void DanhSachKetQuaUC_Load(object sender, EventArgs e)
        {
            // Ảnh header (nếu có)

            InitializeButtonIcons();
            InitializeButtonStyles();
            InitializeCustomSearchBox();

            SetupDataGridView();
            LoadDanhSachKetQua();

            CalculateLayout();
        }
        #endregion

        #region DataGridView setup & data
        private void SetupDataGridView()
        {
            if (dgvDanhsachketqua == null) return;

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

            // Chiều cao
            int headerHeight = 40;
            int rowHeight = 45;
            dgvDanhsachketqua.ColumnHeadersHeight = headerHeight;
            dgvDanhsachketqua.RowTemplate.Height = rowHeight;

            // Header style
            dgvDanhsachketqua.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(0, 152, 70),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10.5F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = Color.FromArgb(0, 152, 70),
                Padding = new Padding(5),
                WrapMode = DataGridViewTriState.False
            };

            // Cell style
            dgvDanhsachketqua.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9.5F),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(33, 37, 41),
                SelectionBackColor = Color.FromArgb(111, 207, 151),
                SelectionForeColor = Color.White,
                Padding = new Padding(8, 0, 8, 0),
                WrapMode = DataGridViewTriState.False
            };

            dgvDanhsachketqua.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                SelectionBackColor = Color.FromArgb(111, 207, 151),
                SelectionForeColor = Color.White
            };

            // Cột
            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STT",
                HeaderText = "STT",
                Width = 60,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 102, 204)
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaDot",
                HeaderText = "Mã Đợt",
                DataPropertyName = "MaDot",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 102, 204),
                    BackColor = Color.FromArgb(230, 245, 255)
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DotQuanTrac",
                HeaderText = "Tên Đợt Quan Trắc",
                DataPropertyName = "DotQuanTrac",
                Width = 240,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular)
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgayTao",
                HeaderText = "Ngày Tạo",
                DataPropertyName = "NgayTao",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy",
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(245, 250, 255),
                    Font = new Font("Segoe UI", 9.5F)
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgayTraKQ",
                HeaderText = "Ngày Trả KQ",
                DataPropertyName = "NgayTraKQ",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy",
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    BackColor = Color.FromArgb(255, 250, 245),
                    Font = new Font("Segoe UI", 9.5F)
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
                    Font = new Font("Segoe UI", 9.5F)
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "SoNenMau",
                HeaderText = "Số Nền",
                DataPropertyName = "SoNenMau",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 10.5F, FontStyle.Bold)
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Trạng Thái",
                DataPropertyName = "TrangThai",
                Width = 160,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    Padding = new Padding(8, 0, 8, 0)
                }
            });

            dgvDanhsachketqua.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GhiChu",
                HeaderText = "Ghi Chú",
                DataPropertyName = "GhiChu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                MinimumWidth = 220,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Padding = new Padding(10, 0, 10, 0),
                    WrapMode = DataGridViewTriState.False,
                    Font = new Font("Segoe UI", 9F, FontStyle.Italic),
                    ForeColor = Color.FromArgb(108, 117, 125)
                }
            });

            // Double click mở chi tiết
            dgvDanhsachketqua.CellDoubleClick -= dgvDanhsachketqua_CellDoubleClick;
            dgvDanhsachketqua.CellDoubleClick += dgvDanhsachketqua_CellDoubleClick;

            // Vẽ watermark
            dgvDanhsachketqua.Paint -= dgvDanhsachketqua_Paint;
            dgvDanhsachketqua.Paint += dgvDanhsachketqua_Paint;
        }

        private void LoadDanhSachKetQua()
        {
            try
            {
                List<DTO_KetQuaHeader> list = ketQuaBLL.LayDanhSachKetQuaMoi();

                dgvDanhsachketqua.Rows.Clear();

                int stt = 0;
                foreach (var item in list)
                {
                    stt++;
                    int rowIndex = dgvDanhsachketqua.Rows.Add();
                    var row = dgvDanhsachketqua.Rows[rowIndex];

                    row.Cells["STT"].Value = stt;
                    row.Cells["MaDot"].Value = item.MaDot ?? "";
                    row.Cells["DotQuanTrac"].Value = item.DotQuanTrac ?? "";
                    row.Cells["NgayTao"].Value = item.NgayTao;
                    row.Cells["NgayTraKQ"].Value = item.NgayTraKQ;
                    row.Cells["TenNhanVien"].Value = item.TenNhanVien ?? "";
                    row.Cells["SoNenMau"].Value = item.SoNenMau;
                    row.Cells["TrangThai"].Value = item.TrangThai;
                    row.Cells["GhiChu"].Value = item.GhiChu ?? "";

                    // lưu MaKQ để mở chi tiết
                    row.Tag = item.MaKQ;
                }

                FormatDataGridView();

                // Cập nhật tiêu đề
                if (panel6 != null)
                {
                    panel6.Controls.Clear();
                    Label lblTitle = new Label
                    {
                        Text = $"📊 DANH SÁCH KẾT QUẢ QUAN TRẮC ({list.Count} kết quả)",
                        Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(0, 152, 70),
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    panel6.Controls.Add(lblTitle);
                }

                if (list.Count == 0)
                {
                    MessageBox.Show("Chưa có kết quả quan trắc nào!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Nếu có phân trang: cập nhật nhãn
                if (soTrang != null && totalPages > 0)
                    soTrang.Text = $"Trang {currentPage}/{totalPages}";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách kết quả: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        row.Cells["TrangThai"].Value = "Đã xác nhận";
                        row.Cells["TrangThai"].Style.BackColor = Color.FromArgb(200, 255, 200);
                        row.Cells["TrangThai"].Style.ForeColor = Color.FromArgb(0, 128, 0);
                    }
                    else
                    {
                        row.Cells["TrangThai"].Value = "Chờ xác nhận";
                        row.Cells["TrangThai"].Style.BackColor = Color.FromArgb(255, 245, 200);
                        row.Cells["TrangThai"].Style.ForeColor = Color.FromArgb(204, 136, 0);
                    }
                }

                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(111, 207, 151);
                row.DefaultCellStyle.SelectionForeColor = Color.White;
            }
        }
        #endregion

        #region Events - DoubleClick, Paint, ContentClick
        private void dgvDanhsachketqua_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isPhongKetQua) return;
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

                    // Mở form chi tiết
                    ChiTietKetQua formChiTiet = new ChiTietKetQua(maKQ);

                    // Center theo parent (UC)
                    CenterFormOnParent(formChiTiet);

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

        private void dgvDanhsachketqua_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Resources.greenlogo == null) return;

            int dgvWidth = dgvDanhsachketqua.Width;
            int dgvHeight = dgvDanhsachketqua.Height;
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

        private void dgvDanhsachketqua_CellContentClick(object sender, DataGridViewCellEventArgs e) { }
        #endregion

        #region Layout & Resize (theo mẫu UC)
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.Width < 100) return;
            CalculateLayout();
        }

        private void CalculateLayout()
        {
            // Vị trí – cụm filter & search giống mẫu
            if (pictureFilter != null) pictureFilter.Left = MARGIN;

            if (btnXuatfile != null && btnThemuser != null)
            {
                int isMaximizedPad = 0;
                int btnWidth = 66, btnHeight = 40, btnRadius = 20;

                // Nếu parent form đang maximized → có thể tăng size
                Form parentForm = this.FindForm();
                bool isMaximized = parentForm != null && parentForm.WindowState == FormWindowState.Maximized;
                if (isMaximized)
                {
                    btnWidth = 80; btnHeight = 50; btnRadius = 25;
                }

                btnXuatfile.Size = new Size(btnWidth, btnHeight);
                btnThemuser.Size = new Size(btnWidth, btnHeight);
                BoGocButton(btnXuatfile, btnRadius);
                BoGocButton(btnThemuser, btnRadius);

                Control btnParent = btnXuatfile.Parent; // thường là panel6
                int parentWidth = (btnParent != null) ? btnParent.Width : this.Width;

                btnXuatfile.Left = parentWidth - btnWidth - MARGIN - isMaximizedPad;
                btnThemuser.Left = btnXuatfile.Left - btnWidth - SPACING;

                int topPosition = 10;
                btnXuatfile.Top = topPosition;
                btnThemuser.Top = topPosition;
            }

            if (panel7 != null && panel6 != null)
            {
                panel7.Top = panel6.Bottom + 15;
            }

            // Tính chiều rộng search box
            if (containersearch != null && searchtextbox != null && btnThemuser != null && pictureFilter != null && picturemicro != null)
            {
                int leftBoundary = pictureFilter.Right + SPACING;
                int rightBoundary = btnThemuser.Left - SPACING - picturemicro.Width - SPACING;
                int availableWidth = rightBoundary - leftBoundary;

                int searchWidth = Math.Max(MIN_SEARCH_WIDTH, Math.Min(availableWidth, MAX_SEARCH_WIDTH));
                if (searchWidth < MIN_SEARCH_WIDTH)
                {
                    searchWidth = Math.Max(150, availableWidth);
                }

                containersearch.Left = leftBoundary;
                containersearch.Width = Math.Max(0, searchWidth);
                containersearch.Height = SEARCH_HEIGHT;

                searchtextbox.Width = Math.Max(0, searchWidth - (borderSize * 2 + 10));
                searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);

                picturemicro.Left = containersearch.Right + SPACING;

                if (this.FindForm() != null && this.FindForm().WindowState == FormWindowState.Maximized)
                {
                    if (btnThemuser != null) btnThemuser.Padding = new Padding(10, 5, 10, 5);
                    if (btnXuatfile != null) btnXuatfile.Padding = new Padding(10, 5, 10, 5);
                }
                else
                {
                    if (btnThemuser != null) btnThemuser.Padding = new Padding(5, 3, 5, 3);
                    if (btnXuatfile != null) btnXuatfile.Padding = new Padding(5, 3, 5, 3);
                }

                containersearch.Invalidate();
            }
        }

        #endregion

        #region Button init & style (giữ đúng mẫu UC)
        private void InitializeButtonIcons()
        {
            if (btnThemuser != null && btnThemuser.Image != null)
            {
                btnThemuser.Image = new Bitmap(btnThemuser.Image, new Size(24, 24));
            }

            if (btnXuatfile != null && btnXuatfile.Image != null)
            {
                btnXuatfile.Image = new Bitmap(btnXuatfile.Image, new Size(24, 24));
            }
        }

        private void InitializeButtonStyles()
        {
            if (btnThemuser == null || btnXuatfile == null) return;

            btnThemuser.Size = new Size(66, 40);
            btnXuatfile.Size = new Size(66, 40);

            BoGocButton(btnThemuser, 20);
            BoGocButton(btnXuatfile, 20);
            if (btnTruoc != null) BoGocButton(btnTruoc, 20);
            if (btnSau != null) BoGocButton(btnSau, 20);

            btnThemuser.Click -= btnThemuser_Click;
            btnThemuser.Click += btnThemuser_Click;
        }

        private void BoGocButton(Button btn, int radius)
        {
            if (btn == null) return;

            using (var path = new System.Drawing.Drawing2D.GraphicsPath())
            {
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
        }
        #endregion

        #region Search box (optional giữ cấu trúc mẫu)
        private void InitializeCustomSearchBox()
        {
            if (containersearch == null || searchtextbox == null) return;

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

            searchtextbox.Enter -= searchtextbox_Enter;
            searchtextbox.Leave -= searchtextbox_Leave;
            searchtextbox.TextChanged -= searchtextbox_TextChanged_1;
            searchtextbox.KeyDown -= searchtextbox_KeyDown;

            searchtextbox.Enter += searchtextbox_Enter;
            searchtextbox.Leave += searchtextbox_Leave;
            searchtextbox.TextChanged += searchtextbox_TextChanged_1;
            searchtextbox.KeyDown += searchtextbox_KeyDown;

            containersearch.Paint -= containersearch_Paint;
            containersearch.Paint += containersearch_Paint;
        }

        private void containersearch_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            float offset = borderSize / 2f;
            RectangleF rect = new RectangleF(
                offset,
                offset,
                containersearch.ClientSize.Width - borderSize,
                containersearch.ClientSize.Height - borderSize
            );

            using (var path = CreateRoundedRectPath(rect, borderRadius))
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

        private System.Drawing.Drawing2D.GraphicsPath CreateRoundedRectPath(RectangleF rect, float radius)
        {
            float effectiveRadius = Math.Min(radius, Math.Min(rect.Width / 2f, rect.Height / 2f));
            float diameter = effectiveRadius * 2f;

            var path = new System.Drawing.Drawing2D.GraphicsPath();
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
                lastSearchKeyword = "";
                // Sau này có binding thì reset lại
                //ReloadForSearchResult(null);
            }
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
                    dgvDanhsachketqua.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                searchtextbox.Clear();
                lastSearchKeyword = "";
                ReloadForSearchResult(null);
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
            string keyword = searchtextbox.Text?.Trim().ToLower() ?? "";
            if (string.IsNullOrEmpty(keyword))
            {
                ReloadForSearchResult(null);
                return;
            }

            // Lấy lại nguồn (đang ở hàng của grid, không binding list sẵn)
            var results = new List<DTO_KetQuaHeader>();
            foreach (DataGridViewRow r in dgvDanhsachketqua.Rows)
            {
                if (r.IsNewRow) continue;

                var item = new DTO_KetQuaHeader
                {
                    MaKQ = r.Tag?.ToString(),
                    MaDot = r.Cells["MaDot"].Value?.ToString(),
                    DotQuanTrac = r.Cells["DotQuanTrac"].Value?.ToString(),
                    TenNhanVien = r.Cells["TenNhanVien"].Value?.ToString(),
                    GhiChu = r.Cells["GhiChu"].Value?.ToString(),
                };

                // lọc theo vài trường chính
                bool hit =
                    (item.MaDot ?? "").ToLower().Contains(keyword) ||
                    (item.DotQuanTrac ?? "").ToLower().Contains(keyword) ||
                    (item.TenNhanVien ?? "").ToLower().Contains(keyword) ||
                    (item.GhiChu ?? "").ToLower().Contains(keyword);

                if (hit) results.Add(item);
            }

            ReloadForSearchResult(results);
        }

        private void ReloadForSearchResult(List<DTO_KetQuaHeader> filtered)
        {
            // xóa rồi vẽ lại
            dgvDanhsachketqua.Rows.Clear();
            List<DTO_KetQuaHeader> list = filtered ?? ketQuaBLL.LayDanhSachKetQuaMoi();

            int stt = 0;
            foreach (var item in list)
            {
                stt++;
                int rowIndex = dgvDanhsachketqua.Rows.Add();
                var row = dgvDanhsachketqua.Rows[rowIndex];

                row.Cells["STT"].Value = stt;
                row.Cells["MaDot"].Value = item.MaDot ?? "";
                row.Cells["DotQuanTrac"].Value = item.DotQuanTrac ?? "";
                row.Cells["NgayTao"].Value = item.NgayTao;
                row.Cells["NgayTraKQ"].Value = item.NgayTraKQ;
                row.Cells["TenNhanVien"].Value = item.TenNhanVien ?? "";
                row.Cells["SoNenMau"].Value = item.SoNenMau;
                row.Cells["TrangThai"].Value = item.TrangThai;
                row.Cells["GhiChu"].Value = item.GhiChu ?? "";
                row.Tag = item.MaKQ;
            }

            FormatDataGridView();
        }
        #endregion

        #region Buttons
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

            // Nếu bạn có form tạo mới kết quả → mở tại đây
            // Ví dụ: var frm = new ThemKetQuaForm();
            // currentOpenForm = frm;
            // CenterFormOnParent(frm);
            // frm.FormClosed += (s, ev) => currentOpenForm = null;
            // if (frm.ShowDialog() == DialogResult.OK) LoadDanhSachKetQua();

            MessageBox.Show("Chức năng thêm mới kết quả: vui lòng gắn form phù hợp.",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadDanhSachKetQua();
            }
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadDanhSachKetQua();
        }
        #endregion

        #region Helpers
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
        #endregion
    }
}