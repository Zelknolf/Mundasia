using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mundasia.Interface
{
    public class LoreViewer: Form
    {
        private static int padding = 5;
        private static int offset = 200;
        
        public LoreTOC CurrentLoreTOC;
        public LoreText CurrentLoreText;

        public LoreViewer()
        {
            this.Height = 600;
            this.Width = 800;
            Rectangle workingArea = Screen.FromControl(this).WorkingArea;
            this.Location = new Point
            {
                X = Math.Max(workingArea.X, workingArea.X + (workingArea.Width - this.Width) / 2),
                Y = Math.Max(workingArea.Y, workingArea.Y + (workingArea.Height - this.Height) / 2)
            };

            CurrentLoreText = new LoreText(this);
            CurrentLoreTOC = new LoreTOC(this);

            this.BackgroundImage = Image.FromFile(System.IO.Directory.GetCurrentDirectory() + "\\Images\\BackPattern.png");
            this.BackgroundImageLayout = ImageLayout.Tile;

            this.Text = "Mundasia Lore";

            this.Resize += LoreViewer_Resize;
        }

        void LoreViewer_Resize(object sender, EventArgs e)
        {
            CurrentLoreTOC.Height = this.ClientRectangle.Height - (padding * 2);
            CurrentLoreText.Width = this.ClientRectangle.Width - offset - padding;
            CurrentLoreText.Height = this.ClientRectangle.Height - (padding * 2);
        }
    }
}
