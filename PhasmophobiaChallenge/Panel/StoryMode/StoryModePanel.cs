﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.StoryMode
{
    public partial class StoryModePanel : APhasmophobiaCompanionPanel
    {
        private readonly StoryModeData m_Data = new StoryModeData();
        private const int DefaultFontSize = 20;
        private const int DefaultLabelWidth = 260;
        private const int DefaultLabelHeight = 25;
        private readonly List<StoryModeProfile> m_Profiles = new List<StoryModeProfile>();
        private readonly List<InventoryItemUI> m_Inventory = new List<InventoryItemUI>();
        private int m_Current = 0;

        public StoryModePanel(MainWindow mainWindow): base(mainWindow, EPanelType.StoryMode, "panel.storymode")
        {
            InitializeComponent();

            MoneyLabel.Font = new Font(GetDefaultFontFamily(), 24f);
            JobLabel.Font = new Font(GetDefaultFontFamily(), 24f);
            TraitsLabel.Font = new Font(GetDefaultFontFamily(), 24f, FontStyle.Bold);
            InventoryLabel.Font = new Font(GetDefaultFontFamily(), 24f, FontStyle.Bold);
            BackButton.Font = new Font(GetDefaultFontFamily(), 28f, FontStyle.Bold);
            NewProfile.Font = new Font(GetDefaultFontFamily(), 24f, FontStyle.Bold);
            Profile.Font = new Font(GetDefaultFontFamily(), 28f, FontStyle.Bold);

            DataFragment data = GetData();
            m_Profiles = data.GetArray<StoryModeProfile>("profiles");
            if (data.Find("lastProfile"))
                m_Current = Math.Min(data.Get<int>("lastProfile"), m_Profiles.Count);
            else
                m_Current = 0;
            UpdateProfile();
            BackButton.TextChanged += new EventHandler(OnButtonTextChanged);
            NewProfile.TextChanged += new EventHandler(OnButtonTextChanged);
            Profile.TextChanged += new EventHandler(OnProfileTextChanged);
        }

        public override void OnInitialize()
        {
            foreach (int itemType in m_Data.GetItemID())
            {
                InventoryItemUI item = new InventoryItemUI(this, itemType);
                FlowLayoutInventoryPanel.Controls.Add(item);
                m_Inventory.Add(item);
            }
        }

        public override void OnOpen()
        {
            Translator translator = GetTranslator();
            translator.RegisterControl("other.back", BackButton);
            translator.RegisterControl("storymode.text.newprofile", NewProfile);
            translator.RegisterControl("storymode.text.traits", TraitsLabel);
            translator.RegisterControl("storymode.text.inventory", InventoryLabel);
            PanelUIManager.RegisterImageButton(NewProfile, Properties.Resources.main_menu_panel_button_background, Properties.Resources.main_menu_panel_button_background_over, Properties.Resources.main_menu_panel_button_background_over);
            PanelUIManager.RegisterImageButton(BackButton, Properties.Resources.main_menu_panel_button_background, Properties.Resources.main_menu_panel_button_background_over, Properties.Resources.main_menu_panel_button_background_over);
            PanelUIManager.RegisterImageButton(PreviousProfile, Properties.Resources.left_ui_arrow_border, Properties.Resources.left_ui_arrow_border_over, Properties.Resources.left_ui_arrow_border_over);
            PanelUIManager.RegisterImageButton(NextProfile, Properties.Resources.right_ui_arrow_border, Properties.Resources.right_ui_arrow_border_over, Properties.Resources.right_ui_arrow_border_over);
            PanelUIManager.RegisterImageButton(AddTraitButton, Properties.Resources.right_ui_arrow_border, Properties.Resources.right_ui_arrow_border_over, Properties.Resources.right_ui_arrow_border_over);
            PanelUIManager.RegisterImageButton(AddMoney, Properties.Resources.right_ui_arrow_border, Properties.Resources.right_ui_arrow_border_over, Properties.Resources.right_ui_arrow_border_over);
            PanelUIManager.RegisterImageButton(RemoveMoney, Properties.Resources.right_ui_arrow_border, Properties.Resources.right_ui_arrow_border_over, Properties.Resources.right_ui_arrow_border_over);
            PanelUIManager.RegisterImageButton(RemoveProfile, Properties.Resources.right_ui_arrow_border, Properties.Resources.right_ui_arrow_border_over, Properties.Resources.right_ui_arrow_border_over);

            foreach (InventoryItemUI item in m_Inventory)
                item.OnOpen();
        }

        private void SaveProfiles()
        {
            DataFragment data = GetData();
            data.Set("lastProfile", m_Current);
            data.Set("profiles", m_Profiles);
            data.Save();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            GetMainWindow().SetPanel(EPanelType.TitleScreen);
        }

        private void UpdateProfile()
        {
            Translator translator = GetTranslator();
            if (m_Profiles.Count != 0)
            {
                StoryModeProfile current = m_Profiles[m_Current];
                Profile.Text = current.GetName();
                MoneyLabel.Text = string.Format("{0}: {1}$", translator.GetString("storymode.text.money"), current.GetMoney());
                JobLabel.Text = string.Format("{0}: {1}", translator.GetString("storymode.text.job"), translator.GetString(m_Data.GetJobName(current.GetJob())));
                UpdateTraits(current);
                UpdateInventory();
            }
            else
            {
                Profile.Text = "";
                MoneyLabel.Text = string.Format("{0}: 0$", translator.GetString("storymode.text.money"));
                JobLabel.Text = string.Format("{0}: {1}", translator.GetString("storymode.text.job"), translator.GetString("storymode.text.job"));
                FlowLayoutTraitPanel.VerticalScroll.Enabled = false;
                FlowLayoutTraitPanel.Controls.Clear();
            }
        }

        private void UpdateInventory()
        {
            foreach (InventoryItemUI item in m_Inventory)
                item.UpdateLabel();
        }

        private void UpdateTraits(StoryModeProfile current)
        {
            Translator translator = GetTranslator();
            FlowLayoutTraitPanel.Controls.Clear();
            FlowLayoutTraitPanel.VerticalScroll.Enabled = false;
            foreach (int trait in current.GetTraits())
                AddTrait(translator.GetString(m_Data.GetTraitName(trait)));
        }

        private void AddTrait(string str)
        {
            Font font = new Font(GetDefaultFontFamily(), DefaultFontSize, FontStyle.Bold);
            SizeF stringSize = TextRenderer.MeasureText(str, font);
            float newFontSize = DefaultFontSize * Math.Min(DefaultLabelHeight / stringSize.Height, (DefaultLabelWidth - 10) / stringSize.Width);
            if (newFontSize < DefaultFontSize)
                font = new Font(GetDefaultFontFamily(), newFontSize, FontStyle.Bold);
            FlowLayoutTraitPanel.Controls.Add(new Label()
            {
                Size = new Size(DefaultLabelWidth, DefaultLabelHeight),
                TextAlign = ContentAlignment.TopLeft,
                ForeColor = Color.FromArgb(101, 108, 124),
                Font = font,
                Text = str
            });
        }

        private void NewProfile_Click(object sender, EventArgs e)
        {
            string profileName = ShowDialog("storymode.text.profilename", "storymode.text.enterprofilename").Trim();
            if (profileName != "")
            {
                m_Current = m_Profiles.Count;
                m_Profiles.Add(new StoryModeProfile(profileName, m_Data));
                UpdateProfile();
                SaveProfiles();
            }
        }

        private void RemoveProfile_Click(object sender, EventArgs e)
        {
            if (m_Profiles.Count != 0)
            {
                if (ShowConfirmDialog("storymode.text.confirmdeleteprofile"))
                {
                    m_Profiles.RemoveAt(m_Current); if (m_Current == 0)
                    {
                        if (m_Profiles.Count > 0)
                            m_Current = m_Profiles.Count - 1;
                    }
                    else
                        --m_Current;
                    UpdateProfile();
                    SaveProfiles();
                }
            }
        }

        private void NextProfile_Click(object sender, EventArgs e)
        {
            int previous = m_Current;
            ++m_Current;
            if (m_Current == m_Profiles.Count)
                m_Current = 0;
            if (m_Current != previous)
            {
                UpdateProfile();
                SaveProfiles();
            }
        }

        private void PreviousProfile_Click(object sender, EventArgs e)
        {
            int previous = m_Current;
            if (m_Current == 0)
            {
                if (m_Profiles.Count > 0)
                    m_Current = m_Profiles.Count - 1;
            }
            else
                --m_Current;
            if (m_Current != previous)
            {
                UpdateProfile();
                SaveProfiles();
            }
        }

        public void OnButtonTextChanged(object sender, EventArgs args)
        {
            if (sender is Button button)
            {
                SizeF stringSize = TextRenderer.MeasureText(button.Text, button.Font);
                float newFontSize = button.Font.Size * Math.Min(button.Size.Height / stringSize.Height, (button.Size.Width - 12) / stringSize.Width);
                if (newFontSize < 28)
                    button.Font = new Font(GetDefaultFontFamily(), newFontSize, FontStyle.Bold);
                else
                    button.Font = new Font(GetDefaultFontFamily(), 28, FontStyle.Bold);
            }
        }

        public void OnProfileTextChanged(object sender, EventArgs args)
        {
            if (sender is Label label)
            {
                SizeF stringSize = TextRenderer.MeasureText(label.Text, label.Font);
                float newFontSize = label.Font.Size * Math.Min(label.Size.Height / stringSize.Height, (label.Size.Width - 12) / stringSize.Width);
                if (newFontSize < 28)
                    label.Font = new Font(GetDefaultFontFamily(), newFontSize, FontStyle.Bold);
                else
                    label.Font = new Font(GetDefaultFontFamily(), 28, FontStyle.Bold);
            }
        }

        public string ShowDialog(string text, string title)
        {
            Translator translator = GetTranslator();
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = translator.GetString(title),
                StartPosition = FormStartPosition.CenterParent
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = translator.GetString(text) };
            TextBox textBox = new TextBox() { Left = 50, Top = 50, Width = 400 };
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            if (prompt.ShowDialog() == DialogResult.OK)
                return textBox.Text;
            return "";
        }

        public int ShowNumDialog(string text, string title)
        {
            Translator translator = GetTranslator();
            Form prompt = new Form()
            {
                Width = 500,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = translator.GetString(title),
                StartPosition = FormStartPosition.CenterParent
            };
            Label textLabel = new Label() { Left = 50, Top = 20, Text = translator.GetString(text) };
            NumericUpDown textBox = new NumericUpDown() { Left = 50, Top = 50, Width = 400 };
            textBox.Maximum = int.MaxValue;
            Button confirmation = new Button() { Text = "Ok", Left = 350, Width = 100, Top = 70, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            if (prompt.ShowDialog() == DialogResult.OK)
                return (int)textBox.Value;
            return -1;
        }

        public bool ShowConfirmDialog(string text)
        {
            Translator translator = GetTranslator();
            Form prompt = new Form()
            {
                Width = 300,
                Height = 120,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = translator.GetString("other.areyousure"),
                StartPosition = FormStartPosition.CenterParent
            };
            Label textLabel = new Label() { Left = 10, Top = 20, AutoSize = true, Text = translator.GetString(text) };
            Button confirmation = new Button() { Text = translator.GetString("other.yes"), Left = 180, Width = 100, Top = 50, DialogResult = DialogResult.OK };
            Button cancel = new Button() { Text = translator.GetString("other.no"), Left = 80, Width = 100, Top = 50, DialogResult = DialogResult.Cancel };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(cancel);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;
            return (prompt.ShowDialog() == DialogResult.OK);
        }

        private void AddTraitButton_Click(object sender, EventArgs e)
        {
            if (m_Profiles.Count != 0)
            {
                int newTrait = m_Profiles[m_Current].NewTrait();
                if (newTrait > 0)
                {
                    AddTrait(GetTranslator().GetString(m_Data.GetTraitName(newTrait)));
                    SaveProfiles();
                }
            }
        }

        private void AddMoney_Click(object sender, EventArgs e)
        {
            if (m_Profiles.Count != 0)
            {
                int money = ShowNumDialog("storymode.text.moneytoadd", "storymode.text.entermoneytoadd");
                if (money > 0)
                {
                    StoryModeProfile current = m_Profiles[m_Current];
                    current.AddMoney((uint)money);
                    MoneyLabel.Text = string.Format("{0}: {1}$", GetTranslator().GetString("storymode.text.money"), current.GetMoney());
                    SaveProfiles();
                }
            }
        }

        private void RemoveMoney_Click(object sender, EventArgs e)
        {
            if (m_Profiles.Count != 0)
            {
                int money = ShowNumDialog("storymode.text.moneytopay", "storymode.text.entermoneytopay");
                if (money > 0)
                {
                    StoryModeProfile current = m_Profiles[m_Current];
                    if (current.PayMoney((uint)money))
                    {
                        MoneyLabel.Text = string.Format("{0}: {1}$", GetTranslator().GetString("storymode.text.money"), current.GetMoney());
                        SaveProfiles();
                    }
                }
            }
        }

        internal uint GetQuantity(int type)
        {
            if (m_Profiles.Count != 0)
            {
                return m_Profiles[m_Current].GetQuantity(type);
            }
            return 0;
        }

        internal void BuyItem(int type)
        {
            if (m_Profiles.Count != 0)
            {
                StoryModeProfile current = m_Profiles[m_Current];
                current.Buy(type, 1);
                MoneyLabel.Text = string.Format("{0}: {1}$", GetTranslator().GetString("storymode.text.money"), current.GetMoney());
            }
        }

        internal void SellItem(int type)
        {
            if (m_Profiles.Count != 0)
            {
                StoryModeProfile current = m_Profiles[m_Current];
                current.Consume(type, 1);
                MoneyLabel.Text = string.Format("{0}: {1}$", GetTranslator().GetString("storymode.text.money"), current.GetMoney());
            }
        }

        internal string GetItemName(int item) { return m_Data.GetItemName(item); }
    }
}
