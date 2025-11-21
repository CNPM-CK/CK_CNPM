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
    public partial class NhanDienKhuonMat : Form
    {
        private VideoCapture camera;
        private readonly NhanDienKhuonMatBLL nhanDienKhuonMatBLL = new NhanDienKhuonMatBLL();
        private Image<Bgr, byte> anhDaChup;
        private readonly string tenTaiKhoan;

        public bool NhanDienThanhCong { get; private set; }

        // ===== BỘ ĐẾM ĐỘ ỔN ĐỊNH =====
        private int demKhungHinhOnDinh = 0;
        private const int SO_KHUNG_HINH_YEU_CAU = 15;
        private DateTime thoiDiemBatDauOnDinh = DateTime.MinValue;

        public NhanDienKhuonMat(string tenTK)
        {
            InitializeComponent();
            tenTaiKhoan = tenTK;
            NhanDienThanhCong = false;
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

        private void NhanDienKhuonMat_Load(object sender, EventArgs e)
        {
            ApplyRoundedCorners(btnBatCamera, 15);
            ApplyRoundedCorners(btnChupAnh, 15);
            ApplyRoundedCorners(btnXacNhan, 15);

            lblTrangThai.Text = $"Xác thực: {tenTaiKhoan}\nNhấn 'Bật Camera'";
            lblTrangThai.ForeColor = Color.Gray;

            btnChupAnh.Enabled = false;
            btnXacNhan.Enabled = false;
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
                        throw new Exception("Không thể mở camera.");
                    }

                    Application.Idle += XuLyKhungHinh;

                    btnBatCamera.Text = "Tắt Camera";
                    btnChupAnh.Enabled = true;
                    lblTrangThai.Text = "⏳ Giữ ổn định... Đang kiểm tra chất lượng";
                    lblTrangThai.ForeColor = Color.FromArgb(255, 152, 0);

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
                    $"Lỗi: {ex.Message}\n\nKiểm tra:\n- Webcam kết nối?\n- App khác dùng camera?",
                    "Lỗi Camera",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

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

                        // Kiểm tra độ ổn định
                        if (KiemTraKhungHinhOnDinh(khungHinh))
                        {
                            demKhungHinhOnDinh++;

                            if (demKhungHinhOnDinh == 1)
                            {
                                thoiDiemBatDauOnDinh = DateTime.Now;
                            }

                            double phanTram = (demKhungHinhOnDinh / (double)SO_KHUNG_HINH_YEU_CAU) * 100;

                            if (demKhungHinhOnDinh < SO_KHUNG_HINH_YEU_CAU)
                            {
                                lblTrangThai.Text = $"⏳ Ổn định... {phanTram:F0}% ({demKhungHinhOnDinh}/{SO_KHUNG_HINH_YEU_CAU})";
                                lblTrangThai.ForeColor = Color.FromArgb(255, 152, 0);
                            }
                            else if (demKhungHinhOnDinh == SO_KHUNG_HINH_YEU_CAU)
                            {
                                lblTrangThai.Text = "✅ ỔN ĐỊNH! Có thể quét khuôn mặt";
                                lblTrangThai.ForeColor = Color.FromArgb(76, 175, 80);
                            }
                        }
                        else
                        {
                            if (demKhungHinhOnDinh > 0)
                            {
                                demKhungHinhOnDinh = 0;
                                thoiDiemBatDauOnDinh = DateTime.MinValue;
                                lblTrangThai.Text = "⚠️ Mất ổn định! Giữ yên...";
                                lblTrangThai.ForeColor = Color.FromArgb(244, 67, 54);
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

        private bool KiemTraKhungHinhOnDinh(Mat khungHinh)
        {
            try
            {
                using (Mat gray = new Mat())
                {
                    CvInvoke.CvtColor(khungHinh, gray, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);

                    MCvScalar mean = CvInvoke.Mean(gray);
                    double brightness = mean.V0;

                    if (brightness < 45 || brightness > 210)
                    {
                        return false;
                    }

                    MCvScalar stdDev = new MCvScalar();
                    MCvScalar meanOut = new MCvScalar();
                    CvInvoke.MeanStdDev(gray, ref meanOut, ref stdDev);

                    if (stdDev.V0 < 25)
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
                if (demKhungHinhOnDinh < SO_KHUNG_HINH_YEU_CAU)
                {
                    double phanTram = (demKhungHinhOnDinh / (double)SO_KHUNG_HINH_YEU_CAU) * 100;
                    MessageBox.Show(
                        $"⚠️ Chờ hệ thống ổn định!\n\n" +
                        $"Tiến trình: {phanTram:F0}% ({demKhungHinhOnDinh}/{SO_KHUNG_HINH_YEU_CAU})\n\n" +
                        $"Giữ khuôn mặt ổn định trong khung hình",
                        "Chưa sẵn sàng",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                Mat khungHinh = camera.QueryFrame();
                if (khungHinh != null && !khungHinh.IsEmpty)
                {
                    anhDaChup = khungHinh.ToImage<Bgr, byte>();

                    Application.Idle -= XuLyKhungHinh;
                    camera.Dispose();
                    camera = null;

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
                    btnXacNhan.Enabled = true;

                    double thoiGianOnDinh = (DateTime.Now - thoiDiemBatDauOnDinh).TotalSeconds;
                    lblTrangThai.Text = $"✓ Đã quét! (ổn định {thoiGianOnDinh:F1}s)\nNhấn 'Xác Nhận Đăng Nhập'";
                    lblTrangThai.ForeColor = Color.FromArgb(76, 175, 80);

                    demKhungHinhOnDinh = 0;

                    khungHinh.Dispose();
                }
                else
                {
                    MessageBox.Show("Không thể quét!", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Camera chưa bật!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnXacNhan_Click(object sender, EventArgs e)
        {
            if (anhDaChup == null)
            {
                MessageBox.Show("Vui lòng quét khuôn mặt trước!", "Cảnh báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            lblTrangThai.Text = $"⏳ Đang xác thực: {tenTaiKhoan}...";
            lblTrangThai.ForeColor = Color.FromArgb(33, 150, 243);
            btnXacNhan.Enabled = false;
            btnBatCamera.Enabled = false;
            Application.DoEvents();

            try
            {
                var ketQua = nhanDienKhuonMatBLL.NhanDienKhuonMatTheoTaiKhoan(tenTaiKhoan, anhDaChup);

                if (ketQua.thanhCong)
                {
                    NhanDienThanhCong = true;
                    lblTrangThai.Text = "✓ " + ketQua.thongBao;
                    lblTrangThai.ForeColor = Color.FromArgb(76, 175, 80);

                    MessageBox.Show(
                        $"Xin chào!\n\n{ketQua.thongBao}",
                        "Đăng nhập thành công",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    lblTrangThai.Text = "✗ Thất bại";
                    lblTrangThai.ForeColor = Color.FromArgb(244, 67, 54);

                    MessageBox.Show(
                        ketQua.thongBao,
                        "Nhận diện thất bại",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                    btnBatCamera.Enabled = true;
                    btnXacNhan.Enabled = false;

                    if (anhDaChup != null)
                    {
                        anhDaChup.Dispose();
                        anhDaChup = null;
                    }
                }
            }
            catch (Exception ex)
            {
                lblTrangThai.Text = "✗ Lỗi";
                lblTrangThai.ForeColor = Color.FromArgb(244, 67, 54);

                MessageBox.Show(
                    $"Lỗi: {ex.Message}",
                    "Lỗi",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);

                btnBatCamera.Enabled = true;
                btnXacNhan.Enabled = false;
            }
        }

        private void NhanDienKhuonMat_FormClosing(object sender, FormClosingEventArgs e)
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