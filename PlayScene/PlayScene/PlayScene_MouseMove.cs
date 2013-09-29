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
    }
}
