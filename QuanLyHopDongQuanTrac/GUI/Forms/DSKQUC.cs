using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class DSKQUC : UserControl
    {
        private KetQuaBLL ketQuaBLL = new KetQuaBLL();

        public DSKQUC()
        {
            InitializeComponent();
            this.Load += DSKQUC_Load;
            this.Resize += DSKQUC_Resize;
            dgvDanhsachketqua.CellDoubleClick += dgvDanhsachketqua_CellDoubleClick;
        }

        private void DSKQUC_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            LoadDanhSachKetQua();

            // FORCE SET KÍCH THƯỚC SAU KHI LOAD
            dgvDanhsachketqua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // FORCE SET LẠI WIDTH TỪNG CỘT
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

            // TẮT AUTO SIZE COLUMNS
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

            // 2. Tên Công Ty
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

        public void LoadDanhSachKetQua()
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

                    // Tag để lưu MaKQ cho việc mở chi tiết
                    row.Tag = item.MaKQ;
                }

                FormatDataGridView();

                // FORCE SET LẠI WIDTH SAU KHI LOAD DATA
                dgvDanhsachketqua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
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

                // Cập nhật title
                UpdateTitle(list.Count);

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

        private void UpdateTitle(int count)
        {
            if (panel6 != null)
            {
                panel6.Controls.Clear();
                Label lblTitle = new Label
                {
                    Text = $"📊 DANH SÁCH KẾT QUẢ QUAN TRẮC ({count} kết quả)",
                    Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                };
                panel6.Controls.Add(lblTitle);
            }
        }

        private void FormatDataGridView()
        {
            foreach (DataGridViewRow row in dgvDanhsachketqua.Rows)
            {
                // FORMAT TRẠNG THÁI
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

                // HIGHLIGHT DÒNG HOVER
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

                    // Mở form chi tiết
                    ChiTietKetQua formChiTiet = new ChiTietKetQua(maKQ);
                    DialogResult result = formChiTiet.ShowDialog();

                    // CHỈ REFRESH KHI CÓ THAY ĐỔI
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

        private void DSKQUC_Resize(object sender, EventArgs e)
        {
            // Có thể thêm logic resize nếu cần
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

        private void panel6_Paint(object sender, PaintEventArgs e) { }

        private void panel6_Paint_1(object sender, PaintEventArgs e) { }
    }
}