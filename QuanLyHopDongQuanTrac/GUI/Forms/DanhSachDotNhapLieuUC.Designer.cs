namespace GUI.Forms
{
    partial class DanhSachDotNhapLieuUC
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DanhSachDotNhapLieuUC));
            panel5 = new Panel();
            panel7 = new Panel();
            panel1 = new Panel();
            btnSau = new Button();
            soTrang = new Label();
            btnTruoc = new Button();
            dgvDsdotquantrac = new DataGridView();
            panel6 = new Panel();
            containersearch = new Panel();
            searchtextbox = new TextBox();
            picturemicro = new PictureBox();
            pictureFilter = new PictureBox();
            panel5.SuspendLayout();
            panel7.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDsdotquantrac).BeginInit();
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
            panel7.Controls.Add(panel1);
            panel7.Controls.Add(dgvDsdotquantrac);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(0, 61);
            panel7.Name = "panel7";
            panel7.Size = new Size(820, 411);
            panel7.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnSau);
            panel1.Controls.Add(soTrang);
            panel1.Controls.Add(btnTruoc);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 365);
            panel1.Name = "panel1";
            panel1.Size = new Size(820, 46);
            panel1.TabIndex = 3;
            // 
            // btnSau
            // 
            btnSau.Anchor = AnchorStyles.None;
            btnSau.BackColor = Color.FromArgb(255, 107, 53);
            btnSau.FlatAppearance.BorderColor = SystemColors.ActiveBorder;
            btnSau.FlatStyle = FlatStyle.Flat;
            btnSau.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSau.ForeColor = Color.White;
            btnSau.Location = new Point(485, 9);
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
            soTrang.Location = new Point(384, 13);
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
            btnTruoc.Location = new Point(211, 9);
            btnTruoc.Name = "btnTruoc";
            btnTruoc.Size = new Size(153, 33);
            btnTruoc.TabIndex = 0;
            btnTruoc.Text = "Trang trước";
            btnTruoc.UseVisualStyleBackColor = false;
            btnTruoc.Click += btnTruoc_Click;
            // 
            // dgvDsdotquantrac
            // 
            dgvDsdotquantrac.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDsdotquantrac.BackgroundColor = Color.White;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(0, 152, 70);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvDsdotquantrac.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvDsdotquantrac.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvDsdotquantrac.Dock = DockStyle.Fill;
            dgvDsdotquantrac.Location = new Point(0, 0);
            dgvDsdotquantrac.Name = "dgvDsdotquantrac";
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(0, 152, 70);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvDsdotquantrac.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvDsdotquantrac.RowHeadersWidth = 51;
            dgvDsdotquantrac.Size = new Size(820, 411);
            dgvDsdotquantrac.TabIndex = 1;
            dgvDsdotquantrac.CellContentClick += dgvDSKH_CellContentClick;
            // 
            // panel6
            // 
            panel6.BackColor = Color.White;
            panel6.Controls.Add(containersearch);
            panel6.Controls.Add(picturemicro);
            panel6.Controls.Add(pictureFilter);
            panel6.Dock = DockStyle.Top;
            panel6.Font = new Font("Segoe UI", 10F);
            panel6.Location = new Point(0, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(820, 61);
            panel6.TabIndex = 0;
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
            picturemicro.Click += BtnMic_Click;
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
            pictureFilter.Visible = false;
            pictureFilter.Click += pictureFilter_Click;
            // 
            // DanhSachDotNhapLieuUC
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel5);
            Name = "DanhSachDotNhapLieuUC";
            Size = new Size(820, 472);
            Load += DanhSachThongSo_Load;
            panel5.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDsdotquantrac).EndInit();
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
        private DataGridView dgvDsdotquantrac;
        private Panel panel6;
        private Panel containersearch;
        private TextBox searchtextbox;
        private PictureBox picturemicro;
        private PictureBox pictureFilter;
        private Panel panel1;
        private Button btnSau;
        private Label soTrang;
        private Button btnTruoc;
    }
}
