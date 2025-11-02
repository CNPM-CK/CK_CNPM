namespace GUI.Forms
{
    partial class DanhSachNhanVien
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DanhSachNhanVien));
            panel2 = new Panel();
            pictureBox2 = new PictureBox();
            pictureBoxSetting = new PictureBox();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            label1 = new Label();
            sidebar = new Panel();
            btnHopdong = new Button();
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
            panel1.Size = new Size(1021, 565);
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
            panel4.Size = new Size(1021, 472);
            panel4.TabIndex = 2;
            // 
            // panel5
            // 
            panel5.Controls.Add(label1);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(193, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(828, 472);
            panel5.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Black", 22.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(938, 100);
            label1.TabIndex = 0;
            label1.Text = "Chào mừng bạn đến với ECOS \r\nHãy cùng bắt đầu một ngày làm việc hiệu quả nhé!";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.FromArgb(224, 234, 230);
            sidebar.Controls.Add(btnHopdong);
            sidebar.Controls.Add(btnDanhsachnv);
            sidebar.Dock = DockStyle.Left;
            sidebar.Location = new Point(0, 0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(193, 472);
            sidebar.TabIndex = 0;
            // 
            // btnHopdong
            // 
            btnHopdong.BackColor = Color.FromArgb(10, 113, 78);
            btnHopdong.Cursor = Cursors.Hand;
            btnHopdong.FlatStyle = FlatStyle.Flat;
            btnHopdong.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHopdong.ForeColor = Color.White;
            btnHopdong.Image = (Image)resources.GetObject("btnHopdong.Image");
            btnHopdong.ImageAlign = ContentAlignment.MiddleLeft;
            btnHopdong.Location = new Point(0, 61);
            btnHopdong.Name = "btnHopdong";
            btnHopdong.Size = new Size(193, 61);
            btnHopdong.TabIndex = 1;
            btnHopdong.Text = "Danh sách hợp đồng";
            btnHopdong.TextAlign = ContentAlignment.MiddleRight;
            btnHopdong.UseVisualStyleBackColor = false;
            btnHopdong.Click += button1_Click;
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
            btnDanhsachnv.Size = new Size(193, 61);
            btnDanhsachnv.TabIndex = 0;
            btnDanhsachnv.Text = "Danh sách nhân viên ";
            btnDanhsachnv.TextAlign = ContentAlignment.MiddleRight;
            btnDanhsachnv.UseVisualStyleBackColor = false;
            btnDanhsachnv.Click += btnDanhsachnv_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(labelFooter);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 536);
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
            // DanhSachNhanVien
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1021, 565);
            Controls.Add(panel1);
            Name = "DanhSachNhanVien";
            Text = "Form1";
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
        private Button btnHopdong;
        private Panel panel5;
        private Label label1;
    }
}