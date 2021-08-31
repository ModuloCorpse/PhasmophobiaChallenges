namespace PhasmophobiaChallenge.Panel.StoryMode
{
    class StoryModeItem
    {
        private readonly int m_ID;
        private readonly string m_Name;
        private readonly uint m_Price;

        public StoryModeItem(Json json)
        {
            m_ID = json.Get<int>("id");
            m_Name = json.Get<string>("name");
            m_Price = json.Get<uint>("price");
        }

        public int GetID() { return m_ID; }
        public string GetName() { return m_Name; }
        public uint GetPrice() { return m_Price; }
    }
}
