using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mundasia.Objects;

namespace Mundasia.Communication
{
    public class MoveRequest
    {
        private static string delimiter = "|";
        private static char[] delim = new char[] { '|' };

        public string AccountName;
        public string CharacterName;
        public int X;
        public int Y;
        public int Z;

        public MoveRequest() { }

        public MoveRequest(Character ch, int x, int y, int z)
        {
            AccountName = ch.AccountName;
            CharacterName = ch.CharacterName;
            X = x;
            Y = y;
            Z = z;
        }

        public MoveRequest(string fileLine)
        {
            string[] inputs = fileLine.Split(delim);
            if (inputs.Length < 5) return;
            AccountName = inputs[0];
            CharacterName = inputs[1];
            Int32.TryParse(inputs[2], out X);
            Int32.TryParse(inputs[3], out Y);
            Int32.TryParse(inputs[4], out Z);
        }

        public override string ToString()
        {
            StringBuilder st = new StringBuilder();
            st.Append(AccountName);
            st.Append(delimiter);
            st.Append(CharacterName);
            st.Append(delimiter);
            st.Append(X);
            st.Append(delimiter);
            st.Append(Y);
            st.Append(delimiter);
            st.Append(Z);
            return st.ToString();
        }

    }
}
