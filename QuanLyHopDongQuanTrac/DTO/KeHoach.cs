using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class KeHoach
    {
        //    // Các thuộc tính cơ bản
        //    public string maKeHoach { get; set; }
        //    public string tenKeHoach { get; set; }
        //    public string maHopDong { get; set; }
        //    public DateTime ngayBatDau { get; set; }
        //    public DateTime ngayDuKien { get; set; }
        //    public string trangThai { get; set; }
        //    public string noiDung { get; set; }

        //    // Thuộc tính bổ sung để hiển thị (không map trực tiếp từ DB)
        //    public string tenHopDong { get; set; }

        //    // Thuộc tính computed để hiển thị Hợp đồng trong DataGridView
        //    public string hopDongDisplay
        //    {
        //        get
        //        {
        //            if (!string.IsNullOrEmpty(maHopDong) && !string.IsNullOrEmpty(tenHopDong))
        //                return $"{maHopDong} - {tenHopDong}";
        //            else if (!string.IsNullOrEmpty(maHopDong))
        //                return maHopDong;
        //            else if (!string.IsNullOrEmpty(tenHopDong))
        //                return tenHopDong;
        //            return "";
        //        }
        //    }

        //    // Constructor mặc định
        //    public KeHoach()
        //    {
        //        maKeHoach = "";
        //        tenKeHoach = "";
        //        maHopDong = "";
        //        ngayBatDau = DateTime.Now;
        //        ngayDuKien = DateTime.Now;
        //        trangThai = "Chưa bắt đầu";
        //        noiDung = "";
        //        tenHopDong = "";
        //    }

        //    // Constructor đầy đủ
        //    public KeHoach(string maKeHoach, string tenKeHoach, string maHopDong,
        //                  DateTime ngayBatDau, DateTime ngayDuKien, string trangThai,
        //                  string noiDung, string tenHopDong = "")
        //    {
        //        this.maKeHoach = maKeHoach;
        //        this.tenKeHoach = tenKeHoach;
        //        this.maHopDong = maHopDong;
        //        this.ngayBatDau = ngayBatDau;
        //        this.ngayDuKien = ngayDuKien;
        //        this.trangThai = trangThai;
        //        this.noiDung = noiDung;
        //        this.tenHopDong = tenHopDong;
        //    }

        //    // Phương thức kiểm tra tính hợp lệ
        //    public bool IsValid(out string errorMessage)
        //    {
        //        errorMessage = "";

        //        if (string.IsNullOrWhiteSpace(maKeHoach))
        //        {
        //            errorMessage = "Mã kế hoạch không được để trống!";
        //            return false;
        //        }

        //        if (string.IsNullOrWhiteSpace(tenKeHoach))
        //        {
        //            errorMessage = "Tên kế hoạch không được để trống!";
        //            return false;
        //        }

        //        if (string.IsNullOrWhiteSpace(maHopDong))
        //        {
        //            errorMessage = "Mã hợp đồng không được để trống!";
        //            return false;
        //        }

        //        if (ngayDuKien < ngayBatDau)
        //        {
        //            errorMessage = "Ngày dự kiến phải sau hoặc bằng ngày bắt đầu!";
        //            return false;
        //        }

        //        if (string.IsNullOrWhiteSpace(trangThai))
        //        {
        //            errorMessage = "Trạng thái không được để trống!";
        //            return false;
        //        }

        //        return true;
        //    }

        //    // Phương thức tính số ngày thực hiện
        //    public int TinhSoNgayThucHien()
        //    {
        //        return (ngayDuKien - ngayBatDau).Days;
        //    }

        //    // Phương thức kiểm tra kế hoạch có bị trễ không
        //    public bool IsTreHan()
        //    {
        //        return DateTime.Now > ngayDuKien && trangThai != "Hoàn thành";
        //    }

        //    // Override ToString để dễ debug
        //    public override string ToString()
        //    {
        //        return $"[{maKeHoach}] {tenKeHoach} - {trangThai}";
        //    }

        //    // Phương thức Clone
        //    public KeHoach Clone()
        //    {
        //        return new KeHoach
        //        {
        //            maKeHoach = this.maKeHoach,
        //            tenKeHoach = this.tenKeHoach,
        //            maHopDong = this.maHopDong,
        //            ngayBatDau = this.ngayBatDau,
        //            ngayDuKien = this.ngayDuKien,
        //            trangThai = this.trangThai,
        //            noiDung = this.noiDung,
        //            tenHopDong = this.tenHopDong
        //        };
        //    }
    }
}