using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using BLL;
using DTO;

namespace GUI.Forms
{
    public partial class QuanLyHopDongChuKy : Form
    {
        private readonly HopDongBLL hopDongBLL = new HopDongBLL();

        // Controls
        private Chart chartThongKe;
        private ComboBox cboNam;
        private ComboBox cboChuKy;
        private Button btnRefresh;

        // Thẻ thống kê
        private Panel pnlTongHD;
        private Panel pnlDungHen;
        private Panel pnlTreHen;
        private Panel pnlChuaHoanThanh;

        private DataGridView dgvHopDong;

        // Dữ liệu
        private List<DTO_HopDong> danhSachHopDong;

        public QuanLyHopDongChuKy()
        {
            InitializeComponent();
            this.Load += QuanLyHopDongChuKy_Load;
            this.Resize += QuanLyHopDongChuKy_Resize;
        }

        private void QuanLyHopDongChuKy_Load(object sender, EventArgs e)
        {
            try
            {
                // ✅ Hiển thị form ở giữa màn hình
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.Size = new Size(1600, 900);
                this.MinimumSize = new Size(1400, 800);
                this.Text = "📊 Biểu đồ quản lý hợp đồng";

                InitializeControls();
                LoadData();
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi khởi tạo form: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeControls()
        {
            // Panel chính
            Panel mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(240, 242, 245),
                Padding = new Padding(15)
            };
            this.Controls.Add(mainPanel);

            // ========== PHẦN HEADER ==========
            Panel headerPanel = new Panel
            {
                Height = 80,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };
            mainPanel.Controls.Add(headerPanel);

            Label lblTitle = new Label
            {
                Text = "📊 THỐNG KÊ CHU KỲ HỢP ĐỒNG",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 204),
                AutoSize = false,
                Size = new Size(600, 40),
                Location = new Point(20, 15)
            };
            headerPanel.Controls.Add(lblTitle);

            // Bộ lọc
            Label lblNam = new Label
            {
                Text = "Năm:",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(650, 25),
                AutoSize = true
            };
            headerPanel.Controls.Add(lblNam);

            cboNam = new ComboBox
            {
                Location = new Point(700, 22),
                Size = new Size(100, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear - 2; i <= currentYear + 2; i++)
                cboNam.Items.Add(i);
            cboNam.SelectedItem = currentYear;
            cboNam.SelectedIndexChanged += Filter_Changed;
            headerPanel.Controls.Add(cboNam);

            Label lblChuKy = new Label
            {
                Text = "Chu kỳ:",
                Font = new Font("Segoe UI", 10F),
                Location = new Point(820, 25),
                AutoSize = true
            };
            headerPanel.Controls.Add(lblChuKy);

            cboChuKy = new ComboBox
            {
                Location = new Point(885, 22),
                Size = new Size(120, 30),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F)
            };
            cboChuKy.Items.AddRange(new string[] { "Theo Quý", "6 Tháng" });
            cboChuKy.SelectedIndex = 0;
            cboChuKy.SelectedIndexChanged += Filter_Changed;
            headerPanel.Controls.Add(cboChuKy);

            btnRefresh = new Button
            {
                Text = "🔄 Làm mới",
                Location = new Point(1020, 20),
                Size = new Size(120, 35),
                BackColor = Color.FromArgb(40, 167, 69),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Click += BtnRefresh_Click;
            headerPanel.Controls.Add(btnRefresh);

            // ========== PHẦN THẺ THỐNG KÊ ==========
            Panel cardsPanel = new Panel
            {
                Height = 120,
                Dock = DockStyle.Top,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 10)
            };
            mainPanel.Controls.Add(cardsPanel);

            // Tạo các thẻ với vị trí ban đầu
            pnlTongHD = CreateStatCard("📋 Tổng hợp đồng", "0", Color.FromArgb(0, 123, 255));
            pnlTongHD.Location = new Point(10, 10);
            cardsPanel.Controls.Add(pnlTongHD);

            pnlDungHen = CreateStatCard("✅ Đúng hẹn", "0 (0%)", Color.FromArgb(40, 167, 69));
            pnlDungHen.Location = new Point(300, 10);
            cardsPanel.Controls.Add(pnlDungHen);

            pnlTreHen = CreateStatCard("⚠️ Trễ hẹn", "0 (0%)", Color.FromArgb(255, 193, 7));
            pnlTreHen.Location = new Point(590, 10);
            cardsPanel.Controls.Add(pnlTreHen);

            pnlChuaHoanThanh = CreateStatCard("🔄 Chưa hoàn thành", "0 (0%)", Color.FromArgb(220, 53, 69));
            pnlChuaHoanThanh.Location = new Point(880, 10);
            cardsPanel.Controls.Add(pnlChuaHoanThanh);

            // Thêm sự kiện resize sau khi tạo các thẻ
            cardsPanel.Resize += CardsPanel_Resize;

            // ========== PHẦN BIỂU ĐỒ ==========
            Panel chartPanel = new Panel
            {
                Height = 350,
                Dock = DockStyle.Top,
                BackColor = Color.White,
                Padding = new Padding(15),
                Margin = new Padding(0, 10, 0, 10)
            };
            mainPanel.Controls.Add(chartPanel);

            chartThongKe = new Chart
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };

