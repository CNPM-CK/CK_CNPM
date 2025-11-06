namespace GUI.Forms
{
    partial class DanhSachHD
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DanhSachHD));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panel2 = new Panel();
            pictureBox4 = new PictureBox();
            pictureBoxSetting = new PictureBox();
            pictureBox1 = new PictureBox();
            panel1 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            panel7 = new Panel();
            dgvdanhsachHopDong = new DataGridView();
            dataGridView1 = new DataGridView();
            panel6 = new Panel();
            picturemicro = new PictureBox();
            containersearch = new Panel();
            searchtextbox = new TextBox();
            btnXuatfile = new Button();
            btnThemuser = new Button();
            pictureFilter = new PictureBox();
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
            panel5.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvdanhsachHopDong).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picturemicro).BeginInit();
            containersearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureFilter).BeginInit();
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
            panel5.Controls.Add(panel7);
            panel5.Controls.Add(panel6);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(211, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(810, 472);
            panel5.TabIndex = 1;
            // 
            // panel7
            // 
            panel7.Controls.Add(dgvdanhsachHopDong);
            panel7.Controls.Add(dataGridView1);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(0, 61);
            panel7.Name = "panel7";
            panel7.Size = new Size(810, 411);
            panel7.TabIndex = 1;
            // 
            // dgvdanhsachHopDong
            // 
            dgvdanhsachHopDong.AllowUserToAddRows = false;
            dgvdanhsachHopDong.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvdanhsachHopDong.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(0, 152, 70);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvdanhsachHopDong.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvdanhsachHopDong.ColumnHeadersHeight = 30;
            dgvdanhsachHopDong.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvdanhsachHopDong.DefaultCellStyle = dataGridViewCellStyle2;
            dgvdanhsachHopDong.Dock = DockStyle.Fill;
            dgvdanhsachHopDong.EnableHeadersVisualStyles = false;
            dgvdanhsachHopDong.Location = new Point(0, 0);
            dgvdanhsachHopDong.Name = "dgvdanhsachHopDong";
            dgvdanhsachHopDong.RowHeadersWidth = 51;
            dgvdanhsachHopDong.Size = new Size(810, 385);
            dgvdanhsachHopDong.TabIndex = 1;
            dgvdanhsachHopDong.CellClick += dgvdanhsachHopDong_CellClick;
            dgvdanhsachHopDong.CellContentClick += dgvdanhsachHopDong_CellContentClick;
            dgvdanhsachHopDong.CellFormatting += dgvdanhsachHopDong_CellFormatting;
            dgvdanhsachHopDong.CellPainting += dgvdanhsachHopDong_CellPainting;
            dgvdanhsachHopDong.Paint += dgvdanhsachHopDong_Paint;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = SystemColors.MenuHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Bottom;
            dataGridView1.Location = new Point(0, 385);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(810, 26);
            dataGridView1.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.BackColor = Color.White;
            panel6.Controls.Add(picturemicro);
            panel6.Controls.Add(containersearch);
            panel6.Controls.Add(btnXuatfile);
            panel6.Controls.Add(btnThemuser);
            panel6.Controls.Add(pictureFilter);
            panel6.Dock = DockStyle.Top;
            panel6.Font = new Font("Segoe UI", 10F);
            panel6.Location = new Point(0, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(810, 61);
            panel6.TabIndex = 0;
            // 
            // picturemicro
            // 
            picturemicro.Cursor = Cursors.Hand;
            picturemicro.Image = (Image)resources.GetObject("picturemicro.Image");
            picturemicro.Location = new Point(308, 12);
            picturemicro.Name = "picturemicro";
            picturemicro.Size = new Size(39, 34);
            picturemicro.SizeMode = PictureBoxSizeMode.Zoom;
            picturemicro.TabIndex = 8;
            picturemicro.TabStop = false;
            // 
            // containersearch
            // 
            containersearch.Controls.Add(searchtextbox);
            containersearch.Location = new Point(42, 3);
            containersearch.Name = "containersearch";
            containersearch.Size = new Size(250, 58);
            containersearch.TabIndex = 7;
            containersearch.Paint += containersearch_Paint_1;
            // 
            // searchtextbox
            // 
            searchtextbox.Location = new Point(45, 13);
            searchtextbox.Name = "searchtextbox";
            searchtextbox.PlaceholderText = "Tìm kiếm khách hàng";
            searchtextbox.Size = new Size(177, 30);
            searchtextbox.TabIndex = 0;
            searchtextbox.TextChanged += searchtextbox_TextChanged_1;
            // 
            // btnXuatfile
            // 
            btnXuatfile.Anchor = AnchorStyles.Right;
            btnXuatfile.BackColor = Color.FromArgb(255, 107, 53);
            btnXuatfile.BackgroundImageLayout = ImageLayout.None;
            btnXuatfile.Cursor = Cursors.Hand;
            btnXuatfile.FlatAppearance.BorderColor = Color.White;
            btnXuatfile.FlatStyle = FlatStyle.Flat;
            btnXuatfile.Image = (Image)resources.GetObject("btnXuatfile.Image");
            btnXuatfile.Location = new Point(732, 10);
            btnXuatfile.Name = "btnXuatfile";
            btnXuatfile.Size = new Size(66, 40);
            btnXuatfile.TabIndex = 5;
            btnXuatfile.UseVisualStyleBackColor = false;
            // 
            // btnThemuser
            // 
            btnThemuser.Anchor = AnchorStyles.None;
            btnThemuser.BackColor = Color.FromArgb(255, 107, 53);
            btnThemuser.BackgroundImageLayout = ImageLayout.Zoom;
            btnThemuser.Cursor = Cursors.Hand;
            btnThemuser.FlatAppearance.BorderColor = Color.White;
            btnThemuser.FlatStyle = FlatStyle.Flat;
            btnThemuser.Image = (Image)resources.GetObject("btnThemuser.Image");
            btnThemuser.Location = new Point(559, 10);
            btnThemuser.Name = "btnThemuser";
            btnThemuser.Size = new Size(66, 40);
            btnThemuser.TabIndex = 4;
            btnThemuser.UseVisualStyleBackColor = false;
            btnThemuser.Click += btnThemuser_Click;
            // 
            // pictureFilter
            // 
            pictureFilter.Cursor = Cursors.Hand;
            pictureFilter.Image = Properties.Resources.filter__1_;
            pictureFilter.Location = new Point(3, 12);
            pictureFilter.Name = "pictureFilter";
            pictureFilter.Size = new Size(33, 33);
            pictureFilter.SizeMode = PictureBoxSizeMode.Zoom;
            pictureFilter.TabIndex = 0;
            pictureFilter.TabStop = false;
            pictureFilter.Click += pictureBox4_Click;
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
            button1.Padding = new Padding(0, 0, 3, 0);
            button1.Size = new Size(211, 61);
            button1.TabIndex = 1;
            button1.Text = "Danh sách hợp đồng";
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
            btnDanhsachnv.Padding = new Padding(0, 0, 3, 0);
            btnDanhsachnv.Size = new Size(211, 61);
            btnDanhsachnv.TabIndex = 0;
            btnDanhsachnv.Text = "Danh sách hợp đồng";
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
            // DanhSachHD
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1021, 565);
            Controls.Add(panel1);
            Name = "DanhSachHD";
            Text = "Quản lý hợp đồng";
            WindowState = FormWindowState.Maximized;
            Load += DanhSachHopDong_Load;
            Click += DanhSachNhanVien_Click;
            Resize += DanhSachNhanVien_Resize;
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxSetting).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvdanhsachHopDong).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picturemicro).EndInit();
            containersearch.ResumeLayout(false);
            containersearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureFilter).EndInit();
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
        private Panel panel5;
        private Panel panel7;
        private DataGridView dgvdanhsachHopDong;
        private DataGridView dataGridView1;
        private Panel panel6;
        private PictureBox pictureFilter;
        private Button btnThemuser;
        private Button btnXuatfile;
        private Label labelFooter;
        private Button button1;
        private Panel containersearch;
        private TextBox searchtextbox;
        private PictureBox picturemicro;
        private PictureBox pictureBox4;
    }
}