using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mundasia.Objects
{
    public class Authority
    {
        public Authority(string fileLine)
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

        private static Dictionary<uint, Authority> _library = new Dictionary<uint, Authority>();

        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\MoralsAuthority.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    Authority toAdd = new Authority(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        public static Authority GetAuthority(uint index)
        {
            if(_library.ContainsKey(index))
            {
                return _library[index];
            }
            return null;
        }

        public static IEnumerable<Authority> GetAuthorities()
        {
            return _library.Values;
        }
    }
}
