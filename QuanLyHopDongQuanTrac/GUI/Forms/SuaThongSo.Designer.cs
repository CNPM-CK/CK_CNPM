namespace GUI.Forms
{
    partial class SuaThongSo
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
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            label1 = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel3 = new Panel();
            label4 = new Label();
            panelPhuongphap = new Panel();
            txtPhuongphap = new TextBox();
            panelDonvi = new Panel();
            txtDonvi = new TextBox();
            panelTents = new Panel();
            txtTents = new TextBox();
            label3 = new Label();
            label2 = new Label();
            panel4 = new Panel();
            btnHuy = new Button();
            panelMin = new Panel();
            txtMin = new TextBox();
            btnThem = new Button();
            panelMax = new Panel();
            txtMax = new TextBox();
            label6 = new Label();
            label5 = new Label();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel3.SuspendLayout();
            panelPhuongphap.SuspendLayout();
            panelDonvi.SuspendLayout();
            panelTents.SuspendLayout();
            panel4.SuspendLayout();
            panelMin.SuspendLayout();
            panelMax.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(511, 468);
            panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            tableLayoutPanel1.Size = new Size(511, 468);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(0, 152, 70);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Fill;
            panel2.ForeColor = Color.White;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(505, 64);
            panel2.TabIndex = 0;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(134, 19);
            label1.Name = "label1";
            label1.Size = new Size(227, 31);
            label1.TabIndex = 0;
            label1.Text = "Chỉnh sửa thông số ";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel3, 0, 0);
            tableLayoutPanel2.Controls.Add(panel4, 0, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 73);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tableLayoutPanel2.Size = new Size(505, 392);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(label4);
            panel3.Controls.Add(panelPhuongphap);
            panel3.Controls.Add(panelDonvi);
            panel3.Controls.Add(panelTents);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(499, 209);
            panel3.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(107, 143);
            label4.Name = "label4";
            label4.Size = new Size(111, 20);
            label4.TabIndex = 5;
            label4.Text = "Phương pháp :";
            // 
            // panelPhuongphap
            // 
            panelPhuongphap.Controls.Add(txtPhuongphap);
            panelPhuongphap.Location = new Point(110, 166);
            panelPhuongphap.Name = "panelPhuongphap";
            panelPhuongphap.Size = new Size(275, 40);
            panelPhuongphap.TabIndex = 4;
            // 
            // txtPhuongphap
            // 
            txtPhuongphap.BorderStyle = BorderStyle.FixedSingle;
            txtPhuongphap.Location = new Point(3, 3);
            txtPhuongphap.Name = "txtPhuongphap";
            txtPhuongphap.Size = new Size(269, 27);
            txtPhuongphap.TabIndex = 1;
            // 
            // panelDonvi
            // 
            panelDonvi.Controls.Add(txtDonvi);
            panelDonvi.Location = new Point(110, 89);
            panelDonvi.Name = "panelDonvi";
            panelDonvi.Size = new Size(275, 40);
            panelDonvi.TabIndex = 3;
            // 
            // txtDonvi
            // 
            txtDonvi.BorderStyle = BorderStyle.FixedSingle;
            txtDonvi.Location = new Point(3, 3);
            txtDonvi.Name = "txtDonvi";
            txtDonvi.Size = new Size(269, 27);
            txtDonvi.TabIndex = 1;
            // 
            // panelTents
            // 
            panelTents.Controls.Add(txtTents);
            panelTents.Location = new Point(110, 23);
            panelTents.Name = "panelTents";
            panelTents.Size = new Size(275, 40);
            panelTents.TabIndex = 2;
            // 
            // txtTents
            // 
            txtTents.BorderStyle = BorderStyle.FixedSingle;
            txtTents.Location = new Point(3, 3);
            txtTents.Name = "txtTents";
            txtTents.Size = new Size(269, 27);
            txtTents.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(107, 66);
            label3.Name = "label3";
            label3.Size = new Size(62, 20);
            label3.TabIndex = 1;
            label3.Text = "Đơn vị :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(107, 0);
            label2.Name = "label2";
            label2.Size = new Size(108, 20);
            label2.TabIndex = 0;
            label2.Text = "Tên thông số :";
            // 
            // panel4
            // 
            panel4.Controls.Add(btnHuy);
            panel4.Controls.Add(panelMin);
            panel4.Controls.Add(btnThem);
            panel4.Controls.Add(panelMax);
            panel4.Controls.Add(label6);
            panel4.Controls.Add(label5);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 218);
            panel4.Name = "panel4";
            panel4.Size = new Size(499, 171);
            panel4.TabIndex = 1;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.FromArgb(255, 107, 53);
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(268, 132);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(120, 33);
            btnHuy.TabIndex = 4;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += btnHuy_Click;
            // 
            // panelMin
            // 
            panelMin.Controls.Add(txtMin);
            panelMin.Location = new Point(110, 23);
            panelMin.Name = "panelMin";
            panelMin.Size = new Size(275, 40);
            panelMin.TabIndex = 7;
            // 
            // txtMin
            // 
            txtMin.BorderStyle = BorderStyle.FixedSingle;
            txtMin.Location = new Point(3, 3);
            txtMin.Name = "txtMin";
            txtMin.Size = new Size(269, 27);
            txtMin.TabIndex = 4;
            // 
            // btnThem
            // 
            btnThem.BackColor = Color.FromArgb(255, 107, 53);
            btnThem.FlatStyle = FlatStyle.Flat;
            btnThem.ForeColor = Color.White;
            btnThem.Location = new Point(110, 132);
            btnThem.Name = "btnThem";
            btnThem.Size = new Size(120, 33);
            btnThem.TabIndex = 3;
            btnThem.Text = "Sửa đổi";
            btnThem.UseVisualStyleBackColor = false;
            btnThem.Click += btnSua_Click;
            // 
            // panelMax
            // 
            panelMax.Controls.Add(txtMax);
            panelMax.Location = new Point(110, 86);
            panelMax.Name = "panelMax";
            panelMax.Size = new Size(275, 40);
            panelMax.TabIndex = 6;
            // 
            // txtMax
            // 
            txtMax.BorderStyle = BorderStyle.FixedSingle;
            txtMax.Location = new Point(3, 3);
            txtMax.Name = "txtMax";
            txtMax.Size = new Size(269, 27);
            txtMax.TabIndex = 4;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(107, 66);
            label6.Name = "label6";
            label6.Size = new Size(123, 20);
            label6.TabIndex = 5;
            label6.Text = "Giới hạn tối đa : ";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(107, 0);
            label5.Name = "label5";
            label5.Size = new Size(142, 20);
            label5.TabIndex = 3;
            label5.Text = "Giới hạn tối thiểu : ";
            // 
            // SuaThongSo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(511, 468);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "SuaThongSo";
            Text = "ThemNenMau";
            Load += SuaThongSo_Load;
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panelPhuongphap.ResumeLayout(false);
            panelPhuongphap.PerformLayout();
            panelDonvi.ResumeLayout(false);
            panelDonvi.PerformLayout();
            panelTents.ResumeLayout(false);
            panelTents.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panelMin.ResumeLayout(false);
            panelMin.PerformLayout();
            panelMax.ResumeLayout(false);
            panelMax.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel3;
        private Label label2;
        private TextBox txtTents;
        private Label label6;
        private TextBox txtMax;
        private Label label5;
        private Panel panel4;
        private Label label3;
        private Button btnHuy;
        private Button btnThem;
        private Panel panelDonvi;
        private TextBox txtDonvi;
        private Panel panelTents;
        private Panel panelMin;
        private TextBox txtMin;
        private Panel panelMax;
        private Label label4;
        private Panel panelPhuongphap;
        private TextBox txtPhuongphap;
    }
}