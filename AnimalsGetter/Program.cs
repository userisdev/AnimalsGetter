using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AnimalsGetter
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
        static async Task Main(string[] args)
        {
            var apiKey = args.FirstOrDefault();
            var searchID = args.ElementAtOrDefault(1);
            var query = args.ElementAtOrDefault(2);
            var index = args.ElementAtOrDefault(3);
            var savePath = args.ElementAtOrDefault(4);

            if (new[] { apiKey, searchID, query,index, savePath }.Any(string.IsNullOrEmpty))
            {
                Console.WriteLine("invalid args.");
                Console.WriteLine(string.Join(Environment.NewLine, new[] { ":Usage", "AnimalsGetter [apiKey] [searchID] [query] [index] [savePath]" }));
                Environment.Exit(1);
            }


            string searchType = "image";
            var apiUrl = $"https://www.googleapis.com/customsearch/v1?key={apiKey}&cx={searchID}&q={query}&searchType={searchType}&start={index}";

            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        var json = JObject.Parse(result);
                        var items = json["items"] as JArray;
                        List<string> list = new List<string>();
                        foreach(var item in items)
                        {
                            var url = item["link"].ToString();
                            list.Add(url);
                        }

                        File.WriteAllLines(savePath, list);
                        Console.WriteLine($"{query} {index}/{list.Count} items.");

                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode} - {response.ReasonPhrase}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex}");
                }
            }

        }
    }
}
