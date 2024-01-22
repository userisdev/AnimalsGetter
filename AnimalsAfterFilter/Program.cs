using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnimalsAfterFilter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var srcPath = args.FirstOrDefault();
            var imageDirPath = args.ElementAtOrDefault(1);
            var dstPath = args.ElementAtOrDefault(2);

            if (new[] { srcPath, imageDirPath,dstPath }.Any(string.IsNullOrEmpty))
            {
                Console.WriteLine("invalid args.");
                Console.WriteLine(string.Join(Environment.NewLine, new[] { ":Usage", "AnimalsAfterFilter [srcPath] [imagePath] [dstPath]" }));
                Environment.Exit(1);
            }

            var items = Directory.GetFiles(imageDirPath, "*.*", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileNameWithoutExtension)
                .Select(e =>e.Split('_'))
                .Select(s=> (Kind: s[0], Index: int.Parse(s[1])))
                .ToArray();

            var map = new Dictionary<string, HashSet<int>   >();
            foreach(var item in items)
            {
                if (!map.ContainsKey(item.Kind))
                {
                    map[item.Kind] = new HashSet<int>();
                }

                map[item.Kind].Add(item.Index);
            }


            List<string> list = new List<string>();
            string[] lines = File.ReadAllLines(srcPath);
            foreach ((string line, int i) in lines.Select((e, i) => (e, i)))
            {
                try
                {
                    string[] s = line.Split(',');
                    if (map[s[0]].Contains(i))
                    {
                        list.Add(line);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception:{ex}");
                }
            }

            File.WriteAllLines(dstPath, list);
        }
    }
}
