using BLL;
using DTO;
using GUI;
using GUI.Common;
using GUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GUI
{
    public partial class DanhSachThongSoNhapLieuForm : Form
    {
        private DTO_DotQuanTrac dotHienTai;
        private UserControl? currentUC = null;
        public DanhSachThongSoNhapLieuForm(DTO_DotQuanTrac dot)
        {
            dotHienTai = dot;
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void DanhSachThongSoNhapLieuForm_Load(object sender, EventArgs e)
        {
            try
            {
                string? maPhong = SessionStore.Current.MaPhong;

                if (string.IsNullOrEmpty(maPhong))
                {
                    //MessageBox.Show("Không tìm thấy mã phòng trong phiên đăng nhập!",
                    //    "Lỗi session", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //return;
                    maPhong = "P003";
                }
                var bll = new NenMauNhapLieuBLL();
                var dsNenmau = bll.LayDanhSachNenMauNhapLieu(maPhong, dotHienTai.MaDot);


                if (dsNenmau == null || dsNenmau.Count == 0)
                {
                    MessageBox.Show("Không có hợp đồng nào để lập kế hoạch quan trắc!\n" +
                        "Vui lòng tạo hợp đồng trước.",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    comboBox1.Enabled = false;
                    return;
                }

                comboBox1.DataSource = dsNenmau;
                comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
                comboBox1.DisplayMember = "HienThi";
                comboBox1.ValueMember = "MaDotNen";
                if (dsNenmau != null && dsNenmau.Count > 0)
                {
                    var data = new List<NenMauNhapLieuDTO>();
                    data.Add(new NenMauNhapLieuDTO { MaDotNen = null, HienThi = "— Chọn nền mẫu —" });
                    data.AddRange(dsNenmau);

                    comboBox1.DataSource = data;
                    comboBox1.SelectedIndex = 0;
                }
                else
                {
                    MessageBox.Show("Không có nền mẫu nào trong DB!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi load danh sách nền mẫu :\n" + ex.Message,
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex <= 0 || comboBox1.SelectedValue == null)
                return;

            string maDN = comboBox1.SelectedValue.ToString()!;

            if (currentUC != null)
            {
                panel4.Controls.Remove(currentUC);
                currentUC.Dispose();
                currentUC = null;
            }

            currentUC = new NenMauNhapLieuConTrol(maDN);
            currentUC.Dock = DockStyle.Fill;

            panel4.Controls.Add(currentUC);
            currentUC.BringToFront();
        }
    }
}
