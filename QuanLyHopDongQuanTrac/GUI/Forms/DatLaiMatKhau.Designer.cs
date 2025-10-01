namespace GUI.Forms
{
    partial class DatLaiMatKhau
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatLaiMatKhau));
            panel1 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            panel3 = new Panel();
            textBox2 = new TextBox();
            label8 = new Label();
            textBox1 = new TextBox();
            label5 = new Label();
            label6 = new Label();
            button1 = new Button();
            label2 = new Label();
            panel4 = new Panel();
            pictureBox6 = new PictureBox();
            pictureBox5 = new PictureBox();
            label7 = new Label();
            label4 = new Label();
            label3 = new Label();
            pictureBox2 = new PictureBox();
            pictureBox3 = new PictureBox();
            pictureBox4 = new PictureBox();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.AntiqueWhite;
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1272, 724);
            panel1.TabIndex = 0;
            panel1.Paint += panel1_Paint;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(panel2, 0, 0);
            tableLayoutPanel1.Controls.Add(panel3, 0, 1);
            tableLayoutPanel1.Location = new Point(346, 29);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 18.22289F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 81.77711F));
            tableLayoutPanel1.Size = new Size(616, 664);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(125, 199, 84);
            panel2.Controls.Add(pictureBox1);
            panel2.Controls.Add(label1);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(3, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(610, 115);
            panel2.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(111, 109);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label1.Location = new Point(155, 27);
            label1.Name = "label1";
            label1.Size = new Size(335, 54);
            label1.TabIndex = 0;
            label1.Text = "Đặt lại mật khẩu";
            // 
            // panel3
            // 
            panel3.BackColor = SystemColors.Window;
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(textBox2);
            panel3.Controls.Add(label8);
            panel3.Controls.Add(textBox1);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(label6);
            panel3.Controls.Add(button1);
            panel3.Controls.Add(label2);
            panel3.Controls.Add(panel4);
            panel3.Dock = DockStyle.Fill;
            panel3.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            panel3.Location = new Point(3, 124);
            panel3.Name = "panel3";
            panel3.Size = new Size(610, 537);
            panel3.TabIndex = 1;
            // 
            // textBox2
            // 
            textBox2.BorderStyle = BorderStyle.FixedSingle;
            textBox2.Location = new Point(128, 296);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(369, 59);
            textBox2.TabIndex = 17;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = Color.Green;
            label8.Location = new Point(74, 260);
            label8.Name = "label8";
            label8.Size = new Size(211, 31);
            label8.TabIndex = 16;
            label8.Text = "Nhập lại mật khẩu";
            // 
            // textBox1
            // 
            textBox1.BorderStyle = BorderStyle.FixedSingle;
            textBox1.Location = new Point(128, 179);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(369, 59);
            textBox1.TabIndex = 15;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.ForeColor = Color.Green;
            label5.Location = new Point(74, 145);
            label5.Name = "label5";
            label5.Size = new Size(227, 31);
            label5.TabIndex = 14;
            label5.Text = "Nhập mật khẩu mới";
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.Font = new Font("Arial", 10.2F, FontStyle.Regular, GraphicsUnit.Point, 163);
            label6.ForeColor = SystemColors.WindowText;
            label6.Location = new Point(142, 485);
            label6.Name = "label6";
            label6.Size = new Size(325, 19);
            label6.TabIndex = 13;
            label6.Text = "© 2025 ECOS. Bản quyền thuộc về ECOS.";
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(125, 199, 84);
            button1.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            button1.ForeColor = Color.DarkGreen;
            button1.Location = new Point(128, 383);
            button1.Name = "button1";
            button1.Size = new Size(369, 63);
            button1.TabIndex = 11;
            button1.Text = "Hoàn tất";
            button1.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label2.ForeColor = Color.Green;
            label2.Location = new Point(155, 112);
            label2.Name = "label2";
            label2.Size = new Size(355, 25);
            label2.TabIndex = 10;
            label2.Text = "Nhập mật khẩu mới để hoàn tất quá trình";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel4
            // 
            panel4.Controls.Add(pictureBox6);
            panel4.Controls.Add(pictureBox5);
            panel4.Controls.Add(label7);
            panel4.Controls.Add(label4);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(pictureBox2);
            panel4.Controls.Add(pictureBox3);
            panel4.Controls.Add(pictureBox4);
            panel4.Location = new Point(9, 12);
            panel4.Name = "panel4";
            panel4.Size = new Size(598, 87);
            panel4.TabIndex = 0;
            // 
            // pictureBox6
            // 
            pictureBox6.Image = (Image)resources.GetObject("pictureBox6.Image");
            pictureBox6.Location = new Point(352, 0);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(70, 87);
            pictureBox6.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox6.TabIndex = 19;
            pictureBox6.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(182, 0);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(70, 87);
            pictureBox5.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox5.TabIndex = 18;
            pictureBox5.TabStop = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.BackColor = Color.FromArgb(125, 199, 84);
            label7.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label7.ForeColor = SystemColors.Window;
            label7.Location = new Point(448, 19);
            label7.Name = "label7";
            label7.Size = new Size(40, 46);
            label7.TabIndex = 17;
            label7.Text = "3";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.BackColor = Color.FromArgb(125, 199, 84);
            label4.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label4.ForeColor = SystemColors.Window;
            label4.Location = new Point(282, 18);
            label4.Name = "label4";
            label4.Size = new Size(40, 46);
            label4.TabIndex = 15;
            label4.Text = "2";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(125, 199, 84);
            label3.Font = new Font("Segoe UI", 19.8000011F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label3.ForeColor = SystemColors.Window;
            label3.Location = new Point(111, 18);
            label3.Name = "label3";
            label3.Size = new Size(40, 46);
            label3.TabIndex = 14;
            label3.Text = "1";
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(80, 0);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(103, 87);
            pictureBox2.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox2.TabIndex = 0;
            pictureBox2.TabStop = false;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(250, 0);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(103, 87);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 20;
            pictureBox3.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(418, 0);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(103, 87);
            pictureBox4.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox4.TabIndex = 21;
            pictureBox4.TabStop = false;
            // 
            // DatLaiMatKhau1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1272, 724);
            Controls.Add(panel1);
            Name = "DatLaiMatKhau1";
            Text = "Form1";
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private PictureBox pictureBox1;
        private Label label1;
        private Panel panel3;
        private TextBox textBox1;
        private Label label5;
        private Label label6;
        private Button button1;
        private Label label2;
        private Panel panel4;
        private PictureBox pictureBox6;
        private PictureBox pictureBox5;
        private Label label7;
        private Label label4;
        private Label label3;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private TextBox textBox2;
        private Label label8;
    }
}
