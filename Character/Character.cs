using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;

namespace Mundasia.Objects
{
    public partial class Character
    {
        private static string delimiter = "|";
        private static char[] delim = new char[] { '|' };
        private static string dictDelimiter = "[";
        private static char[] dictDelim = new char[] { '[' };
        private static string keyDelimiter = "]";
        private static char[] keyDelim = new char[] { ']' };

        public DisplayCharacter CachedDisplay = null;

        public Character() { }

        public Character(string message)
        {
            string[] split = message.Split(delim);
            if (split.Length < 36) return;
            AccountName = split[0];
            CharacterName = split[1];
            uint.TryParse(split[2], out CharacterRace);
            int.TryParse(split[3], out Sex);
            uint.TryParse(split[4], out CharacterVirtue);
            uint.TryParse(split[5], out CharacterVice);
            uint.TryParse(split[6], out MoralsCare);
            uint.TryParse(split[7], out MoralsFairness);
            uint.TryParse(split[8], out MoralsLoyalty);
            uint.TryParse(split[9], out MoralsAuthority);
            uint.TryParse(split[10], out MoralsTradition);
            uint.TryParse(split[11], out CharacterProfession);
            uint.TryParse(split[12], out CharacterTalent);
            uint.TryParse(split[13], out CharacterHobby);
            string[] abilDict = split[14].Split(dictDelim);
            Abilities = new Dictionary<uint, int>();
            foreach(string keyVal in abilDict)
            {
                if (String.IsNullOrWhiteSpace(keyVal)) continue;
                string[] kv = keyVal.Split(keyDelim);
                uint key;
                int value;
                uint.TryParse(kv[0], out key);
                int.TryParse(kv[1], out value);
                Abilities.Add(key, value);
            }
            string[] skillDict = split[15].Split(dictDelim);
            Skills = new Dictionary<uint, int>();
            foreach (string keyVal in skillDict)
            {
                if (String.IsNullOrWhiteSpace(keyVal)) continue;
                string[] kv = keyVal.Split(keyDelim);
                uint key;
                int value;
                uint.TryParse(kv[0], out key);
                int.TryParse(kv[1], out value);
                Skills.Add(key, value);
            }
            uint.TryParse(split[16], out CurrentActionPoints);
            uint.TryParse(split[17], out MaxActionPoints);
            uint.TryParse(split[18], out DecayedActionPoints);
            uint.TryParse(split[19], out DestroyedActionPoints);
            uint.TryParse(split[20], out CurrentWillpowerPoints);
            uint.TryParse(split[21], out MaxWillpowerPoints);
            uint.TryParse(split[22], out DecayedWillpowerPoints);
            uint.TryParse(split[23], out DestroyedWillpowerPoints);
            uint.TryParse(split[24], out CurrentStun);
            uint.TryParse(split[25], out MaxStun);
            uint.TryParse(split[26], out DecayedStun);
            uint.TryParse(split[27], out DestroyedStun);
            uint.TryParse(split[28], out CurrentStructure);
            uint.TryParse(split[29], out MaxStructure);
            uint.TryParse(split[30], out DecayedStructure);
            uint.TryParse(split[31], out DestroyedStructure);
            uint.TryParse(split[32], out CurrentVital);
            uint.TryParse(split[33], out MaxVital);
            uint.TryParse(split[34], out DecayedVital);
            uint.TryParse(split[35], out DestroyedVital);
            uint.TryParse(split[36], out HairStyle);
            uint.TryParse(split[37], out HairColor);
            uint.TryParse(split[38], out SkinColor);
            int.TryParse(split[39], out LocationX);
            int.TryParse(split[40], out LocationY);
            int.TryParse(split[41], out LocationZ);
            Direction.TryParse(split[42], out LocationFacing);
            Map = split[43];
            string[] invDict = split[44].Split(dictDelim);
            Equipment = new Dictionary<int, InventoryItem>();
            Inventory = new List<InventoryItem>();
            foreach (string keyVal in invDict)
            {
                if (String.IsNullOrWhiteSpace(keyVal)) continue;
                InventoryItem it = new InventoryItem(keyVal);
                if (it.EquipKey > 0) Equipment.Add(it.EquipKey, it);
                else Inventory.Add(it);
            }
            bool.TryParse(split[45], out IsDM);
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder();
            str.Append(AccountName);
            str.Append(delimiter);
            str.Append(CharacterName);
            str.Append(delimiter);
            str.Append(CharacterRace);
            str.Append(delimiter);
            str.Append(Sex);
            str.Append(delimiter);
            str.Append(CharacterVirtue);
            str.Append(delimiter);
            str.Append(CharacterVice);
            str.Append(delimiter);
            str.Append(MoralsCare);
            str.Append(delimiter);
            str.Append(MoralsFairness);
            str.Append(delimiter);
            str.Append(MoralsLoyalty);
            str.Append(delimiter);
            str.Append(MoralsAuthority);
            str.Append(delimiter);
            str.Append(MoralsTradition);
            str.Append(delimiter);
            str.Append(CharacterProfession);
            str.Append(delimiter);
            str.Append(CharacterTalent);
            str.Append(delimiter);
            str.Append(CharacterHobby);
            str.Append(delimiter);
            foreach(KeyValuePair<uint, int> pair in Abilities)
            {
                str.Append(pair.Key);
                str.Append(keyDelimiter);
                str.Append(pair.Value);
                str.Append(dictDelimiter);
            }
            str.Append(delimiter);
            foreach (KeyValuePair<uint, int> pair in Skills)
            {
                str.Append(pair.Key);
                str.Append(keyDelimiter);
                str.Append(pair.Value);
                str.Append(dictDelimiter);
            }
            str.Append(delimiter);
            str.Append(CurrentActionPoints);
            str.Append(delimiter);
            str.Append(MaxActionPoints);
            str.Append(delimiter);
            str.Append(DecayedActionPoints);
            str.Append(delimiter);
            str.Append(DestroyedActionPoints);
            str.Append(delimiter);
            str.Append(CurrentWillpowerPoints);
            str.Append(delimiter);
            str.Append(MaxWillpowerPoints);
            str.Append(delimiter);
            str.Append(DecayedWillpowerPoints);
            str.Append(delimiter);
            str.Append(DestroyedWillpowerPoints);
            str.Append(delimiter);
            str.Append(CurrentStun);
            str.Append(delimiter);
            str.Append(MaxStun);
            str.Append(delimiter);
            str.Append(DecayedStun);
            str.Append(delimiter);
            str.Append(DestroyedStun);
            str.Append(delimiter);
            str.Append(CurrentStructure);
            str.Append(delimiter);
            str.Append(MaxStructure);
            str.Append(delimiter);
            str.Append(DecayedStructure);
            str.Append(delimiter);
            str.Append(DestroyedStructure);
            str.Append(delimiter);
            str.Append(CurrentVital);
            str.Append(delimiter);
            str.Append(MaxVital);
            str.Append(delimiter);
            str.Append(DecayedVital);
            str.Append(delimiter);
            str.Append(DestroyedVital);
            str.Append(delimiter);
            str.Append(HairStyle);
            str.Append(delimiter);
            str.Append(HairColor);
            str.Append(delimiter);
            str.Append(SkinColor);
            str.Append(delimiter);
            str.Append(LocationX);
            str.Append(delimiter);
            str.Append(LocationY);
            str.Append(delimiter);
            str.Append(LocationZ);
            str.Append(delimiter);
            str.Append(LocationFacing);
            str.Append(delimiter);
            str.Append(Map);
            str.Append(delimiter);
            if (Equipment != null)
            {
                foreach (InventoryItem it in Equipment.Values)
                {
                    str.Append(it.ToString());
                    str.Append(dictDelimiter);
                }
            }
            if (Inventory != null)
            {
                foreach (InventoryItem it in Inventory)
                {
                    str.Append(it.ToString());
                    str.Append(dictDelimiter);
                }
            }
            str.Append(delimiter);
            str.Append(IsDM);
            return str.ToString();
        }
        
