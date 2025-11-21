using BLL;
using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class DangKyKhuonMat : Form
    {
        private VideoCapture camera;
        private readonly NhanDienKhuonMatBLL nhanDienKhuonMatBLL = new NhanDienKhuonMatBLL();
        private Image<Bgr, byte> anhDaChup;
        private readonly string tenTK;

        // ===== THÊM BỘ ĐẾM ĐỂ ĐẢM BẢO CHẤT LƯỢNG =====
        private int demKhungHinhOnDinh = 0;
        private const int SO_KHUNG_HINH_YEU_CAU = 15; // Yêu cầu 15 khung hình ổn định (~0.5 giây)
        private DateTime thoiDiemBatDauOnDinh = DateTime.MinValue;

        public DangKyKhuonMat(string tenTaiKhoan)
        {
            InitializeComponent();
            tenTK = tenTaiKhoan;
        }

        private Bitmap MatToBitmap(Mat mat)
        {
            if (mat == null || mat.IsEmpty)
                return null;

            try
            {
                int width = mat.Width;
                int height = mat.Height;
                int stride = mat.Step;

                Bitmap bitmap = new Bitmap(width, height, stride,
                    System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                    mat.DataPointer);

                return new Bitmap(bitmap);
            }
            catch
            {
                try
                {
                    var image = mat.ToImage<Bgr, byte>();
                    return ImageToBitmap(image);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"MatToBitmap Error: {ex.Message}");
                    return null;
                }
            }
        }

        private Bitmap ImageToBitmap(Image<Bgr, byte> image)
        {
            if (image == null)
                return null;

            try
            {
                int width = image.Width;
                int height = image.Height;

                Bitmap bitmap = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);

                System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(
                    new Rectangle(0, 0, width, height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    bitmap.PixelFormat);

                int bytes = Math.Abs(bmpData.Stride) * height;
                byte[] rgbValues = new byte[bytes];

                Marshal.Copy(image.Mat.DataPointer, rgbValues, 0, bytes);
                Marshal.Copy(rgbValues, 0, bmpData.Scan0, bytes);

                bitmap.UnlockBits(bmpData);

                return bitmap;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ImageToBitmap Error: {ex.Message}");

                try
                {
                    Bitmap bitmap = new Bitmap(image.Width, image.Height, image.Mat.Step,
                        System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                        image.Mat.DataPointer);
                    return new Bitmap(bitmap);
                }
                catch
                {
                    return null;
                }
            }
        }

        private GraphicsPath GetRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;

            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);

            path.CloseFigure();
            return path;
        }

        private void ApplyRoundedCorners(Button button, int radius = 15)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;

            GraphicsPath path = GetRoundedRectanglePath(
                new Rectangle(0, 0, button.Width, button.Height),
                radius
            );

            button.Region = new Region(path);
        }

        private void DangKyKhuonMat_Load(object sender, EventArgs e)
        {
            ApplyRoundedCorners(btnBatCamera, 15);
            ApplyRoundedCorners(btnChupAnh, 15);
            ApplyRoundedCorners(btnLuuKhuonMat, 15);

            lblTrangThai.Text = "Nhấn \"Bật Camera\" để bắt đầu";
            lblTrangThai.ForeColor = Color.Gray;
            btnChupAnh.Enabled = false;
            btnLuuKhuonMat.Enabled = false;
        }

        private void btnBatCamera_Click(object sender, EventArgs e)
        {
            try
            {
                if (camera == null || !camera.IsOpened)
                {
                    camera = new VideoCapture(0);

                    if (!camera.IsOpened)
                    {
                        throw new Exception("Không thể mở camera. Kiểm tra kết nối webcam.");
                    }

                    Application.Idle += XuLyKhungHinh;

                    btnBatCamera.Text = "Tắt Camera";
                    btnChupAnh.Enabled = true;
                    lblTrangThai.Text = "⏳ Giữ ổn định... Hệ thống đang kiểm tra chất lượng";
                    lblTrangThai.ForeColor = Color.FromArgb(255, 152, 0); // Màu cam

                    // Reset bộ đếm
                    demKhungHinhOnDinh = 0;
                    thoiDiemBatDauOnDinh = DateTime.MinValue;
                }
                else
                {
                    Application.Idle -= XuLyKhungHinh;
                    camera.Dispose();
                    camera = null;

                    btnBatCamera.Text = "Bật Camera";
                    btnChupAnh.Enabled = false;

                    if (pictureBoxCamera.Image != null)
                    {
                        pictureBoxCamera.Image.Dispose();
                        pictureBoxCamera.Image = null;
                    }

                    lblTrangThai.Text = "Camera đã tắt";
                    lblTrangThai.ForeColor = Color.Gray;

                    demKhungHinhOnDinh = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Lỗi khi mở camera: {ex.Message}\n\n" +
                    "Kiểm tra:\n" +
                    "- Webcam có kết nối?\n" +
                    "- App khác đang dùng camera?\n" +
                    "- Driver đã cài đặt?",
                    "Lỗi Camera",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        /// Xử lý khung hình với ĐỘ TRỄ ĐẢM BẢO CHẤT LƯỢNG
        private void XuLyKhungHinh(object sender, EventArgs e)
        {
            if (camera != null && camera.IsOpened)
            {
                try
                {
                    Mat khungHinh = camera.QueryFrame();
                    if (khungHinh != null && !khungHinh.IsEmpty)
                    {
                        var bitmap = MatToBitmap(khungHinh);
                        if (bitmap != null)
                        {
                            if (pictureBoxCamera.Image != null)
                            {
                                var oldImage = pictureBoxCamera.Image;
                                pictureBoxCamera.Image = null;
                                oldImage.Dispose();
                            }
                            pictureBoxCamera.Image = bitmap;
                        }

                        // ===== KIỂM TRA ĐỘ ỔN ĐỊNH =====
                        if (KiemTraKhungHinhOnDinh(khungHinh))
                        {
                            demKhungHinhOnDinh++;

                            if (demKhungHinhOnDinh == 1)
                            {
                                thoiDiemBatDauOnDinh = DateTime.Now;
                            }

                            // Hiển thị tiến trình
                            double phanTram = (demKhungHinhOnDinh / (double)SO_KHUNG_HINH_YEU_CAU) * 100;

                            if (demKhungHinhOnDinh < SO_KHUNG_HINH_YEU_CAU)
                            {
                                lblTrangThai.Text = $"⏳ Đang ổn định... {phanTram:F0}% ({demKhungHinhOnDinh}/{SO_KHUNG_HINH_YEU_CAU})";
                                lblTrangThai.ForeColor = Color.FromArgb(255, 152, 0); // Cam
                            }
                            else if (demKhungHinhOnDinh == SO_KHUNG_HINH_YEU_CAU)
                            {
                                lblTrangThai.Text = "✅ ỔN ĐỊNH! Có thể chụp ảnh ngay";
                                lblTrangThai.ForeColor = Color.FromArgb(76, 175, 80); // Xanh lá
                            }
                        }
                        else
                        {
                            // Reset nếu không ổn định
                            if (demKhungHinhOnDinh > 0)
                            {
                                demKhungHinhOnDinh = 0;
                                thoiDiemBatDauOnDinh = DateTime.MinValue;
                                lblTrangThai.Text = "⚠️ Mất ổn định! Giữ camera yên...";
                                lblTrangThai.ForeColor = Color.FromArgb(244, 67, 54); // Đỏ
                            }
                        }

                        khungHinh.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"XuLyKhungHinh Error: {ex.Message}");
                }
            }
        }

        /// Kiểm tra khung hình có ổn định không (đơn giản - kiểm tra độ sáng)
        private bool KiemTraKhungHinhOnDinh(Mat khungHinh)
        {
            try
            {
                // Chuyển sang grayscale để kiểm tra
                using (Mat gray = new Mat())
                {
                    CvInvoke.CvtColor(khungHinh, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                    // Kiểm tra độ sáng trung bình
                    MCvScalar mean = CvInvoke.Mean(gray);
                    double brightness = mean.V0;

                    // Kiểm tra độ sáng trong khoảng hợp lý (45-210)
                    if (brightness < 45 || brightness > 210)
                    {
                        return false;
                    }

                    // Kiểm tra độ tương phản
                    MCvScalar stdDev = new MCvScalar();
                    MCvScalar meanOut = new MCvScalar();
                    CvInvoke.MeanStdDev(gray, ref meanOut, ref stdDev);

                    if (stdDev.V0 < 25) // Độ tương phản quá thấp
                    {
                        return false;
                    }

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        private void btnChupAnh_Click(object sender, EventArgs e)
        {
            if (camera != null && camera.IsOpened)
            {
                // ===== KIỂM TRA ĐỘ ỔN ĐỊNH TRƯỚC KHI CHỤP =====
                if (demKhungHinhOnDinh < SO_KHUNG_HINH_YEU_CAU)
                {
                    double phanTram = (demKhungHinhOnDinh / (double)SO_KHUNG_HINH_YEU_CAU) * 100;
                    MessageBox.Show(
                        $"⚠️ Vui lòng chờ hệ thống ổn định!\n\n" +
                        $"Tiến trình: {phanTram:F0}% ({demKhungHinhOnDinh}/{SO_KHUNG_HINH_YEU_CAU})\n\n" +
                        $"📌 Giữ khuôn mặt ổn định trong khung hình\n" +
                        $"📌 Đảm bảo ánh sáng đủ và không rung lắc",
                        "Chưa sẵn sàng",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                Mat khungHinh = camera.QueryFrame();
                if (khungHinh != null && !khungHinh.IsEmpty)
                {
                    anhDaChup = khungHinh.ToImage<Bgr, byte>();

                    // Tắt camera
                    Application.Idle -= XuLyKhungHinh;
                    camera.Dispose();
                    camera = null;

                    // Hiển thị ảnh đã chụp
                    var bitmap = ImageToBitmap(anhDaChup);
                    if (bitmap != null)
                    {
                        if (pictureBoxCamera.Image != null)
                        {
                            var oldImage = pictureBoxCamera.Image;
                            pictureBoxCamera.Image = null;
                            oldImage.Dispose();
                        }
                        pictureBoxCamera.Image = bitmap;
                    }

                    btnBatCamera.Text = "Bật Camera";
                    btnBatCamera.Enabled = true;
                    btnChupAnh.Enabled = false;
                    btnLuuKhuonMat.Enabled = true;

                    double thoiGianOnDinh = (DateTime.Now - thoiDiemBatDauOnDinh).TotalSeconds;
                    lblTrangThai.Text = $"✓ Đã chụp ảnh chất lượng cao! (ổn định {thoiGianOnDinh:F1}s)\nNhấn 'Lưu Khuôn Mặt'";
                    lblTrangThai.ForeColor = Color.FromArgb(76, 175, 80);

                    // Reset bộ đếm
                    demKhungHinhOnDinh = 0;

                    khungHinh.Dispose();
                }
                else
                {
                    MessageBox.Show("Không thể chụp ảnh!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Camera chưa bật!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnLuuKhuonMat_Click(object sender, EventArgs e)
        {
            if (anhDaChup == null)
            {
                MessageBox.Show("Vui lòng chụp ảnh trước!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblTrangThai.Text = "⏳ Đang lưu khuôn mặt...";
            lblTrangThai.ForeColor = Color.FromArgb(33, 150, 243);
            btnLuuKhuonMat.Enabled = false;
            btnBatCamera.Enabled = false;
            Application.DoEvents();

            try
            {
                var ketQua = nhanDienKhuonMatBLL.DangKyKhuonMat(tenTK, anhDaChup);

                if (ketQua.thanhCong)
                {
                    lblTrangThai.Text = "✓ " + ketQua.thongBao;
                    lblTrangThai.ForeColor = Color.FromArgb(76, 175, 80);

                    MessageBox.Show(
                        ketQua.thongBao + "\n\nBạn có thể dùng Face ID để đăng nhập!",
                        "Thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    lblTrangThai.Text = "✗ " + ketQua.thongBao;
                    lblTrangThai.ForeColor = Color.FromArgb(244, 67, 54);

                    MessageBox.Show(
                        ketQua.thongBao,
                        "Lỗi",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    btnBatCamera.Enabled = true;
                    btnLuuKhuonMat.Enabled = false;

                    if (anhDaChup != null)
                    {
                        anhDaChup.Dispose();
                        anhDaChup = null;
                    }
                }
            }
            catch (Exception ex)
            {
                lblTrangThai.Text = "✗ Lỗi khi lưu";
                lblTrangThai.ForeColor = Color.FromArgb(244, 67, 54);

                MessageBox.Show(
                    $"Lỗi: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                btnBatCamera.Enabled = true;
                btnLuuKhuonMat.Enabled = false;
            }
        }

        private void FormDangKyKhuonMat_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (camera != null)
                {
                    Application.Idle -= XuLyKhungHinh;
                    camera.Dispose();
                    camera = null;
                }

                if (pictureBoxCamera.Image != null)
                {
                    pictureBoxCamera.Image.Dispose();
                    pictureBoxCamera.Image = null;
                }

                if (anhDaChup != null)
                {
                    anhDaChup.Dispose();
                    anhDaChup = null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"FormClosing Error: {ex.Message}");
            }
        }
    }
}