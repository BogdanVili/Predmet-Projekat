using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace PhishingApp.Config
{
    public class StaticFileService
    {
        private readonly string _baseFolder;
        private readonly HttpListener _listener;
        private readonly string _uri;

        public StaticFileService(string baseFolder, string prefix)
        {
            _baseFolder = Path.GetFullPath(baseFolder);
            _uri = prefix;
            _listener = new HttpListener();
            _listener.Prefixes.Add(_uri);
        }

        public void Start()
        {
            _listener.Start();
            Console.WriteLine($"Static file server running on {_uri}, serving folder: {_baseFolder}");
            Task.Run(async () => await ListenLoop());
        }

        private async Task ListenLoop()
        {
            while (true)
            {
                var context = await _listener.GetContextAsync();
                string urlPath = context.Request.Url.AbsolutePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
                string filePath = Path.Combine(_baseFolder, urlPath);

                if (Directory.Exists(filePath))
                    filePath = Path.Combine(filePath, "index.html");

                if (File.Exists(filePath))
                {
                    byte[] content = File.ReadAllBytes(filePath);
                    context.Response.ContentType = GetContentType(filePath);
                    context.Response.ContentLength64 = content.Length;
                    await context.Response.OutputStream.WriteAsync(content, 0, content.Length);
                }
                else
                {
                    context.Response.StatusCode = 404;
                    using (var writer = new StreamWriter(context.Response.OutputStream))
                    {
                        writer.WriteLine("File not found");
                    }
                }

                context.Response.OutputStream.Close();
            }
        }

        private string GetContentType(string file)
        {
            string ext = Path.GetExtension(file).ToLower();
            switch (ext)
            {
                case ".html":
                    return "text/html";
                case ".css":
                    return "text/css";
                case ".js":
                    return "application/javascript";
                case ".png":
                    return "image/png";
                case ".jpg":
                    return "image/jpeg";
                case ".gif":
                    return "image/gif";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
