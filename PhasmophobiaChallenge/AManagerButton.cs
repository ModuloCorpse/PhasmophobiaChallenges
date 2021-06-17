using System;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge
{
    abstract class AManagerButton
    {
        private bool m_Clicked = false;

        public AManagerButton(Button button)
        {
            button.MouseEnter += OnMouseEnter;
            button.MouseLeave += OnMouseLeave;
            button.MouseDown += OnMouseDown;
            button.MouseUp += OnMouseUp;

            button.FlatAppearance.BorderSize = 0;
            button.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 255, 255, 255);
            button.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 255, 255, 255);
            button.FlatAppearance.BorderColor = Color.FromArgb(0, 255, 255, 255);
            button.BackColor = Color.FromArgb(0, 255, 255, 255);
        }

        protected bool IsClicked() { return m_Clicked; }

        private void OnMouseDown(object sender, EventArgs e)
        {
            Button button = sender as Button;
            m_Clicked = true;
            OnButtonPress(button);
        }

        private void OnMouseUp(object sender, EventArgs e)
        {
            Button button = sender as Button;
            m_Clicked = false;
            OnButtonRelease(button);
        }

        private void OnMouseEnter(object sender, EventArgs e)
        {
            Button button = sender as Button;
            OnButtonOver(button);
        }

        private void OnMouseLeave(object sender, EventArgs e)
        {
            Button button = sender as Button;
            OnButtonUnover(button);
        }

        protected abstract void OnButtonPress(Button button);
        protected abstract void OnButtonRelease(Button button);
        protected abstract void OnButtonOver(Button button);
        protected abstract void OnButtonUnover(Button button);
    }
}
