using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Data.Json;

namespace IlarosLauncher.Modules
{
    class ModuleProps
    {
        internal ModuleWorker ModuleWorker { get; private set; }

        public ModuleProps(ModuleWorker worker)
        {
            ModuleWorker = worker;
        }

        public string GameServerIP
        {
            get => Server.GameServerIP;
            set
            {
                Server.GameServerIP = value;
                Server.News.Add(new Services.NewsEntry("ip", null, string.IsNullOrEmpty(value) ? JsonValue.Create(false) : JsonValue.Create(value)));
            }
        }
    }
}
