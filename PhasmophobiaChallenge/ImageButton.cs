using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge
{
    class ImageButton: AManagerButton
    {
        private readonly Image m_Image;
        private readonly Image m_OverImage;
        private readonly Image m_ClickedImage;
        private Image m_LastImage = Properties.Resources.red_arrow;

        public ImageButton(Button button, Image standard, Image over, Image clicked): base(button)
        {
            m_Image = standard;
            m_OverImage = over;
            m_ClickedImage = clicked;
        }

        override protected void OnButtonPress(Button button)
        {
            button.BackgroundImage = m_ClickedImage;
        }

        override protected void OnButtonRelease(Button button)
        {
            button.BackgroundImage = m_LastImage;
        }

        override protected void OnButtonOver(Button button)
        {
            if (!IsClicked())
                button.BackgroundImage = m_OverImage;
            m_LastImage = m_OverImage;
        }

        override protected void OnButtonUnover(Button button)
        {
            if (!IsClicked())
                button.BackgroundImage = m_Image;
            m_LastImage = m_Image;
        }
    }
}
