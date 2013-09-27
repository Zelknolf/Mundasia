using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace Mundasia.Objects
{
    public class TileImage : IComparable<TileImage>
    {
        public int DrawIndex;
        public Point ImageLocation;
        public Tile SourceTile;
        public Image DayImage;
        public Image TwilightImage;
        public Image NightImage;
        public Image MouseOverImage;
        public Image SelectedImage;

        public bool MousedOver = true;
        public bool Selected = true;

        /// <summary>
        /// Used to sort a collection of TileImages such that the first objects
        /// in the collection are the first who need to be drawn.
        /// </summary>
        /// <param name="other">The other tile to be sorted against</param>
        /// <returns>The result of draw index sorting</returns>
        public int CompareTo(TileImage other)
        {
            return DrawIndex.CompareTo(other.DrawIndex);
        }

        /// <summary>
        /// Returns the TileImage which represents the tile which is on top of the drawing
        /// at the point
        /// </summary>
        /// <param name="clientRectLocation">The location of the point to be evaluated</param>
        /// <param name="presentImages">The TileImages used to construct the view</param>
        /// <returns>The TileImage which is receiving the mouseover</returns>
        public static TileImage GetTarget(Point clientRectLocation, List<TileImage> presentImages)
        {
            List<TileImage> mousedOver = new List<TileImage>();
            foreach (TileImage image in presentImages)
            {
                if (image.ImageLocation.X < clientRectLocation.X &&
                    image.ImageLocation.X + image.NightImage.Width > clientRectLocation.X &&
                    image.ImageLocation.Y < clientRectLocation.Y &&
                    image.ImageLocation.Y + image.NightImage.Height > clientRectLocation.Y)
                {
                    mousedOver.Add(image);
                }
            }
            if (mousedOver.Count > 0)
            {
                mousedOver.Sort();
                while (mousedOver.Count > 0)
                {
                    int imgPosX = clientRectLocation.X - mousedOver[mousedOver.Count - 1].ImageLocation.X;
                    int imgPosY = clientRectLocation.Y - mousedOver[mousedOver.Count - 1].ImageLocation.Y;
                    Color px = (mousedOver[mousedOver.Count - 1].NightImage as Bitmap).GetPixel(imgPosX, imgPosY);
                    if (px.A != 0)
                    {
                        return mousedOver[mousedOver.Count - 1];
                    }
                    else
                    {
                        mousedOver.Remove(mousedOver[mousedOver.Count - 1]);
                    }
                }
            }
            return null;
        }
    }
}
