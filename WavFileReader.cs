using System;
using System.Text;

namespace WavSplitter
{
    public class WavFileReader : IDisposable
    {
        private readonly BinaryFileReader _reader;
        private readonly WavFileHeader _header;

        public WavFileHeader Header => _header;

        public WavFileReader(string filename)
        {
            _reader = new BinaryFileReader(filename);
            _header = ReadHeader();
        }

        private WavFileHeader ReadHeader()
        {
            var header = new WavFileHeader();
            ExpectString("RIFF");
            header.ChunkSize = ReadInt();
            ExpectString("WAVE");
            ExpectString("fmt ");
            header.Subchunk1Size = ReadInt();
            header.AudioFormat = ReadShort();
            header.NumChannels = ReadShort();
            header.SampleRate = ReadInt();
            header.ByteRate = ReadInt();
            header.BlockAlign = ReadShort();
            header.BitsPerSample = ReadShort();
            if (header.Subchunk1Size > 16)
            {
                var extras = _reader.ReadBytes(header.Subchunk1Size - 16);
                Console.WriteLine($"Subchunk 1 had {extras.Length} extra bytes: [{string.Join(", ", extras)}]");
            }
            ExpectString("data");

            header.Subchunk2Size = ReadInt();

            foreach(var prop in typeof(WavFileHeader).GetProperties())
            {
                var val = prop.GetValue(header);
                var name = prop.Name;
                Console.WriteLine($"  - {name.PadRight(20)} = {val}");
            }
            return header;
        }

        public byte[] ReadSample()
        {
            var bytesToRead = Header.BitsPerSample / 8;
            return _reader.ReadBytes(bytesToRead);
        }

        private int ReadInt()
        {
            return BitConverter.ToInt32(_reader.ReadBytes(4));
        }

        private short ReadShort()
        {
            return BitConverter.ToInt16(_reader.ReadBytes(2));
        }

        private bool ExpectString(string expected)
        {
            var actual = Encoding.ASCII.GetString(_reader.ReadBytes(expected.Length));
            Console.WriteLine($"Expecting '{expected}', got {actual}. {(actual == expected ? InGreen("\u2714") : InRed("\u274c"))}");
            return actual == expected;
        }

        private string InGreen(object x)
        {
            return $"\u001b[32;1m{x}\u001b[0m";
        }
        private string InRed(object x)
        {
            return $"\u001b[31;1m{x}\u001b[0m";
        }
        public void Dispose()
        {
            _reader.Dispose();
        }
    }

}