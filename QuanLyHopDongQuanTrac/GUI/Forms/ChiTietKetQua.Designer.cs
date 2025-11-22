namespace GUI.Forms
{
    partial class ChiTietKetQua
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            label7 = new Label();
            label6 = new Label();
            txtMaKQ = new TextBox();
            txtGhiChu = new TextBox();
            dtpNgayDo = new DateTimePicker();
            txtNenMau = new TextBox();
            txtTrangThai = new TextBox();
            txtNhanVienNhap = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            panel2 = new Panel();
            label1 = new Label();
            btnXuatFile = new Button();
            btnXacNhan = new Button();
            btnHuyXacNhan = new Button();
            dgvChiTiet = new DataGridView();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(txtMaKQ);
            panel1.Controls.Add(txtGhiChu);
            panel1.Controls.Add(dtpNgayDo);
            panel1.Controls.Add(txtNenMau);
            panel1.Controls.Add(txtTrangThai);
            panel1.Controls.Add(txtNhanVienNhap);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(panel2);
            panel1.Controls.Add(btnXuatFile);
            panel1.Controls.Add(btnXacNhan);
            panel1.Controls.Add(btnHuyXacNhan);
            panel1.Controls.Add(dgvChiTiet);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1295, 771);
            panel1.TabIndex = 0;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label7.ForeColor = Color.Green;
            label7.Location = new Point(748, 116);
            label7.Name = "label7";
            label7.Size = new Size(108, 25);
            label7.TabIndex = 22;
            label7.Text = "Mã kết quả";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label6.ForeColor = Color.Green;
            label6.Location = new Point(748, 222);
            label6.Name = "label6";
            label6.Size = new Size(77, 25);
            label6.TabIndex = 21;
            label6.Text = "Ghi chú";
            // 
            // txtMaKQ
            // 
            txtMaKQ.Location = new Point(875, 118);
            txtMaKQ.Name = "txtMaKQ";
            txtMaKQ.ReadOnly = true;
            txtMaKQ.Size = new Size(234, 27);
            txtMaKQ.TabIndex = 20;
            // 
            // txtGhiChu
            // 
            txtGhiChu.Location = new Point(875, 223);
            txtGhiChu.Name = "txtGhiChu";
            txtGhiChu.ReadOnly = true;
            txtGhiChu.Size = new Size(234, 27);
            txtGhiChu.TabIndex = 19;
            // 
            // dtpNgayDo
            // 
            dtpNgayDo.Enabled = false;
            dtpNgayDo.Location = new Point(278, 225);
            dtpNgayDo.Name = "dtpNgayDo";
            dtpNgayDo.Size = new Size(233, 27);
            dtpNgayDo.TabIndex = 18;
            // 
            // txtNenMau
            // 
            txtNenMau.Location = new Point(278, 117);
            txtNenMau.Name = "txtNenMau";
            txtNenMau.ReadOnly = true;
            txtNenMau.Size = new Size(233, 27);
            txtNenMau.TabIndex = 17;
            // 
            // txtTrangThai
            // 
            txtTrangThai.Location = new Point(875, 167);
            txtTrangThai.Name = "txtTrangThai";
            txtTrangThai.ReadOnly = true;
            txtTrangThai.Size = new Size(234, 27);
            txtTrangThai.TabIndex = 15;
            // 
            // txtNhanVienNhap
            // 
            txtNhanVienNhap.Location = new Point(278, 167);
            txtNhanVienNhap.Name = "txtNhanVienNhap";
            txtNhanVienNhap.ReadOnly = true;
            txtNhanVienNhap.Size = new Size(233, 27);
            txtNhanVienNhap.TabIndex = 14;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label5.ForeColor = Color.Green;
            label5.Location = new Point(748, 169);
            label5.Name = "label5";
            label5.Size = new Size(99, 25);
            label5.TabIndex = 13;
            label5.Text = "Trạng thái";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label4.ForeColor = Color.Green;
            label4.Location = new Point(158, 225);
            label4.Name = "label4";
            label4.Size = new Size(84, 25);
            label4.TabIndex = 10;
            label4.Text = "Ngày đo";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label3.ForeColor = Color.Green;
            label3.Location = new Point(158, 169);
            label3.Name = "label3";
            label3.Size = new Size(113, 25);
            label3.TabIndex = 9;
            label3.Text = "Người nhập";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label2.ForeColor = Color.Green;
            label2.Location = new Point(158, 117);
            label2.Name = "label2";
            label2.Size = new Size(129, 25);
            label2.TabIndex = 8;
            label2.Text = "Đợt quan trắc";
            // 
            // panel2
            // 
            panel2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            panel2.BackColor = Color.FromArgb(0, 152, 70);
            panel2.Controls.Add(label1);
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1295, 89);
            panel2.TabIndex = 5;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label1.ForeColor = Color.White;
            label1.Location = new Point(483, 22);
            label1.Name = "label1";
            label1.Size = new Size(328, 54);
            label1.TabIndex = 4;
            label1.Text = "Chi Tiết Kết Quả";
            // 
            // btnXuatFile
            // 
            btnXuatFile.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btnXuatFile.Location = new Point(997, 693);
            btnXuatFile.Name = "btnXuatFile";
            btnXuatFile.Size = new Size(188, 57);
            btnXuatFile.TabIndex = 3;
            btnXuatFile.Text = "Xuất file";
            btnXuatFile.UseVisualStyleBackColor = true;
            // 
            // btnXacNhan
            // 
            btnXacNhan.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btnXacNhan.Location = new Point(609, 693);
            btnXacNhan.Name = "btnXacNhan";
            btnXacNhan.Size = new Size(184, 57);
            btnXacNhan.TabIndex = 2;
            btnXacNhan.Text = "Xác nhận";
            btnXacNhan.UseVisualStyleBackColor = true;
            btnXacNhan.Click += btnXacNhan_Click;
            // 
            // btnHuyXacNhan
            // 
            btnHuyXacNhan.Font = new Font("Segoe UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 163);
            btnHuyXacNhan.Location = new Point(158, 693);
            btnHuyXacNhan.Name = "btnHuyXacNhan";
            btnHuyXacNhan.Size = new Size(166, 57);
            btnHuyXacNhan.TabIndex = 1;
            btnHuyXacNhan.Text = "Hủy xác nhận";
            btnHuyXacNhan.UseVisualStyleBackColor = true;
            btnHuyXacNhan.Click += btnHuyXacNhan_Click;
            // 
            // dgvChiTiet
            // 
            dgvChiTiet.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvChiTiet.Location = new Point(-3, -262);
            dgvChiTiet.Name = "dgvChiTiet";
            dgvChiTiet.RowHeadersWidth = 51;
            dgvChiTiet.Size = new Size(1286, 388);
            dgvChiTiet.TabIndex = 0;
            // 
            // ChiTietKetQua
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1295, 771);
            Controls.Add(panel1);
            MaximizeBox = false;
            Name = "ChiTietKetQua";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Chi tiết kết quả";
            Load += ChiTietKetQua_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvChiTiet).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button btnXuatFile;
        private Button btnXacNhan;
        private Button btnHuyXacNhan;
        private DataGridView dgvChiTiet;
        private Panel panel2;
        private Label label1;
        private Label label3;
        private Label label2;
        private Label label5;
        private TextBox txtNhanVienNhap;
        private TextBox txtNenMau;
        private TextBox txtTrangThai;
        private TextBox txtMaKQ;
        private TextBox txtGhiChu;
        private DateTimePicker dtpNgayDo;
        private Label label4;
        private Label label7;
        private Label label6;
    }
}