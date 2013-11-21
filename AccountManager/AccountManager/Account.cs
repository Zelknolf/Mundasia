using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Timers;

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
        /// <param name="password">The hash of the password to be used.</param>
        public Account(string userName, string password)
        {
            UserName = userName;
            Password = password;
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
            _lastAccessed = DateTime.UtcNow;
            _cachedAccounts.Add(userName, this);
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
        private static string GetSha256Hash(string password)
        {
            HashAlgorithm alg = SHA256.Create();
            byte[] hashByte = alg.ComputeHash(Encoding.ASCII.GetBytes(password));
            StringBuilder ret = new StringBuilder();
            foreach (byte b in hashByte)
                ret.Append(b.ToString("X2"));

            return ret.ToString();
        }

        /// <summary>
        /// Loads an account with the userName provided.
        /// </summary>
        /// <param name="userName">The username to load.</param>
        /// <returns>The account, or null on error.</returns>
        public static Account LoadAccount(string userName)
        {
            if (_cachedAccounts.Keys.Contains(userName)) return _cachedAccounts[userName];

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
                    ret._lastAccessed = DateTime.UtcNow;
                    _cachedAccounts.Add(userName, ret);
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
        /// Used to tell if given credentials are valid to use to log in.
        /// </summary>
        /// <param name="password">the password as sent by the user (expecting to be a hash of the sessionId and password</param>
        /// <returns>true if the credentials are valid</returns>
        public bool Authenticate(string remotePass)
        {
            _lastAccessed = DateTime.UtcNow;
            string localPass = GetSha256Hash(_password + UserName + DateTime.UtcNow.ToShortDateString());
            if (remotePass == localPass)
                return true;
            return false;
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
        /// Defines the amount of time, in minutes, that an account can sit idle before the
        /// cleanup loop will determine it to be lost and unload it.
        /// </summary>
        [NonSerialized]
        public const int AccountTimeout = 2;

        /// <summary>
        /// The field we actually hold the password in during regular running. Kept in the private field
        /// as a little buffer against components which play fast and loose with account objects.
        /// </summary>
        [NonSerialized]
        private string _password;

        /// <summary>
        /// This field is used to keep track of the last time this account accessed
        /// </summary>
        [NonSerialized]
        private DateTime _lastAccessed;

        /// <summary>
        /// This field is used to identify the session by number.
        /// </summary>
        [NonSerialized]
        public int SessionId;

        /// <summary>
        /// A static cache of accounts that we've used recently, indexed by account name for 
        /// quicker response in case of needing reauthentication.
        /// </summary>
        [NonSerialized]
        private static Dictionary<string, Account> _cachedAccounts = new Dictionary<string, Account>();

        /// <summary>
        /// A static cache of accounts which are currently logged in, indexed by session id for
        /// quicker response during the use of regular commands.
        /// </summary>
        [NonSerialized]
        public static Dictionary<int, Account> ActiveSessions = new Dictionary<int, Account>();

        /// <summary>
        /// Holds the last session Id assigned.
        /// </summary>
        [NonSerialized]
        private static int _lastId = 1;

        /// <summary>
        /// Timer that slowly checks on active accounts, and cleans up ones that have been stale too long.
        /// </summary>
        private static Timer _sessionEnder = new Timer();

        public static int GetSessionId(string userId)
        {
            Account loadedAccount = LoadAccount(userId);
            if (loadedAccount != null)
            {
                if (ActiveSessions.ContainsKey(loadedAccount.SessionId) &&
                    ActiveSessions[loadedAccount.SessionId] == loadedAccount)
                {
                    // Player has relogged; clear old session data.
                    ActiveSessions.Remove(loadedAccount.SessionId);
                }
                loadedAccount.SessionId = _lastId;
                _lastId++;
                ActiveSessions.Add(loadedAccount.SessionId, loadedAccount);
                return loadedAccount.SessionId;
            }
            return -1;
        }

        
        /// <summary>
        /// Establishes the events needed for the regular check of the event checker, so it can
        /// remove the caching of accounts which haven't been used recently.
        /// </summary>
        private static void StartSessionEnder()
        {
            // Don't restart the session ender if it's already running.
            if (_sessionEnder.Enabled) { return; }

            _sessionEnder.AutoReset = true;
            _sessionEnder.Interval = 60 * 1000; // once a minute

            _sessionEnder.Elapsed += new ElapsedEventHandler(_sessionEnder_Elapsed);

            _sessionEnder.Start();
        }

        /// <summary>
        /// Method which is called when _sessionEnder elapses.
        /// </summary>
        private static void _sessionEnder_Elapsed(object sender, ElapsedEventArgs e)
        {
            List<string> toRemove = new List<string>();
            foreach (KeyValuePair<string, Account> account in _cachedAccounts)
            {
                if (account.Value._lastAccessed == null)
                {
                    // Must've snuck by somehow. We'll start the count now.
                    account.Value._lastAccessed = DateTime.UtcNow;
                    continue;
                }
                if (DateTime.UtcNow - account.Value._lastAccessed > TimeSpan.FromMinutes(AccountTimeout))
                {
                    toRemove.Add(account.Key);
                    ActiveSessions.Remove(account.Value.SessionId);
                }
            }
            foreach (string rem in toRemove)
            {
                _cachedAccounts.Remove(rem);
            }
        }
    }
}
