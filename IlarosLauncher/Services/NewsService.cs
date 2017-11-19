using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Net.Webserver;
using MaxLib.Data.Json;
using MaxLib.Collections;

namespace IlarosLauncher.Services
{
    class NewsService : WebService
    {
        Dictionary<string, NewsEntryList> tokenLists = new Dictionary<string, NewsEntryList>();
        NewsEntryList globalList = new NewsEntryList(true);
        object lockList = new object();
        Random r = new Random();

        void MinimizeList()
        {
            lock (lockList)
            {
                for (int i = 0; i < tokenLists.Count; ++i)
                    if (tokenLists.ElementAt(i).Value.LastAccess + new TimeSpan(0, 0, 30) <= DateTime.Now)
                    {
                        tokenLists.Remove(tokenLists.ElementAt(i).Key);
                        i--;
                    }
            }
        }

        public void Add(NewsEntry entry)
        {
            MinimizeList();
            lock (lockList)
            {
                foreach (var l in tokenLists)
                    l.Value.Add(entry);
                globalList.Add(entry);
            }
        }

        public NewsService() : base(WebServiceType.PreCreateDocument)
        {
        }

        public override bool CanWorkWith(WebProgressTask task)
        {
            return task.Document.RequestHeader.Location.IsUrl(new[] { "news" });
        }

        public override void ProgressTask(WebProgressTask task)
        {
            var token = task.Document.RequestHeader.Location.GetParameter.ContainsKey("token") ?
                task.Document.RequestHeader.Location.GetParameter["token"] : null;
            NewsEntryList list;
            lock (lockList)
            {
                if (tokenLists.ContainsKey(token))
                {
                    list = tokenLists[token];
                    list.LastAccess = DateTime.Now;
                }
                else
                {
                    if (token == null) token = RandomToken();
                    list = globalList.ToChildList();
                    tokenLists.Add(token, list);
                }
            }
            MinimizeList();
            var json = list.ToJson().Json;
            task.Document.DataSources.Add(new HttpStringDataSource(json)
            {
                MimeType = MimeTypes.ApplicationJson,
                TextEncoding = "utf-8",
                TransferCompleteData = true
            });
        }

        string RandomToken()
        {
            var b = new byte[8];
            r.NextBytes(b);
            var hex = "0123456789abcdef";
            var sb = new StringBuilder(b.Length * 2);
            foreach (var n in b)
            {
                sb.Append(hex[n << 4]);
                sb.Append(hex[n & 15]);
            }
            return sb.ToString();
        }
    }

    class NewsEntryList
    {
        SyncedList<NewsEntry> list = new SyncedList<NewsEntry>();

        public DateTime LastAccess { get; set; }

        public bool GlobalList { get; private set; }

        public NewsEntryList(bool globalList)
        {
            GlobalList = globalList;
            LastAccess = DateTime.Now;
        }

        public void Add(NewsEntry entry)
        {
            if (!list.Contains(entry))
                list.Add(entry);
        }

        public NewsEntryList ToChildList()
        {
            var nl = new NewsEntryList(false);
            list.Execute(() => nl.list.AddRange(list));
            return nl;
        }

        public JsonElement ToJson()
        {
            var j = new JsonArray();
            list.Execute(() =>
            {
                foreach (var e in list)
                    j.Add(e.ToJson());
                if (!GlobalList) list.Clear();
            });
            return j;
        }
    }

    class NewsEntry : IEquatable<NewsEntry>
    {
        static int lastId = 0;
        public int Id { get; private set; }

        public string Type { get; private set; }

        public JsonValue Key { get; private set; }

        public DateTime Date { get; private set; }

        public JsonValue Value { get; private set; }

        public NewsEntry(string type, JsonValue key, JsonValue value)
        {
            Id = lastId++;
            Type = type;
            Key = key;
            Date = DateTime.Now;
            Value = value;
        }

        public JsonElement ToJson()
        {
            var j = new JsonObject();
            j.Add("id", JsonValue.Create(Id));
            j.Add("type", JsonValue.Create(Type));
            j.Add("key", Key);
            j.Add("date", JsonValue.Create(Date.ToString("s")));
            j.Add("value", Value);
            return j;
        }

        public override bool Equals(object obj)
        {
            if (obj is NewsEntry) return Equals(obj as NewsEntry);
            else return false;
        }

        public bool Equals(NewsEntry other)
        {
            if (other == null) return false;
            return Type == other.Type && Key?.ArgumentString == other.Key?.ArgumentString;
        }

        public override int GetHashCode()
        {
            return Key != null ? Type.GetHashCode() ^ Key.ArgumentString.GetHashCode() : Type.GetHashCode();
        }

        public static bool operator ==(NewsEntry n1, NewsEntry n2)
        {
            return Equals(n1, n2);
        }

        public static bool operator !=(NewsEntry n1, NewsEntry n2)
        {
            return !Equals(n1, n2);
        }
    }
}
