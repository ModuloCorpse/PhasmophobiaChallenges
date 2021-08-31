using System.Collections.Generic;

namespace PhasmophobiaChallenge.Panel.StoryMode
{
    public class StoryModeData
    {
        private readonly Dictionary<int, StoryModeItem> m_Items = new Dictionary<int, StoryModeItem>();
        private readonly Dictionary<int, StoryModeJob> m_Jobs = new Dictionary<int, StoryModeJob>();
        private readonly Dictionary<int, StoryModeTrait> m_Traits = new Dictionary<int, StoryModeTrait>();

        public StoryModeData()
        {
            Json datas = new Json(Properties.Resources.storymode);

            List<Json> items = datas.GetArray<Json>("items");
            foreach (Json itemJson in items)
            {
                if (itemJson.Find("id") && itemJson.Find("name"))
                {
                    StoryModeItem newItem = new StoryModeItem(itemJson);
                    m_Items.Add(newItem.GetID(), newItem);
                }
            }

            List<Json> jobs = datas.GetArray<Json>("jobs");
            foreach (Json jobJson in jobs)
            {
                if (jobJson.Find("id") && jobJson.Find("name"))
                {
                    StoryModeJob newJob = new StoryModeJob(jobJson);
                    m_Jobs.Add(newJob.GetID(), newJob);
                }
            }

            List<Json> traits = datas.GetArray<Json>("traits");
            foreach (Json traitJson in traits)
            {
                if (traitJson.Find("id") && traitJson.Find("name"))
                {
                    StoryModeTrait newTrait = new StoryModeTrait(traitJson);
                    m_Traits.Add(newTrait.GetID(), newTrait);
                }
            }
        }

        public ICollection<int> GetItemID() { return m_Items.Keys; }
        public ICollection<int> GetTraitsID() { return m_Traits.Keys; }
        public int GetNumberOfJob() { return m_Jobs.Count; }
        public uint GetItemPrice(int item)
        {
            if (m_Items.TryGetValue(item, out StoryModeItem itemData))
                return itemData.GetPrice();
            return 0;
        }

        public string GetItemName(int item)
        {
            if (m_Items.TryGetValue(item, out StoryModeItem itemData))
                return itemData.GetName();
            return "";
        }

        public string GetJobName(int job)
        {
            if (m_Jobs.TryGetValue(job, out StoryModeJob jobData))
                return jobData.GetName();
            return "";
        }

        public string GetTraitName(int trait)
        {
            if (m_Traits.TryGetValue(trait, out StoryModeTrait traitData))
                return traitData.GetName();
            return "";
        }

        public List<int> GetTraitExclusivity(int trait)
        {
            if (m_Traits.TryGetValue(trait, out StoryModeTrait traitData))
                return traitData.GetExclusivity();
            return new List<int>();
        }
    }
}
