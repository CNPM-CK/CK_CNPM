using BLL;
using BLL.Speech;
using DTO;
using GUI.Common;
using GUI.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class DanhSachDotNhapLieuUC : UserControl
    {
        private VoiceRecorder _recorder;
        private WhisperService _whisper;
        private string _wavPath;
        private bool _ready = false;
        private readonly bool _isPhongTN_HT = SessionStore.Current.MaPhong == "P003" || SessionStore.Current.MaPhong == "P004";
        private string maDotHienTai = null;
        #region Fields
        // Search box styling
        private Color borderColor = Color.Black;
        private int borderRadius = 12;
        private int borderSize = 2;
        private const int SEARCH_HEIGHT = 50;
        private const string PLACEHOLDER_TEXT = "Tìm kiếm đợt quan trắc...";

        // Layout constants
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;

        // Data & State
        private BindingList<DanhSachDotNhapLieuDTO> dsDotQuanTrac;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";
        private Form currentOpenForm = null;

        // Cell action rectangles
        private Rectangle editRect;
        #endregion

        #region Constructor
        public DanhSachDotNhapLieuUC()
        {
            InitializeComponent();
            this.Load += DanhSachThongSo_Load;
            this.Resize += DanhSachThongSo_Resize;
        }
        #endregion

        #region Initialization
        private async void DanhSachThongSo_Load(object sender, EventArgs e)
        {
            try
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
                //LoadData();
                InitializeDataGridView();
                InitializeCustomSearchBox();
                InitializeWatermark();
                CalculateLayout();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadData()
        {
            totalRecords = 0;
            LoadKeHoachPage();
        }

        private void InitializeDataGridView()
        {

            dgvDsdotquantrac.AutoGenerateColumns = false;
            dgvDsdotquantrac.Columns.Clear();
            dgvDsdotquantrac.AllowUserToAddRows = false;
            dgvDsdotquantrac.ReadOnly = true;
            dgvDsdotquantrac.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvDsdotquantrac.MultiSelect = false;
            dgvDsdotquantrac.RowTemplate.Height = 50;
            dgvDsdotquantrac.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvDsdotquantrac.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;


            // Font settings
            dgvDsdotquantrac.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            dgvDsdotquantrac.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvDsdotquantrac.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgvDsdotquantrac.RowHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9.75F);

            // Header styling
            dgvDsdotquantrac.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 152, 70);
            dgvDsdotquantrac.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvDsdotquantrac.EnableHeadersVisualStyles = false;

            // Cell styling
            dgvDsdotquantrac.DefaultCellStyle.BackColor = Color.White;
            dgvDsdotquantrac.DefaultCellStyle.ForeColor = Color.Black;
            dgvDsdotquantrac.DefaultCellStyle.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dgvDsdotquantrac.DefaultCellStyle.SelectionForeColor = Color.Black;
            dgvDsdotquantrac.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Define columns
            dgvDsdotquantrac.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "maDot",
                    HeaderText = "MÃ ĐỢT",
                    Name = "maDot",
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "maHD",
                    HeaderText = "MÃ HỢP ĐỒNG",
                    Name = "maHD",
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ngayBatDau",
                    HeaderText = "NGÀY BẮT ĐẦU",
                    Name = "ngayBatDau",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ngayDuKien",
                    HeaderText = "NGÀY DỰ KIẾN",
                    Name = "ngayDuKien",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ngayConLai",
                    HeaderText = "SỐ NGÀY CÒN LẠI",
                    Name = "ngayConLai",
                },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "trangThai",
                    HeaderText = "TRẠNG THÁI",
                    Name = "trangThai",
                }

            });

            if (_isPhongTN_HT)
            {
                DataGridViewImageColumn thaoTacCol = new DataGridViewImageColumn
                {
                    Name = "ThaoTac",
                    HeaderText = "Thao tác",
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                dgvDsdotquantrac.Columns.Add(thaoTacCol);
            }
            dgvDsdotquantrac.ReadOnly = true;

            // Bind data
            dgvDsdotquantrac.DataSource = dsDotQuanTrac;

            dgvDsdotquantrac.CellPainting += DgvDsdotquantrac_CellPainting;
            dgvDsdotquantrac.CellClick += DgvDsdotquantrac_CellClick;
            LoadKeHoachPage();
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
            if (Properties.Resources.greenlogo == null || dgvDsdotquantrac == null) return;

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

                dgvDsdotquantrac.BackgroundImage = bmp;
                dgvDsdotquantrac.BackgroundImageLayout = ImageLayout.Center;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Watermark error: {ex.Message}");
            }
        }
        #endregion

        #region Layout & Resize
        private void DanhSachThongSo_Resize(object sender, EventArgs e)
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

            int btnWidth = isMaximized ? 80 : 66;
            int btnHeight = isMaximized ? 50 : 40;
            int topOffset = 10;

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

            containersearch.Top = topOffset;
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

        #region DataGridView Events - ✅ FIXED GIỐNG DanhSachNhanVien
        private void DgvDsdotquantrac_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (!_isPhongTN_HT) return;
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvDsdotquantrac.Columns["ThaoTac"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                int iconWidth = 24;
                int iconHeight = 24;
                int spacing = 15;
                int totalWidth = (iconWidth) + spacing;

                int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;
                int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

                editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
                if (Properties.Resources.edit != null)
                {
                    e.Graphics.DrawImage(Properties.Resources.edit, editRect);
                }

                e.Handled = true;
            }
        }

        private DateTime _lastClickTime = DateTime.MinValue; // biến toàn cục trong class

        private void DgvDsdotquantrac_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isPhongTN_HT) return;

            // 🧱 Chống double-click nhanh
            if ((DateTime.Now - _lastClickTime).TotalMilliseconds < 250)
                return; // bỏ qua nếu click quá nhanh
            _lastClickTime = DateTime.Now;

            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            // Kiểm tra cột ThaoTac tồn tại
            if (!dgvDsdotquantrac.Columns.Contains("ThaoTac")) return;
            if (e.ColumnIndex != dgvDsdotquantrac.Columns["ThaoTac"].Index) return;

            // Kiểm tra row hợp lệ
            if (e.RowIndex >= dgvDsdotquantrac.Rows.Count) return;

            DataGridViewRow row = dgvDsdotquantrac.Rows[e.RowIndex];

            // Kiểm tra cell maDot tồn tại và có giá trị
            if (!dgvDsdotquantrac.Columns.Contains("maDot") || row.Cells["maDot"].Value == null)
                return;

            // Lấy điểm click trong tọa độ của DataGridView
            var clickPoint = dgvDsdotquantrac.PointToClient(Cursor.Position);

            // Kiểm tra có click đúng vào icon edit không
            if (editRect.Contains(clickPoint))
                HandleEdit(row);
        }
        #endregion

        #region CRUD Operations
        private void HandleEdit(DataGridViewRow row)
        {
            Debug.WriteLine($"CellClick fired: {DateTime.Now:HH:mm:ss.fff}");

            if (currentOpenForm != null && !currentOpenForm.IsDisposed)
            {
                currentOpenForm.BringToFront();
                currentOpenForm.Focus();
                MessageBox.Show("Vui lòng hoàn thành thao tác hiện tại trước khi thực hiện thao tác mới!",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string maDot = row.Cells["maDot"].Value?.ToString();
            DTO_DotQuanTrac dot = new DTO_DotQuanTrac
            {
                MaDot = maDot,
            };

            DanhSachThongSoNhapLieuForm frmNhapLieu = new DanhSachThongSoNhapLieuForm(dot);
            currentOpenForm = frmNhapLieu;
            CenterFormOnParent(frmNhapLieu);
            frmNhapLieu.FormClosed += (s, ev) => { currentOpenForm = null; };
            //frmNhapLieu.SuccesfullyUpdated += (s, ev) => RefreshData();
            frmNhapLieu.Show(this.FindForm());

        }

        //private void HandleDelete(DataGridViewRow row)
        //{
        //    string maDot = row.Cells["maDot"].Value?.ToString();
        //    string noiDung = row.Cells["noiDung"].Value?.ToString();

        //    DialogResult result = MessageBox.Show(
        //        $"Bạn có chắc chắn muốn xóa đợt quan trắc '{noiDung}' (Mã: {maDot}) không?",
        //        "Xác nhận xóa",
        //        MessageBoxButtons.YesNo,
        //        MessageBoxIcon.Question
        //    );

        //    if (result == DialogResult.Yes)
        //    {
        //        try
        //        {
        //            BLL_DotQuanTrac bll = new BLL_DotQuanTrac();
        //            bll.xoaDotQuanTrac(maDot);

        //            MessageBox.Show("Đã xóa đợt quan trắc thành công!", "Thông báo",
        //                MessageBoxButtons.OK, MessageBoxIcon.Information);

        //            //RefreshData();
        //            LoadData();
        //        }
        //        catch (Exception ex)
        //        {
        //            MessageBox.Show($"Có lỗi xảy ra khi xóa đợt quan trắc: {ex.Message}",
        //                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        }
        //    }
        //}

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
                dgvDsdotquantrac.DataSource = dsDotQuanTrac;
                lastSearchKeyword = "";
            }
        }

        private void Searchtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (dgvDsdotquantrac.Rows.Count > 0)
                {
                    dgvDsdotquantrac.ClearSelection();
                    dgvDsdotquantrac.Rows[0].Selected = true;
                    dgvDsdotquantrac.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                searchtextbox.Clear();
                dgvDsdotquantrac.DataSource = dsDotQuanTrac;
                lastSearchKeyword = "";
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
            //string keyword = searchtextbox.Text.Trim().ToLower();

            //if (string.IsNullOrEmpty(keyword))
            //{
            //    dgvDsdotquantrac.DataSource = dsDotQuanTrac;
            //    return;
            //}

            //var filtered = dsDotQuanTrac
            //    .Where(ts =>
            //        (ts.MaDot ?? "").ToLower().Contains(keyword) ||
            //        (ts.MaHD ?? "").ToLower().Contains(keyword) ||
            //        (ts.NoiDung ?? "").ToLower().Contains(keyword) ||
            //        //(ts.TrangThai ?? "").ToLower().Contains(keyword) ||
            //        (ts.DotQuanTrac ?? "").ToLower().Contains(keyword)
            //    )
            //    .ToList();

            //dgvDsdotquantrac.DataSource = new BindingList<DTO_DotQuanTrac>(filtered);
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

        #region Helper Methods
        private void CenterFormOnParent(Form childForm)
        {
            Form parentForm = this.FindForm();
            if (parentForm != null)
            {
                childForm.Width = (int)(childForm.Width * 1.5);
                childForm.StartPosition = FormStartPosition.Manual;
                childForm.Location = new Point(
                    parentForm.Location.X + (parentForm.Width - childForm.Width) / 2,
                    parentForm.Location.Y + (parentForm.Height - childForm.Height) / 2
                );
            }
        }


        private void dgvDSKH_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        #endregion
        int currentPage = 1;
        int pageSize = 15;
        int totalRecords = 0;
        int totalPages = 0;

        private void LoadKeHoachPage()
        {
            var bll = new DotQuanTracNhapLieuBLL();

            if (totalRecords == 0)
            {
                totalRecords = bll.demTongKHQT();
                totalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
            }

            string maPhong = SessionStore.Current?.MaPhong;
            var data = new List<DanhSachDotNhapLieuDTO>();
            if (string.IsNullOrEmpty(maPhong))
            {
                if (SessionStore.Current?.VaiTro == 1)
                {
                    data = bll.layDanhSachDotQuanTracNhapLieu_PhanTrang(currentPage, pageSize, "");

                }
                else
                {
                    MessageBox.Show("Không tìm thấy mã phòng trong phiên đăng nhập!",
                        "Lỗi session", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                data = bll.layDanhSachDotQuanTracNhapLieu_PhanTrang(currentPage, pageSize, maPhong);
            }

            // Bind data an toàn
            dgvDsdotquantrac.AutoGenerateColumns = false;
            dgvDsdotquantrac.DataSource = null;
            dgvDsdotquantrac.DataSource = data;

            soTrang.Text = $"Trang {currentPage}/{totalPages}";
            btnTruoc.Enabled = currentPage > 1;
            btnSau.Enabled = currentPage < totalPages;
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            if (currentPage > 1)
            {
                currentPage--;
                LoadKeHoachPage();
            }
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            currentPage++;
            LoadKeHoachPage();
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
    }
}