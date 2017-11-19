using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Net.Webserver;
using MaxLib.Net.ServerScripts;
using System.IO;

namespace IlarosLauncher.Services
{
    class ServerScriptService : WebService
    {
        public ServerScriptService() : base(WebServiceType.PostCreateDocument)
        {
        }

        public override bool CanWorkWith(WebProgressTask task)
        {
            if (task.Document.DataSources.Count == 0) return false;
            var tiles = task.Document.RequestHeader.Location.DocumentPathTiles;
            if (tiles.Length == 0) return false;
            var loc = tiles[tiles.Length - 1].ToLower();
            return loc.EndsWith(".js.html") || loc.EndsWith(".js.htm") || loc.EndsWith(".jsh");
        }

        public override void ProgressTask(WebProgressTask task)
        {
            var source = task.Document.DataSources[0];
            var stream = new MemoryStream((int)source.AproximateLength());
            source.WriteToStream(stream);
            stream.Position = 0;
            var parse = new ExtendedServerScript(Server.CurrentServer)
            {
                NeedJsTag = true,
                ExportReturnValue = false,
                MergeJS = true,
            };
            parse.ScriptObject.GetParam = task.Document.RequestHeader.Location.GetParameter;
            task.Document.DataSources[0] = new HttpStreamDataSource(parse.Parse(stream))
            {
                MimeType = MimeTypes.TextHtml,
                TransferCompleteData = true
            };
            source.Dispose();
        }
    }

    class ExtendedServerScript : ServerScript
    {
        public ExtendedServerScript(WebServer server) : base(server)
        {
            ScriptObject = new ExtendedServerScriptObject(server);
        }
    }

    class ExtendedServerScriptObject : ServerScriptObject
    {
        public ExtendedServerScriptObject(WebServer server) : base(server)
        {
        }
        
        public string Version(string key)
        {
            if (string.IsNullOrEmpty(key)) return "";
            const string supported = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ.,-_ ";
            for (var i = 0; i < key.Length; ++i)
                if (!supported.Contains(key[i])) return "";
            var file = System.Reflection.Assembly.GetAssembly(GetType()).Location;
            file = new FileInfo(file).Directory.FullName + "\\Versions\\" + key + ".version";
            if (!File.Exists(file)) return "";
            else return File.ReadAllText(file);
        }
        
        //public string GetBackgroundImages()
        //{
        //    var json = SmartJson.CreateArray();
        //    Jobs.BackgroundDownloader.Backgrounds.Lock();
        //    foreach (var path in Jobs.BackgroundDownloader.Backgrounds)
        //        json.Add(SmartJson.CreateValue(path));
        //    Jobs.BackgroundDownloader.Backgrounds.Unlock();
        //    return json.ToString(JsonParser.SingleLine);
        //}
    }
}
