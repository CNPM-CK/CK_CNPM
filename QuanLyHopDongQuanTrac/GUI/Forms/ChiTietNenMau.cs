using BLL;
using DTO;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GUI.Forms
{

    public partial class ChiTietNenMau : Form
    {
        public bool IsEditMode { get; set; } = false;

        private Rectangle deleteRect;
        public List<ChiTietQuanTracView> ChiTietDaChon { get; private set; }
        public string MoTaNen { get; private set; }
        public string MaNen { get; set; }

        public string MaDN { get; set; }
        public string TenNenMauDaChon { get; set; }

        private string _tenViTriEdit;
        
        private string _toaDoEdit;
        
        private string _ghiChuEdit;

        public string TenViTri
        {
            get { return txtTenvitri.Text; }
            set { txtTenvitri.Text = value; }
        }

        public string ToaDo
        {
            get { return txtToado.Text; }
            set { txtToado.Text = value; }
        }

        public string GhiChu
        {
            get { return txtGhichu.Text; }
            set { txtGhichu.Text = value; }
        }

        private List<ChiTietQuanTracView> _danhSachChinhSua;

        private BindingList<ChiTietQuanTracView> danhSachChiTiet = new BindingList<ChiTietQuanTracView>();
        private NenMauBLL bllNenMau;
        public ChiTietNenMau()
        {
            bllNenMau = new NenMauBLL();
            InitializeComponent();
            ChiTietDaChon = new List<ChiTietQuanTracView>();
        }
        #region Custom TextBox và Label cho Form Nhân viên
        private void InitializeButtonStyles()
        {

            BoGocButton(btnThemts, 25);
            BoGocButton(btnHuy, 25);
            BoGocButton(btnLuu, 25);
            BoGocButton(btnThemthongso, 25);


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
        private void Form1_Load(object sender, EventArgs e)
        {
            ApplyRoundedInput(panelChonthongso, cboThongso, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelPhongpt, cboPhongpt, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelTennenmau, txtTennenmau, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelVitri, txtTenvitri, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelGhichu, txtGhichu, 12, 2, Color.FromArgb(0, 152, 70));
            ApplyRoundedInput(panelToado, txtToado, 12, 2, Color.FromArgb(0, 152, 70));
            InitializeButtonStyles();
            cboPhongpt.Enabled = false;

            var thongSo = new ThongSoBLL();
            var phongBan = new PhongBanBLL();
            var listPhongban = phongBan.LayPTNvaPHT();

            if (listPhongban != null && listPhongban.Count > 0)
            {
                cboPhongpt.DataSource = listPhongban;
                cboPhongpt.DisplayMember = "tenPhong";
                cboPhongpt.ValueMember = "maPhong";
                cboPhongpt.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Không có phòng ban nào trong DB!");
            }

            if (!string.IsNullOrEmpty(TenNenMauDaChon))
            {
                txtTennenmau.Text = TenNenMauDaChon;
                txtTennenmau.ReadOnly = true; // Không cho sửa
                txtTennenmau.BackColor = Color.FromArgb(240, 240, 240); // Màu xám nhạt

                if (IsEditMode)
                {
                    this.Text = $"Sửa nền mẫu - {TenNenMauDaChon}";
                    label.Text = "Cập nhật thông tin nền mẫu";
                    btnLuu.Text = "Cập nhật";
                }
                else
                {
                    this.Text = $"Chi tiết nền mẫu - {TenNenMauDaChon}";
                    label.Text = "Thông tin nền mẫu ";
                    btnLuu.Text = "Lưu nền mẫu";

                }
            }

            LoadComboBoxThongSo();

            var list = thongSo.LayDanhSachThongSo();
            if (list != null && list.Count > 0)
            {
                cboThongso.DataSource = list;
                cboThongso.DisplayMember = "TenTS";
                cboThongso.ValueMember = "MaTS";
                cboThongso.SelectedIndex = -1;
            }
            else
            {
                MessageBox.Show("Không có thông số nào trong DB!");
            }
            dgvThongso.AutoGenerateColumns = false;
            dgvThongso.Columns.Clear();
            dgvThongso.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenTS",
                HeaderText = "Tên Thông Số",
                Name = "TenTS"
            });

            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "DonVi",
                HeaderText = "Đơn vị",
                Name = "DonVi"
            });

            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaTriToiThieu",
                HeaderText = "Giá trị tối thiểu",
                Name = "GiaTriToiThieu"
            });

            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "GiaTriToiDa",
                HeaderText = "Giá trị tối đa",
                Name = "GiaTriToiDa"
            });


            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "TenPhong",
                HeaderText = "Tên phòng",
                Name = "TenPhong"
            });

            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "PhuongPhap",
                HeaderText = "Phương pháp",
                Name = "PhuongPhap"
            });

            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaTS",
                DataPropertyName = "MaTS",
                Visible = false
            });

            dgvThongso.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "MaPhong",
                DataPropertyName = "MaPhong",
                Visible = false
            });
            DataGridViewImageColumn thaoTacCol = new DataGridViewImageColumn
            {
                Name = "ThaoTac",
                HeaderText = "Thao tác",
                ImageLayout = DataGridViewImageCellLayout.Zoom,
                Width = 80
            };

            dgvThongso.Columns.Add(thaoTacCol);


            dgvThongso.DataSource = danhSachChiTiet;
            dgvThongso.CellPainting += dgvThongso_CellPainting;
            dgvThongso.CellClick += dgvThongso_CellClick;

            if (IsEditMode && _danhSachChinhSua != null)
            {
                LoadDataForEdit(_tenViTriEdit, _toaDoEdit, _ghiChuEdit, _danhSachChinhSua);
            }

        }


        private void btnThemts_Click(object sender, EventArgs e)
        {
            ThemThongSo themThongSo = new ThemThongSo();
            themThongSo.StartPosition = FormStartPosition.CenterParent;
            if (themThongSo.ShowDialog(this) == DialogResult.OK)
            {
                LoadComboBoxThongSo();
            }
        }

        public void SetDataForEdit(string tenViTri, string toaDo, string ghiChu, List<ChiTietQuanTracView> danhSach)
        {
            _tenViTriEdit = tenViTri;
            _toaDoEdit = toaDo;
            _ghiChuEdit = ghiChu;
            _danhSachChinhSua = danhSach;
        }

        public void LoadDataForEdit(string tenViTri, string toaDo, string ghiChu, List<ChiTietQuanTracView> danhSach)
        {
            txtTenvitri.Text = tenViTri;
            txtToado.Text = toaDo;
            txtGhichu.Text = ghiChu;

            if (danhSach != null && danhSach.Count > 0)
            {
                danhSachChiTiet.Clear();
                foreach (var item in danhSach)
                {
                    danhSachChiTiet.Add(item);
                }
            }
        }


        private void LoadComboBoxThongSo()
        {
            var thongSo = new ThongSoBLL();
            var list = thongSo.LayDanhSachThongSo();

            if (list != null && list.Count > 0)
            {
                // Lưu lại item đang chọn (nếu có)
                object selectedValue = cboThongso.SelectedValue;

                cboThongso.DataSource = list;
                cboThongso.DisplayMember = "TenTS";
                cboThongso.ValueMember = "MaTS";

                // Khôi phục lại item đã chọn hoặc chọn item mới nhất
                if (selectedValue != null && list.Any(x => x.MaTS == selectedValue.ToString()))
                {
                    cboThongso.SelectedValue = selectedValue;
                }
                else
                {
                    // Chọn item mới nhất (vừa thêm)
                    cboThongso.SelectedIndex = list.Count - 1;
                }
            }
            else
            {
                cboThongso.DataSource = null;
            }
        }


        private void btnLuu_Click(object sender, EventArgs e)
        {
            try
            {
                // ✅ Validation form
                if (string.IsNullOrWhiteSpace(txtTennenmau.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên nền mẫu!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTennenmau.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtTenvitri.Text))
                {
                    MessageBox.Show("Vui lòng nhập tên vị trí!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtTenvitri.Focus();
                    return;
                }

                // ✅ Kiểm tra phải có ít nhất 1 thông số
                if (danhSachChiTiet.Count == 0)
                {
                    MessageBox.Show("Vui lòng thêm ít nhất một thông số!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // ✅ Kiểm tra MaDN có tồn tại không
                if (string.IsNullOrWhiteSpace(MaDN))
                {
                    MessageBox.Show("Không tìm thấy mã đợt nền (MaDN)!\nVui lòng tạo lại nền mẫu.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ✅ Gọi BLL để lưu
                BLL_DotQuanTrac bll = new BLL_DotQuanTrac();

                bool ketQua = bll.LuuChiTietNenMau(
                    maDN: MaDN,
                    tenViTri: txtTenvitri.Text.Trim(),
                    toaDo: txtToado.Text.Trim(),
                    ghiChu: txtGhichu.Text.Trim(),
                    danhSachThongSo: danhSachChiTiet.ToList()
                );

                if (ketQua)
                {
                    MessageBox.Show(
                        $"Lưu chi tiết nền mẫu thành công!\n\n" +
                        $"Nền mẫu: {txtTennenmau.Text}\n" +
                        $"Vị trí: {txtTenvitri.Text}\n" +
                        $"Số thông số: {danhSachChiTiet.Count}",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information
                    );

                    // ✅ Lưu thông tin để trả về form cha
                    this.ChiTietDaChon = danhSachChiTiet.ToList();
                    this.TenNenMauDaChon = txtTennenmau.Text.Trim();
                    this.MoTaNen = txtGhichu.Text.Trim();

                    // ✅ Đóng form với DialogResult.OK
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lưu thất bại! Vui lòng kiểm tra lại dữ liệu.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu chi tiết nền mẫu:\n{ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
       

        private void btnThemthongso_Click_1(object sender, EventArgs e)
        {

            if (cboThongso.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn thông số cần thêm.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // ✅ Lấy thông số đã chọn
            var thongSoDaChon = (ThongSo)cboThongso.SelectedItem;

            if (danhSachChiTiet.Any(x => x.MaTS == thongSoDaChon.MaTS))
            {
                MessageBox.Show("Thông số này đã có trong danh sách!", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SuaThongSo frmSua = new SuaThongSo(thongSoDaChon);
            frmSua.StartPosition = FormStartPosition.CenterParent;

            if (frmSua.ShowDialog(this) == DialogResult.OK)
            {
                var thongSoDaChinhSua = frmSua.ThongSoDaChinhSua;

                if (cboPhongpt.SelectedItem == null)
                {
                    MessageBox.Show("Vui lòng chọn phòng phân tích!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var phong = (PhongBan)cboPhongpt.SelectedItem;

                var item = new ChiTietQuanTracView
                {
                    MaTS = thongSoDaChinhSua.MaTS,
                    TenTS = thongSoDaChinhSua.TenTS,
                    DonVi = thongSoDaChinhSua.DonVi,
                    GiaTriToiThieu = thongSoDaChinhSua.GiaTriToiThieu,
                    GiaTriToiDa = thongSoDaChinhSua.GiaTriToiDa,
                    MaPhong = phong.maPhong,
                    TenPhong = phong.tenPhong,
                    PhuongPhap = thongSoDaChinhSua.phuongPhap
                };

                danhSachChiTiet.Add(item);

                MessageBox.Show($"Đã thêm thông số '{thongSoDaChinhSua.TenTS}' vào danh sách!",
                    "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                cboThongso.SelectedIndex = -1;
                cboPhongpt.SelectedIndex = -1;
            }

        }

        private void btnHuy_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }


        private void dgvThongso_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvThongso.Columns["ThaoTac"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                int iconWidth = 24;
                int iconHeight = 24;

                int startX = e.CellBounds.Left + (e.CellBounds.Width - iconWidth) / 2;
                int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

                deleteRect = new Rectangle(startX, startY, iconWidth, iconHeight);

                if (Properties.Resources.trash_can != null)
                {
                    e.Graphics.DrawImage(Properties.Resources.trash_can, deleteRect);
                }

                e.Handled = true;
            }
        }

        private void dgvThongso_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Kiểm tra click vào cột Thao tác
            if (e.RowIndex < 0 || e.ColumnIndex != dgvThongso.Columns["ThaoTac"].Index)
                return;

            var clickPoint = dgvThongso.PointToClient(Cursor.Position);

            // Kiểm tra click vào icon delete
            if (deleteRect.Contains(clickPoint))
            {
                DataGridViewRow row = dgvThongso.Rows[e.RowIndex];

                if (row.Cells["MaTS"].Value == null) return;

                string tenTS = row.Cells["TenTS"].Value?.ToString();

                DialogResult result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa thông số '{tenTS}' khỏi danh sách?",
                    "Xác nhận xóa",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                );

                if (result == DialogResult.Yes)
                {
                    // Lấy item từ BindingList
                    var item = danhSachChiTiet[e.RowIndex];
                    danhSachChiTiet.Remove(item);

                    MessageBox.Show($"Đã xóa thông số '{tenTS}' khỏi danh sách!",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        

        private void cboThongso_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboThongso.SelectedIndex != -1)
            {
                cboPhongpt.Enabled = true;
            }
            else
            {
                cboPhongpt.Enabled = false;
                cboPhongpt.SelectedIndex = -1;
            }
        }


        private void cboPhongpt_Click(object sender, EventArgs e)
        {
            if (cboThongso.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn Thông số trước!",
                                "Thông báo",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }
        }

        private void dgvThongso_CellContentClick_1(object sender, DataGridViewCellEventArgs e){}
        private void cboPhongpt_SelectedIndexChanged(object sender, EventArgs e){}
        private void cboChon_SelectedIndexChanged(object sender, EventArgs e){}
        private void dgvThongso_CellContentClick(object sender, DataGridViewCellEventArgs e){}
        private void txtTennenmau_TextChanged(object sender, EventArgs e){}
    }

}
