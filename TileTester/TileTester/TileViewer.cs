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

        Direction topDirection = Direction.NorthWest;
        PlayScene scene;
        TileEditPane editPane;

        int timeOfDay = 0;

        public TileViewer()
        {
            InitializeComponent();
            scene = new PlayScene();

            // Build four little pyramids to show slopes and blocks of every height
            drawnTiles.Add(new Tile(TestedTileset, Direction.DirectionLess, 4, 5, 5, 4));
            drawnTiles.Add(new Tile(TestedTileset, Direction.DirectionLess, 3, 6, 5, 3));
            drawnTiles.Add(new Tile(TestedTileset, Direction.NorthEast, 4, 6, 6, 4));
            drawnTiles.Add(new Tile(TestedTileset, Direction.North, 4, 5, 6, 4));
            drawnTiles.Add(new Tile(TestedTileset, Direction.NorthWest, 4, 4, 6, 4));
            drawnTiles.Add(new Tile(TestedTileset, Direction.West, 4, 4, 5, 4));
            drawnTiles.Add(new Tile(TestedTileset, Direction.SouthWest, 4, 4, 4, 4));
            drawnTiles.Add(new Tile(TestedTileset, Direction.South, 4, 5, 4, 4));
            drawnTiles.Add(new Tile(TestedTileset, Direction.SouthEast, 4, 6, 4, 4));

            drawnTiles.Add(new Tile(TestedTileset, Direction.DirectionLess, 3, 5, 0, 3));
            drawnTiles.Add(new Tile(TestedTileset, Direction.DirectionLess, 2, 6, 0, 2));
            drawnTiles.Add(new Tile(TestedTileset, Direction.NorthEast, 3, 6, 1, 3));
            drawnTiles.Add(new Tile(TestedTileset, Direction.North, 3, 5, 1, 3));
            drawnTiles.Add(new Tile(TestedTileset, Direction.NorthWest, 3, 4, 1, 3));
            drawnTiles.Add(new Tile(TestedTileset, Direction.West, 3, 4, 0, 3));
            drawnTiles.Add(new Tile(TestedTileset, Direction.SouthWest, 3, 4, -1, 3));
            drawnTiles.Add(new Tile(TestedTileset, Direction.South, 3, 5, -1, 3));
            drawnTiles.Add(new Tile(TestedTileset, Direction.SouthEast, 3, 6, -1, 3));

            drawnTiles.Add(new Tile(TestedTileset, Direction.DirectionLess, 2, 0, 5, 2));
            drawnTiles.Add(new Tile(TestedTileset, Direction.DirectionLess, 1, 1, 5, 1));
            drawnTiles.Add(new Tile(TestedTileset, Direction.NorthEast, 2, 1, 6, 2));
            drawnTiles.Add(new Tile(TestedTileset, Direction.North, 2, 0, 6, 2));
            drawnTiles.Add(new Tile(TestedTileset, Direction.NorthWest, 2, -1, 6, 2));
            drawnTiles.Add(new Tile(TestedTileset, Direction.West, 2, -1, 5, 2));
            drawnTiles.Add(new Tile(TestedTileset, Direction.SouthWest, 2, -1, 4, 2));
            drawnTiles.Add(new Tile(TestedTileset, Direction.South, 2, 0, 4, 2));
            drawnTiles.Add(new Tile(TestedTileset, Direction.SouthEast, 2, 1, 4, 2));

            drawnTiles.Add(new Tile(TestedTileset, Direction.DirectionLess, 1, 0, 0, 1));
            //drawnTiles.Add(new Tile(TestedTileset, Direction.DirectionLess, 1, 1, 0, 0)); // this is just the floor. Bring back if you drop the floor.
            drawnTiles.Add(new Tile(TestedTileset, Direction.NorthEast, 1, 1, 1, 1));
            drawnTiles.Add(new Tile(TestedTileset, Direction.North, 1, 0, 1, 1));
            drawnTiles.Add(new Tile(TestedTileset, Direction.NorthWest, 1, -1, 1, 1));
            drawnTiles.Add(new Tile(TestedTileset, Direction.West, 1, -1, 0, 1));
            drawnTiles.Add(new Tile(TestedTileset, Direction.SouthWest, 1, -1, -1, 1));
            drawnTiles.Add(new Tile(TestedTileset, Direction.South, 1, 0, -1, 1));
            drawnTiles.Add(new Tile(TestedTileset, Direction.SouthEast, 1, 1, -1, 1));


            // Add a flat floor.
            for (int x = -7; x < 8; x++)
            {
                for (int y = -7; y < 8; y++)
                {
                    drawnTiles.Add(new Tile(TestedTileset, Direction.DirectionLess, 1, x, y, 0));
                }
            }

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
            scene.ViewCenterX = 0;
            scene.ViewCenterY = 0;
            scene.ViewCenterZ = 0;
            scene.Add(drawnTiles);
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 7, y = -6, z = 0, Facing = Direction.North, Race = 0, Sex = 0, Hair = 2, SkinColor = 0, HairColor = 4 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 6, y = -6, z = 0, Facing = Direction.North, Race = 0, Sex = 1, Hair = 2, SkinColor = 0, HairColor = 4 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 5, y = -6, z = 0, Facing = Direction.North, Race = 1, Sex = 0, Hair = 2, SkinColor = 0, HairColor = 1 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 4, y = -6, z = 0, Facing = Direction.North, Race = 1, Sex = 1, Hair = 2, SkinColor = 0, HairColor = 1 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 3, x = 3, y = -6, z = 0, Facing = Direction.North, Race = 2, Sex = 0, Hair = 1, SkinColor = 0, HairColor = 2 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 3, x = 2, y = -6, z = 0, Facing = Direction.North, Race = 2, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 2 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 1, y = -6, z = 0, Facing = Direction.North, Race = 3, Sex = 0, Hair = 1, SkinColor = 0, HairColor = 3 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 0, y = -6, z = 0, Facing = Direction.North, Race = 3, Sex = 1, Hair = 1, SkinColor = 0, HairColor = 3 });

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
            scene.Remove(editPane.shownTile);
        }

        void AddTile_Click(object sender, EventArgs e)
        {
            Tile newTile = null;
            if (editPane.shownTile == null)
            {
                newTile = new Tile(0, Direction.DirectionLess, 1, 0, 0, 0);
            }
            else
            {
                newTile = new Tile(editPane.shownTile.CurrentTileSet, editPane.shownTile.Slope, editPane.shownTile.TileHeight, editPane.shownTile.PosX, editPane.shownTile.PosY, editPane.shownTile.PosZ + editPane.shownTile.TileHeight);
            }
            scene.Add(newTile);
            editPane.SetTile(newTile);
        }

        void TileDirection_SelectedValueChanged(object sender, EventArgs e)
        {
            if (editPane.SettingNewTile) return;
            if (editPane.shownTile == null) return;
            Direction dir;
            if(Enum.TryParse<Direction>(editPane.TileDirection.SelectedItem.ToString(), out dir))
            {
                scene.Remove(editPane.shownTile);
                Tile replacement = new Tile(editPane.shownTile.CurrentTileSet, dir, editPane.shownTile.TileHeight, editPane.shownTile.PosX, editPane.shownTile.PosY, editPane.shownTile.PosZ);
                scene.Add(replacement);
                editPane.SetTile(replacement);
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
                scene.Remove(editPane.shownTile);
                Tile replacement = new Tile(editPane.shownTile.CurrentTileSet, editPane.shownTile.Slope, height, editPane.shownTile.PosX, editPane.shownTile.PosY, editPane.shownTile.PosZ);
                scene.Add(replacement);
                editPane.SetTile(replacement);
            }
        }

        void Set_SelectedValueChanged(object sender, EventArgs e)
        {
            if (editPane.SettingNewTile) return;
            if (editPane.shownTile == null) return;
            scene.Remove(editPane.shownTile);
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
            scene.Add(replacement);
            editPane.SetTile(replacement);
        }

        void Z_ValueChanged(object sender, EventArgs e)
        {
            if (editPane.SettingNewTile) return;
            if (editPane.shownTile == null) return;
            scene.Remove(editPane.shownTile);
            Tile replacement = new Tile(editPane.shownTile.CurrentTileSet, editPane.shownTile.Slope, editPane.shownTile.TileHeight, editPane.shownTile.PosX, editPane.shownTile.PosY, (int)editPane.Z.Value);
            scene.Add(replacement);
            editPane.SetTile(replacement);
        }

        void Y_ValueChanged(object sender, EventArgs e)
        {
            if (editPane.SettingNewTile) return;
            if (editPane.shownTile == null) return;
            scene.Remove(editPane.shownTile);
            Tile replacement = new Tile(editPane.shownTile.CurrentTileSet, editPane.shownTile.Slope, editPane.shownTile.TileHeight, editPane.shownTile.PosX, (int)editPane.Y.Value, editPane.shownTile.PosZ);
            scene.Add(replacement);
            editPane.SetTile(replacement);
        }

        void X_ValueChanged(object sender, EventArgs e)
        {
            if (editPane.SettingNewTile) return;
            if (editPane.shownTile == null) return; 
            scene.Remove(editPane.shownTile);
            Tile replacement = new Tile(editPane.shownTile.CurrentTileSet, editPane.shownTile.Slope, editPane.shownTile.TileHeight, (int)editPane.X.Value, editPane.shownTile.PosY, editPane.shownTile.PosZ);
            scene.Add(replacement);
            editPane.SetTile(replacement);
        }

        void scene_TileSelected(object Sender, TileSelectEventArgs e)
        {
            editPane.SetTile(e.tile);
        }

        void TileViewer_Resize(object sender, EventArgs e)
        {
            scene.Size = this.Size;
        }
    }
}
