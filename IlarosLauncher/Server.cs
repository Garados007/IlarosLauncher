using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaxLib.Net.Webserver;
using Serv = MaxLib.Net.Webserver.Services;

namespace IlarosLauncher
{
    static class Server
    {
        public static WebServer CurrentServer { get; private set; }

        public static void Start()
        {
            CurrentServer = new WebServer(new WebServerSettings(45789, 5000));

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
                CurrentServer.Settings.Debug_LogConnections =
                    CurrentServer.Settings.Debug_WriteRequests = true;
#endif
            CreateServices();

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
        }
    }
}
