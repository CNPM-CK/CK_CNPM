namespace GUI.Forms
{
    partial class ChiTietNenMau
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
            pictureBox1 = new PictureBox();
            label = new Label();
            panel2 = new Panel();
            groupBox1 = new GroupBox();
            dgvThongso = new DataGridView();
            panel3 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel7 = new Panel();
            label2 = new Label();
            panelPhongpt = new Panel();
            cboPhongpt = new ComboBox();
            panel5 = new Panel();
            panelChonthongso = new Panel();
            cboThongso = new ComboBox();
            label3 = new Label();
            tableLayoutPanel3 = new TableLayoutPanel();
            panel6 = new Panel();
            btnThemthongso = new Button();
            panel8 = new Panel();
            tableLayoutPanel4 = new TableLayoutPanel();
            panel15 = new Panel();
            label7 = new Label();
            panelGhichu = new Panel();
            txtGhichu = new TextBox();
            panel13 = new Panel();
            label6 = new Label();
            panelToado = new Panel();
            txtToado = new TextBox();
            panel11 = new Panel();
            label5 = new Label();
            panelVitri = new Panel();
            txtTenvitri = new TextBox();
            panel10 = new Panel();
            label4 = new Label();
            panelTennenmau = new Panel();
            txtTennenmau = new TextBox();
            panel4 = new Panel();
            btnThemts = new Button();
            btnHuy = new Button();
            btnLuu = new Button();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvThongso).BeginInit();
            panel3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel7.SuspendLayout();
            panelPhongpt.SuspendLayout();
            panel5.SuspendLayout();
            panelChonthongso.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel6.SuspendLayout();
            panel8.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel15.SuspendLayout();
            panelGhichu.SuspendLayout();
            panel13.SuspendLayout();
            panelToado.SuspendLayout();
            panel11.SuspendLayout();
            panelVitri.SuspendLayout();
            panel10.SuspendLayout();
            panelTennenmau.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(0, 152, 70);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(label);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1137, 65);
            panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.remove_background_logo;
            pictureBox1.Location = new Point(0, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(155, 65);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label
            // 
            label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label.AutoSize = true;
            label.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label.ForeColor = Color.White;
            label.Location = new Point(469, 9);
            label.Name = "label";
            label.Size = new Size(270, 38);
            label.TabIndex = 0;
            label.Text = "CHI TIẾT NỀN MẪU";
            label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            panel2.Controls.Add(groupBox1);
            panel2.Controls.Add(panel8);
            panel2.Controls.Add(panel4);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 65);
            panel2.Name = "panel2";
            panel2.Size = new Size(1137, 465);
            panel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dgvThongso);
            groupBox1.Controls.Add(panel3);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            groupBox1.Location = new Point(0, 142);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1137, 268);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông số quan trắc";
            // 
            // dgvThongso
            // 
            dgvThongso.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvThongso.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvThongso.Dock = DockStyle.Fill;
            dgvThongso.Location = new Point(3, 98);
            dgvThongso.Name = "dgvThongso";
            dgvThongso.RowHeadersWidth = 51;
            dgvThongso.Size = new Size(1131, 167);
            dgvThongso.TabIndex = 1;
            dgvThongso.CellClick += dgvThongso_CellClick;
            dgvThongso.CellContentClick += dgvThongso_CellContentClick_1;
            dgvThongso.CellPainting += dgvThongso_CellPainting;
            // 
            // panel3
            // 
            panel3.Controls.Add(tableLayoutPanel2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(3, 23);
            panel3.Name = "panel3";
            panel3.Size = new Size(1131, 75);
            panel3.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80.20362F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 19.796381F));
            tableLayoutPanel2.Controls.Add(tableLayoutPanel1, 0, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel3, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(1131, 75);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(panel7, 1, 0);
            tableLayoutPanel1.Controls.Add(panel5, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(901, 69);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // panel7
            // 
            panel7.Controls.Add(label2);
            panel7.Controls.Add(panelPhongpt);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(453, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(445, 63);
            panel7.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(10, 23);
            label2.Name = "label2";
            label2.Size = new Size(132, 20);
            label2.TabIndex = 0;
            label2.Text = "Phòng thực hiện :";
            // 
            // panelPhongpt
            // 
            panelPhongpt.Anchor = AnchorStyles.None;
            panelPhongpt.Controls.Add(cboPhongpt);
            panelPhongpt.Location = new Point(159, 10);
            panelPhongpt.Name = "panelPhongpt";
            panelPhongpt.Size = new Size(212, 45);
            panelPhongpt.TabIndex = 1;
            // 
            // cboPhongpt
            // 
            cboPhongpt.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhongpt.FormattingEnabled = true;
            cboPhongpt.Items.AddRange(new object[] { "Phòng thí nghiệm", "Phòng hiện trường" });
            cboPhongpt.Location = new Point(3, 10);
            cboPhongpt.Name = "cboPhongpt";
            cboPhongpt.Size = new Size(206, 28);
            cboPhongpt.TabIndex = 0;
            cboPhongpt.SelectedIndexChanged += cboPhongpt_SelectedIndexChanged;
            cboPhongpt.Click += cboPhongpt_Click;
            // 
            // panel5
            // 
            panel5.Controls.Add(panelChonthongso);
            panel5.Controls.Add(label3);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(3, 3);
            panel5.Name = "panel5";
            panel5.Size = new Size(444, 63);
            panel5.TabIndex = 0;
            // 
            // panelChonthongso
            // 
            panelChonthongso.Anchor = AnchorStyles.None;
            panelChonthongso.Controls.Add(cboThongso);
            panelChonthongso.Location = new Point(131, 10);
            panelChonthongso.Name = "panelChonthongso";
            panelChonthongso.Size = new Size(212, 45);
            panelChonthongso.TabIndex = 0;
            // 
            // cboThongso
            // 
            cboThongso.DropDownStyle = ComboBoxStyle.DropDownList;
            cboThongso.FormattingEnabled = true;
            cboThongso.Location = new Point(3, 9);
            cboThongso.Name = "cboThongso";
            cboThongso.Size = new Size(206, 28);
            cboThongso.TabIndex = 0;
            cboThongso.SelectedIndexChanged += cboThongso_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(3, 22);
            label3.Name = "label3";
            label3.Size = new Size(119, 20);
            label3.TabIndex = 0;
            label3.Text = "Chọn thông số :";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(panel6, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(910, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(218, 69);
            tableLayoutPanel3.TabIndex = 4;
            // 
            // panel6
            // 
            panel6.Anchor = AnchorStyles.None;
            panel6.Controls.Add(btnThemthongso);
            panel6.Location = new Point(36, 6);
            panel6.Name = "panel6";
            panel6.Size = new Size(146, 57);
            panel6.TabIndex = 2;
            // 
            // btnThemthongso
            // 
            btnThemthongso.BackColor = Color.FromArgb(0, 152, 70);
            btnThemthongso.Cursor = Cursors.Hand;
            btnThemthongso.FlatStyle = FlatStyle.Flat;
            btnThemthongso.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThemthongso.ForeColor = Color.White;
            btnThemthongso.Location = new Point(3, 7);
            btnThemthongso.Name = "btnThemthongso";
            btnThemthongso.Size = new Size(140, 45);
            btnThemthongso.TabIndex = 0;
            btnThemthongso.Text = "Thêm";
            btnThemthongso.UseVisualStyleBackColor = false;
            btnThemthongso.Click += btnThemthongso_Click_1;
            // 
            // panel8
            // 
            panel8.Controls.Add(tableLayoutPanel4);
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(0, 0);
            panel8.Name = "panel8";
            panel8.Size = new Size(1137, 142);
            panel8.TabIndex = 5;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(panel15, 1, 1);
            tableLayoutPanel4.Controls.Add(panel13, 0, 1);
            tableLayoutPanel4.Controls.Add(panel11, 1, 0);
            tableLayoutPanel4.Controls.Add(panel10, 0, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(1137, 142);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // panel15
            // 
            panel15.Controls.Add(label7);
            panel15.Controls.Add(panelGhichu);
            panel15.Dock = DockStyle.Fill;
            panel15.Location = new Point(571, 74);
            panel15.Name = "panel15";
            panel15.Size = new Size(563, 65);
            panel15.TabIndex = 3;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(9, 26);
            label7.Name = "label7";
            label7.Size = new Size(70, 20);
            label7.TabIndex = 1;
            label7.Text = "Ghi chú :";
            // 
            // panelGhichu
            // 
            panelGhichu.Controls.Add(txtGhichu);
            panelGhichu.Location = new Point(134, 22);
            panelGhichu.Name = "panelGhichu";
            panelGhichu.Size = new Size(316, 35);
            panelGhichu.TabIndex = 0;
            // 
            // txtGhichu
            // 
            txtGhichu.BorderStyle = BorderStyle.FixedSingle;
            txtGhichu.Location = new Point(3, 4);
            txtGhichu.Multiline = true;
            txtGhichu.Name = "txtGhichu";
            txtGhichu.Size = new Size(310, 28);
            txtGhichu.TabIndex = 0;
            // 
            // panel13
            // 
            panel13.Controls.Add(label6);
            panel13.Controls.Add(panelToado);
            panel13.Dock = DockStyle.Fill;
            panel13.Location = new Point(3, 74);
            panel13.Name = "panel13";
            panel13.Size = new Size(562, 65);
            panel13.TabIndex = 2;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(12, 26);
            label6.Name = "label6";
            label6.Size = new Size(65, 20);
            label6.TabIndex = 1;
            label6.Text = "Tọa độ :";
            // 
            // panelToado
            // 
            panelToado.Controls.Add(txtToado);
            panelToado.Location = new Point(134, 18);
            panelToado.Name = "panelToado";
            panelToado.Size = new Size(316, 35);
            panelToado.TabIndex = 0;
            // 
            // txtToado
            // 
            txtToado.BorderStyle = BorderStyle.FixedSingle;
            txtToado.Location = new Point(3, 4);
            txtToado.Multiline = true;
            txtToado.Name = "txtToado";
            txtToado.Size = new Size(310, 28);
            txtToado.TabIndex = 0;
            // 
            // panel11
            // 
            panel11.Controls.Add(label5);
            panel11.Controls.Add(panelVitri);
            panel11.Dock = DockStyle.Fill;
            panel11.Location = new Point(571, 3);
            panel11.Name = "panel11";
            panel11.Size = new Size(563, 65);
            panel11.TabIndex = 1;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(9, 26);
            label5.Name = "label5";
            label5.Size = new Size(78, 20);
            label5.TabIndex = 1;
            label5.Text = "Tên vị trí :";
            // 
            // panelVitri
            // 
            panelVitri.Controls.Add(txtTenvitri);
            panelVitri.Location = new Point(134, 18);
            panelVitri.Name = "panelVitri";
            panelVitri.Size = new Size(316, 35);
            panelVitri.TabIndex = 0;
            // 
            // txtTenvitri
            // 
            txtTenvitri.BorderStyle = BorderStyle.FixedSingle;
            txtTenvitri.Location = new Point(3, 4);
            txtTenvitri.Multiline = true;
            txtTenvitri.Name = "txtTenvitri";
            txtTenvitri.Size = new Size(310, 28);
            txtTenvitri.TabIndex = 0;
            // 
            // panel10
            // 
            panel10.Controls.Add(label4);
            panel10.Controls.Add(panelTennenmau);
            panel10.Dock = DockStyle.Fill;
            panel10.Location = new Point(3, 3);
            panel10.Name = "panel10";
            panel10.Size = new Size(562, 65);
            panel10.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(9, 26);
            label4.Name = "label4";
            label4.Size = new Size(107, 20);
            label4.TabIndex = 1;
            label4.Text = "Tên nền mẫu :";
            // 
            // panelTennenmau
            // 
            panelTennenmau.Controls.Add(txtTennenmau);
            panelTennenmau.Location = new Point(134, 18);
            panelTennenmau.Name = "panelTennenmau";
            panelTennenmau.Size = new Size(316, 35);
            panelTennenmau.TabIndex = 0;
            // 
            // txtTennenmau
            // 
            txtTennenmau.BorderStyle = BorderStyle.FixedSingle;
            txtTennenmau.Location = new Point(6, 4);
            txtTennenmau.Multiline = true;
            txtTennenmau.Name = "txtTennenmau";
            txtTennenmau.Size = new Size(307, 28);
            txtTennenmau.TabIndex = 0;
            txtTennenmau.TextChanged += txtTennenmau_TextChanged;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnThemts);
            panel4.Controls.Add(btnHuy);
            panel4.Controls.Add(btnLuu);
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(0, 410);
            panel4.Name = "panel4";
            panel4.Size = new Size(1137, 55);
            panel4.TabIndex = 4;
            // 
            // btnThemts
            // 
            btnThemts.BackColor = Color.FromArgb(255, 107, 53);
            btnThemts.Cursor = Cursors.Hand;
            btnThemts.FlatStyle = FlatStyle.Flat;
            btnThemts.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnThemts.ForeColor = Color.White;
            btnThemts.Location = new Point(690, 5);
            btnThemts.Name = "btnThemts";
            btnThemts.Size = new Size(140, 45);
            btnThemts.TabIndex = 0;
            btnThemts.Text = "Thông số mới";
            btnThemts.UseVisualStyleBackColor = false;
            btnThemts.Click += btnThemts_Click;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.Red;
            btnHuy.Cursor = Cursors.Hand;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(529, 5);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(140, 45);
            btnHuy.TabIndex = 2;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += btnHuy_Click;
            // 
            // btnLuu
            // 
            btnLuu.BackColor = Color.FromArgb(0, 152, 70);
            btnLuu.FlatStyle = FlatStyle.Flat;
            btnLuu.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnLuu.ForeColor = Color.White;
            btnLuu.Location = new Point(366, 5);
            btnLuu.Name = "btnLuu";
            btnLuu.Size = new Size(140, 45);
            btnLuu.TabIndex = 1;
            btnLuu.Text = "Lưu nền mẫu";
            btnLuu.UseVisualStyleBackColor = false;
            btnLuu.Click += btnLuu_Click;
            // 
            // ChiTietNenMau
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1137, 530);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "ChiTietNenMau";
            Text = "Form1";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvThongso).EndInit();
            panel3.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            panelPhongpt.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            panelChonthongso.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel8.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            panel15.ResumeLayout(false);
            panel15.PerformLayout();
            panelGhichu.ResumeLayout(false);
            panelGhichu.PerformLayout();
            panel13.ResumeLayout(false);
            panel13.PerformLayout();
            panelToado.ResumeLayout(false);
            panelToado.PerformLayout();
            panel11.ResumeLayout(false);
            panel11.PerformLayout();
            panelVitri.ResumeLayout(false);
            panelVitri.PerformLayout();
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            panelTennenmau.ResumeLayout(false);
            panelTennenmau.PerformLayout();
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label;
        private Panel panel2;
        private Button btnThemts;
        private Panel panel4;
        private Button btnHuy;
        private Button btnLuu;
        private Panel panel8;
        private Label label4;
        private Panel panelTennenmau;
        private TextBox txtTennenmau;
        private TableLayoutPanel tableLayoutPanel4;
        private Panel panel15;
        private Label label7;
        private Panel panelGhichu;
        private TextBox txtGhichu;
        private Panel panel13;
        private Label label6;
        private Panel panelToado;
        private TextBox txtToado;
        private Panel panel11;
        private Label label5;
        private Panel panelVitri;
        private TextBox txtTenvitri;
        private Panel panel10;
        private GroupBox groupBox1;
        private DataGridView dgvThongso;
        private Panel panel3;
        private TableLayoutPanel tableLayoutPanel2;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel7;
        private Label label2;
        private Panel panelPhongpt;
        private ComboBox cboPhongpt;
        private Panel panel5;
        private Panel panelChonthongso;
        private ComboBox cboThongso;
        private Label label3;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel panel6;
        private Button btnThemthongso;
        private PictureBox pictureBox1;
    }
}