using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Mundasia.Communication
{
    public class ServiceConsumer
    {
        // TODO: Move this to app.config
        public static string baseServerTarget = "";

        public static string Ping()
        {
            try
            {
                string wrURI = baseServerTarget + "ping";
                WebRequest wreq = WebRequest.Create(wrURI);
                WebResponse wresp = wreq.GetResponse();
                using (StreamReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}
