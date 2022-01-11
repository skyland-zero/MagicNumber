using BenchmarkDotNet.Attributes;
using FileSignatures;
using Myrmec;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MagicNumber.Tests.Benchmarks
{
    [MemoryDiagnoser]
    public class CheckFileExtensionBenchmarks
    {
        private static readonly string[] files;
        private readonly Magic magic;
        private readonly FileFormatInspector inspector;
        private readonly Sniffer sniffer;
        static CheckFileExtensionBenchmarks()
        {
            files = Directory.GetFiles(Path.Combine(AppContext.BaseDirectory, "Benchmarks", "Files"));
        }

        public CheckFileExtensionBenchmarks()
        {
            magic = new Magic();
            inspector = new FileFormatInspector();

            sniffer = new Sniffer();
            sniffer.Populate(FileTypes.Common);
            sniffer.Populate(FileTypes.Unfrequent);
        }

        public static string[] Data()
        {
            return files;
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public bool CheckFile_Magic(string file)
        {
            var ext = Path.GetExtension(file).Replace(".", "").ToLower();
            using (FileStream stream = File.Open(file, FileMode.Open))
            {
                return magic.Match(stream, ext);
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public bool CheckFile_FileSignatures(string file)
        {
            var ext = Path.GetExtension(file).Replace(".", "").ToLower();
            using (FileStream stream = File.Open(file, FileMode.Open))
            {
                return inspector.DetermineFileFormat(stream)?.Extension == ext ? true : false;
            }
        }

        [Benchmark]
        [ArgumentsSource(nameof(Data))]
        public bool CheckFile_Myrmec(string file)
        {
            var ext = Path.GetExtension(file).Replace(".", "").ToLower();
            using (FileStream stream = File.Open(file, FileMode.Open))
            {
                // get file head byte, may be 20 bytes enough.
                byte[] fileHead = new byte[stream.Length];
                stream.Read(fileHead, 0, fileHead.Length);
                stream.Position = 0;

                // start match.
                List<string> results = sniffer.Match(fileHead);

                return results.Any(x => x == ext);
            }
        }

    }
}
