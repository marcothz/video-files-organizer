using System;
using System.IO;
using System.Reflection;
using System.Linq;
using System.Text;
using VideoFilesOrganizer.BL;

namespace VideoFilesOrganizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var inputPath = args[0];
            var outputPath = args[1];

            var asm = Assembly.GetExecutingAssembly();
            Console.WriteLine("{0} {1}", asm.GetName().Name, asm.GetName().Version);
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Searching for videos in \"{0}\"...", inputPath);

            var result = VideoFileLoader.GetVideoFilesWithProperties(inputPath);

            Console.WriteLine("  > {0} were found.", result.Count());

            Console.WriteLine("");
            Console.WriteLine("Renaming videos files in \"{0}\"...", outputPath);

            VideoFileRenamer.Rename(result, outputPath);

            Console.WriteLine("");
            Console.WriteLine("Done.");
        }
    }
}
