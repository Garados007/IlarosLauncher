using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace IlarosLauncher.UpdateClient.Update
{
    class CloseWaiter : UpdateStage<CloseWaiter.CloseWaiterTask>
    {
        public override string GlobalTaskVerb => "Warten:";

        public override bool CanStart(UpdateManager manager)
        {
            return manager.GetStage<DownloadFile>().Finished;
        }

        public CloseWaiter()
        {
            Tasks.Add(new CloseWaiterTask());
        }

        public class CloseWaiterTask : UpdateTask
        {
            public override void Execute(UpdateManager manager)
            {
                Name = "Warte bis laufende IlarosLauncher Instanzen geschlossen wurden";
                var file = new FileInfo(DownloadSettings.Current.LauncherPath + "\\IlarosLauncher.exe").FullName;
                var p = Process.GetProcessesByName("IlarosLauncher").ToList();
                Task.WaitAll(p.ConvertAll((pr) => CloseProcess(pr, file)).ToArray());
                Progress = 1;
            }

            async Task CloseProcess(Process p, string file)
            {
                await Task.Run(() =>
                {
                    if (file != new FileInfo(p.MainModule.FileName).FullName) return;
                    if (!p.CloseMainWindow() || !p.WaitForExit(2000)) p.Kill();
                    p.Dispose();
                });
            }
        }
    }
}
