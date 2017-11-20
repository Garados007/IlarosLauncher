using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace IlarosLauncher.UpdateClient.Update
{
    class ManageFiles : UpdateStage<ManageFiles.FileTask>
    {
        public override string GlobalTaskVerb => "Kopiere: ";

        public override bool CanStart(UpdateManager manager)
        {
            return manager.GetStage<FileSearcher>().Finished && manager.GetStage<CloseWaiter>().Finished;
        }

        public class FileTask : UpdateTask
        {
            public string Source { get; private set; }

            public string Target { get; private set; }

            public FileTask(string copySource, string copyTarget)
            {
                Source = copySource;
                Target = copyTarget;
                Name = "Verschiebe Datei " + new FileInfo(copySource).Name;
            }

            public FileTask(string deleteFile)
            {
                Source = deleteFile;
                Target = null;
                Name = "Lösche Datei " + new FileInfo(deleteFile).Name;
            }

            public override void Execute(UpdateManager manager)
            {
                if (File.Exists(Source))
                    if (Target == null) File.Delete(Source);
                    else
                    {
                        var fi = new FileInfo(Target);
                        if (!fi.Directory.Exists) fi.Directory.Create();
                        File.Copy(Source, Target, true);
                    }
                Progress = 1;
            }
        }
    }
}
