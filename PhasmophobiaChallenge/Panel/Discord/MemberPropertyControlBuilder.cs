using DSharpPlus.Entities;
using PhasmophobiaChallenge.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.Discord
{
    public class MemberPropertyControlBuilder : PropertiesControlFactory.IBuilder
    {
        private readonly Dictionary<ulong, string> m_IdToName = new Dictionary<ulong, string>();
        private readonly Dictionary<string, ulong> m_NameToId = new Dictionary<string, ulong>();

        public void AddMember(DiscordMember member)
        {
            ulong id = member.Id;
            string name = member.DisplayName;
            if (member.Username != name)
                name += " (" + member.Username + ")";
            m_IdToName[id] = name;
            m_NameToId[name] = id;
        }

        public APropertyControl Build(object value, string name) { return new DiscordIdPropertyControl(value, name, m_IdToName, m_NameToId); }
    }
}
