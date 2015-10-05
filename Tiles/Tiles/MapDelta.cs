using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Objects
{
    public class MapDelta
    {
        public Dictionary<int, List<Tile>> AddedTiles = new Dictionary<int, List<Tile>>();

        public Dictionary<int, List<Tile>> RemovedTiles = new Dictionary<int, List<Tile>>();

        public Dictionary<int, DisplayCharacter> ChangedCharacters = new Dictionary<int, DisplayCharacter>();

        public Dictionary<int, DisplayCharacter> AddedCharacters = new Dictionary<int, DisplayCharacter>();

        public Dictionary<int, DisplayCharacter> RemovedCharacters = new Dictionary<int, DisplayCharacter>();
    }
}
