using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace Mundasia.Objects
{
    public class DisplayCharacter
    {
        public DisplayCharacter(string fileLine)
        {

        }

        public DisplayCharacter(Character ch)
        {

        }

        public uint CharacterId;

        public int Height;
        public int x;
        public int y;
        public int z;
        public Direction Facing;

        public int Race;
        public int Sex;

        public CharacterImage CachedImage;

        public override string ToString()
        {
            return base.ToString();
        }

        /// <summary>
        /// Provides a TileImage containing images that might be drawn to represent this tile.
        /// 
        /// TileImage is recalculated per call, as the details may change based on position or angle.
        /// </summary>
        /// <param name="centerCoordX">the X coordinate of the center tile, where a larger X is east</param>
        /// <param name="centerCoordY">the Y coordinate of the center tile, where a larger Y is north</param>
        /// <param name="centerCoordZ">the Z coordinate of the top of the tallest part of the tile</param>
        /// <param name="topDirection">the Direction of the screen</param>
        /// <param name="targetPanel">the panel which is intended to receive the tiles</param>
        /// <returns></returns>
        public CharacterImage Image(int centerCoordX, int centerCoordY, int centerCoordZ, Direction topDirection, System.Windows.Forms.Panel targetPanel)
        {
            // -------------------------------------------
            // This region reviews the panel, gathering
            // information that will later be necessary
            // to determine the draw order of the tile
            // and what can fit in the view
            // -------------------------------------------
            #region Study the Panel
            // Find out how many rows of tiles we can fit on this panel. If we don't get an odd number, add one.
            int indexDepth = targetPanel.Height / 10;
            if (indexDepth % 2 != 1) indexDepth += 1;
            #endregion

            // -------------------------------------------
            // This section determines the X/Y coordinates
            // of the top left corner of the image to be
            // used to display the tile.
            // -------------------------------------------
            #region Position Calculation
            // The center of the panel is (roughly) half of the height and width of the panel.
            int centerY = targetPanel.Height / 2 - 10;
            int centerX = targetPanel.Width / 2 - 20;

            // Find offsets for this tile on the in-world coordinates
            int easternOffset = x - centerCoordX;
            int northernOffset = y - centerCoordY;
            int verticalOffset = z - centerCoordZ;

            int imagePosX = centerX;
            int imagePosY = centerY;
            switch (topDirection)
            {
                case Direction.NorthWest:
                    // North is top right. East is bottom right.
                    imagePosX += northernOffset * 20;
                    imagePosY -= northernOffset * 10;
                    imagePosX += easternOffset * 20;
                    imagePosY += easternOffset * 10;
                    break;
                case Direction.SouthWest:
                    // North is bottom right. East is bottom left.
                    imagePosX += northernOffset * 20;
                    imagePosY += northernOffset * 10;
                    imagePosX -= easternOffset * 20;
                    imagePosY += easternOffset * 10;
                    break;
                case Direction.SouthEast:
                    // North is bottom left. East is top left.
                    imagePosX -= northernOffset * 20;
                    imagePosY += northernOffset * 10;
                    imagePosX -= easternOffset * 20;
                    imagePosY -= easternOffset * 10;
                    break;
                default: // "default" is northeast.
                    // North is top left. East is top right.
                    imagePosX -= northernOffset * 20;
                    imagePosY -= northernOffset * 10;
                    imagePosX += easternOffset * 20;
                    imagePosY -= easternOffset * 10;
                    break;
            }
            int sortOrder = ((imagePosY / 10) * 100) + 5; // Force the tiles into rows initially, with wide spaces in sort order
            imagePosY -= (verticalOffset * 10) + (Height * 10);

            // vertical offset needs to be more than 50 for this to run into other rows-- which is 500 pixels tall. The thing
            // is already horrendously dominating the screen if it exists.
            sortOrder += verticalOffset;
            #endregion

            // -------------------------------------------
            // This region takes the orientation of the
            // tile compared to the top edge direction to
            // determine which image should be used to
            // draw the tile.
            // -------------------------------------------
            #region Get a File Name
            string fileName = "stand";
            switch (topDirection)
            {
                case Direction.NorthWest:
                    switch (Facing)
                    {
                        // Direction.Directionless is unhandled, as it has no suffix after block_#
                        case Direction.North: fileName += "_tr"; break;
                        case Direction.East: fileName += "_br"; break;
                        case Direction.South: fileName += "_bl"; break;
                        case Direction.West: fileName += "_tl"; break;
                    }
                    break;
                case Direction.SouthEast:
                    switch (Facing)
                    {
                        case Direction.North: fileName += "_bl"; break;
                        case Direction.East: fileName += "_tl"; break;
                        case Direction.South: fileName += "_tr"; break;
                        case Direction.West: fileName += "_br"; break;
                    }
                    break;
                case Direction.SouthWest:
                    switch (Facing)
                    {
                        case Direction.North: fileName += "_br"; break;
                        case Direction.East: fileName += "_bl"; break;
                        case Direction.South: fileName += "_tl"; break;
                        case Direction.West: fileName += "_tr"; break;
                    }
                    break;
                case Direction.NorthEast:
                    switch (Facing)
                    {
                        case Direction.North: fileName += "_tl"; break;
                        case Direction.East: fileName += "_tr"; break;
                        case Direction.South: fileName += "_br"; break;
                        case Direction.West: fileName += "_bl"; break;
                    }
                    break;
            }
            fileName += ".png";
            #endregion

            // -------------------------------------------
            // This region checks for a cached version of
            // the images, and creates one if such an
            // image does not exist already.
            // -------------------------------------------
            #region Get an Image
            Image Day = System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Characters\\"+ Race +"\\" + Sex + "\\" + fileName);
            Bitmap NightBmp = new Bitmap(Day);
            Bitmap TwilightBmp = new Bitmap(Day);
            Bitmap MouseOverBmp = new Bitmap(Day);
            Bitmap SelectionBmp = new Bitmap(Day);

            // Convert night to dark blue
            for (int c = 0; c < NightBmp.Width; c++)
            {
                for (int cc = 0; cc < NightBmp.Height; cc++)
                {
                    Color px = NightBmp.GetPixel(c, cc);
                    if (px.A == 0) continue;
                    int nR = px.R / 4;
                    int nG = px.G * 3 / 8;
                    int nB = px.B / 2;
                    NightBmp.SetPixel(c, cc, Color.FromArgb(px.A, nR, nG, nB));
                }
            }
            // Convert twilight to warm red
            for (int c = 0; c < TwilightBmp.Width; c++)
            {
                for (int cc = 0; cc < TwilightBmp.Height; cc++)
                {
                    Color px = TwilightBmp.GetPixel(c, cc);
                    if (px.A == 0) continue;
                    int nR = px.R;
                    int nG = px.G * 3 / 4;
                    int nB = px.B / 2;
                    TwilightBmp.SetPixel(c, cc, Color.FromArgb(px.A, nR, nG, nB));
                }
            }
            // Create a mouse over sheet
            for (int c = 0; c < MouseOverBmp.Width; c++)
            {
                for (int cc = 0; cc < MouseOverBmp.Height; cc++)
                {
                    Color px = MouseOverBmp.GetPixel(c, cc);
                    if (px.A == 0) continue;
                    MouseOverBmp.SetPixel(c, cc, Color.FromArgb(50, 0, 255, 255));
                    SelectionBmp.SetPixel(c, cc, Color.FromArgb(100, 0, 150, 255));
                }
            }

            DayCache[CharacterId] = Day;
            TwilightCache[CharacterId] = TwilightBmp;
            NightCache[CharacterId] = NightBmp;
            MouseOverCache[CharacterId] = MouseOverBmp;
            SelectedCache[CharacterId] = SelectionBmp;
            #endregion

            #region Return a new TileImage
            CharacterImage retVal = new CharacterImage()
            {
                DayImage = DayCache[CharacterId],
                NightImage = NightCache[CharacterId],
                TwilightImage = TwilightCache[CharacterId],
                MouseOverImage = MouseOverCache[CharacterId],
                SelectedImage = SelectedCache[CharacterId],
                Selected = false,
                MousedOver = false,
                DrawIndex = sortOrder,
                ImageLocation = new Point(imagePosX, imagePosY),
                SourceCharacter = this
            };
            this.CachedImage = retVal;
            return retVal;
            #endregion
        }

        #region Caching of Tiles
        /// <summary>
        /// A static resource to contain images which represent post-processing images for given tiles. Expected syntax would be DayCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<uint, Image> DayCache = new Dictionary<uint, Image>();

        /// <summary>
        /// A static resource to contain images which represent post-processing images for given tiles. Expected syntax would be TwilightCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<uint, Image> TwilightCache = new Dictionary<uint, Image>();

        /// <summary>
        /// A static resource to contain images which represent post-processing images for given tiles. Expected syntax would be NightCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<uint, Image> NightCache = new Dictionary<uint, Image>();

        /// <summary>
        /// A static resource to contain images which indicate a mouse over effect for given tiles. Expected syntax would be MouseOverCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<uint, Image> MouseOverCache = new Dictionary<uint, Image>();

        /// <summary>
        /// A static resource to contain images which indicate a selection effect for given tiles. Expected syntax would be MouseOverCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<uint, Image> SelectedCache = new Dictionary<uint, Image>();
        #endregion
    }
}
