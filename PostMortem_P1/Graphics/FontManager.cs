using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PostMortem_P1.Graphics
{
    public class FontManager
    {
        public SpriteFont MainFont { get; private set; }

        public void LoadContent(ContentManager content)
        {
            MainFont = content.Load<SpriteFont>("mainFont");
        }
    }
}
