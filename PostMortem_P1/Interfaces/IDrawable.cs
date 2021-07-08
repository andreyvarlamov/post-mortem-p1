using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

namespace PostMortem_P1.Interfaces
{
    public interface IDrawable
    {
        int X { get; set; }
        int Y { get; set; }
        Texture2D Sprite { get; set; }

        void Draw(SpriteBatch spriteBatch);
    }
}
