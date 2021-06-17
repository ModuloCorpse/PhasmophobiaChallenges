using System;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge
{
    public class APhasmophobiaCompanionPanel : UserControl
    {
        private bool m_ShowInMenu = true;
        private EPanelType m_Type = EPanelType.Invalid;
        private EString m_Name = EString.Invalid;
        private readonly MainWindow m_MainWindow;
        private readonly Translator m_Translator;
        private readonly DataFragment m_Data;
        private readonly FontFamily m_FontFamily;

        [Obsolete("Designer only", true)]
        public APhasmophobiaCompanionPanel()
        {
            m_MainWindow = null;
            m_Translator = null;
            m_Data = null;
            m_FontFamily = null;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        protected APhasmophobiaCompanionPanel(MainWindow mainWindow, EPanelType type, EString name)
        {
            m_Type = type;
            m_Name = name;
            m_MainWindow = mainWindow;
            m_Translator = mainWindow.GetTranslator();
            m_Data = mainWindow.GetDataFragment(type);
            m_FontFamily = mainWindow.GetFontFamily();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        protected MainWindow GetMainWindow() { return m_MainWindow; }
        public Translator GetTranslator() { return m_Translator; }
        protected DataFragment GetData() { return m_Data; }
        public FontFamily GetFontFamily() { return m_FontFamily; }

        public bool ShowInMenu() { return m_ShowInMenu; }
        public EPanelType GetPanelType() { return m_Type; }
        public EString GetName() { return m_Name; }
        public void HideFromMenu() { m_ShowInMenu = false; }
        public Control AsControl() { return this; }

        public void Open()
        {
            OnOpen();
        }

        public void Close()
        {
            m_Translator.UnregisterControls();
            PanelUIManager.Reset();
            OnClose();
        }

        public void Initialize()
        {
            OnInitialize();
        }

        public virtual void OnOpen() {}
        public virtual void OnClose() { }
        public virtual void OnInitialize() {}
    }
}
