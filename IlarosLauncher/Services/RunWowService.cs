using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Net.Webserver;
using System.IO;

namespace IlarosLauncher.Services
{
    class RunWowService : WebService
    {
        public RunWowService() : base(WebServiceType.PreCreateDocument)
        {
        }

        public override bool CanWorkWith(WebProgressTask task)
        {
            return task.Document.RequestHeader.Location.IsUrl(new[] { "run-wow" });
        }

        public override void ProgressTask(WebProgressTask task)
        {
            var wow = Server.UserSettings.UserSettings["wow"].Options.GetString("path", null);
            if (wow == null || string.IsNullOrEmpty(Server.GameServerIP) || !File.Exists(wow + "\\Wow.exe")) return;
            var cache = new DirectoryInfo(wow + "\\Cache");
            if (cache.Exists)
            {
                foreach (var f in cache.EnumerateFiles()) f.Delete();
                foreach (var d in cache.EnumerateDirectories()) d.Delete(true);
            }
            if (!Directory.Exists(wow + @"\Data\deDE")) return;
            File.WriteAllText(wow + @"\Data\deDE\realmlist.wtf", "set realmlist " + Server.GameServerIP);
            System.Diagnostics.Process.Start(wow + "\\Wow.exe");
        }
    }
}
