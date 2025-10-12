using BLL;
using DTO;
using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class DanhSachNhanVien : Form
    {
        #region Fields

        private Color borderColor = Color.Black;
        private int borderRadius = 12;
        private int borderSize = 2;

        // Layout constants
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;
        private const int SEARCH_HEIGHT = 50;


        public DanhSachNhanVien()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
        }

        [DllImport("gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect,
            int nTopRect,
            int nRightRect,
            int nBottomRect,
            int nWidthEllipse,
            int nHeightEllipse
        );
        #endregion

        #region Form Load
        private void DanhSachNhanVien_Load(object sender, EventArgs e)
        {
            InitializeContextMenu();
            InitializeButtonIcons();
            InitializeButtonStyles();
            InitializeCustomSearchBox();
            CalculateLayout();

            NhanVienBLL nvBLL = new NhanVienBLL();
            List<NhanVien> ds = nvBLL.LayDanhSachNhanVien();

            dgvDanhsachnhanvien.AutoGenerateColumns = false;
            dgvDanhsachnhanvien.Columns.Clear();

            dgvDanhsachnhanvien.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "maNV",
                HeaderText = "Mã nhân viên",
                Name = "maNV"
            });

            dgvDanhsachnhanvien.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "hoTen",
                HeaderText = "Họ Tên",
                Name = "hoTen"
            });

            dgvDanhsachnhanvien.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "email",
                HeaderText = "Email",
                Name = "email"
            });

            dgvDanhsachnhanvien.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "maPhong",
                HeaderText = "Mã Phòng",
                Name = "maPhong"
            });

            dgvDanhsachnhanvien.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "ngaySinh",
                HeaderText = "Ngày Sinh",
                Name = "ngaySinh"
            });

            dgvDanhsachnhanvien.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "gioiTinh",
                HeaderText = "Giới Tính",
                Name = "gioiTinh"
            });

            dgvDanhsachnhanvien.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "diaChi",
                HeaderText = "Địa Chỉ",
                Name = "diaChi"
            });

            dgvDanhsachnhanvien.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "soDienThoai",
                HeaderText = "Số Điện Thoại",
                Name = "soDienThoai"
            });

            dgvDanhsachnhanvien.Columns.Add(new DataGridViewTextBoxColumn()
            {
                DataPropertyName = "tenPhong",
                HeaderText = "Phòng Ban",
                Name = "tenPhong"
            });

            // Thêm cột Thao tác (luôn cuối)
            DataGridViewImageColumn thaoTacCol = new DataGridViewImageColumn();
            thaoTacCol.Name = "ThaoTac";
            thaoTacCol.HeaderText = "Thao tác";
            thaoTacCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
            dgvDanhsachnhanvien.Columns.Add(thaoTacCol);

            dgvDanhsachnhanvien.DataSource = ds;
        }


        private void LoadDanhSachNhanVien()
        {
            NhanVienBLL nvBLL = new NhanVienBLL();
            List<NhanVien> ds = nvBLL.LayDanhSachNhanVien();
            dgvDanhsachnhanvien.DataSource = null;
            dgvDanhsachnhanvien.DataSource = ds;

            // Giữ nguyên thứ tự cột
            dgvDanhsachnhanvien.Columns["ThaoTac"].DisplayIndex = dgvDanhsachnhanvien.Columns.Count - 1;
        }

        private Rectangle editRect;
        private Rectangle deleteRect;

        private void dgvDanhsachnhanvien_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvDanhsachnhanvien.Columns["ThaoTac"].Index)
            {
                e.PaintBackground(e.ClipBounds, true);

                int iconWidth = 24;
                int iconHeight = 24;
                int spacing = 10;

                int totalWidth = (iconWidth * 2) + spacing;

                // Tính điểm bắt đầu để căn giữa
                int startX = e.CellBounds.Left + (e.CellBounds.Width - totalWidth) / 2;
                int startY = e.CellBounds.Top + (e.CellBounds.Height - iconHeight) / 2;

                // Vẽ icon sửa
                editRect = new Rectangle(startX, startY, iconWidth, iconHeight);
                e.Graphics.DrawImage(Properties.Resources.edit, editRect);

                // Vẽ icon xóa
                deleteRect = new Rectangle(startX + iconWidth + spacing, startY, iconWidth, iconHeight);
                e.Graphics.DrawImage(Properties.Resources.trash_can, deleteRect);

                e.Handled = true;
            }
        }


        private void dgvDanhsachnhanvien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == dgvDanhsachnhanvien.Columns["ThaoTac"].Index)
            {
                var clickPoint = dgvDanhsachnhanvien.PointToClient(Cursor.Position);

                if (editRect.Contains(clickPoint)) 
                {
                    DataGridViewRow row = dgvDanhsachnhanvien.Rows[e.RowIndex];

                    NhanVien nv = new NhanVien
                    {
                        maNV = row.Cells["maNV"].Value.ToString(),
                        maPhong = row.Cells["maPhong"].Value.ToString(),
                        hoTen = row.Cells["hoTen"].Value.ToString(),
                        ngaySinh = Convert.ToDateTime(row.Cells["ngaySinh"].Value),
                        gioiTinh = row.Cells["gioiTinh"].Value.ToString(),
                        diaChi = row.Cells["diaChi"].Value.ToString(),
                        soDienThoai = row.Cells["soDienThoai"].Value.ToString(),
                        email = row.Cells["email"].Value.ToString()
                    };

                    // Gọi form sửa nhân viên, truyền dữ liệu sang
                    SuaNhanVien frmSua = new SuaNhanVien(nv);
                    frmSua.StartPosition = FormStartPosition.CenterParent;

                    if (frmSua.ShowDialog() == DialogResult.OK)
                    {
                        LoadDanhSachNhanVien();
                    }
                }
                else if (deleteRect.Contains(clickPoint)) 
                {
                    string maNV = dgvDanhsachnhanvien.Rows[e.RowIndex].Cells["maNV"].Value.ToString();
                    string hoTen = dgvDanhsachnhanvien.Rows[e.RowIndex].Cells["hoTen"].Value.ToString();

                    DialogResult result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn xóa nhân viên '{hoTen}' (Mã: {maNV}) không?",
                        "Xác nhận xóa",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            NhanVienBLL nvBLL = new NhanVienBLL();
                            nvBLL.XoaNhanVien(maNV);

                            MessageBox.Show("Đã xóa nhân viên thành công!", "Thông báo",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);

                            LoadDanhSachNhanVien(); 
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Có lỗi xảy ra khi xóa nhân viên: " + ex.Message,
                                "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }


        private void dgvDanhsachnhanvien_Paint(object sender, PaintEventArgs e)
        {

            // Đảm bảo DataGridView có ảnh watermark
            if (Properties.Resources.greenlogo == null) return;

            // Lấy kích thước DataGridView
            int dgvWidth = dgvDanhsachnhanvien.Width;
            int dgvHeight = dgvDanhsachnhanvien.Height;

            // Lấy ảnh watermark
            Image watermark = Properties.Resources.greenlogo;

            // Tính vị trí căn giữa
            int x = (dgvWidth - watermark.Width) / 2;
            int y = (dgvHeight - watermark.Height) / 2;

            // Tạo brush mờ
            ColorMatrix matrix = new ColorMatrix();
            matrix.Matrix33 = 0.3f; // độ mờ 0.0 - 1.0
            ImageAttributes attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            // Vẽ watermark trên DataGridView
            e.Graphics.DrawImage(watermark,
                new Rectangle(x, y, watermark.Width, watermark.Height),
                0, 0, watermark.Width, watermark.Height,
                GraphicsUnit.Pixel,
                attributes);
        }

        private void DanhSachNhanVien_Click(object sender, EventArgs e)
        {

        }


        private void InitializeCustomSearchBox()
        {
            // Tạo container panel
            containersearch.BackColor = Color.Transparent;
            containersearch.Size = new Size(400, SEARCH_HEIGHT);
            containersearch.BringToFront();

            // Tạo TextBox
            searchtextbox.BorderStyle = BorderStyle.None;
            searchtextbox.BackColor = Color.White;
            searchtextbox.Font = new Font("Segoe UI", 10F);
            searchtextbox.ForeColor = Color.FromArgb(64, 64, 64);
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);
            searchtextbox.Size = new Size(containersearch.Width - (borderSize * 2 + 10), 28);
            searchtextbox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            containersearch.Controls.Add(searchtextbox);

            SetPlaceholder();

            //Gọi sự kiện 
            searchtextbox.Enter += searchtextbox_Enter;
            searchtextbox.Leave += searchtextbox_Leave;
            searchtextbox.TextChanged += searchtextbox_TextChanged;
            containersearch.Paint += containersearch_Paint;
        }

        private void InitializeContextMenu()
        {
            ContextMenuStrip menu = new ContextMenuStrip();

            ToolStripMenuItem pdfItem = new ToolStripMenuItem("Xuất PDF");
            pdfItem.Click += (s, ev) => { MessageBox.Show("Xuất PDF..."); };

            ToolStripMenuItem excelItem = new ToolStripMenuItem("Xuất Excel");
            excelItem.Click += (s, ev) => { MessageBox.Show("Xuất Excel..."); };

            menu.Items.Add(pdfItem);
            menu.Items.Add(excelItem);

            btnXuatfile.Click += (s, ev) =>
            {
                menu.Show(btnXuatfile, new Point(0, btnXuatfile.Height));
            };
        }

        private void InitializeButtonIcons()
        {
            // Icon danh sách nhân viên
            if (btnDanhsachnv.Image != null)
            {
                btnDanhsachnv.Image = new Bitmap(btnDanhsachnv.Image, new Size(30, 30));
                btnDanhsachnv.ImageAlign = ContentAlignment.MiddleLeft;
                btnDanhsachnv.TextAlign = ContentAlignment.MiddleRight;
                btnDanhsachnv.Padding = new Padding(0, 0, 5, 0);
            }

            // Icon thêm user
            if (btnThemuser.Image != null)
            {
                btnThemuser.Image = new Bitmap(btnThemuser.Image, new Size(24, 24));
            }

            // Icon xuất file
            if (btnXuatfile.Image != null)
            {
                btnXuatfile.Image = new Bitmap(btnXuatfile.Image, new Size(24, 24));
            }
        }

        private void InitializeButtonStyles()
        {
            btnThemuser.Size = new Size(66, 40);
            btnXuatfile.Size = new Size(66, 40);

            BoGocButton(btnThemuser, 20);
            BoGocButton(btnXuatfile, 20);
        }

        private void SetPlaceholder()
        {
            if (string.IsNullOrWhiteSpace(searchtextbox.Text))
            {
                searchtextbox.Text = "Tìm kiếm nhân viên...";
                searchtextbox.ForeColor = Color.Silver;
            }
        }
        #endregion

        #region Layout & Resize
        private void DanhSachNhanVien_Resize(object sender, EventArgs e)
        {
            if (this.ClientSize.Width < 100) return;
            CalculateLayout();
        }

        private void CalculateLayout()
        {
            int formWidth = this.ClientSize.Width;
            bool isMaximized = this.WindowState == FormWindowState.Maximized;
            int btnWidth = isMaximized ? 80 : 66;
            int btnHeight = isMaximized ? 50 : 40;
            int btnRadius = isMaximized ? 25 : 20;

            // Resize buttons
            btnXuatfile.Size = new Size(btnWidth, btnHeight);
            btnThemuser.Size = new Size(btnWidth, btnHeight);
            BoGocButton(btnXuatfile, btnRadius);
            BoGocButton(btnThemuser, btnRadius);

            // Đặt buttons ở bên phải
            btnXuatfile.Left = formWidth - btnWidth - MARGIN;
            btnThemuser.Left = btnXuatfile.Left - btnWidth - SPACING;

            // Đặt vị trí icon filter bên trái
            pictureFilter.Left = MARGIN;

            // Tính toán available space cho search box
            int leftBoundary = pictureFilter.Right + SPACING;
            int rightBoundary = btnThemuser.Left - SPACING - picturemicro.Width - SPACING;
            int availableWidth = rightBoundary - leftBoundary;

            // Tính width cho search box
            int searchWidth = Math.Max(MIN_SEARCH_WIDTH, Math.Min(availableWidth, MAX_SEARCH_WIDTH));
            if (searchWidth < MIN_SEARCH_WIDTH)
            {
                searchWidth = Math.Max(150, availableWidth);
            }

            // Đặt vị trí search container
            containersearch.Left = leftBoundary;
            containersearch.Width = searchWidth;
            containersearch.Height = SEARCH_HEIGHT;

            // Cập nhật width và vị trí của textbox bên trong
            searchtextbox.Width = searchWidth - (borderSize * 2 + 10); // Đồng bộ với InitializeCustomSearchBox
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2); // Căn chỉnh trong container

            // Đặt icon micro ngay sau search box
            picturemicro.Left = containersearch.Right + SPACING;

            // Cập nhật padding cho buttons
            if (isMaximized)
            {
                btnThemuser.Padding = new Padding(10, 5, 10, 5);
                btnXuatfile.Padding = new Padding(10, 5, 10, 5);
            }
            else
            {
                btnThemuser.Padding = new Padding(5, 3, 5, 3);
                btnXuatfile.Padding = new Padding(5, 3, 5, 3);
            }

            // DEBUG: Kiểm tra kích thước
            System.Diagnostics.Debug.WriteLine($"containersearch.Width: {containersearch.Width}, searchtextbox.Width: {searchtextbox.Width}, searchtextbox.Location: {searchtextbox.Location}");

            // Vẽ lại search container
            containersearch.Invalidate();
        }
        #endregion

        #region Button Styling
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
        #endregion

        #region Custom Search Box Paint
        private void containersearch_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Tạo rectangle với offset để viền không bị cắt
            float offset = borderSize / 2f;
            RectangleF rect = new RectangleF(
                offset,
                offset,
                containersearch.ClientSize.Width - borderSize,
                containersearch.ClientSize.Height - borderSize
            );

            using (GraphicsPath path = CreateRoundedRectPath(rect, borderRadius))
            {
                // Fill nền trắng
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillPath(brush, path);
                }

                // Vẽ viền đen
                using (Pen pen = new Pen(borderColor, borderSize))
                {
                    e.Graphics.DrawPath(pen, path);
                }
            }
        }

        private GraphicsPath CreateRoundedRectPath(RectangleF rect, float radius)
        {
            float effectiveRadius = Math.Min(radius, Math.Min(rect.Width / 2f, rect.Height / 2f));
            float diameter = effectiveRadius * 2f;

            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.Left, rect.Top, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Top, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.Left, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            return path;
        }
        #endregion

        #region TextBox Events
        private void searchtextbox_TextChanged(object sender, EventArgs e)
        {
            // Logic tìm kiếm ở đây
            if (searchtextbox.ForeColor != Color.Silver)
            {
                // Xử lý tìm kiếm thực tế
            }
        }

        private void searchtextbox_Enter(object sender, EventArgs e)
        {
            if (searchtextbox.Text == "Tìm kiếm nhân viên..." && searchtextbox.ForeColor == Color.Silver)
            {
                searchtextbox.Text = "";
                searchtextbox.ForeColor = Color.FromArgb(64, 64, 64);
            }
        }

        private void searchtextbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchtextbox.Text))
            {
                SetPlaceholder();
            }
        }
        #endregion

        #region Unused Events
        private void pictureBox5_Click(object sender, EventArgs e)
        {
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
        }
        #endregion


        private void dgvDanhsachnhanvien_CellContentClick(object sender, DataGridViewCellEventArgs e){}

        private void containersearch_Paint_1(object sender, PaintEventArgs e){}

        private void btnThemuser_Click(object sender, EventArgs e)
        {
            ThemNhanVien fThem = new ThemNhanVien();

            // Hiển thị form dưới dạng cửa sổ pop-up (modal)
            fThem.StartPosition = FormStartPosition.CenterParent; // Canh giữa form cha
            if (fThem.ShowDialog() == DialogResult.OK)
            {
                LoadDanhSachNhanVien(); // LOAD LẠI DANH SÁCH
            }
        }
    }
}