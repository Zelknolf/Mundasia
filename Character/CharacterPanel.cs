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

namespace Mundasia.Objects
{
    public class CharacterPanel : Form
    {
        public Character ShownCharacter;
        Label LabelName = new Label();
        Label LabelSexRace = new Label();

        Label LabelAbilityHead = new Label();
        Label LabelPhysicalHead = new Label();
        Label LabelMentalHead = new Label();
        Label LabelSocialHead = new Label();
        Label LabelSupernaturalHead = new Label();

        Label LabelSkillHead = new Label();
        Label LabelCombatSkillHead = new Label();
        Label LabelArtisanSkillHead = new Label();
        Label LabelScholarshipSkillHead = new Label();
        Label LabelSocialSkillHead = new Label();
        Label LabelSupernaturalSkillHead = new Label();
        Label LabelSurvivalSkillHead = new Label();

        public int padding = 5;
        public bool ReadOnly;
        public CharacterPanel(Character toDisplay) 
        {
            Init(toDisplay, true);
        }

        public CharacterPanel(Character toDisplay, bool readOnly)
        {
            Init(toDisplay, readOnly);
        }

        private void Init(Character toDisplay, bool readOnly)
        {
            ShownCharacter = toDisplay;
            this.Text = ShownCharacter.CharacterName;
            this.BackColor = Color.Black;
            this.ForeColor = Color.White;
            this.Width = 600;
            this.Height = 700;
            ReadOnly = readOnly;

            int currentY= 0;
            if (readOnly)
            {
                LabelName.Text = ShownCharacter.CharacterName;
                StyleLabel(LabelName);
                LabelName.Location = new Point(Math.Max(0, (this.Width - LabelName.Width) / 2), padding);
                Controls.Add(LabelName);
                currentY = LabelName.Location.Y + LabelName.Height;
            }
            else
            {
                TextBox ChangeName = new TextBox();
                ChangeName.Text = ShownCharacter.CharacterName;
                StyleLabel(ChangeName);
                ChangeName.BackColor = Color.DarkGray;
                ChangeName.Width = 500;
                ChangeName.Location = new Point(Math.Max(0, (this.Width - ChangeName.Width) / 2), padding);
                ChangeName.TextChanged += ChangeName_TextChanged;
                Controls.Add(ChangeName);
                currentY = ChangeName.Location.Y + ChangeName.Height;
            }

            if (readOnly)
            {
                LabelSexRace.Text = Race.GetRace(ShownCharacter.CharacterRace).Name;
                if (ShownCharacter.Sex == 1)
                {
                    LabelSexRace.Text += " Male ";
                }
                else
                {
                    LabelSexRace.Text += " Female ";
                }
                LabelSexRace.Text += Profession.GetProfession(ShownCharacter.CharacterProfession).Name;
                StyleLabel(LabelSexRace);
                LabelSexRace.Location = new Point(Math.Max(0, (this.Width - LabelSexRace.Width) / 2), currentY);
                currentY = LabelSexRace.Location.Y + LabelSexRace.Height;
            }
            else
            {
                ComboBox ComboRace = new ComboBox();
                ComboRace.ForeColor = Color.White;
                ComboRace.BackColor = Color.Black;
                ComboRace.Height = ComboRace.PreferredHeight;
                ComboRace.Width = 90;
                foreach(Race race in Race.GetRaces())
                {
                    ComboRace.Items.Add(race.Name);
                }
                ComboRace.AutoCompleteMode = AutoCompleteMode.Suggest;
                ComboRace.AutoCompleteSource = AutoCompleteSource.CustomSource;
                ComboRace.AutoCompleteCustomSource = new AutoCompleteStringCollection();
                ComboRace.Text = Race.GetRace(ShownCharacter.CharacterRace).Name;
                ComboRace.SelectedIndexChanged += ComboRace_SelectedIndexChanged;
                ComboRace.Location = new Point(100, currentY);
                this.Controls.Add(ComboRace);

                ComboBox ComboSex = new ComboBox();
                ComboSex.ForeColor = Color.White;
                ComboSex.BackColor = Color.Black;
                ComboSex.Height = ComboSex.PreferredHeight;
                ComboSex.Width = 90;
                ComboSex.Items.Add("Female");
                ComboSex.Items.Add("Male");
                ComboSex.AutoCompleteMode = AutoCompleteMode.Suggest;
                ComboSex.AutoCompleteSource = AutoCompleteSource.CustomSource;
                ComboSex.AutoCompleteCustomSource = new AutoCompleteStringCollection();
                if(ShownCharacter.Sex == 1)
                {
                    ComboSex.Text = "Male";
                }
                else
                {
                    ComboSex.Text = "Female";
                }
                ComboSex.SelectedIndexChanged += ComboSex_SelectedIndexChanged;
                ComboSex.Location = new Point(200, currentY);
                this.Controls.Add(ComboSex);

                ComboBox ComboProfession = new ComboBox();
                ComboProfession.ForeColor = Color.White;
                ComboProfession.BackColor = Color.Black;
                ComboProfession.Height = ComboProfession.PreferredHeight;
                ComboProfession.Width = 200;
                foreach(Profession prof in Profession.GetProfessions())
                {
                    ComboProfession.Items.Add(prof.Name);
                }
                ComboProfession.AutoCompleteMode = AutoCompleteMode.Suggest;
                ComboProfession.AutoCompleteSource = AutoCompleteSource.CustomSource;
                ComboProfession.AutoCompleteCustomSource = new AutoCompleteStringCollection();
                ComboProfession.Text = Profession.GetProfession(ShownCharacter.CharacterProfession).Name;
                ComboProfession.SelectedIndexChanged += ComboProfession_SelectedIndexChanged;
                ComboProfession.Location = new Point(300, currentY);
                this.Controls.Add(ComboProfession);
                currentY = ComboProfession.Location.Y + ComboProfession.Height;
            }

            LabelAbilityHead.Text = "Abilities";
            StyleLabel(LabelAbilityHead);
            LabelAbilityHead.Location = new Point(Math.Max(0, (this.Width - LabelAbilityHead.Width) / 2), currentY + (padding * 2));

            int colWidth = (this.ClientRectangle.Width - (padding * 5)) / 4;

            LabelPhysicalHead.Text = "Physical";
            StyleLabel(LabelPhysicalHead);
            LabelPhysicalHead.Location = new Point(padding, LabelAbilityHead.Location.Y + LabelAbilityHead.Height);

            LabelMentalHead.Text = "Mental";
            StyleLabel(LabelMentalHead);
            LabelMentalHead.Location = new Point((padding * 2) + colWidth, LabelAbilityHead.Location.Y + LabelAbilityHead.Height);

            LabelSocialHead.Text = "Social";
            StyleLabel(LabelSocialHead);
            LabelSocialHead.Location = new Point((padding * 3) + (colWidth * 2), LabelAbilityHead.Location.Y + LabelAbilityHead.Height);

            LabelSupernaturalHead.Text = "Supernatural";
            StyleLabel(LabelSupernaturalHead);
            LabelSupernaturalHead.Location = new Point((padding * 4) + (colWidth * 3), LabelAbilityHead.Location.Y + LabelAbilityHead.Height);

            Control lastAb = null;
            int lastCol = -1;
            foreach(Ability ab in Ability.GetAbilities())
            {
                Label abilityLabel = new Label();
                abilityLabel.Text = ab.Name;
                StyleLabelLesser(abilityLabel);

                if (readOnly)
                {
                    Label scoreLabel = new Label();
                    scoreLabel.Text = ShownCharacter.Abilities[ab.Id].ToString();
                    StyleLabelLesser(scoreLabel);

                    int col = (int)ab.Id / 3;
                    if (col > 3) col = 3;

                    if (lastCol != col || lastAb == null)
                    {
                        abilityLabel.Location = new Point((padding * (col + 1)) + (colWidth * col), LabelSupernaturalHead.Location.Y + LabelAbilityHead.Height);
                        scoreLabel.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - scoreLabel.Width, LabelSupernaturalHead.Location.Y + LabelAbilityHead.Height);
                        lastAb = abilityLabel;
                    }
                    else
                    {
                        abilityLabel.Location = new Point((padding * (col + 1)) + (colWidth * col), lastAb.Location.Y + scoreLabel.Height);
                        scoreLabel.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - scoreLabel.Width, lastAb.Location.Y + scoreLabel.Height);
                        lastAb = abilityLabel;
                    }
                    lastCol = col;
                    Controls.Add(abilityLabel);
                    Controls.Add(scoreLabel);
                }
                else
                {
                    StatBox scoreLabel = new StatBox(ab.Id);
                    scoreLabel.Value = ShownCharacter.Abilities[ab.Id];
                    scoreLabel.ValueChanged += scoreLabel_ValueChanged;

                    int col = (int)ab.Id / 3;
                    if (col > 3) col = 3;

                    if (lastCol != col || lastAb == null)
                    {
                        abilityLabel.Location = new Point((padding * (col + 1)) + (colWidth * col), LabelSupernaturalHead.Location.Y + LabelAbilityHead.Height);
                        scoreLabel.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - scoreLabel.Width, LabelSupernaturalHead.Location.Y + LabelAbilityHead.Height);
                        lastAb = abilityLabel;
                    }
                    else
                    {
                        abilityLabel.Location = new Point((padding * (col + 1)) + (colWidth * col), lastAb.Location.Y + scoreLabel.Height);
                        scoreLabel.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - scoreLabel.Width, lastAb.Location.Y + scoreLabel.Height);
                        lastAb = abilityLabel;
                    }
                    lastCol = col;
                    Controls.Add(abilityLabel);
                    Controls.Add(scoreLabel);
                }
            }

            Dictionary<uint, int> combatSkills = new Dictionary<uint, int>();
            Dictionary<uint, int> artisanSkills = new Dictionary<uint, int>();
            Dictionary<uint, int> scholarshipSkills = new Dictionary<uint, int>();
            Dictionary<uint, int> socialSkills = new Dictionary<uint, int>();
            Dictionary<uint, int> survivalSkills = new Dictionary<uint, int>();
            Dictionary<uint, int> supernaturalSkills = new Dictionary<uint, int>();

            foreach(KeyValuePair<uint, int> skillPair in ShownCharacter.Skills)
            {
                switch(Skill.GetSkill(skillPair.Key).Category)
                {
                    case SkillCategory.Artisan:
                        artisanSkills.Add(skillPair.Key, skillPair.Value);
                        break;
                    case SkillCategory.Combat:
                        combatSkills.Add(skillPair.Key, skillPair.Value);
                        break;
                    case SkillCategory.Scholarship:
                        scholarshipSkills.Add(skillPair.Key, skillPair.Value);
                        break;
                    case SkillCategory.Social:
                        socialSkills.Add(skillPair.Key, skillPair.Value);
                        break;
                    case SkillCategory.Supernatural:
                        supernaturalSkills.Add(skillPair.Key, skillPair.Value);
                        break;
                    case SkillCategory.Survival:
                        survivalSkills.Add(skillPair.Key, skillPair.Value);
                        break;
                }
            }

            int currentPosY = lastAb.Location.Y + lastAb.Height;

            if(artisanSkills.Count > 0)
            {
                int nCount = 0;
                LabelArtisanSkillHead.Text = "Artisan Skills";
                StyleLabel(LabelArtisanSkillHead);
                LabelArtisanSkillHead.Location = new Point(Math.Max(((this.ClientRectangle.Width - LabelArtisanSkillHead.Width) / 2), 0), currentPosY + (padding * 3));
                this.Controls.Add(LabelArtisanSkillHead);

                foreach(KeyValuePair<uint, int> skillPair in artisanSkills)
                {
                    Skill nSk = Skill.GetSkill(skillPair.Key);
                    Label addingSkill = new Label();
                    addingSkill.Text = nSk.Name;
                    StyleLabelLesser(addingSkill);

                    int col = nCount % 4;
                    int row = nCount / 4;

                    addingSkill.Location = new Point((padding * (col + 1)) + (colWidth * col), LabelArtisanSkillHead.Location.Y + LabelArtisanSkillHead.Height + (lastAb.Height * row));
                    if(readOnly)
                    {
                        Label addingSkillScore = new Label();
                        addingSkillScore.Text = skillPair.Value.ToString();
                        StyleLabelLesser(addingSkillScore);
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelArtisanSkillHead.Location.Y + LabelArtisanSkillHead.Height + (lastAb.Height * row));
                        currentPosY = addingSkill.Location.Y + addingSkill.Height;
                        this.Controls.Add(addingSkillScore);
                    }
                    else
                    {
                        SkillBox addingSkillScore = new SkillBox(nSk.Id);
                        addingSkillScore.Value = skillPair.Value;
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelArtisanSkillHead.Location.Y + LabelArtisanSkillHead.Height + (lastAb.Height * row));
                        currentPosY = addingSkillScore.Location.Y + addingSkillScore.Height;
                        addingSkillScore.ValueChanged += addingSkillScore_ValueChanged;
                        lastAb = addingSkillScore;
                        this.Controls.Add(addingSkillScore);
                    }
                    currentPosY = addingSkill.Location.Y + addingSkill.Height;
                    this.Controls.Add(addingSkill);
                    nCount++;
                }
            }

            if(combatSkills.Count > 0)
            {
                int nCount = 0;
                LabelCombatSkillHead.Text = "Combat Skills";
                StyleLabel(LabelCombatSkillHead);
                LabelCombatSkillHead.Location = new Point(Math.Max(((this.ClientRectangle.Width - LabelCombatSkillHead.Width) / 2), 0), currentPosY + (padding * 3));
                this.Controls.Add(LabelCombatSkillHead);

                foreach (KeyValuePair<uint, int> skillPair in combatSkills)
                {
                    Skill nSk = Skill.GetSkill(skillPair.Key);
                    Label addingSkill = new Label();
                    addingSkill.Text = nSk.Name;
                    StyleLabelLesser(addingSkill);

                    int col = nCount % 4;
                    int row = nCount / 4;

                    addingSkill.Location = new Point((padding * (col + 1)) + (colWidth * col), LabelCombatSkillHead.Location.Y + LabelCombatSkillHead.Height + (lastAb.Height * row));
                    if (readOnly)
                    {
                        Label addingSkillScore = new Label();
                        addingSkillScore.Text = skillPair.Value.ToString();
                        StyleLabelLesser(addingSkillScore);
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelCombatSkillHead.Location.Y + LabelCombatSkillHead.Height + (lastAb.Height * row));
                        currentPosY = addingSkill.Location.Y + addingSkill.Height;
                        this.Controls.Add(addingSkillScore);
                    }
                    else
                    {
                        SkillBox addingSkillScore = new SkillBox(nSk.Id);
                        addingSkillScore.Value = skillPair.Value;
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelCombatSkillHead.Location.Y + LabelCombatSkillHead.Height + (lastAb.Height * row));
                        currentPosY = addingSkillScore.Location.Y + addingSkillScore.Height;
                        addingSkillScore.ValueChanged += addingSkillScore_ValueChanged;
                        lastAb = addingSkillScore;
                        this.Controls.Add(addingSkillScore);
                    }
                    currentPosY = addingSkill.Location.Y + addingSkill.Height;
                    this.Controls.Add(addingSkill);
                    nCount++;
                }
            }

            if (scholarshipSkills.Count > 0)
            {
                int nCount = 0;
                LabelScholarshipSkillHead.Text = "Scholarship Skills";
                StyleLabel(LabelScholarshipSkillHead);
                LabelScholarshipSkillHead.Location = new Point(Math.Max(((this.ClientRectangle.Width - LabelScholarshipSkillHead.Width) / 2), 0), currentPosY + (padding * 3));
                this.Controls.Add(LabelScholarshipSkillHead);

                foreach (KeyValuePair<uint, int> skillPair in scholarshipSkills)
                {
                    Skill nSk = Skill.GetSkill(skillPair.Key);
                    Label addingSkill = new Label();
                    addingSkill.Text = nSk.Name;
                    StyleLabelLesser(addingSkill);

                    int col = nCount % 4;
                    int row = nCount / 4;

                    addingSkill.Location = new Point((padding * (col + 1)) + (colWidth * col), LabelScholarshipSkillHead.Location.Y + LabelScholarshipSkillHead.Height + (lastAb.Height * row));
                    if (readOnly)
                    {
                        Label addingSkillScore = new Label();
                        addingSkillScore.Text = skillPair.Value.ToString();
                        StyleLabelLesser(addingSkillScore);
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelScholarshipSkillHead.Location.Y + LabelScholarshipSkillHead.Height + (lastAb.Height * row));
                        currentPosY = addingSkill.Location.Y + addingSkill.Height;
                        this.Controls.Add(addingSkillScore);
                    }
                    else
                    {
                        SkillBox addingSkillScore = new SkillBox(nSk.Id);
                        addingSkillScore.Value = skillPair.Value;
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelScholarshipSkillHead.Location.Y + LabelScholarshipSkillHead.Height + (lastAb.Height * row));
                        currentPosY = addingSkillScore.Location.Y + addingSkillScore.Height;
                        addingSkillScore.ValueChanged += addingSkillScore_ValueChanged;
                        lastAb = addingSkillScore;
                        this.Controls.Add(addingSkillScore);
                    }
                    currentPosY = addingSkill.Location.Y + addingSkill.Height;
                    this.Controls.Add(addingSkill);
                    nCount++;
                }
            }

            if (socialSkills.Count > 0)
            {
                int nCount = 0;
                LabelSocialSkillHead.Text = "Social Skills";
                StyleLabel(LabelSocialSkillHead);
                LabelSocialSkillHead.Location = new Point(Math.Max(((this.ClientRectangle.Width - LabelSocialSkillHead.Width) / 2), 0), currentPosY + (padding * 3));
                this.Controls.Add(LabelSocialSkillHead);

                foreach (KeyValuePair<uint, int> skillPair in socialSkills)
                {
                    Skill nSk = Skill.GetSkill(skillPair.Key);
                    Label addingSkill = new Label();
                    addingSkill.Text = nSk.Name;
                    StyleLabelLesser(addingSkill);

                    int col = nCount % 4;
                    int row = nCount / 4;

                    addingSkill.Location = new Point((padding * (col + 1)) + (colWidth * col), LabelSocialSkillHead.Location.Y + LabelSocialSkillHead.Height + (lastAb.Height * row));
                    if (readOnly)
                    {
                        Label addingSkillScore = new Label();
                        addingSkillScore.Text = skillPair.Value.ToString();
                        StyleLabelLesser(addingSkillScore);
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelSocialSkillHead.Location.Y + LabelSocialSkillHead.Height + (lastAb.Height * row));
                        currentPosY = addingSkill.Location.Y + addingSkill.Height;
                        this.Controls.Add(addingSkillScore);
                    }
                    else
                    {
                        SkillBox addingSkillScore = new SkillBox(nSk.Id);
                        addingSkillScore.Value = skillPair.Value;
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelSocialSkillHead.Location.Y + LabelSocialSkillHead.Height + (lastAb.Height * row));
                        currentPosY = addingSkillScore.Location.Y + addingSkillScore.Height;
                        addingSkillScore.ValueChanged += addingSkillScore_ValueChanged;
                        lastAb = addingSkillScore;
                        this.Controls.Add(addingSkillScore);
                    }
                    currentPosY = addingSkill.Location.Y + addingSkill.Height;
                    this.Controls.Add(addingSkill);
                    nCount++;
                }
            }

            if (supernaturalSkills.Count > 0)
            {
                int nCount = 0;
                LabelSupernaturalSkillHead.Text = "Supernatural Skills";
                StyleLabel(LabelSupernaturalSkillHead);
                LabelSupernaturalSkillHead.Location = new Point(Math.Max(((this.ClientRectangle.Width - LabelSupernaturalSkillHead.Width) / 2), 0), currentPosY + (padding * 3));
                this.Controls.Add(LabelSupernaturalSkillHead);

                foreach (KeyValuePair<uint, int> skillPair in supernaturalSkills)
                {
                    Skill nSk = Skill.GetSkill(skillPair.Key);
                    Label addingSkill = new Label();
                    addingSkill.Text = nSk.Name;
                    StyleLabelLesser(addingSkill);

                    int col = nCount % 4;
                    int row = nCount / 4;

                    addingSkill.Location = new Point((padding * (col + 1)) + (colWidth * col), LabelSupernaturalSkillHead.Location.Y + LabelSupernaturalSkillHead.Height + (lastAb.Height * row));
                    if (readOnly)
                    {
                        Label addingSkillScore = new Label();
                        addingSkillScore.Text = skillPair.Value.ToString();
                        StyleLabelLesser(addingSkillScore);
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelSupernaturalSkillHead.Location.Y + LabelSupernaturalSkillHead.Height + (lastAb.Height * row));
                        currentPosY = addingSkill.Location.Y + addingSkill.Height;
                        this.Controls.Add(addingSkillScore);
                    }
                    else
                    {
                        SkillBox addingSkillScore = new SkillBox(nSk.Id);
                        addingSkillScore.Value = skillPair.Value;
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelSupernaturalSkillHead.Location.Y + LabelSupernaturalSkillHead.Height + (lastAb.Height * row));
                        currentPosY = addingSkillScore.Location.Y + addingSkillScore.Height;
                        addingSkillScore.ValueChanged += addingSkillScore_ValueChanged;
                        lastAb = addingSkillScore;
                        this.Controls.Add(addingSkillScore);
                    }
                    currentPosY = addingSkill.Location.Y + addingSkill.Height;
                    this.Controls.Add(addingSkill);
                    nCount++;
                }
            }

            if (survivalSkills.Count > 0)
            {
                int nCount = 0;
                LabelSurvivalSkillHead.Text = "Survival Skills";
                StyleLabel(LabelSurvivalSkillHead);
                LabelSurvivalSkillHead.Location = new Point(Math.Max(((this.ClientRectangle.Width - LabelSurvivalSkillHead.Width) / 2), 0), currentPosY + (padding * 3));
                this.Controls.Add(LabelSurvivalSkillHead);

                foreach (KeyValuePair<uint, int> skillPair in survivalSkills)
                {
                    Skill nSk = Skill.GetSkill(skillPair.Key);
                    Label addingSkill = new Label();
                    addingSkill.Text = nSk.Name;
                    StyleLabelLesser(addingSkill);

                    int col = nCount % 4;
                    int row = nCount / 4;

                    addingSkill.Location = new Point((padding * (col + 1)) + (colWidth * col), LabelSurvivalSkillHead.Location.Y + LabelSurvivalSkillHead.Height + (lastAb.Height * row));
                    if (readOnly)
                    {
                        Label addingSkillScore = new Label();
                        addingSkillScore.Text = skillPair.Value.ToString();
                        StyleLabelLesser(addingSkillScore);
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelSurvivalSkillHead.Location.Y + LabelSurvivalSkillHead.Height + (lastAb.Height * row));
                        currentPosY = addingSkill.Location.Y + addingSkill.Height;
                        this.Controls.Add(addingSkillScore);
                    }
                    else
                    {
                        SkillBox addingSkillScore = new SkillBox(nSk.Id);
                        addingSkillScore.Value = skillPair.Value;
                        addingSkillScore.Location = new Point((padding * (col + 1)) + (colWidth * (col + 1)) - addingSkillScore.Width, LabelSurvivalSkillHead.Location.Y + LabelSurvivalSkillHead.Height + (addingSkillScore.Height * row));
                        currentPosY = addingSkillScore.Location.Y + addingSkillScore.Height;
                        addingSkillScore.ValueChanged += addingSkillScore_ValueChanged;
                        lastAb = addingSkillScore;
                        this.Controls.Add(addingSkillScore);
                    }
                    currentPosY = addingSkill.Location.Y + addingSkill.Height;
                    this.Controls.Add(addingSkill);
                    nCount++;
                }
            }

            if(!readOnly)
            {
                ComboBox addSkill = new ComboBox();
                addSkill.ForeColor = Color.White;
                addSkill.BackColor = Color.Black;
                addSkill.Height = addSkill.PreferredHeight;
                addSkill.Width = 300;
                foreach(Skill sk in Skill.GetSkills())
                {
                    if(!ShownCharacter.Skills.Keys.Contains(sk.Id))
                    {
                        addSkill.Items.Add(sk.Name);
                    }
                }
                addSkill.AutoCompleteMode = AutoCompleteMode.Suggest;
                addSkill.AutoCompleteSource = AutoCompleteSource.CustomSource;
                addSkill.AutoCompleteCustomSource = new AutoCompleteStringCollection();
                addSkill.Text = "<Add a Skill>";
                addSkill.SelectedIndexChanged += addSkill_SelectedIndexChanged;
                addSkill.Location = new Point(padding, currentPosY + (padding * 2));
                currentPosY = addSkill.Location.Y + addSkill.Height;
                this.Controls.Add(addSkill);
            }

            Label Virt = new Label();
            Virt.Text = "Virtue: " + Virtue.GetVirtue(ShownCharacter.CharacterVirtue).Name;
            StyleLabel(Virt);
            Virt.Location = new Point(padding, currentPosY + padding);
            this.Controls.Add(Virt);

            Label Vic = new Label();
            Vic.Text = "Vice: " + Vice.GetVice(ShownCharacter.CharacterVice).Name;
            StyleLabel(Vic);
            Vic.Location = new Point(this.ClientRectangle.Width / 2 + padding, currentPosY + padding);
            this.Controls.Add(Vic);

            currentPosY = Vic.Location.Y + Vic.Height;

            Label auth = new Label();
            auth.Text = "Authority: " + Authority.GetAuthority(ShownCharacter.MoralsAuthority).Name;
            StyleLabel(auth);
            auth.Location = new Point(padding, currentPosY + padding);
            this.Controls.Add(auth);

            Label care = new Label();
            care.Text = "Care: " + Care.GetCare(ShownCharacter.MoralsCare).Name;
            StyleLabel(care);
            care.Location = new Point(padding, auth.Location.Y + auth.Height + padding);
            this.Controls.Add(care);

            Label fairness = new Label();
            fairness.Text = "Fairness: " + Fairness.GetFairness(ShownCharacter.MoralsFairness).Name;
            StyleLabel(fairness);
            fairness.Location = new Point(padding, care.Location.Y + care.Height + padding);
            this.Controls.Add(fairness);

            Label loyalty = new Label();
            loyalty.Text = "Loyalty: " + Loyalty.GetLoyalty(ShownCharacter.MoralsLoyalty).Name;
            StyleLabel(loyalty);
            loyalty.Location = new Point(padding, fairness.Location.Y + fairness.Height + padding);
            this.Controls.Add(loyalty);

            Label trad = new Label();
            trad.Text = "Tradition: " + Tradition.GetTradition(ShownCharacter.MoralsTradition).Name;
            StyleLabel(trad);
            trad.Location = new Point(padding, loyalty.Location.Y + loyalty.Height + padding);
            this.Controls.Add(trad);            

            Controls.Add(LabelSexRace);
            Controls.Add(LabelAbilityHead);
            Controls.Add(LabelPhysicalHead);
            Controls.Add(LabelMentalHead);
            Controls.Add(LabelSocialHead);
            Controls.Add(LabelSupernaturalHead);
        }

        void addSkill_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboProfession = (ComboBox)sender;
            string index = ComboProfession.SelectedItem.ToString();
            foreach(Skill sk in Skill.GetSkills())
            {
                if(index == sk.Name)
                {
                    ShownCharacter.Skills.Add(sk.Id, 1);
                    Redraw();
                    return;
                }
            }
        }

        void addingSkillScore_ValueChanged(object sender, EventArgs e)
        {
            SkillBox box = sender as SkillBox;
            int nVal = (int)box.Value;
            if(nVal < 1)
            {
                ShownCharacter.Skills.Remove(box.SkillId);
                Redraw();
                return;
            }
            if(nVal > 10)
            {
                nVal = 10;
                box.Value = 10;
            }
            ShownCharacter.Skills[box.SkillId] = (int)box.Value;

        }

        void scoreLabel_ValueChanged(object sender, EventArgs e)
        {
            StatBox box = sender as StatBox;
            int nVal = (int)box.Value;
            if (nVal > 10)
            {
                nVal = 10;
                box.Value = 10;
            }
            if(nVal < 1)
            {
                nVal = 1;
                box.Value = 1;
            }
            ShownCharacter.Abilities[box.StatId] = (int)box.Value;
        }

        private void Redraw()
        {
            this.Controls.Clear();
            Init(ShownCharacter, ReadOnly);
        }

        void ComboProfession_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboProfession = (ComboBox)sender;
            string index = ComboProfession.SelectedItem.ToString();
            foreach(Profession prof in Profession.GetProfessions())
            {
                if(index == prof.Name)
                {
                    ShownCharacter.CharacterProfession = prof.Id;
                }
            }
        }

        void ComboSex_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboSex = (ComboBox)sender;
            string index = ComboSex.SelectedItem.ToString();
            if(index == "Male")
            {
                ShownCharacter.Sex = 1;
            }
            else
            {
                ShownCharacter.Sex = 0;
            }
        }

        void ComboRace_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox ComboRace = (ComboBox)sender;
            string index = ComboRace.SelectedItem.ToString();
            foreach(Race race in Race.GetRaces())
            {
                if(index == race.Name)
                {
                    ShownCharacter.CharacterRace = race.Id;
                }
            }
        }

        void ChangeName_TextChanged(object sender, EventArgs e)
        {
            TextBox text = sender as TextBox;
            if(!String.IsNullOrWhiteSpace(text.Text))
            {
                ShownCharacter.CharacterName = text.Text;
            }
        }

        /// <summary>
        /// Font used by the controls on this form.
        /// </summary>
        private static Font labelFont = new Font(FontFamily.GenericSansSerif, 12.0f, FontStyle.Bold);

        private static Font labelFontLesser = new Font(FontFamily.GenericSansSerif, 10.0f);

        /// <summary>
        /// Used to provide the style information for all of the labels and
        /// buttons on the form.
        /// </summary>
        /// <param name="toStyle">the control to style</param>
        private static void StyleLabel(Control toStyle)
        {
            toStyle.Font = labelFont;
            toStyle.ForeColor = Color.White;
            toStyle.BackColor = Color.Black;
            toStyle.Size = toStyle.PreferredSize;
        }

        private static void StyleLabelLesser(Control toStyle)
        {
            toStyle.Font = labelFontLesser;
            toStyle.ForeColor = Color.White;
            toStyle.BackColor = Color.Black;
            toStyle.Size = toStyle.PreferredSize;
        }
    }

    public class StatBox: NumericUpDown
    {
        public uint StatId;
        public StatBox(uint statId)
        {
            StatId = statId;
            this.Font = new Font(FontFamily.GenericSansSerif, 10.0f);
            ForeColor = Color.White;
            BackColor = Color.Black;
            Size = PreferredSize;
        }
    }

    public class SkillBox: NumericUpDown
    {
        public uint  SkillId;
        public SkillBox(uint skillId)
        {
            SkillId = skillId;
            this.Font = new Font(FontFamily.GenericSansSerif, 10.0f);
            ForeColor = Color.White;
            BackColor = Color.Black;
            Size = PreferredSize;
        }
    }
}
