using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.Speedrun
{
    public partial class SpeedrunPanel : APhasmophobiaCompanionPanel
    {
        private static readonly int EVIDENCE_LIMIT = 3;
        private List<KeyValuePair<EString, Control>> m_RegisteredControls = new List<KeyValuePair<EString, Control>>();
        private readonly List<Ghost> m_Ghosts = new List<Ghost>();
        private readonly HashSet<Evidence> m_SelectedEvidence = new HashSet<Evidence>();
        private readonly Dictionary<CheckBox, Evidence> m_CheckBoxToEvidence = new Dictionary<CheckBox, Evidence>();
        private readonly Dictionary<Evidence, CheckBox> m_EvidenceToCheckBox = new Dictionary<Evidence, CheckBox>();

        public SpeedrunPanel(MainWindow mainWindow): base(mainWindow, EPanelType.Speedrun, EString.Speedrun)
        {
            InitializeComponent();
            RegisterEvidenceCheckBox(Evidence.EMF5, EString.EMF5);
            RegisterEvidenceCheckBox(Evidence.SpiritBox, EString.SpiritBox);
            RegisterEvidenceCheckBox(Evidence.Fingerprints, EString.Fingerprints);
            RegisterEvidenceCheckBox(Evidence.GhostOrb, EString.GhostOrb);
            RegisterEvidenceCheckBox(Evidence.GhostWriting, EString.GhostWriting);
            RegisterEvidenceCheckBox(Evidence.Freezing, EString.Freezing);
            RegisterGhost(EString.Spirit, Evidence.SpiritBox, Evidence.Fingerprints, Evidence.GhostWriting);
            RegisterGhost(EString.Wraith, Evidence.Fingerprints, Evidence.Freezing, Evidence.SpiritBox);
            RegisterGhost(EString.Phantom, Evidence.EMF5, Evidence.GhostOrb, Evidence.Freezing);
            RegisterGhost(EString.Poltergeist, Evidence.SpiritBox, Evidence.Fingerprints, Evidence.GhostOrb);
            RegisterGhost(EString.Banshee, Evidence.EMF5, Evidence.Fingerprints, Evidence.Freezing);
            RegisterGhost(EString.Jinn, Evidence.SpiritBox, Evidence.EMF5, Evidence.GhostOrb);
            RegisterGhost(EString.Mare, Evidence.SpiritBox, Evidence.GhostOrb, Evidence.Freezing);
            RegisterGhost(EString.Revenant, Evidence.EMF5, Evidence.Fingerprints, Evidence.GhostWriting);
            RegisterGhost(EString.Shade, Evidence.EMF5, Evidence.GhostOrb, Evidence.GhostWriting);
            RegisterGhost(EString.Demon, Evidence.SpiritBox, Evidence.Freezing, Evidence.GhostWriting);
            RegisterGhost(EString.Yurei, Evidence.GhostOrb, Evidence.Freezing, Evidence.GhostWriting);
            RegisterGhost(EString.Oni, Evidence.SpiritBox, Evidence.EMF5, Evidence.GhostWriting);
            RegisterGhost(EString.Yokai, Evidence.SpiritBox, Evidence.GhostWriting, Evidence.GhostOrb);
            RegisterGhost(EString.Hantu, Evidence.Fingerprints, Evidence.GhostWriting, Evidence.GhostOrb);
            MoveGhostLabels();
            UpdateEvidences();
        }

        private void MoveGhostLabels()
        {
            Translator translator = GetTranslator();
            int labelListHeight = 490;
            int labelWidth = 200;
            float fontSize = 20;
            int labelHeight = labelListHeight / (m_Ghosts.Count - 1); // - 1 as we don't count the last one which will be placed at Y = 500
            SizeF stringSize = TextRenderer.MeasureText("A", new Font(GetFontFamily(), fontSize));
            float newFontSize = fontSize * Math.Min(labelHeight / stringSize.Height, (labelWidth - 10) / stringSize.Width);
            if (newFontSize < fontSize)
                fontSize = newFontSize;
            int labelYPosition = 10;
            foreach (Ghost ghost in m_Ghosts)
            {
                Label ghostLabel = new Label()
                {
                    Location = new Point(25, labelYPosition),
                    Font = new Font(GetFontFamily(), fontSize),
                    AutoSize = false,
                    Size = new Size(labelWidth, labelHeight),
                    BackColor = Color.Transparent,
                    ForeColor = Color.White,
                };
                labelYPosition += labelHeight;
                translator.RegisterControl(ghost.GetName(), ghostLabel);
                Controls.Add(ghostLabel);
                ghost.SetLabel(ghostLabel);
            }
        }

        private void RegisterEvidenceCheckBox(Evidence evidence, EString name)
        {
            CheckBox checkBox = new CheckBox
            {
                Appearance = Appearance.Button,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(GetFontFamily(), 24),
                ForeColor = Color.White,
                UseVisualStyleBackColor = false,
                AutoSize = true
            };
            m_RegisteredControls.Add(new KeyValuePair<EString, Control>(name, checkBox));
            checkBox.FlatAppearance.BorderSize = 0;
            checkBox.FlatAppearance.CheckedBackColor = Color.Transparent;
            checkBox.FlatAppearance.MouseDownBackColor = Color.Gray;
            checkBox.FlatAppearance.MouseOverBackColor = Color.Gray;
            m_CheckBoxToEvidence[checkBox] = evidence;
            m_EvidenceToCheckBox[evidence] = checkBox;
            checkBox.Click += new EventHandler(CheckBoxClicked);
            Controls.Add(checkBox);
        }

        private void RegisterGhost(EString name, params Evidence[] evidences)
        {
            Ghost ghost = new Ghost(name);
            foreach (Evidence evidence in evidences)
                ghost.AddEvidence(evidence);
            m_Ghosts.Add(ghost);
        }

        public override void OnOpen()
        {
            Translator translator = GetTranslator();
            foreach (KeyValuePair<EString, Control> pair in m_RegisteredControls)
                translator.RegisterControl(pair.Key, pair.Value);
            PanelUIManager.RegisterImageButton(BackButton, Properties.Resources.red_arrow, Properties.Resources.red_arrow_over, Properties.Resources.red_arrow_clicked);
            Reset();
        }

        private void Reset()
        {
            m_SelectedEvidence.Clear();
            foreach (CheckBox checkBox in m_EvidenceToCheckBox.Values)
            {
                checkBox.Checked = false;
                checkBox.ForeColor = Color.White;
            }
            UpdateEvidences();
        }

        private void UpdateEvidences()
        {
            HashSet<Evidence> possibleEvidence = new HashSet<Evidence>();
            List<Ghost> possibleGhosts = new List<Ghost>();
            foreach (Ghost ghost in m_Ghosts)
            {
                Label ghostLabel = ghost.GetLabel();
                if (ghost.Match(m_SelectedEvidence))
                {
                    possibleGhosts.Add(ghost);
                    foreach (Evidence ghostEvidence in ghost.GetEvidences())
                        possibleEvidence.Add(ghostEvidence);
                    ghostLabel.Enabled = true;
                    ghostLabel.Visible = true;
                }
                else
                {
                    ghostLabel.Enabled = false;
                    ghostLabel.Visible = false;
                }
            }
            int checkBoxY = 450;
            foreach (Evidence evidence in Enum.GetValues(typeof(Evidence)))
            {
                CheckBox checkBox = m_EvidenceToCheckBox[evidence];
                if (!possibleEvidence.Contains(evidence))
                {
                    checkBox.Enabled = false;
                    checkBox.Visible = false;
                }
                else
                {
                    checkBox.Enabled = true;
                    checkBox.Visible = true;
                    checkBox.Location = new Point(505, checkBoxY);
                    checkBoxY -= 55;
                }
            }
        }

        private void CheckBoxClicked(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox.Checked)
            {
                if (m_SelectedEvidence.Count < EVIDENCE_LIMIT)
                {
                    m_SelectedEvidence.Add(m_CheckBoxToEvidence[checkBox]);
                    checkBox.ForeColor = Color.Red;
                    UpdateEvidences();
                }
                else
                {
                    checkBox.Checked = false;
                    checkBox.ForeColor = Color.White;
                }
            }
            else
            {
                m_SelectedEvidence.Remove(m_CheckBoxToEvidence[checkBox]);
                checkBox.ForeColor = Color.White;
                UpdateEvidences();
            }
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            GetMainWindow().SetPanel(EPanelType.TitleScreen);
        }
    }
}
