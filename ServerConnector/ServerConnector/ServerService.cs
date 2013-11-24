using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Security.Cryptography;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

using Mundasia.Communication;
using Mundasia.Objects;


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

        public string CreateCharacter(string message)
        {
            CharacterCreation nChar = new CharacterCreation(message);
            Account targetAccount = Account.LoadAccount(nChar.UserId);
            if(targetAccount == null)
            {
                return "Invalid account";
            }
            if (targetAccount.SessionId != nChar.SessionId)
            {
                return "Invalid session Id";
            }
            Character chr;
            try
            {
                chr = new Character()
                {
                    AccountName = targetAccount.UserName,
                    CharacterHobby = (uint)nChar.Hobby,
                    CharacterName = nChar.Name,
                    CharacterProfession = (uint)nChar.Profession,
                    CharacterRace = (uint)nChar.Race,
                    CharacterTalent = (uint)nChar.Talent,
                    CharacterVice = (uint)nChar.Vice,
                    CharacterVirtue = (uint)nChar.Virtue,
                    MoralsAuthority = (uint)nChar.Authority,
                    MoralsCare = (uint)nChar.Authority,
                    MoralsFairness = (uint)nChar.Fairness,
                    MoralsLoyalty = (uint)nChar.Loyalty,
                    MoralsTradition = (uint)nChar.Tradition,
                    Sex = nChar.Sex,
                };
            }
            catch
            {
                return "Invalid character data; likely a negative number passed to an unsigned field.";
            }
            if (!chr.ValidateCharacter())
            {
                return "Character violates rules of character creation.";
            }
            if(targetAccount.LoadCharacter(chr.CharacterName) != null)
            {
                return "A character with that name already exists";
            }
            if (!targetAccount.NewCharacter(chr))
            {
                return "Unable to save character";
            }
            return "Success: " + chr.CharacterName;
        }
    }
}
