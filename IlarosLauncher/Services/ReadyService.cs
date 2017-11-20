using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Net.Webserver;

namespace IlarosLauncher.Services
{
    class ReadyService : WebService
    {
        public ReadyService() : base(WebServiceType.PreCreateDocument)
        {
        }

        public override bool CanWorkWith(WebProgressTask task)
        {
            return task.Document.RequestHeader.Location.IsUrl(new[] { "ready" });
        }

        public override void ProgressTask(WebProgressTask task)
        {
            task.Document.DataSources.Add(new HttpStringDataSource("1")
            {
                MimeType = MimeTypes.TextPlain,
                TransferCompleteData = true
            });
            task.Document.ResponseHeader.HeaderParameter["Cache-Control"] = "no-cache, no-store, must-revalidate";
            task.Document.ResponseHeader.HeaderParameter["Pragma"] = "no-cache";
            task.Document.ResponseHeader.HeaderParameter["Expires"] = "0";
            task.Document.ResponseHeader.HeaderParameter["Access-Control-Allow-Origin"] = "*";
        }
    }
}
