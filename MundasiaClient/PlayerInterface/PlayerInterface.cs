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

        private static Character drivingCharacter;

        private static Form host;
        private static PlayScene playScene;

        private static bool _eventsInitialized = false;

        public PlayerInterface() { }

        public static void Set(Form primaryForm, CharacterSelection initialScene)
        {
            host = primaryForm;
            drivingCharacter = initialScene.CentralCharacter;
            playScene = new PlayScene();
            playScene.Location = new Point(padding, padding);
            playScene.Size = new Size(host.ClientRectangle.Width - padding * 2, host.ClientRectangle.Height - padding * 2);
            host.Resize += host_Resize;
            if(!_eventsInitialized)
            {
                _eventsInitialized = true;
                playScene.TileSelected += playScene_TileSelected;
            }
            playScene.ViewCenterX = initialScene.CentralCharacter.LocationX;
            playScene.ViewCenterY = initialScene.CentralCharacter.LocationY;
            playScene.ViewCenterZ = initialScene.CentralCharacter.LocationZ;

            host.Controls.Add(playScene);
            playScene.Add(initialScene.visibleTiles);
            playScene.Add(initialScene.visibleCharacters);
        }

        static void playScene_TileSelected(object Sender, TileSelectEventArgs e)
        {
            playScene.ClearControls();
            PlaySceneControl ctl = new PlaySceneControl(playScene.ViewCenterX, playScene.ViewCenterY, playScene.ViewCenterZ, e.tile.PosX, e.tile.PosY, e.tile.PosZ, playScene, playScene.TopDirection, Mundasia.Interface.PlaySceneControlType.Move, 1);
            ctl.ControlSelected += playScene_ControlSelected;
            playScene.Add(ctl);
        }

        static void playScene_ControlSelected(object Sender, EventArgs e)
        {
            PlaySceneControl ctl = Sender as PlaySceneControl;
            string resp = ServiceConsumer.MoveCharacter(drivingCharacter.AccountName, drivingCharacter.CharacterName, ctl.GetObjectPositionX(), ctl.GetObjectPositionY(), ctl.GetObjectPositionZ());
            if(!String.IsNullOrEmpty(resp))
            {
                MapDelta changes = new MapDelta(resp);
                playScene.ClearControls();
                playScene.Remove(changes.RemovedTiles);
                playScene.Remove(changes.RemovedCharacters);
                playScene.Add(changes.AddedTiles);
                playScene.Add(changes.AddedCharacters);
                playScene.ManageChanges(changes.ChangedCharacters);
            }
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
