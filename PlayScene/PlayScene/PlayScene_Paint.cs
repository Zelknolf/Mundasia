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
        /// <summary>
        /// Draws all of the positioned tiles with the lighting of the scene, as well as the two
        /// standard buttons.
        /// </summary>
        private void PlayScene_Paint(object sender, PaintEventArgs e)
        {
            switch (timeOfDay)
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
            e.Graphics.DrawImage(Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\RotateCCW.png"), new Point(this.Width - 53, 0));
        }
    }
}
