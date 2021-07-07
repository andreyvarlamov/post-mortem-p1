using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

using PostMortem_P1.Interfaces;

using IDrawable = PostMortem_P1.Interfaces.IDrawable;

namespace PostMortem_P1.Core
{
    public class Actor : IActor, IDrawable
    {
        #region IActor
        public string Name { get; set; }
        public int Awareness { get; set; }
        #endregion

        #region IDrawable
        public int X { get; set; }
        public int Y { get; set; }
        public Texture2D Sprite { get; set; }
        public void Draw(SpriteBatch spriteBatch, IMap map)
        {
            if (!map.GetCell(X, Y).IsExplored && !Global.Debugging)
            {
                return;
            }

            if (map.IsInFov(X, Y) || Global.Debugging)
            {
                spriteBatch.Draw(Sprite, new Vector2(X * Sprite.Width, Y * Sprite.Width), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Actors);
            }
        }
        #endregion
    }
}
