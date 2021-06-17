using DSharpPlus.Entities;
using System.Collections.Generic;

namespace PhasmophobiaChallenge.Panel.Discord
{
    internal class Challenge
    {
        private bool m_CanBeEdit;
        private ulong m_MessageId;
        private ulong m_AuthorId;
        private string m_Author;
        private string m_Name;
        private string m_Description;

        internal Challenge(string author, string name, string description, ulong messageId, bool canBeEdit)
        {
            m_CanBeEdit = canBeEdit;
            m_MessageId = messageId;
            m_Author = author;
            m_Name = name;
            m_Description = description;
        }

        internal Challenge(DiscordMessage message, DiscordMember author, ulong botID)
        {
            string content = message.Content;
            string[] split = content.Split(new[] { '\r', '\n' });
            List<string> lines = new List<string>();
            foreach (string line in split)
                lines.Add(line.Trim());
            int idx = 0;
            if (author != null)
                m_AuthorId = author.Id;
            else
                m_AuthorId = message.Author.Id;
            if (m_AuthorId == botID)
            {
                m_CanBeEdit = true;
                m_Name = lines[idx].Substring(15, lines[idx].Length);
                ++idx;
            }
            else if (!author.IsBot)
            {
                m_CanBeEdit = false;
                if (author != null)
                    m_Author = author.DisplayName;
                else
                    m_Author = message.Author.Username;
            }
            m_Name = lines[idx].Substring(2, lines[idx].Length - 4);
            ++idx;
            string description = "";
            while (idx != lines.Count)
            {
                description += lines[idx] + "\n";
                ++idx;
            }
            m_Description = description.Substring(0, description.Length - 1);
        }

        internal bool CanBeEdit() { return m_CanBeEdit; }
        internal ulong GetId() { return m_MessageId; }
        internal string GetAuthor() { return m_Author; }
        internal ulong GetAuthorId() { return m_AuthorId; }
        internal string GetName() { return m_Name; }
        internal string GetDescription() { return m_Description; }
        internal string GetMessage() { return "*Proposé par *" + m_Author + "\n**" + m_Name + "**\n" + m_Description; }
    }
}
