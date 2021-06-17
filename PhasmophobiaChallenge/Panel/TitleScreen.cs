using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Panel
{
    public partial class TitleScreen : APhasmophobiaCompanionPanel
    {
        private static string ms_Version = "1.0";
        private int m_Idx = 0;
        private List<APhasmophobiaCompanionPanel> m_Panels = new List<APhasmophobiaCompanionPanel>();
        private readonly Dictionary<Button, EPanelType> m_ButtonToType = new Dictionary<Button, EPanelType>();

        public TitleScreen(MainWindow mainWindow): base(mainWindow, EPanelType.TitleScreen, EString.TitleScreen)
        {
            HideFromMenu();
            InitializeComponent();
            PanelButton1.TextChanged += new EventHandler(OnScreenTextChanged);
            PanelButton1.Click += new EventHandler(OnScreenButtonClicked);
            PanelButton2.TextChanged += new EventHandler(OnScreenTextChanged);
            PanelButton2.Click += new EventHandler(OnScreenButtonClicked);
            PanelButton3.TextChanged += new EventHandler(OnScreenTextChanged);
            PanelButton3.Click += new EventHandler(OnScreenButtonClicked);
            PanelButton4.TextChanged += new EventHandler(OnScreenTextChanged);
            PanelButton4.Click += new EventHandler(OnScreenButtonClicked);
            PanelButton5.TextChanged += new EventHandler(OnScreenTextChanged);
            PanelButton5.Click += new EventHandler(OnScreenButtonClicked);
        }

        public override void OnOpen()
        {
            VersionLabel.Text = string.Format("{0}: {1}", GetTranslator().GetString(EString.Version), ms_Version);
            IEnumerable<APhasmophobiaCompanionPanel> panels = GetMainWindow().GetPanels();
            foreach (APhasmophobiaCompanionPanel panel in panels)
            {
                if (panel.ShowInMenu())
                    m_Panels.Add(panel);
            }
            PanelUIManager.RegisterImageButton(PanelButton1, Properties.Resources.main_menu_panel_button_background, Properties.Resources.main_menu_panel_button_background_over, Properties.Resources.main_menu_panel_button_background_over);
            PanelUIManager.RegisterImageButton(PanelButton2, Properties.Resources.main_menu_panel_button_background, Properties.Resources.main_menu_panel_button_background_over, Properties.Resources.main_menu_panel_button_background_over);
            PanelUIManager.RegisterImageButton(PanelButton3, Properties.Resources.main_menu_panel_button_background, Properties.Resources.main_menu_panel_button_background_over, Properties.Resources.main_menu_panel_button_background_over);
            PanelUIManager.RegisterImageButton(PanelButton4, Properties.Resources.main_menu_panel_button_background, Properties.Resources.main_menu_panel_button_background_over, Properties.Resources.main_menu_panel_button_background_over);
            PanelUIManager.RegisterImageButton(PanelButton5, Properties.Resources.main_menu_panel_button_background, Properties.Resources.main_menu_panel_button_background_over, Properties.Resources.main_menu_panel_button_background_over);
            PanelUIManager.RegisterImageButton(Previous, Properties.Resources.left_ui_arrow_border, Properties.Resources.left_ui_arrow_border_over, Properties.Resources.left_ui_arrow_border_over);
            PanelUIManager.RegisterImageButton(Next, Properties.Resources.right_ui_arrow_border, Properties.Resources.right_ui_arrow_border_over, Properties.Resources.right_ui_arrow_border_over);
            PanelUIManager.RegisterImageButton(Options, Properties.Resources.main_menu_panel_button_background, Properties.Resources.main_menu_panel_button_background_over, Properties.Resources.main_menu_panel_button_background_over);
            PanelUIManager.RegisterImageButton(Exit, Properties.Resources.main_menu_panel_button_background, Properties.Resources.main_menu_panel_button_background_over, Properties.Resources.main_menu_panel_button_background_over);
            GetTranslator().RegisterControl(EString.Exit, Exit);
            UpdateButtons();
        }

        private void UpdateButtons()
        {
            m_ButtonToType.Clear();
            UpdateButtonWith(PanelButton1, (m_Idx * 5));
            UpdateButtonWith(PanelButton2, (m_Idx * 5) + 1);
            UpdateButtonWith(PanelButton3, (m_Idx * 5) + 2);
            UpdateButtonWith(PanelButton4, (m_Idx * 5) + 3);
            UpdateButtonWith(PanelButton5, (m_Idx * 5) + 4);
        }

        private void UpdateButtonWith(Button button, int idx)
        {
            if (idx < m_Panels.Count)
            {
                APhasmophobiaCompanionPanel panel = m_Panels[idx];
                button.Enabled = true;
                button.Visible = true;
                button.Text = GetTranslator().GetString(panel.GetName());
                m_ButtonToType[button] = panel.GetPanelType();
            }
            else
            {
                button.Enabled = false;
                button.Visible = false;
            }
        }

        private void NextIdx()
        {
            ++m_Idx;
            if ((m_Idx * 5) > (m_Panels.Count - 1))
                m_Idx = 0;
            UpdateButtons();
        }

        private void PreviousIdx()
        {
            if (m_Idx == 0)
            {
                int nbPanel = m_Panels.Count - 1;
                if (nbPanel < 0)
                    m_Idx = 0;
                else
                {
                    while (nbPanel % 5 != 0)
                        --nbPanel;
                    m_Idx = nbPanel / 5;
                }
            }
            else
                --m_Idx;
            UpdateButtons();
        }

        public override void OnClose()
        {
            m_Idx = 0;
            m_Panels.Clear();
            m_ButtonToType.Clear();
        }

        public void OnScreenTextChanged(object sender, EventArgs args)
        {
            if (sender is Button button)
            {
                SizeF stringSize = TextRenderer.MeasureText(button.Text, button.Font);
                float newFontSize = button.Font.Size * Math.Min(button.Size.Height / stringSize.Height, (button.Size.Width - 12) / stringSize.Width);
                if (newFontSize < 36)
                    button.Font = new Font(GetFontFamily(), newFontSize, FontStyle.Bold);
                else
                    button.Font = new Font(GetFontFamily(), 36, FontStyle.Bold);
            }
        }

        public void OnScreenButtonClicked(object sender, EventArgs args)
        {
            if (sender is Button button)
            {
                if (m_ButtonToType.TryGetValue(button, out EPanelType type))
                    GetMainWindow().SetPanel(type);
            }
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            PreviousIdx();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            NextIdx();
        }

        private void Options_Click(object sender, EventArgs e)
        {
            GetMainWindow().SetPanel(EPanelType.Option);
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            GetMainWindow().Close();
        }
    }
}
