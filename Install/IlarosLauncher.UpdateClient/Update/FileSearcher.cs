using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IlarosLauncher.UpdateClient.Update
{
    class FileSearcher : UpdateStage<FileSearcher.SearchTask>
    {
        public override string GlobalTaskVerb => throw new NotImplementedException();

        public override bool CanStart(UpdateManager manager)
        {
            return manager.GetStage<DownloadFile>().Finished;
        }

        public FileSearcher()
        {
            Tasks.Add(new SearchTask());
        }

        public class SearchTask : UpdateTask
        {
            public override void Execute(UpdateManager manager)
            {
                Name = "Durchsuche zu kopierende Dateien";
                var ds = DownloadSettings.Current;
                var dir = ds.UseTemp ? Environment.ExpandEnvironmentVariables("%TEMP%\\IlarosLauncher\\Downloads\\Client")
                    : ds.LauncherPath + "\\Temp\\Downloads\\Client";
                SearchDir(new DirectoryInfo(dir), ds.LauncherPath, manager.GetStage<ManageFiles>());
                Progress = 1;
            }

            void SearchDir(DirectoryInfo dir, string path, ManageFiles mf)
            {
                if (!dir.Exists) return;
                foreach (var sd in dir.GetDirectories())
                    SearchDir(sd, path + "\\" + sd.Name, mf);
                foreach (var f in dir.GetFiles())
                    mf.Tasks.Add(new ManageFiles.FileTask(f.FullName, path + "\\" + f.Name));
            }
        }
    }
}
