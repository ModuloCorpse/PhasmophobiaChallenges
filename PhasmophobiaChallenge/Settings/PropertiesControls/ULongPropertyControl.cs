using System;
using System.Configuration;
using System.Windows.Forms;

namespace PhasmophobiaChallenge.Settings.PropertiesControls
{
    public class ULongPropertyControl : APropertyControl
    {
        public class Builder : PropertiesControlFactory.IBuilder
        {
            public APropertyControl Build(object value, string name) { return new ULongPropertyControl(value, name); }
        }

        private TextBox m_TextBox;

        public ULongPropertyControl(object value, string name) : base(value, name) {}

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
                Text = ((ulong)m_Property).ToString()
            };
            m_TextBox.Size = new System.Drawing.Size(300, m_TextBox.Size.Height);
            m_TextBox.KeyPress += HandleNumeric;
            panel.Controls.Add(nameLabel);
            panel.Controls.Add(m_TextBox);
            return panel;
        }

        private void HandleNumeric(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // If you want, you can allow decimal (float) numbers
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        protected override object ComputeValue()
        {
            return ulong.Parse(m_TextBox.Text);
        }
    }
}
