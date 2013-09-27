using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace Mundasia.Objects
{
    /// <summary>
    /// The Tile class is responsible for defining the space that tiles occupy, for determining the appearance of specific
    /// tiles, organizing how those tiles place relative to one another, and handling the mechanics of line of sight and 
    /// pathfinding.
    /// 
    /// As we expect tiles to be used and re-used frequently, this class should expect the use of caching when possible.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// This class defines a tile, and provides methods which make said tile useful to other parts of the program.
        /// </summary>
        /// <param name="TileSet">a string representing where the images associated with the tile can be located</param>
        /// <param name="Model">a TileModel representing whether the tile is sloped or a block</param>
        /// <param name="SlopeSide">if the Tile is sloped, which side or corner of the tile is the bottom one.</param>
        /// <param name="Height">The height of the tallest side of the tile</param>
        /// <param name="X">The X coordinate of the tile, where a larger X is east</param>
        /// <param name="Y">The Y coordinate of the tile, where a larger Y is north</param>
        /// <param name="Z">The Z coordinate of the top of the tallest part of the tile</param>
        public Tile(string TileSet, Direction SlopeSide, int Height, int X, int Y, int Z)
        {
            tileSet = TileSet;
            slopeSide = SlopeSide;
            height = Height;
            x = X;
            y = Y;
            z = Z;
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
        public TileImage Image(int centerCoordX, int centerCoordY, int centerCoordZ, Direction topDirection, System.Windows.Forms.Panel targetPanel)
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
            int sortOrder = (imagePosY / 10) * 100; // Force the tiles into rows initially, with wide spaces in sort order
            imagePosY -= verticalOffset * 10;

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
            string fileName = "block_" + height.ToString();
            switch (topDirection)
            {
                case Direction.NorthWest:
                    switch (slopeSide)
                    {
                        // Direction.Directionless is unhandled, as it has no suffix after block_#
                        case Direction.North: fileName += "_tr"; break;
                        case Direction.NorthEast: fileName += "_r"; break;
                        case Direction.East: fileName += "_br"; break;
                        case Direction.SouthEast: fileName += "_b"; break;
                        case Direction.South: fileName += "_bl"; break;
                        case Direction.SouthWest: fileName += "_l"; break;
                        case Direction.West: fileName += "_tl"; break;
                        case Direction.NorthWest: fileName += "_t"; break;
                    }
                    break;
                case Direction.SouthEast:
                    switch (slopeSide)
                    {
                        case Direction.North: fileName += "_bl"; break;
                        case Direction.NorthEast: fileName += "_l"; break;
                        case Direction.East: fileName += "_tl"; break;
                        case Direction.SouthEast: fileName += "_t"; break;
                        case Direction.South: fileName += "_tr"; break;
                        case Direction.SouthWest: fileName += "_r"; break;
                        case Direction.West: fileName += "_br"; break;
                        case Direction.NorthWest: fileName += "_b"; break;
                    }
                    break;
                case Direction.SouthWest:
                    switch (slopeSide)
                    {
                        case Direction.North: fileName += "_br"; break;
                        case Direction.NorthEast: fileName += "_b"; break;
                        case Direction.East: fileName += "_bl"; break;
                        case Direction.SouthEast: fileName += "_l"; break;
                        case Direction.South: fileName += "_tl"; break;
                        case Direction.SouthWest: fileName += "_t"; break;
                        case Direction.West: fileName += "_tr"; break;
                        case Direction.NorthWest: fileName += "_r"; break;
                    }
                    break;
                case Direction.NorthEast:
                    switch (slopeSide)
                    {
                        case Direction.North: fileName += "_tl"; break;
                        case Direction.NorthEast: fileName += "_t"; break;
                        case Direction.East: fileName += "_tr"; break;
                        case Direction.SouthEast: fileName += "_r"; break;
                        case Direction.South: fileName += "_br"; break;
                        case Direction.SouthWest: fileName += "_b"; break;
                        case Direction.West: fileName += "_bl"; break;
                        case Direction.NorthWest: fileName += "_l"; break;
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
            if (!DayCache.ContainsKey(tileSet))
            {
                DayCache[tileSet] = new Dictionary<string, Image>();
                TwilightCache[tileSet] = new Dictionary<string, Image>();
                NightCache[tileSet] = new Dictionary<string, Image>();
                MouseOverCache[tileSet] = new Dictionary<string, Image>();
                SelectedCache[tileSet] = new Dictionary<string, Image>();
            }
            if (!DayCache[tileSet].ContainsKey(fileName))
            {
                Image Day = System.Drawing.Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\Tiles\\" + tileSet + "\\" + fileName);
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

                DayCache[tileSet][fileName] = Day;
                TwilightCache[tileSet][fileName] = TwilightBmp;
                NightCache[tileSet][fileName] = NightBmp;
                MouseOverCache[tileSet][fileName] = MouseOverBmp;
                SelectedCache[tileSet][fileName] = SelectionBmp;
            }
            #endregion

            #region Return a new TileImage
            return new TileImage()
            {
                DayImage = DayCache[tileSet][fileName],
                NightImage = NightCache[tileSet][fileName],
                TwilightImage = TwilightCache[tileSet][fileName],
                MouseOverImage = MouseOverCache[tileSet][fileName],
                SelectedImage = SelectedCache[tileSet][fileName],
                Selected = false,
                MousedOver = false,
                DrawIndex = sortOrder,
                ImageLocation = new Point(imagePosX, imagePosY),
                SourceTile = this
            };
            #endregion
        }

        #region Tile Definition
        /// <summary>
        /// Storage of the folder name where we expect to find the sources for our tiles.
        /// </summary>
        private string tileSet;

        /// <summary>
        /// Storage for which side of the tile is the low point of the slope.
        /// </summary>
        private Direction slopeSide;

        /// <summary>
        /// How tall the tile is. Expecting 1 - 4.
        /// </summary>
        private int height;

        /// <summary>
        /// The X coordinate of the tile.
        /// </summary>
        private int x;

        /// <summary>
        /// The Y coordinate of the tile.
        /// </summary>
        private int y;

        /// <summary>
        /// The Z coordinate of the tile.
        /// </summary>
        private int z;
        #endregion

        #region Caching of Tiles
        /// <summary>
        /// A static resource to contain images which represent post-processing images for given tiles. Expected syntax would be DayCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<string, Dictionary<string, Image>> DayCache = new Dictionary<string, Dictionary<string, Image>>();

        /// <summary>
        /// A static resource to contain images which represent post-processing images for given tiles. Expected syntax would be TwilightCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<string, Dictionary<string, Image>> TwilightCache = new Dictionary<string, Dictionary<string, Image>>();

        /// <summary>
        /// A static resource to contain images which represent post-processing images for given tiles. Expected syntax would be NightCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<string, Dictionary<string, Image>> NightCache = new Dictionary<string, Dictionary<string, Image>>();

        /// <summary>
        /// A static resource to contain images which indicate a mouse over effect for given tiles. Expected syntax would be MouseOverCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<string, Dictionary<string, Image>> MouseOverCache = new Dictionary<string, Dictionary<string, Image>>();

        /// <summary>
        /// A static resource to contain images which indicate a selection effect for given tiles. Expected syntax would be MouseOverCache["TileSet"]["FileName"]
        /// </summary>
        private static Dictionary<string, Dictionary<string, Image>> SelectedCache = new Dictionary<string, Dictionary<string, Image>>();
        #endregion
    }
}
