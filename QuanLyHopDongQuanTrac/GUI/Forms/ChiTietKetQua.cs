using BLL;
using DTO;
using System;
using SelectPdf;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Alias để tránh xung đột namespace
using DrawingPoint = System.Drawing.Point;
using DrawingRectangle = System.Drawing.Rectangle;

namespace GUI.Forms
{
    public partial class ChiTietKetQua : Form
    {
        private readonly string maKQ;
        private readonly KetQuaBLL ketQuaBLL = new KetQuaBLL();
        private readonly EmailService emailService = new EmailService();
        private DTO_KetQuaFull ketQuaFull;
        private Panel panelNenMau;
        private bool daThayDoiTrangThai = false;

        public bool DaThayDoiTrangThai => daThayDoiTrangThai;

        public ChiTietKetQua(string maKQ)
        {
            InitializeComponent();
            this.maKQ = maKQ;

            this.Load += ChiTietKetQua_Load;
            this.Resize += ChiTietKetQua_Resize;
            this.FormClosing += ChiTietKetQua_FormClosing;

            if (this.btnXuatFile != null)
            {
                try { this.btnXuatFile.Click -= btnXuatFile_Click; } catch { }
                this.btnXuatFile.Click += btnXuatFile_Click;
            }
        }

        private void ChiTietKetQua_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.DialogResult = daThayDoiTrangThai ? DialogResult.OK : DialogResult.Cancel;
        }

