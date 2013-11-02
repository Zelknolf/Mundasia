﻿using System;
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
        public const string StringNamespace = "http://schemas.microsoft.com/2003/10/Serialization/";

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
            AccountCreation ac;
            try
            {
                string wrURI = baseServerTarget + "getpublickey";
                WebRequest wreq = WebRequest.Create(wrURI);
                WebResponse wresp = wreq.GetResponse();
                using (StreamReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer rsa = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)rsa.Deserialize(sr);
                    ac = new AccountCreation(resp);
                }
            }
            catch
            {
                return "Error.";
            }

            // Not meant as encryption, but at least makes sure that the text we
            // save on the server isn't an actual password.
            password = Encryption.GetSha256Hash(password);
            ac.message = Encryption.Encrypt(String.Format("{0}\n{1}", userName, password), ac.pubKey);

            try
            {
                string wrURI = baseServerTarget + "createaccount";
                string msg = ac.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(bool), StringNamespace);
                    bool resp = (bool)xml.Deserialize(sr);
                    return resp.ToString();
                }

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public static int ClientLogin(string userName, string password)
        {
            password = Encryption.GetSha256Hash(password);
            Login lg = new Login();
            lg.userName = userName;
            lg.password = Encryption.GetSha256Hash(password + userName + DateTime.UtcNow.ToShortDateString());

            try
            {
                string wrURI = baseServerTarget + "login";
                string msg = lg.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(int), StringNamespace);
                    int resp = (int)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch
            {
                return -1;
            }
        }
    }
}
