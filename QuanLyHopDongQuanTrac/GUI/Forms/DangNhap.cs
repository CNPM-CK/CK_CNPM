using BLL;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Windows.Forms;
namespace GUI.Forms
{
    public partial class DangNhap : Form
    {

        private readonly TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL();

        public DangNhap()
        {
            InitializeComponent();
            this.AcceptButton = button1;
            textBoxmatkhau.KeyDown += textBoxMatKhau_KeyDown;
        }


        private void textBox1_TextChanged(object sender, EventArgs e) // ô nhập tài khoản
        {
        }

        private void textBox2_TextChanged(object sender, EventArgs e) // ô nhập mật khẩu
        {
        }

        private void button1_Click(object sender, EventArgs e) // nút đăng nhập
        {
            string username = textBox1.Text.Trim();
            string password = textBoxmatkhau.Text.Trim();

            var result = taiKhoanBLL.DangNhap(username, password);
            if (!result.success)
            {
                MessageBox.Show(result.message, "Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //Chuyển hướng nếu vai trò là admin
            if (result.account!.vaiTro == 1)
            {

                DanhSachNhanVien listEmployees = new DanhSachNhanVien();
                listEmployees.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show($"Chào {result.account.tenTK}, bạn không có quyền truy cập danh sách nhân viên.",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
        private void textBoxMatKhau_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                button1.PerformClick(); 
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e) // nút quên mật khẩu
        {
            QuenMatKhau1 quenMKForm = new QuenMatKhau1();
            quenMKForm.Show();
            this.Hide();
        }  
    }
}
