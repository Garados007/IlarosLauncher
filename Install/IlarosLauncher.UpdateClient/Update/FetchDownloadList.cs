using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Data.Json;
using MaxLib.Data.Json.Smart;
using System.Net;
using System.IO;

namespace IlarosLauncher.UpdateClient.Update
{
    class FetchDownloadList : UpdateStage<FetchDownloadList.FetchDownloadTask>
    {
        public override string GlobalTaskVerb => "Vorbereitung:";
        
        public override bool CanStart(UpdateManager manager)
        {
            return true;
        }

        public FetchDownloadList()
        {
            Tasks.Add(new FetchDownloadTask());
        }

        public class FetchDownloadTask : UpdateTask
        {
            public FetchDownloadTask()
            {
                Name = "Lade Dateiliste herunter";
            }

            public override void Execute(UpdateManager manager)
            {
                var json = GetFileList();
                var dft = manager.GetStage<DownloadFile>().Tasks;
                var mft = manager.GetStage<ManageFiles>().Tasks;
                Progress = 0.4f;
                foreach (JsonObject entry in json["modules"].Element.Array)
                {
                    dft.Add(new DownloadFile.DownloadFileTask(
                        entry["path"].Value.Get<string>(),
                        entry["name"].Value.Get<string>(),
                        entry["version"].Value.Get<string>()));
                }
                Progress = 0.7f;
                foreach (JsonObject entry in json["ressources"].Element.Array)
                {
                    dft.Add(new DownloadFile.DownloadFileTask(
                        entry["path"].Value.Get<string>(),
                        entry["name"].Value.Get<string>(),
                        entry["hash"].Value.Get<string>()));
                }
                if (json["deleted"] != null)
                    foreach (JsonObject entry in json["deleted"].Element.Array)
                    {
                        var path = entry["path"].Value.Get<string>();
                        if (path.StartsWith("Client/"))
                            mft.Add(new ManageFiles.FileTask(DownloadSettings.Current.LauncherPath + "\\" + path.Substring("Client/".Length)));
                    }
                Progress = 1;
            }

            SmartJson GetFileList()
            {
                using (var wc = new WebClient())
                {
                    var vf = DownloadSettings.Current.LauncherPath + "\\package.version";
                    string vi = null;
                    if (File.Exists(vf)) vi = File.ReadAllText(vf);
                    var code = wc.DownloadString(DownloadSettings.ServerUrl + "?mode=changes" + (vi == null ? "" : "&version=" + vi));
                    return new JsonParser().ParseSmart(code);
                }
            }
        }
    }
}
