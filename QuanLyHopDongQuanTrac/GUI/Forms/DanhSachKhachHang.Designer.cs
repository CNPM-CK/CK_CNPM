namespace GUI.Forms
{
    partial class DanhSachKhachHang
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DanhSachKhachHang));
            panel2 = new Panel();
            pictureBox4 = new PictureBox();
            pictureBoxSetting = new PictureBox();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            sidebar = new Panel();
            button1 = new Button();
            btnDanhsachnv = new Button();
            panel3 = new Panel();
            labelFooter = new Label();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSetting).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            sidebar.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(0, 152, 70);
            panel2.Controls.Add(pictureBox4);
            panel2.Controls.Add(pictureBoxSetting);
            panel2.Controls.Add(pictureBox1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1021, 64);
            panel2.TabIndex = 0;
            // 
            // pictureBox4
            // 
            pictureBox4.Anchor = AnchorStyles.Right;
            pictureBox4.Cursor = Cursors.Hand;
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(876, 12);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(41, 38);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 4;
            pictureBox4.TabStop = false;
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
            pictureBoxSetting.Click += pictureBoxSetting_Click;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.remove_background_logo;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(211, 64);
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
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(211, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(810, 472);
            panel5.TabIndex = 1;
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.FromArgb(224, 234, 230);
            sidebar.Controls.Add(button1);
            sidebar.Controls.Add(btnDanhsachnv);
            sidebar.Dock = DockStyle.Left;
            sidebar.Location = new Point(0, 0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(211, 472);
            sidebar.TabIndex = 0;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(10, 113, 78);
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(0, 61);
            button1.Name = "button1";
            button1.Size = new Size(211, 61);
            button1.TabIndex = 1;
            button1.Text = "Danh sách khách hàng";
            button1.TextAlign = ContentAlignment.MiddleRight;
            button1.UseVisualStyleBackColor = false;
            // 
            // btnDanhsachnv
            // 
            btnDanhsachnv.BackColor = Color.FromArgb(10, 113, 78);
            btnDanhsachnv.BackgroundImageLayout = ImageLayout.Zoom;
            btnDanhsachnv.Cursor = Cursors.Hand;
            btnDanhsachnv.FlatStyle = FlatStyle.Flat;
            btnDanhsachnv.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnDanhsachnv.ForeColor = Color.White;
            btnDanhsachnv.Image = (Image)resources.GetObject("btnDanhsachnv.Image");
            btnDanhsachnv.ImageAlign = ContentAlignment.MiddleLeft;
            btnDanhsachnv.Location = new Point(0, 0);
            btnDanhsachnv.Name = "btnDanhsachnv";
            btnDanhsachnv.Size = new Size(211, 61);
            btnDanhsachnv.TabIndex = 0;
            btnDanhsachnv.Text = "Danh sách khách hàng  ";
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
            // DanhSachKhachHang
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1021, 565);
            Controls.Add(panel1);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DanhSachKhachHang";
            Text = "Form1";
            WindowState = FormWindowState.Maximized;
            Load += DanhSachNhanVien_Load;
            Click += DanhSachNhanVien_Click;
            Resize += DanhSachNhanVien_Resize;
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSetting).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
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
        private Button button1;
        private PictureBox pictureBox4;
        private Panel panel5;
    }
}