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
            //clear cache
            var cache = new DirectoryInfo(wow + "\\Cache");
            if (cache.Exists)
            {
                foreach (var f in cache.EnumerateFiles()) f.Delete();
                foreach (var d in cache.EnumerateDirectories()) d.Delete(true);
            }
            //set ip
            if (!Directory.Exists(wow + @"\Data\deDE")) return;
            File.WriteAllText(wow + @"\Data\deDE\realmlist.wtf", "set realmlist " + Server.GameServerIP);
            //set username
            var g = Server.UserSettings.UserSettings["names"];
            var ind = g?.Options.GetInt32("currentId", -1) ?? -1;
            var name = g?.Options.GetString("name[" + ind.ToString() + "]", null);
            var l = new List<string>();
            try
            {
                if (name != null)
                    using (var f = new FileStream(wow + "\\WTF\\Config.wtf", FileMode.OpenOrCreate))
                    using (var r = new StreamReader(f))
                    using (var w = new StreamWriter(f))
                    {
                        while (!r.EndOfStream)
                        {
                            var line = r.ReadLine();
                            if (!line.StartsWith("SET accountName")) l.Add(line);
                        }
                        l.Add("SET accountName \"" + name + "\"");
                        f.SetLength(0);
                        f.Position = 0;
                        for (int i = 0; i < l.Count; ++i) w.WriteLine(l[i]);
                    }
            }
            catch { }
            //start
            System.Diagnostics.Process.Start(wow + "\\Wow.exe");
        }
    }
}
