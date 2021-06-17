using DSharpPlus.Entities;
using PhasmophobiaChallenge.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.Discord
{
    public class DiscordIdPropertyControl : APropertyControl
    {
        private readonly Dictionary<ulong, string> m_IdToName;
        private readonly Dictionary<string, ulong> m_NameToId;
        private ComboBox m_ComboBox;

        public DiscordIdPropertyControl(object value, string name, Dictionary<ulong, string> idToName, Dictionary<string, ulong> nameToId) : base(value, name)
        {
            m_IdToName = idToName;
            m_NameToId = nameToId;
        }

        protected override Control GenerateControl()
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true
            };
            Label nameLabel = new Label
            {
                Text = m_Name,
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            nameLabel.Size = new System.Drawing.Size(125, nameLabel.Size.Height);
            ulong selectedId = (ulong)m_Property;
            string selectedChannel = "";
            if (m_IdToName.ContainsKey(selectedId))
                selectedChannel = m_IdToName[selectedId];
            m_ComboBox = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            foreach (string channel in m_NameToId.Keys)
            {
                m_ComboBox.Items.Add(channel);
                if (selectedChannel == channel)
                    m_ComboBox.SelectedItem = channel;
            }
            m_ComboBox.Size = new System.Drawing.Size(300, m_ComboBox.Size.Height);
            panel.Controls.Add(nameLabel);
            panel.Controls.Add(m_ComboBox);
            return panel;
        }

        protected override object ComputeValue()
        {
            return m_NameToId[m_ComboBox.SelectedItem as string];
        }
    }
}
