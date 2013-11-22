using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mundasia.Objects
{
    public class Vice
    {
        public Vice(string fileLine)
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


        public static Dictionary<uint, Vice> _library = new Dictionary<uint, Vice>();

        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Vices.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    Vice toAdd = new Vice(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        public static Vice GetVice(uint index)
        {
            if(_library.ContainsKey(index))
            {
                return _library[index];
            }
            return null;
        }

        public static IEnumerable<Vice> GetVices()
        {
            return _library.Values;
        }
    }
}
