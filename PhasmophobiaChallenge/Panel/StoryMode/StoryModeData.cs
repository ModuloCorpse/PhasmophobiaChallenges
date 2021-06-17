using System;
using System.Collections.Generic;

namespace PhasmophobiaChallenge.Panel.StoryMode
{
    public enum EItemType
    {
        EMFReader, //0
        Flashlight, //1
        StrongFlashlight, //2
        PhotoCamera, //3
        Lighter, //4
        Candle, //5
        UVLight, //6
        Crucifix, //7
        VideoCamera, //8
        SpiritBox, //9
        Salt, //10
        SmudgeSticks, //11
        Tripod, //12
        MotionSensor, //13
        SoundSensor, //14
        SanityPills, //15
        Thermometer, //16
        GhostWritingBook, //17
        InfraredLightSensor, //18
        ParabolicMicrophone, //19
        Glowstick, //20
        HeadMountedCamera //21
    }

    public enum EJob
    {
        Archaeologist, //0
        TeenageMovieActor, //1
        Unemployed, //2
        BabySitter, //3
        Speedrunner, //4
        Podcaster, //5
        Cameraman, //6
        Doctor //7
    }

    public enum ETrait
    {
        Sensitive, //0
        Technophobe, //1
        Dwarf, //2
        Bold, //3
        Chilly, //4
        ButterFingers, //5
        Courteous, //6
        Coward, //7
        Deaf, //8
        Confuse, //9
        Alzheimer, //10
        Germophobe, //11
        Cardiac, //12
        Asthmatic, //13
        Nycotophobic, //14
        Giant, //15
        Paranoide, //16
        Attentive, //17
        Conspiratorial, //18
        Vampire, //19
        Teenage, //20
        Hurry, //21
        Shy, //22
        SmallPocket, //23
        Fussy, //24
        Inattentive, //25
        WeakBladder, //26
        Invalid
    }

    class StoryModeData: IJsonable
    {
        private string m_Name;
        private uint m_Money;
        private EJob m_Job;
        private readonly List<ETrait> m_FreeTraits = new List<ETrait>();
        private readonly List<ETrait> m_Traits = new List<ETrait>();
        private readonly Random rand = new Random();
        private readonly Dictionary<EItemType, uint> m_Items = new Dictionary<EItemType, uint>();

        private StoryModeData()
        {
            foreach (ETrait trait in Enum.GetValues(typeof(ETrait)))
            {
                if (trait != ETrait.Invalid)
                    m_FreeTraits.Add(trait);
            }
        }

        public StoryModeData(string name): this()
        {
            m_Name = name;
            m_Job = (EJob)rand.Next(Enum.GetValues(typeof(EJob)).Length);
            NewTrait();
        }

        private StoryModeData(Json json) : this()
        {
            if (json.Find("name"))
                m_Name = json.Get<string>("name");
            else
                m_Name = "";
            if (json.Find("traits"))
            {
                List<uint> storedTrait = json.GetArray<uint>("traits");
                foreach (uint trait in storedTrait)
                    AddTrait((ETrait)trait);
            }
            else
                NewTrait();
            if (json.Find("money"))
                m_Money = json.Get<uint>("money");
            else
                m_Money = 0;
            if (json.Find("job"))
                m_Job = (EJob)json.Get<uint>("job");
            else
                m_Job = (EJob)rand.Next(Enum.GetValues(typeof(EJob)).Length);
            if (json.Find("inventory"))
            {
                List<Json> inventory = json.GetArray<Json>("inventory");
                foreach (Json item in inventory)
                {
                    if (item.Find("item") && item.Find("quantity"))
                        SetQuantity((EItemType)item.Get<uint>("item"), item.Get<uint>("quantity"));
                }
            }
        }

        public string GetName() { return m_Name; }
        public uint GetMoney() { return m_Money; }
        public EJob GetJob() { return m_Job; }
        public List<ETrait> GetTraits() { return m_Traits; }

        public ETrait NewTrait()
        {
            if (m_FreeTraits.Count != 0)
            {
                ETrait newTrait = m_FreeTraits[rand.Next(m_FreeTraits.Count)];
                AddTrait(newTrait);
                return newTrait;
            }
            return ETrait.Invalid;
        }

        private void AddTrait(ETrait trait)
        {
            if (trait != ETrait.Invalid)
            {
                m_Traits.Add(trait);
                m_FreeTraits.Remove(trait);
                switch (trait)
                {
                    case ETrait.Deaf:
                        {
                            m_FreeTraits.Remove(ETrait.Confuse);
                            break;
                        }
                    case ETrait.Confuse:
                        {
                            m_FreeTraits.Remove(ETrait.Deaf);
                            break;
                        }
                    case ETrait.Nycotophobic:
                        {
                            m_FreeTraits.Remove(ETrait.Vampire);
                            break;
                        }
                    case ETrait.Vampire:
                        {
                            m_FreeTraits.Remove(ETrait.Nycotophobic);
                            break;
                        }
                }
            }
        }

        public uint GetQuantity(EItemType type)
        {
            if (m_Items.TryGetValue(type, out uint ret))
                return ret;
            return 0;
        }

        public void SetQuantity(EItemType type, uint quantity)
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

        public bool Buy(EItemType type, uint quantity)
        {
            uint price = quantity * GetItemPrice(type);
            if (price <= m_Money)
            {
                m_Money -= price;
                SetQuantity(type, GetQuantity(type) + quantity);
                return true;
            }
            return false;
        }

