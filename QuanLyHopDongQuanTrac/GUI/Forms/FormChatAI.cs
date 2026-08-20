using BLL;
using DTO.ChatDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GUI.Forms
{
    public partial class FormChatAI : Form
    {
        private readonly AIPollutionChatBLL _chatBll = new AIPollutionChatBLL();
        private readonly List<ChatMessageDTO> _history = new List<ChatMessageDTO>();

        public FormChatAI()
        {
            InitializeComponent();
            this.Load += FormChatAI_Load;
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            string userText = txtUserInput.Text.Trim();
            if (string.IsNullOrEmpty(userText))
                return;

            // Append user msg lên UI
            AppendMessageToHistoryBox("Bạn", userText);
            _history.Add(new ChatMessageDTO
            {
                Role = ChatRole.User,
                Content = userText
            });

            txtUserInput.Clear();
            btnSend.Enabled = false;

            try
            {
                var result = await _chatBll.ChatAsync(_history, userText);

                string aiText = result.ReplyText;

                // Append AI msg lên UI
                AppendMessageToHistoryBox("ECOS Trợ Lý", aiText);
                _history.Add(new ChatMessageDTO
                {
                    Role = ChatRole.Assistant,
                    Content = aiText
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi gọi AI: " + ex.Message);
            }
            finally
            {
                btnSend.Enabled = true;
            }
        }

        private void AppendMessageToHistoryBox(string senderName, string text)
        {
            // Chuẩn hoá mọi kiểu newline (\r\n, \r, \n) thành Environment.NewLine
            if (!string.IsNullOrEmpty(text))
            {
                text = text.Replace("\r\n", "\n")
                           .Replace("\r", "\n")
                           .Replace("\n", Environment.NewLine);
            }

            txtChatHistory.AppendText(senderName + ":" + Environment.NewLine);
            txtChatHistory.AppendText(text + Environment.NewLine);
            txtChatHistory.AppendText(new string('-', 40) + Environment.NewLine);
            txtChatHistory.AppendText(Environment.NewLine); // thêm dòng trống cho dễ đọc

            // Auto scroll xuống cuối
            txtChatHistory.SelectionStart = txtChatHistory.TextLength;
            txtChatHistory.ScrollToCaret();
        }


        private void FormChatAI_Load(object sender, EventArgs e)
        {

            // Lịch sử chat
            txtChatHistory.Multiline = true;
            txtChatHistory.ScrollBars = ScrollBars.Vertical;
            txtChatHistory.AcceptsReturn = true;
            txtChatHistory.WordWrap = true;
            txtChatHistory.ReadOnly = true;

            // Ô nhập
            txtUserInput.Multiline = true;
            txtUserInput.ScrollBars = ScrollBars.Vertical;
            txtUserInput.AcceptsReturn = true;
            txtUserInput.WordWrap = true;
        }
    }
}
