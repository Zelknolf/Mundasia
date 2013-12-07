using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mundasia.Client
{
    public class Lore
    {
        public Lore(string fileLine)
        {
            string[] split = fileLine.Split(new char[] { '|' });
            uint index = 0;
            if (uint.TryParse(split[0], out index) &&
                !_library.ContainsKey(index))
            {
                Id = index;
                Name = split[1];
                Image = split[2];

                if (!uint.TryParse(split[3], out Description))
                {
                    Description = uint.MaxValue;
                }
            }
        }

        public string Name;
        public string Image;
        public uint Id;
        public uint Description;
        
        private static Dictionary<uint, Lore> _library = new Dictionary<uint, Lore>();

        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Lore.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    Lore toAdd = new Lore(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        public static Lore GetLore(uint index)
        {
            if (_library.ContainsKey(index))
            {
                return _library[index];
            }
            return null;
        }

        public static IEnumerable<Lore> GetLores()
        {
            return _library.Values;
        }
    }
}
