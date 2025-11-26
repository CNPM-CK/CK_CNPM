namespace GUI.Forms
{
    partial class DSKQUC
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DSKQUC));
            panel5 = new Panel();
            panel7 = new Panel();
            dgvDanhsachketqua = new DataGridView();
            panel1 = new Panel();
            btnSau = new Button();
            soTrang = new Label();
            btnTruoc = new Button();
            panel6 = new Panel();
            button1 = new Button();
            pictureFilter = new PictureBox();
            picturemicro = new PictureBox();
            containersearch = new Panel();
            searchtextbox = new TextBox();
            panel5.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhsachketqua).BeginInit();
            panel1.SuspendLayout();
            panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureFilter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picturemicro).BeginInit();
            containersearch.SuspendLayout();
            SuspendLayout();
            // 
            // panel5
            // 
            panel5.Controls.Add(panel7);
            panel5.Controls.Add(panel6);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(800, 450);
            panel5.TabIndex = 0;
            // 
            // panel7
            // 
            panel7.Controls.Add(dgvDanhsachketqua);
            panel7.Controls.Add(panel1);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(0, 61);
            panel7.Name = "panel7";
            panel7.Size = new Size(800, 389);
            panel7.TabIndex = 1;
            // 
            // dgvDanhsachketqua
            // 
            dgvDanhsachketqua.BackgroundColor = Color.White;
            dgvDanhsachketqua.ColumnHeadersHeight = 40;
            dgvDanhsachketqua.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDanhsachketqua.Dock = DockStyle.Fill;
            dgvDanhsachketqua.EnableHeadersVisualStyles = false;
            dgvDanhsachketqua.Location = new Point(0, 0);
            dgvDanhsachketqua.Name = "dgvDanhsachketqua";
            dgvDanhsachketqua.RowHeadersWidth = 51;
            dgvDanhsachketqua.Size = new Size(800, 343);
            dgvDanhsachketqua.TabIndex = 0;
            dgvDanhsachketqua.CellContentClick += dgvDanhsachketqua_CellContentClick;
            dgvDanhsachketqua.Paint += dgvDanhsachketqua_Paint;
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.Controls.Add(btnSau);
            panel1.Controls.Add(soTrang);
            panel1.Controls.Add(btnTruoc);
            panel1.Dock = DockStyle.Bottom;
            panel1.Location = new Point(0, 343);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 46);
            panel1.TabIndex = 5;
            // 
            // btnSau
            // 
            btnSau.Anchor = AnchorStyles.None;
            btnSau.BackColor = Color.FromArgb(255, 107, 53);
            btnSau.FlatAppearance.BorderColor = SystemColors.ActiveBorder;
            btnSau.FlatStyle = FlatStyle.Flat;
            btnSau.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSau.ForeColor = Color.White;
            btnSau.Location = new Point(463, 6);
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
            soTrang.Location = new Point(344, 12);
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
            btnTruoc.Location = new Point(184, 7);
            btnTruoc.Name = "btnTruoc";
            btnTruoc.Size = new Size(153, 33);
            btnTruoc.TabIndex = 0;
            btnTruoc.Text = "Trang trước";
            btnTruoc.UseVisualStyleBackColor = false;
            btnTruoc.Click += btnTruoc_Click;
            // 
            // panel6
            // 
            panel6.BackColor = Color.White;
            panel6.Controls.Add(button1);
            panel6.Controls.Add(pictureFilter);
            panel6.Controls.Add(picturemicro);
            panel6.Controls.Add(containersearch);
            panel6.Dock = DockStyle.Top;
            panel6.Font = new Font("Segoe UI", 10F);
            panel6.Location = new Point(0, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(800, 61);
            panel6.TabIndex = 0;
            panel6.Paint += panel6_Paint_2;
            // 
            // button1
            // 
            button1.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Location = new Point(676, 3);
            button1.Name = "button1";
            button1.Size = new Size(46, 58);
            button1.TabIndex = 11;
            button1.UseVisualStyleBackColor = true;
            // 
            // pictureFilter
            // 
            pictureFilter.Cursor = Cursors.Hand;
            pictureFilter.Image = (Image)resources.GetObject("pictureFilter.Image");
            pictureFilter.Location = new Point(24, 14);
            pictureFilter.Name = "pictureFilter";
            pictureFilter.Size = new Size(33, 33);
            pictureFilter.SizeMode = PictureBoxSizeMode.Zoom;
            pictureFilter.TabIndex = 10;
            pictureFilter.TabStop = false;
            // 
            // picturemicro
            // 
            picturemicro.Cursor = Cursors.Hand;
            picturemicro.Image = Properties.Resources.microphone;
            picturemicro.Location = new Point(381, 13);
            picturemicro.Name = "picturemicro";
            picturemicro.Size = new Size(39, 34);
            picturemicro.SizeMode = PictureBoxSizeMode.Zoom;
            picturemicro.TabIndex = 9;
            picturemicro.TabStop = false;
            // 
            // containersearch
            // 
            containersearch.Controls.Add(searchtextbox);
            containersearch.Location = new Point(76, 3);
            containersearch.Name = "containersearch";
            containersearch.Size = new Size(250, 58);
            containersearch.TabIndex = 8;
            // 
            // searchtextbox
            // 
            searchtextbox.Location = new Point(74, 22);
            searchtextbox.Name = "searchtextbox";
            searchtextbox.Size = new Size(125, 30);
            searchtextbox.TabIndex = 0;
            // 
            // DSKQUC
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel5);
            Name = "DSKQUC";
            Size = new Size(800, 450);
            panel5.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDanhsachketqua).EndInit();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureFilter).EndInit();
            ((System.ComponentModel.ISupportInitialize)picturemicro).EndInit();
            containersearch.ResumeLayout(false);
            containersearch.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel5;
        private Panel panel7;
        private DataGridView dgvDanhsachketqua;
        private Panel panel6;
        private Panel containersearch;
        private TextBox searchtextbox;
        private PictureBox picturemicro;
        private PictureBox pictureFilter;
        private Panel panel1;
        private Button btnSau;
        private Label soTrang;
        private Button btnTruoc;
        private Button button1;
    }
}