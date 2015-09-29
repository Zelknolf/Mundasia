using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Mundasia.Objects;
using Mundasia.Interface;

namespace TileTester
{  
    public partial class TileViewer : Form
    {
        List<Tile> drawnTiles = new List<Tile>();

        public static uint TestedTileset = 2;

        PlayScene scene;
        TileEditPane editPane;

        Map currentMap = new Map();

        int timeOfDay = 0;

        public TileViewer()
        {
            InitializeComponent();
            scene = new PlayScene();

            currentMap.Name = "World";

            editPane = new TileEditPane(null);
            editPane.Size = new Size(200, this.ClientRectangle.Height);
            editPane.Location = new Point(this.ClientRectangle.Width - 200, 0);
            editPane.X.ValueChanged += X_ValueChanged;
            editPane.Y.ValueChanged += Y_ValueChanged;
            editPane.Z.ValueChanged += Z_ValueChanged;
            editPane.Set.SelectedValueChanged += Set_SelectedValueChanged;
            editPane.TileHeight.SelectedValueChanged += TileHeight_SelectedValueChanged;
            editPane.TileDirection.SelectedValueChanged += TileDirection_SelectedValueChanged;
            editPane.AddTile.Click += AddTile_Click;
            editPane.RemoveTile.Click += RemoveTile_Click;
            this.Controls.Add(editPane);

            scene.Size = new Size(this.ClientRectangle.Width - 200, this.ClientRectangle.Height);
            scene.TileSelected += scene_TileSelected;
            scene.ViewCenterX = 19999990;
            scene.ViewCenterY = 20000000;
            scene.ViewCenterZ = 0;

            currentMap.LoadNearby(scene.ViewCenterX, scene.ViewCenterY, scene.ViewCenterZ);
            for(int X = scene.ViewCenterX - 40; X < scene.ViewCenterX + 40; X++)
            {
                if (!currentMap.Tiles.ContainsKey(X)) continue;
                for (int Y = scene.ViewCenterY - 40; Y < scene.ViewCenterY + 40; Y++)
                {
                    if (!currentMap.Tiles[X].ContainsKey(Y)) continue;
                    for (int Z = scene.ViewCenterZ - 40; Z < scene.ViewCenterZ + 40; Z++)
                    {
                        if (!currentMap.Tiles[X][Y].ContainsKey(Z)) continue;
                        drawnTiles.Add(currentMap.Tiles[X][Y][Z]);
                    }
                }
            }
            scene.Add(drawnTiles);

            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 20000002, y = 19999992, z = 8, Facing = Direction.North, CharacterRace = 3, Sex = 0, Hair = 1, SkinColor = 0, HairColor = 3, Clothes = 0, ClothColorA = 1, ClothColorB = 0 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 20000003, y = 19999992, z = 8, Facing = Direction.North, CharacterRace = 3, Sex = 0, Hair = 1, SkinColor = 0, HairColor = 3, Clothes = 1, ClothColorA = 1, ClothColorB = 0 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 20000004, y = 19999992, z = 8, Facing = Direction.North, CharacterRace = 3, Sex = 0, Hair = 1, SkinColor = 0, HairColor = 3, Clothes = 2, ClothColorA = 1, ClothColorB = 0 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 20000005, y = 19999992, z = 8, Facing = Direction.North, CharacterRace = 3, Sex = 0, Hair = 1, SkinColor = 0, HairColor = 3, Clothes = 3, ClothColorA = 1, ClothColorB = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 6, y = -6, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 2, SkinColor = 0, HairColor = 4 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 5, y = -6, z = 0, Facing = Direction.North, Race = 1, Sex = 0, Hair = 2, SkinColor = 0, HairColor = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 4, y = -6, z = 0, Facing = Direction.North, Race = 1, Sex = 1, Hair = 2, SkinColor = 0, HairColor = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 3, x = 3, y = -6, z = 0, Facing = Direction.North, Race = 2, Sex = 0, Hair = 1, SkinColor = 0, HairColor = 2 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 3, x = 2, y = -6, z = 0, Facing = Direction.North, Race = 2, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 2 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 1, y = -6, z = 0, Facing = Direction.North, Race = 3, Sex = 0, Hair = 1, SkinColor = 0, HairColor = 3 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 0, y = -6, z = 0, Facing = Direction.North, Race = 3, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 3 });

