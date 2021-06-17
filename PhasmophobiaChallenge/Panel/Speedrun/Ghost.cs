using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.Speedrun
{
    internal class Ghost
    {
        private HashSet<Evidence> m_Evidences = new HashSet<Evidence>();
        private readonly Label m_Label;

        internal Ghost(EString name, Translator translator)
        {
            m_Label = new Label
            {
                BackColor = Color.Transparent,
                Font = new Font("Yahfie", 24),
                ForeColor = Color.White,
                AutoSize = true,
            };
            translator.RegisterControl(name, m_Label);
        }

        internal HashSet<Evidence> GetEvidences() { return m_Evidences; }
        internal Label GetLabel() { return m_Label; }

        internal void AddEvidence(Evidence evidence) { m_Evidences.Add(evidence); }

        internal bool Match(HashSet<Evidence> evidences)
        {
            foreach (Evidence evidence in evidences)
            {
                if (!m_Evidences.Contains(evidence))
                    return false;
            }
            return true;
        }
    }
}
