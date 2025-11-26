namespace GUI.Forms
{
    partial class DSNV_Uc
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        // Dispose resources
        //        voiceSearch?.Dispose();
        //        initTimer?.Dispose();
        //        _voiceLock?.Dispose();

        //        components?.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DSNV_Uc));
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            panel6 = new Panel();
            btnXuatfile = new Button();
            btnThemuser = new Button();
            containersearch = new Panel();
            searchtextbox = new TextBox();
            picturemicro = new PictureBox();
            pictureFilter = new PictureBox();
            panel7 = new Panel();
            dgvDanhsachnhanvien = new DataGridView();
            panel1 = new Panel();
            btnSau = new Button();
            soTrang = new Label();
            btnTruoc = new Button();
            panel6.SuspendLayout();
            containersearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picturemicro).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureFilter).BeginInit();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhsachnhanvien).BeginInit();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // panel6
            // 
            panel6.BackColor = Color.White;
            panel6.Controls.Add(btnXuatfile);
            panel6.Controls.Add(btnThemuser);
            panel6.Controls.Add(containersearch);
            panel6.Controls.Add(picturemicro);
            panel6.Controls.Add(pictureFilter);
            panel6.Dock = DockStyle.Top;
            panel6.Font = new Font("Segoe UI", 10F);
            panel6.Location = new Point(0, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(828, 70);
            panel6.TabIndex = 4;
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
            btnXuatfile.Location = new Point(727, 15);
            btnXuatfile.Name = "btnXuatfile";
            btnXuatfile.Size = new Size(90, 40);
            btnXuatfile.TabIndex = 8;
            btnXuatfile.UseVisualStyleBackColor = false;
            // 
            // btnThemuser
            // 
            btnThemuser.Anchor = AnchorStyles.Right;
            btnThemuser.BackColor = Color.FromArgb(255, 107, 53);
            btnThemuser.BackgroundImageLayout = ImageLayout.None;
            btnThemuser.Cursor = Cursors.Hand;
            btnThemuser.FlatAppearance.BorderColor = Color.White;
            btnThemuser.FlatStyle = FlatStyle.Flat;
            btnThemuser.Image = (Image)resources.GetObject("btnThemuser.Image");
            btnThemuser.Location = new Point(631, 15);
            btnThemuser.Name = "btnThemuser";
            btnThemuser.Size = new Size(90, 40);
            btnThemuser.TabIndex = 4;
            btnThemuser.UseVisualStyleBackColor = false;
            btnThemuser.Click += btnThemuser_Click;
            // 
            // containersearch
            // 
            containersearch.Controls.Add(searchtextbox);
            containersearch.Location = new Point(42, 3);
            containersearch.Name = "containersearch";
            containersearch.Size = new Size(250, 58);
            containersearch.TabIndex = 7;
            // 
            // searchtextbox
            // 
            searchtextbox.Location = new Point(74, 22);
            searchtextbox.Name = "searchtextbox";
            searchtextbox.Size = new Size(125, 30);
            searchtextbox.TabIndex = 0;
            searchtextbox.TextChanged += searchtextbox_TextChanged_2;
            // 
            // picturemicro
            // 
            picturemicro.Cursor = Cursors.Hand;
            picturemicro.Image = Properties.Resources.microphone;
            picturemicro.Location = new Point(325, 11);
            picturemicro.Name = "picturemicro";
            picturemicro.Size = new Size(39, 34);
            picturemicro.SizeMode = PictureBoxSizeMode.Zoom;
            picturemicro.TabIndex = 6;
            picturemicro.TabStop = false;
            picturemicro.Click += picturemicro_Click;
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
            pictureFilter.Click += pictureFilter_Click;
            // 
            // panel7
            // 
            panel7.Controls.Add(dgvDanhsachnhanvien);
            panel7.Controls.Add(panel1);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(0, 70);
            panel7.Name = "panel7";
            panel7.Size = new Size(828, 402);
            panel7.TabIndex = 5;
            // 
            // dgvDanhsachnhanvien
            // 
            dgvDanhsachnhanvien.AllowUserToAddRows = false;
            dgvDanhsachnhanvien.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhsachnhanvien.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(0, 152, 70);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDanhsachnhanvien.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDanhsachnhanvien.ColumnHeadersHeight = 30;
            dgvDanhsachnhanvien.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvDanhsachnhanvien.DefaultCellStyle = dataGridViewCellStyle2;
            dgvDanhsachnhanvien.Dock = DockStyle.Fill;
            dgvDanhsachnhanvien.EnableHeadersVisualStyles = false;
            dgvDanhsachnhanvien.Location = new Point(0, 0);
            dgvDanhsachnhanvien.Name = "dgvDanhsachnhanvien";
            dgvDanhsachnhanvien.RowHeadersWidth = 51;
            dgvDanhsachnhanvien.Size = new Size(828, 356);
            dgvDanhsachnhanvien.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnSau);
            panel1.Controls.Add(soTrang);
            panel1.Controls.Add(btnTruoc);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 356);
            panel1.Name = "panel1";
            panel1.Size = new Size(828, 46);
            panel1.TabIndex = 4;
            panel1.Paint += panel1_Paint;
            // 
            // btnSau
            // 
            btnSau.Anchor = AnchorStyles.None;
            btnSau.BackColor = Color.FromArgb(255, 107, 53);
            btnSau.FlatAppearance.BorderColor = SystemColors.ActiveBorder;
            btnSau.FlatStyle = FlatStyle.Flat;
            btnSau.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSau.ForeColor = Color.White;
            btnSau.Location = new Point(490, 6);
            btnSau.Name = "btnSau";
            btnSau.Size = new Size(153, 33);
            btnSau.TabIndex = 1;
            btnSau.Text = "Trang sau";
            btnSau.UseVisualStyleBackColor = false;
            btnSau.Click += btnSau_Click;
            // 
            // soTrang
            // 
            soTrang.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            soTrang.AutoSize = true;
            soTrang.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            soTrang.ForeColor = Color.Black;
            soTrang.Location = new Point(362, 10);
            soTrang.Name = "soTrang";
            soTrang.Size = new Size(71, 23);
            soTrang.TabIndex = 2;
            soTrang.Text = "Trang 1";
            soTrang.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnTruoc
            // 
            btnTruoc.Anchor = AnchorStyles.None;
            btnTruoc.BackColor = Color.FromArgb(255, 107, 53);
            btnTruoc.FlatAppearance.BorderColor = SystemColors.ActiveBorder;
            btnTruoc.FlatStyle = FlatStyle.Flat;
            btnTruoc.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnTruoc.ForeColor = Color.White;
            btnTruoc.Location = new Point(190, 6);
            btnTruoc.Name = "btnTruoc";
            btnTruoc.Size = new Size(153, 33);
            btnTruoc.TabIndex = 0;
            btnTruoc.Text = "Trang trước";
            btnTruoc.UseVisualStyleBackColor = false;
            btnTruoc.Click += btnTruoc_Click;
            // 
            // DSNV_Uc
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel7);
            Controls.Add(panel6);
            Name = "DSNV_Uc";
            Size = new Size(828, 472);
            Load += DSNV_Uc_Load;
            panel6.ResumeLayout(false);
            containersearch.ResumeLayout(false);
            containersearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picturemicro).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureFilter).EndInit();
            panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDanhsachnhanvien).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel6;
        private Button btnXuatfile;
        private Button btnThemuser;
        private Panel containersearch;
        private TextBox searchtextbox;
        private PictureBox picturemicro;
        private PictureBox pictureFilter;
        private Panel panel7;
        private DataGridView dgvDanhsachnhanvien;
        private Panel panel1;
        private Button btnSau;
        private Label soTrang;
        private Button btnTruoc;
    }
}
