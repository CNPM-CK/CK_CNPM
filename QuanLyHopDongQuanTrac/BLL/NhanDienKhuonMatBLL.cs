using DAL;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;

namespace BLL
{
    public class NhanDienKhuonMatBLL
    {
        private readonly NhanDienKhuonMatDAL dal = new NhanDienKhuonMatDAL();
        private readonly CascadeClassifier boPhatHienKhuonMat;
        private readonly EigenFaceRecognizer boNhanDien;

        // ===== GIẢM ĐỘ KHÓ CHO WEBCAM THÔNG THƯỜNG =====
        private const double NGUONG_TIN_CAY_TUYET_VOI = 3000;   // Tăng lên (dễ hơn)
        private const double NGUONG_TIN_CAY_TOT = 4500;         // Tăng lên (dễ hơn)
        private const double NGUONG_TIN_CAY_CHAP_NHAN = 6000;   // Tăng từ 4500 lên 6000 (DỄ NHIỀU)

        private const double NGUONG_TU_CHOI_TUYET_DOI = 7000;   // Tăng lên (dễ hơn)

        private const int KICH_THUOC_KHUON_MAT = 500;
        private const int SO_THANH_PHAN_CHINH = 150;

        // ===== GIẢM YÊU CẦU TỶ LỆ KHUÔN MẶT =====
        private const double TY_LE_KHUON_MAT_TOI_THIEU = 0.35;  // Giảm từ 0.50 xuống 0.35 (DỄ NHIỀU)
        private const double TY_LE_KHUON_MAT_TOI_DA = 0.99;     // Tăng lên 0.99 (gần như không giới hạn)

        public NhanDienKhuonMatBLL()
        {
            try
            {
                string cascadePath = TimFileCascade();

                if (string.IsNullOrEmpty(cascadePath))
                {
                    throw new FileNotFoundException(
                        "Không tìm thấy file 'haarcascade_frontalface_default.xml'!");
                }

                boPhatHienKhuonMat = new CascadeClassifier(cascadePath);
                boNhanDien = new EigenFaceRecognizer(SO_THANH_PHAN_CHINH, NGUONG_TU_CHOI_TUYET_DOI);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khởi tạo Face Recognition: {ex.Message}", ex);
            }
        }

        private string TimFileCascade()
        {
            string[] cacViTriTimKiem = new[]
            {
                "haarcascade_frontalface_default.xml",
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "haarcascade_frontalface_default.xml"),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "haarcascade_frontalface_default.xml"),
                Path.Combine(Directory.GetCurrentDirectory(), "haarcascade_frontalface_default.xml"),
                @"C:\opencv\data\haarcascades\haarcascade_frontalface_default.xml"
            };

            foreach (string viTri in cacViTriTimKiem)
            {
                if (File.Exists(viTri))
                    return viTri;
            }

