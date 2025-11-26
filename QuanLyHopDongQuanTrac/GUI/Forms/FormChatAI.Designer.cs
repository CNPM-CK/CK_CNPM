namespace GUI.Forms
{
    partial class FormChatAI
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
            txtUserInput = new TextBox();
            txtChatHistory = new TextBox();
            btnSend = new Button();
            SuspendLayout();
            // 
            // txtUserInput
            // 
            txtUserInput.Location = new Point(120, 29);
            txtUserInput.Name = "txtUserInput";
            txtUserInput.Size = new Size(336, 27);
            txtUserInput.TabIndex = 0;
            // 
            // txtChatHistory
            // 
            txtChatHistory.AcceptsReturn = true;
            txtChatHistory.Location = new Point(120, 191);
            txtChatHistory.Multiline = true;
            txtChatHistory.Name = "txtChatHistory";
            txtChatHistory.ScrollBars = ScrollBars.Both;
            txtChatHistory.Size = new Size(596, 214);
            txtChatHistory.TabIndex = 1;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(568, 29);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(94, 29);
            btnSend.TabIndex = 2;
            btnSend.Text = "btnSend";
            btnSend.UseVisualStyleBackColor = true;
            btnSend.Click += btnSend_Click;
            // 
            // FormChatAI
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(btnSend);
            Controls.Add(txtChatHistory);
            Controls.Add(txtUserInput);
            Name = "FormChatAI";
            Text = "FormChatAI";
            Load += FormChatAI_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox txtUserInput;
        private TextBox txtChatHistory;
        private Button btnSend;
    }
}