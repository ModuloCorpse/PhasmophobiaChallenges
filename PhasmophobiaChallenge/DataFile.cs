using System;
using System.Collections.Generic;

namespace PhasmophobiaChallenge
{
    public class DataFile
    {
        private readonly List<KeyValuePair<EPanelType, DataFragment>> m_Fragments = new List<KeyValuePair<EPanelType, DataFragment>>();

        public DataFile()
        {
            Json json = Json.LoadFromFile("config.json");
            foreach (EPanelType type in Enum.GetValues(typeof(EPanelType)))
            {
                if (type != EPanelType.Count && type != EPanelType.Invalid)
                {
                    Json fragment = json.Get<Json>(type.ToString());
                    if (fragment == null)
                        fragment = new Json();
                    m_Fragments.Add(new KeyValuePair<EPanelType, DataFragment>(type, new DataFragment(fragment, this)));
                }
            }
        }

        public DataFragment GetFragment(EPanelType type)
        {
            foreach (KeyValuePair<EPanelType, DataFragment> pair in m_Fragments)
            {
                if (pair.Key == type)
                    return pair.Value;
            }
            return null;
        }

        public void Save()
        {
            Json json = new Json();
            foreach (KeyValuePair<EPanelType, DataFragment> pair in m_Fragments)
                json.Add(pair.Key.ToString(), pair.Value.GetData());
            json.ToFile("config.json");
        }
    }
}
