using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mundasia.Objects;
using Mundasia.Communication;

namespace Mundasia.Interface
{
    [System.ComponentModel.DesignerCategory("")]
    public class PlayerInterface: Panel
    {
        private static int padding = 5;

        private static Form host;
        private static PlayScene playScene;

        private static bool _eventsInitialized = false;

        public PlayerInterface() { }

        public static void Set(Form primaryForm, CharacterSelection initialScene)
        {
            host = primaryForm;
            playScene = new PlayScene();
            playScene.Location = new Point(padding, padding);
            playScene.Size = new Size(host.ClientRectangle.Width - padding * 2, host.ClientRectangle.Height - padding * 2);
            host.Resize += host_Resize;
            if(_eventsInitialized)
            {
                _eventsInitialized = true;
            }
            playScene.ViewCenterX = initialScene.CentralCharacter.LocationX;
            playScene.ViewCenterY = initialScene.CentralCharacter.LocationY;
            playScene.ViewCenterZ = initialScene.CentralCharacter.LocationZ;
            playScene.TileSelected += playScene_TileSelected;
            host.Controls.Add(playScene);
            playScene.Add(initialScene.visibleTiles);
            playScene.Add(initialScene.visibleCharacters);
            playScene.Add(DisplayCharacter.GetDisplayCharacter(initialScene.CentralCharacter));
            int foo = 1;
        }

        static void playScene_TileSelected(object Sender, TileSelectEventArgs e)
        {
            MessageBox.Show(e.tile.PosX + "," + e.tile.PosY + "," + e.tile.PosZ);
            return;
        }

        public static void Clear(Form primaryForm)
        {
            host.Controls.Clear();
        }

        static void host_Resize(object sender, EventArgs e)
        {
            playScene.Size = new Size(host.ClientRectangle.Width - padding * 2, host.ClientRectangle.Height - padding * 2);
        }
    }
}
