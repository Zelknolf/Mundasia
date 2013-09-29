using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;
using System.Windows.Forms;

using Mundasia.Objects;

namespace Mundasia.Interface
{
    public class PlayScene : Panel
    {
        /// <summary>
        /// TopDirection stores the PlayScene's current idea of which in-game direction translates to
        /// the top of the panel. It is of no interest to the server.
        /// </summary>
        private Direction topDirection = Direction.NorthWest;

        /// <summary>
        /// A sortable collection of images which contains all that should be visible in the current scene.
        /// </summary>
        private List<TileImage> drawableImages = new List<TileImage>();

        /// <summary>
        /// A collection of tiles which should be visible in the current scene.
        /// </summary>
        private List<Tile> drawableTiles = new List<Tile>();

        /// <summary>
        /// stores which object is currently displaying as moused over to the user,
        /// and thus what the user would expect to receive a click.
        /// </summary>
        private TileImage currentMouseOver = null;

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

        public PlayScene()
        {
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(PlayScene_Paint);
            this.MouseClick += new MouseEventHandler(PlayScene_MouseClick);
            this.MouseMove += new MouseEventHandler(PlayScene_MouseMove);
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
        /// Manages the calculation of mouse overs, and predicts click targets.
        /// </summary>
        private void PlayScene_MouseMove(object sender, MouseEventArgs e)
        {
            TileImage target = TileImage.GetTarget(e.Location, drawableImages);
            if (currentMouseOver != null)
            {
                this.Invalidate(new Rectangle(currentMouseOver.ImageLocation, currentMouseOver.DayImage.Size));
                currentMouseOver.MousedOver = false;
            }
            if (target != null)
            {
                this.Invalidate(new Rectangle(target.ImageLocation, target.DayImage.Size));
                target.MousedOver = true;
            }
            currentMouseOver = target;
            this.Update();
        }

        private void PlayScene_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                // TODO: Don't hard code these values.
                if (e.Location.X > this.Width - 63 &&
                    e.Location.Y < 50)
                {
                    switch (topDirection)
                    {
                        case Direction.NorthWest:
                            topDirection = Direction.NorthEast;
                            break;
                        case Direction.NorthEast:
                            topDirection = Direction.SouthEast;
                            break;
                        case Direction.SouthEast:
                            topDirection = Direction.SouthWest;
                            break;
                        case Direction.SouthWest:
                            topDirection = Direction.NorthWest;
                            break;
                    }
                    drawableImages.Clear();
                    foreach (Tile tile in drawableTiles)
                    {
                        drawableImages.Add(tile.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this));
                    }
                    drawableImages.Sort();
                    this.Refresh();
                }
                // TODO: Don't hard code these values.
                else if (e.Location.X < 50 &&
                        e.Location.Y < 50)
                {
                    switch (topDirection)
                    {    
                        case Direction.NorthWest:
                            topDirection = Direction.SouthWest;
                            break;
                        case Direction.NorthEast:
                            topDirection = Direction.NorthWest;
                            break;
                        case Direction.SouthEast:
                            topDirection = Direction.NorthEast;
                            break;
                        case Direction.SouthWest:
                            topDirection = Direction.SouthEast;
                            break;
                    }
                    drawableImages.Clear();
                    foreach (Tile tile in drawableTiles)
                    {
                        drawableImages.Add(tile.Image(ViewCenterX, ViewCenterY, ViewCenterZ, topDirection, this));
                    }
                    drawableImages.Sort();
                    this.Refresh();
                }
                else if (currentMouseOver != null)
                {
                    currentMouseOver.Selected = !currentMouseOver.Selected;
                }
            }

            // TODO: Remove the temporary measure which allows adjustment of lighting.
            else
            {
                timeOfDay++;
                if (timeOfDay > 2) timeOfDay = 0;
                this.Refresh();
            }
        }

        private void PlayScene_Paint(object sender, PaintEventArgs e)
        {
            switch(timeOfDay)
            {
                case 1:
                    e.Graphics.Clear(Color.LightCoral);
                    break;
                case 2:
                    e.Graphics.Clear(Color.Black);
                    break;
                default:
                    e.Graphics.Clear(Color.LightBlue);
                    break;
            }
            foreach (TileImage image in drawableImages)
            {
                switch (timeOfDay)
                {
                    case 1:
                        e.Graphics.DrawImage(image.TwilightImage, image.ImageLocation);
                        break;
                    case 2:
                        e.Graphics.DrawImage(image.NightImage, image.ImageLocation);
                        break;
                    default:
                        e.Graphics.DrawImage(image.DayImage, image.ImageLocation);
                        break;
                }
                if (image.MousedOver)
                {
                    e.Graphics.DrawImage(image.MouseOverImage, image.ImageLocation);
                }
                if (image.Selected)
                {
                    e.Graphics.DrawImage(image.SelectedImage, image.ImageLocation);
                }
            }
            e.Graphics.DrawImage(Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\RotateCW.png"), new Point(0, 0));
            e.Graphics.DrawImage(Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\RotateCCW.png"), new Point(this.Width-53, 0));
        }
    }
}
