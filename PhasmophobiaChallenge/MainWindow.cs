using PhasmophobiaChallenge.Panel;
//using PhasmophobiaChallenge.Panel.Discord;
using PhasmophobiaChallenge.Panel.RandomStuff;
using PhasmophobiaChallenge.Panel.Speedrun;
using PhasmophobiaChallenge.Panel.StoryMode;
using System.Collections.Generic;
using System.Windows.Forms;

namespace PhasmophobiaChallenge
{
    public partial class MainWindow : Form
    {
        private readonly DataFile m_Config;
        private readonly Translator m_Translator;
        private EPanelType m_CurrentPanelType = EPanelType.Invalid;
        private APhasmophobiaCompanionPanel m_CurrentPanel = null;
        private readonly Dictionary<EPanelType, APhasmophobiaCompanionPanel> m_Panels = new Dictionary<EPanelType, APhasmophobiaCompanionPanel>();

        public MainWindow()
        {
            m_Config = new DataFile();
            m_Translator = new Translator(m_Config.GetFragment(EPanelType.Option));
            InitializeComponent();
            InitializePanels();
            if (m_Panels.TryGetValue(EPanelType.TitleScreen, out APhasmophobiaCompanionPanel panel))
                SetCurrentPanel(panel, EPanelType.TitleScreen);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        public DataFragment GetDataFragment(EPanelType type) { return m_Config.GetFragment(type); }
        public Translator GetTranslator() { return m_Translator; }
        public IEnumerable<APhasmophobiaCompanionPanel> GetPanels() { return m_Panels.Values; }

        private void InitializePanels()
        {
            RegisterPanel(new TitleScreen(this));
            RegisterPanel(new OptionPanel(this));
            RegisterPanel(new StoryModePanel(this));
            RegisterPanel(new RandomStuffPanel(this));
            RegisterPanel(new SpeedrunPanel(this));
            //RegisterPanel(new DiscordPanel(this));
            RegisterPanel(new DummyPanel(this, EPanelType.Dummy1, EString.Dummy));
            RegisterPanel(new DummyPanel(this, EPanelType.Dummy2, EString.Dummy));
            RegisterPanel(new DummyPanel(this, EPanelType.Dummy3, EString.Dummy));
            RegisterPanel(new DummyPanel(this, EPanelType.Dummy4, EString.Dummy));
            RegisterPanel(new DummyPanel(this, EPanelType.Dummy5, EString.Dummy));
        }

        private void RegisterPanel(APhasmophobiaCompanionPanel panel)
        {
            EPanelType panelType = panel.GetPanelType();
            if (panelType != EPanelType.Count && panelType != EPanelType.Invalid)
                m_Panels[panelType] = panel;
        }

        private void SetCurrentPanel(APhasmophobiaCompanionPanel panel, EPanelType type)
        {
            if (m_CurrentPanel != null)
            {
                m_CurrentPanel.Close();
                PanelUIManager.Reset();
            }
            Controls.Clear();
            if (panel != null)
            {
                if (type != EPanelType.TitleScreen)
                    Text = "Pasmophobia Companion [" + m_Translator.GetString(panel.GetName()) + "]";
                else
                    Text = "Pasmophobia Companion";
                panel.Open();
                Controls.Add(panel.AsControl());
            }
            else
                Text = "Pasmophobia Companion";
            m_CurrentPanel = panel;
            m_CurrentPanelType = type;
        }

        public void UpdateTitle()
        {
            if (m_CurrentPanel != null && m_CurrentPanelType != EPanelType.TitleScreen)
                Text = m_Translator.GetString(EString.AppTitle) + " [" + m_Translator.GetString(m_CurrentPanel.GetName()) + "]";
            else
                Text = m_Translator.GetString(EString.AppTitle);
        }

        public void SetPanel(EPanelType page)
        {
            if (m_CurrentPanelType == page || page == EPanelType.Count || page == EPanelType.Invalid)
                return;
            else if (m_Panels.TryGetValue(page, out APhasmophobiaCompanionPanel panel))
                SetCurrentPanel(panel, page);
        }
    }
}
