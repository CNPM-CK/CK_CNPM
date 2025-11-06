using System;
using System.IO;
using System.Threading.Tasks;
using NAudio.Wave;
using Vosk;

namespace BLL
{
    public class TimKiemGiongNoi : IDisposable
    {
        private Model _model;
        private bool _isDisposed = false;
        private WaveInEvent _waveIn;

        // Event partial text realtime
        public event Action<string> OnPartialResult;

        public TimKiemGiongNoi(string modelPath)
        {
            if (!Directory.Exists(modelPath))
                throw new DirectoryNotFoundException($"Model không tồn tại: {modelPath}");

            string[] requiredFiles = new[]
            {
                Path.Combine(modelPath, "am", "final.mdl"),
                Path.Combine(modelPath, "graph", "HCLG.fst"),
                Path.Combine(modelPath, "graph", "words.txt")
            };

            foreach (var file in requiredFiles)
                if (!File.Exists(file))
                    throw new FileNotFoundException($"Thiếu file: {file}");

            Vosk.Vosk.SetLogLevel(-1);
            _model = new Model(modelPath); // Load 1 lần
        }

        public async Task<string> RecognizeFromMicAsync(int maxDurationSeconds = 5)
        {
            if (_isDisposed || _model == null)
                return "(Dịch vụ không khả dụng)";

            return await Task.Run(async () =>
            {
                VoskRecognizer recognizer = null;
                string finalResult = "";

                var tcs = new TaskCompletionSource<bool>();

                try
                {
                    if (WaveInEvent.DeviceCount == 0)
                        return "(Không tìm thấy microphone)";

                    recognizer = new VoskRecognizer(_model, 16000f);

                    _waveIn = new WaveInEvent
                    {
                        DeviceNumber = 0,
                        WaveFormat = new WaveFormat(16000, 1),
                        BufferMilliseconds = 100
                    };

                    _waveIn.DataAvailable += (s, e) =>
                    {
                        if (recognizer.AcceptWaveform(e.Buffer, e.BytesRecorded))
                        {
                            string text = ExtractText(recognizer.Result());
                            if (!string.IsNullOrWhiteSpace(text))
                            {
                                finalResult = text;
                                OnPartialResult?.Invoke(text);
                            }
                        }
                    };

                    _waveIn.RecordingStopped += (s, e) =>
                    {
                        try
                        {
                            string text = ExtractText(recognizer.FinalResult());
                            if (!string.IsNullOrWhiteSpace(text))
                                finalResult = text;

                            tcs.TrySetResult(true); // hoàn tất Task
                        }
                        catch
                        {
                            tcs.TrySetResult(true);
                        }
                    };

                    _waveIn.StartRecording();

                    // Dừng recording sau maxDurationSeconds
                    await Task.Delay(maxDurationSeconds * 1000);
                    _waveIn.StopRecording();

                    // Chờ recordingStopped event
                    await tcs.Task;

                    return string.IsNullOrWhiteSpace(finalResult) ? "(Không nhận được giọng nói)" : finalResult;
                }
                catch (Exception ex)
                {
                    return $"(Lỗi: {ex.Message})";
                }
                finally
                {
                    try { _waveIn?.Dispose(); _waveIn = null; } catch { }
                    try { recognizer?.Dispose(); } catch { }
                }
            });
        }

        private string ExtractText(string jsonResult)
        {
            if (string.IsNullOrWhiteSpace(jsonResult)) return "";
            int start = jsonResult.IndexOf("\"text\"");
            if (start == -1) return "";
            start = jsonResult.IndexOf(":", start) + 1;
            int end = jsonResult.IndexOf("\"", start + 1);
            if (end == -1) return "";
            return jsonResult.Substring(start, end - start).Trim().Trim('"');
        }

        public void StopRecordingIfActive()
        {
            try { _waveIn?.StopRecording(); } catch { }
        }

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            try { _waveIn?.StopRecording(); _waveIn?.Dispose(); _waveIn = null; } catch { }
            try { _model?.Dispose(); _model = null; } catch { }
        }
    }
}
