﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Drawing;

namespace Mundasia.Objects
{
    public class TileImage : IPlaySceneDrawable
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
        public int CompareTo(IPlaySceneDrawable other)
        {
            return DrawIndex.CompareTo(other.GetDrawIndex());
        }

        public int GetDrawIndex()
        {
            return DrawIndex;
        }

        public bool GetSelected()
        {
            return Selected;
        }

        public void SetSelected(bool State)
        {
            Selected = State;
        }

        public bool GetMousedOver()
        {
            return MousedOver;
        }

        public void SetMousedOver(bool State)
        {
            MousedOver = State;
        }

        public Point GetImageLocation()
        {
            return ImageLocation;
        }

        public Image GetTemplateImage()
        {
            return DayImage;
        }

        public Image GetDayImage()
        {
            return DayImage;
        }

        public Image GetNightImage()
        {
            return NightImage;
        }

        public Image GetTwilightImage()
        {
            return TwilightImage;
        }

        public Image GetSelectedImage()
        {
            return SelectedImage;
        }

        public Image GetMouseOverImage()
        {
            return MouseOverImage;
        }
    }
}
