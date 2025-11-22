namespace GUI.Forms
{
    partial class DSKQUC
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            panel5 = new Panel();
            panel7 = new Panel();
            dgvDanhsachketqua = new DataGridView();
            panel6 = new Panel();
            panel5.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDanhsachketqua).BeginInit();
            SuspendLayout();
            // 
            // panel5
            // 
            panel5.Controls.Add(panel7);
            panel5.Controls.Add(panel6);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(0, 0);
            panel5.Name = "panel5";
            panel5.Size = new Size(800, 450);
            panel5.TabIndex = 0;
            // 
            // panel7
            // 
            panel7.Controls.Add(dgvDanhsachketqua);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(0, 61);
            panel7.Name = "panel7";
            panel7.Size = new Size(800, 389);
            panel7.TabIndex = 1;
            // 
            // dgvDanhsachketqua
            // 
            dgvDanhsachketqua.BackgroundColor = Color.White;
            dgvDanhsachketqua.ColumnHeadersHeight = 40;
            dgvDanhsachketqua.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvDanhsachketqua.Dock = DockStyle.Fill;
            dgvDanhsachketqua.EnableHeadersVisualStyles = false;
            dgvDanhsachketqua.Location = new Point(0, 0);
            dgvDanhsachketqua.Name = "dgvDanhsachketqua";
            dgvDanhsachketqua.RowHeadersWidth = 51;
            dgvDanhsachketqua.Size = new Size(800, 389);
            dgvDanhsachketqua.TabIndex = 0;
            dgvDanhsachketqua.CellContentClick += dgvDanhsachketqua_CellContentClick;
            dgvDanhsachketqua.Paint += dgvDanhsachketqua_Paint;
            // 
            // panel6
            // 
            panel6.BackColor = Color.White;
            panel6.Dock = DockStyle.Top;
            panel6.Font = new Font("Segoe UI", 10F);
            panel6.Location = new Point(0, 0);
            panel6.Name = "panel6";
            panel6.Size = new Size(800, 61);
            panel6.TabIndex = 0;
            // 
            // DSKQUC
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel5);
            Name = "DSKQUC";
            Size = new Size(800, 450);
            panel5.ResumeLayout(false);
            panel7.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDanhsachketqua).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel5;
        private Panel panel7;
        private DataGridView dgvDanhsachketqua;
        private Panel panel6;
    }
}