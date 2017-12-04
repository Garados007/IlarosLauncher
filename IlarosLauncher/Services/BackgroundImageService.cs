using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Net.Webserver;
using System.IO;
using Microsoft.Win32;
using MaxLib.Data.Json;

namespace IlarosLauncher.Services
{
    class BackgroundImageService : WebService
    {
        string dataPath;
        public BackgroundImageService() : base(WebServiceType.PreCreateDocument)
        {
            using (var key = Registry.CurrentUser.OpenSubKey("Software", false))
            using (var skey = key?.OpenSubKey("IlarosLauncher", false))
                dataPath = skey.GetValue("DataPath").ToString() + "\\Content\\Images";
        }

        public override bool CanWorkWith(WebProgressTask task)
        {
            return task.Document.RequestHeader.Location.StartsUrlWith(new[] { "bg" }) &&
                task.Document.RequestHeader.Location.DocumentPathTiles.Length <= 2;
        }

        public override void ProgressTask(WebProgressTask task)
        {
            if (task.Document.RequestHeader.Location.DocumentPathTiles.Length == 1)
            {
                var l = new List<string>();
                if (Directory.Exists(dataPath))
                    foreach (var f in new DirectoryInfo(dataPath).EnumerateFiles())
                        l.Add(f.Name);
                var json = new JsonArray();
                foreach (var e in l) json.Add(JsonValue.Create(e));
                task.Document.DataSources.Add(new HttpStringDataSource(json.Json)
                {
                    MimeType = MimeTypes.ApplicationJson,
                    TransferCompleteData = true
                });
            }
            else
            {
                var path = dataPath + "\\" + task.Document.RequestHeader.Location.DocumentPathTiles[1];
                if (!File.Exists(path)) return;
                var ext = new FileInfo(path).Extension;
                var mime = task.Server.Settings.DefaultFileMimeAssociation.ContainsKey(ext) ?
                    task.Server.Settings.DefaultFileMimeAssociation[ext] : MimeTypes.ImageJpeg;
                task.Document.DataSources.Add(new HttpFileDataSource(path, true)
                {
                    MimeType = mime,
                    TransferCompleteData = true
                });
            }
        }
    }
}
