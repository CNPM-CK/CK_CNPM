namespace GUI.Forms
{
    partial class NenMauConTrol
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
            btnSua = new Button();
            btnXoa = new Button();
            panel4 = new Panel();
            label2 = new Label();
            panelVitri = new Panel();
            txtVitri = new TextBox();
            panelTennenmau = new Panel();
            txtTennenmau = new TextBox();
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
            panelVitri.SuspendLayout();
            panelTennenmau.SuspendLayout();
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
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 23F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 7F));
            tableLayoutPanel1.Controls.Add(btnSua, 1, 0);
            tableLayoutPanel1.Controls.Add(btnXoa, 2, 0);
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
            // btnSua
            // 
            btnSua.Anchor = AnchorStyles.Right;
            btnSua.BackColor = Color.FromArgb(0, 152, 70);
            btnSua.FlatStyle = FlatStyle.Flat;
            btnSua.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnSua.ForeColor = Color.White;
            btnSua.Location = new Point(864, 11);
            btnSua.Name = "btnSua";
            btnSua.Size = new Size(60, 30);
            btnSua.TabIndex = 4;
            btnSua.Text = "Sửa";
            btnSua.UseVisualStyleBackColor = false;
            btnSua.Click += btnSua_Click;
            // 
            // btnXoa
            // 
            btnXoa.Anchor = AnchorStyles.Right;
            btnXoa.BackColor = Color.FromArgb(220, 53, 69);
            btnXoa.FlatStyle = FlatStyle.Flat;
            btnXoa.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnXoa.ForeColor = Color.White;
            btnXoa.Location = new Point(935, 11);
            btnXoa.Name = "btnXoa";
            btnXoa.Size = new Size(60, 30);
            btnXoa.TabIndex = 2;
            btnXoa.Text = "Xóa";
            btnXoa.UseVisualStyleBackColor = false;
            btnXoa.Click += btnXoa_Click;
            // 
            // panel4
            // 
            panel4.Controls.Add(label2);
            panel4.Controls.Add(panelVitri);
            panel4.Controls.Add(panelTennenmau);
            panel4.Controls.Add(lblNenmau);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(692, 47);
            panel4.TabIndex = 5;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new Point(358, 9);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(63, 25);
            label2.TabIndex = 6;
            label2.Text = "Vị trí :";
            // 
            // panelVitri
            // 
            panelVitri.Controls.Add(txtVitri);
            panelVitri.Location = new Point(439, 3);
            panelVitri.Name = "panelVitri";
            panelVitri.Size = new Size(250, 39);
            panelVitri.TabIndex = 5;
            // 
            // txtVitri
            // 
            txtVitri.Enabled = false;
            txtVitri.Location = new Point(3, 5);
            txtVitri.Multiline = true;
            txtVitri.Name = "txtVitri";
            txtVitri.Size = new Size(244, 31);
            txtVitri.TabIndex = 0;
            // 
            // panelTennenmau
            // 
            panelTennenmau.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            panelTennenmau.Controls.Add(txtTennenmau);
            panelTennenmau.Location = new Point(132, 3);
            panelTennenmau.Name = "panelTennenmau";
            panelTennenmau.Size = new Size(202, 38);
            panelTennenmau.TabIndex = 4;
            // 
            // txtTennenmau
            // 
            txtTennenmau.BorderStyle = BorderStyle.FixedSingle;
            txtTennenmau.Enabled = false;
            txtTennenmau.ForeColor = Color.FromArgb(0, 152, 70);
            txtTennenmau.Location = new Point(3, 6);
            txtTennenmau.Multiline = true;
            txtTennenmau.Name = "txtTennenmau";
            txtTennenmau.Size = new Size(196, 29);
            txtTennenmau.TabIndex = 3;
            txtTennenmau.TextChanged += txtTennenmau_TextChanged;
            // 
            // lblNenmau
            // 
            lblNenmau.Anchor = AnchorStyles.Left;
            lblNenmau.AutoSize = true;
            lblNenmau.Location = new Point(4, 9);
            lblNenmau.Margin = new Padding(4, 0, 4, 0);
            lblNenmau.Name = "lblNenmau";
            lblNenmau.Size = new Size(125, 25);
            lblNenmau.TabIndex = 2;
            lblNenmau.Text = "▼ Nền mẫu 1";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Top;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(110, 20);
            label1.TabIndex = 0;
            label1.Text = "Mô tả chi tiết :";
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
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvThongso.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvThongso.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvThongso.Dock = DockStyle.Fill;
            dgvThongso.Enabled = false;
            dgvThongso.Location = new Point(0, 0);
            dgvThongso.Name = "dgvThongso";
            dgvThongso.ReadOnly = true;
            dgvThongso.RowHeadersWidth = 51;
            dgvThongso.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvThongso.Size = new Size(998, 307);
            dgvThongso.TabIndex = 5;
            dgvThongso.CellContentClick += dgvThongso_CellContentClick;
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
            // NenMauConTrol
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
            Name = "NenMauConTrol";
            Size = new Size(998, 428);
            Load += NenMauConTrol_Load;
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panelVitri.ResumeLayout(false);
            panelVitri.PerformLayout();
            panelTennenmau.ResumeLayout(false);
            panelTennenmau.PerformLayout();
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
        private Button btnXoa;
        private Label label1;
        private TextBox txtMota;
        private Button btnSua;
        private Panel panel2;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panelMota;
        private Panel panel3;
        private DataGridView dgvThongso;
        private Panel panel4;
        private Label lblNenmau;
        private TextBox txtTennenmau;
        private Panel panelTennenmau;
        private Panel panelVitri;
        private TextBox txtVitri;
        private Label label2;
    }
}
