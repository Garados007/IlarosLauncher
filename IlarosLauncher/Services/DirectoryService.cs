using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Net.Webserver;
using MaxLib.Data.Json;
using System.IO;

namespace IlarosLauncher.Services
{
    class DirectoryService : WebService
    {
        class Settings
        {
            public bool files = true, dir = true, strict = false, details = true, emptydirs = true;
            public string[] filter = new[] { "all" };
        }

        public DirectoryService() : base(WebServiceType.PreCreateDocument)
        {
        }

        public override bool CanWorkWith(WebProgressTask task)
        {
            return task.Document.RequestHeader.Location.StartsUrlWith(new[] { "dir" });
        }

        const FileAttributes IgnoreFiles = FileAttributes.Hidden | FileAttributes.IntegrityStream | FileAttributes.System;

        public override void ProgressTask(WebProgressTask task)
        {
            var sb = new StringBuilder();
            var t = task.Document.RequestHeader.Location.DocumentPathTiles;
            for (int i = 1; i<t.Length; ++i)
            {
                if (i > 1) sb.Append('\\');
                sb.Append(t[i]);
                if (i == 1) sb.Append(':');
            }
            var path = sb.ToString();
            var set = GetSettings(task.Document.RequestHeader.Location.GetParameter);
            var url = task.Document.RequestHeader.Location.DocumentPath.TrimEnd('/') + '/';
            JsonElement json;

            if (path == "")
            {
                var g = new JsonObject();
                g.Add("url", JsonValue.Create(url));
                g.Add("path", JsonValue.Create(""));
                g.Add("name", JsonValue.Create(""));
                if (set.dir)
                {
                var l = new JsonArray();
                    g.Add("dirs", l);
                    foreach (var drive in Directory.GetLogicalDrives())
                        l.Add(GetJson(new DirectoryInfo(drive), set, false, "/dir/" + drive[0]));
                }
                json = g;
            }
            else
            {
                if (path.EndsWith(":")) path += "\\";
                var dir = new DirectoryInfo(path);
                if (dir.Exists) json = GetJson(dir, set, true, url);
                else
                {
                    var g = new JsonObject();
                    g.Add("url", JsonValue.Create(url));
                    json = g;
                }
            }

            task.Document.DataSources.Add(new HttpStringDataSource(json.Json)
            {
                MimeType = MimeTypes.ApplicationJson,
                TransferCompleteData = true
            });
        }

        Settings GetSettings(Dictionary<string, string> get)
        {
            var s = new Settings();
            if (get.ContainsKey("files") && !bool.TryParse(get["files"], out s.files)) s.files = true;
            if (get.ContainsKey("dir") && !bool.TryParse(get["dir"], out s.dir)) s.dir = true;
            if (get.ContainsKey("strict") && !bool.TryParse(get["strict"], out s.strict)) s.dir = false;
            if (get.ContainsKey("details") && !bool.TryParse(get["details"], out s.details)) s.details = true;
            if (get.ContainsKey("emptydirs") && !bool.TryParse(get["emptydirs"], out s.emptydirs)) s.emptydirs = true;
            if (get.ContainsKey("filter"))
            {
                var l = new List<string>();
                var p = get["filter"].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var f in p)
                    if (f != "!" && !l.Contains(f))
                        l.Add(f);
                s.filter = l.ToArray();
            }
            return s;
        }

        JsonElement GetJson(DirectoryInfo dir, Settings set, bool parent, string url)
        {
            var json = new JsonObject
            {
                { "url", JsonValue.Create(url) },
                { "path", JsonValue.Create(dir.FullName) },
                { "name", JsonValue.Create(dir.Name) }
            };
            if (set.details)
            {
                json.Add("created", JsonValue.Create(dir.CreationTime.ToString("s")));
                json.Add("modified", JsonValue.Create(dir.LastWriteTime.ToString("s")));
            }
            if (!parent) return json;
            if (set.dir)
            {
                var list = new JsonArray();
                json.Add("dirs", list);
                foreach (var sd in dir.GetDirectories())
                { 
                    if ((!set.emptydirs && sd.GetDirectories().Length + sd.GetFiles().Length == 0) ||
                        (sd.Attributes & IgnoreFiles) != 0) continue;
                    list.Add(GetJson(sd, set, false, url + WebServerHelper.EncodeUri(sd.Name) + "/"));
                }
            }
            if (set.files)
            {
                var list = new JsonArray();
                json.Add("files", list);
                foreach (var f in dir.GetFiles())
                {
                    if (f.Extension == "" || f.IsReadOnly || (f.Attributes &  IgnoreFiles) != 0) continue;
                    var types = GetFilters(f);
                    if (Match(types, set.filter, set.strict))
                        list.Add(GetJson(f, set, types));
                }
            }
            return json;
        }

        JsonElement GetJson(FileInfo file, Settings set, string[] filter)
        {
            var json = new JsonObject();
            json.Add("path", JsonValue.Create(file.FullName));
            json.Add("name", JsonValue.Create(file.Name));
            if (set.details)
            {
                json.Add("created", JsonValue.Create(file.CreationTime.ToString("s")));
                json.Add("modified", JsonValue.Create(file.LastWriteTime.ToString("s")));
                json.Add("size", JsonValue.Create(file.Length));
                json.Add("sizet", JsonValue.Create(WebServerHelper.GetVolumeString(file.Length, true, 3)));
                var t = new JsonArray();
                json.Add("types", t);
                foreach (var f in filter)
                    if (f != "all")
                        t.Add(JsonValue.Create(f));
            }
            return json;
        }

        string[] GetFilters(FileInfo file)
        {
            var filter = new List<string>
            {
                "all",
                file.Extension.ToLower()
            };
            switch (file.Extension.ToLower())
            {
                case ".exe":
                case ".jar":
                case ".jad": filter.Add("app"); break;
                case ".png":
                case ".pneg":
                case ".jpg":
                case ".jpeg":
                case ".gif":
                case ".bmp":
                case ".tiff":
                case ".ico": filter.Add("img"); break;
            }
            if (file.Name == "WoW.exe") filter.Add("wow");
            return filter.ToArray();
        }

        bool Match(string[] file, string[] filter, bool strict)
        {
            bool match = true;
            foreach (var f in filter)
                if (f.StartsWith("!"))
                {
                    if (file.Contains(f.Substring(1))) return false;
                }
                else
                {
                    if (file.Contains(f))
                    {
                        if (!strict) return true;
                    }
                    else match = false;
                }
            return match;
        }
    }
}
