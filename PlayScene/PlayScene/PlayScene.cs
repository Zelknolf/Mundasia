using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

using Mundasia.Objects;

namespace Mundasia.Interface
{
    public partial class PlayScene : Panel
    {
        public PlayScene()
        {
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(PlayScene_Paint);
            this.MouseClick += new MouseEventHandler(PlayScene_MouseClick);
            this.MouseMove += new MouseEventHandler(PlayScene_MouseMove);
            this.Resize += new EventHandler(PlayScene_Resize);
        }

        /// <summary>
        /// Adds tiles to those which are to be drawn in the scene.
        /// </summary>
        /// <param name="tiles">A list of tiles to add</param>
        public void Add(List<Tile> tiles)
        {
            foreach (Tile tile in tiles)
            {
                drawableTiles.Add(tile);
                TileImage image = tile.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
                image.SourceTile = tile;
                drawableImages.Add(image);
            }
            drawableImages.Sort();
            this.Refresh();
        }

        public void Add(DisplayCharacter ch)
        {
            drawableCharacters.Add(ch);
            CharacterImage image = ch.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
            drawableImages.Add(image);
            drawableImages.Sort();
            this.Refresh();
        }
    

        /// <summary>
        /// Adds tiles to those which are to be drawn in the scene.
        /// 
        /// If many tiles are to be added, they should be added as a list to reduce redrawing.
        /// </summary>
        /// <param name="tile">A tile to add</param>
        public void Add(Tile tile)
        {
            drawableTiles.Add(tile);
            TileImage image = tile.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this);
            image.SourceTile = tile;
            drawableImages.Add(image);
            drawableImages.Sort();
            this.Refresh();
        }

        /// <summary>
        /// Removes tiles from those which are to be drawn in the scene.
        /// </summary>
        /// <param name="tiles">A list of tiles to remove.</param>
        public void Remove(List<Tile> tiles)
        {
            foreach(Tile tile in tiles)
            {
                drawableTiles.Remove(tile);
                drawableImages.Remove(tile.CachedImage);
            }
            this.Refresh();
        }

        /// <summary>
        /// Removes tiles from those which are to be drawn in the scene.
        /// 
        /// If many tiles are to be removed, they should be removed as a list to reduce redrawing.
        /// </summary>
        /// <param name="tile">A tile to remove</param>
        public void Remove(Tile tile)
        {
            drawableTiles.Remove(tile);
            drawableImages.Remove(tile.CachedImage);
            this.Refresh();
        }

        /// <summary>
        /// TopDirection stores the PlayScene's current idea of which in-game direction translates to
        /// the top of the panel. It is of no interest to the server.
        /// </summary>
        private Direction topDirection = Direction.NorthWest;

        /// <summary>
        /// A sortable collection of images which contains all that should be visible in the current scene.
        /// </summary>
        private List<IPlaySceneDrawable> drawableImages = new List<IPlaySceneDrawable>();

        /// <summary>
        /// A collection of tiles which should be visible in the current scene.
        /// </summary>
        private List<Tile> drawableTiles = new List<Tile>();

        private List<DisplayCharacter> drawableCharacters = new List<DisplayCharacter>();

        /// <summary>
        /// stores which object is currently displaying as moused over to the user,
        /// and thus what the user would expect to receive a click.
        /// </summary>
        private IPlaySceneDrawable currentMouseOver = null;

        /// <summary>
        /// TODO: Turn this into a real system for time of day.
        /// </summary>
        int timeOfDay = 0;

        /// <summary>
        /// Contains the X coordinate of the center of the view.
        /// </summary>
        public int ViewCenterX;

        /// <summary>
        /// Contains the Y coordinate of the center of the view.
        /// </summary>
        public int ViewCenterY;

        /// <summary>
        /// Contains the Z coordinate of the center of the view.
        /// </summary>
        public int ViewCenterZ;
    }
}
