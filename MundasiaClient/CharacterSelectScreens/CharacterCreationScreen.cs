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
    public class CharacterCreationScreen: Panel
    {
        public CharacterCreationScreen() {}

        private static int selectionHeight = 250;
        private static int padding = 5;
        private static int indent = 20;

        private static CharacterCreationScreen _panel = new CharacterCreationScreen();
        private static Form _form;

        private static Panel _characterSheet = new Panel();
        private static Panel _currentEntry = new Panel();
        private static Panel _description = new Panel();

        private static Label _characterName = new Label();
        private static Label _characterSexRace = new Label();

        private static Label _traitsHead = new Label();
        private static Label _characterVirtue = new Label();
        private static Label _characterVice = new Label();

        private static Label _moralsHead = new Label();
        private static Label _characterMoralsAuthority = new Label();
        private static Label _characterMoralsCare = new Label();
        private static Label _characterMoralsFairness = new Label();
        private static Label _characterMoralsLoyalty = new Label();
        private static Label _characterMoralsTradition = new Label();

        private static Label _abilityHead = new Label();
        private static Label _characterProfession = new Label();
        private static Label _characterTalent = new Label();
        private static Label _characterHobby = new Label();

        private static Label _nameEntryLabel = new Label();
        private static TextBox _nameEntry = new TextBox();

        private static string _name;
        private static int _sex;
        private static int _race;
        private static int _traitVirtue;
        private static int _traitVice;
        private static int _moralsAuthority;
        private static int _moralsCare;
        private static int _moralsFairness;
        private static int _moralsLoyalty;
        private static int _moralsTradition;
        private static int _abilityProfession;
        private static int _abilityTalent;
        private static int _abilityHobby;

        public static void Set(Form primaryForm)
        {
            _name = "";
            _sex = -1;
            _race = -1;
            _traitVirtue = -1;
            _traitVice = -1;
            _moralsAuthority = -1;
            _moralsCare = -1;
            _moralsFairness = -1;
            _moralsLoyalty = -1;
            _moralsTradition = -1;
            _abilityProfession = -1;
            _abilityTalent = -1;
            _abilityHobby = -1;
            
            _form = primaryForm;
            _form.Resize += _form_Resize;

            int width = _form.ClientRectangle.Width / 2 - (padding * 2);
            int height = _form.ClientRectangle.Height - (padding * 2);

            Label derp = new Label();
            derp.ForeColor = Color.White;
            derp.BackColor = Color.Black;
            derp.Text = "*";
            derp.Size = derp.PreferredSize;

            _characterSheet.Height = height;
            _characterSheet.Width = width;
            _characterSheet.Location = new Point(padding, padding);
            _characterSheet.BorderStyle = BorderStyle.FixedSingle;
            _characterSheet.BackColor = Color.Black;
            _characterSheet.ForeColor = Color.White;

            _currentEntry.Height = selectionHeight;
            _currentEntry.Width = width;
            _currentEntry.Location = new Point(width + (3 * padding), padding);
            _currentEntry.BorderStyle = BorderStyle.FixedSingle;
            _currentEntry.BackColor = Color.Black;
            _currentEntry.ForeColor = Color.White;

            _description.Height = Math.Max(height - (2 * padding) - selectionHeight, 0);
            _description.Width = width;
            _description.Location = new Point(width + (3 * padding), selectionHeight + (3 * padding));
            _description.BorderStyle = BorderStyle.FixedSingle;
            _description.BackColor = Color.Black;
            _description.ForeColor = Color.White;

            _form.Controls.Add(_characterSheet);
            _form.Controls.Add(_currentEntry);
            _form.Controls.Add(_description);

            _nameEntry.TextChanged += _nameEntry_TextChanged;
            _characterName.Click += _characterName_Click;
            _characterSexRace.Click += _characterSexRace_Click;
            _characterVirtue.Click += _characterVirtue_Click;
            _characterVice.Click += _characterVice_Click;
            _characterMoralsAuthority.Click += _characterMoralsAuthority_Click;
            _characterMoralsCare.Click += _characterMoralsCare_Click;
            _characterMoralsFairness.Click += _characterMoralsFairness_Click;
            _characterMoralsLoyalty.Click += _characterMoralsLoyalty_Click;
            _characterMoralsTradition.Click += _characterMoralsTradition_Click;

            _abilityHead = new Label();
            _characterProfession = new Label();
            _characterTalent = new Label();
            _characterHobby = new Label();

            UpdateCharacterSheet();
            SetUnusedToPanel();
        }

        static void _characterMoralsTradition_Click(object sender, EventArgs e)
        {
            SetTraditionToPanel();
        }

        static void _characterMoralsLoyalty_Click(object sender, EventArgs e)
        {
            SetLoyaltyToPanel();
        }

        static void _characterMoralsFairness_Click(object sender, EventArgs e)
        {
            SetFairnessToPanel();
        }

        static void _characterMoralsCare_Click(object sender, EventArgs e)
        {
            SetCareToPanel();
        }

        static void _characterMoralsAuthority_Click(object sender, EventArgs e)
        {
            SetAuthorityToPanel();
        }

        static void _characterVice_Click(object sender, EventArgs e)
        {
            SetViceToPanel();
        }

        static void _characterVirtue_Click(object sender, EventArgs e)
        {
            SetVirtueToPanel();
        }

        static void _characterSexRace_Click(object sender, EventArgs e)
        {
            SetRaceSexToPanel();
        }

        static void _characterName_Click(object sender, EventArgs e)
        {
            SetNameToPanel();
        }

        static void _nameEntry_TextChanged(object sender, EventArgs e)
        {
            _name = _nameEntry.Text;
            _characterName.Text = _nameEntry.Text;
            _characterName.Size = _characterName.PreferredSize;
        }

        public static void Clear(Form primaryForm)
        {
            _form.Controls.Remove(_characterSheet);
            _form.Controls.Remove(_currentEntry);
            _form.Controls.Remove(_description);
            _form = null;
        }

        private static void UpdateCharacterSheet()
        {
            #region Define Text Contents of Labels
            if (_name == "") _characterName.Text = "No Name";
            else _characterName.Text = _name;

            if (_sex == 0) _characterSexRace.Text = StringLibrary.GetString(10);
            else if (_sex == 1) _characterSexRace.Text = StringLibrary.GetString(11);
            else _characterSexRace.Text = "No Sex";

            if(_race == -1)  _characterSexRace.Text += " No Race";
            else _characterSexRace.Text += " " + Race.GetRace((uint)_race).Name;

            if(_traitVirtue == -1) _characterVirtue.Text = "No Virtue";
            else _characterVirtue.Text = Virtue.GetVirtue((uint)_traitVirtue).Name;

            if(_traitVice == -1) _characterVice.Text = "No Vice";
            else _characterVice.Text = Vice.GetVice((uint)_traitVice).Name;

            if(_moralsAuthority == -1) _characterMoralsAuthority.Text = "No setting on moral authority";
            else _characterMoralsAuthority.Text = Authority.GetAuthority((uint)_moralsAuthority).Name;

            if(_moralsCare == -1) _characterMoralsCare.Text = "No setting on moral care";
            else _characterMoralsCare.Text = Care.GetCare((uint)_moralsCare).Name;

            if(_moralsFairness == -1) _characterMoralsFairness.Text = "No setting on moral fairness";
            else _characterMoralsFairness.Text = Fairness.GetFairness((uint)_moralsFairness).Name;

            if(_moralsLoyalty == -1) _characterMoralsLoyalty.Text = "No setting on moral loyalty";
            else _characterMoralsLoyalty.Text = Loyalty.GetLoyalty((uint)_moralsLoyalty).Name;

            if(_moralsTradition == -1) _characterMoralsTradition.Text = "No setting on moral tradition";
            else _characterMoralsTradition.Text = Tradition.GetTradition((uint)_moralsTradition).Name;

            if(_abilityProfession == -1) _characterProfession.Text = "No profession";
            else _characterProfession.Text = "Profession: " + Profession.GetProfession((uint)_abilityProfession).Name;

            if(_abilityTalent == -1) _characterTalent.Text = "No talent";
            else _characterTalent.Text = "Talent: " + Ability.GetAbility((uint)_abilityTalent).Name;
            
            if(_abilityHobby == -1) _characterHobby.Text = "No hobby";
            else _characterHobby.Text = "Hobby: " + Skill.GetSkill((uint)_abilityHobby).Name;

            _traitsHead.Text = "Traits";
            _moralsHead.Text = "Morals";
            _abilityHead.Text = "Abilities";
            #endregion

            #region Formatting and Positioning
            StyleLabel(_characterName);
            _characterName.Location = new Point(padding, padding);
            StyleLabel(_characterSexRace);
            _characterSexRace.Location = new Point(padding, _characterName.Location.Y + _characterName.Height);

            StyleLabel(_traitsHead);
            _traitsHead.Location = new Point(padding, _characterSexRace.Location.Y + (_characterSexRace.Height * 2));
            StyleLabel(_characterVirtue);
            _characterVirtue.Location = new Point(padding + indent, _traitsHead.Location.Y + _traitsHead.Height);
            StyleLabel(_characterVice);
            _characterVice.Location = new Point(padding + indent, _characterVirtue.Location.Y + _characterVirtue.Height);

            StyleLabel(_moralsHead);
            _moralsHead.Location = new Point(padding, _characterVice.Location.Y + (_characterVice.Height * 2));
            StyleLabel(_characterMoralsAuthority);
            _characterMoralsAuthority.Location = new Point(padding + indent, _moralsHead.Location.Y + _moralsHead.Height);
            StyleLabel(_characterMoralsCare);
            _characterMoralsCare.Location = new Point(padding + indent, _characterMoralsAuthority.Location.Y + _characterMoralsAuthority.Height);
            StyleLabel(_characterMoralsFairness);
            _characterMoralsFairness.Location = new Point(padding + indent, _characterMoralsCare.Location.Y + _characterMoralsCare.Height);
            StyleLabel(_characterMoralsLoyalty);
            _characterMoralsLoyalty.Location = new Point(padding + indent, _characterMoralsFairness.Location.Y + _characterMoralsFairness.Height);
            StyleLabel(_characterMoralsTradition);
            _characterMoralsTradition.Location = new Point(padding + indent, _characterMoralsLoyalty.Location.Y + _characterMoralsLoyalty.Height);

            StyleLabel(_abilityHead);
            _abilityHead.Location = new Point(padding, _characterMoralsTradition.Location.Y + (_characterMoralsTradition.Height * 2));
            StyleLabel(_characterProfession);
            _characterProfession.Location = new Point(padding + indent, _abilityHead.Location.Y + _abilityHead.Height);
            StyleLabel(_characterTalent);
            _characterTalent.Location = new Point(padding + indent, _characterProfession.Location.Y + _characterProfession.Height);
            StyleLabel(_characterHobby);
            _characterHobby.Location = new Point(padding + indent, _characterTalent.Location.Y + _characterTalent.Height);
            #endregion

            _characterName.MouseEnter += _clickableMouseOver;
            _characterSexRace.MouseEnter += _clickableMouseOver;
            _characterVirtue.MouseEnter += _clickableMouseOver;
            _characterVice.MouseEnter += _clickableMouseOver;
            _characterMoralsAuthority.MouseEnter += _clickableMouseOver;
            _characterMoralsCare.MouseEnter += _clickableMouseOver;
            _characterMoralsFairness.MouseEnter += _clickableMouseOver;
            _characterMoralsLoyalty.MouseEnter += _clickableMouseOver;
            _characterMoralsTradition.MouseEnter += _clickableMouseOver;
            _characterProfession.MouseEnter += _clickableMouseOver;
            _characterTalent.MouseEnter += _clickableMouseOver;
            _characterHobby.MouseEnter += _clickableMouseOver;

            _characterName.MouseLeave += _clickableMouseLeave;
            _characterSexRace.MouseLeave += _clickableMouseLeave;
            _characterVirtue.MouseLeave += _clickableMouseLeave;
            _characterVice.MouseLeave += _clickableMouseLeave;
            _characterMoralsAuthority.MouseLeave += _clickableMouseLeave;
            _characterMoralsCare.MouseLeave += _clickableMouseLeave;
            _characterMoralsFairness.MouseLeave += _clickableMouseLeave;
            _characterMoralsLoyalty.MouseLeave += _clickableMouseLeave;
            _characterMoralsTradition.MouseLeave += _clickableMouseLeave;
            _characterProfession.MouseLeave += _clickableMouseLeave;
            _characterTalent.MouseLeave += _clickableMouseLeave;
            _characterHobby.MouseLeave += _clickableMouseLeave;

            _characterSheet.Controls.Add(_characterName);
            _characterSheet.Controls.Add(_characterSexRace);
            _characterSheet.Controls.Add(_traitsHead);
            _characterSheet.Controls.Add(_characterVirtue);
            _characterSheet.Controls.Add(_characterVice);
            _characterSheet.Controls.Add(_moralsHead);
            _characterSheet.Controls.Add(_characterMoralsAuthority);
            _characterSheet.Controls.Add(_characterMoralsCare);
            _characterSheet.Controls.Add(_characterMoralsFairness);
            _characterSheet.Controls.Add(_characterMoralsLoyalty);
            _characterSheet.Controls.Add(_characterMoralsTradition);
            _characterSheet.Controls.Add(_abilityHead);
            _characterSheet.Controls.Add(_characterProfession);
            _characterSheet.Controls.Add(_characterTalent);
            _characterSheet.Controls.Add(_characterHobby);
        }
        
        private static void SetUnusedToPanel()
        {
            if (_name == "") SetNameToPanel();
            else if (_sex == -1 || _race == -1) SetRaceSexToPanel();
            else if (_traitVirtue == -1) SetVirtueToPanel();
            else if (_traitVice == -1) SetViceToPanel();
            else if (_moralsAuthority == -1) SetAuthorityToPanel();
            else if (_moralsCare == -1) SetCareToPanel();
            else if (_moralsFairness == -1) SetFairnessToPanel();
            else if (_moralsLoyalty == -1) SetLoyaltyToPanel();
            else if (_moralsTradition == -1) SetTraditionToPanel();
            else if (_abilityProfession == -1) SetProfessionToPanel();
            else if (_abilityTalent == -1) SetTalentToPanel();
            else if (_abilityHobby == -1) SetHobbyToPanel();
        }

        private static void ClearOld()
        {
            List<Control> list = new List<Control>();
            foreach (Control c in _currentEntry.Controls)
            {
                list.Add(c);
            }
            // Can't modify a collection while looping through it.
            foreach (Control c in list)
            {
                _currentEntry.Controls.Remove(c);
            }
        }

        private static void SetNameToPanel()
        {
            ClearOld();

            StyleLabel(_nameEntryLabel);
            _nameEntryLabel.Text = StringLibrary.GetString(12);
            _nameEntryLabel.Size = _nameEntryLabel.PreferredSize;
            _nameEntryLabel.Location = new Point(padding, padding);

            _nameEntry.Font = labelFont;
            _nameEntry.Height = _currentEntry.PreferredSize.Height;
            _nameEntry.Width = _currentEntry.Width - (padding * 2);
            _nameEntry.Location = new Point(padding, _nameEntryLabel.Location.Y + _nameEntryLabel.Height + padding);
            _nameEntry.BackColor = Color.DarkGray;
            _nameEntry.ForeColor = Color.White;

            _currentEntry.Controls.Add(_nameEntryLabel);
            _currentEntry.Controls.Add(_nameEntry);
            _nameEntry.Focus();
        }

        private static void SetRaceSexToPanel()
        {

        }

        private static void SetVirtueToPanel()
        {

        }

        private static void SetViceToPanel()
        {

        }

        private static void SetAuthorityToPanel()
        {

        }

        private static void SetCareToPanel()
        {

        }

        private static void SetFairnessToPanel()
        {

        }
        
        private static void SetLoyaltyToPanel()
        {

        }

        private static void SetTraditionToPanel()
        {

        }

        private static void SetProfessionToPanel()
        {

        }

        private static void SetTalentToPanel()
        {

        }

        private static void SetHobbyToPanel()
        {

        }

        static void _clickableMouseOver(object sender, EventArgs e)
        {
            Control over = sender as Control;
            if (over == null) return;

            over.ForeColor = Color.Yellow;
        }

        static void _clickableMouseLeave(object sender, EventArgs e)
        {
            Control over = sender as Control;
            if (over == null) return;

            over.ForeColor = Color.White;
        }

        static void _form_Resize(object sender, EventArgs e)
        {
            int width = _form.ClientRectangle.Width / 2 - (padding * 2);
            int height = _form.ClientRectangle.Height - (padding * 2);

            _characterSheet.Height = height;
            _characterSheet.Width = width;
            _characterSheet.Location = new Point(padding, padding);

            _currentEntry.Height = selectionHeight;
            _currentEntry.Width = width;
            _currentEntry.Location = new Point(width + (3 * padding), padding);

            _description.Height = Math.Max(height - (2 * padding) - selectionHeight, 0);
            _description.Width = width;
            _description.Location = new Point(width + (3 * padding), selectionHeight + (3 * padding));

            _nameEntry.Width = _currentEntry.Width - (padding * 2);
        }

        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 12.0f);

        private static void StyleLabel(Control toStyle)
        {
            toStyle.Size = toStyle.PreferredSize;
            toStyle.Font = labelFont;
            toStyle.ForeColor = Color.White;
            toStyle.BackColor = Color.Black;
            toStyle.Size = toStyle.PreferredSize;
        }
    }
}
