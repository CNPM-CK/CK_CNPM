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

        // khoảng thời gian kiểm tra (1 phút để test, có thể đổi thành TimeSpan.FromHours(24))
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

                        // B1. Kiểm tra và sinh thông báo quá hạn mới vào DB
                        bll.kiemTraVaSinhThongBaoQuaHan();

                        // B2. Lấy danh sách thông báo quá hạn
                        DataTable dsThongBao = bll.layDanhSachThongBao();
                        var ngayHomNay = DateTime.Today;

                        // B3. Duyệt qua các thông báo để gửi email cảnh báo
                        foreach (DataRow row in dsThongBao.Rows)
                        {
                            string maDot = row["maDot"].ToString();
                            string tenKH = row["tenKhachHang"].ToString();
                            // Lấy ngày dự kiến từ DB (phải thêm cột ngayDuKien trong truy vấn sp)
                            DateTime ngayDuKien = Convert.ToDateTime(row["ngayDuKien"]);
                            int soNgayTre = (DateTime.Today - ngayDuKien.Date).Days;


                            //// chỉ gửi nếu là thông báo mới tạo (ngayTao = hôm nay)
                            //if (ngayTao.Date != ngayHomNay)
                            //    continue;

                            bll.guiEmailCanhBao(maDot, tenKH, soNgayTre);
                            Console.WriteLine($"Gửi email cảnh báo cho đợt {maDot} ({tenKH}) thành công.");
                            bll.capNhatTrangThaiEmail(maDot);
                        }

                        Console.WriteLine($"[{DateTime.Now}] Đã kiểm tra & gửi cảnh báo quá hạn xong.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Lỗi BackgroundService: {ex.Message}");
                    }

                    // Chờ đến lần kiểm tra tiếp theo
                    await Task.Delay(_interval, _cts.Token);
                }
            }, _cts.Token);
        }

        public void Stop()
        {
            _cts.Cancel();
            Console.WriteLine("Dịch vụ thông báo đã dừng.");
        }
    }
}
