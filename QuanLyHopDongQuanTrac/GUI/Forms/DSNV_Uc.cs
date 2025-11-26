
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
using BLL.Speech;
using GUI.Helper;
using System.Diagnostics;


namespace GUI.Forms
{
    public partial class DSNV_Uc : UserControl
    {
        //giong noi
        private VoiceRecorder _recorder;
        private WhisperService _whisper;
        private string _wavPath;
        private bool _ready = false;

        private readonly bool _isAdmin = (SessionStore.Current?.VaiTro ?? 0) == 1;

        private bool HasThaoTacColumn() =>
            dgvDanhsachnhanvien.Columns.Contains("ThaoTac");

        private int ThaoTacIndex() =>
            HasThaoTacColumn() ? dgvDanhsachnhanvien.Columns["ThaoTac"].Index : -1;
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
        private string tuKhoacuoicung = "";
        private Form currentOpenForm = null;



        public DSNV_Uc()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            if (LicenseManager.UsageMode != LicenseUsageMode.Designtime)
            {
                this.Load += DSNV_Uc_Load;
                picturemicro.Click += BtnMic_Click;
            }
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


            // Thêm các cột
            dgvDanhsachnhanvien.Columns.AddRange(new DataGridViewColumn[]
            {
                new DataGridViewTextBoxColumn { DataPropertyName = "maNV", HeaderText = "Mã nhân viên", Name = "maNV" },
                new DataGridViewTextBoxColumn { DataPropertyName = "hoTen", HeaderText = "Họ Tên", Name = "hoTen" },
                new DataGridViewTextBoxColumn { DataPropertyName = "email", HeaderText = "Email", Name = "email" },
                new DataGridViewTextBoxColumn { DataPropertyName = "maPhong", HeaderText = "Mã Phòng", Name = "maPhong" },
                new DataGridViewTextBoxColumn
                {
                    DataPropertyName = "ngaySinh",
                    HeaderText = "Ngày Sinh",
                    Name = "ngaySinh",
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
                },
                new DataGridViewTextBoxColumn { DataPropertyName = "gioiTinh", HeaderText = "Giới Tính", Name = "gioiTinh" },
                new DataGridViewTextBoxColumn { DataPropertyName = "diaChi", HeaderText = "Địa Chỉ", Name = "diaChi" },
                new DataGridViewTextBoxColumn { DataPropertyName = "soDienThoai", HeaderText = "Số Điện Thoại", Name = "soDienThoai" },
                new DataGridViewTextBoxColumn { DataPropertyName = "tenPhong", HeaderText = "Phòng Ban", Name = "tenPhong" },
                new DataGridViewTextBoxColumn { DataPropertyName = "tenTrangThai", HeaderText = "Trạng Thái", Name = "tenTrangThai" }

            });
            if (_isAdmin)
            {
                var thaoTacCol = new DataGridViewImageColumn
                {
                    Name = "ThaoTac",
                    HeaderText = "Thao tác",
                    ImageLayout = DataGridViewImageCellLayout.Zoom
                };
                dgvDanhsachnhanvien.Columns.Add(thaoTacCol);
            }

            dgvDanhsachnhanvien.CellFormatting += dgvDanhsachnhanvien_CellFormatting;

            if (_isAdmin && HasThaoTacColumn())
            {
                dgvDanhsachnhanvien.CellPainting += dgvDanhsachnhanvien_CellPainting;
                dgvDanhsachnhanvien.CellClick += dgvDanhsachnhanvien_CellClick;
                dgvDanhsachnhanvien.Paint += dgvDanhsachnhanvien_Paint;
                dgvDanhsachnhanvien.Columns["ThaoTac"].ReadOnly = false;
            }
        }

        private void taiDanhSachNhanVien()
        {
            tongSoBanGhi = 0;
            taiTrangkhachhang();
        }

        private Rectangle editRect;
        private Rectangle deleteRect;

        private void dgvDanhsachnhanvien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (!_isAdmin || !HasThaoTacColumn()) return;
            if (e.RowIndex < 0) return;
            if (e.ColumnIndex != ThaoTacIndex()) return;

            e.PaintBackground(e.ClipBounds, true);

            int iconWidth = 24, iconHeight = 24, spacing = 10;
            int totalWidth = (iconWidth * 2) + spacing;
            int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;
            int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

            editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
            deleteRect = new Rectangle(startX + iconWidth + spacing, startY, iconWidth, iconHeight);

            if (Properties.Resources.edit != null)
                e.Graphics.DrawImage(Properties.Resources.edit, editRect);
            if (Properties.Resources.trash_can != null)
                e.Graphics.DrawImage(Properties.Resources.trash_can, deleteRect);

            e.Handled = true;
        }

        private void dgvDanhsachnhanvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (!_isAdmin || !HasThaoTacColumn()) return;
            if (e.RowIndex < 0 || e.RowIndex >= dgvDanhsachnhanvien.Rows.Count) return;
            if (e.ColumnIndex != ThaoTacIndex()) return;

            var clickPoint = dgvDanhsachnhanvien.PointToClient(Cursor.Position);
            DataGridViewRow row = dgvDanhsachnhanvien.Rows[e.RowIndex];

            if (row?.Cells["maNV"]?.Value == null) return;

            if (editRect.Contains(clickPoint)) HandleEdit(row);
            else if (deleteRect.Contains(clickPoint)) HandleDelete(row);
        }




        private void HandleEdit(DataGridViewRow row)
        {
            if (currentOpenForm != null && !currentOpenForm.IsDisposed)
            {
                currentOpenForm.BringToFront();
                currentOpenForm.Focus();
                return;
            }

            // Lấy trực tiếp từ DataBoundItem
            NhanVien nvSource = row.DataBoundItem as NhanVien;

            if (nvSource == null)
            {
                MessageBox.Show("Không thể lấy thông tin nhân viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Sử dụng trực tiếp nvSource, KHÔNG TẠO MỚI
            ThemNhanVien frmSua = new ThemNhanVien();
            frmSua.isEditMode = true;
            frmSua.NhanVienHienTai = nvSource;  // Truyền trực tiếp nvSource

            currentOpenForm = frmSua;
            CenterFormOnParent(frmSua);

            frmSua.FormClosed += (s, ev) =>
            {
                currentOpenForm = null;
            };

            frmSua.SuccesfullyUpdated += (s, ev) => taiDanhSachNhanVien();
            if (frmSua.ShowDialog(this) == DialogResult.OK)
            {
                taiDanhSachNhanVien(); // ✅ Refresh lại sau khi đóng form
            }
        }

        private void HandleDelete(DataGridViewRow row)
        {
            string maNV = row.Cells["maNV"].Value?.ToString();
            string hoTen = row.Cells["hoTen"].Value?.ToString();
            string tenTrangThai = row.Cells["tenTrangThai"].Value?.ToString();

            if (string.IsNullOrEmpty(maNV))
                return;

            // ✅ Lấy trạng thái nhân viên
            NhanVien nvSource = row.DataBoundItem as NhanVien;
            int trangThai = nvSource?.trangThai ?? 0;

            // ✅ Kiểm tra: Chỉ cho xóa nếu trạng thái = 6
            if (trangThai != 6)
            {
                MessageBox.Show(
                    $"Không thể xóa nhân viên '{hoTen}'!\n\n" +
                    $"Trạng thái hiện tại: {tenTrangThai}\n" +
                    $"Chỉ được xóa nhân viên có trạng thái 'Ngưng hoạt động'.",
                    "Không thể xóa",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // ✅ Nếu trạng thái = 6 → Cho phép xóa mềm
            DialogResult result = MessageBox.Show(
                $"Bạn có chắc chắn muốn xóa nhân viên '{hoTen}' (Mã: {maNV}) không?\n\n" +
                $"Nhân viên này sẽ bị ẩn khỏi danh sách.",
                "Xác nhận xóa",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );

            if (result == DialogResult.Yes)
            {
                try
                {
                    NhanVienBLL nvBLL = new NhanVienBLL();
                    nvBLL.xoaNhanVien(maNV);

                    MessageBox.Show("Đã xóa nhân viên thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ Delay refresh để tránh conflict
                    this.BeginInvoke(new Action(() =>
                    {
                        taiDanhSachNhanVien();
                    }));
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Có lỗi xảy ra khi xóa nhân viên: " + ex.Message,
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void CenterFormOnParent(Form childForm)
        {
            childForm.StartPosition = FormStartPosition.Manual;
            childForm.Location = new Point(
                this.Location.X + (this.Width - childForm.Width) / 2,
                this.Location.Y + (this.Height - childForm.Height) / 2
            );
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

            // Đăng ký events
            searchtextbox.Enter += searchtextbox_Enter;
            searchtextbox.Leave += searchtextbox_Leave;
            searchtextbox.TextChanged += searchtextbox_TextChanged_1;
            searchtextbox.KeyDown += searchtextbox_KeyDown;
            containersearch.Paint += containersearch_Paint;
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

            btnXuatfile.Click += (s, ev) =>
            {
                menu.Show(btnXuatfile, new Point(0, btnXuatfile.Height));
            };
        }



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
            btnThemuser.Visible = _isAdmin;
            btnXuatfile.Visible = _isAdmin;
            btnThemuser.Size = new Size(66, 40);
            btnXuatfile.Size = new Size(66, 40);

            BoGocButton(btnThemuser, 20);
            BoGocButton(btnXuatfile, 20);
            BoGocButton(btnTruoc, 20);
            BoGocButton(btnSau, 20);

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

        #region TextBox Events - FIX CHÍNH Ở ĐÂY
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
                tuKhoacuoicung = "";
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

                // ✅ Reset về phân trang
                trangHientai = 1;
                tongSoBanGhi = 0;
                taiTrangkhachhang();

                tuKhoacuoicung = "";
            }
        }


        private void searchtextbox_TextChanged_1(object sender, EventArgs e)
        {
            if (isPlaceholder)
                return;
            string currentKeyword = searchtextbox.Text.Trim().ToLower();
            if (currentKeyword == tuKhoacuoicung)
                return;
            tuKhoacuoicung = currentKeyword;
            PerformSearch();
        }


        private void PerformSearch()
        {
            string keyword = searchtextbox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                // Reset về phân trang
                trangHientai = 1;
                tongSoBanGhi = 0;
                taiTrangkhachhang();
                return;
            }

            // ✅ TÌM KIẾM DÙNG DTO MỚI
            try
            {
                NhanVienBLL bll = new NhanVienBLL();
                var allData = bll.layDanhSachNhanVien_TimKiem(); // ✅ Dùng method mới

                var filtered = allData
                    .Where(nv =>
                        (nv.hoTen ?? "").ToLower().Contains(keyword) ||
                        (nv.email ?? "").ToLower().Contains(keyword) ||
                        (nv.tenPhong ?? "").ToLower().Contains(keyword) ||
                        (nv.soDienThoai ?? "").Contains(keyword) ||
                        (nv.diaChi ?? "").ToLower().Contains(keyword) ||
                        nv.maNV.ToString().ToLower().Contains(keyword)
                    )
                    .ToList();

                // ✅ Bind vào DataGridView với DTO mới
                dgvDanhsachnhanvien.DataSource = new BindingList<NhanVienSearch>(filtered);

                // Hiển thị số kết quả
                soTrang.Text = $"Tìm thấy {filtered.Count} kết quả";

                // Disable nút phân trang khi search
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

        private void dgvDanhsachnhanvien_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvDanhsachnhanvien.Columns[e.ColumnIndex].Name == "gioiTinh" && e.Value != null)
            {
                string gioiTinh = e.Value.ToString().Trim();

                if (gioiTinh == "0" || gioiTinh.ToLower() == "false")
                    e.Value = "Nam";
                else if (gioiTinh == "1" || gioiTinh.ToLower() == "true")
                    e.Value = "Nữ";

                e.FormattingApplied = true;
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
        #endregion




        private async void DSNV_Uc_Load(object sender, EventArgs e)
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
            NhanVienBLL nvBLL = new NhanVienBLL();
            dsNhanVien = new BindingList<NhanVien>(nvBLL.layDanhSachNhanVien_PhanTrang(trangHientai, kichthuocTrang));
            InitializeContextMenu();
            InitializeButtonIcons();
            InitializeButtonStyles();
            InitializeCustomSearchBox();
            InitializeDataGridView();
            CalculateLayout();

            dgvDanhsachnhanvien.DataSource = dsNhanVien;
            dgvDanhsachnhanvien.ReadOnly = true;
            taiTrangkhachhang();

        }


        private Task<string> _currentVoiceTask;
        private CancellationTokenSource _voiceCts;
        private bool _isDisposed = false;

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


            ThemNhanVien frmThem = new ThemNhanVien();
            currentOpenForm = frmThem;
            CenterFormOnParent(frmThem);
            frmThem.FormClosed += (s, ev) =>
            {
                currentOpenForm = null;
            };

            frmThem.SuccesfullyUpdated += (s, ev) => taiDanhSachNhanVien();
            frmThem.Show(this);
        }


        int trangHientai = 1;
        int kichthuocTrang = 15;
        int tongSoBanGhi = 0;
        int tongSoTrang = 0;
        private void taiTrangkhachhang()
        {
            var bll = new NhanVienBLL();

            if (tongSoBanGhi == 0)
            {
                tongSoBanGhi = bll.demSoLuongNhanVien();
                tongSoTrang = (int)Math.Ceiling((double)tongSoBanGhi / kichthuocTrang);
            }

            var data = bll.layDanhSachNhanVien_PhanTrang(trangHientai, kichthuocTrang);
            dgvDanhsachnhanvien.DataSource = data;

            soTrang.Text = $"Trang {trangHientai}/{tongSoTrang}";
            btnTruoc.Enabled = trangHientai > 1;
            btnSau.Enabled = trangHientai < tongSoTrang;
        }

        private void btnTruoc_Click(object sender, EventArgs e)
        {
            if (trangHientai > 1)
            {
                trangHientai--;
                taiTrangkhachhang();
            }
        }

        private void btnSau_Click(object sender, EventArgs e)
        {
            if (trangHientai < tongSoTrang)
            {
                trangHientai++;
                taiTrangkhachhang();
            }
        }

        private SemaphoreSlim _voiceLock = new SemaphoreSlim(1, 1); // ✅ Thêm field này



        private void searchtextbox_TextChanged_2(object sender, EventArgs e)
        {

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

        private void pictureFilter_Click(object sender, EventArgs e)
        {
            LocTimKiemNhanVien filter = new LocTimKiemNhanVien();
            if (filter.ShowDialog() == DialogResult.OK)
            {
                apDungBoLoc(filter.SelectedPhongBan, filter.SelectedGioiTinh, filter.SelectedTrangThai);
            }
            // ✅ Nếu user nhấn Cancel → Reset về phân trang
            else
            {
                ResetFilter();
            }
        }
        private void apDungBoLoc(string maPhong, string gioiTinh, string trangThai)
        {

            try
            {
                // ✅ Lấy TẤT CẢ nhân viên từ BLL (không phân trang)
                NhanVienBLL bll = new NhanVienBLL();
                var allData = bll.layDanhSachNhanVien_TimKiem(); // ✅ Method trả về tất cả

                // Bắt đầu lọc từ danh sách đầy đủ
                var result = allData.AsEnumerable();

                // 1) Lọc phòng ban
                if (!string.IsNullOrEmpty(maPhong))
                    result = result.Where(nv => nv.maPhong == maPhong);

                // 2) Lọc giới tính
                if (!string.IsNullOrEmpty(gioiTinh))
                {
                    result = result.Where(nv => nv.gioiTinh == gioiTinh);
                }

                // 3) Lọc trạng thái
                if (!string.IsNullOrEmpty(trangThai))
                {
                    int trangThaiValue = int.Parse(trangThai);
                    result = result.Where(nv => nv.trangThai == trangThaiValue);
                }

                // ✅ Hiển thị kết quả
                var filteredList = result.ToList();
                dgvDanhsachnhanvien.DataSource = new BindingList<NhanVienSearch>(filteredList);

                // ✅ Tắt phân trang khi đang lọc
                btnTruoc.Enabled = false;
                btnSau.Enabled = false;
                soTrang.Text = $"Tìm thấy {filteredList.Count} kết quả";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi lọc dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void ResetFilter()
        {
            // Clear search box
            if (isPlaceholder == false)
            {
                isPlaceholder = true;
                searchtextbox.Text = PLACEHOLDER_TEXT;
                searchtextbox.ForeColor = Color.Silver;
                tuKhoacuoicung = "";
            }
            trangHientai = 1;
            tongSoBanGhi = 0;
            taiTrangkhachhang();
        }
        private void picturemicro_Click(object sender, EventArgs e)
        {
        }
    }
}