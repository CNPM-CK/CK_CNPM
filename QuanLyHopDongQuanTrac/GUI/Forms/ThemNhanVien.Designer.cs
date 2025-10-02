namespace GUI.Forms
{
    partial class ThemNhanVien
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
            panel3 = new Panel();
            panelHoten = new Panel();
            textboxHoten = new TextBox();
            label2 = new Label();
            panel4 = new Panel();
            panel2 = new Panel();
            label1 = new Label();
            panel1.SuspendLayout();
            panel3.SuspendLayout();
            panelHoten.SuspendLayout();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(panel3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 740);
            panel1.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(panelHoten);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(panel4);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(0, 81);
            panel3.Name = "panel3";
            panel3.Size = new Size(800, 659);
            panel3.TabIndex = 1;
            // 
            // panelHoten
            // 
            panelHoten.Controls.Add(textboxHoten);
            panelHoten.Location = new Point(368, 34);
            panelHoten.Name = "panelHoten";
            panelHoten.Size = new Size(250, 46);
            panelHoten.TabIndex = 3;
            // 
            // textboxHoten
            // 
            textboxHoten.Location = new Point(52, 3);
            textboxHoten.Name = "textboxHoten";
            textboxHoten.Size = new Size(176, 27);
            textboxHoten.TabIndex = 4;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(383, 11);
            label2.Name = "label2";
            label2.Size = new Size(75, 20);
            label2.TabIndex = 2;
            label2.Text = "Họ và Tên";
            // 
            // panel4
            // 
            panel4.BackColor = SystemColors.ActiveCaption;
            panel4.Dock = DockStyle.Bottom;
            panel4.Location = new Point(0, 580);
            panel4.Name = "panel4";
            panel4.Size = new Size(800, 79);
            panel4.TabIndex = 1;
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
            label1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(0, 0);
            label1.Name = "label1";
            label1.Size = new Size(800, 81);
            label1.TabIndex = 0;
            label1.Text = "Thêm nhân viên mới";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // ThemNhanVien
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 740);
            Controls.Add(panel1);
            Name = "ThemNhanVien";
            Text = "Form1";
            Load += ThemNhanVien_Load;
            panel1.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panelHoten.ResumeLayout(false);
            panelHoten.PerformLayout();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private Panel panel3;
        private Panel panel4;
        private Label label2;
        private Panel panelHoten;
        private TextBox textboxHoten;
    }
}