using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Mundasia.Objects;
using System.Drawing;
using System.IO;

namespace Mundasia.Interface
{
    [System.ComponentModel.DesignerCategory("")]
    public class InventoryForm: Form
    {
        int InventoryPadding = 20;
        Size IconSize = new Size(64, 64);
        ListView unequippedItems = new ListView();

        Panel NeckSlotPanel = new Panel();
        Panel LeftRingPanel = new Panel();
        Panel RightRingPanel = new Panel();
        Panel LeftHandPanel = new Panel();
        Panel RightHandPanel = new Panel();
        Panel ChestPanel = new Panel();
        Panel BeltPanel = new Panel();

        public InventoryForm(Character ch)
        {
            if(ch.Equipment.ContainsKey((int)InventorySlot.Chest))
            {
                ChestPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.Chest].Icon);
            }
            else
            {
                ChestPanel.BackgroundImage = GetInventoryIconByTag("EmptyChest");
            }

            if(ch.Equipment.ContainsKey((int)InventorySlot.Neck))
            {
                NeckSlotPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.Neck].Icon);
            }
            else
            {
                NeckSlotPanel.BackgroundImage = GetInventoryIconByTag("EmptyNeck");
            }

            if (ch.Equipment.ContainsKey((int)InventorySlot.Belt))
            {
                BeltPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.Belt].Icon);
            }
            else
            {
                BeltPanel.BackgroundImage = GetInventoryIconByTag("EmptyBelt");
            }

            if (ch.Equipment.ContainsKey((int)InventorySlot.LeftRing))
            {
                LeftRingPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.LeftRing].Icon);
            }
            else
            {
                LeftRingPanel.BackgroundImage = GetInventoryIconByTag("EmptyRing");
            }

            if (ch.Equipment.ContainsKey((int)InventorySlot.RightRing))
            {
                RightRingPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.RightRing].Icon);
            }
            else
            {
                RightRingPanel.BackgroundImage = GetInventoryIconByTag("EmptyRing");
            }

            if (ch.Equipment.ContainsKey((int)InventorySlot.LeftHand))
            {
                LeftHandPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.LeftHand].Icon);
            }
            else
            {
                LeftHandPanel.BackgroundImage = GetInventoryIconByTag("EmptyHand");
            }

            if (ch.Equipment.ContainsKey((int)InventorySlot.RightHand))
            {
                RightHandPanel.BackgroundImage = GetInventoryIconByTag(ch.Equipment[(int)InventorySlot.RightHand].Icon);
            }
            else
            {
                RightHandPanel.BackgroundImage = GetInventoryIconByTag("EmptyHand");
            }

            NeckSlotPanel.Size = IconSize;
            LeftRingPanel.Size = IconSize;
            RightRingPanel.Size = IconSize;
            LeftHandPanel.Size = IconSize;
            RightHandPanel.Size = IconSize;
            ChestPanel.Size = IconSize;
            BeltPanel.Size = IconSize;

            LeftHandPanel.Location = new Point(InventoryPadding, InventoryPadding * 2 + IconSize.Height);
            LeftRingPanel.Location = new Point(InventoryPadding, InventoryPadding * 3 + IconSize.Height * 2);

            NeckSlotPanel.Location = new Point(InventoryPadding * 2 + IconSize.Width, InventoryPadding);
            ChestPanel.Location = new Point(InventoryPadding * 2 + IconSize.Width, InventoryPadding * 2 + IconSize.Height);
            BeltPanel.Location = new Point(InventoryPadding * 2 + IconSize.Width, InventoryPadding * 3 + IconSize.Height * 2);

            RightHandPanel.Location = new Point(InventoryPadding * 3 + IconSize.Width * 2, InventoryPadding * 2 + IconSize.Height);
            RightRingPanel.Location = new Point(InventoryPadding * 3 + IconSize.Width * 2, InventoryPadding * 3 + IconSize.Height * 2);

            this.BackColor = Color.Black;
            this.Size = new Size(InventoryPadding * 4 + IconSize.Width * 3 + this.Size.Width - this.ClientRectangle.Size.Width, InventoryPadding * 4 + IconSize.Height * 3 + this.Size.Height - this.ClientRectangle.Size.Height);

            Controls.Add(NeckSlotPanel);
            Controls.Add(LeftRingPanel);
            Controls.Add(RightRingPanel);
            Controls.Add(LeftHandPanel);
            Controls.Add(RightHandPanel);
            Controls.Add(ChestPanel);
            Controls.Add(BeltPanel);

            this.FormClosed += InventoryForm_OnClosed;
            this.KeyDown += InventoryForm_KeyPress;
        }
        
        public static Image GetInventoryIconByTag(string Tag)
        {
            if(IconCache.ContainsKey(Tag))
            {
                return IconCache[Tag];
            }
            return Image.FromFile(Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Images" + Path.DirectorySeparatorChar + "Icons" + Path.DirectorySeparatorChar + "Items" + Path.DirectorySeparatorChar + Tag + ".png");
        }

        public static Dictionary<string, Image> IconCache = new Dictionary<string, Image>();

        enum InventorySlot
        {
            Chest = 0,
            Neck = 1,
            Belt = 2,
            LeftRing = 3,
            RightRing = 4,
            LeftHand = 5,
            RightHand = 6,
        }

        private void InventoryForm_OnClosed(object Sender, EventArgs e)
        {
            PlayerInterface.InventoryWindow = null;
        }

        private void InventoryForm_KeyPress(object Sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
