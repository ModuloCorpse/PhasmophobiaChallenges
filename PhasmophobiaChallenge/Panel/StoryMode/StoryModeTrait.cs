using System.Collections.Generic;

namespace PhasmophobiaChallenge.Panel.StoryMode
{
    public class StoryModeTrait
    {
        private readonly int m_ID;
        private readonly string m_Name;
        private readonly List<int> m_Exclusivity;

        public StoryModeTrait(Json json)
        {
            m_ID = json.Get<int>("id");
            m_Name = json.Get<string>("name");
            m_Exclusivity = json.GetArray<int>("exclude");
        }

        public int GetID() { return m_ID; }
        public string GetName() { return m_Name; }
        public List<int> GetExclusivity() { return m_Exclusivity; }
    }
}
