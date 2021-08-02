using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using PostMortem_P1.Input;

namespace PostMortem_P1.Menus
{
    public class Overlay
    {
        public int PxX { get; set; }
        public int PxY { get; set; }
        public int PxWidth { get; set; }
        public int PxHeight { get; set; }

        protected GraphicsDeviceManager graphics;

        protected Texture2D canvas;

        protected int viewportWidth;
        protected int viewportHeight;

        public Overlay(GraphicsDeviceManager graphics)
        {
            viewportWidth = graphics.GraphicsDevice.Viewport.Width;
            viewportHeight = graphics.GraphicsDevice.Viewport.Height;

            PxX = 0;
            PxY = 0;
            PxWidth = viewportWidth;
            PxHeight = viewportHeight;

            this.graphics = graphics;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }

        public virtual void DrawThroughWorldCamera(SpriteBatch spriteBatch)
        {
        }

        public virtual void ProcessFromInput(InputManager inputManager)
        {
        }

        public static Texture2D CreateRectangle(int width, int height, Color color, GraphicsDeviceManager graphics)
        {
            Texture2D rectangle = new Texture2D(graphics.GraphicsDevice, width, height);

            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = color;
            }

            rectangle.SetData(data);

            return rectangle;
        }

        protected Texture2D CreateStaticCanvas()
        {
            Texture2D canvas = new Texture2D(this.graphics.GraphicsDevice, PxWidth, PxHeight);

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
    }
}
