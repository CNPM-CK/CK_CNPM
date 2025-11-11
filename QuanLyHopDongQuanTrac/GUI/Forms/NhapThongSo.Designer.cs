namespace GUI.Forms
{
    partial class NhapThongSo
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
            numericUpDown1 = new NumericUpDown();
            dateTimePicker1 = new DateTimePicker();
            label6 = new Label();
            label7 = new Label();
            textBox3 = new TextBox();
            label2 = new Label();
            textBox4 = new TextBox();
            label5 = new Label();
            textBox1 = new TextBox();
            label4 = new Label();
            textBox2 = new TextBox();
            btnCancel = new Button();
            buttonAddnew = new Button();
            label3 = new Label();
            panel2 = new Panel();
            label1 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(numericUpDown1);
            panel1.Controls.Add(dateTimePicker1);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(textBox3);
            panel1.Controls.Add(label2);
            panel1.Controls.Add(textBox4);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(textBox1);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(textBox2);
            panel1.Controls.Add(btnCancel);
            panel1.Controls.Add(buttonAddnew);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(800, 632);
            panel1.TabIndex = 0;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(473, 429);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(277, 27);
            numericUpDown1.TabIndex = 32;
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(69, 429);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(277, 27);
            dateTimePicker1.TabIndex = 31;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label6.Location = new Point(473, 402);
            label6.Name = "label6";
            label6.Size = new Size(132, 23);
            label6.TabIndex = 29;
            label6.Text = "Giá trị đo được";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label7.Location = new Point(69, 402);
            label7.Name = "label7";
            label7.Size = new Size(78, 23);
            label7.TabIndex = 27;
            label7.Text = "Ngày đo";
            // 
            // textBox3
            // 
            textBox3.Enabled = false;
            textBox3.Location = new Point(473, 289);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(277, 27);
            textBox3.TabIndex = 26;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.Location = new Point(473, 262);
            label2.Name = "label2";
            label2.Size = new Size(112, 23);
            label2.TabIndex = 25;
            label2.Text = "Giá trị tối đa";
            // 
            // textBox4
            // 
            textBox4.Enabled = false;
            textBox4.Location = new Point(69, 289);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(277, 27);
            textBox4.TabIndex = 24;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label5.Location = new Point(69, 262);
            label5.Name = "label5";
            label5.Size = new Size(133, 23);
            label5.TabIndex = 23;
            label5.Text = "Giá trị tối thiểu";
            // 
            // textBox1
            // 
            textBox1.Enabled = false;
            textBox1.Location = new Point(473, 151);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(277, 27);
            textBox1.TabIndex = 22;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label4.Location = new Point(473, 124);
            label4.Name = "label4";
            label4.Size = new Size(63, 23);
            label4.TabIndex = 21;
            label4.Text = "Đơn vị";
            // 
            // textBox2
            // 
            textBox2.Enabled = false;
            textBox2.Location = new Point(69, 151);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(277, 27);
            textBox2.TabIndex = 20;
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
            buttonAddnew.Text = "Lưu";
            buttonAddnew.UseVisualStyleBackColor = false;
            buttonAddnew.Click += buttonAddnew_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label3.Location = new Point(69, 124);
            label3.Name = "label3";
            label3.Size = new Size(112, 23);
            label3.TabIndex = 11;
            label3.Text = "Tên thông số";
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
            label1.Text = "NHẬP THÔNG SỐ";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // NhapThongSo
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 632);
            Controls.Add(panel1);
            MaximizeBox = false;
            Name = "NhapThongSo";
            Text = "Nhập thông số";
            Load += ThemHopDong_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Panel panel2;
        private Label label1;
        private Label label3;
        private Button buttonAddnew;
        private Button btnCancel;
        private TextBox textBox1;
        private Label label4;
        private TextBox textBox2;
        private DateTimePicker dateTimePicker1;
        private Label label6;
        private Label label7;
        private TextBox textBox3;
        private Label label2;
        private TextBox textBox4;
        private Label label5;
        private NumericUpDown numericUpDown1;
    }
}