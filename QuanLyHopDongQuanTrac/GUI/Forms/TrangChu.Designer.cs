namespace GUI.Forms
{
    partial class TrangChu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrangChu));
            panel2 = new Panel();
            pictureBox2 = new PictureBox();
            pictureBoxSetting = new PictureBox();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            label1 = new Label();
            sidebar = new Panel();
            btnDanhSachKetQua = new Button();
            btnDanhSachNhapLieu = new Button();
            btnDanhSachThongSo = new Button();
            btnDanhSachNenMau = new Button();
            btnDanhSachDotQT = new Button();
            btnDanhSachHopDong = new Button();
            btnDanhSachKhachHang = new Button();
            btnDanhsachnv = new Button();
            panel3 = new Panel();
            labelFooter = new Label();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSetting).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            sidebar.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(0, 152, 70);
            panel2.Controls.Add(pictureBox2);
            panel2.Controls.Add(pictureBoxSetting);
            panel2.Controls.Add(pictureBox1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1021, 64);
            panel2.TabIndex = 0;
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Right;
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(880, 12);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(41, 38);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // pictureBoxSetting
            // 
            pictureBoxSetting.Anchor = AnchorStyles.Right;
            pictureBoxSetting.Cursor = Cursors.Hand;
            pictureBoxSetting.Image = Properties.Resources.settingicon_2;
            pictureBoxSetting.Location = new Point(943, 12);
            pictureBoxSetting.Name = "pictureBoxSetting";
            pictureBoxSetting.Size = new Size(44, 38);
            pictureBoxSetting.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBoxSetting.TabIndex = 2;
            pictureBoxSetting.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.remove_background_logo;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(193, 64);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // panel1
            // 
            panel1.Controls.Add(panel4);
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1021, 678);
            panel1.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.BackColor = SystemColors.Control;
            panel4.Controls.Add(panel5);
            panel4.Controls.Add(sidebar);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 64);
            panel4.Name = "panel4";
            panel4.Size = new Size(1021, 585);
            panel4.TabIndex = 2;
            // 
            // panel5
            // 
            panel5.Controls.Add(label1);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(220, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(801, 585);
            panel5.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(16, 0);
            label1.Name = "label1";
            label1.Size = new Size(938, 100);
            label1.TabIndex = 0;
            label1.Text = "Chào mừng bạn đến với ECOS \r\nHãy cùng bắt đầu một ngày làm việc hiệu quả nhé!";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.FromArgb(224, 234, 230);
            sidebar.Controls.Add(btnDanhSachKetQua);
            sidebar.Controls.Add(btnDanhSachNhapLieu);
            sidebar.Controls.Add(btnDanhSachThongSo);
            sidebar.Controls.Add(btnDanhSachNenMau);
            sidebar.Controls.Add(btnDanhSachDotQT);
            sidebar.Controls.Add(btnDanhSachHopDong);
            sidebar.Controls.Add(btnDanhSachKhachHang);
            sidebar.Controls.Add(btnDanhsachnv);
            sidebar.Dock = DockStyle.Left;
            sidebar.Location = new Point(0, 0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(220, 585);
            sidebar.TabIndex = 0;
            // 
            // btnDanhSachKetQua
            // 
            btnDanhSachKetQua.BackColor = Color.FromArgb(10, 113, 78);
            btnDanhSachKetQua.Cursor = Cursors.Hand;
            btnDanhSachKetQua.FlatStyle = FlatStyle.Flat;
            btnDanhSachKetQua.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDanhSachKetQua.ForeColor = Color.White;
            btnDanhSachKetQua.ImageAlign = ContentAlignment.MiddleLeft;
            btnDanhSachKetQua.Location = new Point(0, 420);
            btnDanhSachKetQua.Name = "btnDanhSachKetQua";
            btnDanhSachKetQua.Size = new Size(220, 61);
            btnDanhSachKetQua.TabIndex = 7;
            btnDanhSachKetQua.Text = "Danh sách kết quả";
            btnDanhSachKetQua.TextAlign = ContentAlignment.MiddleLeft;
            btnDanhSachKetQua.UseVisualStyleBackColor = false;
            btnDanhSachKetQua.Click += btnDanhSachKetQua_Click;
            // 
            // btnDanhSachNhapLieu
            // 
            btnDanhSachNhapLieu.BackColor = Color.FromArgb(10, 113, 78);
            btnDanhSachNhapLieu.Cursor = Cursors.Hand;
            btnDanhSachNhapLieu.FlatStyle = FlatStyle.Flat;
            btnDanhSachNhapLieu.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDanhSachNhapLieu.ForeColor = Color.White;
            btnDanhSachNhapLieu.ImageAlign = ContentAlignment.MiddleLeft;
            btnDanhSachNhapLieu.Location = new Point(0, 360);
            btnDanhSachNhapLieu.Name = "btnDanhSachNhapLieu";
            btnDanhSachNhapLieu.Size = new Size(220, 61);
            btnDanhSachNhapLieu.TabIndex = 6;
            btnDanhSachNhapLieu.Text = "Danh sách nhập liệu";
            btnDanhSachNhapLieu.TextAlign = ContentAlignment.MiddleLeft;
            btnDanhSachNhapLieu.UseVisualStyleBackColor = false;
            btnDanhSachNhapLieu.Click += btnDanhSachNhapLieu_Click;
            // 
            // btnDanhSachThongSo
            // 
            btnDanhSachThongSo.BackColor = Color.FromArgb(10, 113, 78);
            btnDanhSachThongSo.Cursor = Cursors.Hand;
            btnDanhSachThongSo.FlatStyle = FlatStyle.Flat;
            btnDanhSachThongSo.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDanhSachThongSo.ForeColor = Color.White;
            btnDanhSachThongSo.Image = (Image)resources.GetObject("btnDanhSachThongSo.Image");
            btnDanhSachThongSo.ImageAlign = ContentAlignment.MiddleLeft;
            btnDanhSachThongSo.Location = new Point(0, 300);
            btnDanhSachThongSo.Name = "btnDanhSachThongSo";
            btnDanhSachThongSo.Size = new Size(220, 61);
            btnDanhSachThongSo.TabIndex = 5;
            btnDanhSachThongSo.Text = "Danh sách thông số";
            btnDanhSachThongSo.TextAlign = ContentAlignment.MiddleLeft;
            btnDanhSachThongSo.UseVisualStyleBackColor = false;
            btnDanhSachThongSo.Click += btnDanhSachThongSo_Click;
            // 
            // btnDanhSachNenMau
            // 
            btnDanhSachNenMau.BackColor = Color.FromArgb(10, 113, 78);
            btnDanhSachNenMau.Cursor = Cursors.Hand;
            btnDanhSachNenMau.FlatStyle = FlatStyle.Flat;
            btnDanhSachNenMau.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDanhSachNenMau.ForeColor = Color.White;
            btnDanhSachNenMau.Image = (Image)resources.GetObject("btnDanhSachNenMau.Image");
            btnDanhSachNenMau.ImageAlign = ContentAlignment.MiddleLeft;
            btnDanhSachNenMau.Location = new Point(0, 240);
            btnDanhSachNenMau.Name = "btnDanhSachNenMau";
            btnDanhSachNenMau.Size = new Size(220, 61);
            btnDanhSachNenMau.TabIndex = 4;
            btnDanhSachNenMau.Text = "Danh sách nền mẫu";
            btnDanhSachNenMau.TextAlign = ContentAlignment.MiddleLeft;
            btnDanhSachNenMau.UseVisualStyleBackColor = false;
            btnDanhSachNenMau.Click += btnDanhSachNenMau_Click;
            // 
            // btnDanhSachDotQT
            // 
            btnDanhSachDotQT.BackColor = Color.FromArgb(10, 113, 78);
            btnDanhSachDotQT.Cursor = Cursors.Hand;
            btnDanhSachDotQT.FlatStyle = FlatStyle.Flat;
            btnDanhSachDotQT.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDanhSachDotQT.ForeColor = Color.White;
            btnDanhSachDotQT.Image = (Image)resources.GetObject("btnDanhSachDotQT.Image");
            btnDanhSachDotQT.ImageAlign = ContentAlignment.MiddleLeft;
            btnDanhSachDotQT.Location = new Point(0, 180);
            btnDanhSachDotQT.Name = "btnDanhSachDotQT";
            btnDanhSachDotQT.Size = new Size(220, 61);
            btnDanhSachDotQT.TabIndex = 3;
            btnDanhSachDotQT.Text = "Danh sách đợt quan trắc";
            btnDanhSachDotQT.TextAlign = ContentAlignment.MiddleLeft;
            btnDanhSachDotQT.UseVisualStyleBackColor = false;
            btnDanhSachDotQT.Click += btnDanhSachDotQT_Click;
            // 
            // btnDanhSachHopDong
            // 
            btnDanhSachHopDong.BackColor = Color.FromArgb(10, 113, 78);
            btnDanhSachHopDong.Cursor = Cursors.Hand;
            btnDanhSachHopDong.FlatStyle = FlatStyle.Flat;
            btnDanhSachHopDong.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDanhSachHopDong.ForeColor = Color.White;
            btnDanhSachHopDong.Image = (Image)resources.GetObject("btnDanhSachHopDong.Image");
            btnDanhSachHopDong.ImageAlign = ContentAlignment.MiddleLeft;
            btnDanhSachHopDong.Location = new Point(0, 120);
            btnDanhSachHopDong.Name = "btnDanhSachHopDong";
            btnDanhSachHopDong.Size = new Size(220, 61);
            btnDanhSachHopDong.TabIndex = 2;
            btnDanhSachHopDong.Text = "Danh sách hợp đồng";
            btnDanhSachHopDong.TextAlign = ContentAlignment.MiddleLeft;
            btnDanhSachHopDong.UseVisualStyleBackColor = false;
            btnDanhSachHopDong.Click += btnDanhSachHopDong_Click;
            // 
            // btnDanhSachKhachHang
            // 
            btnDanhSachKhachHang.BackColor = Color.FromArgb(10, 113, 78);
            btnDanhSachKhachHang.Cursor = Cursors.Hand;
            btnDanhSachKhachHang.FlatStyle = FlatStyle.Flat;
            btnDanhSachKhachHang.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDanhSachKhachHang.ForeColor = Color.White;
            btnDanhSachKhachHang.Image = (Image)resources.GetObject("btnDanhSachKhachHang.Image");
            btnDanhSachKhachHang.ImageAlign = ContentAlignment.MiddleLeft;
            btnDanhSachKhachHang.Location = new Point(0, 60);
            btnDanhSachKhachHang.Name = "btnDanhSachKhachHang";
            btnDanhSachKhachHang.Size = new Size(220, 61);
            btnDanhSachKhachHang.TabIndex = 1;
            btnDanhSachKhachHang.Text = "Danh sách khách hàng";
            btnDanhSachKhachHang.TextAlign = ContentAlignment.MiddleLeft;
            btnDanhSachKhachHang.UseVisualStyleBackColor = false;
            btnDanhSachKhachHang.Click += btnDanhSachKhachHang_Click;
            // 
            // btnDanhsachnv
            // 
            btnDanhsachnv.BackColor = Color.FromArgb(10, 113, 78);
            btnDanhsachnv.Cursor = Cursors.Hand;
            btnDanhsachnv.FlatStyle = FlatStyle.Flat;
            btnDanhsachnv.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDanhsachnv.ForeColor = Color.White;
            btnDanhsachnv.Image = (Image)resources.GetObject("btnDanhsachnv.Image");
            btnDanhsachnv.ImageAlign = ContentAlignment.MiddleLeft;
            btnDanhsachnv.Location = new Point(0, 0);
            btnDanhsachnv.Name = "btnDanhsachnv";
            btnDanhsachnv.Size = new Size(220, 61);
            btnDanhsachnv.TabIndex = 0;
            btnDanhsachnv.Text = "Danh sách nhân viên";
            btnDanhsachnv.TextAlign = ContentAlignment.MiddleLeft;
            btnDanhsachnv.UseVisualStyleBackColor = false;
            btnDanhsachnv.Click += btnDanhsachnv_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(labelFooter);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 649);
            panel3.Name = "panel3";
            panel3.Size = new Size(1021, 29);
            panel3.TabIndex = 1;
            // 
            // labelFooter
            // 
            labelFooter.Anchor = AnchorStyles.None;
            labelFooter.AutoSize = true;
            labelFooter.Location = new Point(380, 3);
            labelFooter.Name = "labelFooter";
            labelFooter.Size = new Size(277, 20);
            labelFooter.TabIndex = 0;
            labelFooter.Text = "© 2025 ECOS. Bản quyền thuộc về ECOS.";
            labelFooter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // TrangChu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1021, 678);
            Controls.Add(panel1);
            Name = "TrangChu";
            Text = "Trang chủ";
            WindowState = FormWindowState.Maximized;
            Load += DanhSachNhanVien_Load;
            Click += DanhSachNhanVien_Click;
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSetting).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            sidebar.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel2;
        private PictureBox pictureBoxSetting;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Panel panel3;
        private Panel panel4;
        private Panel sidebar;
        private Button btnDanhsachnv;
        private Label labelFooter;
        private PictureBox pictureBox2;
        private Panel panel5;
        private Label label1;
        private Button btnDanhSachKhachHang;
        private Button btnDanhSachHopDong;
        private Button btnDanhSachDotQT;
        private Button btnDanhSachThongSo;
        private Button btnDanhSachNenMau;
        private Button btnDanhSachNhapLieu;
        private Button btnDanhSachKetQua;
    }
}