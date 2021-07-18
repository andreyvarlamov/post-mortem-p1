using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PostMortem_P1.Core;

namespace PostMortem_P1.Menus.Overlays
{
    public class Hud : Overlay
    {
        public Hud(GraphicsDeviceManager graphics, int widthPercent, bool isRightSide) : base(graphics)
        {
            PxWidth = (int)(this.viewportWidth * (widthPercent / 100.0f));
            PxHeight = this.viewportHeight;

            if (isRightSide)
            {
                PxX = this.viewportWidth - PxWidth;
            }

            this.canvas = CreateRectangle(PxWidth, PxHeight, Color.Black, this.graphics);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {

            Vector2 position = new Vector2(PxX, PxY);
            spriteBatch.Draw(this.canvas, position, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Hud);
        }
    }
}
