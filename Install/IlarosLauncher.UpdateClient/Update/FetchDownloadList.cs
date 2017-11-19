using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IlarosLauncher.UpdateClient.Update
{
    class FetchDownloadList : UpdateStage<FetchDownloadList.FetchDownloadTask>
    {
        public override string GlobalTaskVerb => "Vorbereitung:";

        bool finishes = false;
        public override bool Finishes => finishes;

        public override bool CanStart(UpdateManager manager)
        {
            return true;
        }

        public class FetchDownloadTask : UpdateTask
        {
            public override void Execute(UpdateManager manager)
            {

            }
        }
    }
}
