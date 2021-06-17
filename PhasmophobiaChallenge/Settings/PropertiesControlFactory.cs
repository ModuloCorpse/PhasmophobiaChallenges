using PhasmophobiaChallenge.Settings.PropertiesControls;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace PhasmophobiaChallenge.Settings
{
    public class PropertiesControlFactory
    {
        public interface IBuilder
        {
            APropertyControl Build(object value, string name);
        }

        private readonly Dictionary<string, IBuilder> m_NameBuilders = new Dictionary<string, IBuilder>();
        private readonly Dictionary<Type, IBuilder> m_TypeBuilders = new Dictionary<Type, IBuilder>();

        public PropertiesControlFactory()
        {
            AddBuilder(typeof(string), new TextPropertyControl.Builder());
            AddBuilder(typeof(ulong), new ULongPropertyControl.Builder());
        }

        public void AddBuilder(string name, IBuilder builder)
        {
            m_NameBuilders[name] = builder;
        }

        public void AddBuilder(Type type, IBuilder builder)
        {
            m_TypeBuilders[type] = builder;
        }

        internal APropertyControl Build(string property, object value)
        {
            IBuilder builder = null;
            if (m_NameBuilders.ContainsKey(property))
                builder = m_NameBuilders[property];
            else if (m_TypeBuilders.ContainsKey(value.GetType())) //TODO
                builder = m_TypeBuilders[value.GetType()];
            if (builder != null)
                return builder.Build(value, property);
            return null;
        }
    }
}
