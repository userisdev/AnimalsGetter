using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AnimalsGenerator
{
    /// <summary> Program class. </summary>
    internal class Program
    {
        /// <summary> Defines the entry point of the application. </summary>
        /// <param name="args"> The arguments. </param>
        private static void Main(string[] args)
        {
            string srcDirPath = args.FirstOrDefault();
            string dstPath = args.ElementAtOrDefault(1);
            if (new[] { srcDirPath, dstPath }.Any(string.IsNullOrEmpty))
            {
                Console.WriteLine("invalid args.");
                Console.WriteLine(string.Join(Environment.NewLine, new[] { ":Usage", "AnimalsGenerator [srcDirPath] [dstPath]" }));
                Environment.Exit(1);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(srcDirPath);
            Console.WriteLine($"src {dirInfo.FullName}");
            FileInfo[] files = dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly).Where(e => e.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase)).ToArray();
            Console.WriteLine($"files {files.Length} items.");
            List<string> list = new List<string>();
            foreach (FileInfo file in files)
            {
                string name = Path.GetFileName(file.Name).Split('_')[0];
                IEnumerable<string> items = File.ReadAllLines(file.FullName).Select(e => $"{name},{e}");
                list.AddRange(items);
            }

            Console.WriteLine($"csv {list.Count} items.");
            File.WriteAllLines(dstPath, list);
        }
    }
}
