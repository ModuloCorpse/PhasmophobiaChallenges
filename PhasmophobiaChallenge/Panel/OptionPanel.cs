using System;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel
{
    public partial class OptionPanel : APhasmophobiaCompanionPanel
    {
        public OptionPanel(MainWindow mainWindow): base(mainWindow, EPanelType.Option, "panel.option")
        {
            HideFromMenu();
            InitializeComponent();
            Local.Font = new Font(GetDefaultFontFamily(), 36f, FontStyle.Bold);
            BackButton.Font = new Font(GetDefaultFontFamily(), 36F, FontStyle.Bold, GraphicsUnit.Point, 0);

        }

        public override void OnOpen()
        {
            Translator translator = GetTranslator();
            Local.Text = translator.GetLocalName();
            translator.RegisterControl("other.back", BackButton);
            PanelUIManager.RegisterImageButton(BackButton, Properties.Resources.main_menu_panel_button_background, Properties.Resources.main_menu_panel_button_background_over, Properties.Resources.main_menu_panel_button_background_over);
            PanelUIManager.RegisterImageButton(PreviousLocal, Properties.Resources.left_ui_arrow_border, Properties.Resources.left_ui_arrow_border_over, Properties.Resources.left_ui_arrow_border_over);
            PanelUIManager.RegisterImageButton(NextLocal, Properties.Resources.right_ui_arrow_border, Properties.Resources.right_ui_arrow_border_over, Properties.Resources.right_ui_arrow_border_over);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            GetMainWindow().SetPanel(EPanelType.TitleScreen);
        }

        private void PreviousLocal_Click(object sender, EventArgs e)
        {
            Translator translator = GetTranslator();
            translator.PreviousLocal();
            Local.Text = translator.GetLocalName();
            GetMainWindow().UpdateTitle();
        }

        private void NextLocal_Click(object sender, EventArgs e)
        {
            Translator translator = GetTranslator();
            translator.NextLocal();
            Local.Text = translator.GetLocalName();
            GetMainWindow().UpdateTitle();
        }
    }
}
