using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace IlarosLauncher.UpdateClient.Update
{
    class BackgroundDownloader : UpdateStage<BackgroundDownloader.BackgroundDownloadTask>
    {
        public override string GlobalTaskVerb => "Download: ";

        public override bool CanStart(UpdateManager manager)
        {
            return manager.GetStage<BackgroundSearcher>().Finished;
        }

        public class BackgroundDownloadTask : UpdateTask
        {
            public string Url { get; private set; }

            public string Path { get; private set; }

            public BackgroundDownloadTask(string url, string path)
            {
                Url = url;
                Path = path;
                Name = "Downloade Hintergrundbild " + new FileInfo(path).Name;
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

                    var ds = DownloadSettings.Current;
                    wc.DownloadFileAsync(new Uri(Url), Path);

                    tcs.Task.Wait();
                }
            }
        }
    }
}
