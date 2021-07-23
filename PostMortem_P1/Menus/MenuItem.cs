using System;
using System.Collections.Generic;
using System.Text;

namespace PostMortem_P1.Menus
{
    public class MenuItem
    {
        public string Text { get; set; }
        public MenuAction MenuAction { get; set; }

        public MenuItem(string text, MenuAction menuAction)
        {
            Text = text;
            MenuAction = menuAction;
        }
    }
}
