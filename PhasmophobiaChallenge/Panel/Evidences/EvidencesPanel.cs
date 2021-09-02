using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.Evidences
{
    public partial class EvidencesPanel : APhasmophobiaCompanionPanel
    {
        private static readonly Color SELECTED_EVIDENCE = Color.DarkOliveGreen;
        private static readonly Color UNSELECTED_EVIDENCE = Color.White;
        private static readonly Color REMOVED_EVIDENCE = Color.LightSalmon;
        private static readonly Color IMPOSSIBLE_EVIDENCE = Color.DarkGray;

        private List<KeyValuePair<string, Control>> m_RegisteredControls = new List<KeyValuePair<string, Control>>();
        private readonly List<Ghost> m_Ghosts = new List<Ghost>();
        private readonly HashSet<Ghost> m_RemovedGhost = new HashSet<Ghost>();
        private readonly Dictionary<CheckBox, Ghost> m_CheckBoxToGhost = new Dictionary<CheckBox, Ghost>();
        private Ghost m_SelectedGhost = null;
        private readonly EvidenceOverlay m_Overlay = new EvidenceOverlay();
        private CheckBox m_CheckBoxDownOn = null;
        private readonly HashSet<uint> m_SelectedEvidences = new HashSet<uint>();
        private readonly HashSet<uint> m_RemovedEvidences = new HashSet<uint>();
        private readonly Dictionary<CheckBox, uint> m_CheckBoxToEvidence = new Dictionary<CheckBox, uint>();
        private readonly Dictionary<uint, CheckBox> m_EvidenceToCheckBox = new Dictionary<uint, CheckBox>();
        private uint m_NbEvidence = 0;

        public EvidencesPanel(MainWindow mainWindow): base(mainWindow, EPanelType.Speedrun, "panel.evidences")
        {
            InitializeComponent();

            Json datas = new Json(Properties.Resources.evidences);

            List<Json> evidences = datas.GetArray<Json>("evidences");
            foreach (Json evidenceJson in evidences)
            {
                uint? id = evidenceJson.Get<uint>("id");
                string name = evidenceJson.Get<string>("name");
                string image = evidenceJson.Get<string>("image");
                if (id != null && name != null && image != null)
                    RegisterEvidenceCheckBox((uint)id, name, image);
            }

            List<Json> ghosts = datas.GetArray<Json>("ghosts");
            foreach (Json ghostJson in ghosts)
            {
                string name = ghostJson.Get<string>("name");
                List<uint> ghostEvidences = ghostJson.GetArray<uint>("evidences");
                if (name != null && ghostEvidences.Count != 0)
                    RegisterGhost(name, ghostEvidences);
            }

            MoveGhostLabels();
            UpdateEvidences();
            UpdateCheckBoxPosition();
        }

        private void MoveGhostLabels()
        {
            Translator translator = GetTranslator();
            int labelListHeight = 490;
            int labelWidth = 200;
            float fontSize = 20;
            int labelHeight = labelListHeight / (m_Ghosts.Count - 1); // - 1 as we don't count the last one which will be placed at Y = 500
            SizeF stringSize = TextRenderer.MeasureText("A", new Font(GetDefaultFontFamily(), fontSize));
            float newFontSize = fontSize * Math.Min(labelHeight / stringSize.Height, (labelWidth - 10) / stringSize.Width);
            if (newFontSize < fontSize)
                fontSize = newFontSize;
            int labelYPosition = 10;
            foreach (Ghost ghost in m_Ghosts)
            {
                CheckBox ghostCheckBox = new CheckBox
                {
                    Appearance = Appearance.Button,
                    BackColor = Color.Transparent,
                    FlatStyle = FlatStyle.Flat,
                    Font = new Font(GetDefaultFontFamily(), fontSize - 3),
                    ForeColor = UNSELECTED_EVIDENCE,
                    UseVisualStyleBackColor = false,
                    AutoSize = false,
                    Location = new Point(25, labelYPosition),
                    Size = new Size(labelWidth, labelHeight)
                };
                ghostCheckBox.FlatAppearance.BorderSize = 0;
                ghostCheckBox.FlatAppearance.CheckedBackColor = Color.Transparent;
                ghostCheckBox.FlatAppearance.MouseDownBackColor = Color.Gray;
                ghostCheckBox.FlatAppearance.MouseOverBackColor = Color.Gray;
                m_CheckBoxToGhost[ghostCheckBox] = ghost;
                ghostCheckBox.MouseDown += new MouseEventHandler(CheckBoxMouseDown);
                ghostCheckBox.MouseUp += new MouseEventHandler(GhostCheckBoxMouseUp);
                labelYPosition += labelHeight;
                translator.RegisterControl(ghost.GetName(), ghostCheckBox);
                Controls.Add(ghostCheckBox);
                ghost.SetCheckBox(ghostCheckBox);
            }
        }

        private void RegisterEvidenceCheckBox(uint evidence, string name, string image)
        {
            CheckBox checkBox = new CheckBox
            {
                Appearance = Appearance.Button,
                BackColor = Color.Transparent,
                FlatStyle = FlatStyle.Flat,
                Font = new Font(GetDefaultFontFamily(), 24),
                ForeColor = UNSELECTED_EVIDENCE,
                UseVisualStyleBackColor = false,
                AutoSize = true
            };
            m_RegisteredControls.Add(new KeyValuePair<string, Control>(name, checkBox));
            checkBox.FlatAppearance.BorderSize = 0;
            checkBox.FlatAppearance.CheckedBackColor = Color.Transparent;
            checkBox.FlatAppearance.MouseDownBackColor = Color.Gray;
            checkBox.FlatAppearance.MouseOverBackColor = Color.Gray;
            m_CheckBoxToEvidence[checkBox] = evidence;
            m_EvidenceToCheckBox[evidence] = checkBox;
            m_Overlay.RegisterEvidenceImage(evidence, image);
            checkBox.MouseDown += new MouseEventHandler(CheckBoxMouseDown);
            checkBox.MouseUp += new MouseEventHandler(EvidenceCheckBoxMouseUp);
            Controls.Add(checkBox);
            ++m_NbEvidence;
        }

        private void UpdateCheckBoxPosition()
        {
            int checkBoxListHeight = 800;
            int checkBoxWidth = 200;
            float fontSize = 24;
            int checkBoxHeight = checkBoxListHeight / (m_Ghosts.Count - 1); // - 1 as we don't count the last one which will be placed at Y = 500
            SizeF stringSize = TextRenderer.MeasureText("A", new Font(GetDefaultFontFamily(), fontSize));
            float newFontSize = fontSize * Math.Min(checkBoxHeight / stringSize.Height, (checkBoxWidth - 10) / stringSize.Width);
            if (newFontSize < fontSize)
                fontSize = newFontSize;
            int checkBoxYPosition = 450;
            for (uint evidence = 0; evidence != m_NbEvidence; ++evidence)
            {
                CheckBox checkBox = m_EvidenceToCheckBox[(uint)evidence];
                checkBox.Location = new Point(505, checkBoxYPosition);
                checkBox.Font = new Font(GetDefaultFontFamily(), fontSize);
                checkBoxYPosition -= checkBoxHeight;
            }
        }

        private void RegisterGhost(string name, List<uint> evidences)
        {
            Ghost ghost = new Ghost(name);
            foreach (uint evidence in evidences)
                ghost.AddEvidence(evidence);
            m_Ghosts.Add(ghost);
        }

        public override void OnOpen()
        {
            m_Overlay.OnOpen();
            Translator translator = GetTranslator();
            foreach (KeyValuePair<string, Control> pair in m_RegisteredControls)
                translator.RegisterControl(pair.Key, pair.Value);
            PanelUIManager.RegisterImageButton(BackButton, Properties.Resources.red_arrow, Properties.Resources.red_arrow_over, Properties.Resources.red_arrow_clicked);
            Reset();
        }

        public override void OnClose()
        {
            m_Overlay.OnClose();
        }

        public override Control GetOverlayControl()
        {
            return m_Overlay;
        }

        private void Reset()
        {
            m_SelectedEvidences.Clear();
            m_RemovedEvidences.Clear();
            UpdateEvidences();
        }

        private void UpdateEvidences()
        {
            HashSet<uint> possibleEvidence = new HashSet<uint>();
            List<Ghost> possibleGhost = new List<Ghost>();

            if (m_SelectedGhost != null)
            {
                foreach (Ghost ghost in m_Ghosts)
                {
                    CheckBox ghostCheckBox = ghost.GetCheckBox();
                    ghostCheckBox.Enabled = false;
                    ghostCheckBox.ForeColor = IMPOSSIBLE_EVIDENCE;
                }

                foreach (uint ghostEvidence in m_SelectedGhost.GetEvidences())
                    possibleEvidence.Add(ghostEvidence);
                CheckBox selectedGhostCheckBox = m_SelectedGhost.GetCheckBox();
                selectedGhostCheckBox.Enabled = true;
                selectedGhostCheckBox.ForeColor = SELECTED_EVIDENCE;
                possibleGhost.Add(m_SelectedGhost);
            }
            else
            {
                foreach (Ghost ghost in m_Ghosts)
                {
                    CheckBox ghostCheckBox = ghost.GetCheckBox();
                    if (m_RemovedGhost.Contains(ghost))
                    {
                        ghostCheckBox.ForeColor = REMOVED_EVIDENCE;
                    }
                    else
                    {
                        if (ghost.Match(m_SelectedEvidences, m_RemovedEvidences))
                        {
                            foreach (uint ghostEvidence in ghost.GetEvidences())
                                possibleEvidence.Add(ghostEvidence);
                            ghostCheckBox.Enabled = true;
                            ghostCheckBox.ForeColor = UNSELECTED_EVIDENCE;
                            possibleGhost.Add(ghost);
                        }
                        else
                        {
                            ghostCheckBox.Enabled = false;
                            ghostCheckBox.ForeColor = IMPOSSIBLE_EVIDENCE;
                        }
                    }
                }
            }

            m_Overlay.Update(m_SelectedEvidences, possibleGhost);

            for (uint evidence = 0; evidence != m_NbEvidence; ++evidence)
            {
                CheckBox checkBox = m_EvidenceToCheckBox[evidence];
                if (m_RemovedEvidences.Contains(evidence))
                    checkBox.ForeColor = REMOVED_EVIDENCE;
                else if (m_SelectedEvidences.Contains(evidence))
                    checkBox.ForeColor = SELECTED_EVIDENCE;
                else
                {
                    if (!possibleEvidence.Contains(evidence))
                    {
                        checkBox.Enabled = false;
                        checkBox.ForeColor = IMPOSSIBLE_EVIDENCE;
                    }
                    else
                    {
                        checkBox.Enabled = true;
                        checkBox.ForeColor = UNSELECTED_EVIDENCE;
                    }
                }
            }
        }

        private void CheckBoxMouseDown(object sender, MouseEventArgs e)
        {
            m_CheckBoxDownOn = sender as CheckBox;
        }

        private void EvidenceCheckBoxMouseUp(object sender, MouseEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == m_CheckBoxDownOn)
            {
                uint evidence = m_CheckBoxToEvidence[checkBox];
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        {
                            if (!m_RemovedEvidences.Contains(evidence))
                            {
                                if (checkBox.Checked)
                                    m_SelectedEvidences.Add(evidence);
                                else
                                    m_SelectedEvidences.Remove(evidence);
                            }
                            else
                                checkBox.Checked = false;
                            break;
                        }
                    case MouseButtons.Right:
                        {
                            if (!checkBox.Checked)
                            {
                                if (!m_RemovedEvidences.Contains(evidence))
                                    m_RemovedEvidences.Add(evidence);
                                else
                                    m_RemovedEvidences.Remove(evidence);
                            }
                            break;
                        }
                }
                UpdateEvidences();
            }
            m_CheckBoxDownOn = null;
        }

        private void GhostCheckBoxMouseUp(object sender, MouseEventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            if (checkBox == m_CheckBoxDownOn)
            {
                Ghost ghost = m_CheckBoxToGhost[checkBox];
                switch (e.Button)
                {
                    case MouseButtons.Left:
                        {
                            if (!m_RemovedGhost.Contains(ghost))
                            {
                                if (checkBox.Checked)
                                    m_SelectedGhost = ghost;
                                else
                                    m_SelectedGhost = null;
                            }
                            else
                                checkBox.Checked = false;
                            break;
                        }
                    case MouseButtons.Right:
                        {
                            if (!checkBox.Checked)
                            {
                                if (!m_RemovedGhost.Contains(ghost))
                                    m_RemovedGhost.Add(ghost);
                                else
                                    m_RemovedGhost.Remove(ghost);
                            }
                            break;
                        }
                }
                UpdateEvidences();
            }
            m_CheckBoxDownOn = null;
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            GetMainWindow().SetPanel(EPanelType.TitleScreen);
        }
    }
}
