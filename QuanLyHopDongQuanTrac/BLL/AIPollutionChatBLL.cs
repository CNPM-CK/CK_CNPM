using DTO.ChatDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class AIPollutionChatBLL
    {
        private readonly AIPollutionChatService _service;

        public AIPollutionChatBLL()
        {
            _service = new AIPollutionChatService();
        }

        public Task<AIPollutionChatResultDTO> ChatAsync(
            List<ChatMessageDTO> history,
            string userMessage)
        {
            return _service.SendMessageAsync(history, userMessage);
        }
    }
}
