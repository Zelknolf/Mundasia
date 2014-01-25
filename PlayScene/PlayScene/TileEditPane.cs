using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using System.Drawing;
using System.Windows.Forms;

using Mundasia.Objects;

namespace Mundasia.Interface
{
    public class TileEditPane : Panel
    {
        public static int padding = 5;
        public TileEditPane(Tile tile)
        {
            this.BackColor = Color.Black;
            shownTile = tile;

            Label labX = new Label();
            labX.BackColor = Color.Black;
            labX.ForeColor = Color.White;
            labX.Text = "X: ";
            labX.Size = labX.PreferredSize;
            labX.Location = new Point(padding, padding);
            this.Controls.Add(labX);

            X = new NumericUpDown();
            X.Minimum = Int32.MinValue;
            X.Maximum = Int32.MaxValue;
            X.BackColor = Color.Black;
            X.ForeColor = Color.White;
            if(tile != null) X.Value = tile.PosX;
            X.Height = X.PreferredHeight;
            X.Width = Math.Max(0, this.ClientRectangle.Width - labX.Width - (padding * 3));
            X.Location = new Point(labX.Location.X + labX.Width + padding, labX.Location.Y);
            this.Controls.Add(X);

            Label labY = new Label();
            labY.BackColor = Color.Black;
            labY.ForeColor = Color.White;
            labY.Text = "Y: ";
            labY.Size = labY.PreferredSize;
            labY.Location = new Point(padding, labX.Location.Y + labX.Height + padding);
            this.Controls.Add(labY);

            Y = new NumericUpDown();
            Y.Minimum = Int32.MinValue;
            Y.Maximum = Int32.MaxValue;
            Y.BackColor = Color.Black;
            Y.ForeColor = Color.White;
            if(tile != null) Y.Value = tile.PosY;
            Y.Height = Y.PreferredHeight;
            Y.Width = Math.Max(0, this.ClientRectangle.Width - labY.Width - (padding * 3));
            Y.Location = new Point(labY.Location.X + labY.Width + padding, labY.Location.Y);
            this.Controls.Add(Y);

            Label labZ = new Label();
            labZ.BackColor = Color.Black;
            labZ.ForeColor = Color.White;
            labZ.Text = "Z: ";
            labZ.Size = labZ.PreferredSize;
            labZ.Location = new Point(padding, labY.Location.Y + labY.Height + padding);
            this.Controls.Add(labZ);

            Z = new NumericUpDown();
            Z.Minimum = Int32.MinValue;
            Z.Maximum = Int32.MaxValue;
            Z.BackColor = Color.Black;
            Z.ForeColor = Color.White;
            if(tile != null) Z.Value = tile.PosZ;
            Z.Height = Z.PreferredHeight;
            Z.Width = Math.Max(0, this.ClientRectangle.Width - labZ.Width - (padding * 3));
            Z.Location = new Point(labZ.Location.X + labZ.Width + padding, labZ.Location.Y);
            this.Controls.Add(Z);
        }

        public Tile shownTile;

        public NumericUpDown X;
        public NumericUpDown Y;
        public NumericUpDown Z;

        public void SetTile(Tile tile)
        {
            shownTile = tile;
            if (tile != null)
            {
                X.Value = tile.PosX;
                Y.Value = tile.PosY;
                Z.Value = tile.PosZ;
            }
            else
            {
                Z.Value = 0;
                Y.Value = 0;
                X.Value = 0;
            }
        }

        public void RefreshDisplay()
        {
            if (shownTile != null)
            {
                X.Value = shownTile.PosX;
                Y.Value = shownTile.PosY;
                Z.Value = shownTile.PosZ;
            }
            else
            {
                Z.Value = 0;
                Y.Value = 0;
                X.Value = 0;
            }
        }
    }
}
