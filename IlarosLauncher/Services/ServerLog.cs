using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Net.Webserver;

namespace IlarosLauncher.Services
{
#if DEBUG
    class ServerLog : WebService
    {
        public ServerLog() : base(WebServiceType.PreCreateDocument)
        {
        }

        public override bool CanWorkWith(WebProgressTask task)
        {
            return task.Document.RequestHeader.Location.StartsUrlWith(new[] { "server-log" });
        }

        public override void ProgressTask(WebProgressTask task)
        {
            var text = new StringBuilder();
            var log = WebServerInfo.Information.ToArray();
            for (int i = 0; i < log.Length; ++i)
            {
                text.AppendLine(log[i].ToString());
            }
            task.Document.DataSources.Add(new HttpStringDataSource(text.ToString())
            {
                MimeType = MimeTypes.TextPlain,
                TextEncoding = "utf-8",
                TransferCompleteData = true
            });
            task.Document.ResponseHeader.StatusCode = HttpStateCode.OK;
            task.Document.PrimaryEncoding = "utf-8";
        }
    }
#endif
}
