using GUI.Properties;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace GUI.Forms
{
    partial class AI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AI));
            textBox1 = new TextBox();
            panelInput = new GUI.Helper.ModernPanel4Goc();
            btnMic = new GUI.Helper.ModernButton();
            btnSend = new GUI.Helper.ModernButton();
            flowLayoutPanelChat = new FlowLayoutPanel();
            modernPanel4Goc2 = new GUI.Helper.ModernPanel4Goc();
            flowLayoutPanelHistory = new FlowLayoutPanel();
            tableLayoutPanel1 = new TableLayoutPanel();
            panelChatArea = new GUI.Helper.ModernPanel4Goc();
            panelInput.SuspendLayout();
            modernPanel4Goc2.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            panelChatArea.SuspendLayout();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.AcceptsReturn = true;
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.BorderStyle = BorderStyle.None;
            textBox1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
            textBox1.Location = new Point(10, 10);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Nhập câu hỏi....";
            textBox1.Size = new Size(2102, 48);
            textBox1.TabIndex = 0;
            // 
            // panelInput
            // 
            panelInput.BackColor = Color.White;
            panelInput.BackColorFill = Color.White;
            panelInput.BorderColor = Color.FromArgb(200, 200, 200);
            panelInput.BorderRadius = 20;
            panelInput.Controls.Add(btnMic);
            panelInput.Controls.Add(btnSend);
            panelInput.Controls.Add(textBox1);
            panelInput.Dock = DockStyle.Bottom;
            panelInput.DrawTextOnPanel = false;
            panelInput.Location = new Point(10, 413);
            panelInput.Margin = new Padding(30, 3, 30, 3);
            panelInput.Name = "panelInput";
            panelInput.Padding = new Padding(10);
            panelInput.Size = new Size(1249, 70);
            panelInput.TabIndex = 1;
            // 
            // btnMic
            // 
            btnMic.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnMic.BackColor = Color.White;
            btnMic.BackColorHover = Color.FromArgb(174, 153, 68);
            btnMic.BackColorNormal = Color.FromArgb(255, 255, 255);
            btnMic.BorderColor = Color.FromArgb(180, 180, 180);
            btnMic.BorderRadius = 20;
            btnMic.ButtonImage = Resources.microphone;
            btnMic.FlatStyle = FlatStyle.Flat;
            btnMic.ImageTextPadding = 6;
            btnMic.Location = new Point(2099, 5);
            btnMic.Name = "btnMic";
            btnMic.Size = new Size(55, 48);
            btnMic.TabIndex = 0;
            btnMic.TabStop = false;
            btnMic.UseVisualStyleBackColor = false;
            // 
            // btnSend
            // 
            btnSend.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSend.BackColor = Color.Transparent;
            btnSend.BackColorHover = Color.FromArgb(174, 153, 68);
            btnSend.BackColorNormal = Color.FromArgb(255, 255, 255);
            btnSend.BorderColor = Color.FromArgb(180, 180, 180);
            btnSend.BorderRadius = 20;
            btnSend.ButtonImage = (Image)resources.GetObject("btnSend.ButtonImage");
            btnSend.FlatStyle = FlatStyle.Flat;
            btnSend.ImageTextPadding = 6;
            btnSend.Location = new Point(2159, 5);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(55, 48);
            btnSend.TabIndex = 1;
            btnSend.UseVisualStyleBackColor = false;
            // 
            // flowLayoutPanelChat
            // 
            flowLayoutPanelChat.AutoScroll = true;
            flowLayoutPanelChat.BackColor = Color.White;
            flowLayoutPanelChat.BorderStyle = BorderStyle.FixedSingle;
            flowLayoutPanelChat.Dock = DockStyle.Fill;
            flowLayoutPanelChat.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelChat.Location = new Point(10, 10);
            flowLayoutPanelChat.Margin = new Padding(10);
            flowLayoutPanelChat.Name = "flowLayoutPanelChat";
            flowLayoutPanelChat.Size = new Size(1249, 403);
            flowLayoutPanelChat.TabIndex = 0;
            flowLayoutPanelChat.WrapContents = false;
            // 
            // modernPanel4Goc2
            // 
            modernPanel4Goc2.BackColor = Color.White;
            modernPanel4Goc2.BackColorFill = Color.White;
            modernPanel4Goc2.BorderColor = Color.FromArgb(200, 200, 200);
            modernPanel4Goc2.BorderRadius = 20;
            modernPanel4Goc2.Controls.Add(flowLayoutPanelHistory);
            modernPanel4Goc2.Dock = DockStyle.Fill;
            modernPanel4Goc2.DrawTextOnPanel = false;
            modernPanel4Goc2.Location = new Point(3, 3);
            modernPanel4Goc2.Name = "modernPanel4Goc2";
            modernPanel4Goc2.Padding = new Padding(10);
            modernPanel4Goc2.Size = new Size(226, 493);
            modernPanel4Goc2.TabIndex = 0;
            // 
            // flowLayoutPanelHistory
            // 
            flowLayoutPanelHistory.Dock = DockStyle.Fill;
            flowLayoutPanelHistory.Location = new Point(10, 10);
            flowLayoutPanelHistory.Margin = new Padding(10);
            flowLayoutPanelHistory.Name = "flowLayoutPanelHistory";
            flowLayoutPanelHistory.Size = new Size(206, 473);
            flowLayoutPanelHistory.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15.4F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 84.6F));
            tableLayoutPanel1.Controls.Add(modernPanel4Goc2, 0, 0);
            tableLayoutPanel1.Controls.Add(panelChatArea, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(1507, 499);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // panelChatArea
            // 
            panelChatArea.BackColor = Color.White;
            panelChatArea.BackColorFill = Color.White;
            panelChatArea.BorderColor = Color.FromArgb(200, 200, 200);
            panelChatArea.BorderRadius = 20;
            panelChatArea.Controls.Add(flowLayoutPanelChat);
            panelChatArea.Controls.Add(panelInput);
            panelChatArea.Dock = DockStyle.Fill;
            panelChatArea.DrawTextOnPanel = false;
            panelChatArea.Location = new Point(235, 3);
            panelChatArea.Name = "panelChatArea";
            panelChatArea.Padding = new Padding(10);
            panelChatArea.Size = new Size(1269, 493);
            panelChatArea.TabIndex = 1;
            // 
            // AI
            // 
            Controls.Add(tableLayoutPanel1);
            Name = "AI";
            Size = new Size(1507, 499);
            panelInput.ResumeLayout(false);
            panelInput.PerformLayout();
            modernPanel4Goc2.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            panelChatArea.ResumeLayout(false);
            ResumeLayout(false);
        }


        #endregion

        private TextBox textBox1;
        private Helper.ModernPanel4Goc panelInput;
        private FlowLayoutPanel flowLayoutPanelChat;
        private Helper.ModernButton btnSend;
        private Helper.ModernPanel4Goc modernPanel4Goc2;
        private Helper.ModernButton btnMic;
        private TableLayoutPanel tableLayoutPanel1;
        private GUI.Helper.ModernPanel4Goc panelChatArea;
        private FlowLayoutPanel flowLayoutPanelHistory;
    }
}
