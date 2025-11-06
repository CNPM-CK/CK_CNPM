namespace GUI.Forms
{
    partial class DanhSachThongSo
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DanhSachThongSo));
            panel5 = new Panel();
            panel7 = new Panel();
            dgvDSTS = new DataGridView();
            panel6 = new Panel();
            btnXuatfile = new Button();
            containersearch = new Panel();
            searchtextbox = new TextBox();
            picturemicro = new PictureBox();
            btnThemuser = new Button();
            pictureFilter = new PictureBox();
            panel5.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDSTS).BeginInit();
            panel6.SuspendLayout();
            containersearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picturemicro).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureFilter).BeginInit();
            SuspendLayout();
            // 
            // panel5
            // 
            panel5.Controls.Add(panel7);
            panel5.Controls.Add(panel6);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(820, 472);
            panel5.TabIndex = 2;
            // 
            // panel7
            // 
            panel7.Controls.Add(dgvDSTS);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(0, 61);
            panel7.Name = "panel7";
            panel7.Size = new Size(820, 411);
            panel7.TabIndex = 1;
            // 
            // dgvDSTS
            // 
            dgvDSTS.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDSTS.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(0, 152, 70);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvDSTS.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvDSTS.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDSTS.Dock = DockStyle.Fill;
            dgvDSTS.Location = new Point(0, 0);
            dgvDSTS.Name = "dgvDSTS";
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(0, 152, 70);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvDSTS.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvDSTS.RowHeadersWidth = 51;
            dgvDSTS.Size = new Size(820, 411);
            dgvDSTS.TabIndex = 1;
            dgvDSTS.CellContentClick += dgvDSKH_CellContentClick;
            dgvDSTS.Paint += dgvDSTS_Paint_1;
            // 
            // panel6
            // 
            panel6.BackColor = Color.White;
            panel6.Controls.Add(btnXuatfile);
            panel6.Controls.Add(containersearch);
            panel6.Controls.Add(picturemicro);
            panel6.Controls.Add(btnThemuser);
            panel6.Controls.Add(pictureFilter);
            panel6.Dock = DockStyle.Top;
            panel6.Font = new Font("Segoe UI", 10F);
            panel6.Location = new Point(0, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(820, 61);
            panel6.TabIndex = 0;
            panel6.Paint += panel6_Paint;
            // 
            // btnXuatfile
            // 
            btnXuatfile.Anchor = AnchorStyles.None;
            btnXuatfile.BackColor = Color.FromArgb(255, 107, 53);
            btnXuatfile.BackgroundImageLayout = ImageLayout.None;
            btnXuatfile.Cursor = Cursors.Hand;
            btnXuatfile.FlatAppearance.BorderColor = Color.White;
            btnXuatfile.FlatStyle = FlatStyle.Flat;
            btnXuatfile.Image = (Image)resources.GetObject("btnXuatfile.Image");
            btnXuatfile.Location = new Point(742, 12);
            btnXuatfile.Name = "btnXuatfile";
            btnXuatfile.Size = new Size(66, 40);
            btnXuatfile.TabIndex = 8;
            btnXuatfile.UseVisualStyleBackColor = false;
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
            searchtextbox.Location = new Point(22, 22);
            searchtextbox.Multiline = true;
            searchtextbox.Name = "searchtextbox";
            searchtextbox.Size = new Size(177, 30);
            searchtextbox.TabIndex = 0;
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
            // btnThemuser
            // 
            btnThemuser.Anchor = AnchorStyles.None;
            btnThemuser.BackColor = Color.FromArgb(255, 107, 53);
            btnThemuser.BackgroundImageLayout = ImageLayout.None;
            btnThemuser.Cursor = Cursors.Hand;
            btnThemuser.FlatAppearance.BorderColor = Color.White;
            btnThemuser.FlatStyle = FlatStyle.Flat;
            btnThemuser.Image = (Image)resources.GetObject("btnThemuser.Image");
            btnThemuser.Location = new Point(670, 12);
            btnThemuser.Name = "btnThemuser";
            btnThemuser.Size = new Size(66, 40);
            btnThemuser.TabIndex = 4;
            btnThemuser.UseVisualStyleBackColor = false;
            btnThemuser.Click += btnThemuser_Click_1;
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
            // 
            // DanhSachThongSo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel5);
            Name = "DanhSachThongSo";
            Size = new Size(820, 472);
            Load += DanhSachThongSo_Load;
            panel5.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDSTS).EndInit();
            panel6.ResumeLayout(false);
            containersearch.ResumeLayout(false);
            containersearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picturemicro).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureFilter).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel5;
        private Panel panel7;
        private DataGridView dgvDSTS;
        private Panel panel6;
        private Panel containersearch;
        private TextBox searchtextbox;
        private PictureBox picturemicro;
        private Button btnThemuser;
        private PictureBox pictureFilter;
        private Button btnXuatfile;
    }
}
