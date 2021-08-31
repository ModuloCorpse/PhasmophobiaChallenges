using System;
using System.Collections.Generic;

namespace PhasmophobiaChallenge.Panel.StoryMode
{
    class StoryModeProfile: IJsonable
    {
        private readonly StoryModeData m_Data;
        private string m_Name;
        private uint m_Money;
        private int m_Job;
        private readonly List<int> m_FreeTraits = new List<int>();
        private readonly List<int> m_Traits = new List<int>();
        private readonly Random rand = new Random();
        private readonly Dictionary<int, uint> m_Items = new Dictionary<int, uint>();

        private StoryModeProfile(StoryModeData data)
        {
            m_Data = data;
            m_FreeTraits.AddRange(data.GetTraitsID());
        }

        public StoryModeProfile(string name, StoryModeData data) : this(data)
        {
            m_Name = name;
            m_Job = rand.Next(data.GetNumberOfJob());
            NewTrait();
        }

        private StoryModeProfile(Json json, StoryModeData data) : this(data)
        {
            if (json.Find("name"))
                m_Name = json.Get<string>("name");
            else
                m_Name = "";
            if (json.Find("traits"))
            {
                List<int> storedTrait = json.GetArray<int>("traits");
                foreach (int trait in storedTrait)
                    AddTrait(trait);
            }
            else
                NewTrait();
            if (json.Find("money"))
                m_Money = json.Get<uint>("money");
            else
                m_Money = 0;
            if (json.Find("job"))
                m_Job = json.Get<int>("job");
            else
                m_Job = rand.Next(data.GetNumberOfJob());
            if (json.Find("inventory"))
            {
                List<Json> inventory = json.GetArray<Json>("inventory");
                foreach (Json item in inventory)
                {
                    if (item.Find("item") && item.Find("quantity"))
                        SetQuantity(item.Get<int>("item"), item.Get<uint>("quantity"));
                }
            }
        }

        public string GetName() { return m_Name; }
        public uint GetMoney() { return m_Money; }
        public int GetJob() { return m_Job; }
        public List<int> GetTraits() { return m_Traits; }

        public int NewTrait()
        {
            if (m_FreeTraits.Count != 0)
            {
                int newTrait = m_FreeTraits[rand.Next(m_FreeTraits.Count)];
                AddTrait(newTrait);
                return newTrait;
            }
            return -1;
        }

        private void AddTrait(int trait)
        {
            if (trait > 0)
            {
                m_Traits.Add(trait);
                m_FreeTraits.Remove(trait);
                foreach (int exclusivity in m_Data.GetTraitExclusivity(trait))
                    m_FreeTraits.Remove(exclusivity);
            }
        }

        public uint GetQuantity(int type)
        {
            if (m_Items.TryGetValue(type, out uint ret))
                return ret;
            return 0;
        }

        public void SetQuantity(int type, uint quantity)
        {
            m_Items[type] = quantity;
        }

        public void AddMoney(uint money)
        {
            m_Money += money;
        }

        public bool PayMoney(uint money)
        {
            if (money <= m_Money)
            {
                m_Money -= money;
                return true;
            }
            return false;
        }

        public bool Buy(int type, uint quantity)
        {
            uint price = quantity * m_Data.GetItemPrice(type);
            if (price <= m_Money)
            {
                m_Money -= price;
                SetQuantity(type, GetQuantity(type) + quantity);
                return true;
            }
            return false;
        }

        public void Consume(int type, uint quantity)
        {
            uint currentQuantity = GetQuantity(type);
            if (quantity <= currentQuantity)
                SetQuantity(type, currentQuantity - quantity);
            else
                SetQuantity(type, 0);
        }

        public Json Serialize()
        {
            Json ret = new Json();
            ret.Set("name", m_Name);
            ret.Set("money", m_Money);
            ret.Set("job", (uint)m_Job);
            List<uint> traits = new List<uint>();
            foreach (int trait in m_Traits)
                traits.Add((uint)trait);
            ret.Set("traits", traits);
            List<Json> inventory = new List<Json>();
            foreach (KeyValuePair<int, uint> pair in m_Items)
            {
                Json tmp = new Json();
                tmp.Add("item", (uint)pair.Key);
                tmp.Add("quantity", pair.Value);
                inventory.Add(tmp);
            }
            ret.Set("inventory", inventory);
            return ret;
        }

        public static StoryModeProfile Deserialize(Json json)
        {
            return new StoryModeProfile(json, new StoryModeData());
        }
    }
}
