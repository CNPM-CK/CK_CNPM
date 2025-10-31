namespace GUI.Forms
{
    partial class ThemHopDong
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
            btnCancel = new Button();
            buttonAddnew = new Button();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label8 = new Label();
            cbbTanSuatQT = new ComboBox();
            textBox1 = new TextBox();
            dateTimePicker2 = new DateTimePicker();
            dateTimePicker1 = new DateTimePicker();
            cbbKhachHang = new ComboBox();
            panel2 = new Panel();
            label1 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(btnCancel);
            panel1.Controls.Add(buttonAddnew);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(cbbTanSuatQT);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(dateTimePicker2);
            panel1.Controls.Add(dateTimePicker1);
            panel1.Controls.Add(cbbKhachHang);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 632);
            panel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnCancel.BackColor = SystemColors.GradientActiveCaption;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.Location = new Point(406, 565);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(382, 55);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "Làm mới";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // buttonAddnew
            // 
            buttonAddnew.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            buttonAddnew.BackColor = Color.FromArgb(255, 107, 53);
            buttonAddnew.BackgroundImageLayout = ImageLayout.Zoom;
            buttonAddnew.FlatAppearance.BorderSize = 0;
            buttonAddnew.FlatStyle = FlatStyle.Flat;
            buttonAddnew.Font = new Font("Segoe UI", 10F);
            buttonAddnew.ForeColor = Color.White;
            buttonAddnew.Location = new Point(12, 566);
            buttonAddnew.Name = "buttonAddnew";
            buttonAddnew.Size = new Size(370, 54);
            buttonAddnew.TabIndex = 16;
            buttonAddnew.Text = "Thêm nhân viên";
            buttonAddnew.UseVisualStyleBackColor = false;
            buttonAddnew.Click += buttonAddnew_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(473, 259);
            label5.Name = "label5";
            label5.Size = new Size(158, 23);
            label5.TabIndex = 14;
            label5.Text = "Tần suất quan trắc";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(473, 124);
            label4.Name = "label4";
            label4.Size = new Size(114, 23);
            label4.TabIndex = 13;
            label4.Text = "Số hợp đồng";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(69, 124);
            label3.Name = "label3";
            label3.Size = new Size(103, 23);
            label3.TabIndex = 11;
            label3.Text = "Khách hàng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(69, 401);
            label2.Name = "label2";
            label2.Size = new Size(207, 23);
            label2.TabIndex = 9;
            label2.Text = "Ngày kết thúc hợp đồng";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label8.Location = new Point(69, 260);
            label8.Name = "label8";
            label8.Size = new Size(160, 23);
            label8.TabIndex = 7;
            label8.Text = "Ngày ký hợp đồng";
            // 
            // cbbTanSuatQT
            // 
            cbbTanSuatQT.FormattingEnabled = true;
            cbbTanSuatQT.Location = new Point(473, 285);
            cbbTanSuatQT.Name = "cbbTanSuatQT";
            cbbTanSuatQT.Size = new Size(277, 28);
            cbbTanSuatQT.TabIndex = 6;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(473, 151);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Nhập số hợp đồng...";
            textBox1.Size = new Size(277, 27);
            textBox1.TabIndex = 5;
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Location = new Point(69, 427);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(277, 27);
            dateTimePicker2.TabIndex = 4;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(69, 286);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(277, 27);
            dateTimePicker1.TabIndex = 3;
            // 
            // cbbKhachHang
            // 
            cbbKhachHang.FormattingEnabled = true;
            cbbKhachHang.Location = new Point(69, 150);
            cbbKhachHang.Name = "cbbKhachHang";
            cbbKhachHang.Size = new Size(277, 28);
            cbbKhachHang.TabIndex = 1;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(0, 152, 70);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(800, 81);
            panel2.TabIndex = 0;
            // 
            // label1
            // 
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.White;
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(800, 81);
            label1.TabIndex = 0;
            label1.Text = "THÊM HỢP ĐỒNG MỚI";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Click += label1_Click;
            // 
            // ThemHopDong
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 632);
            Controls.Add(panel1);
            MaximizeBox = false;
            Name = "ThemHopDong";
            Text = "Thêm hợp đồng";
            Load += ThemNhanVien_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private TextBox textBox1;
        private DateTimePicker dateTimePicker2;
        private DateTimePicker dateTimePicker1;
        private ComboBox cbbKhachHang;
        private ComboBox cbbTanSuatQT;
        private Label label3;
        private Label label2;
        private Label label8;
        private Label label5;
        private Label label4;
        private Button buttonAddnew;
        private Button btnCancel;
    }
}