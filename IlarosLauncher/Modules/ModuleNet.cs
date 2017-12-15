using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using Jint.Native;
using Jint.Runtime;
using System.Net.NetworkInformation;

namespace IlarosLauncher.Modules
{
    class ModuleNet
    {
        class MyWebClient : WebClient
        {
            public int Timeout { get; set; }

            protected override WebRequest GetWebRequest(Uri address)
            {
                var w = base.GetWebRequest(address);
                w.Timeout = Timeout;
                return w;
            }
        }

        internal ModuleWorker ModuleWorker { get; private set; }

        public ModuleNet(ModuleWorker worker)
        {
            ModuleWorker = worker;
            Cookie = new Dictionary<string, string>();
            Request = new Dictionary<string, string>();
            Response = new Dictionary<string, string>();
            webClient = new MyWebClient()
            {
                Timeout = timeout,
                BaseAddress = "http://localhost:45789/modules/" + worker.ModuleName + "/"
            };
        }

        ~ModuleNet()
        {
            webClient.Dispose();
        }

        private MyWebClient webClient;

        public Dictionary<string, string> Cookie { get; private set; }

        public string Error { get; private set; }

        public Dictionary<string, string> Request { get; private set; }

        public Dictionary<string, string> Response { get; private set; }

        public int Status { get; private set; }

        private int timeout = 100000;
        public int Timeout
        {
            get => timeout;
            set
            {
                if (value <= 0) throw new ArgumentNullException("Timeout");
                webClient.Timeout = timeout = value;
            }
        }

        private void SetHeader()
        {
            foreach (var h in Request)
                webClient.Headers[h.Key] = h.Value;
            var cookie = string.Join("; ", Cookie.ToList().ConvertAll((c) => (c.Key ?? "") + "=" + WebUtility.UrlEncode(c.Value ?? "")));
            webClient.Headers[HttpRequestHeader.Cookie] = cookie;
        }

        private void GetHeaders()
        {
            if (webClient.ResponseHeaders == null) return;
            foreach (var h in webClient.ResponseHeaders.AllKeys)
                Response[h] = webClient.ResponseHeaders[h];
            if (webClient.ResponseHeaders.AllKeys.Contains("Set-Cookie"))
                foreach (var c in webClient.ResponseHeaders.GetValues("Set-Cookie"))
                {
                    var p = c.Split(';');
                    var ind = p[0].IndexOf('=');
                    if (ind == -1)
                        Cookie[p[0]] = "";
                    else
                        Cookie[ind > 0 ? ind == p[0].Length - 1 ? p[0] : p[0].Remove(ind) : ""] =
                            ind < p[0].Length - 1 ? p[0].Substring(ind + 1) : "";
                }
        }

        public string Get(string url)
        {
            SetHeader();
            Status = 0;
            Error = null;
            string result = null;
            try
            {
                result = webClient.DownloadString(url);
            }
            catch (WebException e)
            {
                Error = e.Message;
                if (e.Response != null)
                    Status = (int)((HttpWebResponse)e.Response).StatusCode;
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
            GetHeaders();
            return result;
        }

        public JsValue Ping(string url)
        {
            using (var p = new Ping())
            {
                var r = p.Send(url, Timeout);
                return r.Status == IPStatus.Success ? new JsValue(r.RoundtripTime) : JsValue.Null;
            }
        }

        public string Post(string url, JsValue values)
        {
            SetHeader();
            Status = 0;
            Error = null;
            string result = null;
            var vals = new NameValueCollection();
            string valCode = null;
            switch (values.Type)
            {
                case Types.Boolean:
                    vals.Add("", values.AsBoolean().ToString()); break;
                case Types.Number:
                    vals.Add("", values.AsNumber().ToString()); break;
                case Types.Object:
                    foreach (var e in values.AsObject().GetOwnProperties())
                        vals.Add(e.Key, e.Value.Value.ToString());
                    break;
                case Types.String:
                    valCode = values.AsString(); break;
            }
            try
            {
                if (valCode == null)
                {
                    var data = webClient.UploadValues(url, vals);
                    result = webClient.Encoding.GetString(data);
                }
                else result = webClient.UploadString(url, valCode);
            }
            catch (WebException e)
            {
                Error = e.Message;
                Status = (int)((HttpWebResponse)e.Response).StatusCode;
            }
            catch (Exception e)
            {
                Error = e.Message;
            }
            GetHeaders();
            return result;
        }
    }
}
