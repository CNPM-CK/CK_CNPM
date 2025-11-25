namespace GUI.Forms
{
    partial class LocHopDongvaDQT
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
            panelTrangthai = new Panel();
            cboTrangthai = new ComboBox();
            panelBatdau = new Panel();
            dtmBatdau = new DateTimePicker();
            btnHuy = new Button();
            btnApdung = new Button();
            label3 = new Label();
            labelKetthuc = new Label();
            labelBatdau = new Label();
            panelKetthuc = new Panel();
            dtmKetthuc = new DateTimePicker();
            panelTrangthai.SuspendLayout();
            panelBatdau.SuspendLayout();
            panelKetthuc.SuspendLayout();
            SuspendLayout();
            // 
            // panelTrangthai
            // 
            panelTrangthai.Controls.Add(cboTrangthai);
            panelTrangthai.Location = new Point(106, 110);
            panelTrangthai.Name = "panelTrangthai";
            panelTrangthai.Size = new Size(258, 43);
            panelTrangthai.TabIndex = 15;
            // 
            // cboTrangthai
            // 
            cboTrangthai.DropDownStyle = ComboBoxStyle.DropDownList;
            cboTrangthai.FormattingEnabled = true;
            cboTrangthai.Location = new Point(3, 10);
            cboTrangthai.Name = "cboTrangthai";
            cboTrangthai.Size = new Size(249, 28);
            cboTrangthai.TabIndex = 0;
            // 
            // panelBatdau
            // 
            panelBatdau.Controls.Add(dtmBatdau);
            panelBatdau.Location = new Point(106, 12);
            panelBatdau.Name = "panelBatdau";
            panelBatdau.Size = new Size(258, 43);
            panelBatdau.TabIndex = 13;
            // 
            // dtmBatdau
            // 
            dtmBatdau.CustomFormat = "\"dd/MM/yyyy\"";
            dtmBatdau.Format = DateTimePickerFormat.Custom;
            dtmBatdau.Location = new Point(3, 8);
            dtmBatdau.Name = "dtmBatdau";
            dtmBatdau.ShowCheckBox = true;
            dtmBatdau.Size = new Size(249, 27);
            dtmBatdau.TabIndex = 0;
            // 
            // btnHuy
            // 
            btnHuy.BackColor = Color.Red;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(217, 172);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(94, 29);
            btnHuy.TabIndex = 12;
            btnHuy.Text = "Hủy";
            btnHuy.UseVisualStyleBackColor = false;
            btnHuy.Click += btnHuy_Click;
            // 
            // btnApdung
            // 
            btnApdung.BackColor = Color.FromArgb(0, 152, 70);
            btnApdung.FlatStyle = FlatStyle.Flat;
            btnApdung.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnApdung.ForeColor = Color.White;
            btnApdung.Location = new Point(106, 172);
            btnApdung.Name = "btnApdung";
            btnApdung.Size = new Size(94, 29);
            btnApdung.TabIndex = 11;
            btnApdung.Text = "Áp dụng";
            btnApdung.UseVisualStyleBackColor = false;
            btnApdung.Click += btnApdung_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(12, 117);
            label3.Name = "label3";
            label3.Size = new Size(88, 20);
            label3.TabIndex = 10;
            label3.Text = "Trạng thái :";
            // 
            // labelKetthuc
            // 
            labelKetthuc.AutoSize = true;
            labelKetthuc.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelKetthuc.Location = new Point(12, 70);
            labelKetthuc.Name = "labelKetthuc";
            labelKetthuc.Size = new Size(79, 20);
            labelKetthuc.TabIndex = 9;
            labelKetthuc.Text = "Đến ngày:";
            // 
            // labelBatdau
            // 
            labelBatdau.AutoSize = true;
            labelBatdau.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            labelBatdau.Location = new Point(12, 25);
            labelBatdau.Name = "labelBatdau";
            labelBatdau.Size = new Size(74, 20);
            labelBatdau.TabIndex = 8;
            labelBatdau.Text = "Từ ngày :";
            // 
            // panelKetthuc
            // 
            panelKetthuc.Controls.Add(dtmKetthuc);
            panelKetthuc.Location = new Point(106, 61);
            panelKetthuc.Name = "panelKetthuc";
            panelKetthuc.Size = new Size(258, 43);
            panelKetthuc.TabIndex = 16;
            // 
            // dtmKetthuc
            // 
            dtmKetthuc.CustomFormat = "\"dd/MM/yyyy\"";
            dtmKetthuc.Format = DateTimePickerFormat.Custom;
            dtmKetthuc.Location = new Point(3, 8);
            dtmKetthuc.Name = "dtmKetthuc";
            dtmKetthuc.ShowCheckBox = true;
            dtmKetthuc.Size = new Size(249, 27);
            dtmKetthuc.TabIndex = 0;
            // 
            // LocHopDongvaDQT
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(376, 213);
            Controls.Add(panelKetthuc);
            Controls.Add(panelTrangthai);
            Controls.Add(panelBatdau);
            Controls.Add(btnHuy);
            Controls.Add(btnApdung);
            Controls.Add(label3);
            Controls.Add(labelKetthuc);
            Controls.Add(labelBatdau);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Name = "LocHopDongvaDQT";
            Text = "LocHopDongvaDQT";
            Load += LocHopDongvaDQT_Load;
            panelTrangthai.ResumeLayout(false);
            panelBatdau.ResumeLayout(false);
            panelKetthuc.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelTrangthai;
        private ComboBox cboTrangthai;
        private Panel panelBatdau;
        private DateTimePicker dtmBatdau;
        private Button btnHuy;
        private Button btnApdung;
        private Label label3;
        private Label labelKetthuc;
        private Label labelBatdau;
        private Panel panelKetthuc;
        private DateTimePicker dtmKetthuc;
    }
}