using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mundasia.Objects
{
    public class Care
    {
        public Care(string fileLine)
        {
            string[] split = fileLine.Split(new char[] { '|' });
            uint index = 0;
            if (uint.TryParse(split[0], out index) &&
                !_library.ContainsKey(index))
            {
                Id = index;
                Name = split[1];

                if(!uint.TryParse(split[2], out Description))
                {
                    Description = uint.MaxValue;
                }
            }
        }

        public uint Id;
        public uint Description;
        public string Name;

        private static Dictionary<uint, Care> _library = new Dictionary<uint, Care>();

        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\MoralsCare.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib))
            {
                while (read.Peek() >= 0)
                {
                    Care toAdd = new Care(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        public static Care GetCare(uint index)
        {
            if(_library.ContainsKey(index))
            {
                return _library[index];
            }
            return null;
        }

        public static IEnumerable<Care> GetCares()
        {
            return _library.Values;
        }
    }
}
