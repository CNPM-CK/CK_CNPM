namespace GUI.Forms
{
    partial class LocTimKiemNhanVien
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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            btnApdung = new Button();
            btnHuy = new Button();
            panelPhongban = new Panel();
            cboPhongban = new ComboBox();
            panelGioitinh = new Panel();
            cboGioitinh = new ComboBox();
            panelTrangthai = new Panel();
            cboTrangthai = new ComboBox();
            panelPhongban.SuspendLayout();
            panelGioitinh.SuspendLayout();
            panelTrangthai.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 25);
            label1.Name = "label1";
            label1.Size = new Size(92, 20);
            label1.TabIndex = 0;
            label1.Text = "Phòng ban :";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(12, 70);
            label2.Name = "label2";
            label2.Size = new Size(77, 20);
            label2.TabIndex = 1;
            label2.Text = "Giới tính :";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 117);
            label3.Name = "label3";
            label3.Size = new Size(88, 20);
            label3.TabIndex = 2;
            label3.Text = "Trạng thái :";
            // 
            // btnApdung
            // 
            btnApdung.BackColor = Color.FromArgb(0, 152, 70);
            btnApdung.FlatStyle = FlatStyle.Flat;
            btnApdung.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnApdung.ForeColor = Color.White;
            btnApdung.Location = new Point(63, 172);
            btnApdung.Name = "btnApdung";
            btnApdung.Size = new Size(94, 29);
            btnApdung.TabIndex = 3;
            btnApdung.Text = "Áp dụng";
            btnApdung.UseVisualStyleBackColor = false;
            btnApdung.Click += btnApdung_Click;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.Red;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(184, 172);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(94, 29);
            btnHuy.TabIndex = 4;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += btnHuy_Click;
            // 
            // panelPhongban
            // 
            panelPhongban.Controls.Add(cboPhongban);
            panelPhongban.Location = new Point(110, 12);
            panelPhongban.Name = "panelPhongban";
            panelPhongban.Size = new Size(210, 43);
            panelPhongban.TabIndex = 5;
            // 
            // cboPhongban
            // 
            cboPhongban.DropDownStyle = ComboBoxStyle.DropDownList;
            cboPhongban.FormattingEnabled = true;
            cboPhongban.Location = new Point(3, 10);
            cboPhongban.Name = "cboPhongban";
            cboPhongban.Size = new Size(204, 28);
            cboPhongban.TabIndex = 0;
            // 
            // panelGioitinh
            // 
            panelGioitinh.Controls.Add(cboGioitinh);
            panelGioitinh.Location = new Point(110, 61);
            panelGioitinh.Name = "panelGioitinh";
            panelGioitinh.Size = new Size(210, 43);
            panelGioitinh.TabIndex = 6;
            // 
            // cboGioitinh
            // 
            cboGioitinh.DropDownStyle = ComboBoxStyle.DropDownList;
            cboGioitinh.FormattingEnabled = true;
            cboGioitinh.Location = new Point(3, 10);
            cboGioitinh.Name = "cboGioitinh";
            cboGioitinh.Size = new Size(204, 28);
            cboGioitinh.TabIndex = 0;
            // 
            // panelTrangthai
            // 
            panelTrangthai.Controls.Add(cboTrangthai);
            panelTrangthai.Location = new Point(110, 110);
            panelTrangthai.Name = "panelTrangthai";
            panelTrangthai.Size = new Size(210, 43);
            panelTrangthai.TabIndex = 7;
            // 
            // cboTrangthai
            // 
            cboTrangthai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangthai.FormattingEnabled = true;
            cboTrangthai.Location = new Point(3, 10);
            cboTrangthai.Name = "cboTrangthai";
            cboTrangthai.Size = new Size(204, 28);
            cboTrangthai.TabIndex = 0;
            // 
            // LocTimKiem
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(332, 213);
            Controls.Add(panelTrangthai);
            Controls.Add(panelGioitinh);
            Controls.Add(panelPhongban);
            Controls.Add(btnHuy);
            Controls.Add(btnApdung);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            MaximizeBox = false;
            Name = "LocTimKiem";
            Text = "Tìm kiếm";
            Load += Form1_Load;
            panelPhongban.ResumeLayout(false);
            panelGioitinh.ResumeLayout(false);
            panelTrangthai.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Button btnApdung;
        private Button btnHuy;
        private Panel panelPhongban;
        private ComboBox cboPhongban;
        private Panel panelGioitinh;
        private ComboBox cboGioitinh;
        private Panel panelTrangthai;
        private ComboBox cboTrangthai;
    }
}