﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;
using System.ComponentModel;

namespace Mundasia.Communication
{
    public partial class ServiceConsumer
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
                    XmlSerializer date = new XmlSerializer(typeof(string), StringNamespace);
                    string timeString = (string)date.Deserialize(sr);
                    return timeString;
                }
            }
            catch
            {
                return String.Empty;
            }
        }

        public static bool CreateAccount(string userName, string password)
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
                return false;
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
                    return resp;
                }

            }
            catch (Exception ex)
            {
                return false;
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
                    SessionId = resp;
                    UserId = userName;
                    if(Worker != null)
                    {
                        Worker.CancelAsync();
                        Worker.Dispose();
                        Worker = null;
                    }
                    if (SessionId != -1)
                    {
                        Worker = new BackgroundWorker();
                        Worker.DoWork += Worker_DoWork;
                        Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                        Worker.WorkerSupportsCancellation = true;
                        Worker.RunWorkerAsync();
                    }
                    return resp;
                }
            }
            catch
            {
                return -1;
            }
        }

        public static void Update(string userName, int sessionId)
        {
            try
            {
                string wrURI = baseServerTarget + "update";
                string msg = (new SessionUpdate() { SessionId = sessionId, UserId = userName }).ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                }
            }
            catch {  }
        }

        public static string CreateCharacter(string name, int authority, int care, int fairness, int hobby, int loyalty, int profession, int race, int sex, int talent, int tradition, int vice, int virtue)
        {
            CharacterCreation chr = new CharacterCreation()
            {
                Authority = authority,
                Care = care,
                Fairness = fairness,
                Hobby = hobby,
                Loyalty = loyalty,
                Name = name,
                Profession = profession,
                Race = race,
                SessionId = SessionId,
                Sex = sex,
                Talent = talent,
                Tradition = tradition,
                UserId = UserId,
                Vice = vice,
                Virtue = virtue,
            };
            try
            {
                string wrURI = baseServerTarget + "createcharacter";
                string msg = chr.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch 
            {
                return String.Empty;
            }
        }

        public static string ListCharacters()
        {
            RequestCharacter req = new RequestCharacter()
            {
                SessionId = SessionId,
                UserId = UserId
            };
            try
            {
                string wrURI = baseServerTarget + "listcharacters";
                string msg = req.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch 
            {
                return String.Empty;
            }
        }

        public static string CharacterDetails(string character)
        {
            RequestCharacter req = new RequestCharacter()
            {
                RequestedCharacter = character,
                SessionId = SessionId,
                UserId = UserId
            };
            try
            {
                string wrURI = baseServerTarget + "characterdetails";
                string msg = req.ToString();
                WebRequest wreq = WebRequest.Create(wrURI + "?message=" + msg);
                wreq.Method = "POST";
                wreq.ContentLength = 0;
                WebResponse wresp = wreq.GetResponse();
                using (TextReader sr = new StreamReader(wresp.GetResponseStream()))
                {
                    XmlSerializer xml = new XmlSerializer(typeof(string), StringNamespace);
                    string resp = (string)xml.Deserialize(sr);
                    return resp;
                }
            }
            catch
            {
                return String.Empty;
            }
        }
    }

}
