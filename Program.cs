using System;

namespace WavSplitter
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine($"Loading file '{args[0]}'");
            // Console.WriteLine($"sizeof(int): {sizeof(int)}");
            // using (var reader = new WavFileReader(args[0]))
            // {
            //     reader.Describe();
            // }

            var input = args[0];
            var blockSize = int.Parse(args[1]);
            var skip = int.Parse(args[2]);

            using (var writer1 = new WavFileWriter("part1.1.wav"))
            using (var writer2 = new WavFileWriter("part2.1.wav"))
            using (var writer3 = new WavFileWriter("part3.1.wav"))
            {
                using (var reader = new WavFileReader(args[0]))
                {
                    int i = 0;
                    while (i < skip)
                    {
                        reader.ReadSample();
                        i++;
                    }
                    writer1.WriteHeader(reader.Header);
                    writer2.WriteHeader(reader.Header);
                    writer3.WriteHeader(reader.Header);
                    while(i < reader.Header.NumSamples)
                    {
                        for (int j = 0; j < blockSize; j++) {
                            writer1.WriteSample(reader.ReadSample());
                            i++;
                        }
                        for (int j = 0; j < blockSize; j++) {
                            writer2.WriteSample(reader.ReadSample());
                            i++;
                        }
                        for (int j = 0; j < blockSize; j++) {
                            writer3.WriteSample(reader.ReadSample());
                            i++;
                        }
                    }
                    writer1.Complete();
                    writer2.Complete();
                    writer3.Complete();
                }

            }
        }
    }
}
