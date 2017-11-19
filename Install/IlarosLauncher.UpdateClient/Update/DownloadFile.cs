using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Name = name;
                VersionOrHash = versionOrHash;
            }

            public override void Execute(UpdateManager manager)
            {
                throw new NotImplementedException();
            }
        }
    }
}
