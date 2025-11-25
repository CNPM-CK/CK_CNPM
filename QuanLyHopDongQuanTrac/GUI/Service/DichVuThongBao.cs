using BLL;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace GUI.Service
{
    public class DichVuThongBao
    {
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        // 1 phút để test – khi triển khai thật nên đổi thành TimeSpan.FromHours(24)
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(1);

        public void Start()
        {
            Task.Run(async () =>
            {
                while (!_cts.IsCancellationRequested)
                {
                    try
                    {
                        var bll = new ThongBaoBLL();

                        Console.WriteLine("=== KIỂM TRA THÔNG BÁO TỰ ĐỘNG ===");

                        // ======================================================
                        // 1) XỬ LÝ ĐỢT QUAN TRẮC QUÁ HẠN
                        // ======================================================
                        Console.WriteLine("→ Kiểm tra đợt quan trắc quá hạn...");
                        bll.kiemTraVaSinhThongBaoQuaHan();

                        DataTable dsThongBao = bll.layDanhSachThongBao();
                        foreach (DataRow row in dsThongBao.Rows)
                        {
                            string maDot = row["maDot"]?.ToString();
                            string tenKH = row["tenKhachHang"]?.ToString();

                            if (string.IsNullOrEmpty(maDot))
                                continue;

                            DateTime ngayDuKien = Convert.ToDateTime(row["ngayDuKien"]);
                            int soNgayTre = (DateTime.Today - ngayDuKien.Date).Days;

                            bll.guiEmailCanhBao(maDot, tenKH, soNgayTre);
                            bll.capNhatTrangThaiEmail(maDot);

                            Console.WriteLine($"✔ Đã gửi email cảnh báo đợt {maDot}");
                        }

                        Console.WriteLine("→ Kiểm tra hợp đồng quá hạn...");
                        bll.kiemTraHopDongQuaHan();
                        Console.WriteLine("✔ Kiểm tra hợp đồng quá hạn xong.");

                        Console.WriteLine($"[{DateTime.Now}] ✓ Chu kỳ kiểm tra hoàn tất.");
                        Console.WriteLine("→ Kiểm tra nhắc ký hợp đồng...");
                        bll.sinhThongBaoNhoKyHopDong();

                        Console.WriteLine("Kiểm tra đợt quan trắc sắp đến hạn...");
                        bll.kiemTraVaSinhThongBaoSapDenHanDot();
                        Console.WriteLine("Sinh thông báo nhắc sắp đến hạn xong.");


                        DataTable dsNhacHD = bll.layDanhSachNhacKyHopDong();

                        foreach (DataRow row in dsNhacHD.Rows)
                        {
                            string email = row["email"].ToString();
                            string tenKH = row["tenDoanhNghiep"].ToString();
                            DateTime ngayBatDau = Convert.ToDateTime(row["ngayBatDau"]);
                            string tanSuat = row["tanSuatQuanTrac"].ToString();
                            string maHD = row["maHD"].ToString();
                            string maTB = row["maTB"].ToString();

                            bll.guiEmailNhacHopDong(email, tenKH, maHD, ngayBatDau, tanSuat);

                            bll.capNhatEmailDaGui_NhacHD(maTB);
                        }

                        Console.WriteLine("✔ Đã gửi mail nhắc ký hợp đồng.");



                       
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"❌ Lỗi dịch vụ thông báo: {ex.Message}");
                    }

                    // chờ tới lần chạy tiếp theo
                    await Task.Delay(_interval, _cts.Token);
                }

            }, _cts.Token);
        }

        public void Stop()
        {
            _cts.Cancel();
            Console.WriteLine("⛔ Dịch vụ thông báo đã dừng.");
        }
    }
}