        private void ChiTietKetQua_Load(object sender, EventArgs e)
        {
            try
            {
                // Form có kích thước cố định tương đối, không full màn hình
                this.WindowState = FormWindowState.Normal;
                this.StartPosition = FormStartPosition.CenterScreen;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.Size = new Size(1400, 900);
                this.MinimumSize = new Size(1300, 750);
                this.Text = $"Chi Tiết Kết Quả Quan Trắc - {maKQ}";

                LoadChiTietKetQua();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load chi tiết: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ChiTietKetQua_Resize(object sender, EventArgs e)
        {
            if (panelNenMau != null && panel1 != null)
            {
                int headerHeight = 290;
                int buttonAreaHeight = 90;
                int availableHeight = this.ClientSize.Height - headerHeight - buttonAreaHeight;

                panelNenMau.Location = new DrawingPoint(15, headerHeight);
                panelNenMau.Size = new Size(this.ClientSize.Width - 30, availableHeight);

                foreach (Control ctrl in panelNenMau.Controls)
                {
                    if (ctrl is GroupBox grp)
                    {
                        grp.Width = panelNenMau.ClientSize.Width - 30;

                        foreach (Control innerCtrl in grp.Controls)
                        {
                            if (innerCtrl is Panel pnl && pnl.Name == "dgvContainer")
                            {
                                pnl.Width = grp.Width - 30;
                                foreach (Control dgvCtrl in pnl.Controls)
                                {
                                    if (dgvCtrl is DataGridView dgv)
                                    {
                                        dgv.Width = pnl.Width - 5;
                                        ResizeDataGridViewColumns(dgv);
                                    }
                                }
                            }
                        }
                    }
                }

                RepositionButtons();
            }
        }

        private void LoadChiTietKetQua()
        {
            try
            {
                ketQuaFull = ketQuaBLL.LayChiTietKetQuaTheoMaKQ(maKQ);

                if (ketQuaFull == null || ketQuaFull.Header == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin kết quả!",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return;
                }

                HienThiThongTinHeader();
                HienThiDanhSachNenMauNhomTheoBang();
                RepositionButtons();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách kết quả: " + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void HienThiThongTinHeader()
        {
            Font textFont = new Font("Segoe UI", 10.5F);
            Font boldFont = new Font("Segoe UI", 11F, FontStyle.Bold);

            bool daXacNhan = ketQuaFull.Header.TrangThaiXacNhan;

            if (txtMaKQ != null)
            {
                txtMaKQ.Text = ketQuaFull.Header.MaKQ;
                txtMaKQ.BackColor = System.Drawing.Color.FromArgb(230, 240, 255);
                txtMaKQ.Font = boldFont;
                txtMaKQ.ForeColor = System.Drawing.Color.FromArgb(0, 102, 204);
                txtMaKQ.TextAlign = HorizontalAlignment.Center;
                txtMaKQ.BorderStyle = BorderStyle.FixedSingle;
                txtMaKQ.ReadOnly = true;
            }

            if (dtpNgayDo != null)
            {
                dtpNgayDo.Value = ketQuaFull.Header.NgayTao;
                dtpNgayDo.Font = textFont;
                dtpNgayDo.Format = DateTimePickerFormat.Custom;
                dtpNgayDo.CustomFormat = "dd/MM/yyyy";
                dtpNgayDo.Enabled = false;
            }

            if (txtNhanVienNhap != null)
            {
                txtNhanVienNhap.Text = ketQuaFull.Header.TenNhanVien ?? "";
                txtNhanVienNhap.BackColor = System.Drawing.Color.FromArgb(245, 255, 250);
                txtNhanVienNhap.Font = textFont;
                txtNhanVienNhap.ReadOnly = true;
            }

            if (txtTrangThai != null)
            {
                string trangThai = daXacNhan ? "✅ Đã xác nhận" : "⏳ Chờ xác nhận";
                txtTrangThai.Text = trangThai;
                txtTrangThai.BackColor = daXacNhan ? System.Drawing.Color.FromArgb(200, 255, 200) : System.Drawing.Color.FromArgb(255, 245, 200);
                txtTrangThai.ForeColor = daXacNhan ? System.Drawing.Color.FromArgb(0, 128, 0) : System.Drawing.Color.FromArgb(204, 136, 0);
                txtTrangThai.Font = boldFont;
                txtTrangThai.TextAlign = HorizontalAlignment.Center;
                txtTrangThai.ReadOnly = true;
            }

            if (txtGhiChu != null)
            {
                txtGhiChu.Text = ketQuaFull.Header.GhiChu ?? "";
                txtGhiChu.BackColor = System.Drawing.Color.FromArgb(255, 255, 240);
                txtGhiChu.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
                txtGhiChu.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125);
                txtGhiChu.ReadOnly = true;
            }

            if (txtNenMau != null)
            {
                int soNenMau = ketQuaFull.DanhSachNenMau.Count;
                string dotText = ketQuaFull.Header.DotQuanTrac ?? "Chưa xác định";
                txtNenMau.Text = $"📊 {dotText} ({soNenMau} nền mẫu)";
                txtNenMau.BackColor = System.Drawing.Color.FromArgb(240, 255, 245);
                txtNenMau.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                txtNenMau.ForeColor = System.Drawing.Color.FromArgb(0, 152, 70);
                txtNenMau.ReadOnly = true;
            }

            if (btnXacNhan != null)
            {
                btnXacNhan.Enabled = !daXacNhan;
                btnXacNhan.BackColor = !daXacNhan ? System.Drawing.Color.FromArgb(0, 152, 70) : System.Drawing.Color.Gray;
                btnXacNhan.ForeColor = System.Drawing.Color.White;
                btnXacNhan.FlatStyle = FlatStyle.Flat;
                btnXacNhan.FlatAppearance.BorderSize = 0;
                btnXacNhan.Cursor = !daXacNhan ? Cursors.Hand : Cursors.Default;
                btnXacNhan.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                btnXacNhan.Text = "✓ Xác Nhận";
                ApplyRoundedCorners(btnXacNhan, 12);
            }

            if (btnHuyXacNhan != null)
            {
                btnHuyXacNhan.Enabled = daXacNhan;
                btnHuyXacNhan.BackColor = daXacNhan ? System.Drawing.Color.FromArgb(220, 53, 69) : System.Drawing.Color.Gray;
                btnHuyXacNhan.ForeColor = System.Drawing.Color.White;
                btnHuyXacNhan.FlatStyle = FlatStyle.Flat;
                btnHuyXacNhan.FlatAppearance.BorderSize = 0;
                btnHuyXacNhan.Cursor = daXacNhan ? Cursors.Hand : Cursors.Default;
                btnHuyXacNhan.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                btnHuyXacNhan.Text = "✗ Hủy Xác Nhận";
                ApplyRoundedCorners(btnHuyXacNhan, 12);
            }

            if (btnXuatFile != null)
            {
                btnXuatFile.BackColor = System.Drawing.Color.FromArgb(0, 123, 255);
                btnXuatFile.ForeColor = System.Drawing.Color.White;
                btnXuatFile.FlatStyle = FlatStyle.Flat;
                btnXuatFile.FlatAppearance.BorderSize = 0;
                btnXuatFile.Cursor = Cursors.Hand;
                btnXuatFile.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                btnXuatFile.Text = "📄 Xuất File";
                ApplyRoundedCorners(btnXuatFile, 12);
            }
        }

        private void HienThiDanhSachNenMauNhomTheoBang()
        {
            if (dgvChiTiet != null && panel1.Controls.Contains(dgvChiTiet))
                panel1.Controls.Remove(dgvChiTiet);

            panelNenMau = this.Controls.Find("panelNenMau", true).Length > 0
                ? this.Controls.Find("panelNenMau", true)[0] as Panel
                : null;

            if (panelNenMau == null)
            {
                int headerHeight = 290;
                int buttonAreaHeight = 90;
                int availableHeight = this.ClientSize.Height - headerHeight - buttonAreaHeight;

                panelNenMau = new Panel
                {
                    Name = "panelNenMau",
                    AutoScroll = true,
                    Location = new DrawingPoint(15, headerHeight),
                    Size = new Size(this.ClientSize.Width - 30, availableHeight),
                    BackColor = System.Drawing.Color.FromArgb(240, 242, 245)
                };
                panel1.Controls.Add(panelNenMau);
                panelNenMau.BringToFront();
            }

            panelNenMau.Controls.Clear();

            int yPosition = 15;
            int groupIndex = 0;

            foreach (var nenMau in ketQuaFull.DanhSachNenMau)
            {
                groupIndex++;

                GroupBox grpNenMau = new GroupBox
                {
                    Width = panelNenMau.ClientSize.Width - 30,
                    Location = new DrawingPoint(10, yPosition),
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = System.Drawing.Color.White,
                    Padding = new Padding(3),
                    BackColor = System.Drawing.Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                var headerPanel = new Panel
                {
                    Height = 60,
                    Dock = DockStyle.Top,
                    BackColor = System.Drawing.Color.FromArgb(0, 152, 70)
                };

                var lblHeader = new Label
                {
                    Text = $"   📋 NỀN MẪU {groupIndex}: {nenMau.TenNenMau?.ToUpper() ?? "N/A"} ({nenMau.MaNen})  •  Mã KQ Nền: {nenMau.MaKQNen}",
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = System.Drawing.Color.White,
                    AutoSize = false,
                    Height = 60,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Padding = new Padding(15, 0, 0, 0)
                };
                headerPanel.Controls.Add(lblHeader);

                var infoPanel = new Panel
                {
                    Height = 50,
                    Dock = DockStyle.Top,
                    BackColor = System.Drawing.Color.FromArgb(230, 245, 255),
                    Padding = new Padding(15, 12, 15, 12)
                };

                string viTriText = !string.IsNullOrEmpty(nenMau.ViTri) ? nenMau.ViTri : "Chưa xác định vị trí";
                string toaDoText = !string.IsNullOrEmpty(nenMau.ToaDo) ? nenMau.ToaDo : "Chưa xác định tọa độ";
                int soThongSo = nenMau.DanhSachThongSo?.Count ?? 0;

                var lblInfo = new Label
                {
                    Text = $"📍 Vị trí: {viTriText}     •     🗺️ Tọa độ: {toaDoText}     •     📊 Số thông số: {soThongSo}",
                    Font = new Font("Segoe UI", 10.2F),
                    ForeColor = System.Drawing.Color.FromArgb(0, 102, 204),
                    AutoSize = false,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft
                };
                infoPanel.Controls.Add(lblInfo);

                DataGridView dgvThongSo = TaoDGVThongSo(groupIndex, nenMau, grpNenMau.Width);

                Panel dgvContainer = new Panel
                {
                    Name = "dgvContainer",
                    Dock = DockStyle.Fill,
                    Padding = new Padding(12),
                    BackColor = System.Drawing.Color.White
                };
                dgvContainer.Controls.Add(dgvThongSo);

                int soThongSoHienThi = Math.Max(3, Math.Min(soThongSo, 8));
                int dgvHeight = dgvThongSo.ColumnHeadersHeight + (soThongSoHienThi * dgvThongSo.RowTemplate.Height) + 20;
                dgvThongSo.Height = dgvHeight;

                grpNenMau.Height = headerPanel.Height + infoPanel.Height + dgvHeight + 60;

                grpNenMau.Controls.Add(dgvContainer);
                grpNenMau.Controls.Add(infoPanel);
                grpNenMau.Controls.Add(headerPanel);

                panelNenMau.Controls.Add(grpNenMau);

                yPosition += grpNenMau.Height + 20;
            }
        }

        // ================== DGV THÔNG SỐ (ĐÃ CÂN LẠI TỶ LỆ CỘT, KHÔNG DÙNG FILL) ==================
        private DataGridView TaoDGVThongSo(int groupIndex, DTO_KetQuaNenMau nenMau, int grpWidth)
        {
            DataGridView dgv = new DataGridView
            {
                Name = "dgvThongSo" + groupIndex,
                AutoGenerateColumns = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                MultiSelect = false,
                AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None,
                AllowUserToResizeRows = false,
                AllowUserToResizeColumns = true,
                RowTemplate = { Height = 42 },
                ColumnHeadersHeight = 70,
                BackgroundColor = System.Drawing.Color.White,
                BorderStyle = BorderStyle.None,
                EnableHeadersVisualStyles = false,
                Dock = DockStyle.None,
                ScrollBars = ScrollBars.Both,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing,
                Width = grpWidth - 50,
                Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right
            };

            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(200, 200, 200),
                ForeColor = System.Drawing.Color.Black,
                Font = new Font("Segoe UI", 10.2F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = System.Drawing.Color.FromArgb(200, 200, 200),
                Padding = new Padding(5),
                WrapMode = DataGridViewTriState.True
            };

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                BackColor = System.Drawing.Color.White,
                ForeColor = System.Drawing.Color.Black,
                SelectionBackColor = System.Drawing.Color.FromArgb(200, 200, 200),
                SelectionForeColor = System.Drawing.Color.Black,
                Padding = new Padding(6),
                WrapMode = DataGridViewTriState.False
            };

            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.Color.FromArgb(248, 249, 250),
                SelectionBackColor = System.Drawing.Color.FromArgb(200, 200, 200),
                SelectionForeColor = System.Drawing.Color.Black,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = System.Drawing.Color.Black
            };

            int containerWidth = grpWidth - 50;

            // Tỷ lệ chiều rộng các cột (tổng ~ 1.0)
            // STT            : 6%
            // Thông số       : 24%
            // Đơn vị         : 8%
            // Phương pháp    : 26%
            // Kết quả        : 14%
            // Giới hạn P.H   : 12%
            // QCVN           : 10%  (không quá rộng, vẫn lấp phần trống)
            int sttWidth = (int)(containerWidth * 0.06);
            int tenTsWidth = (int)(containerWidth * 0.24);
            int donViWidth = (int)(containerWidth * 0.08);
            int ppWidth = (int)(containerWidth * 0.26);
            int kqWidth = (int)(containerWidth * 0.14);
            int ghphWidth = (int)(containerWidth * 0.12);
            int qcvnWidth = (int)(containerWidth * 0.10);

            // ==== Cột STT ====
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STT",
                HeaderText = "STT",
                Width = sttWidth,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    ForeColor = System.Drawing.Color.Black
                }
            });