        [XmlElement]
        public string AccountName;

        [XmlElement]
        public string CharacterName;

        [XmlElement]
        public uint CharacterRace;

        [XmlElement]
        public int Sex;

        [XmlElement]
        public uint CharacterVirtue;

        [XmlElement]
        public uint CharacterVice;

        [XmlElement]
        public uint MoralsCare;

        [XmlElement]
        public uint MoralsFairness;

        [XmlElement]
        public uint MoralsLoyalty;

        [XmlElement]
        public uint MoralsAuthority;

        [XmlElement]
        public uint MoralsTradition;

        [XmlElement]
        public uint CharacterProfession;

        [XmlElement]
        public uint CharacterTalent;

        [XmlElement]
        public uint CharacterHobby;

        [XmlArray]
        public Dictionary<uint, int> Abilities;

        [XmlArray]
        public Dictionary<uint, int> Skills;

        [XmlElement]
        public uint CurrentActionPoints;
        
        [XmlElement]
        public uint MaxActionPoints;

        [XmlElement]
        public uint DecayedActionPoints;

        [XmlElement]
        public uint DestroyedActionPoints;

        [XmlElement]
        public uint CurrentWillpowerPoints;

        [XmlElement]
        public uint MaxWillpowerPoints;

        [XmlElement]
        public uint DecayedWillpowerPoints;

        [XmlElement]
        public uint DestroyedWillpowerPoints;

        [XmlElement]
        public uint CurrentStun;

        [XmlElement]
        public uint MaxStun;

        [XmlElement]
        public uint DecayedStun;

        [XmlElement]
        public uint DestroyedStun;

