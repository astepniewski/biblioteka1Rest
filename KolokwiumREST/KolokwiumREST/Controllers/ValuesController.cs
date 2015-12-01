using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Biblioteka1;
using System.IO;
using System.Net.Http.Headers;
using System.Web;

namespace KolokwiumREST.Controllers
{
    public class ValuesController : ApiController
    {

        public HttpResponseMessage Get(string address, string filetype)
        {
            var path = @"C:\Users\student\Desktop\web." + filetype;

            HTTPFileDownloader downloader = new HTTPFileDownloader();
            downloader.DownloadFile(address, path);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(path, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }


        [HttpPost]
        public HttpResponseMessage UploadFile()
        {
            var file = HttpContext.Current.Request.Files.Count > 0 ?
                HttpContext.Current.Request.Files[0] : null;

            var fileName = Path.GetFileName(file.FileName);

            var PathFile = Path.Combine(
                HttpContext.Current.Server.MapPath("~/uploads/tozip"),
                fileName
            );
            file.SaveAs(PathFile);

            var zipPath = Path.Combine(
                HttpContext.Current.Server.MapPath("~/uploads"),
                "archive.zip"
            );
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
            ZipManager zipManager = new ZipManager();
            zipManager.CompressToZip(HttpContext.Current.Server.MapPath("~/uploads/tozip"), zipPath);

            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            var stream = new FileStream(zipPath, FileMode.Open);
            result.Content = new StreamContent(stream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            return result;
        }

    }
}
