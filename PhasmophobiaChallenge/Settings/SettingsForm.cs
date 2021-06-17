using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Settings
{
    public partial class SettingsForm : Form
    {
        private DataFragment m_Configuration;
        private List<APropertyControl> m_PropertiesControls = new List<APropertyControl>();

        public delegate void SettingFormEvent();

        public event SettingFormEvent OnValidateForm;
        public event SettingFormEvent OnCancelForm;

        public SettingsForm(DataFragment configuration) : this(configuration, new PropertiesControlFactory()) { }

        public SettingsForm(DataFragment configuration, PropertiesControlFactory factory)
        {
            InitializeComponent();
            m_Configuration = configuration;
            foreach (string property in configuration.GetKeys())
            {
                APropertyControl propertiesControl = factory.Build(property, m_Configuration.Get<object>(property));
                if (propertiesControl != null)
                {
                    m_PropertiesControls.Add(propertiesControl);
                    propertiesControl.Init();
                    FormFlowLayoutPanel.Controls.Add(propertiesControl.GetControl());
                }
            }
        }

        private void OkButton_Click(object sender, EventArgs e)
        {
            foreach (APropertyControl propertyControl in m_PropertiesControls)
                propertyControl.Save(m_Configuration);
            m_Configuration.Save();
            Close();
            if (OnValidateForm != null)
                OnValidateForm.Invoke();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Close();
            if (OnCancelForm != null)
                OnCancelForm.Invoke();
        }
    }
}
