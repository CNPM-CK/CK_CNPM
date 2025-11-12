namespace GUI.Forms
{
    partial class NenMauNhapLieuConTrol
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
            panel1 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel4 = new Panel();
            txtVitri = new TextBox();
            textBox1 = new TextBox();
            label3 = new Label();
            txtTennenmau = new TextBox();
            label2 = new Label();
            lblNenmau = new Label();
            label1 = new Label();
            txtMota = new TextBox();
            panel2 = new Panel();
            panel3 = new Panel();
            dgvThongso = new DataGridView();
            panelMota = new Panel();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel4.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvThongso).BeginInit();
            panelMota.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.AliceBlue;
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Top;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4);
            panel1.Name = "panel1";
            panel1.Size = new Size(998, 53);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint_1;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.Controls.Add(panel4, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(998, 53);
            tableLayoutPanel1.TabIndex = 0;
            tableLayoutPanel1.Paint += tableLayoutPanel1_Paint;
            // 
            // panel4
            // 
            panel4.Controls.Add(txtVitri);
            panel4.Controls.Add(textBox1);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(txtTennenmau);
            panel4.Controls.Add(label2);
            panel4.Controls.Add(lblNenmau);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(992, 47);
            panel4.TabIndex = 5;
            // 
            // txtVitri
            // 
            txtVitri.Enabled = false;
            txtVitri.Location = new Point(418, 9);
            txtVitri.Multiline = true;
            txtVitri.Name = "txtVitri";
            txtVitri.Size = new Size(244, 31);
            txtVitri.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new Point(752, 8);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(228, 31);
            textBox1.TabIndex = 7;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new Point(674, 8);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(71, 25);
            label3.TabIndex = 8;
            label3.Text = "Tọa độ";
            // 
            // txtTennenmau
            // 
            txtTennenmau.BorderStyle = BorderStyle.FixedSingle;
            txtTennenmau.Enabled = false;
            txtTennenmau.ForeColor = Color.FromArgb(0, 152, 70);
            txtTennenmau.Location = new Point(132, 9);
            txtTennenmau.Multiline = true;
            txtTennenmau.Name = "txtTennenmau";
            txtTennenmau.Size = new Size(196, 29);
            txtTennenmau.TabIndex = 3;
            txtTennenmau.TextChanged += txtTennenmau_TextChanged;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(348, 9);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(53, 25);
            label2.TabIndex = 6;
            label2.Text = "Vị trí";
            // 
            // lblNenmau
            // 
            lblNenmau.Anchor = AnchorStyles.Left;
            lblNenmau.AutoSize = true;
            lblNenmau.Location = new Point(4, 9);
            lblNenmau.Margin = new Padding(4, 0, 4, 0);
            lblNenmau.Name = "lblNenmau";
            lblNenmau.Size = new Size(121, 25);
            lblNenmau.TabIndex = 2;
            lblNenmau.Text = "Tên nền mẫu";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(66, 20);
            label1.TabIndex = 0;
            label1.Text = "Ghi chú:";
            // 
            // txtMota
            // 
            txtMota.BackColor = Color.White;
            txtMota.BorderStyle = BorderStyle.FixedSingle;
            txtMota.Enabled = false;
            txtMota.Location = new Point(4, 13);
            txtMota.Multiline = true;
            txtMota.Name = "txtMota";
            txtMota.Size = new Size(991, 32);
            txtMota.TabIndex = 1;
            txtMota.TextChanged += textBox1_TextChanged;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(panelMota);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 53);
            panel2.Name = "panel2";
            panel2.Size = new Size(998, 375);
            panel2.TabIndex = 1;
            panel2.Paint += panel2_Paint;
            // 
            // panel3
            // 
            panel3.Controls.Add(dgvThongso);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 68);
            panel3.Name = "panel3";
            panel3.Size = new Size(998, 307);
            panel3.TabIndex = 5;
            // 
            // dgvThongso
            // 
            dgvThongso.AllowUserToAddRows = false;
            dgvThongso.AllowUserToDeleteRows = false;
            dgvThongso.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvThongso.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(0, 152, 70);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvThongso.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvThongso.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvThongso.Dock = DockStyle.Fill;
            dgvThongso.Location = new Point(0, 0);
            dgvThongso.Name = "dgvThongso";
            dgvThongso.ReadOnly = true;
            dgvThongso.RowHeadersWidth = 51;
            dgvThongso.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThongso.Size = new Size(998, 307);
            dgvThongso.TabIndex = 5;
            // 
            // panelMota
            // 
            panelMota.Controls.Add(txtMota);
            panelMota.Dock = DockStyle.Top;
            panelMota.Location = new Point(0, 20);
            panelMota.Name = "panelMota";
            panelMota.Size = new Size(998, 48);
            panelMota.TabIndex = 4;
            // 
            // NenMauNhapLieuConTrol
            // 
            AutoScaleDimensions = new SizeF(11F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(panel2);
            Controls.Add(panel1);
            Cursor = Cursors.Hand;
            Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ForeColor = Color.FromArgb(0, 152, 70);
            Margin = new Padding(4);
            Name = "NenMauNhapLieuConTrol";
            Size = new Size(998, 428);
            Load += NenMauConTrol_Load;
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvThongso).EndInit();
            panelMota.ResumeLayout(false);
            panelMota.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label1;
        private TextBox txtMota;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panelMota;
        private Panel panel3;
        private DataGridView dgvThongso;
        private Panel panel4;
        private Label lblNenmau;
        private TextBox txtTennenmau;
        private Label label2;
        private TextBox txtVitri;
        private TextBox textBox1;
        private Label label3;
    }
}
