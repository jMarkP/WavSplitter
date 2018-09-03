using System;
using System.IO;
using System.Text;

namespace WavSplitter
{
    public class WavFileWriter : IDisposable
    {
        private readonly FileStream _fs;
        private readonly BinaryWriter _writer;

        private int _bytesWritten = 0;

        public WavFileWriter(string filename)
        {
            _fs = File.OpenWrite(filename);
            _writer = new BinaryWriter(_fs);
        }

        public void WriteHeader(WavFileHeader header)
        {
            WriteString("RIFF");
            WriteInt(36); // No samples written yet
            WriteString("WAVE");
            WriteString("fmt ");
            WriteInt(header.Subchunk1Size);
            WriteShort(header.AudioFormat);
            WriteShort(header.NumChannels);
            WriteInt(header.SampleRate);
            WriteInt(header.ByteRate);
            WriteShort(header.BlockAlign);
            WriteShort(header.BitsPerSample);
            WriteString("data");
            WriteInt(0); // No samples written yet
        }

        public void WriteSample(byte[] sample)
        {
            _writer.Write(sample);
            _bytesWritten += sample.Length;
        }

        public void Complete()
        {
            _fs.Seek(4, SeekOrigin.Begin);
            _writer.Write(36 + _bytesWritten);
            _fs.Seek(40, SeekOrigin.Begin);
            _writer.Write(_bytesWritten);
        }

        private void WriteString(string val)
        {
            _writer.Write(Encoding.ASCII.GetBytes(val));
        }

        private void WriteInt(int val)
        {
            _writer.Write(val);
        }

        private void WriteShort(short val)
        {
            _writer.Write(val);
        }

        public void Dispose()
        {
            _writer.Dispose();
            _fs.Dispose();
        }
    }
}