using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

namespace PostMortem_P1.Menus
{
    public class MenuAction
    {
        protected GraphicsDeviceManager graphics;

        public MenuAction(GraphicsDeviceManager graphics)
        {
            this.graphics = graphics;
        }

        public virtual bool Do()
        {
            return false;
        }
    }
}
