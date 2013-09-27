using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Mundasia.Objects;

namespace TileTester
{
    public class ViewPort : Panel
    {
        public ViewPort()
        {
            this.DoubleBuffered = true;
        }
    }
    
    public partial class TileViewer : Form
    {
        public ViewPort ViewPort = new ViewPort();

        List<Tile> drawnTiles = new List<Tile>();
        List<TileImage> drawableImages = new List<TileImage>();
        TileImage currentMouseOver = null;
        Direction topDirection = Direction.NorthWest;

        Label topDirectionLabel = new Label();

        int timeOfDay = 0;

        public TileViewer()
        {
            InitializeComponent();
            ViewPort.Size = this.DisplayRectangle.Size;
            this.DoubleBuffered = true;
            this.Controls.Add(topDirectionLabel);
            this.Controls.Add(ViewPort);

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

            foreach (Tile tile in drawnTiles)
            {
                drawableImages.Add(tile.Image(0,0,0,topDirection, ViewPort));
            }
            drawableImages.Sort();

            topDirectionLabel.Text = topDirection.ToString();
            topDirectionLabel.Size = topDirectionLabel.PreferredSize;
            topDirectionLabel.Location = new Point(ViewPort.Width / 2 - topDirectionLabel.Width / 2, 10);

            ViewPort.Paint += new PaintEventHandler(ViewPort_Paint);
            ViewPort.MouseClick += new MouseEventHandler(ViewPort_Click);
            ViewPort.MouseMove += new MouseEventHandler(ViewPort_MouseMove);
        }

        void ViewPort_MouseMove(object sender, MouseEventArgs e)
        {
            TileImage target = TileImage.GetTarget(e.Location, drawableImages);
            if (currentMouseOver != null)
            {
                ViewPort.Invalidate(new Rectangle(currentMouseOver.ImageLocation, currentMouseOver.DayImage.Size));
                currentMouseOver.MousedOver = false;
            }
            if (target != null)
            {
                ViewPort.Invalidate(new Rectangle(target.ImageLocation, target.DayImage.Size));
                target.MousedOver = true;
            }
            currentMouseOver = target;
            ViewPort.Update();
        }

        void ViewPort_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (e.Location.X > ViewPort.Width - 63 &&
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
                    foreach (Tile tile in drawnTiles)
                    {
                        drawableImages.Add(tile.Image(0, 0, 0, topDirection, ViewPort));
                    }
                    drawableImages.Sort();
                    topDirectionLabel.Text = topDirection.ToString();
                    topDirectionLabel.Size = topDirectionLabel.PreferredSize;
                    topDirectionLabel.Location = new Point(ViewPort.Width / 2 - topDirectionLabel.Width / 2, 10);
                    ViewPort.Refresh();
                }
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
                    foreach (Tile tile in drawnTiles)
                    {
                        drawableImages.Add(tile.Image(0, 0, 0, topDirection, ViewPort));
                    }
                    drawableImages.Sort();
                    topDirectionLabel.Text = topDirection.ToString();
                    topDirectionLabel.Size = topDirectionLabel.PreferredSize;
                    topDirectionLabel.Location = new Point(ViewPort.Width / 2 - topDirectionLabel.Width / 2, 10);
                    ViewPort.Refresh();
                }
                else if (currentMouseOver != null)
                {
                    currentMouseOver.Selected = !currentMouseOver.Selected;
                }

            }
            else
            {
                timeOfDay++;
                if (timeOfDay > 2) timeOfDay = 0;
                ViewPort.Refresh();
            }
        }

        void ViewPort_Paint(object sender, PaintEventArgs e)
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
            e.Graphics.DrawImage(Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\RotateCCW.png"), new Point(ViewPort.Width-53, 0));
        }
    }
}
