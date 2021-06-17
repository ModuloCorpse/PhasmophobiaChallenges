using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge
{
    class TextButton: AManagerButton
    {
        private readonly Color m_Color = Color.White;
        private readonly Color m_OverColor = Color.Red;
        private readonly Color m_ClickedColor = Color.DarkRed;
        private Color m_LastColor = Color.White;

        public TextButton(Button button, Color standard, Color over, Color clicked): base(button)
        {
            m_Color = standard;
            m_OverColor = over;
            m_ClickedColor = clicked;
        }

        override protected void OnButtonPress(Button button)
        {
            button.ForeColor = m_ClickedColor;
        }

        override protected void OnButtonRelease(Button button)
        {
            button.ForeColor = m_LastColor;
        }

        override protected void OnButtonOver(Button button)
        {
            if (!IsClicked())
                button.ForeColor = m_OverColor;
            m_LastColor = m_OverColor;
        }

        override protected void OnButtonUnover(Button button)
        {
            if (!IsClicked())
                button.ForeColor = m_Color;
            m_LastColor = m_Color;
        }
    }
}
