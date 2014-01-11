using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mundasia;
using Mundasia.Objects;

namespace CharacterEditor
{
    public partial class CharacterSelect : Form
    {
        private ListView characterList = new ListView();
        public CharacterSelect()
        {
            Text = "Character select";

            characterList.Height = ClientRectangle.Height;
            characterList.Width = ClientRectangle.Width;

            characterList.Columns.Add("Account");
            characterList.Columns.Add("Name");
            characterList.View = View.Details;
            characterList.FullRowSelect = true;
            characterList.Columns[1].Width = characterList.Width - characterList.Columns[0].Width - 20;
            characterList.DoubleClick += characterList_DoubleClick;

            foreach(string file in Directory.GetFiles(Directory.GetCurrentDirectory(), "*.aco", SearchOption.AllDirectories))
            {
                string s = file;
                string[] st = file.Split('\\');

                string accName = st[st.Length - 1].Split('.')[0];

                Account Loading = Account.LoadAccount(accName);

                foreach(string cha in Loading.Characters)
                {
                    ListViewItem adding = new ListViewItem(new string[] { accName, cha });
                    characterList.Items.Add(adding);
                }
            }

            Controls.Add(characterList);
        }

        void characterList_DoubleClick(object sender, EventArgs e)
        {
            if(characterList.SelectedItems.Count > 0)
            {
                Account clickedAccount = Account.LoadAccount(characterList.SelectedItems[0].SubItems[0].Text);
                Character clickedCharacter = clickedAccount.LoadCharacter(characterList.SelectedItems[0].SubItems[1].Text);
                CharacterPanel panel = new CharacterPanel(clickedCharacter, false);
                panel.FormClosing += panel_FormClosing;
                panel.Show();
            }
        }

        void panel_FormClosing(object sender, FormClosingEventArgs e)
        {
            CharacterPanel characterPanel = sender as CharacterPanel;
            Account acc = Account.LoadAccount(characterPanel.ShownCharacter.AccountName);
            acc.SaveCharacter(characterPanel.ShownCharacter);
        }
    }
}
