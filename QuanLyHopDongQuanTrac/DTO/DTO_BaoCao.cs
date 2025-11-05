public class DTO_BaoCao
{
    public string MaBC { get; set; }
    public string MaDot { get; set; }
    public string TenNguoiXuat { get; set; }
    public DateTime NgayXuat { get; set; }
    public int SoNenMau { get; set; }
    public int TongSoThongSo { get; set; }
    public string TrangThai { get; set; }

    public DTO_BaoCao()
    {
        MaBC = string.Empty;
        MaDot = string.Empty;
        TenNguoiXuat = string.Empty;
        NgayXuat = DateTime.Now;
        SoNenMau = 0;
        TongSoThongSo = 0;
        TrangThai = string.Empty;
    }
}