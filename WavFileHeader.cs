namespace WavSplitter
{
    public class WavFileHeader
    {
        public int ChunkSize {get; set;}
        public int Subchunk1Size {get; set;}
        public short AudioFormat {get; set;}
        public short NumChannels {get; set;}
        public int SampleRate {get; set;}
        public int ByteRate {get; set;}
        public short BlockAlign {get; set;}
        public short BitsPerSample {get; set;}

        public int Subchunk2Size {get; set;}

        public int NumSamples => Subchunk2Size / NumChannels / (BitsPerSample/8);

    }

}