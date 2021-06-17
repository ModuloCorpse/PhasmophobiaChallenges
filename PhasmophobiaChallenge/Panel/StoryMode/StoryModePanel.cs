using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.StoryMode
{
    public partial class StoryModePanel : APhasmophobiaCompanionPanel
    {
        private const int DefaultFontSize = 20;
        private const int DefaultLabelWidth = 260;
        private const int DefaultLabelHeight = 25;
        private readonly List<StoryModeData> m_Profiles = new List<StoryModeData>();
        private readonly List<InventoryItemUI> m_Inventory = new List<InventoryItemUI>();
        private int m_Current = 0;

        public StoryModePanel(MainWindow mainWindow): base(mainWindow, EPanelType.StoryMode, EString.StoryMode)
        {
            InitializeComponent();

            DataFragment data = GetData();
            m_Profiles = data.GetArray<StoryModeData>("profiles");
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
            foreach (EItemType itemType in Enum.GetValues(typeof(EItemType)))
            {
                InventoryItemUI item = new InventoryItemUI(this, itemType);
                FlowLayoutInventoryPanel.Controls.Add(item);
                m_Inventory.Add(item);
            }
        }

        public override void OnOpen()
        {
            Translator translator = GetTranslator();
            translator.RegisterControl(EString.Back, BackButton);
            translator.RegisterControl(EString.NewProfile, NewProfile);
            translator.RegisterControl(EString.Traits, TraitsLabel);
            translator.RegisterControl(EString.Inventory, InventoryLabel);
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
                StoryModeData current = m_Profiles[m_Current];
                Profile.Text = current.GetName();
                MoneyLabel.Text = string.Format("{0}: {1}$", translator.GetString(EString.Money), current.GetMoney());
                JobLabel.Text = string.Format("{0}: {1}", translator.GetString(EString.Job), translator.GetString(StoryModeData.GetJobName(current.GetJob())));
                UpdateTraits(current);
                UpdateInventory();
            }
            else
            {
                Profile.Text = "";
                MoneyLabel.Text = string.Format("{0}: 0$", translator.GetString(EString.Money));
                JobLabel.Text = string.Format("{0}: {1}", translator.GetString(EString.Job), translator.GetString(EString.Job));
                FlowLayoutTraitPanel.VerticalScroll.Enabled = false;
                FlowLayoutTraitPanel.Controls.Clear();
            }
        }

        private void UpdateInventory()
        {
            foreach (InventoryItemUI item in m_Inventory)
                item.UpdateLabel();
        }

        private void UpdateTraits(StoryModeData current)
        {
            Translator translator = GetTranslator();
            FlowLayoutTraitPanel.Controls.Clear();
            FlowLayoutTraitPanel.VerticalScroll.Enabled = false;
            foreach (ETrait trait in current.GetTraits())
                AddTrait(translator.GetString(StoryModeData.GetTraitName(trait)));
        }

        private void AddTrait(string str)
        {
            Font font = new Font(GetFontFamily(), DefaultFontSize, FontStyle.Bold);
            SizeF stringSize = TextRenderer.MeasureText(str, font);
            float newFontSize = DefaultFontSize * Math.Min(DefaultLabelHeight / stringSize.Height, (DefaultLabelWidth - 10) / stringSize.Width);
            if (newFontSize < DefaultFontSize)
                font = new Font(GetFontFamily(), newFontSize, FontStyle.Bold);
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
            string profileName = ShowDialog(EString.ProfileName, EString.EnterProfileName).Trim();
            if (profileName != "")
            {
                m_Current = m_Profiles.Count;
                m_Profiles.Add(new StoryModeData(profileName));
                UpdateProfile();
                SaveProfiles();
            }
        }

        private void RemoveProfile_Click(object sender, EventArgs e)
        {
            if (m_Profiles.Count != 0)
            {
                if (ShowConfirmDialog(EString.ConfirmDeleteProfile))
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
                    button.Font = new Font(GetFontFamily(), newFontSize, FontStyle.Bold);
                else
                    button.Font = new Font(GetFontFamily(), 28, FontStyle.Bold);
            }
        }

        public void OnProfileTextChanged(object sender, EventArgs args)
        {
            if (sender is Label label)
            {
                SizeF stringSize = TextRenderer.MeasureText(label.Text, label.Font);
                float newFontSize = label.Font.Size * Math.Min(label.Size.Height / stringSize.Height, (label.Size.Width - 12) / stringSize.Width);
                if (newFontSize < 28)
                    label.Font = new Font(GetFontFamily(), newFontSize, FontStyle.Bold);
                else
                    label.Font = new Font(GetFontFamily(), 28, FontStyle.Bold);
            }
        }

        public string ShowDialog(EString text, EString title)
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

        public int ShowNumDialog(EString text, EString title)
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

        public bool ShowConfirmDialog(EString text)
        {
            Translator translator = GetTranslator();
            Form prompt = new Form()
            {
                Width = 300,
                Height = 120,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = translator.GetString(EString.AreYouSure),
                StartPosition = FormStartPosition.CenterParent
            };
            Label textLabel = new Label() { Left = 10, Top = 20, AutoSize = true, Text = translator.GetString(text) };
            Button confirmation = new Button() { Text = translator.GetString(EString.Yes), Left = 180, Width = 100, Top = 50, DialogResult = DialogResult.OK };
            Button cancel = new Button() { Text = translator.GetString(EString.No), Left = 80, Width = 100, Top = 50, DialogResult = DialogResult.Cancel };
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
                ETrait newTrait = m_Profiles[m_Current].NewTrait();
                if (newTrait != ETrait.Invalid)
                {
                    AddTrait(GetTranslator().GetString(StoryModeData.GetTraitName(newTrait)));
                    SaveProfiles();
                }
            }
        }

        private void AddMoney_Click(object sender, EventArgs e)
        {
            if (m_Profiles.Count != 0)
            {
                int money = ShowNumDialog(EString.MoneyToAdd, EString.EnterMoneyToAdd);
                if (money > 0)
                {
                    StoryModeData current = m_Profiles[m_Current];
                    current.AddMoney((uint)money);
                    MoneyLabel.Text = string.Format("{0}: {1}$", GetTranslator().GetString(EString.Money), current.GetMoney());
                    SaveProfiles();
                }
            }
        }

        private void RemoveMoney_Click(object sender, EventArgs e)
        {
            if (m_Profiles.Count != 0)
            {
                int money = ShowNumDialog(EString.MoneyToPay, EString.EnterMoneyToPay);
                if (money > 0)
                {
                    StoryModeData current = m_Profiles[m_Current];
                    if (current.PayMoney((uint)money))
                    {
                        MoneyLabel.Text = string.Format("{0}: {1}$", GetTranslator().GetString(EString.Money), current.GetMoney());
                        SaveProfiles();
                    }
                }
            }
        }

        internal uint GetQuantity(EItemType type)
        {
            if (m_Profiles.Count != 0)
            {
                return m_Profiles[m_Current].GetQuantity(type);
            }
            return 0;
        }

        internal void BuyItem(EItemType type)
        {
            if (m_Profiles.Count != 0)
            {
                StoryModeData current = m_Profiles[m_Current];
                current.Buy(type, 1);
                MoneyLabel.Text = string.Format("{0}: {1}$", GetTranslator().GetString(EString.Money), current.GetMoney());
            }
        }

        internal void SellItem(EItemType type)
        {
            if (m_Profiles.Count != 0)
            {
                StoryModeData current = m_Profiles[m_Current];
                current.Consume(type, 1);
                MoneyLabel.Text = string.Format("{0}: {1}$", GetTranslator().GetString(EString.Money), current.GetMoney());
            }
        }
    }
}
