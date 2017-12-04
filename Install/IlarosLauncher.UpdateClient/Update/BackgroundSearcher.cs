using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace IlarosLauncher.UpdateClient.Update
{
    class BackgroundSearcher : UpdateStage<BackgroundSearcher.BackgroundSearcherTask>
    {
        public override string GlobalTaskVerb => "Suche: ";

        public override bool CanStart(UpdateManager manager)
        {
            return manager.GetStage<SetupLocalEnvironment>().Finished;
        }

        public BackgroundSearcher()
        {
            if (DownloadSettings.Current.DownloadBackgrounds)
                Tasks.Add(new BackgroundSearcherTask());
        }

        public class BackgroundSearcherTask : UpdateTask
        {
            string localDir;

            public override void Execute(UpdateManager manager)
            {
                Name = "Suche nach neuen Hintergründen";
                localDir = DownloadSettings.Current.UseAppData ?
                    Environment.ExpandEnvironmentVariables("%APPDATA%\\IlarosLauncher\\Content\\Images") :
                    DownloadSettings.Current.LauncherPath + "\\Content\\Images";
                if (!Directory.Exists(localDir)) Directory.CreateDirectory(localDir);
                switch (DownloadSettings.ImgSourceType)
                {
                    case "Direct": MethodDirect(manager.GetStage<BackgroundDownloader>()); break;
                    case "ExtSourceOverCount": MethodExtSourceOverCount(manager.GetStage<BackgroundDownloader>()); break;
                }
                Progress = 1;
            }

            void MethodDirect(BackgroundDownloader downloader)
            {
                using (var wc = new WebClient())
                {
                    var list = wc.DownloadString(DownloadSettings.ServerUrl + "?mode=bglist")
                        .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                    var uribase = new Uri(new Uri(DownloadSettings.ServerUrl), "Backgrounds").AbsoluteUri;
                    foreach (var f in list)
                    {
                        var path = localDir + "\\" + f;
                        if (!File.Exists(path))
                            downloader.Tasks.Add(new BackgroundDownloader.BackgroundDownloadTask(
                                uribase + "\\" + f, path));
                    }
                }
            }

            void MethodExtSourceOverCount(BackgroundDownloader downloader)
            {
                using (var wc = new WebClient())
                {
                    var fc = wc.DownloadString(DownloadSettings.ImgCountLink);
                    int count;
                    if (!int.TryParse(fc, out count)) return;
                    for (int i = 1; i <= count; ++i)
                    {
                        var uri = string.Format(DownloadSettings.ImgFileLink, i);
                        var path = string.Format("{0}\\background-image-{1}.jpg", localDir, i);
                        if (!File.Exists(path))
                            downloader.Tasks.Add(new BackgroundDownloader.BackgroundDownloadTask(uri, path));
                    }
                }
            }
        }
    }
}