            return null;
        }

        private (bool hopLe, string thongBao, double tyLe) KiemTraTyLeKhuonMat(Rectangle khuonMat, Size kichThuocKhungHinh)
        {
            try
            {
                double dienTichKhuonMat = khuonMat.Width * khuonMat.Height;
                double dienTichKhungHinh = kichThuocKhungHinh.Width * kichThuocKhungHinh.Height;
                double tyLe = dienTichKhuonMat / dienTichKhungHinh;

                System.Diagnostics.Debug.WriteLine(
                    $"[Face Size] {khuonMat.Width}x{khuonMat.Height} / " +
                    $"{kichThuocKhungHinh.Width}x{kichThuocKhungHinh.Height} = {tyLe:P2}");

                if (tyLe < TY_LE_KHUON_MAT_TOI_THIEU)
                {
                    return (false,
                        $"⚠️ Khuôn mặt quá nhỏ ({tyLe:P0})\n" +
                        $"📌 Di chuyển GẦN HƠN camera",
                        tyLe);
                }

                if (tyLe > TY_LE_KHUON_MAT_TOI_DA)
                {
                    return (false,
                        $"⚠️ Khuôn mặt quá lớn ({tyLe:P0})\n" +
                        $"📌 Di chuyển RA XA camera",
                        tyLe);
                }

                return (true, $"✓ Kích thước OK ({tyLe:P0})", tyLe);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"KiemTraTyLeKhuonMat Error: {ex.Message}");
                return (false, "Lỗi kiểm tra tỷ lệ", 0);
            }
        }

        private (bool hopLe, string thongBao) KiemTraViTriKhuonMat(Rectangle khuonMat, Size kichThuocKhungHinh)
        {
            try
            {
                int tamX = khuonMat.X + khuonMat.Width / 2;
                int tamY = khuonMat.Y + khuonMat.Height / 2;
                int tamKhungX = kichThuocKhungHinh.Width / 2;
                int tamKhungY = kichThuocKhungHinh.Height / 2;

                double doLechX = Math.Abs(tamX - tamKhungX) / (double)kichThuocKhungHinh.Width;
                double doLechY = Math.Abs(tamY - tamKhungY) / (double)kichThuocKhungHinh.Height;

                const double DO_LECH_TOI_DA = 0.30; // Tăng từ 0.25 lên 0.30 (DỄ HƠN)

                if (doLechX > DO_LECH_TOI_DA || doLechY > DO_LECH_TOI_DA)
                {
                    string huong = "";
                    if (tamX < tamKhungX - kichThuocKhungHinh.Width * 0.15) huong += "PHẢI ";
                    if (tamX > tamKhungX + kichThuocKhungHinh.Width * 0.15) huong += "TRÁI ";
                    if (tamY < tamKhungY - kichThuocKhungHinh.Height * 0.15) huong += "XUỐNG ";
                    if (tamY > tamKhungY + kichThuocKhungHinh.Height * 0.15) huong += "LÊN ";

                    return (false, $"⚠️ Di chuyển {huong}để căn giữa");
                }

                return (true, "✓ Vị trí OK");
            }
            catch
            {
                return (true, "");
            }
        }

        private Image<Gray, byte> ChuanHoaKhuonMat(Image<Gray, byte> khuonMat)
        {
            if (khuonMat == null)
                return null;

            try
            {
                var resized = khuonMat.Resize(KICH_THUOC_KHUON_MAT, KICH_THUOC_KHUON_MAT, Inter.Lanczos4);

                var equalized = resized.Clone();
                CvInvoke.EqualizeHist(resized, equalized);
                resized.Dispose();

                var smoothed = equalized.Clone();
                CvInvoke.GaussianBlur(equalized, smoothed, new Size(3, 3), 0);
                equalized.Dispose();

                double meanBrightness = smoothed.GetAverage().Intensity;
                double targetBrightness = 125.0;
                double alpha = targetBrightness / meanBrightness;

                if (alpha > 0.7 && alpha < 1.5)
                {
                    var normalized = smoothed.Mul(alpha);
                    smoothed.Dispose();
                    return normalized;
                }

                return smoothed;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ChuanHoaKhuonMat Error: {ex.Message}");
                try
                {
                    return khuonMat.Resize(KICH_THUOC_KHUON_MAT, KICH_THUOC_KHUON_MAT, Inter.Cubic);
                }
                catch
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// GIẢM ĐỘ KHÓ CỦA KIỂM TRA CHẤT LƯỢNG
        /// </summary>
        private (bool hopLe, string thongBao, double doSang, double doNet) KiemTraChatLuongAnh(Image<Gray, byte> khuonMat)
        {
            if (khuonMat == null)
                return (false, "Ảnh không hợp lệ", 0, 0);

            try
            {
                // 1. Kiểm tra độ sáng - DỄ NHIỀU
                double doSangTB = khuonMat.GetAverage().Intensity;

                if (doSangTB < 35)  // Giảm từ 45 xuống 35 (chấp nhận tối hơn)
                    return (false, "❌ Quá tối! Bật thêm đèn", doSangTB, 0);

                if (doSangTB > 220) // Tăng từ 210 lên 220 (chấp nhận sáng hơn)
                    return (false, "❌ Quá sáng! Tránh ánh sáng trực tiếp", doSangTB, 0);

                // 2. Kiểm tra độ tương phản - DỄ NHIỀU
                MCvScalar mean = new MCvScalar();
                MCvScalar stdDev = new MCvScalar();
                CvInvoke.MeanStdDev(khuonMat, ref mean, ref stdDev);
                double doTuongPhan = stdDev.V0;

                if (doTuongPhan < 15) // Giảm từ 25 xuống 15 (DỄ NHIỀU)
                    return (false, "❌ Độ tương phản thấp! Cải thiện ánh sáng", doSangTB, 0);

                // 3. Kiểm tra độ nét - DỄ NHIỀU
                double variance = 0;
                using (Mat laplacian = new Mat())
                {
                    CvInvoke.Laplacian(khuonMat, laplacian, DepthType.Cv64F);
                    MCvScalar meanLap = new MCvScalar();
                    MCvScalar stdDevLap = new MCvScalar();
                    CvInvoke.MeanStdDev(laplacian, ref meanLap, ref stdDevLap);
                    variance = stdDevLap.V0 * stdDevLap.V0;
                }

                if (variance < 50) // Giảm từ 80 xuống 50 (DỄ NHIỀU)
                    return (false, "❌ Ảnh bị mờ! Giữ camera ổn định", doSangTB, variance);

                return (true, "✓ Chất lượng tốt", doSangTB, variance);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"KiemTraChatLuongAnh Error: {ex.Message}");
                return (true, "Bỏ qua kiểm tra chất lượng", 0, 0);
            }
        }

        private Bitmap ConvertGrayImageToBitmap(Image<Gray, byte> image)
        {
            if (image == null)
                return null;

            try
            {
                Bitmap bitmap = new Bitmap(image.Width, image.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                var palette = bitmap.Palette;
                for (int i = 0; i < 256; i++)
                {
                    palette.Entries[i] = Color.FromArgb(i, i, i);
                }
                bitmap.Palette = palette;

                System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(
                    new Rectangle(0, 0, image.Width, image.Height),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    bitmap.PixelFormat);

                try
                {
                    byte[] buffer = new byte[image.Bytes.Length];
                    Marshal.Copy(image.Mat.DataPointer, buffer, 0, buffer.Length);
                    Marshal.Copy(buffer, 0, bmpData.Scan0, buffer.Length);
                }
                finally
                {
                    bitmap.UnlockBits(bmpData);
                }

                return bitmap;
            }
            catch
            {
                return null;
            }
        }

        private Image<Gray, byte> ConvertByteArrayToGrayImage(byte[] data)
        {
            if (data == null || data.Length == 0)
                return null;

            try
            {
                Mat m = new Mat();
                CvInvoke.Imdecode(data, ImreadModes.Grayscale, m);
                if (m == null || m.IsEmpty)
                {
                    m?.Dispose();
                    return null;
                }

                var img = m.ToImage<Gray, byte>();
                m.Dispose();
                return img;
            }
            catch
            {
                return null;
            }
        }

        public (bool thanhCong, string thongBao) DangKyKhuonMat(string tenTK, Image<Bgr, byte> hinhAnh)
        {
            if (string.IsNullOrWhiteSpace(tenTK))
                return (false, "Tên tài khoản không hợp lệ!");

            if (hinhAnh == null)
                return (false, "Hình ảnh không hợp lệ!");

            Image<Gray, byte> anhXam = null;
            Image<Gray, byte> vungKhuonMat = null;

            try
            {
                anhXam = hinhAnh.Convert<Gray, byte>();

                // GIẢM ĐỘ KHÓ CỦA PHÁT HIỆN KHUÔN MẶT
                Rectangle[] cacKhuonMat = boPhatHienKhuonMat.DetectMultiScale(
                    anhXam,
                    scaleFactor: 1.08,          // Tăng từ 1.05 lên 1.08 (nhanh hơn)
                    minNeighbors: 8,            // Giảm từ 12 xuống 8 (DỄ NHIỀU)
                    minSize: new Size(100, 100), // Giảm từ 150 xuống 100 (DỄ NHIỀU)
                    maxSize: new Size(900, 900)); // Tăng từ 800 lên 900

                if (cacKhuonMat.Length == 0)
                {
                    return (false,
                        "❌ Không phát hiện khuôn mặt!\n\n" +
                        "📌 Đảm bảo:\n" +
                        "• Ánh sáng đủ\n" +
                        "• Nhìn thẳng camera\n" +
                        "• Khuôn mặt trong khung hình\n" +
                        "• Không đeo khẩu trang/kính râm");
                }

                if (cacKhuonMat.Length > 1)
                {
                    return (false,
                        $"❌ Phát hiện {cacKhuonMat.Length} khuôn mặt!\n" +
                        "Chỉ 1 người trong khung hình!");
                }

                var kiemTraTyLe = KiemTraTyLeKhuonMat(cacKhuonMat[0], anhXam.Size);
                if (!kiemTraTyLe.hopLe)
                {
                    return (false, kiemTraTyLe.thongBao);
                }

                var kiemTraViTri = KiemTraViTriKhuonMat(cacKhuonMat[0], anhXam.Size);
                if (!kiemTraViTri.hopLe)
                {
                    return (false, kiemTraViTri.thongBao);
                }

                var khuonMatGoc = anhXam.Copy(cacKhuonMat[0]);

                var kiemTraChatLuong = KiemTraChatLuongAnh(khuonMatGoc);
                if (!kiemTraChatLuong.hopLe)
                {
                    khuonMatGoc.Dispose();
                    return (false, kiemTraChatLuong.thongBao);
                }

                vungKhuonMat = ChuanHoaKhuonMat(khuonMatGoc);
                khuonMatGoc.Dispose();

                if (vungKhuonMat == null)
                    return (false, "Lỗi xử lý ảnh!");

                byte[] duLieuKhuonMat;
                Bitmap bmp = ConvertGrayImageToBitmap(vungKhuonMat);
                if (bmp == null)
                    return (false, "Lỗi chuyển đổi ảnh!");

                using (bmp)
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    duLieuKhuonMat = ms.ToArray();
                }

                if (duLieuKhuonMat == null || duLieuKhuonMat.Length == 0)
                    return (false, "Lỗi chuyển đổi dữ liệu!");

                var ketQua = dal.LuuDuLieuKhuonMat(tenTK, duLieuKhuonMat);

                if (ketQua.thanhCong)
                {
                    return (true,
                        "✅ Đăng ký thành công!\n\n" +
                        $"📊 Độ sáng: {kiemTraChatLuong.doSang:F0}\n" +
                        $"📊 Độ nét: {kiemTraChatLuong.doNet:F0}\n" +
                        $"📊 Tỷ lệ: {kiemTraTyLe.tyLe:P0}\n\n" +
                        "🔐 Sử dụng Face ID để đăng nhập!");
                }
                else
                {
                    return (false, $"Lỗi lưu: {ketQua.thongBao}");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi: {ex.Message}");
            }
            finally
            {
                anhXam?.Dispose();
                vungKhuonMat?.Dispose();
            }
        }

        public (bool thanhCong, string thongBao) NhanDienKhuonMatTheoTaiKhoan(string tenTK, Image<Bgr, byte> hinhAnh)
        {
            if (string.IsNullOrWhiteSpace(tenTK))
                return (false, "Tên tài khoản không hợp lệ!");

            if (hinhAnh == null)
                return (false, "Hình ảnh không hợp lệ!");

            Image<Gray, byte> anhXam = null;
            Image<Gray, byte> vungKhuonMat = null;
            Image<Gray, byte> khuonMatDaDangKy = null;

            try
            {
                byte[] duLieuKhuonMatDaDangKy = dal.LayDuLieuKhuonMat(tenTK);

                if (duLieuKhuonMatDaDangKy == null || duLieuKhuonMatDaDangKy.Length == 0)
                {
                    return (false,
                        "❌ Tài khoản chưa đăng ký Face ID!\n" +
                        "Vui lòng đăng ký trước.");
                }

                khuonMatDaDangKy = ConvertByteArrayToGrayImage(duLieuKhuonMatDaDangKy);

                if (khuonMatDaDangKy == null ||
                    khuonMatDaDangKy.Width != KICH_THUOC_KHUON_MAT ||
                    khuonMatDaDangKy.Height != KICH_THUOC_KHUON_MAT)
                {
                    return (false,
                        "❌ Dữ liệu không hợp lệ!\n" +
                        "Vui lòng đăng ký lại.");
                }

                anhXam = hinhAnh.Convert<Gray, byte>();

                Rectangle[] cacKhuonMat = boPhatHienKhuonMat.DetectMultiScale(
                    anhXam,
                    scaleFactor: 1.08,
                    minNeighbors: 8,
                    minSize: new Size(100, 100),
                    maxSize: new Size(900, 900));

                if (cacKhuonMat.Length == 0)
                {
                    return (false,
                        "❌ Không phát hiện khuôn mặt!\n" +
                        "Thử lại với ánh sáng tốt hơn");
                }

                if (cacKhuonMat.Length > 1)
                {
                    return (false,
                        $"❌ Phát hiện {cacKhuonMat.Length} khuôn mặt!\n" +
                        "Chỉ 1 người trong khung hình!");
                }

                var kiemTraTyLe = KiemTraTyLeKhuonMat(cacKhuonMat[0], anhXam.Size);
                if (!kiemTraTyLe.hopLe)
                {
                    return (false, kiemTraTyLe.thongBao);
                }

                var kiemTraViTri = KiemTraViTriKhuonMat(cacKhuonMat[0], anhXam.Size);
                if (!kiemTraViTri.hopLe)
                {
                    return (false, kiemTraViTri.thongBao);
                }

                var khuonMatGoc = anhXam.Copy(cacKhuonMat[0]);

                var kiemTraChatLuong = KiemTraChatLuongAnh(khuonMatGoc);
                if (!kiemTraChatLuong.hopLe)
                {
                    khuonMatGoc.Dispose();
                    return (false, kiemTraChatLuong.thongBao);
                }

                vungKhuonMat = ChuanHoaKhuonMat(khuonMatGoc);
                khuonMatGoc.Dispose();

                if (vungKhuonMat == null)
                    return (false, "Lỗi xử lý ảnh!");

                List<Mat> danhSachMat = new List<Mat> { khuonMatDaDangKy.Mat };
                List<int> danhSachNhan = new List<int> { 0 };

                using (var faceMats = new VectorOfMat(danhSachMat.ToArray()))
                using (var labelVector = new VectorOfInt(danhSachNhan.ToArray()))
                {
                    boNhanDien.Train(faceMats, labelVector);
                }

                var ketQua = boNhanDien.Predict(vungKhuonMat.Mat);
                int nhanDuDoan = ketQua.Label;
                double doTinCay = ketQua.Distance;

                System.Diagnostics.Debug.WriteLine(
                    $"[Face Auth - {tenTK}] Label={nhanDuDoan}, Distance={doTinCay:F2}, " +
                    $"Ratio={kiemTraTyLe.tyLe:P2}, Bright={kiemTraChatLuong.doSang:F0}");

                if (nhanDuDoan == 0 && doTinCay < NGUONG_TIN_CAY_CHAP_NHAN)
                {
                    string danhGia;
                    string mauSac;

                    if (doTinCay < NGUONG_TIN_CAY_TUYET_VOI)
                    {
                        danhGia = "Tuyệt vời";
                        mauSac = "🟢";
                    }
                    else if (doTinCay < NGUONG_TIN_CAY_TOT)
                    {
                        danhGia = "Tốt";
                        mauSac = "🟡";
                    }
                    else
                    {
                        danhGia = "Chấp nhận";
                        mauSac = "🟠";
                    }

                    return (true,
                            $"✅ {mauSac} Xác thực thành công!\n\n" +
                            $"👤 Tài khoản: {tenTK}\n" +
                            $"🔐 Độ tin cậy: {danhGia}\n" +
                            $"📊 Distance: {doTinCay:F0} / {NGUONG_TIN_CAY_CHAP_NHAN}\n" +
                            $"📐 Tỷ lệ: {kiemTraTyLe.tyLe:P0}\n" +
                            $"💡 Độ sáng: {kiemTraChatLuong.doSang:F0}");
                }
                else
                {
                    string lyDo;
                    if (nhanDuDoan != 0)
                    {
                        lyDo = "Label không khớp";
                    }
                    else
                    {
                        double chenhLech = doTinCay - NGUONG_TIN_CAY_CHAP_NHAN;
                        double phanTram = (chenhLech / NGUONG_TIN_CAY_CHAP_NHAN) * 100;
                        lyDo = $"Distance quá cao (+{chenhLech:F0}, +{phanTram:F1}%)";
                    }

                    System.Diagnostics.Debug.WriteLine(
                        $"❌ [FAILED] {tenTK}: {lyDo}, Distance={doTinCay:F0}");

                    return (false,
                            $"❌ Xác thực thất bại!\n\n" +
                            $"Khuôn mặt không khớp với '{tenTK}'\n\n" +
                            $"📊 Distance: {doTinCay:F0} (yêu cầu < {NGUONG_TIN_CAY_CHAP_NHAN})\n" +
                            $"📐 Tỷ lệ: {kiemTraTyLe.tyLe:P0}\n\n" +
                            $"📌 Thử lại hoặc đăng ký lại Face ID");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"NhanDienKhuonMatTheoTaiKhoan Error: {ex.Message}");
                return (false, $"❌ Lỗi: {ex.Message}");
            }
            finally
            {
                anhXam?.Dispose();
                vungKhuonMat?.Dispose();
                khuonMatDaDangKy?.Dispose();
            }
        }

        public bool KiemTraKhuonMatDaTonTai(string tenTK)
        {
            try
            {
                return dal.KiemTraDaDangKyFace(tenTK);
            }
            catch
            {
                return false;
            }
        }

        public (bool thanhCong, string thongBao) XoaKhuonMat(string tenTK)
        {
            try
            {
                return dal.XoaDuLieuKhuonMat(tenTK);
            }
            catch (Exception ex)
            {
                return (false, $"Lỗi xóa: {ex.Message}");
            }
        }
    }
}