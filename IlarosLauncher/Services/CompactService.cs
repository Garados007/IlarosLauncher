using MaxLib.Data.CompactFileSystem;
using MaxLib.Net.Webserver;
using System;
using System.IO;

namespace IlarosLauncher.Services
{
    class CompactService : WebService, IDisposable
    {
        public CompactSystem FileSystem { get; private set; }

        public CompactService() : base(WebServiceType.PreCreateDocument)
        {
            FileSystem = new CompactSystem(new WriteAbleStreamFaker(new FileStream("Content\\Asset.csf", FileMode.Open, FileAccess.Read), true, true));
        }

        public override bool CanWorkWith(WebProgressTask task)
        {
            return task.Document.RequestHeader.Location.StartsUrlWith(new[] { "web" }, true);
        }

        string GetMime(WebProgressTask task, string name)
        {
            var ind = name.LastIndexOf('.');
            if (ind == -1) return "text/html";
            name = name.Substring(ind);
            if (task.Server.Settings.DefaultFileMimeAssociation.ContainsKey(name))
                return task.Server.Settings.DefaultFileMimeAssociation[name];
            else return "text/html";
        }

        public void Dispose()
        {
            FileSystem.Dispose();
        }

        public override void ProgressTask(WebProgressTask task)
        {
            var loc = task.Document.RequestHeader.Location.DocumentPathTiles;
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                var path = Environment.CurrentDirectory + "\\Web";
                for (int i = 0; i < loc.Length; ++i)
                    path += "\\" + loc[i];
                if (File.Exists(path))
                    task.Document.DataSources.Add(
                        new HttpFileDataSource(path)
                        {
                            TransferCompleteData = true,
                            MimeType = GetMime(task, loc[loc.Length - 1])
                        });
                return;
            }
#endif
            CompactEntry entry = null;
            for (int i = 0; i < loc.Length; ++i)
            {
                if (entry == null) entry = FileSystem.FileTable.GetRootEntry(loc[i]);
                else entry = entry.GetChild(loc[i]);
                if (entry == null)
                {
                    task.Document.ResponseHeader.StatusCode = HttpStateCode.NotFound;
                    return;
                }
            }
            if (!entry.IsFile)
                task.Document.ResponseHeader.StatusCode = HttpStateCode.NotFound;
            else
            {
                task.Document.DataSources.Add(
                    new HttpStreamDataSource(entry.GetContent())
                    {
                        TransferCompleteData = true,
                        MimeType = GetMime(task, loc[loc.Length - 1])
                    });
            }
        }
    }
}
