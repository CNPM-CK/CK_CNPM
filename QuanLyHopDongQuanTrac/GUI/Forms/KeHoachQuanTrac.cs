using BLL;
using DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class KeHoachQuanTrac : Form
    {
        private BLL_DotQuanTrac bllDotQuanTrac;
        private NenMauBLL bllNenMau;
        private bool taiNenMau = false;
        private string nenMauDuocChon = "";
        public string MaDotHienTai { get; set; }

        public bool dangChinhSua = false;

        public KeHoachQuanTrac()
        {
            InitializeComponent();
            bllNenMau = new NenMauBLL();
            bllDotQuanTrac = new BLL_DotQuanTrac();
        }

        #region Custom TextBox và Label cho Form Nhân viên
        private void InitializeButtonStyles()
        {

            BoGocButton(btnThemnenmau, 25);
            BoGocButton(btnLuu, 25);

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

        private void ApplyRoundedInput(Panel panel, Control ctrl, int borderRadius, int borderSize, Color borderColor)
        {
            // Gỡ event cũ (tránh vẽ chồng)
            panel.Paint -= Panel_Paint;
            panel.Resize -= Panel_Resize;

            // Cài đặt nền và kiểu cho control
            panel.BackColor = Color.White;
            ctrl.BackColor = Color.White;

            if (ctrl is TextBox txt)
            {
                txt.BorderStyle = BorderStyle.None;
            }
            else if (ctrl is ComboBox cbo)
            {
                cbo.FlatStyle = FlatStyle.Flat;
                if (cbo.DropDownStyle != ComboBoxStyle.DropDown)
                    cbo.DropDownStyle = ComboBoxStyle.DropDown;
            }

            // Căn chỉnh vị trí & kích thước control con trong panel
            ctrl.Location = new Point(borderSize + 5, (panel.Height - ctrl.Height) / 2);
            ctrl.Width = panel.Width - (borderSize + 5) * 2;
            ctrl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            // Hàm vẽ bo tròn
            void Panel_Paint(object s, PaintEventArgs e)
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                e.Graphics.CompositingQuality = CompositingQuality.HighQuality;

                using (GraphicsPath path = CreateRoundedPath(panel.ClientRectangle, borderRadius))
                {
                    using (SolidBrush brush = new SolidBrush(panel.BackColor))
                        e.Graphics.FillPath(brush, path);

                    if (borderSize > 0)
                    {
                        using (GraphicsPath borderPath = CreateRoundedPath(
                            new Rectangle(
                                borderSize / 2,
                                borderSize / 2,
                                panel.Width - borderSize,
                                panel.Height - borderSize
                            ),
                            borderRadius))
                        {
                            using (Pen pen = new Pen(borderColor, borderSize))
                            {
                                pen.Alignment = System.Drawing.Drawing2D.PenAlignment.Inset;
                                e.Graphics.DrawPath(pen, borderPath);
                            }
                        }
                    }
                }
            }

            void Panel_Resize(object s, EventArgs e)
            {
                ctrl.Location = new Point(borderSize + 5, (panel.Height - ctrl.Height) / 2);
                ctrl.Width = panel.Width - (borderSize + 5) * 2;
                using (GraphicsPath path = CreateRoundedPath(panel.ClientRectangle, borderRadius))
                {
                    panel.Region = new Region(path);
                }
                panel.Invalidate();

            }

            panel.Paint += Panel_Paint;
            panel.Resize += Panel_Resize;

            using (GraphicsPath path = CreateRoundedPath(panel.ClientRectangle, borderRadius))
                panel.Region = new Region(path);

            panel.Invalidate();
        }

        private GraphicsPath CreateRoundedPath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();

            if (rect.Width <= 0 || rect.Height <= 0)
                return path;

            //int diameter = radius * 2;
            int diameter = Math.Min(radius * 2, Math.Min(rect.Width, rect.Height));
            // Đảm bảo radius không lớn hơn kích thước
            diameter = Math.Min(diameter, Math.Min(rect.Width, rect.Height));

            Rectangle arc = new Rectangle(rect.Location, new Size(diameter, diameter));

            // Top left
            path.AddArc(arc, 180, 90);

            // Top right
            arc.X = rect.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Bottom right
            arc.Y = rect.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Bottom left
            arc.X = rect.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }
        #endregion



        private void KeHoachQuanTrac_Load(object sender, EventArgs e)
        {
            ApplyRoundedInput(panelDotqt, txtDotqt, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelHopdong, cboHopdong, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelNoidung, txtNoidung, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTrangthai, cboTrangthai, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelNenmau, cboNenmau, 12, 2, Color.FromArgb(0, 152, 70));

            InitializeButtonStyles();
            taiDanhSachHopDong();
            taiDanhSachNenMau();
            taiTrangThai();

            dtmBegin.ValueChanged += dtmBegin_ValueChanged;
            dtmDukien.Value = dtmBegin.Value.AddDays(20);

            if (!dangChinhSua)
            {
                dtmDukien.Value = dtmBegin.Value.AddDays(20);
            }

            if (dangChinhSua)
            {
                this.Text = "Chỉnh sửa kế hoạch";
                btnLuu.Text = "Cập nhật";
                label.Text = "Chỉnh sửa kế hoạch";
                dtmEnd.Enabled = true;
                dtmEnd.Visible = true;

            }
        }


        // Trong KeHoachQuanTrac.cs
        public void taiDuLieuChinhsua(string maDot)
        {
            try
            {
                this.MaDotHienTai = maDot;
                this.dangChinhSua = true;
                this.daTaoDot = true;

                // ✅ Gọi BLL
                var chiTiet = bllDotQuanTrac.LayChiTietDotQuanTrac(maDot);

                if (chiTiet == null || chiTiet.ThongTinDot == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin đợt quan trắc!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // ✅ Load thông tin đợt
                txtDotqt.Text = chiTiet.ThongTinDot.DotQuanTrac;
                txtNoidung.Text = chiTiet.ThongTinDot.NoiDung;
                dtmBegin.Value = chiTiet.ThongTinDot.NgayBatDau;
                dtmDukien.Value = chiTiet.ThongTinDot.NgayDuKien;

                if (chiTiet.ThongTinDot.NgayTraKQ.HasValue)
                {
                    dtmEnd.Checked = true;
                    dtmEnd.Value = chiTiet.ThongTinDot.NgayTraKQ.Value;
                }

                cboHopdong.SelectedValue = chiTiet.ThongTinDot.MaHD;
                cboTrangthai.SelectedValue = int.Parse(chiTiet.ThongTinDot.TrangThai);

                // ✅ Load nền mẫu
                flowNenmau.Controls.Clear();

                if (chiTiet.DanhSachNenMau != null && chiTiet.DanhSachNenMau.Count > 0)
                {
                    foreach (var nenMau in chiTiet.DanhSachNenMau)
                    {
                        var uc = new NenMauConTrol();

                        uc.taiNenMau(
                            maDN: nenMau.MaDN,
                            maNen: nenMau.MaNen,
                            tenNenMau: nenMau.TenNenMau,
                            moTaNen: nenMau.MoTaNen,
                            chiTiet: nenMau.DanhSachThongSo,
                            viTri: nenMau.TenViTri,
                            toaDo: nenMau.ToaDo,
                            ghiChu: nenMau.GhiChu
                        );
                        //uc.Invoke((MethodInvoker)(() => uc.Refresh()));
                        uc.nhanXoaNenMau += nhanXoaNenMauUC;
                        uc.nhanSuaNenMau += nhanSuaNenMauUC;
                        uc.Width = flowNenmau.Width - 20;

                        flowNenmau.Controls.Add(uc);
                        uc.Refresh();
                    }

                    capNhatSoThuTuNenMau();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu chỉnh sửa:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void taiDanhSachHopDong()
        {
            try
            {
                List<HopDongVaTenDN> dsHopDong = bllDotQuanTrac.layDanhSachHopDong();

                if (dsHopDong == null || dsHopDong.Count == 0)
                {
                    MessageBox.Show("Không có hợp đồng nào để lập kế hoạch quan trắc!\n" +
                        "Vui lòng tạo hợp đồng trước.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cboHopdong.Enabled = false;
                    btnLuu.Enabled = false;
                    btnThemnenmau.Enabled = false;
                    return;
                }

                cboHopdong.DataSource = dsHopDong;
                cboHopdong.DropDownStyle = ComboBoxStyle.DropDownList;
                cboHopdong.DisplayMember = "maHDVaKH";
                cboHopdong.ValueMember = "maHD";
                cboHopdong.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách hợp đồng:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void taiDanhSachNenMau()
        {
            taiNenMau = true;
            try
            {
                List<NenMau> dsNenmau = bllNenMau.layDSNenMau();

                if (dsNenmau == null || dsNenmau.Count == 0)
                {
                    MessageBox.Show("Không có hợp đồng nào để lập kế hoạch quan trắc!\n" +
                        "Vui lòng tạo hợp đồng trước.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    cboHopdong.Enabled = false;
                    btnLuu.Enabled = false;
                    btnThemnenmau.Enabled = false;
                    return;
                }

                cboNenmau.DataSource = dsNenmau;
                cboNenmau.DropDownStyle = ComboBoxStyle.DropDownList;
                cboNenmau.DisplayMember = "DisplayText";
                cboNenmau.ValueMember = "tenNenMau";
                cboNenmau.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách nền mẫu :\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                taiNenMau = false;
            }
        }


        private void taiTrangThai()
        {
            var bll = new BLL_DotQuanTrac();
            DataTable dt = bll.LayDanhSachTrangThai();

            cboTrangthai.DataSource = dt;
            cboTrangthai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangthai.DisplayMember = "tenTrangThai";
            cboTrangthai.ValueMember = "maTrangThai";
            cboTrangthai.SelectedIndex = 0;
        }

        //KeHoachQuanTrac
        private void nhanSuaNenMauUC(object sender, EventArgs e)
        {
            if (sender is NenMauConTrol uc)
            {
                ChiTietNenMau frmChiTiet = new ChiTietNenMau();
                frmChiTiet.StartPosition = FormStartPosition.CenterParent;

                // ✅ Set thông tin cơ bản - SỬA LẠI
                frmChiTiet.MaDN = uc.MaDN;  // ✅ ĐÚNG - Dùng MaDN từ UserControl
                frmChiTiet.TenNenMauDaChon = uc.TenNenMau;
                frmChiTiet.MaNen = uc.MaNen;
                frmChiTiet.chinhSua = true;

                // ✅ Set dữ liệu trước khi show form
                frmChiTiet.SetDataForEdit(
                    tenViTri: uc.TenViTri ?? "",   // ✅ Thêm null-check
                    toaDo: uc.ToaDo ?? "",
                    ghiChu: uc.GhiChu ?? "",
                    danhSach: uc.GetDanhSachThongSo()
                );

                if (frmChiTiet.ShowDialog(this) == DialogResult.OK)
                {
                    // ✅ Reload lại UserControl - THÊM PARAMETER MaDN
                    uc.taiNenMau(
                        maDN: uc.MaDN,              // ✅ THÊM PARAMETER NÀY
                        maNen: uc.MaNen,
                        tenNenMau: frmChiTiet.TenNenMauDaChon,
                        moTaNen: frmChiTiet.MoTaNen,
                        chiTiet: frmChiTiet.ChiTietDaChon,
                        viTri: frmChiTiet.TenViTri,
                        toaDo: frmChiTiet.ToaDo,
                        ghiChu: frmChiTiet.GhiChu
                    );
                    uc.Invoke((MethodInvoker)(() => uc.Refresh()));
                    MessageBox.Show("Cập nhật nền mẫu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void nhanXoaNenMauUC(object sender, EventArgs e)
        {
            if (sender is NenMauConTrol uc)
            {
                DialogResult dr = MessageBox.Show(
                    "Bạn có chắc muốn xóa nền mẫu này không?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    flowNenmau.Controls.Remove(uc);
                    uc.Dispose();
                    capNhatSoThuTuNenMau();
                }
            }

        }


        private void capNhatSoThuTuNenMau()
        {
            for (int i = 0; i < flowNenmau.Controls.Count; i++)
            {
                if (flowNenmau.Controls[i] is NenMauConTrol uc)
                {
                    uc.SetIndex(i + 1);
                }
            }
        }


        private void KeHoachQuanTrac_Resize(object sender, EventArgs e)
        {
            foreach (Control ctrl in flowNenmau.Controls)
            {
                ctrl.Width = flowNenmau.ClientSize.Width - 20;
            }
        }


        private void btnThemnenmau_Click_2(object sender, EventArgs e)
        {
            ThemNenMau1 add = new ThemNenMau1();
            if (add.ShowDialog() == DialogResult.OK)
            {
                taiDanhSachNenMau();
            }
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                if (cboHopdong.SelectedValue == null)
                {
                    MessageBox.Show("Vui lòng chọn hợp đồng!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDotqt.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên đợt quan trắc!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDotqt.Focus();
                    return;
                }

                if (dtmDukien.Value.Date < dtmBegin.Value.Date)
                {
                    MessageBox.Show("Ngày dự kiến phải lớn hơn hoặc bằng ngày bắt đầu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (flowNenmau.Controls.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một nền mẫu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                foreach (Control control in flowNenmau.Controls)
                {
                    if (control is NenMauConTrol ucNenMau)
                    {
                        string maDN = ucNenMau.MaNen;

                        if (string.IsNullOrWhiteSpace(maDN))
                        {
                            MessageBox.Show("Có nền mẫu chưa được lưu vào database!\n" +
                                "Vui lòng xóa và thêm lại nền mẫu.", "Cảnh báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                    }
                }


                // ✅ Xác nhận
                string tieuDe = dangChinhSua ? "Xác nhận cập nhật" : "Xác nhận lưu";
                string thongDiep = dangChinhSua ? "cập nhật" : "lưu";

                DialogResult dr = MessageBox.Show(
                    $"Bạn có chắc muốn {thongDiep} kế hoạch này không?",
                    tieuDe, MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr != DialogResult.Yes)
                    return;

                btnLuu.Enabled = false;
                Cursor = Cursors.WaitCursor;


                DTO_DotQuanTrac dto = new DTO_DotQuanTrac
                {
                    MaDot = this.MaDotHienTai,
                    MaHD = cboHopdong.SelectedValue.ToString(),
                    DotQuanTrac = txtDotqt.Text.Trim(),
                    NoiDung = txtNoidung.Text.Trim(),
                    NgayBatDau = dtmBegin.Value.Date,
                    NgayDuKien = dtmDukien.Value.Date,
                    NgayTraKQ = dtmEnd.Checked ? (DateTime?)dtmEnd.Value.Date : null,
                    TrangThai = cboTrangthai.SelectedValue.ToString()
                };

                string thongTinXacNhan = $"Bạn có chắc muốn hoàn tất kế hoạch quan trắc?\n\n" +
                    $"Hợp đồng: {cboHopdong.Text}\n" +
                    $"Đợt quan trắc: {dto.DotQuanTrac}\n" +
                    $"Ngày bắt đầu: {dto.NgayBatDau:dd/MM/yyyy}\n" +
                    $"Ngày dự kiến: {dto.NgayDuKien:dd/MM/yyyy}\n" +
                    $"Số lượng nền mẫu: {flowNenmau.Controls.Count}";



                BLL_DotQuanTrac bllDotQuanTrac = new BLL_DotQuanTrac();
                bool ketQua = bllDotQuanTrac.hoanTatKeHoachQuanTrac(dto);

                //if (ketQua)
                //{
                //    MessageBox.Show("Hoàn tất kế hoạch quan trắc thành công!", "Thành công",
                //        MessageBoxButtons.OK, MessageBoxIcon.Information);

                //    this.DialogResult = DialogResult.OK;
                //    this.Close();
                //}
                if (ketQua)
                {
                    string thongBao = dangChinhSua
                        ? "Cập nhật kế hoạch thành công!"
                        : "Lưu kế hoạch thành công!";

                    MessageBox.Show(thongBao, "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể lưu kế hoạch. Vui lòng thử lại!",
                        "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu kế hoạch quan trắc:\n{ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnLuu.Enabled = true;
                //btnThemnenmau.Enabled = true;
                Cursor = Cursors.Default;
            }
        }


        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }

        private void label3_Click(object sender, EventArgs e) { }

        private void flowNenmau_Paint(object sender, PaintEventArgs e) { }

        private void panel11_Resize(object sender, EventArgs e) { }

        private void cboNenmau_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (taiNenMau || cboNenmau.SelectedIndex == -1) return;
            if (!(cboNenmau.SelectedItem is NenMau nenChon)) return;

            try
            {
                // ✅ Tạo Dot_Nen TRƯỚC để lấy maDN
                var bll = new BLL_DotQuanTrac();
                var dn = bll.themNenMauVaoDot(MaDotHienTai, nenChon.maNen);

                if (dn == null)
                {
                    MessageBox.Show("Không tạo được nền mẫu!");
                    return;
                }

                // ✅ Mở form chi tiết
                using (var f = new ChiTietNenMau())
                {
                    f.StartPosition = FormStartPosition.CenterParent;
                    f.MaDN = dn.MaDN;        // lấy từ DB
                    f.MaNen = dn.MaNen;
                    f.TenNenMauDaChon = nenChon.tenNenMau;
                    f.chinhSua = false;

                    if (f.ShowDialog(this) == DialogResult.OK)
                    {
                        // ✅ Lưu chi tiết vào DB
                        bll.luuChiTietNenMau(dn.MaDN, f.TenViTri, f.ToaDo, f.GhiChu, f.ChiTietDaChon);

                        // ✅ Hiển thị lên UI
                        var uc = new NenMauConTrol();
                        uc.taiNenMau(dn.MaDN, dn.MaNen, nenChon.tenNenMau, f.MoTaNen, f.ChiTietDaChon, f.TenViTri, f.ToaDo, f.GhiChu);
                        uc.nhanXoaNenMau += nhanXoaNenMauUC;
                        uc.nhanSuaNenMau += nhanSuaNenMauUC;
                        uc.Width = flowNenmau.Width - 20;

                        flowNenmau.Controls.Add(uc);
                        capNhatSoThuTuNenMau();
                    }
                    else
                    {
                        // ❗ User bấm Hủy → phải xóa Dot_Nen vừa tạo
                        bll.xoaNenMauKhoiDot(dn.MaDN);
                    }
                }
            }
            finally
            {
                taiNenMau = true;
                cboNenmau.SelectedIndex = -1;
                taiNenMau = false;
            }

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cboHopdong_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private bool daTaoDot = false;

        private void KeHoachQuanTrac_FormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!daTaoDot) return;

            //// Nếu chưa lưu hoàn tất thì xóa nháp
            //if (this.DialogResult != DialogResult.OK)
            //{
            //    bllDotQuanTrac.xoaDotQuanTrac(MaDotHienTai);
            //}
            // ✅ CHỈ XÓA nếu là THÊM MỚI và chưa lưu
            if (!daTaoDot)
                return; // Chưa tạo gì cả

            // ✅ Nếu đang CHỈNH SỬA → KHÔNG BAO GIỜ XÓA
            if (dangChinhSua)
                return;

            // ✅ Nếu là THÊM MỚI và user hủy → Xóa nháp
            if (this.DialogResult != DialogResult.OK && !string.IsNullOrEmpty(MaDotHienTai))
            {
                try
                {
                    bllDotQuanTrac.xoaDotQuanTrac(MaDotHienTai);
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng không hiện MessageBox (form đang đóng)
                    System.Diagnostics.Debug.WriteLine($"Lỗi xóa nháp: {ex.Message}");
                }
            }
        }

        private void dtmBegin_ValueChanged(object sender, EventArgs e)
        {
            dtmDukien.Value = dtmBegin.Value.AddDays(20);
        }
    }
}
