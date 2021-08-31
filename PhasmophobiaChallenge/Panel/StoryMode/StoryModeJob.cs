namespace PhasmophobiaChallenge.Panel.StoryMode
{
    public class StoryModeJob
    {
        private readonly int m_ID;
        private readonly string m_Name;

        public StoryModeJob(Json json)
        {
            m_ID = json.Get<int>("id");
            m_Name = json.Get<string>("name");
        }

        public int GetID() { return m_ID; }
        public string GetName() { return m_Name; }
    }
}
