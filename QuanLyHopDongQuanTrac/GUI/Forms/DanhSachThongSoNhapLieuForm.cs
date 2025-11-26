using BLL;
using DTO;
using GUI;
using GUI.Common;
using GUI.Forms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
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
        private int nenMauHienTai = -1;
        private bool isComboBoxLoading = false;
        public DanhSachThongSoNhapLieuForm(DTO_DotQuanTrac dot)
        {
            dotHienTai = dot;
            InitializeComponent();
        }

        private void layDanhSachNenMau(int nenMauHienTai = 0)
        {
            try
            {
                isComboBoxLoading = true;
                string? maPhong = SessionStore.Current.MaPhong;

                if (string.IsNullOrEmpty(maPhong))
                {
                    MessageBox.Show("Không tìm thấy mã phòng trong phiên đăng nhập!",
                        "Lỗi session", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                var bll = new NenMauNhapLieuBLL();
                var dsNenmau = bll.LayDanhSachNenMauNhapLieu(maPhong, dotHienTai.MaDot);


                if (dsNenmau == null || dsNenmau.Count == 0)
                {
                    MessageBox.Show("Không có nền mẫu nào để nhập thông số!\n",
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
                    comboBox1.SelectedIndex = nenMauHienTai;
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

        private void DanhSachThongSoNhapLieuForm_Load(object sender, EventArgs e)
        {
            layDanhSachNenMau();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex <= 0 || comboBox1.SelectedValue == null)
                return;

            string maDN = comboBox1.SelectedValue.ToString()!;
            this.nenMauHienTai = comboBox1.SelectedIndex;
            if (currentUC != null)
            {
                if (currentUC is NenMauNhapLieuConTrol oldUC)
                {
                    oldUC.DataChanged -= NenMauConTrol_DataChanged;
                }
                panel4.Controls.Remove(currentUC);
                currentUC.Dispose();
                currentUC = null;
            }

            NenMauNhapLieuConTrol newUC = new NenMauNhapLieuConTrol(maDN);
            newUC.Dock = DockStyle.Fill;
            newUC.DataChanged += NenMauConTrol_DataChanged;
            currentUC = newUC;

            panel4.Controls.Add(currentUC);
            currentUC.BringToFront();

        }

        private void NenMauConTrol_DataChanged(object? sender, EventArgs e)
        {
            layDanhSachNenMau(this.nenMauHienTai);
        }

    }
}
