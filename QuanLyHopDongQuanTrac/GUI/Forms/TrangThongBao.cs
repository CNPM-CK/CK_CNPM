using BLL;
using DTO;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;
using GUI.Common;


namespace GUI.Forms
{
    public partial class TrangThongBao : UserControl
    {
        private readonly ThongBaoBLL bllThongBao = new ThongBaoBLL();

        #region Fields
        // Search box styling
        private Color borderColor = Color.Black;
        private int borderRadius = 12;
        private int borderSize = 2;
        private const int SEARCH_HEIGHT = 50;
        private const string PLACEHOLDER_TEXT = "Tìm kiếm thông báo...";

        // Layout constants
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;

        // Data & State
        private DataTable dtThongBao;
        private DataTable dtThongBaoFull; // ✅ Lưu toàn bộ dữ liệu để search
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";

        // ✅ Phân trang
        private int currentPage = 1;
        private int pageSize = 10;
        private int totalRecords = 0;
        private int totalPages = 0;
        private string maNV = "";
        #endregion

        #region Constructor
        public TrangThongBao()
        {
            InitializeComponent();
            this.Load += TrangThongBao_Load;
            this.Resize += TrangThongBao_Resize;
        }
        #endregion

        #region Initialization
        private void TrangThongBao_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeDataGridView();
                InitializeCustomSearchBox();
                InitializeWatermark();
                CalculateLayout();
                BoGocButton(btnSau, 20); 
                BoGocButton(btnTruoc, 20);

