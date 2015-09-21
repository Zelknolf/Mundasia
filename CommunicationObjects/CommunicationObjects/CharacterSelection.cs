﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Mundasia.Objects;

namespace Mundasia.Communication
{
    public class CharacterSelection
    {
        private static string delimiter = "^";
        private static char[] delim = new char[] { '^' };

        public Character CentralCharacter;
        public List<Tile> visibleTiles;
        public List<DisplayCharacter> visibleCharacters;

        public CharacterSelection(Character character)
        {
            if (!Map.LoadedMaps.ContainsKey(character.Map)) { Map.LoadedMaps.Add(character.Map, new Map() { Name = character.Map }); }
            Map currentMap = Map.LoadedMaps[character.Map];
            visibleTiles = currentMap.GetNearby(character.LocationX, character.LocationY, character.LocationZ);
            visibleCharacters = currentMap.GetNearbyCharacters(character.LocationX, character.LocationY, character.LocationZ);
            CentralCharacter = character;
            
            // We need to check if this character is already present just in case this is a character
            // relogging.
            if(!currentMap.PresentCharacters.Contains(character))
            {
                currentMap.PresentCharacters.Add(character);
            }
        }
        
        public CharacterSelection(string builder)
        {
            string[] pieces = builder.Split(delim);
            visibleTiles = Tile.TileCollectionFromString(pieces[0]);
            CentralCharacter = new Character(pieces[1]);
            visibleCharacters = DisplayCharacter.DisplayCharacterCollectionFromString(pieces[2]);
        }

        public override string ToString()
        {
            StringBuilder str = new StringBuilder().Append(Tile.TileCollectionToString(visibleTiles));
            str.Append(delimiter);
            str.Append(CentralCharacter.ToString());
            str.Append(delimiter);
            str.Append(DisplayCharacter.DisplayCharacterCollectionToString(visibleCharacters));
            str.Append(delimiter);
            return str.ToString();
        }
    }
}
