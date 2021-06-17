using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.RandomStuff
{
    public partial class RandomStuffPanel : APhasmophobiaCompanionPanel
    {
        private static readonly Random m_Random = new Random();
        private readonly List<EString> m_InventoryList = new List<EString>();

        public RandomStuffPanel(MainWindow mainWindow): base(mainWindow, EPanelType.RandomStuff, EString.RandomStuff)
        {
            InitializeComponent();
        }

        public override void OnOpen()
        {
            ResetInventoryList();
            Translator translator = GetTranslator();
            translator.RegisterControl(EString.Randomize, RandomizeButton);
            translator.RegisterControl(EString.Reset, ResetButton);
            PanelUIManager.RegisterTextButton(RandomizeButton, Color.White, Color.Red, Color.DarkRed);
            PanelUIManager.RegisterTextButton(ResetButton, Color.White, Color.Red, Color.DarkRed);
            PanelUIManager.RegisterImageButton(BackButton, Properties.Resources.red_arrow, Properties.Resources.red_arrow_over, Properties.Resources.red_arrow_clicked);
        }

        private void ResetInventoryList()
        {
            SetSelectedItemTo("");
            m_InventoryList.Clear();
            m_InventoryList.Add(EString.EMFReader);
            m_InventoryList.Add(EString.Lighter);
            m_InventoryList.Add(EString.Candle);
            m_InventoryList.Add(EString.UVLight);
            m_InventoryList.Add(EString.Crucifix);
            m_InventoryList.Add(EString.VideoCamera);
            m_InventoryList.Add(EString.SpiritBoxItem);
            m_InventoryList.Add(EString.Salt);
            m_InventoryList.Add(EString.SmudgeSticks);
            m_InventoryList.Add(EString.Tripod);
            m_InventoryList.Add(EString.MotionSensor);
            m_InventoryList.Add(EString.SoundSensor);
            m_InventoryList.Add(EString.SanityPills);
            m_InventoryList.Add(EString.Thermometer);
            m_InventoryList.Add(EString.GhostWritingBook);
            m_InventoryList.Add(EString.InfraredLightSensor);
            m_InventoryList.Add(EString.ParabolicMicrophone);
            m_InventoryList.Add(EString.Glowstick);
            m_InventoryList.Add(EString.HeadMountedCamera);
        }

        private void SetSelectedItemTo(string item)
        {
            ItemName.Text = item;
            ItemName.Font = new Font(ItemName.Font.FontFamily, 42.0f, ItemName.Font.Style);
            while (ItemName.Width < TextRenderer.MeasureText(ItemName.Text, new Font(ItemName.Font.FontFamily, ItemName.Font.Size, ItemName.Font.Style)).Width)
                ItemName.Font = new Font(ItemName.Font.FontFamily, ItemName.Font.Size - 0.5f, ItemName.Font.Style);
        }

        private void RandomizeButton_Click(object sender, EventArgs e)
        {
            if (m_InventoryList.Count != 0)
            {
                int idx = m_Random.Next(m_InventoryList.Count);
                string item = GetTranslator().GetString(m_InventoryList[idx]);
                item = item.Replace("â", "a");
                item = item.Replace("É", "E");
                item = item.Replace("é", "e");
                item = item.Replace("è", "e");
                m_InventoryList.RemoveAt(idx);
                SetSelectedItemTo(item);
            }
            else
                SetSelectedItemTo(GetTranslator().GetString(EString.OutOfItem));
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            ResetInventoryList();
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            GetMainWindow().SetPanel(EPanelType.TitleScreen);
        }
    }
}
