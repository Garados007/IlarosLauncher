using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint;
using System.IO;

namespace IlarosLauncher.Modules
{
    class ModuleWorker
    {
        public Engine Engine { get; private set; }

        public string ModuleName { get; private set; }

        public ModuleMainAccess MainAccess { get; private set; }
        
        public ModuleWorker(string moduleName)
        {
            MainAccess = new ModuleMainAccess(this);
            ModuleName = moduleName;
#if DEBUG
            Engine = new Engine((o) => o.DebugMode(System.Diagnostics.Debugger.IsAttached));
#else
            Engine = new Engine();
#endif
            Engine.SetValue("$", MainAccess);
        }

        public async Task Execute()
        {
            await Task.Run(() =>
            {
#if DEBUG
                LoadCode("main.js");
#else
                try { LoadCode("main.js"); }
                catch { }
#endif
            });
        }

        public void LoadCode(string path)
        {
            var pt = path.Split(new[] { '\\', '/' }, StringSplitOptions.RemoveEmptyEntries);
            var s = LoadCodeStream(pt);
            if (s!= null)
                using (s)
                using (var r = new StreamReader(s))
                {
                    var c = r.ReadToEnd();
                    Engine.Execute(c);
                }
        }

        Stream LoadCodeStream(string[] pathTiles)
        {
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                var path = "Modules\\" + ModuleName + "\\" + string.Join("\\", pathTiles);
                if (File.Exists(path))
                    return new FileStream(path, FileMode.Open);
            }
#endif
            var cs = Server.CompactService.FileSystem.FileTable;
            var entry = cs.GetRootEntry("Modules");
            if (pathTiles.Length == 0 || entry == null) return null;
            if (pathTiles[0] != "..") entry = entry.GetChild(ModuleName);
            for (int i = 0; i<pathTiles.Length; ++i)
            {
                if (i == 0 && pathTiles[i] == "..") continue;
                entry = entry?.GetChild(pathTiles[i]);
            }
            return entry.IsFile ? entry.GetContent() : null;
        }

        private static List<ModuleWorker> worker = new List<ModuleWorker>();


        public static void LaunchWorker()
        {
            foreach (var n in GetModuleNames())
            {
                var w = new ModuleWorker(n);
                worker.Add(w);
#pragma warning disable CS4014
                w.Execute();
#pragma warning restore CS4014
            }
        }

        private static IEnumerable<string> GetModuleNames()
        {
#if DEBUG
            var used = new HashSet<string>();
            if (System.Diagnostics.Debugger.IsAttached && Directory.Exists("Modules"))
                foreach (var d in new DirectoryInfo("Modules").EnumerateDirectories())
                {
                    used.Add(d.Name);
                    yield return d.Name;
                }
#endif
            var cs = Server.CompactService.FileSystem.FileTable;
            var entry = cs.GetRootEntry("Modules");
            if (entry == null) yield break;
            foreach (var c in entry.GetChilds())
                if (c.IsDirectory)
#if DEBUG
                    if (!used.Contains(c.Name))
#endif
                    yield return c.Name;
        }
    }
}
