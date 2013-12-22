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
        Direction topDirection = Direction.NorthWest;
        PlayScene scene;

        int timeOfDay = 0;

        public TileViewer()
        {
            InitializeComponent();
            scene = new PlayScene();

            // Build four little pyramids to show slopes and blocks of every height
            drawnTiles.Add(new Tile("White", Direction.DirectionLess, 4, 5, 5, 4));
            drawnTiles.Add(new Tile("White", Direction.DirectionLess, 3, 6, 5, 3));
            drawnTiles.Add(new Tile("White", Direction.NorthEast, 4, 6, 6, 4));
            drawnTiles.Add(new Tile("White", Direction.North, 4, 5, 6, 4));
            drawnTiles.Add(new Tile("White", Direction.NorthWest, 4, 4, 6, 4));
            drawnTiles.Add(new Tile("White", Direction.West, 4, 4, 5, 4));
            drawnTiles.Add(new Tile("White", Direction.SouthWest, 4, 4, 4, 4));
            drawnTiles.Add(new Tile("White", Direction.South, 4, 5, 4, 4));
            drawnTiles.Add(new Tile("White", Direction.SouthEast, 4, 6, 4, 4));

            drawnTiles.Add(new Tile("White", Direction.DirectionLess, 3, 5, 0, 3));
            drawnTiles.Add(new Tile("White", Direction.DirectionLess, 2, 6, 0, 2));
            drawnTiles.Add(new Tile("White", Direction.NorthEast, 3, 6, 1, 3));
            drawnTiles.Add(new Tile("White", Direction.North, 3, 5, 1, 3));
            drawnTiles.Add(new Tile("White", Direction.NorthWest, 3, 4, 1, 3));
            drawnTiles.Add(new Tile("White", Direction.West, 3, 4, 0, 3));
            drawnTiles.Add(new Tile("White", Direction.SouthWest, 3, 4, -1, 3));
            drawnTiles.Add(new Tile("White", Direction.South, 3, 5, -1, 3));
            drawnTiles.Add(new Tile("White", Direction.SouthEast, 3, 6, -1, 3));

            drawnTiles.Add(new Tile("White", Direction.DirectionLess, 2, 0, 5, 2));
            drawnTiles.Add(new Tile("White", Direction.DirectionLess, 1, 1, 5, 1));
            drawnTiles.Add(new Tile("White", Direction.NorthEast, 2, 1, 6, 2));
            drawnTiles.Add(new Tile("White", Direction.North, 2, 0, 6, 2));
            drawnTiles.Add(new Tile("White", Direction.NorthWest, 2, -1, 6, 2));
            drawnTiles.Add(new Tile("White", Direction.West, 2, -1, 5, 2));
            drawnTiles.Add(new Tile("White", Direction.SouthWest, 2, -1, 4, 2));
            drawnTiles.Add(new Tile("White", Direction.South, 2, 0, 4, 2));
            drawnTiles.Add(new Tile("White", Direction.SouthEast, 2, 1, 4, 2));

            drawnTiles.Add(new Tile("White", Direction.DirectionLess, 1, 0, 0, 1));
            //drawnTiles.Add(new Tile("White", Direction.DirectionLess, 1, 1, 0, 0)); // this is just the floor. Bring back if you drop the floor.
            drawnTiles.Add(new Tile("White", Direction.NorthEast, 1, 1, 1, 1));
            drawnTiles.Add(new Tile("White", Direction.North, 1, 0, 1, 1));
            drawnTiles.Add(new Tile("White", Direction.NorthWest, 1, -1, 1, 1));
            drawnTiles.Add(new Tile("White", Direction.West, 1, -1, 0, 1));
            drawnTiles.Add(new Tile("White", Direction.SouthWest, 1, -1, -1, 1));
            drawnTiles.Add(new Tile("White", Direction.South, 1, 0, -1, 1));
            drawnTiles.Add(new Tile("White", Direction.SouthEast, 1, 1, -1, 1));


            // Add a flat floor.
            for (int x = -7; x < 8; x++)
            {
                for (int y = -7; y < 8; y++)
                {
                    drawnTiles.Add(new Tile("White", Direction.DirectionLess, 1, x, y, 0));
                }
            }

            scene.Size = this.Size;
            scene.ViewCenterX = 0;
            scene.ViewCenterY = 0;
            scene.ViewCenterZ = 0;
            scene.Add(drawnTiles);
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 7, y = -6, z = 0, Facing = Direction.North, Race = 0, Sex = 0 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 6, y = -6, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 5, y = -6, z = 0, Facing = Direction.North, Race = 1, Sex = 0 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 4, y = -6, z = 0, Facing = Direction.North, Race = 1, Sex = 1 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 3, x = 3, y = -6, z = 0, Facing = Direction.North, Race = 2, Sex = 0 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 3, x = 2, y = -6, z = 0, Facing = Direction.North, Race = 2, Sex = 1 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 1, y = -6, z = 0, Facing = Direction.North, Race = 3, Sex = 0 });
            scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 2, x = 0, y = -6, z = 0, Facing = Direction.North, Race = 3, Sex = 1 });

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
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 7, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 6, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 5, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 4, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 3, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 2, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 1, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 0, y = -5, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 7, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 6, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 5, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 4, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 3, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 2, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 1, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });
            //scene.Add(new DisplayCharacter("") { CharacterId = 1, Height = 4, x = 0, y = -4, z = 0, Facing = Direction.North, Race = 0, Sex = 1 });

            this.Controls.Add(scene);

            this.Resize += new EventHandler(TileViewer_Resize);
        }

        void TileViewer_Resize(object sender, EventArgs e)
        {
            scene.Size = this.Size;
        }
    }
}
