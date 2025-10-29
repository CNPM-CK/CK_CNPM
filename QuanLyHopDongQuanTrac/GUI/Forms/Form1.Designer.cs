namespace GUI.Forms
{
    partial class Form1
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
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dgvThongso);
            groupBox1.Controls.Add(panel3);
            groupBox1.Location = new Point(-168, 109);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1137, 232);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Thông số quan trắc";
            // 
            // dgvThongso
            // 
            dgvThongso.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvThongso.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvThongso.Dock = DockStyle.Fill;
            dgvThongso.Location = new Point(3, 104);
            dgvThongso.Name = "dgvThongso";
            dgvThongso.RowHeadersWidth = 51;
            dgvThongso.Size = new Size(1131, 125);
            dgvThongso.TabIndex = 1;
            // 
            // panel3
            // 
            panel3.Controls.Add(tableLayoutPanel2);
            panel3.Dock = DockStyle.Top;
            panel3.Location = new Point(3, 23);
            panel3.Name = "panel3";
            panel3.Size = new Size(1131, 81);
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
            tableLayoutPanel2.Size = new Size(1131, 81);
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
            tableLayoutPanel1.Size = new Size(901, 75);
            tableLayoutPanel1.TabIndex = 3;
            // 
            // panel7
            // 
            panel7.Controls.Add(label2);
            panel7.Controls.Add(panelPhongpt);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(453, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(445, 69);
            panel7.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(-1, 24);
            label2.Name = "label2";
            label2.Size = new Size(132, 20);
            label2.TabIndex = 0;
            label2.Text = "Phòng thực hiện :";
            // 
            // panelPhongpt
            // 
            panelPhongpt.Anchor = AnchorStyles.None;
            panelPhongpt.Controls.Add(cboPhongpt);
            panelPhongpt.Location = new Point(305, -4);
            panelPhongpt.Name = "panelPhongpt";
            panelPhongpt.Size = new Size(212, 45);
            panelPhongpt.TabIndex = 1;
            // 
            // cboPhongpt
            // 
            cboPhongpt.FormattingEnabled = true;
            cboPhongpt.Items.AddRange(new object[] { "Phòng thí nghiệm", "Phòng hiện trường" });
            cboPhongpt.Location = new Point(3, 10);
            cboPhongpt.Name = "cboPhongpt";
            cboPhongpt.Size = new Size(206, 28);
            cboPhongpt.TabIndex = 0;
            cboPhongpt.Text = "Chọn phòng";
            // 
            // panel5
            // 
            panel5.Controls.Add(panelChonthongso);
            panel5.Controls.Add(label3);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(3, 3);
            panel5.Name = "panel5";
            panel5.Size = new Size(444, 69);
            panel5.TabIndex = 0;
            // 
            // panelChonthongso
            // 
            panelChonthongso.Anchor = AnchorStyles.None;
            panelChonthongso.Controls.Add(cboThongso);
            panelChonthongso.Location = new Point(293, -4);
            panelChonthongso.Name = "panelChonthongso";
            panelChonthongso.Size = new Size(212, 45);
            panelChonthongso.TabIndex = 0;
            // 
            // cboThongso
            // 
            cboThongso.FormattingEnabled = true;
            cboThongso.Location = new Point(3, 10);
            cboThongso.Name = "cboThongso";
            cboThongso.Size = new Size(206, 28);
            cboThongso.TabIndex = 0;
            cboThongso.Text = "Thông số";
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
            tableLayoutPanel3.Size = new Size(218, 75);
            tableLayoutPanel3.TabIndex = 4;
            // 
            // panel6
            // 
            panel6.Anchor = AnchorStyles.None;
            panel6.Controls.Add(btnThemthongso);
            panel6.Location = new Point(36, 9);
            panel6.Name = "panel6";
            panel6.Size = new Size(146, 57);
            panel6.TabIndex = 2;
            // 
            // btnThemthongso
            // 
            btnThemthongso.BackColor = Color.FromArgb(255, 107, 53);
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
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(groupBox1);
            Name = "Form1";
            Text = "Form1";
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
            ResumeLayout(false);
        }

        #endregion

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
    }
}