        [XmlElement]
        public uint CurrentStructure;

        [XmlElement]
        public uint MaxStructure;

        [XmlElement]
        public uint DecayedStructure;

        [XmlElement]
        public uint DestroyedStructure;

        [XmlElement]
        public uint CurrentVital;

        [XmlElement]
        public uint MaxVital;

        [XmlElement]
        public uint DecayedVital;

        [XmlElement]
        public uint DestroyedVital;

        [XmlElement]
        public uint HairStyle;

        [XmlElement]
        public uint HairColor;

        [XmlElement]
        public uint SkinColor;

        [XmlElement]
        public string Map;

        [XmlElement]
        public int LocationX;

        [XmlElement]
        public int LocationY;

        [XmlElement]
        public int LocationZ;

        [XmlElement]
        public Direction LocationFacing;

        [XmlArray]
        public Dictionary<int, InventoryItem> Equipment;

        [XmlArray]
        public List<InventoryItem> Inventory;

        [XmlElement]
        public bool IsDM;

        public bool ValidateCharacter()
        {
            foreach(char ch in Path.GetInvalidFileNameChars())
            {
                if(CharacterName.Contains(ch))
                {
                    return false;
                }
            }
            if(CharacterVirtue == CharacterVice)
            {
                return false;
            }
            if(Sex != 0 && Sex != 1)
            {
                return false;
            }
            if(Race.GetRace(CharacterRace) == null)
            {
                return false;
            }
            if(Vice.GetVice(CharacterVice) == null)
            {
                return false;
            }
            if(Virtue.GetVirtue(CharacterVirtue) == null)
            {
                return false;
            }
            if(Authority.GetAuthority(MoralsAuthority) == null)
            {
                return false;
            }
            if(Care.GetCare(MoralsCare) == null)
            {
                return false;
            }
            if(Fairness.GetFairness(MoralsFairness) == null)
            {
                return false;
            }
            if(Loyalty.GetLoyalty(MoralsLoyalty) == null)
            {
                return false;
            }
            if(Tradition.GetTradition(MoralsTradition) == null)
            {
                return false;
            }
            if(Skill.GetSkill(CharacterHobby) == null)
            {
                return false;
            }
            if(Ability.GetAbility(CharacterTalent) == null)
            {
                return false;
            }
            Profession prof = Profession.GetProfession(CharacterProfession);
            if(prof == null)
            {
                return false;
            }
            if(Skill.GetSkill(prof.SkillOne) == null)
            {
                return false;
            }
            if(Skill.GetSkill(prof.SkillTwo) == null)
            {
                return false;
            }
            if(Skill.GetSkill(prof.SkillThree) == null)
            {
                return false;
            }

            Abilities = new Dictionary<uint, int>();
            Skills = new Dictionary<uint, int>();

            Race r = Race.GetRace(CharacterRace);
            Abilities.Add(0, r.Strength);
            Abilities.Add(1, r.Agility);
            Abilities.Add(2, r.Endurance);
            Abilities.Add(3, r.Perception);
            Abilities.Add(4, r.Quickness);
            Abilities.Add(5, r.Memory);
            Abilities.Add(6, r.Persuasion);
            Abilities.Add(7, r.Glibness);
            Abilities.Add(8, r.Appearance);
            Abilities.Add(9, r.Force);
            Abilities.Add(10, r.Control);
            Abilities.Add(11, r.Discipline);

            Abilities[prof.PrimaryAbility] = Abilities[prof.PrimaryAbility] + 1;
            Skills.Add(prof.SkillOne, 3);
            Skills.Add(prof.SkillTwo, 3);
            Skills.Add(prof.SkillThree, 3);

            if(Skills.ContainsKey(CharacterHobby))
            {
                Skills[CharacterHobby] = Skills[CharacterHobby] + 1;
            }
            else
            {
                Skills.Add(CharacterHobby, 2);
            }

            Abilities[CharacterTalent] = Abilities[CharacterTalent] + 1;

            CurrentActionPoints = 10;
            MaxActionPoints = 10;
            DecayedActionPoints = 0;
            DestroyedActionPoints = 0;
            CurrentWillpowerPoints = 10;
            MaxWillpowerPoints = 10;
            DecayedWillpowerPoints = 0;
            DestroyedWillpowerPoints = 0;
            CurrentStun = 10;
            MaxStun = 10;
            DecayedStun = 0;
            DestroyedStun = 0;
            CurrentStructure = 10;
            MaxStructure = 10;
            DecayedStructure = 0;
            DestroyedStructure = 0;
            CurrentVital = 10;
            MaxVital = 10;
            DecayedVital = 0;
            DestroyedVital = 0;

            LocationX = 20000000;
            LocationY = 20000000;
            LocationZ = 0;
            LocationFacing = Direction.North;
            Map = "Material";
            return true;
        }
    }
}
