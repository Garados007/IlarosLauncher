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
            Server.News.Add(new Services.NewsEntry(type, JsonValue.Create(key), val));
        }

        public void Load(string file)
        {
            ModuleWorker.LoadCode(file);
        }

        public HtmlDomDocument Html(string code)
        {
            return HtmlDomParser.ParseHtml(code);
        }
    }
}
