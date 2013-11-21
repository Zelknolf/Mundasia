﻿using System;
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

        public string GetPublicKey()
        {
            AccountCreation ac = new AccountCreation();
            ac.pubKey = Encryption.GetPubKey();
            ac.message = new byte[0];
            return ac.ToString();
        }

        public bool CreateAccount(string message)
        {
            AccountCreation ac = new AccountCreation(message);
            try
            {
                string decMessage = Encryption.Decrypt(ac.message, ac.pubKey);
                char[] split = new char[] { '\n' };
                string[] credentials = decMessage.Split(split, 2);
                
                // If the account already exists, don't make a new one.
                if(Account.LoadAccount(credentials[0]) != null)
                {
                    return false;
                }

                // otherwise make a new one.
                if (new Account(credentials[0], credentials[1]) != null)
                {
                    return true;
                }
            }
            catch {}
            return false;
        }

        public int Login(string message)
        {
            Login lg = new Login(message);
            Account targetAcct = Account.LoadAccount(lg.userName);
            if (targetAcct.Authenticate(lg.password))
            {
                return Account.GetSessionId(lg.userName);
            }
            return -1;
        }
    }
}
