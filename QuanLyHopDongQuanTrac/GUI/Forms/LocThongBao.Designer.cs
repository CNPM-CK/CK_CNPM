namespace GUI.Forms
{
    partial class LocThongBao
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
            btnHuy = new Button();
            btnApdung = new Button();
            label3 = new Label();
            panelTrangthai.SuspendLayout();
            SuspendLayout();
            // 
            // panelTrangthai
            // 
            panelTrangthai.Controls.Add(cboTrangthai);
            panelTrangthai.Location = new Point(114, 8);
            panelTrangthai.Name = "panelTrangthai";
            panelTrangthai.Size = new Size(210, 43);
            panelTrangthai.TabIndex = 15;
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
            // btnHuy
            // 
            btnHuy.BackColor = Color.Red;
            btnHuy.FlatStyle = FlatStyle.Flat;
            btnHuy.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            btnHuy.ForeColor = Color.White;
            btnHuy.Location = new Point(188, 70);
            btnHuy.Name = "btnHuy";
            btnHuy.Size = new Size(94, 29);
            btnHuy.TabIndex = 14;
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
            btnApdung.Location = new Point(67, 70);
            btnApdung.Name = "btnApdung";
            btnApdung.Size = new Size(94, 29);
            btnApdung.TabIndex = 13;
            btnApdung.Text = "Áp dụng";
            btnApdung.UseVisualStyleBackColor = false;
            btnApdung.Click += btnApdung_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(16, 15);
            label3.Name = "label3";
            label3.Size = new Size(88, 20);
            label3.TabIndex = 12;
            label3.Text = "Trạng thái :";
            // 
            // LocThongBao
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(340, 107);
            Controls.Add(panelTrangthai);
            Controls.Add(btnHuy);
            Controls.Add(btnApdung);
            Controls.Add(label3);
            Name = "LocThongBao";
            Text = "LocThongBao";
            Load += LocThongBao_Load;
            panelTrangthai.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panelTrangthai;
        private ComboBox cboTrangthai;
        private Button btnHuy;
        private Button btnApdung;
        private Label label3;
    }
}