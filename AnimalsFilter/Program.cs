using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnimalsFilter
{
    /// <summary> Program </summary>
    internal static class Program
    {
        /// <summary> Downloads the size of the file and get file. </summary>
        /// <param name="url"> The URL. </param>
        /// <returns> </returns>
        private static async Task<long> DownloadFileAndGetFileSize(string url)
        {
            HttpClient httpClient = HttpClientFactory.Create();

            // ファイルのダウンロード
            HttpResponseMessage response = await httpClient.GetAsync(url);

            // エラーチェック
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"error url:{url}");
            }

            // ファイルのサイズ取得
            long fileSize = response?.Content?.Headers?.ContentLength ?? int.MaxValue;

            Console.WriteLine($"url:{url}/size:{fileSize}");
            return fileSize;
        }

        /// <summary> Defines the entry point of the application. </summary>
        /// <param name="args"> The arguments. </param>
        private static async Task Main(string[] args)
        {
            string srcPath = args.FirstOrDefault();
            int size = int.TryParse(args.ElementAtOrDefault(1), out int tmp) ? tmp : 0;
            string dstPath = args.ElementAtOrDefault(2);
            if (new[] { srcPath, dstPath }.Any(string.IsNullOrEmpty) || size <= 0)
            {
                Console.WriteLine("invalid args.");
                Console.WriteLine(string.Join(Environment.NewLine, new[] { ":Usage", "AnimalsFilter [srcPath] [size(kb)] [dstPath]" }));
                Environment.Exit(1);
            }

            List<string> list = new List<string>();
            string[] lines = File.ReadAllLines(srcPath);
            foreach (string line in lines)
            {
                string[] s = line.Split(',');
                long fileSize = await DownloadFileAndGetFileSize(s[1]);
                if (fileSize <= (size * 1024))
                {
                    list.Add(line);
                }
            }

            File.WriteAllLines(dstPath, list);
        }
    }
}
