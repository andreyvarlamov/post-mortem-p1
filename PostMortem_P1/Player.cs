using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

namespace PostMortem_P1
{
    public class Player : Figure
    {
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, new Vector2(X * Sprite.Width, Y * Sprite.Width), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Figures);
        }

        public bool TryMoveTo(int x, int y, IMap map)
        {
            if (map.IsWalkable(x, y))
            {
                var enemy = Global.CombatManager.EnemyAt(x, y);
                if (enemy == null)
                {
                    X = x;
                    Y = y;
                }
                else
                {
                    Global.CombatManager.Attack(this, enemy);
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HandleInput(InputState inputState, IMap map)
        {
            if (inputState.IsDirN())
            {
                return TryMoveTo(X, Y - 1, map);
            }
            else if (inputState.IsDirS())
            {
                return TryMoveTo(X, Y + 1, map);
            }
            else if (inputState.IsDirW())
            {
                return TryMoveTo(X - 1, Y, map);
            }
            else if (inputState.IsDirE())
            {
                return TryMoveTo(X + 1, Y, map);
            }
            else if (inputState.IsDirNW())
            {
                return TryMoveTo(X - 1, Y - 1, map);
            }
            else if (inputState.IsDirNE())
            {
                return TryMoveTo(X + 1, Y - 1, map);
            }
            else if (inputState.IsDirSW())
            {
                return TryMoveTo(X - 1, Y + 1, map);
            }
            else if (inputState.IsDirSE())
            {
                return TryMoveTo(X + 1, Y + 1, map);
            }
            else if (inputState.IsSkip())
            {
                return true;
            }

            return false;
        }
    }
}
