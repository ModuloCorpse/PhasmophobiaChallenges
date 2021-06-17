using System.Configuration;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Settings
{
    public abstract class APropertyControl
    {
        protected object m_Property;
        protected string m_Name;
        private Control m_Control = null;

        protected APropertyControl(object value, string name)
        {
            m_Property = value;
            m_Name = name;
        }

        internal void Init()
        {
            m_Control = GenerateControl();
            m_Control.Name = m_Name;
        }

        internal void Save(DataFragment configuration)
        {
            configuration.Set(m_Name, ComputeValue());
        }

        internal Control GetControl()
        {
            return m_Control;
        }

        protected abstract Control GenerateControl();
        protected abstract object ComputeValue();
    }
}
