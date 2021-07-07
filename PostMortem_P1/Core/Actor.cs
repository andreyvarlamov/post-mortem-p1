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
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Global.WorldCellMap.GetCell(X, Y).IsExplored && !Global.Debugging)
            {
                return;
            }

            if (Global.WorldCellMap.IsInFov(X, Y) || Global.Debugging)
            {
                spriteBatch.Draw(Sprite, new Vector2(X * Sprite.Width, Y * Sprite.Width), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Actors);
            }
        }
        #endregion

        public bool SetPosition(int x, int y)
        {
            if (Global.WorldCellMap.GetCell(x, y).IsWalkable)
            {
                // Set the previous cell to walkable
                Global.WorldCellMap.SetIsWalkable(X, Y, true);

                X = x;
                Y = y;

                // Set the current cell to not walkable
                Global.WorldCellMap.SetIsWalkable(X, Y, false);

                if (this is Player)
                {
                    Global.WorldCellMap.UpdatePlayerFieldOfView();
                }

                return true;
            }

            return false;
        }
    }
}
