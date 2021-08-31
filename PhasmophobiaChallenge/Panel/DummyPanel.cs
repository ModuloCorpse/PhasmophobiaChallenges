using System;

namespace PhasmophobiaChallenge.Panel
{
    public partial class DummyPanel : APhasmophobiaCompanionPanel
    {
        public DummyPanel(MainWindow mainWindow, EPanelType type, string name): base(mainWindow, type, name)
        {
            InitializeComponent();
        }

        public override void OnOpen()
        {
            PanelUIManager.RegisterImageButton(BackButton, Properties.Resources.red_arrow, Properties.Resources.red_arrow_over, Properties.Resources.red_arrow_clicked);
        }

        private void BackButton_Click(object sender, EventArgs e)
        {
            GetMainWindow().SetPanel(EPanelType.TitleScreen);
        }
    }
}
