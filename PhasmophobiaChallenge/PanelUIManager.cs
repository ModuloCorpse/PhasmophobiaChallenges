using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PhasmophobiaChallenge
{
    static class PanelUIManager
    {
        private static readonly Dictionary<Button, AManagerButton> ms_Buttons = new Dictionary<Button, AManagerButton>();

        public static void Reset()
        {
            ms_Buttons.Clear();
        }

        public static void RegisterImageButton(Button button, Image standard, Image over, Image clicked)
        {
            ms_Buttons[button] = new ImageButton(button, standard, over, clicked);
        }

        public static void RegisterTextButton(Button button, Color standard, Color over, Color clicked)
        {
            ms_Buttons[button] = new TextButton(button, standard, over, clicked);
        }
    }
}
