using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GUI.Helper
{
    public class VoiceRecorder : IDisposable
    {
        private WaveInEvent _waveIn;
        private WaveFileWriter _writer;
        private readonly string _outputPath;
        private bool _isRecording;

        public bool IsRecording => _isRecording;
        public string OutputPath => _outputPath;

        public VoiceRecorder(string outputPath)
        {
            _outputPath = outputPath;
        }

        public void Start()
        {
            if (_isRecording) return;

            Directory.CreateDirectory(Path.GetDirectoryName(_outputPath)!);

            _waveIn = new WaveInEvent
            {
                DeviceNumber = 0,
                WaveFormat = new WaveFormat(16000, 16, 1) // 16kHz, 16bit, mono
            };

            _waveIn.DataAvailable += (s, e) =>
            {
                _writer.Write(e.Buffer, 0, e.BytesRecorded);
            };

            _waveIn.RecordingStopped += (s, e) =>
            {
                _writer?.Dispose();
                _waveIn?.Dispose();
                _isRecording = false;
            };

            _writer = new WaveFileWriter(_outputPath, _waveIn.WaveFormat);
            _waveIn.StartRecording();
            _isRecording = true;
        }

        public void Stop()
        {
            if (!_isRecording) return;
            _waveIn.StopRecording();
        }

        public void Dispose()
        {
            _waveIn?.Dispose();
            _writer?.Dispose();
        }
    }
}
