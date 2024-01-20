using System;
using System.IO;
using System.Linq;

namespace AnimalsSorter
{
    /// <summary> Program </summary>
    internal static class Program
    {
        /// <summary> Defines the entry point of the application. </summary>
        /// <param name="args"> The arguments. </param>
        private static void Main(string[] args)
        {
            string srcPath = args.FirstOrDefault();
            string dstPath = args.ElementAtOrDefault(1);
            if (new[] { srcPath, dstPath }.Any(string.IsNullOrEmpty))
            {
                Console.WriteLine("invalid args.");
                Console.WriteLine(string.Join(Environment.NewLine, new[] { ":Usage", "AnimalsSorter [srcPath] [dstPath]" }));
                Environment.Exit(1);
            }

            string[] lines = File.ReadAllLines(srcPath).ToHashSet().OrderBy(e => e).ToArray();

            File.WriteAllLines(dstPath, lines);
        }
    }
}
