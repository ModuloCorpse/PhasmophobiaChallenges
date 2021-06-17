using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhasmophobiaChallenge
{
    public class DataFragment
    {
        private readonly Json m_Data;
        private readonly DataFile m_File;

        public DataFragment(Json data, DataFile file)
        {
            m_Data = Json.Copy(data);
            m_File = file;
        }

        public Json GetData() { return m_Data; }

        public bool Find(string key) { return m_Data.Find(key); }
        public T Get<T>(string key) { return m_Data.Get<T>(key); }
        public List<T> GetArray<T>(string key) { return m_Data.GetArray<T>(key); }
        public List<string> GetKeys() { return m_Data.GetKeys(); }
        public bool Add(string key, object valueToAdd) { return m_Data.Add(key, valueToAdd); }
        public bool Set(string key, object valueToAdd) { return m_Data.Set(key, valueToAdd); }
        public void Save() { m_File.Save(); }
    }
}
