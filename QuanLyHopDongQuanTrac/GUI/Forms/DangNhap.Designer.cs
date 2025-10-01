namespace GUI.Forms
{
    partial class DangNhap
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DangNhap));
            panel1 = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panel2 = new Panel();
            tableLayoutPanel3 = new TableLayoutPanel();
            panel4 = new Panel();
            label8 = new Label();
            label7 = new Label();
            checkBox1 = new CheckBox();
            label6 = new Label();
            label5 = new Label();
            button2 = new Button();
            button1 = new Button();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            panel3 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            pictureBox1 = new PictureBox();
            label1 = new Label();
            panel1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panel2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.AntiqueWhite;
            panel1.Controls.Add(tableLayoutPanel1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(1255, 680);
            panel1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.Transparent;
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 18.0904522F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 81.9095459F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 15F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 515F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 92F));
            tableLayoutPanel1.Controls.Add(panel2, 3, 1);
            tableLayoutPanel1.Controls.Add(panel3, 1, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 9.25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 90.75F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 54F));
            tableLayoutPanel1.Size = new Size(1255, 680);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            panel2.BackColor = SystemColors.Window;
            panel2.Controls.Add(tableLayoutPanel3);
            panel2.Dock = DockStyle.Fill;
            panel2.ForeColor = SystemColors.Window;
            panel2.Location = new Point(650, 60);
            panel2.Name = "panel2";
            panel2.Size = new Size(509, 562);
            panel2.TabIndex = 0;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 38.2417564F));
            tableLayoutPanel3.Controls.Add(panel4, 0, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(509, 562);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(label8);
            panel4.Controls.Add(label7);
            panel4.Controls.Add(checkBox1);
            panel4.Controls.Add(label6);
            panel4.Controls.Add(label5);
            panel4.Controls.Add(button2);
            panel4.Controls.Add(button1);
            panel4.Controls.Add(textBox2);
            panel4.Controls.Add(textBox1);
            panel4.Controls.Add(label4);
            panel4.Controls.Add(label3);
            panel4.Controls.Add(label2);
            panel4.ForeColor = Color.Green;
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(503, 556);
            panel4.TabIndex = 0;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label8.ForeColor = Color.Green;
            label8.Location = new Point(109, 92);
            label8.Name = "label8";
            label8.Size = new Size(292, 23);
            label8.TabIndex = 11;
            label8.Text = "Truy cập vào hệ thống quản lý ECOS";
            label8.Click += label8_Click;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label7.ForeColor = Color.DarkGreen;
            label7.Location = new Point(159, 108);
            label7.Name = "label7";
            label7.Size = new Size(0, 31);
            label7.TabIndex = 10;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.ForeColor = SystemColors.WindowText;
            checkBox1.Location = new Point(77, 367);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(147, 24);
            checkBox1.TabIndex = 9;
            checkBox1.Text = "Ghi nhớ mật khẩu";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            label6.AutoSize = true;
            label6.ForeColor = SystemColors.WindowText;
            label6.Location = new Point(112, 552);
            label6.Name = "label6";
            label6.Size = new Size(277, 20);
            label6.TabIndex = 8;
            label6.Text = "© 2025 ECOS. Bản quyền thuộc về ECOS.";
            label6.Click += label6_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label5.ForeColor = Color.DarkGreen;
            label5.Location = new Point(170, 482);
            label5.Name = "label5";
            label5.Size = new Size(188, 31);
            label5.TabIndex = 7;
            label5.Text = "Quên mật khẩu?";
            // 
            // button2
            // 
            button2.BackgroundImage = (Image)resources.GetObject("button2.BackgroundImage");
            button2.BackgroundImageLayout = ImageLayout.Zoom;
            button2.Cursor = Cursors.Cross;
            button2.Location = new Point(364, 411);
            button2.Name = "button2";
            button2.Size = new Size(54, 55);
            button2.TabIndex = 6;
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(125, 199, 84);
            button1.Font = new Font("Segoe UI", 16.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            button1.ForeColor = Color.DarkGreen;
            button1.Location = new Point(77, 411);
            button1.Name = "button1";
            button1.Size = new Size(281, 55);
            button1.TabIndex = 5;
            button1.Text = "Đăng nhập";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(77, 303);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(341, 47);
            textBox2.TabIndex = 4;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(77, 181);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(341, 47);
            textBox1.TabIndex = 3;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label4.ForeColor = Color.DarkGreen;
            label4.Location = new Point(59, 248);
            label4.Name = "label4";
            label4.Size = new Size(116, 31);
            label4.TabIndex = 2;
            label4.Text = "Mật khẩu";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13.2000008F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label3.ForeColor = Color.DarkGreen;
            label3.Location = new Point(58, 147);
            label3.Name = "label3";
            label3.Size = new Size(117, 31);
            label3.TabIndex = 1;
            label3.Text = "Tài khoản";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label2.ForeColor = Color.Green;
            label2.Location = new Point(130, 38);
            label2.Name = "label2";
            label2.Size = new Size(268, 54);
            label2.TabIndex = 0;
            label2.Text = "ĐĂNG NHẬP";
            label2.Click += label2_Click;
            // 
            // panel3
            // 
            panel3.BackColor = Color.Green;
            panel3.Controls.Add(tableLayoutPanel2);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(117, 60);
            panel3.Name = "panel3";
            panel3.Size = new Size(512, 562);
            panel3.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.BackColor = Color.FromArgb(125, 199, 84);
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 8.505747F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 91.4942551F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 30F));
            tableLayoutPanel2.Controls.Add(pictureBox1, 1, 1);
            tableLayoutPanel2.Controls.Add(label1, 1, 2);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 4;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 23.1607628F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 76.83923F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 122F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            tableLayoutPanel2.Size = new Size(512, 562);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // pictureBox1
            // 
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(43, 98);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(435, 312);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Font = new Font("Arial", 13.2000008F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 163);
            label1.ForeColor = Color.Green;
            label1.Location = new Point(43, 413);
            label1.Name = "label1";
            label1.Size = new Size(435, 122);
            label1.TabIndex = 1;
            label1.Text = "Hệ thống quản lý \nhợp đồng sinh trắc môi trường";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // DangNhap1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1255, 680);
            Controls.Add(panel1);
            MaximizeBox = false;
            Name = "DangNhap1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Form1";
            Load += Form1_Load;
            panel1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel2;
        private Panel panel3;
        private TableLayoutPanel tableLayoutPanel2;
        private PictureBox pictureBox1;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label2;
        private Panel panel4;
        private Label label3;
        private Button button2;
        private Button button1;
        private TextBox textBox2;
        private TextBox textBox1;
        private Label label4;
        private Label label5;
        private Label label6;
        private CheckBox checkBox1;
        private Label label7;
        private Label label8;
    }
}
