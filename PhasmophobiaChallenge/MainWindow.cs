using PhasmophobiaChallenge.Panel;
//using PhasmophobiaChallenge.Panel.Discord;
using PhasmophobiaChallenge.Panel.RandomStuff;
using PhasmophobiaChallenge.Panel.Evidences;
using PhasmophobiaChallenge.Panel.StoryMode;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace PhasmophobiaChallenge
{
    public partial class MainWindow : Form
    {
        private readonly int m_DefaultFont;
        private readonly DataFile m_Config;
        private readonly Translator m_Translator;
        private EPanelType m_CurrentPanelType = EPanelType.Invalid;
        private APhasmophobiaCompanionPanel m_CurrentPanel = null;
        private readonly Dictionary<EPanelType, APhasmophobiaCompanionPanel> m_Panels = new Dictionary<EPanelType, APhasmophobiaCompanionPanel>();
        private readonly PrivateFontCollection m_PrivateFontCollection = new PrivateFontCollection();

        public MainWindow()
        {
            m_DefaultFont = LoadFont(Properties.Resources.Yahfie);
            m_Config = new DataFile();
            m_Translator = new Translator(m_Config.GetFragment(EPanelType.Option));
            InitializeComponent();
            InitializePanels();
            if (m_Panels.TryGetValue(EPanelType.TitleScreen, out APhasmophobiaCompanionPanel panel))
                SetCurrentPanel(panel, EPanelType.TitleScreen);
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        private int LoadFont(byte[] fontData)
        {
            IntPtr data = Marshal.AllocCoTaskMem(fontData.Length);
            Marshal.Copy(fontData, 0, data, fontData.Length);
            m_PrivateFontCollection.AddMemoryFont(data, fontData.Length);
            return m_PrivateFontCollection.Families.Length - 1;
        }

        public DataFragment GetDataFragment(EPanelType type) { return m_Config.GetFragment(type); }
        public Translator GetTranslator() { return m_Translator; }
        public FontFamily GetDefaultFontFamily() { return m_PrivateFontCollection.Families[m_DefaultFont]; }
        public IEnumerable<APhasmophobiaCompanionPanel> GetPanels() { return m_Panels.Values; }

        private void InitializePanels()
        {
            RegisterPanel(new TitleScreen(this));
            RegisterPanel(new OptionPanel(this));
            RegisterPanel(new StoryModePanel(this));
            RegisterPanel(new RandomStuffPanel(this));
            RegisterPanel(new EvidencesPanel(this));
            //RegisterPanel(new DiscordPanel(this));
            //RegisterPanel(new DummyPanel(this, EPanelType.Dummy1, "panel.dummy"));
            //RegisterPanel(new DummyPanel(this, EPanelType.Dummy2, "panel.dummy"));
            //RegisterPanel(new DummyPanel(this, EPanelType.Dummy3, "panel.dummy"));
            //RegisterPanel(new DummyPanel(this, EPanelType.Dummy4, "panel.dummy"));
            //RegisterPanel(new DummyPanel(this, EPanelType.Dummy5, "panel.dummy"));
        }

        private void RegisterPanel(APhasmophobiaCompanionPanel panel)
        {
            panel.Initialize();
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
                panel.Open();
                Controls.Add(panel.AsControl());
            }
            m_CurrentPanel = panel;
            m_CurrentPanelType = type;
            UpdateTitle();
        }

        public void UpdateTitle()
        {
            if (m_CurrentPanel != null && m_CurrentPanelType != EPanelType.TitleScreen)
                Text = m_Translator.GetString("other.apptitle") + " [" + m_Translator.GetString(m_CurrentPanel.GetName()) + "]";
            else
                Text = m_Translator.GetString("other.apptitle");
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
