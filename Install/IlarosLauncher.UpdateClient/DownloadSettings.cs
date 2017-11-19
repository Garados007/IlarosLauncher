using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Data.IniFiles;
using System.Net;
using System.IO;

namespace IlarosLauncher.UpdateClient
{
    class DownloadSettings
    {
        public bool UseTemp, UseAppData, UseRegistry, LSSearchUpdates, LSDownloadUpdates, 
            LSDownloadBackgrounds, DownloadBackgrounds, CreateDesktopLink;
        public string LauncherPath;

        public static DownloadSettings Current;

        public static string ServerType;
        public static string ServerUrl;

        public static bool LoadFromIni()
        {
            try
            {
                var l = new OptionsLoader("config.ini", true);
                ServerType = l[0].Options.GetString("ServerType");
                ServerUrl = l[0].Options.GetString("ServerUrl");
#if DEBUG
                if (!System.Diagnostics.Debugger.IsAttached) File.Delete("config.ini");
#else
                File.Delete("config.ini");
#endif
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CanConnect()
        {
            const int myversion = 0;
            try
            {
                using (var wc = new WebClient())
                {
                    var t = wc.DownloadString(ServerUrl + "?mode=version");
                    var version = int.Parse(t);
                    return version <= myversion;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}
