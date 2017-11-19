using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Net.Webserver;
using Serv = MaxLib.Net.Webserver.Services;
using IlarosLauncher.Services;
using System.IO;
using MaxLib.Data.IniFiles;

namespace IlarosLauncher
{
    static class Server
    {
        public static WebServer CurrentServer { get; private set; }

        public static CompactService CompactService { get; private set; }

        public static OptionsLoader ServerSettings { get; private set; }

        public static void Start()
        {
            CurrentServer = new WebServer(new WebServerSettings(45789, 5000));

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
                CurrentServer.Settings.Debug_LogConnections =
                    CurrentServer.Settings.Debug_WriteRequests = true;
#endif
            CreateServices();
            GetServerSettings();
            CurrentServer.Start();
        }

        private static void CreateServices()
        {
            //Defaults
            CurrentServer.AddWebService(new Serv.HttpHeaderParser());
            CurrentServer.AddWebService(new Serv.HttpHeaderPostParser());
            CurrentServer.AddWebService(new Serv.HttpHeaderSpecialAction());
            CurrentServer.AddWebService(new Serv.HttpResponseCreator());
            CurrentServer.AddWebService(new Serv.HttpSender());
            CurrentServer.AddWebService(new Serv.HttpDocumentFinder());
            //CurrentServer.AddWebService(new Serv.HttpDirectoryMapper(true));
            //Other
            CurrentServer.AddWebService(CompactService = new CompactService());
            CurrentServer.AddWebService(new ServerScriptService());
            CurrentServer.AddWebService(new ReadyService());
            CurrentServer.AddWebService(new NewsService());
        }

        private static void GetServerSettings()
        {
            var entry = CompactService.FileSystem.FileTable.GetRootEntry("settings.ini");
            var stream = entry.GetContent();
            var bytes = new BinaryReader(stream, Encoding.UTF8, true).ReadBytes((int)stream.Length);
            var text = Encoding.UTF8.GetString(bytes);
            ServerSettings = new OptionsLoader(text);
            stream.Dispose();

            entry = CompactService.FileSystem.FileTable.GetRootEntry("mimetypes.ini");
            stream = entry.GetContent();
            bytes = new BinaryReader(stream, Encoding.UTF8, true).ReadBytes((int)stream.Length);
            text = Encoding.UTF8.GetString(bytes);
            CurrentServer.Settings.LoadSettingFromData(text);
            stream.Dispose();
        }
    }
}
