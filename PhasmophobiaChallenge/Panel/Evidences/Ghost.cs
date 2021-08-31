using System.Collections.Generic;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.Evidences
{
    internal class Ghost
    {
        private HashSet<uint> m_Evidences = new HashSet<uint>();
        private Label m_Label;
        private string m_Name;

        internal Ghost(string name)
        {
            m_Name = name;
        }

        internal HashSet<uint> GetEvidences() { return m_Evidences; }
        internal Label GetLabel() { return m_Label; }
        internal void SetLabel(Label label) { m_Label = label; }
        internal string GetName() { return m_Name; }

        internal void AddEvidence(uint evidence) { m_Evidences.Add(evidence); }

        internal bool Match(HashSet<uint> evidences, HashSet<uint> removedEvidence)
        {
            foreach (uint evidence in evidences)
            {
                if (!m_Evidences.Contains(evidence))
                    return false;
            }
            foreach (uint evidence in removedEvidence)
            {
                if (m_Evidences.Contains(evidence))
                    return false;
            }
            return true;
        }
    }
}
