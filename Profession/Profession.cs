using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mundasia.Objects
{
    public class Profession
    {
        public Profession(string fileLine)
        {
            string[] split = fileLine.Split(new char[] { '|' });
            uint index = 0;
            if (uint.TryParse(split[0], out index) &&
                !_library.ContainsKey(index))
            {
                Id = index;
                Name = split[1];

                if (!uint.TryParse(split[2], out Description))
                {
                    Description = uint.MaxValue;
                }
                if (!uint.TryParse(split[3], out PrimaryAbility))
                {
                    PrimaryAbility = uint.MaxValue;
                }
                if (!uint.TryParse(split[4], out SkillOne))
                {
                    SkillOne = uint.MaxValue;
                }
                if (!uint.TryParse(split[5], out SkillTwo))
                {
                    SkillTwo = uint.MaxValue;
                }
                if (!uint.TryParse(split[6], out SkillThree))
                {
                    SkillThree = uint.MaxValue;
                }
            }
        }

        public uint Id;
        public uint Description;
        public string Name;

        public uint PrimaryAbility;
        public uint SkillOne;
        public uint SkillTwo;
        public uint SkillThree;

        private static Dictionary<uint, Profession> _library = new Dictionary<uint, Profession>();

        public static void Load()
        {
            string file = System.IO.Directory.GetCurrentDirectory() + "\\DataArrays\\Professions.txt";
            FileStream strLib = File.Open(file, FileMode.Open);
            using (StreamReader read = new StreamReader(strLib))
            {
                while (read.Peek() >= 0)
                {
                    Profession toAdd = new Profession(read.ReadLine());
                    _library.Add(toAdd.Id, toAdd);
                }
            }
        }

        public static Profession GetProfession(uint index)
        {
            if(_library.ContainsKey(index))
            {
                return _library[index];
            }
            return null;
        }

        public static IEnumerable<Profession> GetProfessions()
        {
            return _library.Values;
        }
    }
}
