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


        public (bool success, string message, TaiKhoan?account) DangNhap(string tenTK, string matKhau) {

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
            var account = dal.KiemTraDangNhap(tenTK);
            if (account == null)
                return (false, "Tài khoản không tồn tại trong hệ thống ",null);

            //So sánh mật khẩu hash 
            bool isValid = BCrypt.Net.BCrypt.Verify(matKhau, account.matKhau);
            if (!isValid)
                return (false, "Sai mật khẩu", null);

            return (true, "Đăng nhập thành công", account);

        } 
    }
}
