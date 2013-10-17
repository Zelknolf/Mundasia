using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mundasia.Communication;
using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;
using System.IO;


namespace Mundasia.Server.Communication
{
    public class ServerService : IServerService
    {
        public string Ping()
        {
            return DateTime.UtcNow.ToShortTimeString();
        }

        public RSAParameters GetPublicKey()
        {
            return Encryption.GetPubKey();
        }

        public bool CreateAccount(string message)
        {
            int startTrim = message.IndexOf("xmlns");
            int endTrim = message.IndexOf(">");
            message = message.Remove(startTrim, endTrim - startTrim);
            MemoryStream stream = new MemoryStream();
            StreamWriter write = new StreamWriter(stream);
            write.Write(message);
            write.Flush();
            stream.Position = 0;
            XmlSerializer xml = new XmlSerializer(typeof(AccountCreation));
            AccountCreation deSerMessage = (AccountCreation)xml.Deserialize(stream);
            try
            {
                string decMessage = Encryption.Decrypt(deSerMessage.message, deSerMessage.pubKey);
                decMessage = decMessage.Replace("\0", "");
                char[] split = new char[] { '\n' };
                string[] credentials = decMessage.Split(split, 2);
                if (new Account(credentials[0], credentials[1]) != null)
                {
                    return true;
                }
            }
            catch {}
            return false;
        }
    }
}
