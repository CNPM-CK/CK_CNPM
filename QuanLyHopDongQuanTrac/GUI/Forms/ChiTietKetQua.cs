using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                try { this.btnXuatFile.Click -= btnXuatFile_Click_1; } catch { }
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

                panelNenMau.Location = new Point(15, headerHeight);
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
                txtMaKQ.BackColor = Color.FromArgb(230, 240, 255);
                txtMaKQ.Font = boldFont;
                txtMaKQ.ForeColor = Color.FromArgb(0, 102, 204);
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
                txtNhanVienNhap.BackColor = Color.FromArgb(245, 255, 250);
                txtNhanVienNhap.Font = textFont;
                txtNhanVienNhap.ReadOnly = true;
            }

            if (txtTrangThai != null)
            {
                string trangThai = daXacNhan ? "✅ Đã xác nhận" : "⏳ Chờ xác nhận";
                txtTrangThai.Text = trangThai;
                txtTrangThai.BackColor = daXacNhan ? Color.FromArgb(200, 255, 200) : Color.FromArgb(255, 245, 200);
                txtTrangThai.ForeColor = daXacNhan ? Color.FromArgb(0, 128, 0) : Color.FromArgb(204, 136, 0);
                txtTrangThai.Font = boldFont;
                txtTrangThai.TextAlign = HorizontalAlignment.Center;
                txtTrangThai.ReadOnly = true;
            }

            if (txtGhiChu != null)
            {
                txtGhiChu.Text = ketQuaFull.Header.GhiChu ?? "";
                txtGhiChu.BackColor = Color.FromArgb(255, 255, 240);
                txtGhiChu.Font = new Font("Segoe UI", 10F, FontStyle.Italic);
                txtGhiChu.ForeColor = Color.FromArgb(108, 117, 125);
                txtGhiChu.ReadOnly = true;
            }

            if (txtNenMau != null)
            {
                int soNenMau = ketQuaFull.DanhSachNenMau.Count;
                string dotText = ketQuaFull.Header.DotQuanTrac ?? "Chưa xác định";
                txtNenMau.Text = $"📊 {dotText} ({soNenMau} nền mẫu)";
                txtNenMau.BackColor = Color.FromArgb(240, 255, 245);
                txtNenMau.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                txtNenMau.ForeColor = Color.FromArgb(0, 152, 70);
                txtNenMau.ReadOnly = true;
            }

            if (btnXacNhan != null)
            {
                btnXacNhan.Enabled = !daXacNhan;
                btnXacNhan.BackColor = !daXacNhan ? Color.FromArgb(0, 152, 70) : Color.Gray;
                btnXacNhan.ForeColor = Color.White;
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
                btnHuyXacNhan.BackColor = daXacNhan ? Color.FromArgb(220, 53, 69) : Color.Gray;
                btnHuyXacNhan.ForeColor = Color.White;
                btnHuyXacNhan.FlatStyle = FlatStyle.Flat;
                btnHuyXacNhan.FlatAppearance.BorderSize = 0;
                btnHuyXacNhan.Cursor = daXacNhan ? Cursors.Hand : Cursors.Default;
                btnHuyXacNhan.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
                btnHuyXacNhan.Text = "✗ Hủy Xác Nhận";
                ApplyRoundedCorners(btnHuyXacNhan, 12);
            }

            if (btnXuatFile != null)
            {
                btnXuatFile.BackColor = Color.FromArgb(0, 123, 255);
                btnXuatFile.ForeColor = Color.White;
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
                    Location = new Point(15, headerHeight),
                    Size = new Size(this.ClientSize.Width - 30, availableHeight),
                    BackColor = Color.FromArgb(240, 242, 245)
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
                    Location = new Point(10, yPosition),
                    Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                    ForeColor = Color.White,
                    Padding = new Padding(3),
                    BackColor = Color.White,
                    FlatStyle = FlatStyle.Flat
                };

                var headerPanel = new Panel
                {
                    Height = 60,
                    Dock = DockStyle.Top,
                    BackColor = Color.FromArgb(0, 152, 70)
                };

                var lblHeader = new Label
                {
                    Text = $"   📋 NỀN MẪU {groupIndex}: {nenMau.TenNenMau?.ToUpper() ?? "N/A"} ({nenMau.MaNen})  •  Mã KQ Nền: {nenMau.MaKQNen}",
                    Font = new Font("Segoe UI", 12F, FontStyle.Bold),
                    ForeColor = Color.White,
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
                    BackColor = Color.FromArgb(230, 245, 255),
                    Padding = new Padding(15, 12, 15, 12)
                };

                string viTriText = !string.IsNullOrEmpty(nenMau.ViTri) ? nenMau.ViTri : "Chưa xác định vị trí";
                string toaDoText = !string.IsNullOrEmpty(nenMau.ToaDo) ? nenMau.ToaDo : "Chưa xác định tọa độ";
                int soThongSo = nenMau.DanhSachThongSo?.Count ?? 0;

                var lblInfo = new Label
                {
                    Text = $"📍 Vị trí: {viTriText}     •     🗺️ Tọa độ: {toaDoText}     •     📊 Số thông số: {soThongSo}",
                    Font = new Font("Segoe UI", 10.2F),
                    ForeColor = Color.FromArgb(0, 102, 204),
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
                    BackColor = Color.White
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
                BackgroundColor = Color.White,
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
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.Black,
                Font = new Font("Segoe UI", 10.2F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = Color.FromArgb(200, 200, 200),
                Padding = new Padding(5),
                WrapMode = DataGridViewTriState.True  
            };

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                BackColor = Color.White,
                ForeColor = Color.Black,
                SelectionBackColor = Color.FromArgb(200, 200, 200),
                SelectionForeColor = Color.Black,
                Padding = new Padding(6),
                WrapMode = DataGridViewTriState.False
            };

            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                SelectionBackColor = Color.FromArgb(200, 200, 200),
                SelectionForeColor = Color.Black,
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.Black
            };

            int containerWidth = grpWidth - 50;

            // 1. STT - 4%
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STT",
                HeaderText = "STT",
                Width = (int)(containerWidth * 0.04),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });

            // 2. Thông Số Phân Tích - 18%
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenTS",
                HeaderText = "Thông Số Phân Tích",
                DataPropertyName = "TenTS",
                Width = (int)(containerWidth * 0.18),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });

            // 3. Đơn Vị - 7% 
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DonVi",
                HeaderText = "Đơn Vị",
                DataPropertyName = "DonVi",
                Width = (int)(containerWidth * 0.07),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    ForeColor = Color.Black,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular)
                }
            });

            // 4. Phương Pháp Phân Tích - 21% 
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhuongPhapPhanTich",
                HeaderText = "Phương Pháp Phân Tích",
                DataPropertyName = "PhuongPhapPhanTich",
                Width = (int)(containerWidth * 0.21),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });

            // 5. Kết Quả Đo - 11%
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "KetQua",
                HeaderText = "Kết Quả Đo",
                DataPropertyName = "KetQua",
                Width = (int)(containerWidth * 0.11),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Format = "N2"
                }
            });

            // 6. Giới Hạn Phát Hiện - 14%
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GioiHanPhatHien",
                HeaderText = "Giới Hạn Phát Hiện",
                DataPropertyName = "GioiHanPhatHien",
                Width = (int)(containerWidth * 0.14),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });

            // 7. QCVN - 12%
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "QCVN",
                HeaderText = "QCVN 40:2011/BTNMT Cột B",
                DataPropertyName = "QCVN",
                Width = (int)(containerWidth * 0.12),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });

            // 8. Đánh Giá - 13%
            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TinhTrang",
                HeaderText = "Đánh Giá",
                DataPropertyName = "TinhTrang",
                Width = (int)(containerWidth * 0.13),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                    ForeColor = Color.Black
                }
            });


            // Load dữ liệu
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
                    row.Cells["TinhTrang"].Value = thongSo.TinhTrang ?? "";

                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Style.ForeColor = Color.Black;
                        cell.Style.BackColor = Color.White;
                    }
                }
            }

            return dgv;
        }

        private void ResizeDataGridViewColumns(DataGridView dgv)
        {
            if (dgv == null || dgv.Columns.Count == 0) return;
            dgv.Width = dgv.Parent.Width - 10;
        }

        private void RepositionButtons()
        {
            if (btnHuyXacNhan == null || btnXacNhan == null || btnXuatFile == null) return;

            int buttonY = this.ClientSize.Height - 75;
            int buttonHeight = 52;
            int totalWidth = this.ClientSize.Width;

            btnHuyXacNhan.Location = new Point(100, buttonY);
            btnHuyXacNhan.Size = new Size(190, buttonHeight);

            int centerX = (totalWidth - 190) / 2;
            btnXacNhan.Location = new Point(centerX, buttonY);
            btnXacNhan.Size = new Size(190, buttonHeight);

            btnXuatFile.Location = new Point(totalWidth - 290, buttonY);
            btnXuatFile.Size = new Size(190, buttonHeight);
        }

        private void ApplyRoundedCorners(Button button, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            Rectangle rect = new Rectangle(0, 0, button.Width, button.Height);

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
                "⚠️ Xác nhận kết quả",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                var (success, message) = ketQuaBLL.CapNhatTrangThaiKetQua(maKQ, true);
                if (success)
                {
                    MessageBox.Show("✓ " + message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    daThayDoiTrangThai = true;
                    LoadChiTietKetQua();
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
                "⚠️ Hủy xác nhận",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirm == DialogResult.Yes)
            {
                var (success, message) = ketQuaBLL.CapNhatTrangThaiKetQua(maKQ, false);
                if (success)
                {
                    MessageBox.Show("✓ " + message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    daThayDoiTrangThai = true;
                    LoadChiTietKetQua();
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

        private void btnXuatFile_Click_1(object sender, EventArgs e)
        {
            ShowExportMenu();
        }

        private void ShowExportMenu()
        {
            try
            {
                // Tạo form menu tùy chọn
                using (var menuForm = new Form())
                {
                    menuForm.Text = "Chọn hành động";
                    menuForm.Size = new Size(500, 300);
                    menuForm.StartPosition = FormStartPosition.CenterParent;
                    menuForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                    menuForm.MaximizeBox = false;
                    menuForm.MinimizeBox = false;
                    menuForm.BackColor = Color.White;

                    // Label tiêu đề
                    var lblTitle = new Label
                    {
                        Text = "📊 Xuất Báo Cáo Kết Quả",
                        Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                        ForeColor = Color.FromArgb(0, 102, 204),
                        AutoSize = false,
                        Size = new Size(460, 40),
                        Location = new Point(20, 20),
                        TextAlign = ContentAlignment.MiddleCenter
                    };
                    menuForm.Controls.Add(lblTitle);

                    // Label thông tin
                    var lblInfo = new Label
                    {
                        Text = $"Mã kết quả: {maKQ}\nVui lòng chọn hành động:",
                        Font = new Font("Segoe UI", 10F),
                        ForeColor = Color.FromArgb(60, 60, 60),
                        AutoSize = false,
                        Size = new Size(460, 50),
                        Location = new Point(20, 70),
                        TextAlign = ContentAlignment.TopLeft
                    };
                    menuForm.Controls.Add(lblInfo);

                    // Nút "Xuất File PDF"
                    var btnExportPDF = new Button
                    {
                        Text = "📄 Xuất File PDF",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        Size = new Size(460, 50),
                        Location = new Point(20, 130),
                        BackColor = Color.FromArgb(0, 123, 255),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Tag = "export"
                    };
                    btnExportPDF.FlatAppearance.BorderSize = 0;
                    btnExportPDF.Click += (s, e) =>
                    {
                        menuForm.DialogResult = DialogResult.OK;
                        menuForm.Tag = "export";
                        menuForm.Close();
                    };
                    menuForm.Controls.Add(btnExportPDF);

                    // Nút "Xuất và Gửi Email"
                    var btnExportAndEmail = new Button
                    {
                        Text = "📧 Xuất và Gửi Email cho Khách Hàng",
                        Font = new Font("Segoe UI", 11F, FontStyle.Bold),
                        Size = new Size(460, 50),
                        Location = new Point(20, 190),
                        BackColor = Color.FromArgb(40, 167, 69),
                        ForeColor = Color.White,
                        FlatStyle = FlatStyle.Flat,
                        Cursor = Cursors.Hand,
                        Tag = "email"
                    };
                    btnExportAndEmail.FlatAppearance.BorderSize = 0;
                    btnExportAndEmail.Click += (s, e) =>
                    {
                        menuForm.DialogResult = DialogResult.OK;
                        menuForm.Tag = "email";
                        menuForm.Close();
                    };
                    menuForm.Controls.Add(btnExportAndEmail);

                    // Hiển thị menu và xử lý
                    if (menuForm.ShowDialog(this) == DialogResult.OK)
                    {
                        string action = menuForm.Tag?.ToString();

                        if (action == "export")
                        {
                            ExportPdfReport();
                        }
                        else if (action == "email")
                        {
                            ExportAndSendEmail();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== XUẤT PDF ==================

        private void ExportPdfReport()
        {
            try
            {
                if (ketQuaFull == null || ketQuaFull.Header == null)
                {
                    MessageBox.Show("Chưa có dữ liệu để xuất.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Tạo HTML content
                string htmlContent = GenerateHtmlReport();

                // Lưu file HTML tạm
                string tempHtmlPath = Path.Combine(Path.GetTempPath(), $"KetQua_{maKQ}_{DateTime.Now:yyyyMMddHHmmss}.html");
                File.WriteAllText(tempHtmlPath, htmlContent, Encoding.UTF8);

                // Hiển thị SaveFileDialog
                using var sfd = new SaveFileDialog
                {
                    Title = "Lưu báo cáo",
                    Filter = "HTML files (*.html)|*.html|PDF files (*.pdf)|*.pdf|All files (*.*)|*.*",
                    FilterIndex = 1,
                    FileName = $"KetQua_{maKQ}_{DateTime.Now:yyyyMMdd_HHmmss}.html",
                    DefaultExt = "html",
                    AddExtension = true,
                    RestoreDirectory = true
                };

                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    // Copy file HTML
                    File.Copy(tempHtmlPath, sfd.FileName, true);

                    var result = MessageBox.Show(
                        $"✓ Xuất file thành công!\n\n" +
                        $"Đường dẫn: {sfd.FileName}\n\n" +
                        $"Bạn có muốn mở file ngay không?\n\n" +
                        $"💡 Gợi ý: Mở file bằng trình duyệt và sử dụng chức năng Print to PDF để tạo file PDF.",
                        "Thành công",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );

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
                            MessageBox.Show($"Không thể mở file:\n{ex.Message}", "Lỗi",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }

                // Xóa file tạm
                try { File.Delete(tempHtmlPath); } catch { }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi xuất file:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ================== XUẤT VÀ GỬI EMAIL ==================

        private async void ExportAndSendEmail()
        {
            Form loadingForm = null;
            try
            {
                if (ketQuaFull == null || ketQuaFull.Header == null)
                {
                    MessageBox.Show("Chưa có dữ liệu để xuất.", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string emailKhachHang = ketQuaFull.Header.EmailKhachHang;
                string tenKhachHang = ketQuaFull.Header.TenKhachHang ?? "Quý khách hàng";

                if (string.IsNullOrEmpty(emailKhachHang))
                {
                    MessageBox.Show(
                        "⚠️ Không tìm thấy email khách hàng!\n\n" +
                        "Vui lòng kiểm tra:\n" +
                        "• Thông tin khách hàng đã được cập nhật email chưa\n" +
                        "• Email khách hàng có đúng định dạng không",
                        "Thiếu thông tin",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                // Xác nhận trước khi gửi
                var confirm = MessageBox.Show(
                    $"📧 Gửi báo cáo kết quả đến email khách hàng?\n\n" +
                    $"📋 Mã kết quả: {maKQ}\n" +
                    $"👤 Khách hàng: {ketQuaFull.Header.TenKhachHang ?? "N/A"}\n" +
                    $"📧 Email: {emailKhachHang}\n" +
                    $"📅 Đợt quan trắc: {ketQuaFull.Header.DotQuanTrac ?? "N/A"}\n\n" +
                    $"File báo cáo HTML sẽ được gửi kèm theo email.",
                    "Xác nhận gửi email",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (confirm != DialogResult.Yes)
                    return;

                // Hiển thị form loading
                loadingForm = ShowLoadingForm();

                // Tạo HTML content
                string htmlContent = GenerateHtmlReport();

                // Lưu file HTML tạm
                string tempHtmlPath = Path.Combine(Path.GetTempPath(), $"KetQua_{maKQ}_{DateTime.Now:yyyyMMddHHmmss}.html");
                File.WriteAllText(tempHtmlPath, htmlContent, Encoding.UTF8);

                // Gửi email
                bool emailSent = await emailService.GuiEmailBaoCaoAsync(
                    emailKhachHang,
                    ketQuaFull.Header.TenKhachHang ?? "Quý khách hàng",
                    maKQ,
                    ketQuaFull.Header.DotQuanTrac ?? "Quan trắc môi trường",
                    tempHtmlPath
                );

                // Đóng loading form
                loadingForm?.Close();
                loadingForm = null;

                if (emailSent)
                {
                    var result = MessageBox.Show(
                        $"✅ Gửi email thành công!\n\n" +
                        $"📧 Email đã được gửi đến: {emailKhachHang}\n" +
                        $"📋 Mã kết quả: {maKQ}\n" +
                        $"📎 File báo cáo đã được đính kèm\n\n" +
                        $"Bạn có muốn lưu file báo cáo vào máy không?",
                        "Thành công",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information
                    );

                    if (result == DialogResult.Yes)
                    {
                        // Lưu file vào máy
                        using var sfd = new SaveFileDialog
                        {
                            Title = "Lưu báo cáo",
                            Filter = "HTML files (*.html)|*.html",
                            FileName = $"KetQua_{maKQ}_{DateTime.Now:yyyyMMdd_HHmmss}.html",
                            DefaultExt = "html",
                            RestoreDirectory = true
                        };

                        if (sfd.ShowDialog(this) == DialogResult.OK)
                        {
                            File.Copy(tempHtmlPath, sfd.FileName, true);
                            MessageBox.Show($"✓ Đã lưu file tại:\n{sfd.FileName}", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    MessageBox.Show(
                        "❌ Gửi email thất bại!\n\n" +
                        "Vui lòng kiểm tra:\n" +
                        "• Kết nối internet\n" +
                        "• Cấu hình email server\n" +
                        "• Email khách hàng có đúng không\n\n" +
                        "Bạn có thể thử xuất file PDF và gửi thủ công.",
                        "Lỗi gửi email",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }

                // Xóa file tạm
                try { File.Delete(tempHtmlPath); } catch { }
            }
            catch (Exception ex)
            {
                loadingForm?.Close();
                MessageBox.Show(
                    $"❌ Lỗi khi gửi email:\n\n{ex.Message}\n\n" +
                    $"Chi tiết: {ex.StackTrace}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }


        private async Task<string> LayEmailKhachHangAsync()
        {
            try
            {
                // ✅ LẤY TRỰC TIẾP TỪ HEADER (đã được load từ stored procedure)
                if (ketQuaFull?.Header != null && !string.IsNullOrEmpty(ketQuaFull.Header.EmailKhachHang))
                {
                    return ketQuaFull.Header.EmailKhachHang;
                }

                System.Diagnostics.Debug.WriteLine("Email khách hàng không có trong Header");
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
                BackColor = Color.White,
                ShowInTaskbar = false,
                TopMost = true
            };

            var panel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White,
                Padding = new Padding(20)
            };

            var lblLoading = new Label
            {
                Text = "📧 Đang gửi email...",
                Font = new Font("Segoe UI", 14F, FontStyle.Bold),
                ForeColor = Color.FromArgb(0, 123, 255),
                AutoSize = false,
                Size = new Size(360, 40),
                Location = new Point(20, 50),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var lblDetail = new Label
            {
                Text = "Vui lòng đợi trong giây lát...",
                Font = new Font("Segoe UI", 10F),
                ForeColor = Color.Gray,
                AutoSize = false,
                Size = new Size(360, 30),
                Location = new Point(20, 100),
                TextAlign = ContentAlignment.MiddleCenter
            };

            var progressBar = new ProgressBar
            {
                Style = ProgressBarStyle.Marquee,
                MarqueeAnimationSpeed = 30,
                Size = new Size(360, 25),
                Location = new Point(20, 140)
            };

            panel.Controls.Add(lblLoading);
            panel.Controls.Add(lblDetail);
            panel.Controls.Add(progressBar);
            loadingForm.Controls.Add(panel);

            // Border cho form
            loadingForm.Paint += (s, e) =>
            {
                e.Graphics.DrawRectangle(new Pen(Color.FromArgb(0, 123, 255), 2),
                    0, 0, loadingForm.Width - 1, loadingForm.Height - 1);
            };

            loadingForm.Show(this);
            Application.DoEvents();

            return loadingForm;
        }

        // ================== TẠO HTML REPORT ==================

        private string GenerateHtmlReport()
        {
            var sb = new StringBuilder();

            // ====== Đọc logo từ ChiTietKetQua.resx và convert sang Base64 ======
            string logoDataUri = "";
            try
            {
                var resources = new System.ComponentModel.ComponentResourceManager(typeof(ChiTietKetQua));
                object logoObj = resources.GetObject("Logo");
                if (logoObj is Image logoImg)
                {
                    using (var ms = new MemoryStream())
                    {
                        logoImg.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        string logoBase64 = Convert.ToBase64String(ms.ToArray());
                        logoDataUri = $"data:image/png;base64,{logoBase64}";
                    }
                }
            }
            catch
            {
                logoDataUri = "";
            }

            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang='vi'>");
            sb.AppendLine("<head>");
            sb.AppendLine("    <meta charset='UTF-8'>");
            sb.AppendLine("    <meta name='viewport' content='width=device-width, initial-scale=1.0'>");
            sb.AppendLine($"    <title>Phiếu kết quả thử nghiệm - {ketQuaFull.Header.MaKQ}</title>");
            sb.AppendLine("    <style>");
            sb.AppendLine(@"
        @page {
            size: A4;
            margin: 15mm;
        }
        
        * {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
            -webkit-print-color-adjust: exact;
            print-color-adjust: exact;
        }
        
        body {
            font-family: 'Times New Roman', Times, serif;
            font-size: 13pt;
            line-height: 1.5;
            color: #000;
            background: #fff;
            padding: 0;
        }
        
        .container {
            width: 180mm;
            min-height: 257mm;
            margin: 0 auto;
            padding: 10mm 0;
            background: transparent;
            position: relative;
            z-index: 0;
        }
        
        .header-org {
            position: relative;
            text-align: center;
            margin-bottom: 15px;
            border-bottom: 2px solid #000;
            padding-bottom: 10px;
            padding-top: 10px;
        }
        
        .header-logo {
            position: absolute;
            left: 0;
            top: 5px;
            width: 90px;
            height: 90px;
        }
        
        .header-logo img {
            width: 100%;
            height: 100%;
            object-fit: contain;
        }
        
        .header-org .org-name {
            font-size: 11pt;
            font-weight: bold;
            text-transform: uppercase;
            margin-bottom: 3px;
        }
        
        .header-org .org-address {
            font-size: 10pt;
            font-style: italic;
            margin-bottom: 3px;
        }
        
        .watermark {
            position: fixed;
            top: 55%;
            left: 50%;
            transform: translate(-50%, -50%);
            width: 450px;
            height: 450px;
            opacity: 0.10;
            z-index: 5;
            pointer-events: none;
        }
        
        .watermark img {
            width: 100%;
            height: 100%;
            object-fit: contain;
        }
        
        .doc-title {
            text-align: center;
            margin: 20px 0 15px 0;
        }
        
        .doc-title h1 {
            font-size: 14pt;
            font-weight: bold;
            text-transform: uppercase;
            margin-bottom: 8px;
            letter-spacing: 0.5px;
        }
        
        .doc-title .subtitle {
            font-size: 12pt;
            font-weight: normal;
            font-style: italic;
            margin-bottom: 3px;
        }
        
        .doc-title .doc-number {
            font-size: 11pt;
            font-weight: normal;
            margin-top: 5px;
        }
        
        .section-title {
            font-size: 12pt;
            font-weight: bold;
            text-transform: uppercase;
            margin: 15px 0 8px 0;
            border-bottom: 1px solid #000;
            padding-bottom: 3px;
        }
        
        .info-table {
            width: 100%;
            margin-bottom: 15px;
            border-collapse: collapse;
        }
        
        .info-table td {
            padding: 4px 8px;
            vertical-align: top;
            font-size: 12pt;
        }
        
        .info-table td:first-child {
            width: 180px;
            font-weight: bold;
        }
        
        .info-table td:nth-child(2) {
            width: 10px;
        }
        
        .sample-block {
            border: 1px solid #000;
            margin: 12px 0 15px 0;
            padding-bottom: 5px;
            page-break-inside: avoid;
        }
        
        .sample-header {
            font-size: 12pt;
            font-weight: bold;
            padding: 6px 8px;
            background: #e0e0e0;
        }
        
        .sample-info {
            font-size: 11pt;
            margin: 0;
            padding: 5px 8px;
        }
        
        table.result-table {
            width: 100%;
            border-collapse: collapse;
            margin: 5px 0 15px 0;
            page-break-inside: avoid;
        }
        
        table.result-table th,
        table.result-table td {
            border: 0.75pt solid #000;
            padding: 6px 4px;
            text-align: center;
            font-size: 10pt;
        }
        
        table.result-table thead th {
            background: #e0e0e0;
            font-weight: bold;
            text-align: center;
            vertical-align: middle;
        }
        
        table.result-table tbody td {
            font-weight: normal;
        }
        
        table.result-table .text-left {
            text-align: left;
        }
        
        table.result-table .text-right {
            text-align: right;
        }
        
        table.result-table .text-center {
            text-align: center;
        }
        
        .signature-section {
            margin-top: 30px;
            page-break-inside: avoid;
        }
        
        .signature-row {
            display: flex;
            justify-content: space-between;
            margin-bottom: 80px;
        }
        
        .signature-box {
            width: 48%;
            text-align: center;
        }
        
        .signature-box .role {
            font-weight: bold;
            font-size: 12pt;
            margin-bottom: 5px;
        }
        
        .signature-box .instruction {
            font-style: italic;
            font-size: 10pt;
            margin-bottom: 60px;
        }
        
        .signature-box .name {
            font-weight: bold;
            font-size: 12pt;
        }
        
        .footer {
            margin-top: 20px;
            padding-top: 10px;
            border-top: 1px solid #000;
            text-align: center;
            font-size: 10pt;
            font-style: italic;
        }
        
        @media print {
            body {
                margin: 0;
                padding: 0;
            }
            
            .container {
                margin: 0 auto;
                padding: 10mm 0;
                box-shadow: none;
                background: transparent;
                width: 180mm;
            }

            .watermark {
                position: fixed;
                top: 55%;
                left: 50%;
                transform: translate(-50%, -50%);
                width: 450px;
                height: 450px;
                opacity: 0.10;
                z-index: 5;
                pointer-events: none;
            }

            .sample-block {
                page-break-inside: avoid;
            }
            
            table.result-table {
                page-break-inside: avoid;
            }
            
            .signature-section {
                page-break-inside: avoid;
            }
        }
    ");
            sb.AppendLine("    </style>");
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");

            // Watermark logo
            sb.AppendLine("    <div class='watermark'>");
            sb.AppendLine($"        <img src='{logoDataUri}' alt='Watermark'>");
            sb.AppendLine("    </div>");

            sb.AppendLine("    <div class='container'>");

            // Header
            sb.AppendLine("        <div class='header-org'>");
            sb.AppendLine("            <div class='header-logo'>");
            sb.AppendLine($"                <img src='{logoDataUri}' alt='Logo ECOS'>");
            sb.AppendLine("            </div>");
            sb.AppendLine("            <div class='org-name'>CÔNG TY QUẢN LÝ HỢP ĐỒNG QUAN TRẮC MÔI TRƯỜNG ECOS</div>");
            sb.AppendLine("            <div class='org-address'>Địa chỉ: 19 Nguyễn Hữu Thọ, phường Tân Hưng, TPHCM </div>");
            sb.AppendLine("            <div class='org-address'>ĐT: 1900 1234 - Email: ecos@gmail.com</div>");
            sb.AppendLine("        </div>");

            // Tiêu đề
            sb.AppendLine("        <div class='doc-title'>");
            sb.AppendLine("            <h1>PHIẾU KẾT QUẢ THỬ NGHIỆM</h1>");
            sb.AppendLine($"            <div class='subtitle'>{ketQuaFull.Header.DotQuanTrac ?? "Kết quả quan trắc"}</div>");
            sb.AppendLine($"            <div class='doc-number'>Số: {ketQuaFull.Header.MaKQ}/KQ-{DateTime.Now:yyyy}</div>");
            sb.AppendLine("        </div>");

            // I. THÔNG TIN CHUNG
            sb.AppendLine("        <div class='section-title'>I. THÔNG TIN CHUNG</div>");
            sb.AppendLine("        <table class='info-table'>");

            string tenKhachHang = !string.IsNullOrEmpty(ketQuaFull.Header.TenKhachHang)
                ? ketQuaFull.Header.TenKhachHang
                : "[Chưa xác định]";
            string diaChiKH = !string.IsNullOrEmpty(ketQuaFull.Header.DiaChiKhachHang)
                ? ketQuaFull.Header.DiaChiKhachHang
                : "[Chưa xác định]";
            string diaDiemQuanTrac = !string.IsNullOrEmpty(ketQuaFull.Header.DiaDiemQuanTrac)
                ? ketQuaFull.Header.DiaDiemQuanTrac
                : "[Chưa xác định]";

            sb.AppendLine($"            <tr><td>Tên khách hàng</td><td>:</td><td>{tenKhachHang}</td></tr>");
            sb.AppendLine($"            <tr><td>Địa chỉ</td><td>:</td><td>{diaChiKH}</td></tr>");
            sb.AppendLine($"            <tr><td>Địa điểm quan trắc</td><td>:</td><td>{diaDiemQuanTrac}</td></tr>");
            sb.AppendLine($"            <tr><td>Loại mẫu</td><td>:</td><td>Nước thải</td></tr>");
            sb.AppendLine($"            <tr><td>Mã kết quả</td><td>:</td><td><strong>{ketQuaFull.Header.MaKQ}</strong></td></tr>");
            sb.AppendLine($"            <tr><td>Ngày quan trắc</td><td>:</td><td>{ketQuaFull.Header.NgayTao:dd/MM/yyyy}</td></tr>");
            sb.AppendLine($"            <tr><td>Ngày phân tích</td><td>:</td><td>{ketQuaFull.Header.NgayTao:dd/MM/yyyy} đến {(ketQuaFull.Header.NgayTraKQ?.ToString("dd/MM/yyyy") ?? "...")}</td></tr>");
            sb.AppendLine($"            <tr><td>Ngày trả kết quả</td><td>:</td><td>{(ketQuaFull.Header.NgayTraKQ?.ToString("dd/MM/yyyy") ?? "Chưa xác định")}</td></tr>");

            if (!string.IsNullOrEmpty(ketQuaFull.Header.GhiChu))
            {
                sb.AppendLine($"            <tr><td>Ghi chú</td><td>:</td><td>{ketQuaFull.Header.GhiChu}</td></tr>");
            }
            sb.AppendLine("        </table>");

            // II. KẾT QUẢ
            sb.AppendLine("        <div class='section-title'>II. KẾT QUẢ</div>");

            int sampleIndex = 0;
            foreach (var nenMau in ketQuaFull.DanhSachNenMau)
            {
                sampleIndex++;

                sb.AppendLine("        <div class='sample-block'>");
                sb.AppendLine($"            <div class='sample-header'>NỀN MẪU {sampleIndex}: {nenMau.TenNenMau?.ToUpper() ?? "N/A"} ({nenMau.MaNen}) - Mã: {nenMau.MaKQNen}</div>");

                string viTriText = !string.IsNullOrEmpty(nenMau.ViTri) ? nenMau.ViTri : "Chưa xác định";
                string toaDoText = !string.IsNullOrEmpty(nenMau.ToaDo) ? nenMau.ToaDo : "Chưa xác định";

                sb.AppendLine($"            <div class='sample-info'>Vị trí: {viTriText} | Tọa độ: {toaDoText} | Số thông số: {nenMau.DanhSachThongSo?.Count ?? 0}</div>");

                if (nenMau.DanhSachThongSo != null && nenMau.DanhSachThongSo.Count > 0)
                {
                    sb.AppendLine("            <table class='result-table'>");
                    sb.AppendLine("                <thead>");
                    sb.AppendLine("                    <tr>");
                    sb.AppendLine("                        <th style='width: 35px;'>TT</th>");
                    sb.AppendLine("                        <th style='width: 180px;'>Thông số phân tích</th>");
                    sb.AppendLine("                        <th style='width: 60px;'>Đơn vị</th>");
                    sb.AppendLine("                        <th style='width: 150px;'>Phương pháp phân tích</th>");
                    sb.AppendLine("                        <th style='width: 80px;'>Kết quả</th>");
                    sb.AppendLine("                        <th style='width: 110px;'>QCVN 40:2011/BTNMT Cột B</th>");
                    sb.AppendLine("                        <th style='width: 100px;'>Tình trạng</th>");
                    sb.AppendLine("                    </tr>");
                    sb.AppendLine("                </thead>");
                    sb.AppendLine("                <tbody>");

                    int stt = 0;
                    foreach (var thongSo in nenMau.DanhSachThongSo)
                    {
                        stt++;
                        sb.AppendLine("                    <tr>");
                        sb.AppendLine($"                        <td class='text-center'>{stt}</td>");
                        sb.AppendLine($"                        <td class='text-left'>{thongSo.TenTS ?? ""}</td>");
                        sb.AppendLine($"                        <td class='text-center'>{thongSo.DonVi ?? "-"}</td>");
                        sb.AppendLine($"                        <td class='text-left'>{thongSo.PhuongPhapPhanTich ?? ""}</td>");
                        sb.AppendLine($"                        <td class='text-right'><strong>{thongSo.KetQua:N2}</strong></td>");
                        sb.AppendLine($"                        <td class='text-center'>{thongSo.QCVN ?? "Không quy định"}</td>");

                        string tinhTrang = thongSo.TinhTrang ?? "";
                        string tinhTrangDisplay = tinhTrang;

                        if (!string.IsNullOrEmpty(tinhTrang) && tinhTrang.Contains("Vượt"))
                            tinhTrangDisplay = $"<strong>{tinhTrang}</strong>";

                        sb.AppendLine($"                        <td class='text-center'>{tinhTrangDisplay}</td>");
                        sb.AppendLine("                    </tr>");
                    }

                    sb.AppendLine("                </tbody>");
                    sb.AppendLine("            </table>");
                }

                sb.AppendLine("        </div>");
            }

            // Ghi chú chung
            sb.AppendLine("        <div style='margin-top: 15px; font-size: 10pt;'>");
            sb.AppendLine("            <p><em>* Phương pháp phân tích: sử dụng các phương pháp chuẩn theo TCVN, SMEWW</em></p>");
            sb.AppendLine("            <p><em>* Các giá trị ghi đậm là giá trị vượt quy chuẩn cho phép</em></p>");
            sb.AppendLine("            <p><em>* KPH: Không phát hiện (giá trị thấp hơn giới hạn phát hiện)</em></p>");
            sb.AppendLine("            <p><em>* LOD: Giới hạn phát hiện (Limit of Detection)</em></p>");
            sb.AppendLine("        </div>");

            // Chữ ký
            sb.AppendLine("        <div class='signature-section'>");
            sb.AppendLine("            <div class='signature-row'>");
            sb.AppendLine("                <div class='signature-box'>");
            sb.AppendLine("                    <div class='role'>NGƯỜI LẬP PHIẾU</div>");
            sb.AppendLine("                    <div class='instruction'>(Ký, họ tên)</div>");
            sb.AppendLine($"                    <div class='name'>{ketQuaFull.Header.TenNhanVien ?? ""}</div>");
            sb.AppendLine("                </div>");
            sb.AppendLine("                <div class='signature-box'>");
            sb.AppendLine("                    <div class='role'>TRƯỞNG PHÒNG THÍ NGHIỆM</div>");
            sb.AppendLine("                    <div class='instruction'>(Ký, đóng dấu, họ tên)</div>");
            sb.AppendLine("                    <div class='name'>[Tên trưởng phòng]</div>");
            sb.AppendLine("                </div>");
            sb.AppendLine("            </div>");
            sb.AppendLine("        </div>");

            // Footer
            sb.AppendLine("        <div class='footer'>");
            sb.AppendLine($"            <p>Ngày in: {DateTime.Now:dd/MM/yyyy HH:mm:ss}</p>");
            sb.AppendLine("            <p>Phiếu này là bản sao không có giá trị pháp lý</p>");
            sb.AppendLine("        </div>");

            sb.AppendLine("    </div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            return sb.ToString();
        }
    }
}