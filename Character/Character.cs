using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Xml.Serialization;

namespace Mundasia.Objects
{
    public class Character
    {       
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
            return true;
        }
    }
}
