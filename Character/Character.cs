using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mundasia.Objects
{
    public class Character
    {
        public string AccountName;

        public string CharacterName;

        public int Race;

        public int Sex;

        public int Virtue;

        public int Vice;

        public int MoralsCare;
        public int MoralsFairness;
        public int MoralsLoyalty;
        public int MoralsAuthority;
        public int MoralsTradition;

        public int CurrentActionPoints;
        public int MaxActionPoints;
        public int DecayedActionPoints;
        public int DestroyedActionPoints;

        public int CurrentWillpowerPoints;
        public int MaxWillpowerPoints;
        public int DecayedWillpowerPoints;
        public int DestroyedWillpowerPoints;

        public int CurrentStun;
        public int MaxStun;
        public int DecayedStun;
        public int DestroyedStun;

        public int CurrentStructure;
        public int MaxStructure;
        public int DecayedStructure;
        public int DestroyedStructure;

        public int CurrentVital;
        public int MaxVital;
        public int DecayedVital;
        public int DestroyedVital;

        public int Profession;

        public Dictionary<int, int> Abilities;

        public Dictionary<int, int> SKills;
    }
}
