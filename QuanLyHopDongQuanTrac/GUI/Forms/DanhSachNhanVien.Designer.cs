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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            panel2 = new Panel();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox1 = new PictureBox();
            picturemicro = new PictureBox();
            panel1 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            panel7 = new Panel();
            dgvDanhsachnhanvien = new DataGridView();
            dataGridView1 = new DataGridView();
            panel6 = new Panel();
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
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picturemicro).BeginInit();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhsachnhanvien).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel6.SuspendLayout();
            containersearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureFilter).BeginInit();
            sidebar.SuspendLayout();
            panel3.SuspendLayout();
            SuspendLayout();
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(0, 152, 70);
            panel2.Controls.Add(pictureBox2);
            panel2.Controls.Add(pictureBox3);
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
            // pictureBox3
            // 
            pictureBox3.Anchor = AnchorStyles.Right;
            pictureBox3.Cursor = Cursors.Hand;
            pictureBox3.Image = Properties.Resources.settingicon_2;
            pictureBox3.Location = new Point(943, 12);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(44, 38);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 2;
            pictureBox3.TabStop = false;
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
            // picturemicro
            // 
            picturemicro.Cursor = Cursors.Hand;
            picturemicro.Image = (Image)resources.GetObject("picturemicro.Image");
            picturemicro.Location = new Point(325, 11);
            picturemicro.Name = "picturemicro";
            picturemicro.Size = new Size(39, 34);
            picturemicro.SizeMode = PictureBoxSizeMode.Zoom;
            picturemicro.TabIndex = 6;
            picturemicro.TabStop = false;
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
            panel5.Location = new Point(193, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(828, 472);
            panel5.TabIndex = 1;
            // 
            // panel7
            // 
            panel7.Controls.Add(dgvDanhsachnhanvien);
            panel7.Controls.Add(dataGridView1);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(0, 61);
            panel7.Name = "panel7";
            panel7.Size = new Size(828, 411);
            panel7.TabIndex = 1;
            // 
            // dgvDanhsachnhanvien
            // 
            dgvDanhsachnhanvien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhsachnhanvien.BackgroundColor = Color.White;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(0, 152, 70);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvDanhsachnhanvien.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvDanhsachnhanvien.ColumnHeadersHeight = 30;
            dgvDanhsachnhanvien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvDanhsachnhanvien.DefaultCellStyle = dataGridViewCellStyle4;
            dgvDanhsachnhanvien.Dock = DockStyle.Fill;
            dgvDanhsachnhanvien.EnableHeadersVisualStyles = false;
            dgvDanhsachnhanvien.Location = new Point(0, 0);
            dgvDanhsachnhanvien.Name = "dgvDanhsachnhanvien";
            dgvDanhsachnhanvien.RowHeadersWidth = 51;
            dgvDanhsachnhanvien.Size = new Size(828, 385);
            dgvDanhsachnhanvien.TabIndex = 1;
            dgvDanhsachnhanvien.CellClick += dgvDanhsachnhanvien_CellClick;
            dgvDanhsachnhanvien.CellContentClick += dgvDanhsachnhanvien_CellContentClick;
            dgvDanhsachnhanvien.CellPainting += dgvDanhsachnhanvien_CellPainting;
            dgvDanhsachnhanvien.Paint += dgvDanhsachnhanvien_Paint;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = SystemColors.MenuHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Bottom;
            dataGridView1.Location = new Point(0, 385);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(828, 26);
            dataGridView1.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.BackColor = Color.White;
            panel6.Controls.Add(containersearch);
            panel6.Controls.Add(picturemicro);
            panel6.Controls.Add(btnXuatfile);
            panel6.Controls.Add(btnThemuser);
            panel6.Controls.Add(pictureFilter);
            panel6.Dock = DockStyle.Top;
            panel6.Font = new Font("Segoe UI", 10F);
            panel6.Location = new Point(0, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(828, 61);
            panel6.TabIndex = 0;
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
            searchtextbox.Location = new Point(74, 22);
            searchtextbox.Name = "searchtextbox";
            searchtextbox.Size = new Size(125, 30);
            searchtextbox.TabIndex = 0;
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
            btnXuatfile.Location = new Point(750, 10);
            btnXuatfile.Name = "btnXuatfile";
            btnXuatfile.Size = new Size(66, 40);
            btnXuatfile.TabIndex = 5;
            btnXuatfile.UseVisualStyleBackColor = false;
            // 
            // btnThemuser
            // 
            btnThemuser.Anchor = AnchorStyles.None;
            btnThemuser.BackColor = Color.FromArgb(255, 107, 53);
            btnThemuser.BackgroundImageLayout = ImageLayout.None;
            btnThemuser.Cursor = Cursors.Hand;
            btnThemuser.FlatAppearance.BorderColor = Color.White;
            btnThemuser.FlatStyle = FlatStyle.Flat;
            btnThemuser.Image = (Image)resources.GetObject("btnThemuser.Image");
            btnThemuser.Location = new Point(568, 10);
            btnThemuser.Name = "btnThemuser";
            btnThemuser.Size = new Size(66, 40);
            btnThemuser.TabIndex = 4;
            btnThemuser.UseVisualStyleBackColor = false;
            // 
            // pictureFilter
            // 
            pictureFilter.Cursor = Cursors.Hand;
            pictureFilter.Image = (Image)resources.GetObject("pictureFilter.Image");
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
            sidebar.Size = new Size(193, 472);
            sidebar.TabIndex = 0;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(10, 113, 78);
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button1.ForeColor = Color.White;
            button1.Image = (Image)resources.GetObject("button1.Image");
            button1.ImageAlign = ContentAlignment.MiddleLeft;
            button1.Location = new Point(0, 61);
            button1.Name = "button1";
            button1.Size = new Size(193, 61);
            button1.TabIndex = 1;
            button1.Text = "Danh sách nhân viên ";
            button1.TextAlign = ContentAlignment.MiddleRight;
            button1.UseVisualStyleBackColor = false;
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
            Resize += DanhSachNhanVien_Resize;
            panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)picturemicro).EndInit();
            panel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDanhsachnhanvien).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel6.ResumeLayout(false);
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
        private PictureBox pictureBox3;
        private PictureBox pictureBox1;
        private Panel panel1;
        private Panel panel3;
        private Panel panel4;
        private Panel sidebar;
        private Button btnDanhsachnv;
        private Panel panel5;
        private Panel panel7;
        private DataGridView dgvDanhsachnhanvien;
        private DataGridView dataGridView1;
        private Panel panel6;
        private PictureBox pictureFilter;
        private Button btnThemuser;
        private Button btnXuatfile;
        private Label labelFooter;
        private PictureBox pictureBox2;
        private PictureBox picturemicro;
        private Button button1;
        private Panel containersearch;
        private TextBox searchtextbox;
    }
}