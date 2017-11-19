using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace IlarosLauncher.UpdateClient.Update
{
    class DownloadFile : UpdateStage<DownloadFile.DownloadFileTask>
    {
        public override string GlobalTaskVerb => "Download:";
        
        public override bool CanStart(UpdateManager manager)
        {
            return manager.GetStage<FetchDownloadList>().Finished;
        }

        public class DownloadFileTask : UpdateTask
        {
            public string Path { get; private set; }

            public string VersionOrHash { get; private set; }

            public DownloadFileTask(string path, string name, string versionOrHash)
            {
                Path = path;
                Name = "Download " + name;
                VersionOrHash = versionOrHash;
            }

            public override void Execute(UpdateManager manager)
            {
                using (var wc = new WebClient())
                {
                    var tcs = new TaskCompletionSource<object>();
                    wc.DownloadFileCompleted += (s, e) =>
                    {
                        if (e.Error != null) tcs.TrySetException(e.Error);
                        else if (e.Cancelled) tcs.TrySetCanceled();
                        else tcs.TrySetResult(null);
                    };
                    wc.DownloadProgressChanged += (s, e) =>
                    {
                        Progress = (float)e.BytesReceived / e.TotalBytesToReceive;
                    };
                    var target = Environment.CurrentDirectory + "\\Downloads\\" + Path;
                    var fi = new FileInfo(target);
                    if (!fi.Directory.Exists) fi.Directory.Create();
                    wc.DownloadFileAsync(new Uri(DownloadSettings.ServerUrl + "/../" + Path), target);

                    tcs.Task.Wait();
                }
            }
        }
    }
}
