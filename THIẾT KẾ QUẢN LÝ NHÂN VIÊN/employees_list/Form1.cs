using Guna.UI2.WinForms;
using Microsoft.Data.SqlClient;
using System.Data;
namespace employees_list
{
    public partial class Form1 : Form
    {
        String connectionStr = @"Server=.\SQLEXPRESS;Database=test_QL_NHANVIEN;Trusted_Connection=True;TrustServerCertificate=True;";
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            showData.CellPainting += showData_CellPainting;
            laydsnhanvien();
            showData.CellContentClick += showData_CellContentClick;
        }
        public void laydsnhanvien()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionStr))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("dbo.laydsnhanvien", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    adapter.Fill(table);
                    showData.DataSource = table;
                    if (!showData.Columns.Contains("ThaoTac"))
                    {
                        DataGridViewButtonColumn col = new DataGridViewButtonColumn();
                        col.Name = "ThaoTac";
                        col.HeaderText = "Thao tác";
                        col.Text = "Xóa";
                        col.UseColumnTextForButtonValue = true;
                        showData.Columns.Add(col);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error connection : " + ex.Message);
            }
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void buttonPDF_Click(object sender, EventArgs e)
        {

        }

        private void panel5_Paint(object sender, PaintEventArgs e)
        {

        }


        private void guna2Button6_Click(object sender, EventArgs e)
        {
        }

        private void Guna2CircleButton1_Click(object sender, EventArgs e)
        {
            guna2Panel2Setting.Visible = !guna2Panel2Setting.Visible;

        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            guna2Panel2Setting.Visible = false;

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void showData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == showData.Columns["ThaoTac"].Index)
            {
                // Lấy tọa độ click
                var cellRect = showData.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                int iconSize = 50;
                int padding = 5;

                var editRect = new Rectangle(
                    cellRect.Left + padding,
                    cellRect.Top + (cellRect.Height - iconSize) / 2,
                    iconSize,
                    iconSize
                );

                var deleteRect = new Rectangle(
                    cellRect.Left + padding + iconSize + 10,
                    cellRect.Top + (cellRect.Height - iconSize) / 2,
                    iconSize,
                    iconSize
                );

                //Xác định click vào icon nào
                Point clickPoint = showData.PointToClient(Cursor.Position);

                if (editRect.Contains(clickPoint))
                {
                    string maNV = showData.Rows[e.RowIndex].Cells["MaNV"].Value.ToString();
                    MessageBox.Show("Sửa nhân viên: " + maNV);
                    // TODO: mở form sửa
                }
                else if (deleteRect.Contains(clickPoint))
                {
                    string maNV = showData.Rows[e.RowIndex].Cells["MaNV"].Value.ToString();
                    DialogResult result = MessageBox.Show("Xóa NV: " + maNV + " ?",
                                                          "Xác nhận",
                                                          MessageBoxButtons.YesNo,
                                                          MessageBoxIcon.Warning);
                    if (result == DialogResult.Yes)
                    {
                        // TODO: viết lệnh DELETE SQL ở đây
                        MessageBox.Show("Đã xóa " + maNV);
                    }
                }
            }
        }
        private void showData_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == showData.Columns["ThaoTac"].Index)
            {
                e.PaintBackground(e.CellBounds, true);

                int iconSize = 20;
                int padding = 5;

                var editRect = new Rectangle(
                    e.CellBounds.Left + padding,
                    e.CellBounds.Top + (e.CellBounds.Height - iconSize) / 2,
                    iconSize,
                    iconSize
                );
                e.Graphics.DrawImage(Properties.Resources.edit, editRect);

                var deleteRect = new Rectangle(
                    e.CellBounds.Left + padding + iconSize + 10,
                    e.CellBounds.Top + (e.CellBounds.Height - iconSize) / 2,
                    iconSize,
                    iconSize
                );
                e.Graphics.DrawImage(Properties.Resources.trash, deleteRect);

                e.Handled = true;
            }
        }

    }
}
