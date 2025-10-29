using BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DTO;

namespace GUI.Forms
{
    public partial class KeHoachQuanTrac : Form
    {
        private BLL_DotQuanTrac bllDotQuanTrac;
        private NenMauBLL bllNenMau;
        private bool isLoadingComboBox = false;
        private string lastSelectedMaNen = "";
        public string MaDotHienTai { get; set; }

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
            LoadDanhSachHopDong();
            LoadDanhSachNenMau();
            LoadComboBoxTrangThai();
        }

        private void LoadDanhSachHopDong()
        {
            try
            {
                List<HopDong> dsHopDong = bllDotQuanTrac.LayDanhSachHopDong();

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
                cboHopdong.DisplayMember = "DisplayText";
                cboHopdong.ValueMember = "MaHD";
                cboHopdong.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách hợp đồng:\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void LoadDanhSachNenMau()
        {
            isLoadingComboBox = true;
            try
            {
                List<NenMau> dsNenmau = bllNenMau.LayDSNenMau();

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
                isLoadingComboBox = false;
            }
        }


        private void LoadComboBoxTrangThai()
        {
            var bll = new BLL_DotQuanTrac();
            DataTable dt = bll.LayDanhSachTrangThai();

            cboTrangthai.DataSource = dt;
            cboTrangthai.DisplayMember = "tenTrangThai";
            cboTrangthai.ValueMember = "maTrangThai";
            cboTrangthai.SelectedIndex = 0;
        }

        //KeHoachQuanTrac
        private void Uc_SuaNenMauClicked(object sender, EventArgs e)
        {
            if (sender is NenMauConTrol uc)
            {
                ChiTietNenMau frmChiTiet = new ChiTietNenMau();
                frmChiTiet.StartPosition = FormStartPosition.CenterParent;

                // ✅ Set thông tin cơ bản - SỬA LẠI
                frmChiTiet.MaDN = uc.MaDN;  // ✅ ĐÚNG - Dùng MaDN từ UserControl
                frmChiTiet.TenNenMauDaChon = uc.TenNenMau;
                frmChiTiet.MaNen = uc.MaNen;
                frmChiTiet.IsEditMode = true;

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
                    uc.LoadNenMau(
                        maDN: uc.MaDN,              // ✅ THÊM PARAMETER NÀY
                        maNen: uc.MaNen,
                        tenNenMau: frmChiTiet.TenNenMauDaChon,
                        moTaNen: frmChiTiet.MoTaNen,
                        chiTiet: frmChiTiet.ChiTietDaChon,
                        viTri: frmChiTiet.TenViTri,
                        toaDo: frmChiTiet.ToaDo,
                        ghiChu: frmChiTiet.GhiChu
                    );

                    MessageBox.Show("Cập nhật nền mẫu thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void Uc_XoaNenMauClicked(object sender, EventArgs e)
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
                    CapNhatSoThuTuNenMau();
                }
            }
        }


        private void CapNhatSoThuTuNenMau()
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
            add.ShowDialog();
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

                        // Kiểm tra nền mẫu đã có thông tin chi tiết chưa
                        //var bll = new BLL_DotQuanTrac();
                        //var dotNen = bll.LayThongTinDotNen(maDN);

                        //if (dotNen == null || string.IsNullOrWhiteSpace(dotNen.TenViTri))
                        //{
                        //    MessageBox.Show($"Nền mẫu '{ucNenMau.TenNenMau}' chưa có thông tin vị trí!\n" +
                        //        "Vui lòng nhấp đúp vào nền mẫu để nhập chi tiết.", "Cảnh báo",
                        //        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        //    return;
                        //}
                    }
                }

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

                DialogResult dr = MessageBox.Show(thongTinXacNhan, "Xác nhận",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr != DialogResult.Yes)
                    return;

                btnLuu.Enabled = false;
                btnThemnenmau.Enabled = false;
                Cursor = Cursors.WaitCursor;

                BLL_DotQuanTrac bllDotQuanTrac = new BLL_DotQuanTrac();
                bool ketQua = bllDotQuanTrac.HoanTatKeHoachQuanTrac(dto);

                if (ketQua)
                {
                    MessageBox.Show("Hoàn tất kế hoạch quan trắc thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Không thể hoàn tất kế hoạch quan trắc. Vui lòng thử lại!",
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
                btnThemnenmau.Enabled = true;
                Cursor = Cursors.Default;
            }
        }


        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e) { }

        private void label3_Click(object sender, EventArgs e) { }

        private void flowNenmau_Paint(object sender, PaintEventArgs e) { }

        private void panel11_Resize(object sender, EventArgs e) { }

        private void cboNenmau_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isLoadingComboBox || cboNenmau.SelectedIndex == -1) return;
            if (!(cboNenmau.SelectedItem is NenMau nenChon)) return;
            if (nenChon.maNen == lastSelectedMaNen) return;

            try
            {
                DialogResult result = MessageBox.Show(
                    $"Bạn có muốn thêm nền mẫu này vào kế hoạch quan trắc?\n\n" +
                    $"Tên nền mẫu: {nenChon.tenNenMau}",
                    "Xác nhận",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result != DialogResult.Yes)
                    return;

                lastSelectedMaNen = nenChon.maNen;

                // GỌI BLL để thêm vào DB trước khi mở form chi tiết
                var bll = new BLL_DotQuanTrac();
                var dn = bll.ThemNenMauVaoDot(MaDotHienTai, nenChon.maNen);

                if (dn == null)
                {
                    MessageBox.Show("Lưu nền mẫu thất bại!");
                    return;
                }

                // Mở form chi tiết để cập nhật bổ sung
                using (var f = new ChiTietNenMau())
                {
                    f.MaDN = dn.MaDN;           // ✅ Mã Dot_Nen (khóa chính bảng Dot_Nen)
                    f.MaNen = dn.MaNen;         // ✅ Mã nền mẫu (khóa ngoại từ bảng NenMau)
                    f.TenNenMauDaChon = nenChon.tenNenMau;

                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        var uc = new NenMauConTrol();

                        // ✅ SỬA LẠI - TRUYỀN ĐÚNG THỨ TỰ VÀ ĐẦY ĐỦ THAM SỐ
                        uc.LoadNenMau(
                            maDN: dn.MaDN,                      // ✅ Tham số thứ 1: mã Dot_Nen
                            maNen: dn.MaNen,                    // ✅ Tham số thứ 2: mã nền mẫu
                            tenNenMau: nenChon.tenNenMau,       // ✅ Tham số thứ 3: tên nền mẫu
                            moTaNen: f.MoTaNen,                 // ✅ Tham số thứ 4: mô tả
                            chiTiet: f.ChiTietDaChon,           // ✅ Tham số thứ 5: danh sách thông số
                            viTri: f.TenViTri,                  // ✅ Tham số thứ 6: vị trí
                            toaDo: f.ToaDo,                     // ✅ Tham số thứ 7: tọa độ
                            ghiChu: f.GhiChu                    // ✅ Tham số thứ 8: ghi chú
                        );

                        uc.XoaNenMauClicked += Uc_XoaNenMauClicked;
                        uc.SuaNenMauClicked += Uc_SuaNenMauClicked;
                        uc.Width = flowNenmau.Width - 20;

                        flowNenmau.Controls.Add(uc);
                        CapNhatSoThuTuNenMau();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message);
            }
            finally
            {
                isLoadingComboBox = true;
                cboNenmau.SelectedIndex = -1;
                lastSelectedMaNen = "";
                isLoadingComboBox = false;
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
