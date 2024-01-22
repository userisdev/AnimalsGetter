using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AnimalsFilter
{
    /// <summary>
    /// Program
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Downloads the size of the file and get file.
        /// </summary>
        /// <param name="url">The URL.</param>
        /// <param name="savePathWithoutExtension">The save path without extension.</param>
        /// <returns></returns>
        private static async Task<long> DownloadFileAndGetFileSize(string url,string savePathWithoutExtension)
        {
            HttpClient httpClient = HttpClientFactory.Create();

            // ファイルのダウンロード
            HttpResponseMessage response = await httpClient.GetAsync(url);

            // エラーチェック
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"error url:{url}");
                return -1;
            }

            // ファイルのサイズ取得
            long fileSize = response?.Content?.Headers?.ContentLength ?? int.MaxValue;

            string fileExtension = GetFileExtension(response);

            // ファイルの保存
            string savePath = $"{savePathWithoutExtension}.{fileExtension}";
            await SaveFile(response, savePath);

            Console.WriteLine($"url:{url}/size:{fileSize}");
            return fileSize;  
        }

        /// <summary>
        /// Saves the file.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="savePath">The save path.</param>
        private static async Task SaveFile(HttpResponseMessage response, string savePath)
        {
            using (Stream contentStream = await response.Content.ReadAsStreamAsync())
            using (FileStream fileStream = File.Create(savePath))
            {
                await contentStream.CopyToAsync(fileStream);
            }
        }

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <returns></returns>
        private static string GetFileExtension(HttpResponseMessage response)
        {
            // Content-Typeヘッダーを取得
            IEnumerable<string> contentTypeHeaders;
            if (response.Content.Headers.TryGetValues("Content-Type", out contentTypeHeaders))
            {
                string contentType = contentTypeHeaders.FirstOrDefault();

                // Content-Typeからメディアタイプを取得
                if (!string.IsNullOrEmpty(contentType))
                {
                    string[] mediaTypeParts = contentType.Split('/');
                    if (mediaTypeParts.Length == 2)
                    {
                        // メディアタイプのサフィックスを拡張子として使用
                        return mediaTypeParts[1];
                    }
                }
            }

            return "dat";
        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        private static async Task Main(string[] args)
        {
            string srcPath = args.FirstOrDefault();
            string dstPath = args.ElementAtOrDefault(1);
            if (new[] { srcPath, dstPath }.Any(string.IsNullOrEmpty))
            {
                Console.WriteLine("invalid args.");
                Console.WriteLine(string.Join(Environment.NewLine, new[] { ":Usage", "AnimalsFilter [srcPath] [dstPath]" }));
                Environment.Exit(1);
            }

            List<string> list = new List<string>();
            string[] lines = File.ReadAllLines(srcPath);
            foreach ((string line,int i) in lines.Select((e,i)=>(e,i)))
            {
                try
                {
                    string[] s = line.Split(',');
                    var path = Path.Combine(Path.GetDirectoryName(Path.GetFullPath(dstPath)), "image", $"{s[0]}_{i}");
                    var saveDirPath = Path.GetDirectoryName(path);
                    Directory.CreateDirectory(saveDirPath);
                    long fileSize = await DownloadFileAndGetFileSize(s[1], path);
                    if (fileSize > 0)
                    {
                        list.Add(line);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Exception:{ex}");
                }
            }

            File.WriteAllLines(dstPath, list);
        }
    }
}
