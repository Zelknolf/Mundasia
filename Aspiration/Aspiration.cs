using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mundasia.Objects
{
    public class Aspiration
    {
        public Aspiration(string fileLine)
        {
            Skills = new List<uint>();
            Abilities = new List<uint>();

            string[] split = fileLine.Split(new char[] { '|' });
            uint.TryParse(split[0], out Id);
            Name = split[1];
            uint holder;
            uint.TryParse(split[2], out Description);
            if(uint.TryParse(split[3], out holder))
            {
                Abilities.Add(holder);
            }
            if (uint.TryParse(split[4], out holder))
            {
                Abilities.Add(holder);
            }
            if (uint.TryParse(split[5], out holder))
            {
                Abilities.Add(holder);
            }
            if (uint.TryParse(split[6], out holder))
            {
                Skills.Add(holder);
            }
            if (uint.TryParse(split[7], out holder))
            {
                Skills.Add(holder);
            }
            if (uint.TryParse(split[8], out holder))
            {
                Skills.Add(holder);
            }
            if (uint.TryParse(split[9], out holder))
            {
                Skills.Add(holder);
            }
            if (uint.TryParse(split[10], out holder))
            {
                Skills.Add(holder);
            }
            if (uint.TryParse(split[11], out holder))
            {
                Skills.Add(holder);
            }
            if (uint.TryParse(split[12], out holder))
            {
                Skills.Add(holder);
            }
        }

        public string Name;
        public uint Id;
        public uint Description;
        public List<uint> Skills;
        public List<uint> Abilities;

        private static Dictionary<uint, Aspiration> _library = new Dictionary<uint, Aspiration>();

        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Aspirations.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib, Encoding.UTF7))
            {
                while (read.Peek() >= 0)
                {
                    Aspiration toAdd = new Aspiration(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        public static Aspiration GetAspiration(uint id)
        {
            if (_library.ContainsKey(id))
            {
                return _library[id];
            }
            return null;
        }

        public static IEnumerable<Aspiration> GetAspirations()
        {
            return _library.Values;
        }
    }
}
