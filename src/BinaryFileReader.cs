using System;
using System.IO;

namespace WavSplitter
{
    public class BinaryFileReader : IDisposable
    {
        private readonly FileStream _fs;
        private readonly BinaryReader _br;

        public BinaryFileReader(string filename)
        {
            _fs = File.OpenRead(filename);
            _br = new BinaryReader(_fs);
        }

        public byte[] ReadBytes(int num)
        {
            return _br.ReadBytes(num);
        }

        public void Dispose() 
        {
            _fs.Dispose();
        }
    }
}