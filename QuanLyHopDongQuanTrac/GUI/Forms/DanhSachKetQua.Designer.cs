namespace GUI.Forms
{
    partial class DanhSachKetQua
    {
        private System.ComponentModel.IContainer components = null;

  
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
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            panel1 = new Panel();
            panel2 = new Panel();
            panel4 = new Panel();
            panel5 = new Panel();
            panel7 = new Panel();
            dgvDanhsachketqua = new DataGridView();
            dataGridView1 = new DataGridView();
            panel6 = new Panel();
            sidebar = new Panel();
            button2 = new Button();
            panel3 = new Panel();
            label1 = new Label();
            labelFooter = new Label();
            panel8 = new Panel();
            pictureBox1 = new PictureBox();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhsachketqua).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            sidebar.SuspendLayout();
            panel3.SuspendLayout();
            panel8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 450);
            panel1.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.Controls.Add(panel4);
            panel2.Controls.Add(panel3);
            panel2.Controls.Add(panel8);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 450);
            panel2.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.BackColor = SystemColors.Control;
            panel4.Controls.Add(panel5);
            panel4.Controls.Add(sidebar);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(0, 40);
            panel4.Name = "panel4";
            panel4.Size = new Size(800, 381);
            panel4.TabIndex = 2;
            // 
            // panel5
            // 
            panel5.Controls.Add(panel7);
            panel5.Controls.Add(panel6);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(160, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(640, 381);
            panel5.TabIndex = 1;
            // 
            // panel7
            // 
            panel7.Controls.Add(dgvDanhsachketqua);
            panel7.Controls.Add(dataGridView1);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(0, 61);
            panel7.Name = "panel7";
            panel7.Size = new Size(640, 320);
            panel7.TabIndex = 1;
            // 
            // dgvDanhsachketqua
            // 
            dgvDanhsachketqua.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvDanhsachketqua.BackgroundColor = Color.White;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(0, 152, 70);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 10F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvDanhsachketqua.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvDanhsachketqua.ColumnHeadersHeight = 30;
            dgvDanhsachketqua.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Window;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(111, 207, 151);
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.False;
            dgvDanhsachketqua.DefaultCellStyle = dataGridViewCellStyle4;
            dgvDanhsachketqua.Dock = DockStyle.Fill;
            dgvDanhsachketqua.EnableHeadersVisualStyles = false;
            dgvDanhsachketqua.Location = new Point(0, 0);
            dgvDanhsachketqua.Name = "dgvDanhsachketqua";
            dgvDanhsachketqua.RowHeadersWidth = 51;
            dgvDanhsachketqua.Size = new Size(640, 294);
            dgvDanhsachketqua.TabIndex = 1;
            dgvDanhsachketqua.CellContentClick += dgvDanhsachketqua_CellContentClick;
            dgvDanhsachketqua.Paint += dgvDanhsachketqua_Paint;
            // 
            // dataGridView1
            // 
            dataGridView1.BackgroundColor = SystemColors.MenuHighlight;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Bottom;
            dataGridView1.Location = new Point(0, 294);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(640, 26);
            dataGridView1.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.BackColor = Color.White;
            panel6.Dock = DockStyle.Top;
            panel6.Font = new Font("Segoe UI", 10F);
            panel6.Location = new Point(0, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(640, 61);
            panel6.TabIndex = 0;
            panel6.Paint += panel6_Paint;
            // 
            // sidebar
            // 
            sidebar.BackColor = Color.FromArgb(224, 234, 230);
            sidebar.Controls.Add(button2);
            sidebar.Dock = DockStyle.Left;
            sidebar.Location = new Point(0, 0);
            sidebar.Name = "sidebar";
            sidebar.Size = new Size(160, 381);
            sidebar.TabIndex = 0;
            // 
            // button2
            // 
            button2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button2.Location = new Point(0, 105);
            button2.Name = "button2";
            button2.Size = new Size(120, 325);
            button2.TabIndex = 0;
            button2.Text = "button2";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.White;
            panel3.Controls.Add(label1);
            panel3.Controls.Add(labelFooter);
            panel3.Dock = DockStyle.Bottom;
            panel3.Location = new Point(0, 421);
            panel3.Name = "panel3";
            panel3.Size = new Size(800, 29);
            panel3.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom;
            label1.AutoSize = true;
            label1.Location = new Point(273, 3);
            label1.Name = "label1";
            label1.Size = new Size(277, 20);
            label1.TabIndex = 1;
            label1.Text = "© 2025 ECOS. Bản quyền thuộc về ECOS.";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // labelFooter
            // 
            labelFooter.Anchor = AnchorStyles.None;
            labelFooter.AutoSize = true;
            labelFooter.Location = new Point(980, -69);
            labelFooter.Name = "labelFooter";
            labelFooter.Size = new Size(277, 20);
            labelFooter.TabIndex = 0;
            labelFooter.Text = "© 2025 ECOS. Bản quyền thuộc về ECOS.";
            labelFooter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel8
            // 
            panel8.BackColor = Color.FromArgb(0, 152, 70);
            panel8.Controls.Add(pictureBox1);
            panel8.Dock = DockStyle.Top;
            panel8.Location = new Point(0, 0);
            panel8.Name = "panel8";
            panel8.Size = new Size(800, 40);
            panel8.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.remove_background_logo;
            pictureBox1.Location = new Point(-10, 0);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(164, 40);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // DanhSachKetQua
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(panel1);
            Name = "DanhSachKetQua";
            Text = "Danh Sách Kết Quả";
            WindowState = FormWindowState.Maximized;
            Load += Form2_Load;
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel5.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDanhsachketqua).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            sidebar.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Panel panel4;
        private Panel panel5;
        private Panel panel7;
        private DataGridView dgvDanhsachketqua;
        private DataGridView dataGridView1;
        private Panel panel6;
        private Panel sidebar;
        private Button button2;
        private Panel panel3;
        private Label label1;
        private Label labelFooter;
        private Panel panel8;
        private PictureBox pictureBox1;

        private void Form2_Load(object sender, EventArgs e)
        {
            // You can leave this empty or add initialization code here
        }
    }
}