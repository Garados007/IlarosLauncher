using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jint.Runtime;
using Jint.Native;
using Jint.Native.Json;
using MaxLib.Data.Json;
using MaxLib.Data.HtmlDom;

namespace IlarosLauncher.Modules
{
    class ModuleMainAccess
    {
        internal ModuleWorker ModuleWorker { get; private set; }

        public ModuleMainAccess(ModuleWorker worker)
        {
            ModuleWorker = worker;
            Net = new ModuleNet(worker);
            Props = new ModuleProps(worker);
        }

        public ModuleNet Net { get; private set; }

        public ModuleProps Props { get; private set; }

        public void Event(string type, string key, JsValue value)
        {
            if (type == null) throw new ArgumentNullException("type");
            JsonValue val = null;
            if (value != null)
                switch (value.Type)
                {
                    case Types.Boolean: val = JsonValue.Create(value.AsBoolean()); break;
                    case Types.Number: val = JsonValue.Create(value.AsNumber()); break;
                    case Types.Object: val = JsonValue.Create(
                        JsonInstance.CreateJsonObject(ModuleWorker.Engine)
                            .Stringify(value, new JsValue[] { value.AsObject() })); break;
                    case Types.String: val = JsonValue.Create(value.AsString()); break;
                }
            IlarosLauncher.Server.News.Add(new Services.NewsEntry(type, JsonValue.Create(key), val));
        }

        public void Load(string file)
        {
            ModuleWorker.LoadCode(file);
        }

        public HtmlDomDocument Html(string code)
        {
            return HtmlDomParser.ParseHtml(code);
        }

        public void Log(string text)
        {
#if DEBUG
            var file = Environment.ExpandEnvironmentVariables("%APPDATA%\\IlarosLauncher\\Logs\\Modules\\");
            if (!System.IO.Directory.Exists(file)) System.IO.Directory.CreateDirectory(file);
            file += ModuleWorker.ModuleName + ".log";
            System.IO.File.AppendAllText(file, string.Format("[{0:yyyy-MM-dd HH:mm:ss}] {1}{2}", DateTime.Now, text, Environment.NewLine));
#endif
        }

        public JsValue Server(string group, string name)
        {
            var g = group == null ? IlarosLauncher.Server.ServerSettings[0] :
                IlarosLauncher.Server.ServerSettings[group];
            var v = g?.Options.FindName(name);
            if (v == null) return JsValue.Undefined;
            try { return new JsValue(v.GetDouble()); }
            catch { }
            try { return new JsValue(v.GetBool()); }
            catch { }
            try { return new JsValue(v.GetString()); }
            catch { }
            return JsValue.Null;
        }

        public JsValue User(string group, string name)
        {
            var g = group == null ? IlarosLauncher.Server.UserSettings.UserSettings[0] :
                IlarosLauncher.Server.UserSettings.UserSettings[group];
            var v = g?.Options.FindName(name);
            if (v == null) return JsValue.Undefined;
            try { return new JsValue(v.GetDouble()); }
            catch { }
            try { return new JsValue(v.GetBool()); }
            catch { }
            try { return new JsValue(v.GetString()); }
            catch { }
            return JsValue.Null;
        }
    }
}
