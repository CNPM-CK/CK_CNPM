using BLL;
using DTO;
using Microsoft.VisualBasic.ApplicationServices;
using System;
using System.Windows.Forms;
namespace GUI.Forms
{
    public partial class DangNhap : Form
    {
        private readonly TaiKhoanBLL taiKhoanBLL = new TaiKhoanBLL() ;

        public DangNhap()
        {
            InitializeComponent();


        }


        private void textBox1_TextChanged(object sender, EventArgs e){ } // ô nhập username 


        private void textBox2_TextChanged(object sender, EventArgs e) { } // ô nhập mật khâủ


        private void button1_Click(object sender, EventArgs e) // ô đăng nhập 
        {
            string username = textBox1.Text;
            string password = textBox2.Text;

            var result = taiKhoanBLL.DangNhap(username, password);

            if(!result.success)
            {
                MessageBox.Show(result.message,"Đăng nhập thất bại", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Thông báo đăng nhập thành công
            MessageBox.Show($"Chào {result.account!.tenTK}", "Đăng nhập thành công",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);

        }


        private void splitContainer1_Panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Resize(object sender, EventArgs e)
        {

        }

        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Resize(object sender, EventArgs e)
        {

        }

        private void Form1_Resize_1(object sender, EventArgs e)
        {
            // Tính toán size chữ dựa theo chiều rộng form
            float newSize = this.Width / 40f;
            if (newSize < 8) newSize = 8; // chữ nhỏ nhất = 8pt

            // Áp dụng cho tất cả Label trong form
            ScaleFonts(this, newSize);
        }

        private void ScaleFonts(Control control, float fontSize)
        {
            foreach (Control c in control.Controls)
            {
                if (c is Label label)
                {
                    label.Font = new Font(label.Font.FontFamily, fontSize, label.Font.Style);
                }
                // Recursively apply to child controls
                ScaleFonts(c, fontSize);
            }
        }

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint_2(object sender, PaintEventArgs e)
        {

        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel3_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2TextBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label1_Click_2(object sender, EventArgs e)
        {

        }

        private void panel2_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void splitContainer1_Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void materialButton1_Click(object sender, EventArgs e)
        {

        }

        
    }
}
