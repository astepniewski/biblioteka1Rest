using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleMain
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (CommandLine.Parser.Default.ParseArguments(args, options))
            {
                if (options.Url != null && options.FileType != null && options.Path != null)
                {
                    DownloadFileAsync(options.Url, options.FileType, options.Path).Wait();
                }

                if (options.Path != null && options.OutPath != null)
                {
                    UploadImage(options.Path, options.OutPath);
                }
            }
        }

        static async Task DownloadFileAsync(string Url, string filetype, string path)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:1480/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

                HttpResponseMessage response = await client.GetAsync(@"api/values?address=" + Url + "&filetype=" + filetype);
                if (response.IsSuccessStatusCode)
                {
                    using (var fileStream = File.Create(path))
                    {
                        CopyStream(await response.Content.ReadAsStreamAsync(), fileStream);
                    }
                }
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[8 * 1024];
            int len;
            while ((len = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, len);
            }
        }

        public static void UploadImage(string path, string outputPath)
        {
            var wc = new WebClient();
            var response = wc.UploadFile("http://localhost:1480/api/values", "POST", path);
            if (response != null)
            {
                File.WriteAllBytes(outputPath, response);
            }
        }
    }
}
