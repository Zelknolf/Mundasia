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

        private static Label _next = new Label();

        private static Label _nameEntryLabel = new Label();
        private static TextBox _nameEntry = new TextBox();

        private static Label _female = new Label();
        private static Label _male = new Label();

        private static ListView _raceList = new ListView();

        private static ListView _virtueList = new ListView();
        private static ListView _viceList = new ListView();

        private static ListView _authorityList = new ListView();
        private static ListView _careList = new ListView();
        private static ListView _fairnessList = new ListView();
        private static ListView _loyaltyList = new ListView();
        private static ListView _traditionList = new ListView();

        private static ListView _professionList = new ListView();
        private static ListView _talentList = new ListView();
        private static ListView _hobbyList = new ListView();

        private static RichTextBox _descriptionText = new RichTextBox();

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
            _description.Controls.Add(_descriptionText);

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
            _characterHobby.Click += _characterHobby_Click;
            _characterTalent.Click += _characterTalent_Click;
            _characterProfession.Click += _characterProfession_Click;

            _descriptionText.Text = " "; // RichTextBox wants to be 0x0 unless it has contents
            _descriptionText.Location = new Point(padding, padding);
            _descriptionText.ReadOnly = true;
            _descriptionText.BorderStyle = BorderStyle.None;
            _descriptionText.Multiline = true;
            _descriptionText.Height = Math.Max(height - (4 * padding) - selectionHeight, 0);
            _descriptionText.Width = width - (padding * 2);
            _descriptionText.WordWrap = true;
            
            StyleLabel(_descriptionText);

            UpdateCharacterSheet();
            SetUnusedToPanel();
        }

        static void _characterProfession_Click(object sender, EventArgs e)
        {
            SetProfessionToPanel();
        }

        static void _characterTalent_Click(object sender, EventArgs e)
        {
            SetTalentToPanel();
        }

        static void _characterHobby_Click(object sender, EventArgs e)
        {
            SetHobbyToPanel();
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

            UpdateRaceSexText();

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

            _next.Text = StringLibrary.GetString(13);

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
            StyleLabel(_next);
            _next.Location = new Point(_characterSheet.Width - _next.Width - padding, _characterSheet.Height - _next.Height - padding);
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
            _female.MouseEnter += _clickableMouseOver;
            _male.MouseEnter += _clickableMouseOver;
            _next.MouseEnter += _clickableMouseOver;

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
            _female.MouseLeave += _clickableMouseLeave;
            _male.MouseLeave += _clickableMouseLeave;
            _next.MouseLeave += _clickableMouseLeave;

            _female.Click += _female_Click;
            _male.Click += _male_Click;
            _next.Click += _next_Click;

            _raceList.ItemSelectionChanged += _raceList_ItemSelectionChanged;
            _virtueList.ItemSelectionChanged += _virtueList_ItemSelectionChanged;
            _viceList.ItemSelectionChanged += _viceList_ItemSelectionChanged;
            _authorityList.ItemSelectionChanged += _authorityList_ItemSelectionChanged;
            _careList.ItemSelectionChanged += _careList_ItemSelectionChanged;
            _fairnessList.ItemSelectionChanged += _fairnessList_ItemSelectionChanged;
            _loyaltyList.ItemSelectionChanged += _loyaltyList_ItemSelectionChanged;
            _traditionList.ItemSelectionChanged += _traditionList_ItemSelectionChanged;
            _talentList.ItemSelectionChanged += _talentList_ItemSelectionChanged;
            _professionList.ItemSelectionChanged += _professionList_ItemSelectionChanged;
            _hobbyList.ItemSelectionChanged += _hobbyList_ItemSelectionChanged;

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
            _characterSheet.Controls.Add(_next);
        }

        static void _next_Click(object sender, EventArgs e)
        {
            if(String.IsNullOrEmpty(_name) ||
                _sex == -1 ||
                _race == -1 ||
                _traitVirtue == -1 ||
                _traitVice == -1 ||
                _moralsAuthority == -1 ||
                _moralsCare == -1 ||
                _moralsFairness == -1 ||
                _moralsLoyalty == -1 ||
                _moralsTradition == -1 ||
                _abilityProfession == -1 ||
                _abilityTalent == -1 ||
                _abilityHobby == -1)
            {
                SetUnusedToPanel();
                return;
            }

            string creat = Mundasia.Communication.ServiceConsumer.CreateCharacter(_name, 
                                                                _moralsAuthority, 
                                                                _moralsCare, 
                                                                _moralsFairness, 
                                                                _abilityHobby, 
                                                                _moralsLoyalty, 
                                                                _abilityProfession, 
                                                                _race, 
                                                                _sex, 
                                                                _abilityTalent, 
                                                                _moralsTradition,
                                                                _traitVice, 
                                                                _traitVirtue);
            if(!creat.Contains("Success"))
            {
                MessageBox.Show(creat);
            }
            else
            {
                Form host = _form;
                Clear(host);
                CharacterSelectScreen.Set(host);
            }
        }

        static void _hobbyList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                int nHobby;
                if (int.TryParse(e.Item.SubItems[0].Text, out nHobby))
                {
                    _abilityHobby = nHobby;
                    Skill skill = Skill.GetSkill((uint)_abilityHobby);
                    _characterHobby.Text = skill.Name;
                    _characterHobby.Size = _characterHobby.PreferredSize;
                    _descriptionText.Text = StringLibrary.GetString(skill.Description);
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                }
            }
        }

        static void _professionList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                int nProfession;
                if(int.TryParse(e.Item.SubItems[0].Text, out nProfession))
                {
                    _abilityProfession = nProfession;
                    Profession profession = Profession.GetProfession((uint)_abilityProfession);
                    _characterProfession.Text = profession.Name;
                    _characterProfession.Size = _characterProfession.PreferredSize;
                    _descriptionText.Text = StringLibrary.GetString(profession.Description) + Environment.NewLine + 
                        Environment.NewLine + StringLibrary.GetString(28) + ": " + Ability.GetAbility(profession.PrimaryAbility).Name +
                        Environment.NewLine + StringLibrary.GetString(30) + ": " + Skill.GetSkill(profession.SkillOne).Name + ", " + Skill.GetSkill(profession.SkillTwo).Name + ", " + Skill.GetSkill(profession.SkillThree).Name;
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                }
            }
        }

        static void _talentList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                int nAbil;
                if (int.TryParse(e.Item.SubItems[0].Text, out nAbil))
                {
                    _abilityTalent = nAbil;
                    Ability ability = Ability.GetAbility((uint)_abilityTalent);
                    _characterTalent.Text = ability.Name;
                    _characterTalent.Size = _characterTalent.PreferredSize;
                    _descriptionText.Text = StringLibrary.GetString(ability.Description);
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                }
            }
        }

        static void _traditionList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                int nTrad;
                if (int.TryParse(e.Item.SubItems[0].Text, out nTrad))
                {
                    _moralsTradition = nTrad;
                    Tradition trad = Tradition.GetTradition((uint)_moralsTradition);
                    _characterMoralsTradition.Text = trad.Name;
                    _characterMoralsTradition.Size = _characterMoralsTradition.PreferredSize;
                    _descriptionText.Text = StringLibrary.GetString(trad.Description);
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                }
            }
        }

        static void _loyaltyList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                int nLoyal;
                if (int.TryParse(e.Item.SubItems[0].Text, out nLoyal))
                {
                    _moralsLoyalty = nLoyal;
                    Loyalty loyal = Loyalty.GetLoyalty((uint)_moralsLoyalty);
                    _characterMoralsLoyalty.Text = loyal.Name;
                    _characterMoralsLoyalty.Size = _characterMoralsLoyalty.PreferredSize;
                    _descriptionText.Text = StringLibrary.GetString(loyal.Description);
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                }
            }
        }

        static void _fairnessList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                int nFair;
                if (int.TryParse(e.Item.SubItems[0].Text, out nFair))
                {
                    _moralsFairness = nFair;
                    Fairness fair = Fairness.GetFairness((uint)_moralsFairness);
                    _characterMoralsFairness.Text = fair.Name;
                    _characterMoralsFairness.Size = _characterMoralsFairness.PreferredSize;
                    _descriptionText.Text = StringLibrary.GetString(fair.Description);
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                }
            }
        }

        static void _careList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                int nCare;
                if (int.TryParse(e.Item.SubItems[0].Text, out nCare))
                {
                    _moralsCare = nCare;
                    Care care = Care.GetCare((uint)_moralsCare);
                    _characterMoralsCare.Text = care.Name;
                    _characterMoralsCare.Size = _characterMoralsCare.PreferredSize;
                    _descriptionText.Text = StringLibrary.GetString(care.Description);
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                }
            }
        }

        static void _authorityList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                int nAuth;
                if (int.TryParse(e.Item.SubItems[0].Text, out nAuth))
                {
                    _moralsAuthority = nAuth;
                    Authority auth = Authority.GetAuthority((uint)_moralsAuthority);
                    _characterMoralsAuthority.Text = auth.Name;
                    _characterMoralsAuthority.Size = _characterMoralsAuthority.PreferredSize;
                    _descriptionText.Text = StringLibrary.GetString(auth.Description);
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                }
            }
        }

        static void _viceList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                int nVice;
                if (int.TryParse(e.Item.SubItems[0].Text, out nVice))
                {
                    _traitVice = nVice;
                    Vice vice = Vice.GetVice((uint)_traitVice);
                    _characterVice.Text = vice.Name;
                    _characterVice.Size = _characterVice.PreferredSize;
                    if (_traitVirtue == _traitVice)
                    {
                        _traitVirtue = -1;
                        _characterVirtue.Text = "No Virtue";
                        _characterVirtue.Size = _characterVirtue.PreferredSize;
                    }
                    _descriptionText.Text = StringLibrary.GetString(vice.Description);
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                }
            }
        }

        static void _virtueList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                int nVirtue;
                if (int.TryParse(e.Item.SubItems[0].Text, out nVirtue))
                {
                    _traitVirtue = nVirtue;
                    Virtue virt = Virtue.GetVirtue((uint)_traitVirtue);
                    _characterVirtue.Text = virt.Name;
                    _characterVirtue.Size = _characterVirtue.PreferredSize;
                    if(_traitVirtue == _traitVice)
                    {
                        _traitVice = -1;
                        _characterVice.Text = "No Vice";
                        _characterVice.Size = _characterVice.PreferredSize;
                    }
                    _descriptionText.Text = StringLibrary.GetString(virt.Description);
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                }
            }
        }

        static void _raceList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if(e.IsSelected)
            {
                int nRace;
                if(int.TryParse(e.Item.SubItems[0].Text, out nRace))
                {
                    _race = nRace;
                    UpdateRaceSexText();
                    _descriptionText.Text = StringLibrary.GetString(Race.GetRace((uint)_race).Description);
                    _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
                    _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
                }
            }
        }

        static void UpdateRaceSexText()
        {
            if (_sex == 0) _characterSexRace.Text = StringLibrary.GetString(10);
            else if (_sex == 1) _characterSexRace.Text = StringLibrary.GetString(11);
            else _characterSexRace.Text = "No Sex";

            if (_race == -1) _characterSexRace.Text += " No Race";
            else _characterSexRace.Text += " " + Race.GetRace((uint)_race).Name;

            _characterSexRace.Size = _characterSexRace.PreferredSize;
        }

        static void _male_Click(object sender, EventArgs e)
        {
            _sex = 1;
            _male.BorderStyle = BorderStyle.FixedSingle;
            _male.BackColor = Color.DarkGray;
            _female.BorderStyle = BorderStyle.None;
            _female.BackColor = Color.Black;
            _male.Size = _male.PreferredSize;
            UpdateRaceSexText();
        }

        static void _female_Click(object sender, EventArgs e)
        {
            _sex = 0;
            _male.BorderStyle = BorderStyle.None;
            _male.BackColor = Color.Black;
            _female.BorderStyle = BorderStyle.FixedSingle;
            _female.BackColor = Color.DarkGray;
            _female.Size = _female.PreferredSize;
            UpdateRaceSexText();
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
            _descriptionText.Text = StringLibrary.GetString(14);
            _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
            _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
        }

        private static void SetRaceSexToPanel()
        {
            ClearOld();
            StyleLabel(_female);
            _female.Text = StringLibrary.GetString(10);
            _female.Size = _female.PreferredSize;
            _female.Location = new Point(padding, padding);
            if (_sex == 0)
            {
                _female.BorderStyle = BorderStyle.FixedSingle;
                _female.BackColor = Color.DarkGray;
            }
            else
            {
                _female.BorderStyle = BorderStyle.None;
                _female.BackColor = Color.Black;
            }

            StyleLabel(_male);
            _male.Text = StringLibrary.GetString(11);
            _male.Size = _male.PreferredSize;
            _male.Location = new Point((padding * 2) + _female.Location.X + _female.Width, padding);
            if (_sex == 1)
            {
                _male.BorderStyle = BorderStyle.FixedSingle;
                _male.BackColor = Color.DarkGray;
            }
            else
            {
                _male.BorderStyle = BorderStyle.None;
                _male.BackColor = Color.Black;
            }

            _raceList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 3) - _female.Height);
            StyleListView(_raceList);


            foreach(Race race in Race.GetRaces())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { race.Id.ToString(), race.Name });
                StyleListViewItem(toAdd);
                _raceList.Items.Add(toAdd);
            }
            _raceList.Location = new Point(padding, _female.Height + _female.Location.Y + padding);

            _currentEntry.Controls.Add(_male);
            _currentEntry.Controls.Add(_female);
            _currentEntry.Controls.Add(_raceList);

            _descriptionText.Text = StringLibrary.GetString(15);
        }

        private static void SetVirtueToPanel()
        {
            ClearOld();
            _virtueList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 2));
            _virtueList.Location = new Point(padding, padding);
            StyleListView(_virtueList);

            foreach(Virtue virtue in Virtue.GetVirtues())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { virtue.Id.ToString(), virtue.Name });
                StyleListViewItem(toAdd);
                _virtueList.Items.Add(toAdd);
            }

            _currentEntry.Controls.Add(_virtueList);

            _descriptionText.Text = StringLibrary.GetString(16);
        }

        private static void SetViceToPanel()
        {
            ClearOld();
            _viceList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 2));
            _viceList.Location = new Point(padding, padding);
            StyleListView(_viceList);

            foreach(Vice vice in Vice.GetVices())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { vice.Id.ToString(), vice.Name });
                StyleListViewItem(toAdd);
                _viceList.Items.Add(toAdd);
            }

            _currentEntry.Controls.Add(_viceList);

            _descriptionText.Text = StringLibrary.GetString(17);
        }

        private static void SetAuthorityToPanel()
        {
            ClearOld();
            _authorityList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 2));
            _authorityList.Location = new Point(padding, padding);
            StyleListView(_authorityList);

            foreach (Authority auth in Authority.GetAuthorities())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { auth.Id.ToString(), auth.Name });
                StyleListViewItem(toAdd);
                _authorityList.Items.Add(toAdd);
            }

            _currentEntry.Controls.Add(_authorityList);

            _descriptionText.Text = StringLibrary.GetString(18);
        }

        private static void SetCareToPanel()
        {
            ClearOld();
            _careList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 2));
            _careList.Location = new Point(padding, padding);
            StyleListView(_careList);

            foreach (Care care in Care.GetCares())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { care.Id.ToString(), care.Name });
                StyleListViewItem(toAdd);
                _careList.Items.Add(toAdd);
            }

            _currentEntry.Controls.Add(_careList);

            _descriptionText.Text = StringLibrary.GetString(19);
        }

        private static void SetFairnessToPanel()
        {
            ClearOld();
            _fairnessList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 2));
            _fairnessList.Location = new Point(padding, padding);
            StyleListView(_fairnessList);

            foreach (Fairness fair in Fairness.GetFairnesses())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { fair.Id.ToString(), fair.Name });
                StyleListViewItem(toAdd);
                _fairnessList.Items.Add(toAdd);
            }

            _currentEntry.Controls.Add(_fairnessList);

            _descriptionText.Text = StringLibrary.GetString(20);
        }
        
        private static void SetLoyaltyToPanel()
        {
            ClearOld();
            _loyaltyList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 2));
            _loyaltyList.Location = new Point(padding, padding);
            StyleListView(_loyaltyList);

            foreach (Loyalty loyal in Loyalty.GetLoyalties())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { loyal.Id.ToString(), loyal.Name });
                StyleListViewItem(toAdd);
                _loyaltyList.Items.Add(toAdd);
            }

            _currentEntry.Controls.Add(_loyaltyList);

            _descriptionText.Text = StringLibrary.GetString(21);
        }

        private static void SetTraditionToPanel()
        {
            ClearOld();
            _traditionList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 2));
            _traditionList.Location = new Point(padding, padding);
            StyleListView(_traditionList);

            foreach (Tradition trad in Tradition.GetTraditions())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { trad.Id.ToString(), trad.Name });
                StyleListViewItem(toAdd);
                _traditionList.Items.Add(toAdd);
            }

            _currentEntry.Controls.Add(_traditionList);

            _descriptionText.Text = StringLibrary.GetString(22);
        }

        private static void SetProfessionToPanel()
        {
            ClearOld();

            _professionList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 2));
            _professionList.Location = new Point(padding, padding);
            StyleListView(_professionList);

            foreach(Profession prof in Profession.GetProfessions())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { prof.Id.ToString(), prof.Name });
                StyleListViewItem(toAdd);
                _professionList.Items.Add(toAdd);
            }

            _currentEntry.Controls.Add(_professionList);

            _descriptionText.Text = StringLibrary.GetString(23);
        }

        private static void SetTalentToPanel()
        {
            ClearOld();

            _talentList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 2));
            _talentList.Location = new Point(padding, padding);
            StyleListView(_talentList);

            foreach(Ability ab in Ability.GetAbilities())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { ab.Id.ToString(), ab.Name });
                StyleListViewItem(toAdd);
                _talentList.Items.Add(toAdd);
            }

            _currentEntry.Controls.Add(_talentList);

            _descriptionText.Text = StringLibrary.GetString(24);
        }

        private static void SetHobbyToPanel()
        {            
            ClearOld();

            _hobbyList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 2));
            _hobbyList.Location = new Point(padding, padding);
            StyleListView(_hobbyList);

            foreach(Skill skill in Skill.GetSkills())
            {
                ListViewItem toAdd = new ListViewItem(new string[] { skill.Id.ToString(), skill.Name });
                StyleListViewItem(toAdd);
                _hobbyList.Items.Add(toAdd);
            }

            _currentEntry.Controls.Add(_hobbyList);

            _descriptionText.Text = StringLibrary.GetString(25);
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
            _raceList.Size = new Size(_currentEntry.Width - (padding * 2), _currentEntry.Height - (padding * 3) - _female.Height);

            _descriptionText.Height = Math.Max(_description.Height - (padding * 2), 0);
            _descriptionText.Width = Math.Max(0, _description.Width - (padding * 2));
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

        private static void StyleListViewItem(ListViewItem item)
        {
            item.BackColor = Color.Black;
            item.ForeColor = Color.White;
            item.Font = labelFont;
        }
    }
}
