using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BCrypt.Net;
using DAL;
using DTO;


namespace BLL
{
    public  class TaiKhoanBLL
    {

        private readonly DatabaseAccess dal = new DatabaseAccess();


        public (bool success, string message, TaiKhoan?account) dangNhap(string tenTK, string matKhau) {

            //Trường hợp hai trường đều rỗng :
            if (string.IsNullOrWhiteSpace(tenTK) && string.IsNullOrWhiteSpace(matKhau))
                return (false, "Vui lòng nhập đầy đủ tên tài khoản và mật khẩu", null);

            //Trường hợp 1 trong 2 trường rỗng :
            if (string.IsNullOrWhiteSpace(tenTK) || string.IsNullOrWhiteSpace(matKhau))
                return (false, "Vui lòng nhập đầy đủ thông tin", null);

            //Kiểm tra kí tự chứa khoảng trắng
            if (Regex.IsMatch(tenTK, @"\s") || Regex.IsMatch(matKhau, @"\s"))
                return (false, "Thông tin đăng nhập chứa kí tự không hợp lệ ",null);

            //Kiểm tra tính tồn tại của tài khoản 
            var account = dal.kiemTraDangNhap(tenTK);
            if (account == null)
                return (false, "Tài khoản không tồn tại trong hệ thống ",null);

            //So sánh mật khẩu hash 
            bool isValid = BCrypt.Net.BCrypt.Verify(matKhau, account.matKhau);
            if (!isValid)
                return (false, "Sai mật khẩu", null);

            return (true, "Đăng nhập thành công", account);

        }


        public (bool success, string message) doiMatKhau(
      string tenTK,
      string matKhauCu,
      string matKhauMoi,
      string xacNhan)
        {
            // 1. Validate cơ bản
            if (string.IsNullOrWhiteSpace(matKhauCu) ||
                string.IsNullOrWhiteSpace(matKhauMoi) ||
                string.IsNullOrWhiteSpace(xacNhan))
            {
                return (false, "Vui lòng nhập đầy đủ thông tin");
            }

            if (Regex.IsMatch(matKhauMoi, @"\s"))
                return (false, "Mật khẩu mới không được chứa khoảng trắng!");

            // 🔹 Điều kiện mới: mật khẩu phải mạnh
            string pattern = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&.#^()_+\-=]).{8,}$";
            if (!Regex.IsMatch(matKhauMoi, pattern))
            {
                return (false, "Mật khẩu mới bao gồm 8 kí tự trở lên, gồm chữ in hoa, chữ thường và kí tự đặc biệt!");
            }

            if (matKhauMoi != xacNhan)
                return (false, "Xác nhận không trùng khớp!");

            // 2. Lấy tài khoản hiện tại
            TaiKhoan account = dal.kiemTraDangNhap(tenTK);
            if (account == null)
                return (false, "Tài khoản không tồn tại !");

            // 3. Kiểm tra mật khẩu cũ
            bool oldValid = BCrypt.Net.BCrypt.Verify(matKhauCu, account.matKhau);
            if (!oldValid)
                return (false,"Mật khẩu cũ không chính xác !");

            // 4. Kiểm tra trùng mật khẩu cũ
            bool sameAsOld = BCrypt.Net.BCrypt.Verify(matKhauMoi, account.matKhau);
            if (sameAsOld)
                return (false, "Mật khẩu mới không được trùng với mật khẩu cũ !");

            // 5. Hash mật khẩu mới
            string newHash = BCrypt.Net.BCrypt.HashPassword(matKhauMoi);

            // 6. Cập nhật DB
            var result = dal.doiMatKhau(tenTK, newHash);
            return (result.Success, result.Message);
        }

        public TaiKhoan? layThongTinTaiKhoan(string tenTK)
        {
            if (string.IsNullOrWhiteSpace(tenTK))
                return null;

            var account = dal.kiemTraDangNhap(tenTK);
            return account;
        }
    }
}
