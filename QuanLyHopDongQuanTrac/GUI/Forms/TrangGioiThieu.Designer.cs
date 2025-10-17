using GUI.Helper;

namespace GUI.Forms
{
    partial class TrangGioiThieu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrangGioiThieu));
            panel1 = new ModernPanel();
            btn = new ModernButton();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            label2 = new Label();
            pictureBox2 = new PictureBox();
            panel2 = new Panel();
            panel3 = new Panel();
            panel7 = new Panel();
            label9 = new Label();
            label10 = new Label();
            pictureBox5 = new PictureBox();
            panel8 = new Panel();
            label11 = new Label();
            label12 = new Label();
            pictureBox6 = new PictureBox();
            panel6 = new Panel();
            label7 = new Label();
            label8 = new Label();
            pictureBox4 = new PictureBox();
            panel5 = new Panel();
            label6 = new Label();
            label5 = new Label();
            pictureBox3 = new PictureBox();
            label4 = new Label();
            label3 = new Label();
            panel4 = new Panel();
            label26 = new Label();
            pictureBox13 = new PictureBox();
            label25 = new Label();
            pictureBox12 = new PictureBox();
            label14 = new Label();
            pictureBox7 = new PictureBox();
            label13 = new Label();
            panel9 = new Panel();
            panel10 = new Panel();
            label17 = new Label();
            label15 = new Label();
            pictureBox8 = new PictureBox();
            label16 = new Label();
            pictureBox9 = new PictureBox();
            label23 = new Label();
            panel11 = new Panel();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            panel9.SuspendLayout();
            panel10.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).BeginInit();
            panel11.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.Transparent;
            panel1.BackColorFill = Color.FromArgb(75, 153, 96);
            panel1.BorderColor = Color.FromArgb(180, 180, 180);
            panel1.BorderRadius = 20;
            panel1.Controls.Add(btn);
            panel1.Controls.Add(pictureBox1);
            panel1.Dock = DockStyle.Top;
            panel1.DrawTextOnPanel = false;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1907, 103);
            panel1.TabIndex = 0;
            // 
            // btn
            // 
            btn.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btn.BackColor = Color.Transparent;
            btn.BackColorHover = Color.FromArgb(174, 153, 68);
            btn.BackColorNormal = Color.FromArgb(248, 248, 238);
            btn.BorderColor = Color.Empty;
            btn.BorderRadius = 20;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            btn.Location = new Point(1687, 16);
            btn.Name = "btn";
            btn.Size = new Size(195, 70);
            btn.TabIndex = 0;
            btn.Text = "Đăng nhập";
            btn.UseVisualStyleBackColor = false;
            btn.Click += btn_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Dock = DockStyle.Left;
            pictureBox1.Image = Properties.Resources.remove_background_logo;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(249, 103);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = SystemColors.Control;
            label1.Font = new Font("Segoe UI Black", 49F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(122, 73, 0);
            label1.Location = new Point(41, 33);
            label1.Name = "label1";
            label1.Size = new Size(254, 109);
            label1.TabIndex = 1;
            label1.Text = "ECOS";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 26F);
            label2.ForeColor = Color.FromArgb(122, 73, 0);
            label2.Location = new Point(59, 142);
            label2.Name = "label2";
            label2.Size = new Size(762, 120);
            label2.TabIndex = 2;
            label2.Text = "Hệ thống quản lý đơn hàng và\r\nhợp đồng trong quan trắc môi trường";
            // 
            // pictureBox2
            // 
            pictureBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(822, 19);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(1000, 746);
            pictureBox2.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // panel2
            // 
            panel2.Controls.Add(label1);
            panel2.Controls.Add(label2);
            panel2.Controls.Add(pictureBox2);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1886, 799);
            panel2.TabIndex = 4;
            panel2.Paint += panel2_Paint;
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(174, 153, 68);
            panel3.Controls.Add(panel7);
            panel3.Controls.Add(panel8);
            panel3.Controls.Add(panel6);
            panel3.Controls.Add(panel5);
            panel3.Controls.Add(label4);
            panel3.Controls.Add(label3);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(0, 799);
            panel3.Name = "panel3";
            panel3.Size = new Size(1886, 799);
            panel3.TabIndex = 5;
            // 
            // panel7
            // 
            panel7.BackColor = Color.FromArgb(27, 66, 53);
            panel7.Controls.Add(label9);
            panel7.Controls.Add(label10);
            panel7.Controls.Add(pictureBox5);
            panel7.Location = new Point(1441, 304);
            panel7.Name = "panel7";
            panel7.Size = new Size(381, 423);
            panel7.TabIndex = 5;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.FromArgb(237, 211, 197);
            label9.Location = new Point(73, 306);
            label9.Name = "label9";
            label9.Size = new Size(235, 62);
            label9.TabIndex = 2;
            label9.Text = "Giám sát và cập nhật\r\ndữ liệu thời gian thực\r\n";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label10.ForeColor = Color.FromArgb(237, 211, 197);
            label10.Location = new Point(37, 251);
            label10.Name = "label10";
            label10.Size = new Size(317, 37);
            label10.TabIndex = 1;
            label10.Text = "Theo dõi thời gian thực";
            // 
            // pictureBox5
            // 
            pictureBox5.Dock = DockStyle.Top;
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(0, 0);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(381, 232);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 0;
            pictureBox5.TabStop = false;
            // 
            // panel8
            // 
            panel8.BackColor = Color.FromArgb(27, 66, 53);
            panel8.Controls.Add(label11);
            panel8.Controls.Add(label12);
            panel8.Controls.Add(pictureBox6);
            panel8.Location = new Point(982, 304);
            panel8.Name = "panel8";
            panel8.Size = new Size(381, 423);
            panel8.TabIndex = 4;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label11.ForeColor = Color.FromArgb(237, 211, 197);
            label11.Location = new Point(64, 306);
            label11.Name = "label11";
            label11.Size = new Size(270, 62);
            label11.TabIndex = 2;
            label11.Text = "   Điều phối hiệu quả\r\ncác chuyên gia quan trắc\r\n";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label12.ForeColor = Color.FromArgb(237, 211, 197);
            label12.Location = new Point(78, 251);
            label12.Name = "label12";
            label12.Size = new Size(219, 37);
            label12.TabIndex = 1;
            label12.Text = "Quản lý đội ngũ";
            // 
            // pictureBox6
            // 
            pictureBox6.Dock = DockStyle.Top;
            pictureBox6.Image = (Image)resources.GetObject("pictureBox6.Image");
            pictureBox6.Location = new Point(0, 0);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(381, 232);
            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox6.TabIndex = 0;
            pictureBox6.TabStop = false;
            // 
            // panel6
            // 
            panel6.BackColor = Color.FromArgb(27, 66, 53);
            panel6.Controls.Add(label7);
            panel6.Controls.Add(label8);
            panel6.Controls.Add(pictureBox4);
            panel6.Location = new Point(524, 304);
            panel6.Name = "panel6";
            panel6.Size = new Size(381, 423);
            panel6.TabIndex = 3;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.ForeColor = Color.FromArgb(237, 211, 197);
            label7.Location = new Point(83, 306);
            label7.Name = "label7";
            label7.Size = new Size(226, 62);
            label7.TabIndex = 2;
            label7.Text = "Tạo báo cáo chi tiết\r\nvới dữ liệu chính xác\r\n";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label8.ForeColor = Color.FromArgb(237, 211, 197);
            label8.Location = new Point(83, 251);
            label8.Name = "label8";
            label8.Size = new Size(214, 37);
            label8.TabIndex = 1;
            label8.Text = "Báo cáo chi tiết";
            // 
            // pictureBox4
            // 
            pictureBox4.Dock = DockStyle.Top;
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(0, 0);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(381, 232);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 0;
            pictureBox4.TabStop = false;
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(27, 66, 53);
            panel5.Controls.Add(label6);
            panel5.Controls.Add(label5);
            panel5.Controls.Add(pictureBox3);
            panel5.Location = new Point(65, 304);
            panel5.Name = "panel5";
            panel5.Size = new Size(381, 423);
            panel5.TabIndex = 2;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI Semibold", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.FromArgb(237, 211, 197);
            label6.Location = new Point(45, 306);
            label6.Name = "label6";
            label6.Size = new Size(282, 93);
            label6.TabIndex = 2;
            label6.Text = "      Đảm bảo tuân thủ\r\ncác quy định nghiêm ngặt\r\n         từ môi trường\r\n";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            label5.ForeColor = Color.FromArgb(237, 211, 197);
            label5.Location = new Point(45, 251);
            label5.Name = "label5";
            label5.Size = new Size(295, 37);
            label5.TabIndex = 1;
            label5.Text = "Tuân thủ các quy định";
            // 
            // pictureBox3
            // 
            pictureBox3.Dock = DockStyle.Top;
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(0, 0);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(381, 232);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 0;
            pictureBox3.TabStop = false;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 18.2F, FontStyle.Bold);
            label4.ForeColor = Color.FromArgb(57, 27, 11);
            label4.Location = new Point(1043, 72);
            label4.Name = "label4";
            label4.Size = new Size(831, 126);
            label4.TabIndex = 1;
            label4.Text = "Hệ thống cung cấp các dịch vụ cho việc quản lý, \r\ngiám sát và báo cáo các hoạt động quan trắc môi trường \r\nmột cách hiệu quả và chính xác";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 35.8F, FontStyle.Bold);
            label3.ForeColor = Color.FromArgb(57, 27, 11);
            label3.Location = new Point(41, 53);
            label3.Name = "label3";
            label3.Size = new Size(815, 81);
            label3.TabIndex = 0;
            label3.Text = "TỔNG QUAN VỀ HỆ THỐNG";
            // 
            // panel4
            // 
            panel4.Controls.Add(label26);
            panel4.Controls.Add(pictureBox13);
            panel4.Controls.Add(label25);
            panel4.Controls.Add(pictureBox12);
            panel4.Controls.Add(label14);
            panel4.Controls.Add(pictureBox7);
            panel4.Controls.Add(label13);
            panel4.Dock = DockStyle.Top;
            panel4.Location = new Point(0, 1598);
            panel4.Name = "panel4";
            panel4.Size = new Size(1886, 799);
            panel4.TabIndex = 5;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label26.Location = new Point(1369, 200);
            label26.Name = "label26";
            label26.Size = new Size(345, 46);
            label26.TabIndex = 7;
            label26.Text = "Quan trắc không khí";
            // 
            // pictureBox13
            // 
            pictureBox13.Image = (Image)resources.GetObject("pictureBox13.Image");
            pictureBox13.Location = new Point(1389, 265);
            pictureBox13.Name = "pictureBox13";
            pictureBox13.Size = new Size(300, 452);
            pictureBox13.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox13.TabIndex = 6;
            pictureBox13.TabStop = false;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label25.Location = new Point(822, 197);
            label25.Name = "label25";
            label25.Size = new Size(238, 46);
            label25.TabIndex = 5;
            label25.Text = "Quan trắc đất";
            // 
            // pictureBox12
            // 
            pictureBox12.Image = (Image)resources.GetObject("pictureBox12.Image");
            pictureBox12.Location = new Point(787, 265);
            pictureBox12.Name = "pictureBox12";
            pictureBox12.Size = new Size(300, 453);
            pictureBox12.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox12.TabIndex = 4;
            pictureBox12.TabStop = false;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label14.Location = new Point(183, 200);
            label14.Name = "label14";
            label14.Size = new Size(266, 46);
            label14.TabIndex = 3;
            label14.Text = "Quan trắc nước";
            // 
            // pictureBox7
            // 
            pictureBox7.Image = (Image)resources.GetObject("pictureBox7.Image");
            pictureBox7.Location = new Point(164, 265);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(300, 453);
            pictureBox7.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox7.TabIndex = 2;
            pictureBox7.TabStop = false;
            // 
            // label13
            // 
            label13.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 35.8F, FontStyle.Bold);
            label13.ForeColor = Color.FromArgb(57, 27, 11);
            label13.Location = new Point(642, 56);
            label13.Name = "label13";
            label13.Size = new Size(674, 81);
            label13.TabIndex = 1;
            label13.Text = "CÁC DỊCH VỤ NỔI BẬT";
            // 
            // panel9
            // 
            panel9.BackColor = Color.FromArgb(174, 153, 68);
            panel9.Controls.Add(panel10);
            panel9.Controls.Add(label15);
            panel9.Controls.Add(pictureBox8);
            panel9.Controls.Add(label16);
            panel9.Controls.Add(pictureBox9);
            panel9.Controls.Add(label23);
            panel9.Dock = DockStyle.Top;
            panel9.Location = new Point(0, 2397);
            panel9.Name = "panel9";
            panel9.Size = new Size(1886, 799);
            panel9.TabIndex = 6;
            // 
            // panel10
            // 
            panel10.BackColor = Color.White;
            panel10.Controls.Add(label17);
            panel10.Dock = DockStyle.Bottom;
            panel10.Location = new Point(0, 764);
            panel10.Name = "panel10";
            panel10.Size = new Size(1886, 35);
            panel10.TabIndex = 12;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(783, 6);
            label17.Name = "label17";
            label17.Size = new Size(277, 20);
            label17.TabIndex = 0;
            label17.Text = "© 2025 ECOS. Bản quyền thuộc về ECOS.";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label15.Location = new Point(1072, 82);
            label15.Name = "label15";
            label15.Size = new Size(322, 46);
            label15.TabIndex = 11;
            label15.Text = "Quan trắc chất thải";
            // 
            // pictureBox8
            // 
            pictureBox8.Image = (Image)resources.GetObject("pictureBox8.Image");
            pictureBox8.Location = new Point(1085, 150);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(301, 453);
            pictureBox8.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox8.TabIndex = 10;
            pictureBox8.TabStop = false;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label16.Location = new Point(473, 82);
            label16.Name = "label16";
            label16.Size = new Size(319, 46);
            label16.TabIndex = 9;
            label16.Text = "Quan trắc tiếng ồn";
            // 
            // pictureBox9
            // 
            pictureBox9.Image = (Image)resources.GetObject("pictureBox9.Image");
            pictureBox9.Location = new Point(483, 150);
            pictureBox9.Name = "pictureBox9";
            pictureBox9.Size = new Size(300, 453);
            pictureBox9.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox9.TabIndex = 8;
            pictureBox9.TabStop = false;
            // 
            // label23
            // 
            label23.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label23.AutoSize = true;
            label23.Font = new Font("Segoe UI Semibold", 18.2F, FontStyle.Bold);
            label23.ForeColor = Color.FromArgb(57, 27, 11);
            label23.Location = new Point(3454, 67);
            label23.Name = "label23";
            label23.Size = new Size(831, 126);
            label23.TabIndex = 1;
            label23.Text = "Hệ thống cung cấp các dịch vụ cho việc quản lý, \r\ngiám sát và báo cáo các hoạt động quan trắc môi trường \r\nmột cách hiệu quả và chính xác";
            // 
            // panel11
            // 
            panel11.AutoScroll = true;
            panel11.Controls.Add(panel9);
            panel11.Controls.Add(panel4);
            panel11.Controls.Add(panel3);
            panel11.Controls.Add(panel2);
            panel11.Dock = DockStyle.Fill;
            panel11.Location = new Point(0, 103);
            panel11.Name = "panel11";
            panel11.Size = new Size(1907, 952);
            panel11.TabIndex = 4;
            // 
            // TrangGioiThieu
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1907, 1055);
            Controls.Add(panel11);
            Controls.Add(panel1);
            Name = "TrangGioiThieu";
            Text = "Trang Giới Thiệu";
            Load += TrangGioiThieu_Load;
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox13).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox12).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox9).EndInit();
            panel11.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private PictureBox pictureBox1;
        private ModernButton btn;
        private ModernPanel panel1;
        private Label label1;
        private Label label2;
        private PictureBox pictureBox2;
        private Panel panel2;
        private Panel panel3;
        private Label label4;
        private Label label3;
        private Panel panel4;
        private Panel panel5;
        private Label label6;
        private Label label5;
        private PictureBox pictureBox3;
        private Panel panel6;
        private Label label7;
        private Label label8;
        private PictureBox pictureBox4;
        private Panel panel7;
        private Label label9;
        private Label label10;
        private PictureBox pictureBox5;
        private Panel panel8;
        private Label label11;
        private Label label12;
        private PictureBox pictureBox6;
        private Label label13;
        private PictureBox pictureBox7;
        private Label label14;
        private Panel panel9;
        private Label label23;
        private Label label26;
        private PictureBox pictureBox13;
        private Label label25;
        private PictureBox pictureBox12;
        private Label label15;
        private PictureBox pictureBox8;
        private Label label16;
        private PictureBox pictureBox9;
        private Panel panel10;
        private Label label17;
        private Panel panel11;
    }
}