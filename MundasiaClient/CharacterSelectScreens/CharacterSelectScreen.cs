using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Mundasia.Client;
using Mundasia.Objects;

namespace Mundasia.Interface
{
   [System.ComponentModel.DesignerCategory("")] 
    public class CharacterSelectScreen: Panel
    {
        public CharacterSelectScreen() { }
    
        private static int padding = 5;

        private static Panel _character = new Panel();
        private static Panel _message = new Panel();
        private static Panel _description = new Panel();

        private static Form _form;

        private static ListView _characterList = new ListView();
        private static ListView _messageList = new ListView();
        private static RichTextBox _descriptionBox = new RichTextBox();

        private static Label _createCharacter = new Label();

        private static bool _eventsInitialized = false;

        public static void Set(Form primaryForm)
        {
            _form = primaryForm;
            _form.Resize += _form_Resize;

            int width = _form.ClientRectangle.Width / 2 - (padding * 2);
            int height = _form.ClientRectangle.Height - (padding * 2);
            int halfHeight = (_form.ClientRectangle.Height / 2) - (padding * 2);

            if (_eventsInitialized == false)
            {
                StyleLabel(_character);
                StyleLabel(_message);
                StyleLabel(_description);
                StyleLabel(_createCharacter);
                StyleListView(_messageList);

                _characterList.ItemSelectionChanged += _characterList_ItemSelectionChanged;
                _createCharacter.MouseEnter += _createCharacter_MouseEnter;
                _createCharacter.MouseLeave += _createCharacter_MouseLeave;
                _createCharacter.Click += _createCharacter_Click;

                _eventsInitialized = true;
            }

            _character.Size = new Size(width, halfHeight);
            _character.Location = new Point(padding, padding);
            _character.BorderStyle = BorderStyle.FixedSingle;

            _message.Size = new Size(width, halfHeight);
            _message.Location = new Point(padding, halfHeight + (padding * 3));
            _message.BorderStyle = BorderStyle.FixedSingle;

            _description.Size = new Size(width, height);
            _description.Location = new Point(width + (padding * 3), padding);
            _description.BorderStyle = BorderStyle.FixedSingle;

            _createCharacter.Text = StringLibrary.GetString(34);
            _createCharacter.Size = _createCharacter.PreferredSize;
            _createCharacter.Location = new Point(_character.ClientRectangle.Width - _createCharacter.Width, _character.ClientRectangle.Height - _createCharacter.Height);
            _character.Controls.Add(_createCharacter);

            _characterList.Size = new Size(_character.ClientRectangle.Size.Width, _character.ClientRectangle.Size.Height - _createCharacter.Height);
            StyleListView(_characterList);
            _character.Controls.Add(_characterList);

            _messageList.Size = _message.ClientRectangle.Size;
            _message.Controls.Add(_messageList);

            _form.Controls.Add(_character);
            _form.Controls.Add(_message);
            _form.Controls.Add(_description);
            _description.Controls.Add(_descriptionBox);
            StyleLabel(_descriptionBox);
            _descriptionBox.Size = _description.ClientRectangle.Size;

            PopulateCharacterList();
        }


       
        public static void Clear(Form primaryForm)
        {
            _character.Controls.Clear();
            _message.Controls.Clear();
            _description.Controls.Clear();
            _form.Controls.Clear();
        }

        public static void PopulateCharacterList()
        {
            string ch = Mundasia.Communication.ServiceConsumer.ListCharacters();
            if(String.IsNullOrWhiteSpace(ch))
            {
                return;
            }
            string[] chs = ch.Split(new char[] { '|' });
            foreach(string c in chs)
            {
                if(!String.IsNullOrWhiteSpace(c))
                {
                    ListViewItem toAdd = new ListViewItem(new string[] { c, c });
                    StyleListViewItem(toAdd);
                    _characterList.Items.Add(toAdd);
                }
            }
        }

