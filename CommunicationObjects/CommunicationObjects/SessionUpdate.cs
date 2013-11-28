﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Communication
{
    public class SessionUpdate
    {
        private static string delimiter = "|";
        private static char[] delim = new char[] { '|' };

        public SessionUpdate() { }

        public SessionUpdate(string message)
        {
            string[] split = message.Split(delim);
            UserId = split[0];
            if(!int.TryParse(split[1], out SessionId))
            {
                SessionId = int.MaxValue;
            }
        }

        public string UserId;
        public int SessionId;

        public override string ToString()
        {
            return UserId + delimiter + SessionId;
        }
    }
}
