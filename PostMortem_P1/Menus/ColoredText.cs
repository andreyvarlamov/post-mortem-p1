using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace PostMortem_P1.Menus
{
    public class ColoredString
    {
        public string Text { get; set; }
        public Color Color { get; set; }

        public ColoredString(string text, Color color)
        {
            Text = text;
            Color = color;
        }

        public void DrawString(SpriteBatch spriteBatch, SpriteFont spriteFont, Vector2 position, float layerDepth)
        {
            spriteBatch.DrawString(spriteFont, this.Text, position, this.Color, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, layerDepth);
        }

        public static ColoredParagraph operator +(ColoredString a, ColoredString b)
        {
            List<ColoredString> coloredStrings = new List<ColoredString>();
            coloredStrings.Add(a);
            coloredStrings.Add(b);

            ColoredParagraph coloredParagraph = new ColoredParagraph(coloredStrings);

            return coloredParagraph;
        }

        public static ColoredParagraph operator +(ColoredString a, ColoredParagraph b)
        {
            b.ColoredStrings.Add(a);

            return b;
        }

        public static ColoredParagraph operator +(ColoredParagraph a, ColoredString b)
        {
            a.ColoredStrings.Add(b);

            return a;
        }
    }

    public class ColoredParagraph
    {
        public List<ColoredString> ColoredStrings { get; set; }

        public ColoredParagraph(List<ColoredString> coloredStrings)
        {
            ColoredStrings = coloredStrings;
        }

        public ColoredParagraph()
        {
            ColoredStrings = new List<ColoredString>();
        }

        public void DrawParagraph(SpriteBatch spriteBatch, SpriteFont spriteFont, Vector2 position, float layerDepth)
        {
            int currentWidth = 0;
            for (int i = 0; i < ColoredStrings.Count; i++)
            {
                Vector2 stringPosition = new Vector2(position.X + currentWidth, position.Y);
                ColoredStrings[i].DrawString(spriteBatch, spriteFont, stringPosition, layerDepth);
                currentWidth += (int)spriteFont.MeasureString(ColoredStrings[i].Text + " ").X;
            }
        }
    }
}
