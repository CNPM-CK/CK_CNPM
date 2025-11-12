using BLL;
using DTO;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class TrangThongBao : UserControl
    {
        private readonly ThongBaoBLL bllThongBao = new ThongBaoBLL();

        #region Fields
        // Search box styling
        private Color borderColor = Color.Black;
        private int borderRadius = 12;
        private int borderSize = 2;
        private const int SEARCH_HEIGHT = 50;
        private const string PLACEHOLDER_TEXT = "Tìm kiếm thông báo...";

        // Layout constants
        private const int MARGIN = 15;
        private const int SPACING = 10;
        private const int MIN_SEARCH_WIDTH = 200;
        private const int MAX_SEARCH_WIDTH = 500;

        // Data & State
        private DataTable dtThongBao;
        private bool isPlaceholder = true;
        private string lastSearchKeyword = "";
        #endregion

        #region Constructor
        public TrangThongBao()
        {
            InitializeComponent();
            this.Load += TrangThongBao_Load;
            this.Resize += TrangThongBao_Resize;
        }
        #endregion

        #region Initialization
        private void TrangThongBao_Load(object sender, EventArgs e)
        {
            try
            {
                InitializeDataGridView();
                InitializeCustomSearchBox();
                InitializeWatermark();
                CalculateLayout();
                LoadThongBaoQuaHan();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khởi tạo trang thông báo: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeDataGridView()
        {
            dgvdsThongbao.AutoGenerateColumns = false;
            dgvdsThongbao.Columns.Clear();
            dgvdsThongbao.AllowUserToAddRows = false;
            dgvdsThongbao.ReadOnly = true;
            dgvdsThongbao.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvdsThongbao.MultiSelect = false;
            dgvdsThongbao.RowTemplate.Height = 80; // ✅ Tăng chiều cao row

            // ✅ Cho phép text xuống hàng
            dgvdsThongbao.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            dgvdsThongbao.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            // Font settings
            dgvdsThongbao.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular);
            dgvdsThongbao.DefaultCellStyle.Font = new Font("Segoe UI", 9.75F);
            dgvdsThongbao.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);

            // Header styling
            dgvdsThongbao.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 152, 70);
            dgvdsThongbao.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvdsThongbao.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvdsThongbao.EnableHeadersVisualStyles = false;

            // Cell styling
            dgvdsThongbao.DefaultCellStyle.BackColor = Color.White;
            dgvdsThongbao.DefaultCellStyle.ForeColor = Color.Black;
            dgvdsThongbao.DefaultCellStyle.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dgvdsThongbao.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Define columns
            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "loaiTB",
                HeaderText = "LOẠI THÔNG BÁO",
                Width = 150,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });

            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "maTB",
                HeaderText = "MÃ TB",
                Width = 100,
                Visible = false,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "maDot",
                HeaderText = "MÃ ĐỢT",
                Width = 100,
                Visible = false,
                DefaultCellStyle = new DataGridViewCellStyle { Alignment = DataGridViewContentAlignment.MiddleCenter }
            });

            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "tenKhachHang",
                HeaderText = "KHÁCH HÀNG",
                Width = 250,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    WrapMode = DataGridViewTriState.False // ✅ Không xuống hàng
                }
            });

            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "tieuDe",
                HeaderText = "TIÊU ĐỀ",
                Width = 250,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    WrapMode = DataGridViewTriState.False // ✅ Không xuống hàng
                }
            });

            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "noiDung",
                HeaderText = "NỘI DUNG",
                Width = 500, // ✅ Độ rộng cố định
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.TopLeft, // ✅ Căn trên-trái
                    WrapMode = DataGridViewTriState.True, // ✅ Cho phép xuống hàng
                    Padding = new Padding(5, 5, 5, 5) // ✅ Thêm padding cho đẹp
                }
            });

            dgvdsThongbao.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ngayTao",
                HeaderText = "NGÀY TẠO",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Format = "dd/MM/yyyy",
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                }
            });
        }

        private void InitializeCustomSearchBox()
        {
            if (containersearch == null || searchtextbox == null) return;

            containersearch.BackColor = Color.Transparent;
            containersearch.Size = new Size(400, SEARCH_HEIGHT);
            containersearch.BringToFront();

            searchtextbox.BorderStyle = BorderStyle.None;
            searchtextbox.BackColor = Color.White;
            searchtextbox.Font = new Font("Segoe UI", 10F);
            searchtextbox.ForeColor = Color.Silver;
            searchtextbox.Text = PLACEHOLDER_TEXT;
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);
            searchtextbox.Size = new Size(containersearch.Width - (borderSize * 2 + 10), 28);
            searchtextbox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            containersearch.Controls.Add(searchtextbox);

            // Events
            searchtextbox.Enter += Searchtextbox_Enter;
            searchtextbox.Leave += Searchtextbox_Leave;
            searchtextbox.TextChanged += Searchtextbox_TextChanged;
            searchtextbox.KeyDown += Searchtextbox_KeyDown;
            containersearch.Paint += Containersearch_Paint;
        }

        private void InitializeWatermark()
        {
            if (Properties.Resources.greenlogo == null || dgvdsThongbao == null) return;

            try
            {
                Image watermark = Properties.Resources.greenlogo;
                Bitmap bmp = new Bitmap(watermark.Width, watermark.Height);

                using (Graphics g = Graphics.FromImage(bmp))
                {
                    ColorMatrix matrix = new ColorMatrix();
                    matrix.Matrix33 = 0.15f;
                    ImageAttributes attributes = new ImageAttributes();
                    attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                    g.DrawImage(watermark,
                        new Rectangle(0, 0, watermark.Width, watermark.Height),
                        0, 0, watermark.Width, watermark.Height,
                        GraphicsUnit.Pixel,
                        attributes);
                }

                dgvdsThongbao.BackgroundImage = bmp;
                dgvdsThongbao.BackgroundImageLayout = ImageLayout.Center;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Watermark error: {ex.Message}");
            }
        }
        #endregion

        #region Layout & Resize
        private void TrangThongBao_Resize(object sender, EventArgs e)
        {
            if (this.Width < 100) return;
            CalculateLayout();
        }

        private void CalculateLayout()
        {
            if (containersearch == null) return;

            int formWidth = this.Width;

            Form parentForm = this.FindForm();
            bool isMaximized = parentForm != null && parentForm.WindowState == FormWindowState.Maximized;

            int leftBoundary = pictureFilter != null ? pictureFilter.Right + SPACING : MARGIN;
            int rightBoundary = formWidth - MARGIN;

            if (picturemicro != null)
            {
                rightBoundary -= picturemicro.Width + SPACING;
            }

            int availableWidth = rightBoundary - leftBoundary;

            int searchWidth = Math.Max(MIN_SEARCH_WIDTH, Math.Min(availableWidth, MAX_SEARCH_WIDTH));
            if (searchWidth < MIN_SEARCH_WIDTH)
            {
                searchWidth = Math.Max(150, availableWidth);
            }

            if (pictureFilter != null)
            {
                pictureFilter.Left = MARGIN;
            }

            containersearch.Left = leftBoundary;
            containersearch.Width = searchWidth;
            containersearch.Height = SEARCH_HEIGHT;

            searchtextbox.Width = searchWidth - (borderSize * 2 + 10);
            searchtextbox.Location = new Point(borderSize + 5, (SEARCH_HEIGHT - 28) / 2);

            if (picturemicro != null)
            {
                picturemicro.Left = containersearch.Right + SPACING;
            }

            containersearch.Invalidate();
        }
        #endregion

        #region Search Functionality
        private void Searchtextbox_Enter(object sender, EventArgs e)
        {
            if (isPlaceholder)
            {
                isPlaceholder = false;
                searchtextbox.Text = "";
                searchtextbox.ForeColor = Color.FromArgb(64, 64, 64);
            }
        }

        private void Searchtextbox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(searchtextbox.Text))
            {
                isPlaceholder = true;
                searchtextbox.Text = PLACEHOLDER_TEXT;
                searchtextbox.ForeColor = Color.Silver;
                dgvdsThongbao.DataSource = dtThongBao;
                lastSearchKeyword = "";
            }
        }

        private void Searchtextbox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                if (dgvdsThongbao.Rows.Count > 0)
                {
                    dgvdsThongbao.ClearSelection();
                    dgvdsThongbao.Rows[0].Selected = true;
                    dgvdsThongbao.FirstDisplayedScrollingRowIndex = 0;
                }
            }
            else if (e.KeyCode == Keys.Escape)
            {
                searchtextbox.Clear();
                dgvdsThongbao.DataSource = dtThongBao;
                lastSearchKeyword = "";
            }
        }

        private void Searchtextbox_TextChanged(object sender, EventArgs e)
        {
            if (isPlaceholder) return;

            string currentKeyword = searchtextbox.Text.Trim().ToLower();
            if (currentKeyword == lastSearchKeyword) return;

            lastSearchKeyword = currentKeyword;
            PerformSearch();
        }

        private void PerformSearch()
        {
            string keyword = searchtextbox.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                dgvdsThongbao.DataSource = dtThongBao;
                return;
            }

            if (dtThongBao == null || dtThongBao.Rows.Count == 0)
            {
                return;
            }

            try
            {
                DataView dv = dtThongBao.DefaultView;
                dv.RowFilter = string.Format(
                    "loaiTB LIKE '%{0}%' OR " +
                    "tenKhachHang LIKE '%{0}%' OR " +
                    "tieuDe LIKE '%{0}%' OR " +
                    "noiDung LIKE '%{0}%'",
                    keyword.Replace("'", "''"));

                dgvdsThongbao.DataSource = dv.ToTable();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Search error: {ex.Message}");
                dgvdsThongbao.DataSource = dtThongBao;
            }
        }
        #endregion

        #region Custom Paint
        private void Containersearch_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float offset = borderSize / 2f;
            RectangleF rect = new RectangleF(
                offset,
                offset,
                containersearch.ClientSize.Width - borderSize,
                containersearch.ClientSize.Height - borderSize
            );

            using (GraphicsPath path = CreateRoundedRectPath(rect, borderRadius))
            {
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    e.Graphics.FillPath(brush, path);
                }

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

        #region Data Loading
        private void LoadThongBaoQuaHan()
        {
            try
            {
                dtThongBao = bllThongBao.layDanhSachThongBao();
                dgvdsThongbao.DataSource = dtThongBao;
                dgvdsThongbao.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải thông báo: {ex.Message}",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            LoadThongBaoQuaHan();

            // Reset search box
            if (searchtextbox != null)
            {
                searchtextbox.Clear();
                isPlaceholder = true;
                searchtextbox.Text = PLACEHOLDER_TEXT;
                searchtextbox.ForeColor = Color.Silver;
                lastSearchKeyword = "";
            }
        }
        #endregion

        private void dgvdsThongbao_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}