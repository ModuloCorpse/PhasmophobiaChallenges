using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace PhasmophobiaChallenge
{
    public enum ELocal
    {
        English,
        French
    }

    public class Translator
    {
        private class Local
        {
            private readonly Json m_Translations;

            public Local(byte[] byteArray)
            {
                m_Translations = new Json(byteArray);
            }

            public string GetString(string text)
            {
                return m_Translations.Get<string>(text);
            }
        }

        private readonly DataFragment m_Data;
        private List<KeyValuePair<string, Control>> m_Controls =  new List<KeyValuePair<string, Control>>();
        private Dictionary<ELocal, Local> m_Translators = new Dictionary<ELocal, Local>();
        private ELocal[] m_Locals = Enum.GetValues(typeof(ELocal)).Cast<ELocal>().ToArray();
        private uint m_Current = 0;

        public Translator(DataFragment data)
        {
            m_Data = data;
            if (m_Data.Find("local"))
                m_Current = m_Data.Get<uint>("local");

            m_Translators.Add(ELocal.French, new Local(Properties.Resources.french));
            m_Translators.Add(ELocal.English, new Local(Properties.Resources.english));
        }

        public void RegisterGlobalControl(string text, Control control)
        {
            foreach (KeyValuePair<string, Control> pair in m_Controls)
            {
                if (pair.Value == control)
                    return;
            }
            control.Text = GetString(text);
            m_Controls.Add(new KeyValuePair<string, Control>(text, control));
        }

        public void RegisterControl(string text, Control control)
        {
            foreach (KeyValuePair<string, Control> pair in m_Controls)
            {
                if (pair.Value == control)
                    return;
            }
            control.Text = GetString(text);
            m_Controls.Add(new KeyValuePair<string, Control>(text, control));
        }

        public void UnregisterControls()
        {
            m_Controls.Clear();
        }

        public string GetString(string text)
        {
            if (m_Translators.TryGetValue(m_Locals[m_Current], out Local local))
                return local.GetString(text);
            return null;
        }

        public string GetLocalName()
        {
            switch (m_Locals[m_Current])
            {
                case ELocal.English:
                    return "English";
                case ELocal.French:
                    return "Français";
            }
            return null;
        }

        public void NextLocal()
        {
            ++m_Current;
            if (m_Current == m_Locals.Length)
                m_Current = 0;
            m_Data.Set("local", m_Current);
            m_Data.Save();
            Update();
        }

        public void PreviousLocal()
        {
            if (m_Current == 0)
                m_Current = (uint)(m_Locals.Length - 1);
            else
                --m_Current;
            m_Data.Set("local", m_Current);
            m_Data.Save();
            Update();
        }

        private void Update()
        {
            foreach (KeyValuePair<string, Control> pair in m_Controls)
                pair.Value.Text = GetString(pair.Key);
        }
    }
}