            // Uncomment to check for clipping
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 7, y = -5, z = 0, Facing = Direction.South, Race = 0, Sex = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 6, y = -5, z = 0, Facing = Direction.South, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 5, y = -5, z = 0, Facing = Direction.South, Race = 1, Sex = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 4, y = -5, z = 0, Facing = Direction.South, Race = 1, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 3, x = 3, y = -5, z = 0, Facing = Direction.South, Race = 2, Sex = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 3, x = 2, y = -5, z = 0, Facing = Direction.South, Race = 2, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 1, y = -5, z = 0, Facing = Direction.South, Race = 3, Sex = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 0, y = -5, z = 0, Facing = Direction.South, Race = 3, Sex = 1 });

            // Uncomment to check for standing in formation.
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 7, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 6, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 5, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 4, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 3, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 2, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 1, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 0, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 7, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 6, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 5, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 4, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 3, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 2, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 1, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 0, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 0 });

            this.Controls.Add(scene);

            this.Resize += new EventHandler(TileViewer_Resize);
        }

        void RemoveTile_Click(object sender, EventArgs e)
        {
            if (editPane.shownTile == null) return;
            if (currentMap.Remove(editPane.shownTile))
            {
                editPane.shownTile.Delete(currentMap.Name);
                scene.Remove(editPane.shownTile);
            }
            else
            {
                MessageBox.Show(String.Format("Could not remove the tile at ({0},{1},{2})", editPane.shownTile.PosX, editPane.shownTile.PosY, editPane.shownTile.PosZ));
            }
        }

        void AddTile_Click(object sender, EventArgs e)
        {
            Tile newTile = null;
            if (editPane.shownTile == null)
            {
                newTile = new Tile(0, Direction.DirectionLess, 1, 20000000, 20000000, 0);
            }
            else
            {
                newTile = new Tile(editPane.shownTile.CurrentTileSet, editPane.shownTile.Slope, editPane.shownTile.TileHeight, editPane.shownTile.PosX, editPane.shownTile.PosY, editPane.shownTile.PosZ + editPane.shownTile.TileHeight);
            }
            if (currentMap.Add(newTile))
            {
                newTile.Save(currentMap.Name);
                scene.Add(newTile);
                editPane.SetTile(newTile);
            }
            else
            {
                MessageBox.Show(String.Format("Could not add the tile at ({0},{1},{2})", editPane.shownTile.PosX, editPane.shownTile.PosY, editPane.shownTile.PosZ));
            }
        }

        void TileDirection_SelectedValueChanged(object sender, EventArgs e)
        {
            if (editPane.SettingNewTile) return;
            if (editPane.shownTile == null) return;
            Direction dir;
            if(Enum.TryParse<Direction>(editPane.TileDirection.SelectedItem.ToString(), out dir))
            {
                Tile replacement = new Tile(editPane.shownTile.CurrentTileSet, dir, editPane.shownTile.TileHeight, editPane.shownTile.PosX, editPane.shownTile.PosY, editPane.shownTile.PosZ);
                if (SwapTiles(editPane.shownTile, replacement))
                {
                    editPane.SetTile(replacement);
                }
            }
        }

        void TileHeight_SelectedValueChanged(object sender, EventArgs e)
        {
            if (editPane.SettingNewTile) return;
            if (editPane.shownTile == null) return;
            int height = 0;
            int.TryParse(editPane.TileHeight.SelectedItem.ToString(), out height);
            if(height > 0)
            {
                Tile replacement = new Tile(editPane.shownTile.CurrentTileSet, editPane.shownTile.Slope, height, editPane.shownTile.PosX, editPane.shownTile.PosY, editPane.shownTile.PosZ);
                if (SwapTiles(editPane.shownTile, replacement))
                {
                    editPane.SetTile(replacement);
                }
            }
        }

        void Set_SelectedValueChanged(object sender, EventArgs e)
        {
            if (editPane.SettingNewTile) return;
            if (editPane.shownTile == null) return;
            uint nTile = 0;
            foreach(TileSet ts in TileSet.GetSets())
            {
                if(editPane.Set.SelectedItem.ToString() == ts.Name)
                {
                    nTile = ts.Id;
                    break;
                }
            }
            Tile replacement = new Tile(nTile, editPane.shownTile.Slope, editPane.shownTile.TileHeight, editPane.shownTile.PosX, editPane.shownTile.PosY, editPane.shownTile.PosZ);
            if (SwapTiles(editPane.shownTile, replacement))
            {
                editPane.SetTile(replacement);
            }
        }

        void Z_ValueChanged(object sender, EventArgs e)
        {
            if (editPane.SettingNewTile) return;
            if (editPane.shownTile == null) return;
            Tile replacement = new Tile(editPane.shownTile.CurrentTileSet, editPane.shownTile.Slope, editPane.shownTile.TileHeight, editPane.shownTile.PosX, editPane.shownTile.PosY, (int)editPane.Z.Value);
            if (SwapTiles(editPane.shownTile, replacement))
            {
                editPane.SetTile(replacement);
            }
        }

        void Y_ValueChanged(object sender, EventArgs e)
        {
            if (editPane.SettingNewTile) return;
            if (editPane.shownTile == null) return;
            Tile replacement = new Tile(editPane.shownTile.CurrentTileSet, editPane.shownTile.Slope, editPane.shownTile.TileHeight, editPane.shownTile.PosX, (int)editPane.Y.Value, editPane.shownTile.PosZ);
            if (SwapTiles(editPane.shownTile, replacement))
            {
                editPane.SetTile(replacement);
            }
        }

        void X_ValueChanged(object sender, EventArgs e)
        {
            if (editPane.SettingNewTile) return;
            if (editPane.shownTile == null) return; 
            Tile replacement = new Tile(editPane.shownTile.CurrentTileSet, editPane.shownTile.Slope, editPane.shownTile.TileHeight, (int)editPane.X.Value, editPane.shownTile.PosY, editPane.shownTile.PosZ);
            if (SwapTiles(editPane.shownTile, replacement))
            {
                editPane.SetTile(replacement);
            }
        }

        void scene_TileSelected(object Sender, TileSelectEventArgs e)
        {
            editPane.SetTile(e.tile);
        }

        void TileViewer_Resize(object sender, EventArgs e)
        {
            scene.Size = this.Size;
        }

        bool SwapTiles(Tile toRemove, Tile toAdd)
        {
            if(!currentMap.Remove(toRemove))
            {
                return false;
            }
            if(!currentMap.Add(toAdd))
            {
                currentMap.Add(toRemove);
                return false;
            }
            toRemove.Delete(currentMap.Name);
            toAdd.Save(currentMap.Name);
            scene.Remove(toRemove);
            scene.Add(toAdd);
            return true;
        }
    }
}
