namespace GUI.Forms
{
    partial class DangKyKhuonMat
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
            lblTrangThai = new Label();
            lblTieuDe = new Label();
            btnLuuKhuonMat = new Button();
            btnChupAnh = new Button();
            btnBatCamera = new Button();
            pictureBoxCamera = new PictureBox();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxCamera).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(lblTrangThai);
            panel1.Controls.Add(lblTieuDe);
            panel1.Controls.Add(btnLuuKhuonMat);
            panel1.Controls.Add(btnChupAnh);
            panel1.Controls.Add(btnBatCamera);
            panel1.Controls.Add(pictureBoxCamera);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(3, 4, 3, 4);
            panel1.Name = "panel1";
            panel1.Size = new Size(700, 850);
            panel1.TabIndex = 0;
            // 
            // lblTrangThai
            // 
            lblTrangThai.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblTrangThai.ForeColor = Color.FromArgb(0, 152, 70);
            lblTrangThai.Location = new Point(30, 788);
            lblTrangThai.Name = "lblTrangThai";
            lblTrangThai.Size = new Size(640, 38);
            lblTrangThai.TabIndex = 5;
            lblTrangThai.Text = "Nhấn \"Bật Camera\" để bắt đầu";
            lblTrangThai.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblTieuDe
            // 
            lblTieuDe.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTieuDe.ForeColor = Color.FromArgb(0, 152, 70);
            lblTieuDe.Location = new Point(30, 25);
            lblTieuDe.Name = "lblTieuDe";
            lblTieuDe.Size = new Size(640, 44);
            lblTieuDe.TabIndex = 4;
            lblTieuDe.Text = "ĐĂNG KÝ KHUÔN MẶT";
            lblTieuDe.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnLuuKhuonMat
            // 
            btnLuuKhuonMat.BackColor = Color.FromArgb(76, 175, 80);
            btnLuuKhuonMat.Cursor = Cursors.Hand;
            btnLuuKhuonMat.Enabled = false;
            btnLuuKhuonMat.FlatAppearance.BorderSize = 0;
            btnLuuKhuonMat.FlatStyle = FlatStyle.Flat;
            btnLuuKhuonMat.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnLuuKhuonMat.ForeColor = Color.White;
            btnLuuKhuonMat.Location = new Point(470, 712);
            btnLuuKhuonMat.Margin = new Padding(3, 4, 3, 4);
            btnLuuKhuonMat.Name = "btnLuuKhuonMat";
            btnLuuKhuonMat.Size = new Size(200, 56);
            btnLuuKhuonMat.TabIndex = 3;
            btnLuuKhuonMat.Text = "Lưu Khuôn Mặt";
            btnLuuKhuonMat.UseVisualStyleBackColor = false;
            btnLuuKhuonMat.Click += btnLuuKhuonMat_Click;
            // 
            // btnChupAnh
            // 
            btnChupAnh.BackColor = Color.FromArgb(33, 150, 243);
            btnChupAnh.Cursor = Cursors.Hand;
            btnChupAnh.Enabled = false;
            btnChupAnh.FlatAppearance.BorderSize = 0;
            btnChupAnh.FlatStyle = FlatStyle.Flat;
            btnChupAnh.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnChupAnh.ForeColor = Color.White;
            btnChupAnh.Location = new Point(250, 712);
            btnChupAnh.Margin = new Padding(3, 4, 3, 4);
            btnChupAnh.Name = "btnChupAnh";
            btnChupAnh.Size = new Size(200, 56);
            btnChupAnh.TabIndex = 2;
            btnChupAnh.Text = "Quét Khuôn Mặt";
            btnChupAnh.UseVisualStyleBackColor = false;
            btnChupAnh.Click += btnChupAnh_Click;
            // 
            // btnBatCamera
            // 
            btnBatCamera.BackColor = Color.FromArgb(0, 152, 70);
            btnBatCamera.Cursor = Cursors.Hand;
            btnBatCamera.FlatAppearance.BorderSize = 0;
            btnBatCamera.FlatStyle = FlatStyle.Flat;
            btnBatCamera.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            btnBatCamera.ForeColor = Color.White;
            btnBatCamera.Location = new Point(30, 712);
            btnBatCamera.Margin = new Padding(3, 4, 3, 4);
            btnBatCamera.Name = "btnBatCamera";
            btnBatCamera.Size = new Size(200, 56);
            btnBatCamera.TabIndex = 1;
            btnBatCamera.Text = "Bật Camera";
            btnBatCamera.UseVisualStyleBackColor = false;
            btnBatCamera.Click += btnBatCamera_Click;
            // 
            // pictureBoxCamera
            // 
            pictureBoxCamera.BackColor = Color.FromArgb(240, 240, 240);
            pictureBoxCamera.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxCamera.Location = new Point(30, 88);
            pictureBoxCamera.Margin = new Padding(3, 4, 3, 4);
            pictureBoxCamera.Name = "pictureBoxCamera";
            pictureBoxCamera.Size = new Size(640, 600);
            pictureBoxCamera.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxCamera.TabIndex = 0;
            pictureBoxCamera.TabStop = false;
            // 
            // DangKyKhuonMat
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(700, 850);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(3, 4, 3, 4);
            MaximizeBox = false;
            Name = "DangKyKhuonMat";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Đăng Ký Khuôn Mặt";
            FormClosing += FormDangKyKhuonMat_FormClosing;
            Load += DangKyKhuonMat_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxCamera).EndInit();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBoxCamera;
        private System.Windows.Forms.Button btnBatCamera;
        private System.Windows.Forms.Button btnChupAnh;
        private System.Windows.Forms.Button btnLuuKhuonMat;
        private System.Windows.Forms.Label lblTieuDe;
        private System.Windows.Forms.Label lblTrangThai;
    }
}