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
        private readonly EnvironmentalKnowledgeApiService _knowledgeApi;
        private readonly RenewalPredictionBLL _renewalPredictionBll;

        public AIPollutionChatBLL()
        {
            _service = new AIPollutionChatService();
            _knowledgeApi = new EnvironmentalKnowledgeApiService();
            _renewalPredictionBll = new RenewalPredictionBLL();
        }

        public async Task<AIPollutionChatResultDTO> ChatAsync(
            List<ChatMessageDTO> history,
            string userMessage)
        {
            if (RenewalPredictionBLL.IsRenewalIntent(userMessage))
            {
                return await _renewalPredictionBll.BuildChatReplyAsync(userMessage);
            }

            try
            {
                var knowledge = await _knowledgeApi.SearchAsync(
                    userMessage,
                    topK: 5,
                    asOfDate: DateOnly.FromDateTime(DateTime.Today));
                if (knowledge.Results.Count == 0)
                {
                    return new AIPollutionChatResultDTO
                    {
                        ReplyText = knowledge.Warning + Environment.NewLine +
                            "Tôi hỗ trợ tra cứu quy trình quan trắc, lấy/bảo quản mẫu, " +
                            "QA/QC và các QCVN môi trường đã có trong kho tri thức."
                    };
                }

                return await _service.SendGroundedMessageAsync(
                    history, userMessage, knowledge);
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException(
                    "Không kết nối được kho tri thức ECOS. Hãy khởi động ai-taiky-api " +
                    "và kiểm tra endpoint /v1/knowledge/status trước khi dùng trợ lý RAG.",
                    ex);
            }
        }
    }
}