            // ==== Cột Thông số ====
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenTS",
                HeaderText = "Thông Số Phân Tích",
                DataPropertyName = "TenTS",
                Width = tenTsWidth,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    ForeColor = System.Drawing.Color.Black
                }
            });

            // ==== Cột Đơn vị ====
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DonVi",
                HeaderText = "Đơn Vị",
                DataPropertyName = "DonVi",
                Width = donViWidth,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    ForeColor = System.Drawing.Color.Black
                }
            });

            // ==== Cột Phương pháp phân tích ====
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhuongPhapPhanTich",
                HeaderText = "Phương Pháp Phân Tích",
                DataPropertyName = "PhuongPhapPhanTich",
                Width = ppWidth,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    ForeColor = System.Drawing.Color.Black
                }
            });

            // ==== Cột Kết quả ====
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "KetQua",
                HeaderText = "Kết Quả Đo",
                DataPropertyName = "KetQua",
                Width = kqWidth,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = System.Drawing.Color.Black,
                    Format = "N2"
                }
            });

            // ==== Cột Giới hạn phát hiện ====
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GioiHanPhatHien",
                HeaderText = "Giới Hạn Phát Hiện",
                DataPropertyName = "GioiHanPhatHien",
                Width = ghphWidth,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    ForeColor = System.Drawing.Color.Black
                }
            });

            // ==== Cột QCVN (10% rộng, KHÔNG dùng Fill để tránh kéo quá to) ====
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "QCVN",
                HeaderText = "QCVN 40:2011/BTNMT Cột B",
                DataPropertyName = "QCVN",
                Width = qcvnWidth,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    ForeColor = System.Drawing.Color.Black
                }
            });

            int stt = 0;
            if (nenMau.DanhSachThongSo != null)
            {
                foreach (var thongSo in nenMau.DanhSachThongSo)
                {
                    stt++;
                    int rowIndex = dgv.Rows.Add();
                    var row = dgv.Rows[rowIndex];

                    row.Cells["STT"].Value = stt;
                    row.Cells["TenTS"].Value = thongSo.TenTS ?? "";
                    row.Cells["DonVi"].Value = thongSo.DonVi ?? "-";
                    row.Cells["PhuongPhapPhanTich"].Value = thongSo.PhuongPhapPhanTich ?? "";
                    row.Cells["KetQua"].Value = thongSo.KetQua;
                    row.Cells["GioiHanPhatHien"].Value = thongSo.GioiHanPhatHien ?? "";
                    row.Cells["QCVN"].Value = thongSo.QCVN ?? "Không quy định";

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.ForeColor = System.Drawing.Color.Black;
                        cell.Style.BackColor = System.Drawing.Color.White;
                    }
                }
            }

            return dgv;
        }

        // ================== CĂN LẠI CỘT KHI FORM / GROUPBOX RESIZE ==================
        private void ResizeDataGridViewColumns(DataGridView dgv)
        {
            if (dgv == null || dgv.Columns.Count == 0 || dgv.Parent == null) return;

            // Cập nhật lại width tổng của DGV
            dgv.Width = dgv.Parent.Width - 10;
            int containerWidth = dgv.Width;

            // Nếu thiếu các cột mong đợi thì bỏ qua
            if (!dgv.Columns.Contains("STT") ||
                !dgv.Columns.Contains("TenTS") ||
                !dgv.Columns.Contains("DonVi") ||
                !dgv.Columns.Contains("PhuongPhapPhanTich") ||
                !dgv.Columns.Contains("KetQua") ||
                !dgv.Columns.Contains("GioiHanPhatHien") ||
                !dgv.Columns.Contains("QCVN"))
            {
                return;
            }

            // Tỷ lệ giống như trong TaoDGVThongSo
            int sttWidth = (int)(containerWidth * 0.06);
            int tenTsWidth = (int)(containerWidth * 0.24);
            int donViWidth = (int)(containerWidth * 0.08);
            int ppWidth = (int)(containerWidth * 0.26);
            int kqWidth = (int)(containerWidth * 0.14);
            int ghphWidth = (int)(containerWidth * 0.12);
            int qcvnWidth = (int)(containerWidth * 0.10);

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            dgv.Columns["STT"].Width = sttWidth;
            dgv.Columns["TenTS"].Width = tenTsWidth;
            dgv.Columns["DonVi"].Width = donViWidth;
            dgv.Columns["PhuongPhapPhanTich"].Width = ppWidth;
            dgv.Columns["KetQua"].Width = kqWidth;
            dgv.Columns["GioiHanPhatHien"].Width = ghphWidth;
            dgv.Columns["QCVN"].Width = qcvnWidth;
        }

        private void RepositionButtons()
        {
            if (btnHuyXacNhan == null || btnXacNhan == null || btnXuatFile == null) return;

            int buttonY = this.ClientSize.Height - 75;
            int buttonHeight = 52;
            int totalWidth = this.ClientSize.Width;

            btnHuyXacNhan.Location = new DrawingPoint(100, buttonY);
            btnHuyXacNhan.Size = new Size(190, buttonHeight);

            int centerX = (totalWidth - 190) / 2;
            btnXacNhan.Location = new DrawingPoint(centerX, buttonY);
            btnXacNhan.Size = new Size(190, buttonHeight);

            btnXuatFile.Location = new DrawingPoint(totalWidth - 290, buttonY);
            btnXuatFile.Size = new Size(190, buttonHeight);
        }

        private void ApplyRoundedCorners(Button button, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            DrawingRectangle rect = new DrawingRectangle(0, 0, button.Width, button.Height);

            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Right - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Right - radius, rect.Bottom - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius, radius, radius, 90, 90);
            path.CloseFigure();

            button.Region = new Region(path);
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn xác nhận kết quả {maKQ}?\n\n" +
                $"✓ Sau khi xác nhận, kết quả sẽ không thể chỉnh sửa\n" +
                $"✓ Dữ liệu sẽ được sử dụng để tạo báo cáo chính thức",
                "⚠️ Xác nhận kết quả", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var (success, message) = ketQuaBLL.CapNhatTrangThaiKetQua(maKQ, true);
                if (success)
                {
                    MessageBox.Show("✓ " + message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ Đánh dấu đã thay đổi
                    daThayDoiTrangThai = true;

                    // ✅ Load lại dữ liệu MỚI từ database
                    ketQuaFull = ketQuaBLL.LayChiTietKetQuaTheoMaKQ(maKQ);

                    // ✅ Refresh toàn bộ UI (bao gồm nút và textbox)
                    HienThiThongTinHeader();

                    // ✅ Hoặc cập nhật trực tiếp ngay lập tức (đảm bảo 100%)
                    if (btnXacNhan != null)
                    {
                        btnXacNhan.Enabled = false;
                        btnXacNhan.BackColor = System.Drawing.Color.Gray;
                        btnXacNhan.Cursor = Cursors.Default;
                    }

                    if (btnHuyXacNhan != null)
                    {
                        btnHuyXacNhan.Enabled = true;
                        btnHuyXacNhan.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
                        btnHuyXacNhan.Cursor = Cursors.Hand;
                    }

                    if (txtTrangThai != null)
                    {
                        txtTrangThai.Text = "✅ Đã xác nhận";
                        txtTrangThai.BackColor = System.Drawing.Color.FromArgb(200, 255, 200);
                        txtTrangThai.ForeColor = System.Drawing.Color.FromArgb(0, 128, 0);
                    }
                }
                else
                {
                    MessageBox.Show("✗ " + message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnHuyXacNhan_Click(object sender, EventArgs e)
        {
            var confirm = MessageBox.Show(
                $"Bạn có chắc chắn muốn hủy xác nhận kết quả {maKQ}?\n\n" +
                $"• Kết quả sẽ chuyển về trạng thái chưa xác nhận\n" +
                $"• Có thể chỉnh sửa lại dữ liệu",
                "⚠️ Hủy xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                var (success, message) = ketQuaBLL.CapNhatTrangThaiKetQua(maKQ, false);
                if (success)
                {
                    MessageBox.Show("✓ " + message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // ✅ Đánh dấu đã thay đổi
                    daThayDoiTrangThai = true;

                    // ✅ Load lại dữ liệu MỚI từ database
                    ketQuaFull = ketQuaBLL.LayChiTietKetQuaTheoMaKQ(maKQ);

                    // ✅ Refresh toàn bộ UI (bao gồm nút và textbox)
                    HienThiThongTinHeader();

                    // ✅ Hoặc cập nhật trực tiếp ngay lập tức (đảm bảo 100%)
                    if (btnXacNhan != null)
                    {
                        btnXacNhan.Enabled = true;
                        btnXacNhan.BackColor = System.Drawing.Color.FromArgb(0, 152, 70);
                        btnXacNhan.Cursor = Cursors.Hand;
                    }

                    if (btnHuyXacNhan != null)
                    {
                        btnHuyXacNhan.Enabled = false;
                        btnHuyXacNhan.BackColor = System.Drawing.Color.Gray;
                        btnHuyXacNhan.Cursor = Cursors.Default;
                    }

                    if (txtTrangThai != null)
                    {
                        txtTrangThai.Text = "⏳ Chờ xác nhận";
                        txtTrangThai.BackColor = System.Drawing.Color.FromArgb(255, 245, 200);
                        txtTrangThai.ForeColor = System.Drawing.Color.FromArgb(204, 136, 0);
                    }
                }
                else
                {
                    MessageBox.Show("✗ " + message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        // ================== MENU XUẤT FILE ==================

        private void btnXuatFile_Click(object sender, EventArgs e)
        {
            ShowExportMenu();
        }

        private void ShowExportMenu()
        {
            try
            {
                using (var menuForm = new Form())
                {
                    menuForm.Text = "Chọn hành động";
                    menuForm.Size = new Size(500, 300);
                    menuForm.StartPosition = FormStartPosition.CenterParent;
                    menuForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    menuForm.MaximizeBox = false;
                    menuForm.MinimizeBox = false;
                    menuForm.BackColor = System.Drawing.Color.White;

                    var lblTitle = new Label
                    {
                        Text = "📊 Xuất Báo Cáo Kết Quả",
                        Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                        ForeColor = System.Drawing.Color.FromArgb(0, 102, 204),
                        AutoSize = false,
                        Size = new Size(460, 40),
                        Location = new DrawingPoint(20, 20),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    menuForm.Controls.Add(lblTitle);

                    var lblInfo = new Label
                    {
                        Text = $"Mã kết quả: {maKQ}\nVui lòng chọn hành động:",
                        Font = new Font("Segoe UI", 10F),
                        ForeColor = System.Drawing.Color.FromArgb(60, 60, 60),
                        AutoSize = false,
                        Size = new Size(460, 50),
                        Location = new DrawingPoint(20, 70),
                        TextAlign = ContentAlignment.TopLeft
                    };
                    menuForm.Controls.Add(lblInfo);

                    var btnExportPDF = new Button
                    {
                        Text = "📄 Xuất File PDF",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        Size = new Size(460, 50),
                        Location = new DrawingPoint(20, 130),
                        BackColor = System.Drawing.Color.FromArgb(0, 123, 255),
                        ForeColor = System.Drawing.Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Tag = "export"
                    };
                    btnExportPDF.FlatAppearance.BorderSize = 0;
                    btnExportPDF.Click += (s, ev) =>
                    {
                        menuForm.DialogResult = DialogResult.OK;
                        menuForm.Tag = "export";
                        menuForm.Close();
                    };
                    menuForm.Controls.Add(btnExportPDF);

                    var btnExportAndEmail = new Button
                    {
                        Text = "📧 Xuất và Gửi Email cho Khách Hàng",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        Size = new Size(460, 50),
                        Location = new DrawingPoint(20, 190),
                        BackColor = System.Drawing.Color.FromArgb(40, 167, 69),
                        ForeColor = System.Drawing.Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Tag = "email"
                    };
                    btnExportAndEmail.FlatAppearance.BorderSize = 0;
                    btnExportAndEmail.Click += (s, ev) =>
                    {
                        menuForm.DialogResult = DialogResult.OK;
                        menuForm.Tag = "email";
                        menuForm.Close();
                    };
                    menuForm.Controls.Add(btnExportAndEmail);

                    if (menuForm.ShowDialog(this) == DialogResult.OK)
                    {
                        string action = menuForm.Tag?.ToString();
                        if (action == "export")
                            ExportPdfReport();
                        else if (action == "email")
                            ExportAndSendEmail();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== XUẤT PDF ==================

        private void ExportPdfReport()
        {
            try
            {
                if (ketQuaFull == null || ketQuaFull.Header == null)
                {
                    MessageBox.Show("Chưa có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using var sfd = new SaveFileDialog
                {
                    Title = "Lưu báo cáo PDF",
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FilterIndex = 1,
                    FileName = $"KetQua_{maKQ}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                    DefaultExt = "pdf",
                    AddExtension = true,
                    RestoreDirectory = true
                };

                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    CreatePdfFromHtml(sfd.FileName);

                    var result = MessageBox.Show(
                        $"✓ Xuất file PDF thành công!\n\nĐường dẫn: {sfd.FileName}\n\nBạn có muốn mở file ngay không?",
                        "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = sfd.FileName,
                                UseShellExecute = true
                            });
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Không thể mở file:\n{ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất file PDF:\n{ex.Message}\n\nChi tiết: {ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreatePdfFromHtml(string pdfPath)
        {
            try
            {
                string htmlContent = GenerateHtmlReport();

                HtmlToPdf converter = new HtmlToPdf();
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.MarginLeft = 15;
                converter.Options.MarginRight = 15;
                converter.Options.MarginTop = 15;
                converter.Options.MarginBottom = 15;

                PdfDocument doc = converter.ConvertHtmlString(htmlContent);
                doc.Save(pdfPath);
                doc.Close();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tạo PDF: {ex.Message}", ex);
            }
        }

        private string CreateTempPdf()
        {
            string tempPdfPath = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"KetQua_{maKQ}_{DateTime.Now:yyyyMMddHHmmss}.pdf");
            CreatePdfFromHtml(tempPdfPath);
            return tempPdfPath;
        }

        // ================== XUẤT VÀ GỬI EMAIL ==================

        private async void ExportAndSendEmail()
        {
            Form loadingForm = null;
            string tempPdfPath = null;

            try
            {
                if (ketQuaFull == null || ketQuaFull.Header == null)
                {
                    MessageBox.Show("Chưa có dữ liệu để xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string emailKhachHang = await LayEmailKhachHangAsync();

                if (string.IsNullOrEmpty(emailKhachHang))
                {
                    MessageBox.Show("⚠️ Không tìm thấy email khách hàng!\n\nVui lòng kiểm tra:\n• Thông tin khách hàng đã được cập nhật email chưa\n• Email khách hàng có đúng định dạng không",
                        "Thiếu thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var confirm = MessageBox.Show(
                    $"📧 Gửi báo cáo kết quả đến email khách hàng?\n\n📋 Mã kết quả: {maKQ}\n👤 Khách hàng: {ketQuaFull.Header.TenKhachHang ?? "N/A"}\n📧 Email: {emailKhachHang}\n📅 Đợt quan trắc: {ketQuaFull.Header.DotQuanTrac ?? "N/A"}\n\nFile báo cáo PDF sẽ được gửi kèm theo email.",
                    "Xác nhận gửi email", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes) return;

                loadingForm = ShowLoadingForm();
                tempPdfPath = CreateTempPdf();

                bool emailSent = await emailService.GuiEmailBaoCaoPdfAsync(
                    emailKhachHang,
                    ketQuaFull.Header.TenKhachHang ?? "Quý khách hàng",
                    maKQ,
                    ketQuaFull.Header.DotQuanTrac ?? "Quan trắc môi trường",
                    tempPdfPath);

                loadingForm?.Close();
                loadingForm = null;

                if (emailSent)
                {
                    var result = MessageBox.Show(
                        $"✅ Gửi email thành công!\n\n📧 Email đã được gửi đến: {emailKhachHang}\n📋 Mã kết quả: {maKQ}\n📎 File báo cáo PDF đã được đính kèm\n\nBạn có muốn lưu file báo cáo vào máy không?",
                        "Thành công", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        using var sfd = new SaveFileDialog
                        {
                            Title = "Lưu báo cáo PDF",
                            Filter = "PDF files (*.pdf)|*.pdf",
                            FileName = $"KetQua_{maKQ}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf",
                            DefaultExt = "pdf",
                            RestoreDirectory = true
                        };

                        if (sfd.ShowDialog(this) == DialogResult.OK)
                        {
                            File.Copy(tempPdfPath, sfd.FileName, true);
                            MessageBox.Show($"✓ Đã lưu file tại:\n{sfd.FileName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("❌ Gửi email thất bại!\n\nVui lòng kiểm tra:\n• Kết nối internet\n• Cấu hình email server\n• Email khách hàng có đúng không",
                        "Lỗi gửi email", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                loadingForm?.Close();
                MessageBox.Show($"❌ Lỗi khi gửi email:\n\n{ex.Message}\n\nChi tiết: {ex.StackTrace}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                try { if (!string.IsNullOrEmpty(tempPdfPath) && File.Exists(tempPdfPath)) File.Delete(tempPdfPath); } catch { }
            }
        }

        private async Task<string> LayEmailKhachHangAsync()
        {
            try
            {
                if (!string.IsNullOrEmpty(ketQuaFull.Header.TenKhachHang))
                {
                    var khachHangBLL = new KhachHangBLL();
                    string email = await Task.Run(() => khachHangBLL.layEmailKhachHang(ketQuaFull.Header.TenKhachHang));
                    if (!string.IsNullOrEmpty(email)) return email;

                    var danhSachKH = await Task.Run(() => khachHangBLL.layDanhSachKH());
                    var khachHang = danhSachKH.FirstOrDefault(kh =>
                        kh.tenDoanhNghiep != null &&
                        kh.tenDoanhNghiep.Trim().Equals(ketQuaFull.Header.TenKhachHang.Trim(), StringComparison.OrdinalIgnoreCase));

                    if (khachHang != null && !string.IsNullOrEmpty(khachHang.emailDoanhNghiep))
                        return khachHang.emailDoanhNghiep;
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi lấy email khách hàng: {ex.Message}");
                return null;
            }
        }

        private Form ShowLoadingForm()
        {
            var loadingForm = new Form
            {
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.None,
                BackColor = System.Drawing.Color.White,
                ShowInTaskbar = false,
                TopMost = true
            };

            var panel = new Panel { Dock = DockStyle.Fill, BackColor = System.Drawing.Color.White, Padding = new Padding(20) };

            var lblLoading = new Label
            {
                Text = "📧 Đang tạo PDF và gửi email...",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = System.Drawing.Color.FromArgb(0, 123, 255),
                AutoSize = false,
                Size = new Size(360, 40),
                Location = new DrawingPoint(20, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblDetail = new Label
            {
                Text = "Vui lòng đợi trong giây lát...",
                Font = new Font("Segoe UI", 10F),
                ForeColor = System.Drawing.Color.Gray,
                AutoSize = false,
                Size = new Size(360, 30),
                Location = new DrawingPoint(20, 100),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var progressBar = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30,
                Size = new Size(360, 25),
                Location = new DrawingPoint(20, 140)
            };

            panel.Controls.Add(lblLoading);
            panel.Controls.Add(lblDetail);
            panel.Controls.Add(progressBar);
            loadingForm.Controls.Add(panel);

            loadingForm.Paint += (s, ev) =>
            {
                ev.Graphics.DrawRectangle(new Pen(System.Drawing.Color.FromArgb(0, 123, 255), 2), 0, 0, loadingForm.Width - 1, loadingForm.Height - 1);
            };

            loadingForm.Show(this);
            Application.DoEvents();
            return loadingForm;
        }

        private string GenerateHtmlReport()
        {
            var sb = new StringBuilder();

            string logoBase64 = "";
            try
            {
                string logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "logo.png");

                if (!File.Exists(logoPath))
                    logoPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logo.png");

                if (File.Exists(logoPath))
                {
                    byte[] logoBytes = File.ReadAllBytes(logoPath);
                    logoBase64 = Convert.ToBase64String(logoBytes);
                }
                else
                {
                    var resources = new System.ComponentModel.ComponentResourceManager(typeof(ChiTietKetQua));
                    object logoObj = resources.GetObject("Logo");
                    if (logoObj is Image logoImg)
                    {
                        using (var ms = new MemoryStream())
                        {
                            logoImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            logoBase64 = Convert.ToBase64String(ms.ToArray());
                        }
                    }
                }
            }
            catch { logoBase64 = ""; }

            string logoDataUri = !string.IsNullOrEmpty(logoBase64) ? $"data:image/png;base64,{logoBase64}" : "";

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html>");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset='UTF-8'/>");
            sb.AppendLine("<style>");
            sb.AppendLine(@"
        html, body {
            margin: 0;
            padding: 0;
        }
        
        body { 
            font-family: Arial, sans-serif; 
            font-size: 12pt; 
            padding: 20px;
            position: relative;
            min-height: 100%;
        }
        
        /* ========== WRAPPER ĐỂ CHỨA NỘI DUNG ========== */
        .content-wrapper {
            position: relative;
            z-index: 1;
        }
        
        /* ========== WATERMARK - OPACITY 12% - ĐÈ LÊN TẤT CẢ ========== */
        .watermark {
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 550px;
            height: 550px;
            opacity: 0.12;
            z-index: 9999;
            pointer-events: none;
        }
        
        /* ========== HEADER VỚI LOGO ========== */
        .header { 
            position: relative;
            text-align: center; 
            border-bottom: 2px solid #000; 
            padding-bottom: 10px; 
            margin-bottom: 15px;
            min-height: 70px;
            padding-left: 90px;
        }
        
        .logo {
            position: absolute;
            left: 0;
            top: 0;
            width: 75px;
            height: 75px;
        }
        
        .logo img {
            width: 100%;
            height: 100%;
            object-fit: contain;
        }
        
        .company { font-weight: bold; font-size: 11pt; }
        .address { font-size: 10pt; font-style: italic; }
        
        h1 { 
            text-align: center; 
            font-size: 16pt; 
            margin-bottom: 10px;
        }
        
        h2 { 
            font-size: 13pt; 
            margin-top: 15px; 
            margin-bottom: 8px; 
            border-bottom: 1px solid #000;
        }
        
        table { 
            width: 100%; 
            border-collapse: collapse; 
            margin: 10px 0; 
            position: relative;
        }
        
        th, td { 
            border: 1px solid #000; 
            padding: 5px; 
            font-size: 10pt; 
        }
        
        th { 
            background-color: #e0e0e0; 
            font-weight: bold; 
            text-align: center; 
        }
        
        /* ========== CẢI THIỆN HIỂN THỊ CỘT KẾT QUẢ ========== */
        td.result-cell {
            text-align: center;
            font-weight: bold;
        }
        
        .info-table td { 
            border: none; 
            padding: 3px 5px; 
        }
        
        .info-table td:first-child { 
            font-weight: bold; 
            width: 150px; 
        }
        
        .sample-title { 
            background-color: #f0f0f0; 
            font-weight: bold; 
            padding: 8px; 
            margin-top: 15px;
        }
        
        .sample-info { 
            font-size: 10pt; 
            padding: 5px; 
            background-color: #f8f8f8;
        }
        
        .signature { 
            margin-top: 30px;
        }
        
        .sig-row { 
            display: table; 
            width: 100%; 
        }
        
        .sig-box { 
            display: table-cell; 
            width: 50%; 
            text-align: center; 
            padding-top: 50px; 
        }
        
        .footer { 
            text-align: center; 
            font-size: 9pt; 
            margin-top: 20px; 
            border-top: 1px solid #000; 
            padding-top: 10px;
        }
    ");
            sb.AppendLine("</style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            if (!string.IsNullOrEmpty(logoDataUri))
            {
                sb.AppendLine($"<img class='watermark' src='{logoDataUri}' alt=''/>");
            }

            sb.AppendLine("<div class='content-wrapper'>");

            sb.AppendLine("<div class='header'>");
            if (!string.IsNullOrEmpty(logoDataUri))
            {
                sb.AppendLine($"<div class='logo'><img src='{logoDataUri}' alt='Logo'/></div>");
            }
            sb.AppendLine("<div class='company'>CÔNG TY QUẢN LÝ HỢP ĐỒNG QUAN TRẮC MÔI TRƯỜNG ECOS</div>");
            sb.AppendLine("<div class='address'>Địa chỉ: 19 Nguyễn Hữu Thọ, phường Tân Hưng, TPHCM</div>");
            sb.AppendLine("<div class='address'>ĐT: 1900 1234 - Email: ecos@gmail.com</div>");
            sb.AppendLine("</div>");

            sb.AppendLine("<h1>PHIẾU KẾT QUẢ THỬ NGHIỆM</h1>");
            sb.AppendLine($"<p style='text-align:center; font-style:italic;'>{Escape(ketQuaFull.Header.DotQuanTrac ?? "Kết quả quan trắc")}</p>");
            sb.AppendLine($"<p style='text-align:center;'>Số: {Escape(ketQuaFull.Header.MaKQ)}/KQ-{DateTime.Now:yyyy}</p>");

            sb.AppendLine("<h2>I. THÔNG TIN CHUNG</h2>");
            sb.AppendLine("<table class='info-table'>");
            sb.AppendLine($"<tr><td>Tên khách hàng</td><td>: {Escape(ketQuaFull.Header.TenKhachHang ?? "[Chưa xác định]")}</td></tr>");
            sb.AppendLine($"<tr><td>Địa chỉ</td><td>: {Escape(ketQuaFull.Header.DiaChiKhachHang ?? "[Chưa xác định]")}</td></tr>");
            sb.AppendLine($"<tr><td>Địa điểm quan trắc</td><td>: {Escape(ketQuaFull.Header.DiaDiemQuanTrac ?? "[Chưa xác định]")}</td></tr>");
            sb.AppendLine($"<tr><td>Mã kết quả</td><td>: <b>{Escape(ketQuaFull.Header.MaKQ)}</b></td></tr>");
            sb.AppendLine($"<tr><td>Ngày quan trắc</td><td>: {ketQuaFull.Header.NgayTao:dd/MM/yyyy}</td></tr>");
            sb.AppendLine($"<tr><td>Ngày trả kết quả</td><td>: {(ketQuaFull.Header.NgayTraKQ?.ToString("dd/MM/yyyy") ?? "Chưa xác định")}</td></tr>");
            if (!string.IsNullOrEmpty(ketQuaFull.Header.GhiChu))
                sb.AppendLine($"<tr><td>Ghi chú</td><td>: {Escape(ketQuaFull.Header.GhiChu)}</td></tr>");
            sb.AppendLine("</table>");

            sb.AppendLine("<h2>II. KẾT QUẢ</h2>");

            int idx = 0;
            foreach (var nenMau in ketQuaFull.DanhSachNenMau)
            {
                idx++;
                sb.AppendLine($"<div class='sample-title'>NỀN MẪU {idx}: {Escape(nenMau.TenNenMau?.ToUpper() ?? "N/A")} ({Escape(nenMau.MaNen)})</div>");
                sb.AppendLine($"<div class='sample-info'>Vị trí: {Escape(nenMau.ViTri ?? "Chưa xác định")} | Tọa độ: {Escape(nenMau.ToaDo ?? "Chưa xác định")}</div>");

                if (nenMau.DanhSachThongSo != null && nenMau.DanhSachThongSo.Count > 0)
                {
                    sb.AppendLine("<table>");
                    sb.AppendLine("<tr><th>TT</th><th>Thông số</th><th>Đơn vị</th><th>Phương pháp</th><th>Kết quả</th><th>QCVN</th></tr>");

                    int stt = 0;
                    foreach (var ts in nenMau.DanhSachThongSo)
                    {
                        stt++;

                        sb.AppendLine("<tr>");
                        sb.AppendLine($"<td style='text-align:center;'>{stt}</td>");
                        sb.AppendLine($"<td>{Escape(ts.TenTS ?? "")}</td>");
                        sb.AppendLine($"<td style='text-align:center;'>{Escape(ts.DonVi ?? "-")}</td>");
                        sb.AppendLine($"<td>{Escape(ts.PhuongPhapPhanTich ?? "")}</td>");
                        sb.AppendLine($"<td class='result-cell'>{ts.KetQua:N2}</td>");
                        sb.AppendLine($"<td style='text-align:center;'>{Escape(ts.QCVN ?? "KQĐ")}</td>");
                        sb.AppendLine("</tr>");
                    }
                    sb.AppendLine("</table>");
                }
            }

            sb.AppendLine("<p style='font-size:9pt; margin-top:15px;'><i>* Phương pháp phân tích: theo TCVN, SMEWW. Các giá trị in đậm là vượt quy chuẩn.</i></p>");

            sb.AppendLine("<div class='signature'>");
            sb.AppendLine("<div class='sig-row'>");
            sb.AppendLine($"<div class='sig-box'><b>NGƯỜI LẬP PHIẾU</b><br/><i>(Ký, họ tên)</i><br/><br/><br/>{Escape(ketQuaFull.Header.TenNhanVien ?? "")}</div>");
            sb.AppendLine("<div class='sig-box'><b>TRƯỞNG PHÒNG THÍ NGHIỆM</b><br/><i>(Ký, đóng dấu)</i><br/><br/><br/></div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");

            sb.AppendLine($"<div class='footer'>Ngày in: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</div>");

            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }

        private string Escape(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";
            return System.Net.WebUtility.HtmlEncode(text);
        }
    }
}
