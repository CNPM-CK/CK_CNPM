namespace GUI.Forms
{
    partial class ThemNenMau1
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
            label = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel4 = new Panel();
            btnHuy = new Button();
            btnThemm = new Button();
            panel3 = new Panel();
            panelMota = new Panel();
            txtMota = new TextBox();
            panelNenmau = new Panel();
            txtTennenmau = new TextBox();
            label3 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panelMota.SuspendLayout();
            panelNenmau.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(511, 301);
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
            tableLayoutPanel1.Size = new Size(511, 301);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(0, 152, 70);
            panel2.Controls.Add(label);
            panel2.Dock = DockStyle.Fill;
            panel2.ForeColor = Color.White;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(505, 39);
            panel2.TabIndex = 0;
            // 
            // label
            // 
            label.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            label.AutoSize = true;
            label.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label.Location = new Point(158, 6);
            label.Name = "label";
            label.Size = new Size(173, 31);
            label.TabIndex = 0;
            label.Text = "Thêm nền mẫu";
            label.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(panel4, 0, 1);
            tableLayoutPanel2.Controls.Add(panel3, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 48);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.Size = new Size(505, 250);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.Controls.Add(btnHuy);
            panel4.Controls.Add(btnThemm);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 203);
            panel4.Name = "panel4";
            panel4.Size = new Size(499, 44);
            panel4.TabIndex = 2;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.Red;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(283, 5);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(99, 33);
            btnHuy.TabIndex = 6;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += button1_Click_1;
            // 
            // btnThemm
            // 
            btnThemm.BackColor = Color.FromArgb(0, 152, 70);
            btnThemm.FlatStyle = FlatStyle.Flat;
            btnThemm.ForeColor = Color.White;
            btnThemm.Location = new Point(113, 5);
            btnThemm.Name = "btnThemm";
            btnThemm.Size = new Size(99, 33);
            btnThemm.TabIndex = 5;
            btnThemm.Text = "Thêm";
            btnThemm.UseVisualStyleBackColor = false;
            btnThemm.Click += button1_Click;
            // 
            // panel3
            // 
            panel3.Controls.Add(panelMota);
            panel3.Controls.Add(panelNenmau);
            panel3.Controls.Add(label3);
            panel3.Controls.Add(label2);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(3, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(499, 194);
            panel3.TabIndex = 0;
            // 
            // panelMota
            // 
            panelMota.Controls.Add(txtMota);
            panelMota.Location = new Point(110, 101);
            panelMota.Name = "panelMota";
            panelMota.Size = new Size(275, 40);
            panelMota.TabIndex = 3;
            // 
            // txtMota
            // 
            txtMota.BorderStyle = BorderStyle.FixedSingle;
            txtMota.Location = new Point(3, 3);
            txtMota.Name = "txtMota";
            txtMota.Size = new Size(269, 27);
            txtMota.TabIndex = 2;
            // 
            // panelNenmau
            // 
            panelNenmau.Controls.Add(txtTennenmau);
            panelNenmau.Location = new Point(110, 33);
            panelNenmau.Name = "panelNenmau";
            panelNenmau.Size = new Size(275, 40);
            panelNenmau.TabIndex = 2;
            // 
            // txtTennenmau
            // 
            txtTennenmau.BorderStyle = BorderStyle.FixedSingle;
            txtTennenmau.Location = new Point(3, 3);
            txtTennenmau.Name = "txtTennenmau";
            txtTennenmau.Size = new Size(269, 27);
            txtTennenmau.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(107, 78);
            label3.Name = "label3";
            label3.Size = new Size(50, 20);
            label3.TabIndex = 1;
            label3.Text = "Mô tả";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(107, 10);
            label2.Name = "label2";
            label2.Size = new Size(99, 20);
            label2.TabIndex = 0;
            label2.Text = "Tên nền mẫu";
            // 
            // ThemNenMau1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(511, 301);
            Controls.Add(panel1);
            Name = "ThemNenMau1";
            Text = "ThemNenMau";
            Load += ThemNenMau1_Load;
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panelMota.ResumeLayout(false);
            panelMota.PerformLayout();
            panelNenmau.ResumeLayout(false);
            panelNenmau.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private Label label;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel3;
        private Label label2;
        private TextBox txtTennenmau;
        private Label label3;
        private Panel panelMota;
        private TextBox txtMota;
        private Panel panelNenmau;
        private Panel panel4;
        private Button btnThemm;
        private Button btnHuy;
    }
}