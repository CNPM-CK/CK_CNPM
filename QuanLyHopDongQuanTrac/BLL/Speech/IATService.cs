using System;
using System.IO;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DTO;

namespace BLL.Speech
{
    public class IATService
    {
        private readonly string _appId;
        private readonly string _apiKey;
        private readonly string _apiSecret;

        public IATService(string appId, string apiKey, string apiSecret)
        {
            _appId = appId;
            _apiKey = apiKey;
            _apiSecret = apiSecret;
        }

        public static IATService? TryCreateFromConfiguration()
        {
            string? appId = AppConfig.GetOptional("Speech:IAT:AppId");
            string? apiKey = AppConfig.GetOptional("Speech:IAT:ApiKey");
            string? apiSecret = AppConfig.GetOptional("Speech:IAT:ApiSecret");

            if (string.IsNullOrWhiteSpace(appId)
                || string.IsNullOrWhiteSpace(apiKey)
                || string.IsNullOrWhiteSpace(apiSecret))
            {
                return null;
            }

            return new IATService(appId, apiKey, apiSecret);
        }

        private string BuildAuthUrl(string hostUrl)
        {
            var uri = new Uri(hostUrl);
            string host = uri.Host;
            string path = uri.AbsolutePath;

            string date = DateTime.UtcNow.ToString("r");

            string signatureOrigin =
                $"host: {host}\n" +
                $"date: {date}\n" +
                $"GET {path} HTTP/1.1";

            string signatureSha = Convert.ToBase64String(
                new HMACSHA256(Encoding.UTF8.GetBytes(_apiSecret))
                    .ComputeHash(Encoding.UTF8.GetBytes(signatureOrigin))
            );

            string authorizationOrigin =
                $"api_key=\"{_apiKey}\", algorithm=\"hmac-sha256\", headers=\"host date request-line\", signature=\"{signatureSha}\"";

            string authorizationBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(authorizationOrigin));

            var qs = System.Web.HttpUtility.ParseQueryString(string.Empty);
            qs["authorization"] = authorizationBase64;
            qs["date"] = date;
            qs["host"] = host;

            return hostUrl + "?" + qs.ToString();
        }

        /// <summary>
        /// Gửi file WAV 16kHz mono lên iFLYTEK và trả về text.
        /// </summary>
        public async Task<string> TranscribeAsync(string wavPath)
        {
            if (!File.Exists(wavPath))
                throw new FileNotFoundException("Không tìm thấy file âm thanh", wavPath);

            // Đọc WAV và bỏ header 44 byte để lấy PCM raw
            byte[] wav = File.ReadAllBytes(wavPath);
            if (wav.Length <= 44)
                throw new InvalidOperationException("File WAV quá nhỏ, không hợp lệ.");

            byte[] pcm = new byte[wav.Length - 44];
            Buffer.BlockCopy(wav, 44, pcm, 0, pcm.Length);

            string host = "ws://iat-api-sg.xf-yun.com/v2/iat";
            string url = BuildAuthUrl(host);

            using (var ws = new ClientWebSocket())
            {
                await ws.ConnectAsync(new Uri(url), CancellationToken.None);

                // 🔹 Khung đầu tiên: common + business + data (status = 0, gửi audio luôn)
                string audioBase64 = Convert.ToBase64String(pcm);

                var firstFrame = new
                {
                    common = new { app_id = _appId },
                    business = new
                    {
                        language = "vi_VN",   // tiếng Việt
                        domain = "iat",
                        accent = "mandarin"   // non-Chinese dùng "mandarin"
                        // không set dwa để khỏi dynamic correction
                    },
                    data = new
                    {
                        status = 0,
                        format = "audio/L16;rate=16000",
                        encoding = "raw",
                        audio = audioBase64
                    }
                };

                string jsonFirst = JsonConvert.SerializeObject(firstFrame);
                await ws.SendAsync(
                    new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonFirst)),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );

                // 🔹 Khung cuối: báo kết thúc (status = 2)
                var lastFrame = new
                {
                    data = new
                    {
                        status = 2,
                        format = "audio/L16;rate=16000",
                        encoding = "raw",
                        audio = ""
                    }
                };

                string jsonLast = JsonConvert.SerializeObject(lastFrame);
                await ws.SendAsync(
                    new ArraySegment<byte>(Encoding.UTF8.GetBytes(jsonLast)),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );

                // 🔹 Nhận kết quả
                var buffer = new byte[4096];
                var sb = new StringBuilder();

                while (true)
                {
                    var recv = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (recv.MessageType == WebSocketMessageType.Close)
                        break;

                    string msg = Encoding.UTF8.GetString(buffer, 0, recv.Count);

                    // 👉 tạm thời log thử khi debug cho dễ thấy server trả gì:
                    // System.Diagnostics.Debug.WriteLine(msg);

                    dynamic obj = JsonConvert.DeserializeObject(msg);
                    if (obj == null)
                        continue;

                    int code = obj.code != null ? (int)obj.code : 0;
                    if (code != 0)
                    {
                        string err = obj.message != null ? (string)obj.message : "Unknown error";
                        throw new Exception($"iFLYTEK error {code}: {err}");
                    }

                    if (obj.data != null && obj.data.result != null)
                    {
                        // GHÉP TEXT TỪ ws[].cw[].w
                        foreach (var wsNode in obj.data.result.ws)
                        {
                            foreach (var cw in wsNode.cw)
                            {
                                string w = (string)cw.w;
                                sb.Append(w);
                            }
                        }

                        bool isLast = obj.data.result.ls != null && (bool)obj.data.result.ls;
                        if (isLast)
                            break;
                    }

                    // Nếu server nói đây là last result qua data.status == 2 cũng break
                    if (obj.data != null && obj.data.status != null && (int)obj.data.status == 2)
                        break;
                }
                string final = sb.ToString().Trim();

                final = final.TrimEnd(
                    '.', ',', '?', '!', ';', ':', '/', '\\', '"', '\'', '…',
                    '。', '，', '！', '？', '：', '；'
                );
                return final;
            }
        }
    }
}