        private static void SetCharacterDetails(Character cha)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(cha.CharacterName);
            sb.Append(Environment.NewLine);
            if(cha.Sex == 0)
            {
                sb.Append(StringLibrary.GetString(10));
            }
            else
            {
                sb.Append(StringLibrary.GetString(11));
            }
            sb.Append(" ");
            sb.Append(Race.GetRace(cha.CharacterRace).Name);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(StringLibrary.GetString(26));
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(Virtue.GetVirtue(cha.CharacterVirtue).Name);
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(Vice.GetVice(cha.CharacterVice).Name);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(StringLibrary.GetString(27));
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(Authority.GetAuthority(cha.MoralsAuthority).Name);
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(Care.GetCare(cha.MoralsCare).Name);
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(Fairness.GetFairness(cha.MoralsFairness).Name);
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(Loyalty.GetLoyalty(cha.MoralsLoyalty).Name);
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(Tradition.GetTradition(cha.MoralsTradition).Name);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(StringLibrary.GetString(28));
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(StringLibrary.GetString(31) + ": ");
            sb.Append(Profession.GetProfession(cha.CharacterProfession).Name);
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(StringLibrary.GetString(33) + ": ");
            sb.Append(Skill.GetSkill(cha.CharacterHobby).Name);
            sb.Append(Environment.NewLine);
            sb.Append("    ");
            sb.Append(StringLibrary.GetString(32) + ": ");
            sb.Append(Ability.GetAbility(cha.CharacterTalent).Name);
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(StringLibrary.GetString(29));
            foreach(KeyValuePair<uint, int> keyval in cha.Abilities)
            {
                sb.Append(Environment.NewLine);
                sb.Append("    ");
                sb.Append(Ability.GetAbility(keyval.Key).Name + ": " + keyval.Value.ToString());
            }
            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);
            sb.Append(StringLibrary.GetString(30));
            foreach (KeyValuePair<uint, int> keyval in cha.Skills)
            {
                sb.Append(Environment.NewLine);
                sb.Append("    ");
                sb.Append(Skill.GetSkill(keyval.Key).Name + ": " + keyval.Value.ToString());
            }

            _descriptionBox.Text = sb.ToString();
        }

        static void _characterList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                string ret = Mundasia.Communication.ServiceConsumer.CharacterDetails(e.Item.SubItems[0].Text);
                Character ch = new Character(ret);
                SetCharacterDetails(ch);
            }
        }

        static void _createCharacter_Click(object sender, EventArgs e)
        {
            Form host = _form;
            Clear(_form);
            CharacterCreationScreen.Set(_form);
        }

        static void _createCharacter_MouseLeave(object sender, EventArgs e)
        {
            _createCharacter.ForeColor = Color.White;
        }

        static void _createCharacter_MouseEnter(object sender, EventArgs e)
        {
            _createCharacter.ForeColor = Color.Yellow;
        }

        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 12.0f);
        
        private static void StyleListView(ListView listView)
        {
            listView.Clear();
            listView.Columns.Add("");
            listView.Columns.Add("");
            listView.View = View.Details;
            listView.FullRowSelect = true;
            listView.BackColor = Color.Black;
            listView.ForeColor = Color.White;
            listView.HeaderStyle = ColumnHeaderStyle.None;
            listView.Font = labelFont;
            listView.Columns[0].Width = 0;
            listView.Columns[1].Width = listView.ClientRectangle.Width - SystemInformation.VerticalScrollBarWidth;
        }
       
        private static void StyleLabel(Control toStyle)
        {
            toStyle.Size = toStyle.PreferredSize;
            toStyle.Font = labelFont;
            toStyle.ForeColor = Color.White;
            toStyle.BackColor = Color.Black;
            toStyle.Size = toStyle.PreferredSize;
        }

        private static void StyleListViewItem(ListViewItem item)
        {
            item.BackColor = Color.Black;
            item.ForeColor = Color.White;
            item.Font = labelFont;
        }

        static void _form_Resize(object sender, EventArgs e)
        {
            int width = _form.ClientRectangle.Width / 2 - (padding * 2);
            int height = _form.ClientRectangle.Height - (padding * 2);
            int halfHeight = (_form.ClientRectangle.Height / 2) - (padding * 2);

            _character.Size = new Size(width, halfHeight);
            _character.Location = new Point(padding, padding);
            _characterList.Size = new Size(_character.ClientRectangle.Size.Width, _character.ClientRectangle.Size.Height - _createCharacter.Height);

            _message.Size = new Size(width, halfHeight);
            _message.Location = new Point(padding, halfHeight + (padding * 3));

            _description.Size = new Size(width, height);
            _description.Location = new Point(width + (padding * 3), padding);

            _characterList.Size = _character.ClientRectangle.Size;
            _messageList.Size = _message.ClientRectangle.Size;
        }
    }
}