        public void Consume(EItemType type, uint quantity)
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
            foreach (ETrait trait in m_Traits)
                traits.Add((uint)trait);
            ret.Set("traits", traits);
            List<Json> inventory = new List<Json>();
            foreach (KeyValuePair<EItemType, uint> pair in m_Items)
            {
                Json tmp = new Json();
                tmp.Add("item", (uint)pair.Key);
                tmp.Add("quantity", pair.Value);
                inventory.Add(tmp);
            }
            ret.Set("inventory", inventory);
            return ret;
        }

        public static StoryModeData Deserialize(Json json)
        {
            return new StoryModeData(json);
        }

        public static EString GetItemName(EItemType type)
        {
            switch (type)
            {
                case EItemType.EMFReader: return EString.EMFReader;
                case EItemType.Flashlight: return EString.Flashlight;
                case EItemType.StrongFlashlight: return EString.StrongFlashlight;
                case EItemType.PhotoCamera: return EString.PhotoCamera;
                case EItemType.Lighter: return EString.Lighter;
                case EItemType.Candle: return EString.Candle;
                case EItemType.UVLight: return EString.UVLight;
                case EItemType.Crucifix: return EString.Crucifix;
                case EItemType.VideoCamera: return EString.VideoCamera;
                case EItemType.SpiritBox: return EString.SpiritBoxItem;
                case EItemType.Salt: return EString.Salt;
                case EItemType.SmudgeSticks: return EString.SmudgeSticks;
                case EItemType.Tripod: return EString.Tripod;
                case EItemType.MotionSensor: return EString.MotionSensor;
                case EItemType.SoundSensor: return EString.SoundSensor;
                case EItemType.SanityPills: return EString.SanityPills;
                case EItemType.Thermometer: return EString.Thermometer;
                case EItemType.GhostWritingBook: return EString.GhostWritingBook;
                case EItemType.InfraredLightSensor: return EString.InfraredLightSensor;
                case EItemType.ParabolicMicrophone: return EString.ParabolicMicrophone;
                case EItemType.Glowstick: return EString.Glowstick;
                case EItemType.HeadMountedCamera: return EString.HeadMountedCamera;
                default: return EString.Invalid;
            }
        }

        public static uint GetItemPrice(EItemType type)
        {
            switch (type)
            {
                case EItemType.EMFReader: return 45;
                case EItemType.Flashlight: return 30;
                case EItemType.PhotoCamera: return 40;
                case EItemType.Lighter: return 10;
                case EItemType.Candle: return 15;
                case EItemType.UVLight: return 35;
                case EItemType.Crucifix: return 30;
                case EItemType.VideoCamera: return 50;
                case EItemType.SpiritBox: return 50;
                case EItemType.Salt: return 15;
                case EItemType.SmudgeSticks: return 15;
                case EItemType.Tripod: return 25;
                case EItemType.StrongFlashlight: return 50;
                case EItemType.MotionSensor: return 100;
                case EItemType.SoundSensor: return 80;
                case EItemType.SanityPills: return 45;
                case EItemType.Thermometer: return 30;
                case EItemType.GhostWritingBook: return 40;
                case EItemType.InfraredLightSensor: return 65;
                case EItemType.ParabolicMicrophone: return 50;
                case EItemType.Glowstick: return 20;
                case EItemType.HeadMountedCamera: return 60;
                default: return 0;
            }
        }

        public static EString GetJobName(EJob type)
        {
            switch (type)
            {
                case EJob.Archaeologist: return EString.Archaeologist;
                case EJob.TeenageMovieActor: return EString.TeenageMovieActor;
                case EJob.Unemployed: return EString.Unemployed;
                case EJob.BabySitter: return EString.BabySitter;
                case EJob.Speedrunner: return EString.Speedrunner;
                case EJob.Podcaster: return EString.Podcaster;
                case EJob.Cameraman: return EString.Cameraman;
                case EJob.Doctor: return EString.Doctor;
            }
            return EString.Invalid;
        }

        public static EString GetTraitName(ETrait type)
        {
            switch (type)
            {
                case ETrait.Sensitive: return EString.Sensitive;
                case ETrait.Technophobe: return EString.Technophobe;
                case ETrait.Dwarf: return EString.Dwarf;
                case ETrait.Bold: return EString.Bold;
                case ETrait.Chilly: return EString.Chilly;
                case ETrait.ButterFingers: return EString.ButterFingers;
                case ETrait.Courteous: return EString.Courteous;
                case ETrait.Coward: return EString.Coward;
                case ETrait.Deaf: return EString.Deaf;
                case ETrait.Confuse: return EString.Confuse;
                case ETrait.Alzheimer: return EString.Alzheimer;
                case ETrait.Germophobe: return EString.Germophobe;
                case ETrait.Cardiac: return EString.Cardiac;
                case ETrait.Asthmatic: return EString.Asthmatic;
                case ETrait.Nycotophobic: return EString.Nycotophobic;
                case ETrait.Giant: return EString.Giant;
                case ETrait.Paranoide: return EString.Paranoide;
                case ETrait.Attentive: return EString.Attentive;
                case ETrait.Conspiratorial: return EString.Conspiratorial;
                case ETrait.Vampire: return EString.Vampire;
                case ETrait.Teenage: return EString.Teenage;
                case ETrait.Hurry: return EString.Hurry;
                case ETrait.Shy: return EString.Shy;
                case ETrait.SmallPocket: return EString.SmallPocket;
                case ETrait.Fussy: return EString.Fussy;
                case ETrait.Inattentive: return EString.Inattentive;
                case ETrait.WeakBladder: return EString.WeakBladder;
            }
            return EString.Invalid;
        }
    }
}
