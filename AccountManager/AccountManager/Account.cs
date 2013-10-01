﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace Mundasia
{
    /// <summary>
    /// The account class is used to manage the saving and checking of credentials of specific players, as well as to find
    /// the characters associated with each account.
    /// </summary>
    [XmlRootAttribute]
    public class Account
    {
        /// <summary>
        /// Parameterless constructor exists to facilitate serialize/ deserialize operations
        /// </summary>
        internal Account() { }

        /// <summary>
        /// Creates a new account with the stated username and password.
        /// </summary>
        /// <param name="userName">The username to be used.</param>
        /// <param name="password">The password to be used.</param>
        public Account(string userName, string password)
        {
            UserName = userName;
            Password = GetPasswordHash(password);
            string path = GetPathForId(userName);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using (TextWriter stream = new StreamWriter(path + userName + ".aco"))
            {
                XmlSerializer ser = new XmlSerializer(typeof(Account));
                ser.Serialize(stream, this);
                _password = Password;
                Password = null;
            }
        }

        /// <summary>
        /// Loads an account with the userName provided.
        /// </summary>
        /// <param name="userName">The username to load.</param>
        /// <returns>The account, or null on error.</returns>
        public static Account LoadAccount(string userName)
        {
            string path = GetPathForId(userName);
            if (!Directory.Exists(path))
            {
                return null;
            }
            try
            {
                using (TextReader stream = new StreamReader(path + userName + ".aco"))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(Account));
                    Account ret = ser.Deserialize(stream) as Account;
                    ret._password = ret.Password;
                    ret.Password = null;
                    return ret;
                }
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Provides a blob-server-esque path for the account name suggested.
        /// </summary>
        /// <param name="userName">The user name for which to get the path</param>
        /// <returns>The fully-qualified path.</returns>
        private static string GetPathForId(string userName)
        {
            string extraPath = "";
            for(int c = 0; c< userName.Length; c++)
            {
                extraPath = extraPath + "\\" + userName.Substring(c, 1);
            }
            return Directory.GetCurrentDirectory() + extraPath + "\\";
        }

        /// <summary>
        /// Transforms the password into a hash, to make the saved string less-accessible than it would be otherwise.
        /// Presumably the machine itself would also be secure, and we're not going to do anything dumb like put these
        /// on a SQL table that enjoys code injections.
        /// 
        /// Portions of the code which intend to authenticate based on user input should still use public key/secure 
        /// key combinations to secure the sending, otherwise this is still vulnerable to a repeater attack.
        /// </summary>
        /// <param name="password">the password to be hashed</param>
        /// <returns>the hashed password</returns>
        private static string GetPasswordHash(string password)
        {
            HashAlgorithm alg = SHA256.Create();
            byte[] hashByte = alg.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder ret = new StringBuilder();
            foreach (byte b in hashByte)
                ret.Append(b.ToString("X2"));

            return ret.ToString();
        }

        /// <summary>
        /// Used to tell if given credentials are valid to use to log in.
        /// </summary>
        /// <param name="password">the password provded by the user</param>
        /// <returns>true if the credentials are valid</returns>
        public bool Authenticate(string password)
        {
            return _password == GetPasswordHash(password);
        }

        /// <summary>
        /// The user name
        /// </summary>
        [XmlAttribute]
        public string UserName;

        /// <summary>
        /// The field we temporarily hold hashed passwords in while serializing the object.
        /// </summary>
        [XmlAttribute]
        public string Password;

        /// <summary>
        /// The field we actually hold the password in during regular running. Kept in the private field
        /// as a little buffer against components which play fast and loose with account objects.
        /// </summary>
        [NonSerialized]
        private string _password;
    }
}