            ChartArea chartArea = new ChartArea("MainArea")
            {
                BackColor = Color.White,
                BorderWidth = 0
            };
            chartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 9F);
            chartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 9F);
            chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
            chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            chartThongKe.ChartAreas.Add(chartArea);

            // Series
            Series seriesDungHen = new Series("Đúng hẹn")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(40, 167, 69),
                IsValueShownAsLabel = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold)
            };
            chartThongKe.Series.Add(seriesDungHen);

            Series seriesTreHen = new Series("Trễ hẹn")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(255, 193, 7),
                IsValueShownAsLabel = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold)
            };
            chartThongKe.Series.Add(seriesTreHen);

            Series seriesChuaHT = new Series("Chưa hoàn thành")
            {
                ChartType = SeriesChartType.Column,
                Color = Color.FromArgb(220, 53, 69),
                IsValueShownAsLabel = true,
                Font = new Font("Segoe UI", 8F, FontStyle.Bold)
            };
            chartThongKe.Series.Add(seriesChuaHT);

            Legend legend = new Legend
            {
                Docking = Docking.Top,
                Alignment = StringAlignment.Center,
                Font = new Font("Segoe UI", 9F)
            };
            chartThongKe.Legends.Add(legend);

            Title chartTitle = new Title
            {
                Text = "BIỂU ĐỒ KẾ HOẠCH HỢP ĐỒNG",
                Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 204)
            };
            chartThongKe.Titles.Add(chartTitle);

            chartPanel.Controls.Add(chartThongKe);

            // ========== DANH SÁCH HỢP ĐỒNG ==========
            Panel dgvPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(15)
            };
            mainPanel.Controls.Add(dgvPanel);

            Label lblDanhSach = new Label
            {
                Text = "📋 DANH SÁCH HỢP ĐỒNG CHI TIẾT",
                Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 102, 204),
                Dock = DockStyle.Top,
                Height = 35,
                TextAlign = ContentAlignment.MiddleLeft
            };
            dgvPanel.Controls.Add(lblDanhSach);

            dgvHopDong = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                RowHeadersVisible = false,
                EnableHeadersVisualStyles = false,
                AllowUserToResizeRows = false,
                RowTemplate = { Height = 40 },
                ColumnHeadersHeight = 45,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill // Tự động điều chỉnh chiều rộng cột
            };

            dgvHopDong.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(0, 102, 204),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Padding = new Padding(5)
            };

            dgvHopDong.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9.5F),
                BackColor = Color.White,
                ForeColor = Color.Black,
                SelectionBackColor = Color.FromArgb(220, 230, 242),
                SelectionForeColor = Color.Black,
                Padding = new Padding(5)
            };

            dgvHopDong.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250)
            };

            // Columns với FillWeight để điều chỉnh tỷ lệ
            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaHD",
                HeaderText = "Mã HĐ",
                DataPropertyName = "MaHD",
                FillWeight = 10 // 10%
            });

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "KhachHang",
                HeaderText = "Khách hàng",
                DataPropertyName = "TenKhachHang",
                FillWeight = 25 // 25%
            });

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgayKy",
                HeaderText = "Ngày ký",
                DataPropertyName = "NgayKy",
                FillWeight = 12, // 12%
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy",
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "NgayKetThuc",
                HeaderText = "Ngày kết thúc",
                DataPropertyName = "NgayKetThuc",
                FillWeight = 12, // 12%
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy",
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TanSuat",
                HeaderText = "Tần suất",
                DataPropertyName = "TanSuatQuanTrac",
                FillWeight = 18 // 18%
            });

            dgvHopDong.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TrangThai",
                HeaderText = "Tình trạng giao hàng",
                FillWeight = 23, // 23%
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
                }
            });

            dgvPanel.Controls.Add(dgvHopDong);
        }

        // Hàm tạo thẻ thống kê (không cần x, y, width nữa)
        private Panel CreateStatCard(string title, string value, Color color)
        {
            Panel card = new Panel
            {
                Height = 100,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };

            // Thanh màu bên trái
            Panel colorBar = new Panel
            {
                Width = 5,
                Height = 100,
                Location = new Point(0, 0),
                BackColor = color
            };
            card.Controls.Add(colorBar);

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.FromArgb(108, 117, 125),
                AutoSize = false,
                Location = new Point(15, 15),
                Size = new Size(250, 25)
            };
            card.Controls.Add(lblTitle);

            Label lblValue = new Label
            {
                Name = "lblValue",
                Text = value,
                Font = new Font("Segoe UI", 18F, FontStyle.Bold),
                ForeColor = color,
                AutoSize = false,
                Location = new Point(15, 45),
                Size = new Size(250, 40)
            };
            card.Controls.Add(lblValue);

            return card;
        }

        // Sự kiện resize để căn lại vị trí các thẻ thống kê
        private void CardsPanel_Resize(object sender, EventArgs e)
        {
            Panel panel = sender as Panel;
            if (panel == null) return;

            int cardCount = 4;
            int spacing = 15;
            int totalSpacing = spacing * (cardCount + 1);
            int availableWidth = panel.ClientSize.Width - totalSpacing;
            int cardWidth = availableWidth / cardCount;

            Panel[] cards = { pnlTongHD, pnlDungHen, pnlTreHen, pnlChuaHoanThanh };

            for (int i = 0; i < cards.Length; i++)
            {
                if (cards[i] != null)
                {
                    cards[i].Location = new Point(spacing + i * (cardWidth + spacing), 10);
                    cards[i].Width = cardWidth;
                }
            }
        }

        private void LoadData()
        {
            try
            {
                danhSachHopDong = hopDongBLL.LayDanhSachHopDong();

                if (danhSachHopDong == null || danhSachHopDong.Count == 0)
                {
                    MessageBox.Show("Chưa có dữ liệu hợp đồng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    danhSachHopDong = new List<DTO_HopDong>();
                    return;
                }

                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}\n\nChi tiết: {ex.StackTrace}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);

                danhSachHopDong = new List<DTO_HopDong>();
            }
        }

        private void ApplyFilters()
        {
            if (danhSachHopDong == null || danhSachHopDong.Count == 0)
            {
                dgvHopDong.Rows.Clear();
                return;
            }

            if (cboNam.SelectedItem == null)
                return;

            int selectedYear = (int)cboNam.SelectedItem;
            string chuKy = cboChuKy.SelectedItem?.ToString() ?? "Theo Quý";

            var filteredData = danhSachHopDong
                .Where(hd => hd.NgayKy.Year == selectedYear || hd.NgayKetThuc.Year == selectedYear)
                .ToList();

            DisplayHopDongList(filteredData);
            UpdateChart(filteredData, chuKy);
        }

        private void DisplayHopDongList(List<DTO_HopDong> data)
        {
            dgvHopDong.Rows.Clear();

            foreach (var hd in data)
            {
                int rowIndex = dgvHopDong.Rows.Add();
                var row = dgvHopDong.Rows[rowIndex];

                row.Cells["MaHD"].Value = hd.MaHD;
                row.Cells["KhachHang"].Value = hd.TenKhachHang;
                row.Cells["NgayKy"].Value = hd.NgayKy;
                row.Cells["NgayKetThuc"].Value = hd.NgayKetThuc;
                row.Cells["TanSuat"].Value = hd.TanSuatQuanTrac;

                string tinhTrang = DetermineTinhTrang(hd);
                row.Cells["TrangThai"].Value = tinhTrang;

                Color fgColor = Color.Black;

                if (tinhTrang.Contains("Đúng hẹn"))
                {
                    fgColor = Color.FromArgb(40, 167, 69);
                }
                else if (tinhTrang.Contains("Trễ hẹn"))
                {
                    fgColor = Color.FromArgb(255, 193, 7);
                }
                else if (tinhTrang.Contains("Chưa hoàn thành"))
                {
                    fgColor = Color.FromArgb(220, 53, 69);
                }

                row.Cells["TrangThai"].Style.ForeColor = fgColor;
                row.Cells["TrangThai"].Style.Font = new Font("Segoe UI", 9.5F, FontStyle.Bold);
            }
        }

        private string DetermineTinhTrang(DTO_HopDong hd)
        {
            DateTime now = DateTime.Now;

            if (string.IsNullOrWhiteSpace(hd.TrangThai))
            {
                if (now > hd.NgayKetThuc)
                    return "⚠️ Trễ hẹn";
                else
                    return "🔄 Chưa hoàn thành";
            }

            string trangThai = hd.TrangThai.Trim().ToLower();

            if (trangThai.Contains("hoàn thành") || trangThai == "tt03")
            {
                return "✅ Đúng hẹn";
            }

            if (trangThai.Contains("hiệu lực") || trangThai == "tt01")
            {
                if (now > hd.NgayKetThuc)
                    return "⚠️ Trễ hẹn";
                else
                    return "🔄 Chưa hoàn thành";
            }

            if (trangThai.Contains("hết hạn") || trangThai == "tt02")
            {
                return "⚠️ Trễ hẹn";
            }

            if (now > hd.NgayKetThuc)
                return "⚠️ Trễ hẹn";
            else
                return "🔄 Chưa hoàn thành";
        }

        private void UpdateChart(List<DTO_HopDong> data, string chuKy)
        {
            chartThongKe.Series["Đúng hẹn"].Points.Clear();
            chartThongKe.Series["Trễ hẹn"].Points.Clear();
            chartThongKe.Series["Chưa hoàn thành"].Points.Clear();

            if (data == null || data.Count == 0)
                return;

            if (chuKy == "Theo Quý")
            {
                for (int q = 1; q <= 4; q++)
                {
                    var hopDongTrongQuy = data.Where(hd =>
                        GetQuarter(hd.NgayKy) == q || GetQuarter(hd.NgayKetThuc) == q
                    ).ToList();

                    int dungHen = hopDongTrongQuy.Count(hd =>
                        DetermineTinhTrang(hd).Contains("Đúng hẹn"));

                    int treHen = hopDongTrongQuy.Count(hd =>
                        DetermineTinhTrang(hd).Contains("Trễ hẹn"));

                    int chuaHT = hopDongTrongQuy.Count(hd =>
                        DetermineTinhTrang(hd).Contains("Chưa hoàn thành"));

                    chartThongKe.Series["Đúng hẹn"].Points.AddXY($"Quý {q}", dungHen);
                    chartThongKe.Series["Trễ hẹn"].Points.AddXY($"Quý {q}", treHen);
                    chartThongKe.Series["Chưa hoàn thành"].Points.AddXY($"Quý {q}", chuaHT);
                }
            }
            else
            {
                for (int period = 1; period <= 2; period++)
                {
                    var hopDongTrongKy = data.Where(hd =>
                        GetHalfYear(hd.NgayKy) == period || GetHalfYear(hd.NgayKetThuc) == period
                    ).ToList();

                    int dungHen = hopDongTrongKy.Count(hd =>
                        DetermineTinhTrang(hd).Contains("Đúng hẹn"));

                    int treHen = hopDongTrongKy.Count(hd =>
                        DetermineTinhTrang(hd).Contains("Trễ hẹn"));

                    int chuaHT = hopDongTrongKy.Count(hd =>
                        DetermineTinhTrang(hd).Contains("Chưa hoàn thành"));

                    string label = period == 1 ? "6T đầu" : "6T cuối";
                    chartThongKe.Series["Đúng hẹn"].Points.AddXY(label, dungHen);
                    chartThongKe.Series["Trễ hẹn"].Points.AddXY(label, treHen);
                    chartThongKe.Series["Chưa hoàn thành"].Points.AddXY(label, chuaHT);
                }
            }
        }

        private void UpdateStatistics()
        {
            if (danhSachHopDong == null || danhSachHopDong.Count == 0)
            {
                UpdateStatCard(pnlTongHD, "0");
                UpdateStatCard(pnlDungHen, "0 (0%)");
                UpdateStatCard(pnlTreHen, "0 (0%)");
                UpdateStatCard(pnlChuaHoanThanh, "0 (0%)");
                return;
            }

            if (cboNam.SelectedItem == null)
                return;

            int selectedYear = (int)cboNam.SelectedItem;
            var yearData = danhSachHopDong
                .Where(hd => hd.NgayKy.Year == selectedYear || hd.NgayKetThuc.Year == selectedYear)
                .ToList();

            int total = yearData.Count;

            if (total == 0)
            {
                UpdateStatCard(pnlTongHD, "0");
                UpdateStatCard(pnlDungHen, "0 (0%)");
                UpdateStatCard(pnlTreHen, "0 (0%)");
                UpdateStatCard(pnlChuaHoanThanh, "0 (0%)");
                return;
            }

            int dungHen = yearData.Count(hd => DetermineTinhTrang(hd).Contains("Đúng hẹn"));
            int treHen = yearData.Count(hd => DetermineTinhTrang(hd).Contains("Trễ hẹn"));
            int chuaHT = yearData.Count(hd => DetermineTinhTrang(hd).Contains("Chưa hoàn thành"));

            UpdateStatCard(pnlTongHD, total.ToString());
            UpdateStatCard(pnlDungHen, $"{dungHen} ({(dungHen * 100.0 / total):F1}%)");
            UpdateStatCard(pnlTreHen, $"{treHen} ({(treHen * 100.0 / total):F1}%)");
            UpdateStatCard(pnlChuaHoanThanh, $"{chuaHT} ({(chuaHT * 100.0 / total):F1}%)");
        }

        private void UpdateStatCard(Panel card, string value)
        {
            Label lblValue = card.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblValue");
            if (lblValue != null)
                lblValue.Text = value;
        }

        private int GetQuarter(DateTime date)
        {
            return (date.Month - 1) / 3 + 1;
        }

        private int GetHalfYear(DateTime date)
        {
            return date.Month <= 6 ? 1 : 2;
        }

        private void Filter_Changed(object sender, EventArgs e)
        {
            ApplyFilters();
            UpdateStatistics();
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
            UpdateStatistics();
            MessageBox.Show("✅ Dữ liệu đã được làm mới!", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void QuanLyHopDongChuKy_Resize(object sender, EventArgs e)
        {
        }
    }
}