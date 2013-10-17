using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;

namespace Mundasia.Communication
{
    public class ServiceConsumer
    {
        public const string RSAParametersNamespace = "http://schemas.datacontract.org/2004/07/System.Security.Cryptography";
        
        // TODO: Move this to app.config
        public static string baseServerTarget = "http://localhost:6200/MundasiaServerService/";

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

        public static string CreateAccount(string userName, string password)
        {
            RSAParameters key;
            try
            {
                string wrURI = baseServerTarget + "getpublickey";
                WebRequest wreq = WebRequest.Create(wrURI);
                WebResponse wresp = wreq.GetResponse();
                using (StreamReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer rsa = new XmlSerializer(typeof(RSAParameters), RSAParametersNamespace);
                    key = (RSAParameters)rsa.Deserialize(sr);
                }
            }
            catch
            {
                return "Error.";
            }
            byte[] message = Encryption.Encrypt(String.Format("{0}\n{1}", userName, password), key);

            try
            {
                string wrURI = baseServerTarget + "createaccount";
                XmlSerializer xml = new XmlSerializer(typeof(AccountCreation));
                StringWriter strwrt = new StringWriter();                
                xml.Serialize(strwrt, new AccountCreation() { message = message, pubKey = key });
                string msg = strwrt.ToString();
                msg = msg.Substring(msg.IndexOf('>') + 1);
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    return sr.ReadToEnd();
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
