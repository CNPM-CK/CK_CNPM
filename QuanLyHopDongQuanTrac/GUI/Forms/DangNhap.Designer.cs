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
            splitContainer1 = new SplitContainer();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            textBox2 = new TextBox();
            textBox1 = new TextBox();
            button1 = new Button();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            pictureBox3 = new PictureBox();
            checkBox1 = new CheckBox();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.BackColor = Color.FromArgb(0, 152, 70);
            splitContainer1.Panel1.Controls.Add(label1);
            splitContainer1.Panel1.Controls.Add(pictureBox1);
            splitContainer1.Panel1.Paint += splitContainer1_Panel1_Paint;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(textBox2);
            splitContainer1.Panel2.Controls.Add(textBox1);
            splitContainer1.Panel2.Controls.Add(button1);
            splitContainer1.Panel2.Controls.Add(label7);
            splitContainer1.Panel2.Controls.Add(label6);
            splitContainer1.Panel2.Controls.Add(label5);
            splitContainer1.Panel2.Controls.Add(pictureBox3);
            splitContainer1.Panel2.Controls.Add(checkBox1);
            splitContainer1.Panel2.Controls.Add(label4);
            splitContainer1.Panel2.Controls.Add(label3);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Size = new Size(1035, 554);
            splitContainer1.SplitterDistance = 537;
            splitContainer1.TabIndex = 0;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold | FontStyle.Italic, GraphicsUnit.Point, 163);
            label1.ForeColor = Color.MintCream;
            label1.Location = new Point(120, 428);
            label1.Name = "label1";
            label1.Size = new Size(297, 46);
            label1.TabIndex = 1;
            label1.Text = "Hệ thông quản lý hợp đồng sinh trắc\r\n môi trường thông minh và hiệu quả";
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.BackColor = Color.FromArgb(0, 152, 70);
            pictureBox1.Image = Properties.Resources.remove_background_logo;
            pictureBox1.Location = new Point(40, 122);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(456, 277);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(71, 246);
            textBox2.Multiline = true;
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Nhập mật khẩu";
            textBox2.Size = new Size(312, 50);
            textBox2.TabIndex = 46;
            textBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Window;
            textBox1.Location = new Point(71, 148);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Nhập tên tài khoản của bạn";
            textBox1.Size = new Size(312, 50);
            textBox1.TabIndex = 45;
            textBox1.TextAlign = HorizontalAlignment.Center;
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(0, 152, 70);
            button1.Font = new Font("Segoe UI", 13.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            button1.ForeColor = SystemColors.ButtonFace;
            button1.Location = new Point(71, 362);
            button1.Name = "button1";
            button1.Size = new Size(255, 50);
            button1.TabIndex = 44;
            button1.Text = "Đăng nhập";
            button1.UseVisualStyleBackColor = false;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label7.ForeColor = Color.FromArgb(0, 152, 70);
            label7.Location = new Point(123, 78);
            label7.Name = "label7";
            label7.Size = new Size(257, 20);
            label7.TabIndex = 43;
            label7.Text = "Truy cập vào hệ thống quản lý ECOS";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(137, 510);
            label6.Name = "label6";
            label6.Size = new Size(232, 20);
            label6.TabIndex = 42;
            label6.Text = "© 2025 ECOS. All Rights Reserved";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.8F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label5.ForeColor = Color.FromArgb(0, 152, 70);
            label5.Location = new Point(175, 426);
            label5.Name = "label5";
            label5.Size = new Size(151, 25);
            label5.TabIndex = 41;
            label5.Text = "Quên mật khẩu?";
            // 
            // pictureBox3
            // 
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(342, 362);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(74, 50);
            pictureBox3.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox3.TabIndex = 40;
            pictureBox3.TabStop = false;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(93, 317);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(157, 24);
            checkBox1.TabIndex = 5;
            checkBox1.Text = "Ghi nhớ đăng nhập";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label4.Location = new Point(71, 220);
            label4.Name = "label4";
            label4.Size = new Size(84, 23);
            label4.TabIndex = 2;
            label4.Text = "Mật khẩu";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI Semibold", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label3.Location = new Point(71, 122);
            label3.Name = "label3";
            label3.Size = new Size(113, 23);
            label3.TabIndex = 1;
            label3.Text = "Tên tài khoản";
            label3.Click += label3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 163);
            label2.ForeColor = Color.FromArgb(0, 152, 70);
            label2.Location = new Point(122, 24);
            label2.Name = "label2";
            label2.Size = new Size(268, 54);
            label2.TabIndex = 0;
            label2.Text = "ĐĂNG NHẬP";
            // 
            // DangNhap
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = SystemColors.Window;
            ClientSize = new Size(1035, 554);
            Controls.Add(splitContainer1);
            MaximizeBox = false;
            MdiChildrenMinimizedAnchorBottom = false;
            Name = "DangNhap";
            Text = "Đăng Nhập";
            Load += Form1_Load;
            Resize += Form1_Resize_1;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private SplitContainer splitContainer1;
        private PictureBox pictureBox1;
        private Label label1;
        private Label label4;
        private Label label3;
        private Label label2;
        private CheckBox checkBox1;
        private PictureBox pictureBox3;
        private Label label5;
        private Label label6;
        private Label label7;
        private TextBox textBox1;
        private Button button1;
        private TextBox textBox2;
    }
}
