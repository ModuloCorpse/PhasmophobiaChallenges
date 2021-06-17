using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel.StoryMode
{
    public partial class InventoryItemUI : UserControl
    {
        private readonly StoryModePanel m_Panel;
        private readonly EItemType m_ItemType;

        public InventoryItemUI(StoryModePanel panel, EItemType itemType)
        {
            InitializeComponent();
            m_Panel = panel;
            m_ItemType = itemType;
            UpdateLabel();
        }

        public void OnOpen()
        {
            UpdateLabel();
            PanelUIManager.RegisterImageButton(BuyItem, Properties.Resources.right_ui_arrow_border, Properties.Resources.right_ui_arrow_border_over, Properties.Resources.right_ui_arrow_border_over);
            PanelUIManager.RegisterImageButton(SellItem, Properties.Resources.right_ui_arrow_border, Properties.Resources.right_ui_arrow_border_over, Properties.Resources.right_ui_arrow_border_over);
        }

        public void UpdateLabel()
        {
            ItemInventory.Text = string.Format("{0}x {1}", m_Panel.GetQuantity(m_ItemType), m_Panel.GetTranslator().GetString(StoryModeData.GetItemName(m_ItemType)));
            SizeF stringSize = TextRenderer.MeasureText(ItemInventory.Text, ItemInventory.Font);
            float newFontSize = ItemInventory.Font.Size * Math.Min(ItemInventory.Size.Height / stringSize.Height, (ItemInventory.Size.Width - 10) / stringSize.Width);
            if (newFontSize < ItemInventory.Font.Size)
                ItemInventory.Font = new Font("Yahfie", newFontSize, FontStyle.Bold);
        }

        private void BuyItem_Click(object sender, EventArgs e)
        {
            m_Panel.BuyItem(m_ItemType);
            UpdateLabel();
        }

        private void SellItem_Click(object sender, EventArgs e)
        {
            m_Panel.SellItem(m_ItemType);
            UpdateLabel();
        }
    }
}
