using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Whisper.net;
using Whisper.net.Ggml;

namespace BLL.Speech
{
    public class WhisperService : IDisposable
    {
        private readonly string _modelPath;

        // factory dùng chung cho toàn app (chỉ load model 1 lần)
        private static WhisperFactory _sharedFactory;
        private WhisperFactory _factory;
        private readonly IATService? _iat;

        // lock để đảm bảo chỉ 1 task init tại 1 thời điểm
        private static readonly SemaphoreSlim _initLock = new(1, 1);
        private bool _initialized = false;

        public WhisperService(string modelPath, IATService? iat = null)
        {
            _modelPath = modelPath;
            _iat = iat;
        }

        public async Task InitAsync()
        {
            if (_initialized && _factory != null)
                return;

            await _initLock.WaitAsync();
            try
            {
                // Nếu đã có factory dùng chung rồi thì chỉ việc gán lại
                if (_sharedFactory != null)
                {
                    _factory = _sharedFactory;
                    _initialized = true;
                    return;
                }

                // Nếu chưa có file model thì tải về
                if (!File.Exists(_modelPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(_modelPath)!);

                    using var modelStream = await WhisperGgmlDownloader.Default
                        .GetGgmlModelAsync(GgmlType.Tiny);   // đang xài Small

                    // mở file với FileShare.Read để sau này có thể đọc song song
                    using var fs = new FileStream(
                        _modelPath,
                        FileMode.Create,
                        FileAccess.Write,
                        FileShare.Read);

                    await modelStream.CopyToAsync(fs);
                }

                // Load model vào factory dùng chung
                _sharedFactory = WhisperFactory.FromPath(_modelPath);
                _factory = _sharedFactory;
                _initialized = true;
            }
            finally
            {
                _initLock.Release();
            }
        }

        public async Task<string> TranscribeAsync(string wavPath)
        {
            if (string.IsNullOrWhiteSpace(wavPath))
                throw new ArgumentException("Đường dẫn file âm thanh không hợp lệ", nameof(wavPath));

            if (!File.Exists(wavPath))
                throw new FileNotFoundException("Không tìm thấy file âm thanh", wavPath);

            var fi = new FileInfo(wavPath);
            if (fi.Length == 0)
                throw new InvalidOperationException("File âm thanh trống, không thể nhận dạng.");

            if (!_initialized || _factory == null)
                throw new InvalidOperationException("WhisperService chưa được InitAsync.");

            using var processor = _factory
                .CreateBuilder()
                .WithLanguage("vi")   // cố định tiếng Việt cho ổn định
                                      //.WithTranslate(false)
                .Build();

            // Đảm bảo chỉ đọc, không bị process khác lock ghi
            using var fs = File.Open(wavPath, FileMode.Open, FileAccess.Read, FileShare.Read);

            var sb = new StringBuilder();

            await foreach (var seg in processor.ProcessAsync(fs))
            {
                if (!string.IsNullOrWhiteSpace(seg.Text))
                {
                    sb.Append(seg.Text);
                }
            }

            return sb.ToString().Trim();
        }

        public async Task<string> TranscribeIFlytekAsync(string wavPath)
        {
            if (_iat == null)
                throw new InvalidOperationException("Chưa cấu hình IATService cho WhisperService.");

            return await _iat.TranscribeAsync(wavPath);
        }


        public void Dispose()
        {
            // không Dispose _sharedFactory ở đây
            // vì nó dùng chung toàn app; để yên cho đến khi app tắt
        }
    }
}
