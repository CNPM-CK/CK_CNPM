namespace GUI.Forms
{
    partial class DRAF
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
            panel16 = new Panel();
            panelHuyen = new Panel();
            cbbQuan = new ComboBox();
            panelTinh = new Panel();
            cboTinhThanh = new ComboBox();
            pictureBox8 = new PictureBox();
            label10 = new Label();
            panel2 = new Panel();
            panel22 = new Panel();
            panelSonha = new Panel();
            txtDiaChi = new TextBox();
            panelXa = new Panel();
            cbbXa = new ComboBox();
            panel1.SuspendLayout();
            panel16.SuspendLayout();
            panelHuyen.SuspendLayout();
            panelTinh.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            panel2.SuspendLayout();
            panel22.SuspendLayout();
            panelSonha.SuspendLayout();
            panelXa.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel16);
            panel1.Location = new Point(380, 122);
            panel1.Name = "panel1";
            panel1.Size = new Size(250, 125);
            panel1.TabIndex = 0;
            // 
            // panel16
            // 
            panel16.Controls.Add(panelHuyen);
            panel16.Controls.Add(panelTinh);
            panel16.Controls.Add(pictureBox8);
            panel16.Controls.Add(label10);
            panel16.Dock = DockStyle.Fill;
            panel16.Location = new Point(0, 0);
            panel16.Name = "panel16";
            panel16.Size = new Size(250, 125);
            panel16.TabIndex = 7;
            // 
            // panelHuyen
            // 
            panelHuyen.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panelHuyen.Controls.Add(cbbQuan);
            panelHuyen.Location = new Point(68, 95);
            panelHuyen.Name = "panelHuyen";
            panelHuyen.Size = new Size(324, 38);
            panelHuyen.TabIndex = 9;
            // 
            // cbbQuan
            // 
            cbbQuan.Dock = DockStyle.Fill;
            cbbQuan.FormattingEnabled = true;
            cbbQuan.Location = new Point(0, 0);
            cbbQuan.Name = "cbbQuan";
            cbbQuan.Size = new Size(324, 28);
            cbbQuan.TabIndex = 7;
            // 
            // panelTinh
            // 
            panelTinh.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panelTinh.Controls.Add(cboTinhThanh);
            panelTinh.Location = new Point(68, 49);
            panelTinh.Name = "panelTinh";
            panelTinh.Size = new Size(324, 40);
            panelTinh.TabIndex = 8;
            // 
            // cboTinhThanh
            // 
            cboTinhThanh.Dock = DockStyle.Fill;
            cboTinhThanh.FormattingEnabled = true;
            cboTinhThanh.Location = new Point(0, 0);
            cboTinhThanh.Name = "cboTinhThanh";
            cboTinhThanh.Size = new Size(324, 28);
            cboTinhThanh.TabIndex = 0;
            // 
            // pictureBox8
            // 
            pictureBox8.Location = new Point(71, 14);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(27, 20);
            pictureBox8.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox8.TabIndex = 6;
            pictureBox8.TabStop = false;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label10.Location = new Point(126, 11);
            label10.Name = "label10";
            label10.Size = new Size(70, 23);
            label10.TabIndex = 0;
            label10.Text = "Địa chỉ ";
            // 
            // panel2
            // 
            panel2.Controls.Add(panel22);
            panel2.Location = new Point(150, 239);
            panel2.Name = "panel2";
            panel2.Size = new Size(250, 125);
            panel2.TabIndex = 1;
            // 
            // panel22
            // 
            panel22.Controls.Add(panelSonha);
            panel22.Controls.Add(panelXa);
            panel22.Dock = DockStyle.Fill;
            panel22.Location = new Point(0, 0);
            panel22.Name = "panel22";
            panel22.Size = new Size(250, 125);
            panel22.TabIndex = 11;
            // 
            // panelSonha
            // 
            panelSonha.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panelSonha.Controls.Add(txtDiaChi);
            panelSonha.Location = new Point(68, 61);
            panelSonha.Name = "panelSonha";
            panelSonha.Size = new Size(324, 40);
            panelSonha.TabIndex = 5;
            // 
            // txtDiaChi
            // 
            txtDiaChi.BorderStyle = BorderStyle.FixedSingle;
            txtDiaChi.Dock = DockStyle.Fill;
            txtDiaChi.Location = new Point(0, 0);
            txtDiaChi.Name = "txtDiaChi";
            txtDiaChi.PlaceholderText = "Số nhà,Tên đường";
            txtDiaChi.Size = new Size(324, 27);
            txtDiaChi.TabIndex = 3;
            txtDiaChi.TextChanged += txtDiaChi_TextChanged;
            // 
            // panelXa
            // 
            panelXa.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            panelXa.Controls.Add(cbbXa);
            panelXa.Location = new Point(68, 15);
            panelXa.Name = "panelXa";
            panelXa.Size = new Size(324, 40);
            panelXa.TabIndex = 4;
            // 
            // cbbXa
            // 
            cbbXa.Dock = DockStyle.Fill;
            cbbXa.FormattingEnabled = true;
            cbbXa.Location = new Point(0, 0);
            cbbXa.Name = "cbbXa";
            cbbXa.Size = new Size(324, 28);
            cbbXa.TabIndex = 1;
            cbbXa.Text = "Xã/Phường";
            // 
            // DRAF
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "DRAF";
            Text = "DRAF";
            panel1.ResumeLayout(false);
            panel16.ResumeLayout(false);
            panel16.PerformLayout();
            panelHuyen.ResumeLayout(false);
            panelTinh.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            panel2.ResumeLayout(false);
            panel22.ResumeLayout(false);
            panelSonha.ResumeLayout(false);
            panelSonha.PerformLayout();
            panelXa.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel16;
        private Panel panelHuyen;
        private ComboBox cbbQuan;
        private Panel panelTinh;
        private ComboBox cboTinhThanh;
        private PictureBox pictureBox8;
        private Label label10;
        private Panel panel2;
        private Panel panel22;
        private Panel panelSonha;
        private TextBox txtDiaChi;
        private Panel panelXa;
        private ComboBox cbbXa;
    }
}