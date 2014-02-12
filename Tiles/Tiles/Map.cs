﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mundasia.Objects
{
    public class Map
    {
        /// <summary>
        /// A collection of all of the tiles in the area. Tiles appear as Tiles[X][Y][Z].
        /// </summary>
        public Dictionary<int, Dictionary<int, Dictionary<int, Tile>>> Tiles = new Dictionary<int, Dictionary<int, Dictionary<int, Tile>>>();

        /// <summary>
        /// Storage of whether or not a given X-Y-Z/1000 coordinate has had its stack loaded.
        /// (all Z coordinates from 0-999 are "0" in this dictionary, per Tile.LoadStack's behavior.)
        /// </summary>
        public Dictionary<int, Dictionary<int, Dictionary<int, bool>>> TilesLoaded = new Dictionary<int, Dictionary<int, Dictionary<int, bool>>>();

        /// <summary>
        /// The name of the map being built.
        /// </summary>
        public string Name;

        /// <summary>
        /// Add a tile to this map.
        /// </summary>
        /// <param name="ToAdd">The tile to add</param>
        /// <returns>true if the tile is added; false on error</returns>
        public bool Add(Tile ToAdd)
        {
            if(ToAdd.PosX > 40000000 || // 40,000 km. Roughly the circumference of the Earth.
               ToAdd.PosX < 0 ||        // -1 = 40000000
               ToAdd.PosY > 40000000 ||
               ToAdd.PosY < 0 ||
               ToAdd.PosZ < -50000 ||   // thickness of the crust
               ToAdd.PosZ > 8000)       // atmosphere too thin to support life
            {
                return false;
            }
            int tileHeight = ToAdd.TileHeight;
            while (tileHeight > 1)
            {
                tileHeight--;
                if (GetTileExact(ToAdd.PosX, ToAdd.PosY, ToAdd.PosZ - tileHeight) != null)
                {
                    return false;
                }
            }
            if(GetTileOverlap(ToAdd.PosX, ToAdd.PosY, ToAdd.PosZ) != null)
            {
                return false;
            }
            if(!Tiles.ContainsKey(ToAdd.PosX))
            {
                Tiles.Add(ToAdd.PosX, new Dictionary<int, Dictionary<int, Tile>>());
            }
            if(!Tiles[ToAdd.PosX].ContainsKey(ToAdd.PosY))
            {
                Tiles[ToAdd.PosX].Add(ToAdd.PosY, new Dictionary<int, Tile>());
            }
            if(!Tiles[ToAdd.PosX][ToAdd.PosY].ContainsKey(ToAdd.PosZ))
            {
                Tiles[ToAdd.PosX][ToAdd.PosY].Add(ToAdd.PosZ, ToAdd);
                return true;
            }
            else
            {
                Tiles[ToAdd.PosX][ToAdd.PosY][ToAdd.PosZ] = null;
                return true;
            }
        }

        /// <summary>
        /// Remove a tile from this map.
        /// </summary>
        /// <param name="ToRemove">The tile to remove</param>
        /// <returns>True on success; false if the tile is not found</returns>
        public bool Remove(Tile ToRemove)
        {
            if(!Tiles.ContainsKey(ToRemove.PosX))
            {
                return false;
            }
            if(!Tiles[ToRemove.PosX].ContainsKey(ToRemove.PosY))
            {
                return false;
            }
            if(!Tiles[ToRemove.PosX][ToRemove.PosY].ContainsKey(ToRemove.PosZ))
            {
                return false;
            }
            Tiles[ToRemove.PosX][ToRemove.PosY].Remove(ToRemove.PosZ);
            if(Tiles[ToRemove.PosX][ToRemove.PosY].Count == 0)
            {
                Tiles[ToRemove.PosX].Remove(ToRemove.PosY);
            }
            if(Tiles[ToRemove.PosX].Count == 0)
            {
                Tiles.Remove(ToRemove.PosX);
            }
            return true;
        }

        /// <summary>
        /// Finds a tile that is placed exactly at the X/Y/Z coordinates provided.
        /// </summary>
        /// <param name="X">The X coordinate</param>
        /// <param name="Y">The Y coordinate</param>
        /// <param name="Z">The Z coordinate</param>
        /// <returns>The tile, or null if one isn't found.</returns>
        public Tile GetTileExact(int X, int Y, int Z)
        {
            if (!Tiles.ContainsKey(X))
            {
                return null;
            }
            if (!Tiles[X].ContainsKey(Y))
            {
                return null;
            }
            if (Tiles[X][Y].ContainsKey(Z))
            {
                return Tiles[X][Y][Z];
            }
            return null;
        }

        /// <summary>
        /// Returns any tile that overlaps the given coordinates
        /// </summary>
        /// <param name="X">The X coordinate of the tile</param>
        /// <param name="Y">The Y coordinate of the tile</param>
        /// <param name="Z">The Z coordinate of the tile</param>
        /// <returns>The overlapping tile, or null if none.</returns>
        public Tile GetTileOverlap(int X, int Y, int Z)
        {
            Tile exact = GetTileExact(X, Y, Z);
            if (exact != null) return exact;
            if (!Tiles.ContainsKey(X)) return null;
            if (!Tiles[X].ContainsKey(Y)) return null;
            if (!Tiles[X][Y].ContainsKey(Z)) return null;
            if(Tiles[X][Y].ContainsKey(Z+1))
            {
                Tile toCheck = Tiles[X][Y][Z + 1];
                if(toCheck.TileHeight > 1)
                {
                    return toCheck;
                }
            }
            if (Tiles[X][Y].ContainsKey(Z + 2))
            {
                Tile toCheck = Tiles[X][Y][Z + 2];
                if (toCheck.TileHeight > 2)
                {
                    return toCheck;
                }
            }
            if (Tiles[X][Y].ContainsKey(Z + 3))
            {
                Tile toCheck = Tiles[X][Y][Z + 3];
                if (toCheck.TileHeight > 3)
                {
                    return toCheck;
                }
            }
            return null;
        }

        /// <summary>
        /// This method will load all of the tiles within 40 horizontal tiles of the center point, and within the 1000-tall block of tiles.
        /// </summary>
        /// <param name="X">The X coordinate at the center of the loading.</param>
        /// <param name="Y">The Y coordinate at the center of the loading.</param>
        /// <param name="Z">The Z coordinate at the center of the loading.</param>
        /// <returns></returns>
        public void LoadNearby(int X, int Y, int Z)
        {
            for (int c = X - 40; c < X + 40; c++)
            {
                for(int cc = Y - 40; cc < Y + 40; cc++)
                {
                    for(int ccc = Z - 40; ccc < Z + 40; ccc++)
                    {
                        if (!TilesLoaded.ContainsKey(c) ||
                            !TilesLoaded[c].ContainsKey(cc) ||
                            !TilesLoaded[c][cc].ContainsKey(ccc) ||
                            !TilesLoaded[c][cc][ccc])
                        {
                            Tile nearby = Tile.Load(c, cc, ccc, Name);
                            if (nearby != null)
                            {
                                if (Tiles.ContainsKey(c) &&
                                    Tiles[c].ContainsKey(cc) &&
                                    Tiles[c][cc].ContainsKey(nearby.PosZ))
                                {
                                    continue;
                                }
                                if (!Tiles.ContainsKey(c))
                                {
                                    Tiles.Add(c, new Dictionary<int, Dictionary<int, Tile>>());
                                }
                                if (!Tiles[c].ContainsKey(cc))
                                {
                                    Tiles[c].Add(cc, new Dictionary<int, Tile>());
                                }
                                Tiles[c][cc].Add(nearby.PosZ, nearby);
                            }
                            if (!TilesLoaded.ContainsKey(c))
                            {
                                TilesLoaded.Add(c, new Dictionary<int, Dictionary<int, bool>>());
                            }
                            if (!TilesLoaded[c].ContainsKey(cc))
                            {
                                TilesLoaded[c].Add(cc, new Dictionary<int, bool>());
                            }
                            if (!TilesLoaded[c][cc].ContainsKey(ccc))
                            {
                                TilesLoaded[c][cc].Add(ccc, true);
                            }
                            else
                            {
                                TilesLoaded[c][cc][ccc] = true;
                            }
                        }
                    }
                }
            }
        }

        public List<Tile> GetNearby(int X, int Y, int Z)
        {
            LoadNearby(X, Y, Z);
            List<Tile> ret = new List<Tile>();
            for (int c = X - 40; c < X + 40; c++)
            {
                for (int cc = Y - 40; cc < Y + 40; cc++)
                {
                    // Because Tile.LoadStack will provide an empty collection if there's nothing to load, we can assume
                    // that LoadNearby will have put tiles into all of these places.
                    foreach(Tile tile in Tiles[X][Y].Values)
                    {
                        if(Math.Abs(tile.PosZ - Z) < 1000)
                        {
                            ret.Add(tile);
                        }
                    }
                }
            }
            return ret;
        }
    }
}
