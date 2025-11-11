namespace GUI.Forms
{
    partial class SuaHopDongForm
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
            dateTimePicker1 = new DateTimePicker();
            label6 = new Label();
            dateTimePicker2 = new DateTimePicker();
            label5 = new Label();
            cbbTrangThai = new ComboBox();
            label4 = new Label();
            cbbTanSuatQT = new ComboBox();
            textBox1 = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label7 = new Label();
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
            panel1.Controls.Add(dateTimePicker1);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(dateTimePicker2);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(cbbTrangThai);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(cbbTanSuatQT);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(cbbKhachHang);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 545);
            panel1.TabIndex = 0;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            btnCancel.BackColor = SystemColors.GradientActiveCaption;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 10F);
            btnCancel.Location = new Point(427, 467);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(334, 55);
            btnCancel.TabIndex = 17;
            btnCancel.Text = "Hủy";
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
            buttonAddnew.Location = new Point(35, 468);
            buttonAddnew.Name = "buttonAddnew";
            buttonAddnew.Size = new Size(326, 54);
            buttonAddnew.TabIndex = 16;
            buttonAddnew.Text = "Xác nhận ";
            buttonAddnew.UseVisualStyleBackColor = false;
            buttonAddnew.Click += buttonAddnew_Click;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(46, 368);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(301, 27);
            dateTimePicker1.TabIndex = 15;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(46, 335);
            label6.Name = "label6";
            label6.Size = new Size(207, 23);
            label6.TabIndex = 14;
            label6.Text = "Ngày kết thúc hợp đồng";
            // 
            // dateTimePicker2
            // 
            dateTimePicker2.Location = new Point(46, 248);
            dateTimePicker2.Name = "dateTimePicker2";
            dateTimePicker2.Size = new Size(301, 27);
            dateTimePicker2.TabIndex = 13;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(460, 335);
            label5.Name = "label5";
            label5.Size = new Size(92, 23);
            label5.TabIndex = 11;
            label5.Text = "Trạng thái";
            // 
            // cbbTrangThai
            // 
            cbbTrangThai.FormattingEnabled = true;
            cbbTrangThai.Location = new Point(460, 370);
            cbbTrangThai.Name = "cbbTrangThai";
            cbbTrangThai.Size = new Size(301, 28);
            cbbTrangThai.TabIndex = 10;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(460, 215);
            label4.Name = "label4";
            label4.Size = new Size(77, 23);
            label4.TabIndex = 9;
            label4.Text = "Tần suất";
            // 
            // cbbTanSuatQT
            // 
            cbbTanSuatQT.FormattingEnabled = true;
            cbbTanSuatQT.Location = new Point(460, 250);
            cbbTanSuatQT.Name = "cbbTanSuatQT";
            cbbTanSuatQT.Size = new Size(301, 28);
            cbbTanSuatQT.TabIndex = 8;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(460, 142);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(301, 27);
            textBox1.TabIndex = 7;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(460, 106);
            label3.Name = "label3";
            label3.Size = new Size(114, 23);
            label3.TabIndex = 6;
            label3.Text = "Số hợp đồng";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(46, 215);
            label2.Name = "label2";
            label2.Size = new Size(76, 23);
            label2.TabIndex = 4;
            label2.Text = "Ngày ký";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(46, 106);
            label7.Name = "label7";
            label7.Size = new Size(103, 23);
            label7.TabIndex = 2;
            label7.Text = "Khách hàng";
            // 
            // cbbKhachHang
            // 
            cbbKhachHang.FormattingEnabled = true;
            cbbKhachHang.Location = new Point(46, 142);
            cbbKhachHang.Name = "cbbKhachHang";
            cbbKhachHang.Size = new Size(301, 28);
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
            label1.Text = "SỬA HỢP ĐỒNG";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // SuaHopDong
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 545);
            Controls.Add(panel1);
            MaximizeBox = false;
            Name = "SuaHopDong";
            Text = "Form1";
            Load += SuaHopDong_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label label7;
        private ComboBox cbbKhachHang;
        private Panel panel2;
        private Label label1;
        private Label label4;
        private ComboBox cbbTanSuatQT;
        private TextBox textBox1;
        private Label label3;
        private Label label2;
        private Label label5;
        private ComboBox cbbTrangThai;
        private DateTimePicker dateTimePicker1;
        private Label label6;
        private DateTimePicker dateTimePicker2;
        private Button buttonAddnew;
        private Button btnCancel;
    }
}