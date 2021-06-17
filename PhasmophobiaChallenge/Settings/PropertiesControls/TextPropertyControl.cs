using System.Windows.Forms;

namespace PhasmophobiaChallenge.Settings.PropertiesControls
{
    public class TextPropertyControl : APropertyControl
    {
        public class Builder : PropertiesControlFactory.IBuilder
        {
            public APropertyControl Build(object value, string name) { return new TextPropertyControl(value, name); }
        }

        private TextBox m_TextBox;

        public TextPropertyControl(object value, string name) : base(value, name) {}

        protected override Control GenerateControl()
        {
            TableLayoutPanel panel = new TableLayoutPanel
            {
                ColumnCount = 2,
                RowCount = 1,
                AutoSize = true
            };
            Label nameLabel = new Label
            {
                Text = m_Name,
                TextAlign = System.Drawing.ContentAlignment.MiddleRight
            };
            nameLabel.Size = new System.Drawing.Size(125, nameLabel.Size.Height);
            m_TextBox = new TextBox
            {
                Text = m_Property as string
            };
            m_TextBox.Size = new System.Drawing.Size(300, m_TextBox.Size.Height);
            panel.Controls.Add(nameLabel);
            panel.Controls.Add(m_TextBox);
            return panel;
        }

        protected override object ComputeValue()
        {
            return m_TextBox.Text;
        }
    }
}
