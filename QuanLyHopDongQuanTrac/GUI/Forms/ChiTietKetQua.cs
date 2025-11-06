using BLL;
using DTO;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Microsoft.Reporting.NETCore;

namespace GUI.Forms
{
    public partial class ChiTietKetQua : Form
    {
        private readonly string maKQ;
        private readonly KetQuaBLL ketQuaBLL = new KetQuaBLL();
        private DTO_KetQuaFull ketQuaFull;
        private Panel panelNenMau;
        private bool daThayDoiTrangThai = false;

        public bool DaThayDoiTrangThai => daThayDoiTrangThai;

        public ChiTietKetQua(string maKQ)
        {
            InitializeComponent();
            this.maKQ = maKQ;

            // lifecycle
            this.Load += ChiTietKetQua_Load;
            this.Resize += ChiTietKetQua_Resize;
            this.FormClosing += ChiTietKetQua_FormClosing;

            // CHỐT 1 handler cho nút Xuất file
            if (this.btnXuatFile != null)
            {
                try { this.btnXuatFile.Click -= btnXuatFile_Click; } catch { }
                try { this.btnXuatFile.Click -= btnXuatFile_Click_1; } catch { }
                this.btnXuatFile.Click += btnXuatFile_Click;   // chỉ dùng 1
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
                ColumnHeadersHeight = 50,
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
                BackColor = Color.FromArgb(52, 58, 64),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10.2F, FontStyle.Bold),
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                SelectionBackColor = Color.FromArgb(52, 58, 64),
                Padding = new Padding(5),
                WrapMode = DataGridViewTriState.True
            };

            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font = new Font("Segoe UI", 9.5F),
                BackColor = Color.White,
                ForeColor = Color.FromArgb(33, 37, 41),
                SelectionBackColor = Color.FromArgb(111, 207, 151),
                SelectionForeColor = Color.White,
                Padding = new Padding(6),
                WrapMode = DataGridViewTriState.False
            };

            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = Color.FromArgb(248, 249, 250),
                SelectionBackColor = Color.FromArgb(111, 207, 151),
                SelectionForeColor = Color.White
            };

            int containerWidth = grpWidth - 50;

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "STT",
                HeaderText = "STT",
                Width = (int)(containerWidth * 0.04),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.8F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 102, 204)
                }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaKQCT",
                HeaderText = "Mã KQ",
                DataPropertyName = "MaKQCT",
                Width = (int)(containerWidth * 0.07),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 8.5F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(220, 53, 69),
                    BackColor = Color.FromArgb(255, 245, 245)
                }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TenTS",
                HeaderText = "Thông Số Phân Tích",
                DataPropertyName = "TenTS",
                Width = (int)(containerWidth * 0.15),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
                }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "DonVi",
                HeaderText = "Đơn Vị",
                DataPropertyName = "DonVi",
                Width = (int)(containerWidth * 0.06),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhuongPhapPhanTich",
                HeaderText = "Phương Pháp Phân Tích",
                DataPropertyName = "PhuongPhapPhanTich",
                Width = (int)(containerWidth * 0.18),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    Font = new Font("Segoe UI", 9F)
                }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "KetQua",
                HeaderText = "Kết Quả Đo",
                DataPropertyName = "KetQua",
                Width = (int)(containerWidth * 0.10),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleRight,
                    Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(220, 53, 69),
                    Format = "N2"
                }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "GioiHanPhatHien",
                HeaderText = "Giới Hạn Phát Hiện",
                DataPropertyName = "GioiHanPhatHien",
                Width = (int)(containerWidth * 0.13),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9F)
                }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "QCVN",
                HeaderText = "QCVN 40:2011/BTNMT Cột B",
                DataPropertyName = "QCVN",
                Width = (int)(containerWidth * 0.13),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(0, 102, 204)
                }
            });

            dgv.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "TinhTrang",
                HeaderText = "Đánh Giá",
                DataPropertyName = "TinhTrang",
                Width = (int)(containerWidth * 0.14),
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = new Font("Segoe UI", 9.5F, FontStyle.Bold)
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
                    row.Cells["MaKQCT"].Value = thongSo.MaKQCT ?? "";
                    row.Cells["TenTS"].Value = thongSo.TenTS ?? "";
                    row.Cells["DonVi"].Value = thongSo.DonVi ?? "-";
                    row.Cells["PhuongPhapPhanTich"].Value = thongSo.PhuongPhapPhanTich ?? "";
                    row.Cells["KetQua"].Value = thongSo.KetQua;
                    row.Cells["GioiHanPhatHien"].Value = thongSo.GioiHanPhatHien ?? "";
                    row.Cells["QCVN"].Value = thongSo.QCVN ?? "Không quy định";
                    row.Cells["TinhTrang"].Value = thongSo.TinhTrang ?? "";

                    string tinhTrang = thongSo.TinhTrang ?? "";
                    if (tinhTrang.Contains("Vượt") || tinhTrang.Contains("vượt"))
                    {
                        row.Cells["TinhTrang"].Value = "❌ " + tinhTrang;
                        row.Cells["TinhTrang"].Style.BackColor = Color.FromArgb(255, 220, 220);
                        row.Cells["TinhTrang"].Style.ForeColor = Color.FromArgb(139, 0, 0);
                    }
                    else if (tinhTrang.Contains("Dưới") || tinhTrang.Contains("dưới"))
                    {
                        row.Cells["TinhTrang"].Value = "⚠️ " + tinhTrang;
                        row.Cells["TinhTrang"].Style.BackColor = Color.FromArgb(255, 245, 220);
                        row.Cells["TinhTrang"].Style.ForeColor = Color.FromArgb(204, 102, 0);
                    }
                    else if (tinhTrang.Contains("Đạt") || tinhTrang.Contains("đạt"))
                    {
                        row.Cells["TinhTrang"].Value = "✅ " + tinhTrang;
                        row.Cells["TinhTrang"].Style.BackColor = Color.FromArgb(220, 255, 220);
                        row.Cells["TinhTrang"].Style.ForeColor = Color.FromArgb(0, 128, 0);
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

        // ================== XỬ LÝ TRẠNG THÁI ==================

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

        // ================== XUẤT PDF ==================

        // Giữ 2 tên handler để tương thích Designer; cả 2 gọi chung
        private void btnXuatFile_Click(object sender, EventArgs e) => ExportPdfReport();
        private void btnXuatFile_Click_1(object sender, EventArgs e) => ExportPdfReport();

        private void ExportPdfReport()
        {
            try
            {
                if (ketQuaFull == null || ketQuaFull.Header == null)
                {
                    MessageBox.Show("Chưa có dữ liệu để xuất.");
                    return;
                }

                // hỗ trợ tiếng Việt
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

                // RDLC path (thử 2 khả năng)
                string rdlcPath = Path.Combine(Application.StartupPath, "GUI", "Report", "ChiTietKetQua.rdlc");
                if (!File.Exists(rdlcPath))
                    rdlcPath = Path.Combine(Application.StartupPath, "Report", "ChiTietKetQua.rdlc");

                if (!File.Exists(rdlcPath))
                {
                    MessageBox.Show("Không tìm thấy RDLC: " + rdlcPath);
                    return;
                }

                // 1) Chuẩn bị dữ liệu
                var headerData = new[]
                {
                    new {
                        ketQuaFull.Header.MaKQ,
                        ketQuaFull.Header.NgayTao,
                        ketQuaFull.Header.NgayTraKQ,
                        ketQuaFull.Header.TenNhanVien,
                        ketQuaFull.Header.TrangThai,
                        ketQuaFull.Header.GhiChu,
                        ketQuaFull.Header.DotQuanTrac,
                        ketQuaFull.Header.MaDot,
                        ketQuaFull.Header.SoNenMau
                    }
                };

                var chiTietData = ketQuaFull.DanhSachNenMau
                    .SelectMany(nm => nm.DanhSachThongSo.Select(ct => new {
                        MaKQ = ketQuaFull.Header.MaKQ,
                        MaKQNen = nm.MaKQNen,
                        TenNenMau = nm.TenNenMau,
                        ViTri = nm.ViTri,
                        ToaDo = nm.ToaDo,
                        TenTS = ct.TenTS,
                        DonVi = ct.DonVi,
                        PhuongPhapPhanTich = ct.PhuongPhapPhanTich,
                        KetQua = ct.KetQua,
                        GioiHanPhatHien = ct.GioiHanPhatHien,
                        QCVN = ct.QCVN,
                        TinhTrang = ct.TinhTrang
                    }))
                    .ToList();

                // 2) Nạp RDLC
                using var defStream = File.OpenRead(rdlcPath);
                var report = new LocalReport();
                report.LoadReportDefinition(defStream);

                report.DataSources.Clear();
                report.DataSources.Add(new ReportDataSource("dsHeader", headerData));
                report.DataSources.Add(new ReportDataSource("dsChiTiet", chiTietData));

                // 3) TRUYỀN THAM SỐ CHẮC CHẮN (mỗi cái 1 try; nếu RDLC không có thì bỏ qua)
                try
                {
                    report.SetParameters(new ReportParameter("pNgayIn", DateTime.Now.ToString("dd/MM/yyyy")));
                }
                catch { /* RDLC không có pNgayIn */ }

                try
                {
                    report.SetParameters(new ReportParameter("pTieuDe", $"PHIẾU KẾT QUẢ - {ketQuaFull.Header.MaKQ}"));
                }
                catch { /* RDLC không có pTieuDe */ }

                // 4) Render PDF
                string deviceInfo = @"<DeviceInfo>
                    <EmbedFonts>True</EmbedFonts>
                    <HumanReadablePDF>True</HumanReadablePDF>
                </DeviceInfo>";

                byte[] pdfBytes = report.Render("PDF", deviceInfo);

                // 5) Save dialog
                using var sfd = new SaveFileDialog
                {
                    Title = "Lưu báo cáo PDF",
                    Filter = "PDF files (*.pdf)|*.pdf",
                    FileName = $"KetQua_{ketQuaFull.Header.MaKQ}.pdf",
                    AddExtension = true
                };

                if (sfd.ShowDialog(this) == DialogResult.OK)
                {
                    File.WriteAllBytes(sfd.FileName, pdfBytes);
                    MessageBox.Show("Xuất PDF thành công!");
                }
            }
            catch (Exception ex)
            {
                var full = ex.ToString();
                try { Clipboard.SetText(full); } catch { }
                MessageBox.Show("Xuất PDF thất bại:\n\n" + full,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
