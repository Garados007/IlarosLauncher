﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Net.Webserver;
using MaxLib.Data.Json;
using MaxLib.Data.IniFiles;
using Microsoft.Win32;

namespace IlarosLauncher.Services
{
    class SettingsService : WebService
    {
        public OptionsLoader UserSettings { get; private set; }
        string path;

        public SettingsService() : base(WebServiceType.PreCreateDocument)
        {
            var settingPath = Environment.CurrentDirectory;
            var key = Registry.CurrentUser.OpenSubKey("Software");
            key = key?.OpenSubKey("IlarosLauncher");
            settingPath = path = (string)key?.GetValue("DataPath") ?? settingPath;
            UserSettings = new OptionsLoader(settingPath + "\\settings.ini", false);
        }

        public void Save()
        {
            UserSettings.Export(path + "\\settings.ini");
        }

        static string[] types = new[] { "int", "float", "bool", "string" };
        public override bool CanWorkWith(WebProgressTask task)
        {
            var header = task.Document.RequestHeader;
            var method = header.ProtocolMethod;
            var post = header.Post.PostParameter;
            return 
                (
                    method == "GET" || 
                    (
                        method == "POST" && 
                        post.ContainsKey("group") &&
                        post.ContainsKey("setting") &&
                        post.ContainsKey("value") &&
                        post.ContainsKey("type") &&
                        types.Contains(post["type"])
                    )
                ) && 
                header.Location.IsUrl(new[] { "settings" });
        }

        public override void ProgressTask(WebProgressTask task)
        {
            if (task.Document.RequestHeader.ProtocolMethod == "GET")
            {
                var json = new JsonObject();
                foreach (var g in UserSettings.Groups)
                {
                    var gname = g == UserSettings.Groups[0] ? "" : g.Name;
                    var jg = (JsonObject)json[gname] ?? new JsonObject();
                    json[gname] = jg;

                    foreach (OptionsKey v in g.Options.GetSearch().FilterKeys(true))
                    {
                        var jv = new JsonValue();
                        jv.ArgumentString = v.ValueText;
                        jg[v.Name] = jv;

                    }
                }
                task.Document.DataSources.Add(new HttpStringDataSource(json.Json)
                {
                    MimeType = MimeTypes.ApplicationJson,
                    TransferCompleteData = true
                });
            }
            else
            {
                var post = task.Document.RequestHeader.Post.PostParameter;
                var g = UserSettings.Groups.FirstOrDefault(
                    (og) => post["group"] == "" ? og == UserSettings.Groups[0] : og.Name == post["group"]) ?? 
                    UserSettings.Add(post["group"]);
                var s = g.Options.FindName(post["setting"]) ?? g.Options.FastAdd(post["setting"], 0);
                switch (post["type"])
                {
                    case "int":
                        int iv;
                        if (int.TryParse(post["value"], out iv)) s.SetValue(iv);
                        break;
                    case "float":
                        float fv;
                        if (float.TryParse(post["value"], out fv)) s.SetValue(fv);
                        break;
                    case "bool":
                        bool bv;
                        if (bool.TryParse(post["value"], out bv)) s.SetValue(bv);
                        break;
                    case "string":
                        s.SetValue(post["value"]);
                        break;
                }
                Save();
            }
        }
    }
}