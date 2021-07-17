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

        private int _viewportWidth;
        private int _viewportHeight;

        public List<MenuItem> MenuItems { get; set; }

        public int Selection;

        private Texture2D _canvas;

        private bool _willDrawInMiddle;

        private GraphicsDeviceManager _graphics;

        public Menu(int pxWidth, int pxHeight, List<MenuItem> menuItems, GraphicsDeviceManager graphics)
        {
            _viewportWidth = graphics.GraphicsDevice.Viewport.Width;
            _viewportHeight = graphics.GraphicsDevice.Viewport.Height;

            PxWidth = pxWidth;
            PxHeight = pxHeight;

            PxX = _viewportWidth / 2 - PxWidth / 2;
            PxY = _viewportHeight / 2 - PxHeight / 2;

            Debug.WriteLine($"PxX={PxX}; PxY={PxY}");

            _graphics = graphics;

            MenuItems = menuItems;

            Selection = 2;

            _canvas = CreateStaticCanvas();

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawDecoration(spriteBatch);
            DrawContent(spriteBatch);
        }

        private void DrawContent(SpriteBatch spriteBatch)
        {
            int horizontalOffset = 25;
            int verticalOffset = 25;
            int lineHeight = 20;

            for (int i = 0; i < MenuItems.Count; i++)
            {
                Vector2 textSize = Global.FontManager.MainFont.MeasureString(MenuItems[i].Text);
                int textWidth = (int) textSize.X;
                int textHeight = (int) textSize.Y;

                Vector2 textPosition = new Vector2(PxX + horizontalOffset, PxY + verticalOffset + lineHeight * i - textHeight / 2);
                if (i == Selection)
                {

                    Vector2 cursorPosition = new Vector2(PxX + horizontalOffset - 3, PxY + verticalOffset + lineHeight * i - textHeight / 2 - 1);
                    var selectionRect = CreateRectangle(textWidth + 3, textHeight + 1, Color.White);
                    spriteBatch.Draw(selectionRect, cursorPosition, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.MenuSelect);
                    spriteBatch.DrawString(Global.FontManager.MainFont, MenuItems[i].Text, textPosition, Color.Black, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.MenuText);
                }
                else
                {
                    spriteBatch.DrawString(Global.FontManager.MainFont, MenuItems[i].Text, textPosition, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.MenuText);
                }
            }

        }

        private Texture2D CreateRectangle(int width, int height, Color color)
        {
            Texture2D rectangle = new Texture2D(_graphics.GraphicsDevice, width, height);

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = color;
            }

            rectangle.SetData(data);

            return rectangle;
        }

        private Texture2D CreateStaticCanvas()
        {
            Texture2D canvas = new Texture2D(_graphics.GraphicsDevice, PxWidth, PxHeight);

            Color[] data = new Color[PxWidth*PxHeight];

            int borderOffset = 8;
            int borderWidth = 1;

            for (int y = 0; y < PxHeight; y++)
            {
                for (int x = 0; x < PxWidth; x++)
                {
                    if (((x > borderOffset && x <= borderOffset + borderWidth) ||
                        (x > PxWidth - borderOffset - borderWidth - 1 && x <= PxWidth - borderOffset) ||
                        (y > borderOffset && y <= borderOffset + borderWidth) ||
                        (y > PxHeight - borderOffset - borderWidth - 1 && y <= PxHeight - borderOffset)) &&
                        (x > borderOffset && x <= PxWidth - borderOffset && y > borderOffset && y <= PxHeight - borderOffset))
                    {
                        data[y * PxWidth + x] = Color.White;
                    }
                    else
                    {
                        data[y * PxWidth + x] = Color.Black;
                    }
                }
            }

            canvas.SetData(data);

            return canvas;

        }

        private void DrawDecoration(SpriteBatch spriteBatch)
        {
            Vector2 position = new Vector2(PxX, PxY);
            //spriteBatch.Draw(_canvas, position, Color.White);
            spriteBatch.Draw(_canvas, position, null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Menu);
        }

        //public void Update(GameTime gameTime)
        //{

        //}
    }
}
