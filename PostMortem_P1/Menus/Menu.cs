using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PostMortem_P1.Core;

namespace PostMortem_P1.Menus
{
    public class Menu
    {
        public int PxX { get; set; }
        public int PxY { get; set; }
        public int PxWidth { get; set; }
        public int PxHeight { get; set; }

        public List<MenuItem> MenuItems { get; set; }

        private Texture2D _canvas;

        private bool _willDrawInMiddle;

        public Menu(int pxWidth, int pxHeight, List<MenuItem> menuItems, GraphicsDeviceManager graphics)
        {
            PxWidth = pxWidth;
            PxHeight = pxHeight;

            PxX = (graphics.GraphicsDevice.Viewport.Width / 2) - ( pxWidth / 2);
            PxY = (graphics.GraphicsDevice.Viewport.Height / 2) - ( pxHeight / 2);

            Debug.WriteLine($"PxX={PxX}; PxY={PxY}");

            MenuItems = menuItems;

            _canvas = new Texture2D(graphics.GraphicsDevice, PxWidth, PxHeight);

            Color[] data = new Color[PxWidth*PxHeight];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = Color.Red;
            }

            _canvas.SetData(data);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawDecoration(spriteBatch);
            DrawContent(spriteBatch);
        }

        private void DrawContent(SpriteBatch spriteBatch)
        {

        }

        private void DrawDecoration(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(PxY, PxY);
            //spriteBatch.Draw(_canvas, position, Color.White);
            spriteBatch.Draw(_canvas, position, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Menu);
        }

        //public void Update(GameTime gameTime)
        //{

        //}
    }
}
