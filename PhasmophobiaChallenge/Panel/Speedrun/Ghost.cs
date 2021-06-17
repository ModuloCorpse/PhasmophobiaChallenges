using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.Speedrun
{
    internal class Ghost
    {
        private HashSet<Evidence> m_Evidences = new HashSet<Evidence>();
        private Label m_Label;
        private EString m_Name;

        internal Ghost(EString name)
        {
            m_Name = name;
        }

        internal HashSet<Evidence> GetEvidences() { return m_Evidences; }
        internal Label GetLabel() { return m_Label; }
        internal void SetLabel(Label label) { m_Label = label; }
        internal EString GetName() { return m_Name; }

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
