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

            // Thiết lập chiều cao
            int headerHeight = 40;
            int rowHeight = 45;
            dgvDanhsachketqua.ColumnHeadersHeight = headerHeight;
            dgvDanhsachketqua.RowTemplate.Height = rowHeight;

            // Style cho header
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

            // Style cho cells
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

            // ✅ THÊM CÁC CỘT

            // 1. STT
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

            // 2. Mã Đợt Quan Trắc
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

            // 3. Đợt Quan Trắc
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

            // 4. Ngày Tạo
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

            // 5. Ngày Trả KQ
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
                    Font = new Font("Segoe UI", 9.5F)
                }
            });

            // 7. Số Nền Mẫu
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

            // 8. Trạng Thái
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

            // 9. Ghi Chú
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
                    row.Cells["MaDot"].Value = item.MaDot ?? "";
                    row.Cells["DotQuanTrac"].Value = item.DotQuanTrac ?? "";
                    row.Cells["NgayTao"].Value = item.NgayTao;
                    row.Cells["NgayTraKQ"].Value = item.NgayTraKQ;
                    row.Cells["TenNhanVien"].Value = item.TenNhanVien ?? "";
                    row.Cells["SoNenMau"].Value = item.SoNenMau;
                    row.Cells["TrangThai"].Value = item.TrangThai;
                    row.Cells["GhiChu"].Value = item.GhiChu ?? "";

                    // ✅ Tag để lưu MaKQ cho việc mở chi tiết
                    row.Tag = item.MaKQ;
                }

                FormatDataGridView();

                // Cập nhật title
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
                // ✅ FORMAT TRẠNG THÁI
                if (row.Cells["TrangThai"].Value != null)
                {
                    string trangThai = row.Cells["TrangThai"].Value.ToString().Trim();

                    // Kiểm tra cả hai trường hợp
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

                // ✅ HIGHLIGHT DÒNG HOVER
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(111, 207, 151);
                row.DefaultCellStyle.SelectionForeColor = Color.White;
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
    }
}