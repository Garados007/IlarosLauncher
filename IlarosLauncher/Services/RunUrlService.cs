using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Net.Webserver;

namespace IlarosLauncher.Services
{
    class RunUrlService : WebService
    {
        public RunUrlService() : base(WebServiceType.PreCreateDocument)
        {
        }

        public override bool CanWorkWith(WebProgressTask task)
        {
            return task.Document.RequestHeader.Location.IsUrl(new[] { "run-url" }) &&
                task.Document.RequestHeader.Location.GetParameter.ContainsKey("url");
        }

        public override void ProgressTask(WebProgressTask task)
        {
            var url = task.Document.RequestHeader.Location.GetParameter["url"];
            if (!url.StartsWith("http://") && !url.StartsWith("https://")) return;
            System.Diagnostics.Process.Start(url);
        }
    }
}
