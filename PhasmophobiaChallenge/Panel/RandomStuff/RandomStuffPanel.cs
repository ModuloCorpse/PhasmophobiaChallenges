using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.RandomStuff
{
    public partial class RandomStuffPanel : APhasmophobiaCompanionPanel
    {
        private static readonly Random m_Random = new Random();
        private readonly List<string> m_InventoryList = new List<string>();

        public RandomStuffPanel(MainWindow mainWindow): base(mainWindow, EPanelType.RandomStuff, "panel.randomstuff")
        {
            InitializeComponent();
            ItemName.Font = new Font(GetDefaultFontFamily(), 42f, FontStyle.Bold);
            RandomizeButton.Font = new Font(GetDefaultFontFamily(), 36f, FontStyle.Bold);
            ResetButton.Font = new Font(GetDefaultFontFamily(), 36f, FontStyle.Bold);
        }

        public override void OnOpen()
        {
            ResetInventoryList();
            Translator translator = GetTranslator();
            translator.RegisterControl("other.randomize", RandomizeButton);
            translator.RegisterControl("other.reset", ResetButton);
            PanelUIManager.RegisterTextButton(RandomizeButton, Color.White, Color.Red, Color.DarkRed);
            PanelUIManager.RegisterTextButton(ResetButton, Color.White, Color.Red, Color.DarkRed);
            PanelUIManager.RegisterImageButton(BackButton, Properties.Resources.red_arrow, Properties.Resources.red_arrow_over, Properties.Resources.red_arrow_clicked);
        }

        private void ResetInventoryList()
        {
            SetSelectedItemTo("");
            m_InventoryList.Clear();

            Json datas = new Json(Properties.Resources.randomstuff);
            List<string> items = datas.GetArray<string>("items");
            foreach (string item in items)
                m_InventoryList.Add(item);
        }

        private void SetSelectedItemTo(string item)
        {
            SizeF stringSize = TextRenderer.MeasureText(ItemName.Text, ItemName.Font);
            float newFontSize = ItemName.Font.Size * Math.Min(ItemName.Size.Height / stringSize.Height, (ItemName.Size.Width - 10) / stringSize.Width);
            if (newFontSize < ItemName.Font.Size)
                ItemName.Font = new Font(ItemName.Font.FontFamily, newFontSize, ItemName.Font.Style);
            ItemName.Text = item;
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
                SetSelectedItemTo(GetTranslator().GetString("other.outofitem"));
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
