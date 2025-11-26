using DTO;
using DTO.ChatDTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

public class AIPollutionChatService
{
    private readonly string _apiKey;
    private const string ModelName = "gemini-2.5-flash";

    private const string SystemPrompt = @"
Bạn là trợ lý ảo chuyên gia về môi trường, làm việc cho doanh nghiệp quan trắc môi trường, tên bạn là ECOS Trợ Lý.

NHIỆM VỤ:
- Chỉ trả lời các câu hỏi liên quan đến:
  + Dự đoán tình trạng ô nhiễm môi trường tại các tỉnh, huyện, khu công nghiệp.
  + Giải thích các yếu tố ảnh hưởng đến ô nhiễm (khí thải, nước thải, bụi, tiếng ồn,...).
  + Nếu có thông số chính xác về khu vực đó thì thể hiện ra.
  + Ngoài ra bạn còn có thể dự báo khả năng tái ký của khách hàng.
- Nếu người dùng hỏi ngoài chủ đề trên, hãy lịch sự từ chối và hướng họ quay về chủ đề ô nhiễm môi trường.

YÊU CẦU VỀ ĐỊNH DẠNG:
- Trả lời bằng tiếng Việt, không dùng bất kỳ ký tự in đậm hoặc in nghiêng, không dùng dấu * trong câu trả lời.
- Không dùng Markdown, không dùng tiêu đề dạng ###, không dùng bullet phức tạp.
- Luôn trả lời theo đúng MẪU sau (không thêm phần mở đầu dài dòng):

Dự báo ô nhiễm tại khu vực {tên khu vực nếu biết, nếu không thì ghi: khu vực được hỏi}:
- Nước: {mô tả ngắn gọn tình trạng ô nhiễm nước}
- Không khí: {mô tả ngắn gọn tình trạng ô nhiễm không khí}
- Đất: {mô tả ngắn gọn tình trạng ô nhiễm đất, nếu không có thông tin thì ghi: Chưa có đủ thông tin}
- Tiếng ồn: {mô tả ngắn gọn về tiếng ồn, nếu không có thông tin thì ghi: Chưa có đủ thông tin}
Ô nhiễm tổng thể ở mức: {Thấp / Trung bình / Cao / Rất cao + giải thích ngắn 1 câu}

Ví dụ khi người dùng hỏi: 'dự báo ô nhiễm An Giang'
Câu trả lời phải có dạng:

Dự báo ô nhiễm tại khu vực An Giang:\n
- Nước: ...\n
- Không khí: ...\n
- Đất: ...\n
- Tiếng ồn: ...\n
Ô nhiễm tổng thể ở mức: ...\n

Không được thêm các đoạn giới thiệu dài dòng trước hoặc sau mẫu này.";


    public AIPollutionChatService()
    {
        _apiKey = AppConfig.Configuration["GoogleAiStudioKey"];

        if (string.IsNullOrEmpty(_apiKey))
            throw new InvalidOperationException("Không tìm thấy khóa GoogleAiStudioKey trong appsettings.json.");
    }

    public async Task<AIPollutionChatResultDTO> SendMessageAsync(
        List<ChatMessageDTO> history,
        string newUserMessage)
    {
        var requestBody = new
        {
            system_instruction = new
            {
                role = "system",
                parts = new[]
                {
                    new { text = SystemPrompt }
                }
            },
            contents = BuildContents(history, newUserMessage)
        };

        string url =
            $"https://generativelanguage.googleapis.com/v1beta/models/{ModelName}:generateContent?key={_apiKey}";

        using (var client = new HttpClient())
        {
            string json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();

            dynamic obj = JsonConvert.DeserializeObject(responseJson);

            string replyText = (string)obj.candidates[0].content.parts[0].text;

            return new AIPollutionChatResultDTO
            {
                ReplyText = replyText
            };
        }
    }

    private object[] BuildContents(List<ChatMessageDTO> history, string newUserMessage)
    {
        var list = new List<object>();

        if (history != null)
        {
            foreach (var msg in history)
            {
                list.Add(new
                {
                    role = msg.Role == ChatRole.User ? "user" : "model",
                    parts = new[]
                    {
                        new { text = msg.Content }
                    }
                });
            }
        }

        list.Add(new
        {
            role = "user",
            parts = new[]
            {
                new { text = newUserMessage }
            }
        });

        return list.ToArray();
    }
}
