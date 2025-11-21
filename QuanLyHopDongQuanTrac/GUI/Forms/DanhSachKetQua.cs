using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class DanhSachKetQua : Form
    {
        private KetQuaBLL ketQuaBLL = new KetQuaBLL();

        public DanhSachKetQua()
        {
            InitializeComponent();
            this.Load += DanhSachKetQua_Load;
            this.Resize += DanhSachKetQua_Resize;
            dgvDanhsachketqua.CellDoubleClick += dgvDanhsachketqua_CellDoubleClick;
        }

        private void DanhSachKetQua_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new Size(1400, 750);
            this.StartPosition = FormStartPosition.CenterScreen;

            if (pictureBox1 != null)
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

            SetupDataGridView();
            LoadDanhSachKetQua();

            // ✅ FORCE SET KÍCH THƯỚC SAU KHI LOAD - QUAN TRỌNG!
            dgvDanhsachketqua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // ✅ FORCE SET LẠI WIDTH TỪNG CỘT
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

            // ✅ TẮT AUTO SIZE COLUMNS
            dgvDanhsachketqua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // Thiết lập chiều cao
            int headerHeight = 40;
            int rowHeight = 45;
            dgvDanhsachketqua.ColumnHeadersHeight = headerHeight;
            dgvDanhsachketqua.RowTemplate.Height = rowHeight;

            // Style cho header - CHỮ ĐEN IN ĐẬM, NỀN XÁM
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

            // Style cho cells - CHỮ ĐEN THƯỜNG
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

            // ✅ THÊM CÁC CỘT VỚI CHIỀU RỘNG PHÂN BỔ HỢP LÝ

            // 1. STT - Thu nhỏ
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

            // 2. Tên Công Ty - THÊM MỚI
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

            // 3. Đợt Quan Trắc
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

            // 4. Ngày Tạo
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

            // 5. Ngày Trả KQ
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

            // 6. Người Lập
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

            // 7. Trạng Thái
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

            // 8. Ghi Chú - TỰ ĐỘNG FILL
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

                    // Gán dữ liệu
                    row.Cells["STT"].Value = stt;
                    row.Cells["TenCongTy"].Value = item.TenKhachHang ?? "";
                    row.Cells["DotQuanTrac"].Value = item.DotQuanTrac ?? "";
                    row.Cells["NgayTao"].Value = item.NgayTao;
                    row.Cells["NgayTraKQ"].Value = item.NgayTraKQ;
                    row.Cells["TenNhanVien"].Value = item.TenNhanVien ?? "";
                    row.Cells["TrangThai"].Value = item.TrangThai;
                    row.Cells["GhiChu"].Value = item.GhiChu ?? "";

                    // ✅ Tag để lưu MaKQ cho việc mở chi tiết
                    row.Tag = item.MaKQ;
                }

                FormatDataGridView();

                // ✅ FORCE SET LẠI WIDTH SAU KHI LOAD DATA
                dgvDanhsachketqua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                dgvDanhsachketqua.Columns["STT"].Width = 60;
                dgvDanhsachketqua.Columns["TenCongTy"].Width = 280;
                dgvDanhsachketqua.Columns["DotQuanTrac"].Width = 280;
                dgvDanhsachketqua.Columns["NgayTao"].Width = 125;
                dgvDanhsachketqua.Columns["NgayTraKQ"].Width = 125;
                dgvDanhsachketqua.Columns["TenNhanVien"].Width = 200;
                dgvDanhsachketqua.Columns["TrangThai"].Width = 140;

                // ✅ DEBUG: In ra console để kiểm tra
                System.Diagnostics.Debug.WriteLine($"=== DEBUG COLUMN WIDTH ===");
                System.Diagnostics.Debug.WriteLine($"STT Width: {dgvDanhsachketqua.Columns["STT"].Width}");
                System.Diagnostics.Debug.WriteLine($"TenCongTy Width: {dgvDanhsachketqua.Columns["TenCongTy"].Width}");
                System.Diagnostics.Debug.WriteLine($"NgayTao Width: {dgvDanhsachketqua.Columns["NgayTao"].Width}");
                System.Diagnostics.Debug.WriteLine($"AutoSizeColumnsMode: {dgvDanhsachketqua.AutoSizeColumnsMode}");

                // Cập nhật title
                if (panel6 != null)
                {
                    panel6.Controls.Clear();
                    Label lblTitle = new Label
                    {
                        Text = $"📊 DANH SÁCH KẾT QUẢ QUAN TRẮC ({list.Count} kết quả)",
                        Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                        ForeColor = Color.Black,
                        Dock = DockStyle.Fill,
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    panel6.Controls.Add(lblTitle);
                }

                // Thông báo
                if (list.Count == 0)
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

        // ✅ FORMAT TRẠNG THÁI - FIXED
        private void FormatDataGridView()
        {
            foreach (DataGridViewRow row in dgvDanhsachketqua.Rows)
            {
                // ✅ FORMAT TRẠNG THÁI (chỉ dùng in đậm, không màu)
                if (row.Cells["TrangThai"].Value != null)
                {
                    string trangThai = row.Cells["TrangThai"].Value.ToString().Trim();

                    // Kiểm tra cả hai trường hợp
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

                // ✅ HIGHLIGHT DÒNG HOVER
                row.DefaultCellStyle.SelectionBackColor = Color.LightGray;
                row.DefaultCellStyle.SelectionForeColor = Color.Black;
            }
        }

        // ✅ XỬ LÝ DOUBLE CLICK - FIXED
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

                    // Mở form chi tiết
                    ChiTietKetQua formChiTiet = new ChiTietKetQua(maKQ);
                    DialogResult result = formChiTiet.ShowDialog();

                    // ✅ CHỈ REFRESH KHI CÓ THAY ĐỔI
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

        private void DanhSachKetQua_Resize(object sender, EventArgs e)
        {
            try
            {
                float scaleX = (float)this.ClientSize.Width / 1400;
                float scaleY = (float)this.ClientSize.Height / 750;

                // Sidebar
                if (sidebar != null)
                {
                    int newSidebarWidth = (int)(200 * scaleX);
                    sidebar.Width = Math.Max(180, Math.Min(300, newSidebarWidth));
                }

                // Panel header
                if (panel8 != null)
                {
                    int newPanel8Height = (int)(70 * scaleY);
                    panel8.Height = Math.Max(50, Math.Min(90, newPanel8Height));
                }

                // Panel title
                if (panel6 != null)
                {
                    int newPanel6Height = (int)(75 * scaleY);
                    panel6.Height = Math.Max(65, Math.Min(90, newPanel6Height));
                }

                // PictureBox
                if (pictureBox1 != null && sidebar != null && panel8 != null)
                {
                    pictureBox1.Width = sidebar.Width;
                    pictureBox1.Height = panel8.Height;
                    pictureBox1.Location = new Point(0, 0);
                }

                ResizeButtonsInSidebar(Math.Min(scaleX, scaleY));
            }
            catch { }
        }

        private void ResizeButtonsInSidebar(float scale)
        {
            if (sidebar == null) return;

            foreach (Control control in sidebar.Controls)
            {
                if (control is Button btn)
                {
                    int margin = 5;
                    btn.Width = sidebar.Width - (margin * 2);
                    btn.Height = Math.Max(38, Math.Min(65, (int)(45 * scale)));
                    btn.Left = margin;
                    btn.Font = new Font(btn.Font.FontFamily,
                        Math.Max(9.5f, Math.Min(14f, 11f * scale)), btn.Font.Style);
                }
            }
        }

        private void dgvDanhsachketqua_CellContentClick(object sender, DataGridViewCellEventArgs e) { }

        private void dgvDanhsachketqua_Paint(object sender, PaintEventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}