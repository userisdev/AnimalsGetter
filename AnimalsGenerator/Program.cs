using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AnimalsGenerator
{
    /// <summary>
    /// Program class.
    /// </summary>
    internal class Program
    {
        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        static void Main(string[] args)
        {
            var srcDirPath = args.FirstOrDefault();
            var dstPath = args.ElementAtOrDefault(1);
            if (new[] { srcDirPath, dstPath}.Any(string.IsNullOrEmpty))
            {
                Console.WriteLine("invalid args.");
                Console.WriteLine(string.Join(Environment.NewLine, new[] { ":Usage", "AnimalsGenerator [srcDirPath] [dstPath]" }));
                Environment.Exit(1);
            }

            var dirInfo = new DirectoryInfo(srcDirPath);
            Console.WriteLine($"src {dirInfo.FullName}");
            var files= dirInfo.GetFiles("*", SearchOption.TopDirectoryOnly).Where(e => e.Extension.Equals(".csv", StringComparison.OrdinalIgnoreCase)).ToArray();
            Console.WriteLine($"files {files.Length} items.");
            var list = new List<string>();
            foreach( var file in files )
            {
                var name = Path.GetFileName(file.Name).Split('_')[0];
                var items = File.ReadAllLines(file.FullName).Select(e =>$"{name},{e}");
                list.AddRange(items);
            }

            Console.WriteLine($"csv {list.Count} items.");
            File.WriteAllLines(dstPath, list);

        }
    }
}