                // ✅ Lấy mã nhân viên
                string email = SessionStore.Current.UserName;
                if (string.IsNullOrEmpty(email))
                {
                    MessageBox.Show("Không tìm thấy thông tin đăng nhập!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                NhanVienBLL nvBLL = new NhanVienBLL();
                NhanVien nv = nvBLL.layThongTinCaNhan(email);
                if (nv == null)
                {
                    MessageBox.Show("Không lấy được thông tin nhân viên.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                maNV = nv.maNV;

                // ✅ Tải dữ liệu trang đầu tiên
                taiThongBaoQuaHan();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo trang thông báo: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region Fields thao tác
        private Rectangle readRect;
        private Rectangle deleteRect;
        #endregion

        private void InitializeDataGridView()
        {
            dgvdsThongbao.AutoGenerateColumns = false;
            dgvdsThongbao.Columns.Clear();
            dgvdsThongbao.AllowUserToAddRows = false;
            dgvdsThongbao.ReadOnly = true;
            dgvdsThongbao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvdsThongbao.MultiSelect = false;
            dgvdsThongbao.RowTemplate.Height = 80;

            dgvdsThongbao.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvdsThongbao.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            dgvdsThongbao.Font = new Font("Segoe UI", 9.75F);
            dgvdsThongbao.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvdsThongbao.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 152, 70);
            dgvdsThongbao.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvdsThongbao.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdsThongbao.EnableHeadersVisualStyles = false;

            dgvdsThongbao.DefaultCellStyle.BackColor = Color.White;
            dgvdsThongbao.DefaultCellStyle.ForeColor = Color.Black;
            dgvdsThongbao.DefaultCellStyle.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dgvdsThongbao.DefaultCellStyle.SelectionForeColor = Color.Black;

            // --- Các cột dữ liệu ---
            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "loaiTB",
                HeaderText = "LOẠI THÔNG BÁO",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "maTB",
                DataPropertyName = "maTB",
                HeaderText = "MÃ TB",
                Visible = false
            });

            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "tieuDe",
                HeaderText = "TIÊU ĐỀ",
                Width = 250
            });

            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "noiDung",
                HeaderText = "NỘI DUNG",
                Width = 500,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.TopLeft,
                    WrapMode = DataGridViewTriState.True,
                    Padding = new Padding(5)
                }
            });

            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ngayTao",
                HeaderText = "NGÀY TẠO",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy",
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "trangThaiDoc",
                HeaderText = "TRẠNG THÁI ĐỌC",
                Visible = false
            });

            // ✅ Cột thao tác
            DataGridViewTextBoxColumn thaoTacCol = new DataGridViewTextBoxColumn
            {
                HeaderText = "THAO TÁC",
                Name = "colThaoTac",
                Width = 100
            };
            dgvdsThongbao.Columns.Add(thaoTacCol);

            // --- Sự kiện ---
            dgvdsThongbao.CellPainting += dgvdsThongbao_CellPainting;
            dgvdsThongbao.CellClick += dgvdsThongbao_CellClick;
        }

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
        private void dgvdsThongbao_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != dgvdsThongbao.Columns["colThaoTac"].Index) return;

            Point clickPoint = dgvdsThongbao.PointToClient(Cursor.Position);
            DataGridViewRow row = dgvdsThongbao.Rows[e.RowIndex];
            string maTB = row.Cells["maTB"].Value?.ToString();
            if (string.IsNullOrEmpty(maTB)) return;

            if (readRect.Contains(clickPoint))
            {
                try
                {
                    // 🟢 1. Đánh dấu đã đọc trong DATABASE
                    bllThongBao.danhDauThongBaoDaDoc(maTB, maNV);

                    // ✅ 2. CẬP NHẬT NGAY TRONG DATATABLE (dtThongBao - trang hiện tại)
                    DataRowView drv = row.DataBoundItem as DataRowView;
                    if (drv != null)
                    {
                        drv["trangThaiDoc"] = true;
                    }

                    // ✅ 3. CẬP NHẬT TRONG dtThongBaoFull (để search không bị sai)
                    if (dtThongBaoFull != null)
                    {
                        DataRow[] rows = dtThongBaoFull.Select($"maTB = '{maTB}'");
                        foreach (DataRow dr in rows)
                        {
                            dr["trangThaiDoc"] = true;
                        }
                    }

                    // ✅ 4. TÔ MÀU NGAY DÒNG ĐÓ (không cần reload)
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 250, 200);

                    // ✅ 5. Refresh badge thông báo
                    lamMoiChuongThongBao();

                    // ✅ 6. Refresh DataGridView để hiển thị màu
                    dgvdsThongbao.Refresh();

                    MessageBox.Show("Đã đánh dấu thông báo là đã đọc!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi đánh dấu đã đọc: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (deleteRect.Contains(clickPoint))
            {
                DialogResult result = MessageBox.Show("Bạn có chắc muốn xóa thông báo này không?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        bllThongBao.xoaThongBaoNguoiDung(maTB, maNV);

                        // ✅ Xóa cần reload lại vì số lượng thay đổi
                        taiThongBaoQuaHan();
                        lamMoiChuongThongBao();

                        MessageBox.Show("Đã xóa thông báo thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi xóa thông báo: {ex.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void lamMoiChuongThongBao()
        {
            try
            {
                // Tìm form TrangChu và gọi refresh badge
                Form parentForm = this.FindForm();
                if (parentForm is TrangChu trangChu)
                {
                    trangChu.RefreshBadgeThongBao();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi refresh badge: {ex.Message}");
            }
        }
        private void dgvdsThongbao_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != dgvdsThongbao.Columns["colThaoTac"].Index) return;

            e.PaintBackground(e.CellBounds, true);

            int iconWidth = 24, iconHeight = 24, spacing = 10;
            int totalWidth = (iconWidth * 2) + spacing;
            int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;
            int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

            readRect = new Rectangle(startX, startY, iconWidth, iconHeight);
            deleteRect = new Rectangle(startX + iconWidth + spacing, startY, iconWidth, iconHeight);

            // Vẽ icon "đã đọc"
            if (Properties.Resources.seen != null)
                e.Graphics.DrawImage(Properties.Resources.seen, readRect);

            // Vẽ icon "xóa"
            if (Properties.Resources.trash_can != null)
                e.Graphics.DrawImage(Properties.Resources.trash_can, deleteRect);

            e.Handled = true;
        }

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

            containersearch.Controls.Add(searchtextbox);

            // Events
            searchtextbox.Enter += Searchtextbox_Enter;
            searchtextbox.Leave += Searchtextbox_Leave;
            searchtextbox.TextChanged += Searchtextbox_TextChanged;
            searchtextbox.KeyDown += Searchtextbox_KeyDown;
            containersearch.Paint += Containersearch_Paint;
        }

        private void InitializeWatermark()
        {
            if (Properties.Resources.greenlogo == null || dgvdsThongbao == null) return;

            try
            {
                Image watermark = Properties.Resources.greenlogo;
                Bitmap bmp = new Bitmap(watermark.Width, watermark.Height);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    ColorMatrix matrix = new ColorMatrix();
                    matrix.Matrix33 = 0.15f;
                    ImageAttributes attributes = new ImageAttributes();
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    g.DrawImage(watermark,
                        new Rectangle(0, 0, watermark.Width, watermark.Height),
                        0, 0, watermark.Width, watermark.Height,
                        GraphicsUnit.Pixel,
                        attributes);
                }

                dgvdsThongbao.BackgroundImage = bmp;
                dgvdsThongbao.BackgroundImageLayout = ImageLayout.Center;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Watermark error: {ex.Message}");
            }
        }
        #endregion

        #region Layout & Resize
        private void TrangThongBao_Resize(object sender, EventArgs e)
        {
            if (this.Width < 100) return;
            CalculateLayout();
        }

        private void CalculateLayout()
        {
            if (containersearch == null) return;

            int formWidth = this.Width;

            Form parentForm = this.FindForm();
            bool isMaximized = parentForm != null && parentForm.WindowState == FormWindowState.Maximized;

            int leftBoundary = pictureFilter != null ? pictureFilter.Right + SPACING : MARGIN;
            int rightBoundary = formWidth - MARGIN;

            if (picturemicro != null)
            {
                rightBoundary -= picturemicro.Width + SPACING;
            }

            int availableWidth = rightBoundary - leftBoundary;

            int searchWidth = Math.Max(MIN_SEARCH_WIDTH, Math.Min(availableWidth, MAX_SEARCH_WIDTH));
            if (searchWidth < MIN_SEARCH_WIDTH)
            {
                searchWidth = Math.Max(150, availableWidth);
            }

            if (pictureFilter != null)
            {
                pictureFilter.Left = MARGIN;
            }

            containersearch.Left = leftBoundary;
            containersearch.Width = searchWidth;
            containersearch.Height = SEARCH_HEIGHT;

            searchtextbox.Width = searchWidth - (borderSize * 2 + 10);
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);

            if (picturemicro != null)
            {
                picturemicro.Left = containersearch.Right + SPACING;
            }

            containersearch.Invalidate();
        }
        #endregion

        #region Search Functionality
        private void Searchtextbox_Enter(object sender, EventArgs e)
        {
            if (isPlaceholder)
            {
                isPlaceholder = false;
                searchtextbox.Text = "";
                searchtextbox.ForeColor = Color.FromArgb(64, 64, 64);
            }
        }

        private void Searchtextbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchtextbox.Text))
            {
                isPlaceholder = true;
                searchtextbox.Text = PLACEHOLDER_TEXT;
                searchtextbox.ForeColor = Color.Silver;
                lastSearchKeyword = "";

                // ✅ Quay về chế độ phân trang bình thường
                currentPage = 1;
                taiThongBaoQuaHan();
            }
        }

        private void Searchtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (dgvdsThongbao.Rows.Count > 0)
                {
                    dgvdsThongbao.ClearSelection();
                    dgvdsThongbao.Rows[0].Selected = true;
                    dgvdsThongbao.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                searchtextbox.Clear();
                lastSearchKeyword = "";
                currentPage = 1;
                taiThongBaoQuaHan();
            }
        }

        private void Searchtextbox_TextChanged(object sender, EventArgs e)
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
                // ✅ Trở về phân trang bình thường
                currentPage = 1;
                taiThongBaoQuaHan();
                return;
            }

            if (dtThongBaoFull == null || dtThongBaoFull.Rows.Count == 0)
            {
                return;
            }

            try
            {
                DataView dv = new DataView(dtThongBaoFull);
                dv.RowFilter = string.Format(
                    "loaiTB LIKE '%{0}%' OR " +
                    "tenKhachHang LIKE '%{0}%' OR " +
                    "tieuDe LIKE '%{0}%' OR " +
                    "noiDung LIKE '%{0}%'",
                    keyword.Replace("'", "''"));

                dgvdsThongbao.DataSource = dv;
                ToMauTheoTrangThai();
                // ✅ Ẩn phân trang khi đang search
                CapNhatHienThiPhanTrang(0, 0, 0);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Search error: {ex.Message}");
                dgvdsThongbao.DataSource = dtThongBao;
            }
        }
        #endregion

        #region Custom Paint
        private void Containersearch_Paint(object sender, PaintEventArgs e)
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

        #region Data Loading & Pagination
        private void taiThongBaoQuaHan()
        {
            try
            {
                if (string.IsNullOrEmpty(maNV))
                {
                    MessageBox.Show("Không tìm thấy mã nhân viên!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ 1. Đếm tổng số thông báo
                totalRecords = bllThongBao.demTongSoThongBao(maNV);

                if (totalRecords == 0)
                {
                    dgvdsThongbao.DataSource = null;
                    CapNhatHienThiPhanTrang(0, 0, 0);
                    MessageBox.Show("Không có thông báo nào.", "Thông tin",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // ✅ 2. Tính tổng số trang
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);

                // ✅ 3. Kiểm tra currentPage hợp lệ
                if (currentPage > totalPages) currentPage = totalPages;
                if (currentPage < 1) currentPage = 1;

                // ✅ 4. Lấy dữ liệu trang hiện tại
                dtThongBao = bllThongBao.layThongBaoTheoNhanVien_PhanTrang(maNV, currentPage, pageSize);

                // ✅ 5. Lưu toàn bộ dữ liệu để search (chỉ load 1 lần)
                if (dtThongBaoFull == null || dtThongBaoFull.Rows.Count == 0)
                {
                    dtThongBaoFull = bllThongBao.layThongBaoTheoNhanVien(maNV);
                }

                // ✅ 6. Kiểm tra cột trangThaiDoc
                if (!dtThongBao.Columns.Contains("trangThaiDoc"))
                {
                    MessageBox.Show("Lỗi: DataTable không có cột 'trangThaiDoc'!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ 7. Bind dữ liệu
                dgvdsThongbao.DataSource = dtThongBao;

                ToMauTheoTrangThai();

                // ✅ 9. Cập nhật hiển thị phân trang
                CapNhatHienThiPhanTrang(currentPage, totalPages, totalRecords);

                dgvdsThongbao.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông báo: {ex.Message}\n\nStackTrace:\n{ex.StackTrace}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ToMauTheoTrangThai()
        {
            foreach (DataGridViewRow row in dgvdsThongbao.Rows)
            {
                try
                {
                    var cellValue = row.Cells["trangThaiDoc"].Value;
                    bool daDoc = cellValue != null &&
                                 cellValue != DBNull.Value &&
                                 Convert.ToBoolean(cellValue);

                    // ✅ ĐÃ ĐỌC = VÀNG, CHƯA ĐỌC = TRẮNG
                    row.DefaultCellStyle.BackColor = daDoc
                        ? Color.FromArgb(255, 250, 200)  // Vàng nhạt - ĐÃ đọc
                        : Color.White;                    // Trắng - CHƯA đọc
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi tô màu dòng {row.Index}: {ex.Message}");
                }
            }
        }

        private void CapNhatHienThiPhanTrang(int page, int total, int records)
        {
            if (soTrang != null)
            {
                if (total > 0)
                {
                    soTrang.Text = $"Trang {page}/{total}";
                }
                else
                {
                    soTrang.Text = "Không có dữ liệu";
                }
            }

            // ✅ Enable/Disable nút phân trang
            if (btnTruoc != null)
            {
                btnTruoc.Enabled = (page > 1);
            }

            if (btnSau != null)
            {
                btnSau.Enabled = (page < total);
            }
        }
        #endregion

        #region Pagination Buttons
        private void btnTruoc_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                taiThongBaoQuaHan();
            }
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            if (currentPage < totalPages)
            {
                currentPage++;
                taiThongBaoQuaHan();
            }
        }

        private void soTrang_Click(object sender, EventArgs e)
        {
            // Có thể thêm chức năng nhảy tới trang cụ thể
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            // Reset về trang đầu
            searchtextbox.Clear();
            currentPage = 1;
            dtThongBaoFull = null; // Force reload toàn bộ dữ liệu
            taiThongBaoQuaHan();
        }

        private void dgvdsThongbao_Paint(object sender, PaintEventArgs e)
        {
            if (Properties.Resources.greenlogo == null) return;

            int dgvWidth = dgvdsThongbao.Width;
            int dgvHeight = dgvdsThongbao.Height;
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
        #endregion
    }
}