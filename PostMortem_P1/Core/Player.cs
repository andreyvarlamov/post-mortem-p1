using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

namespace PostMortem_P1.Core
{
    public class Player : Actor
    {
        public Player(Texture2D sprite, int x, int y)
        {
            Awareness = 15;
            Name = "Player";
            Sprite = sprite;
            X = x;
            Y = y;
        }

        public bool TryMoveTo(int x, int y, IMap map)
        {
            if (map.IsWalkable(x, y))
            {
                X = x;
                Y = y;
                //var enemy = Global.CombatManager.EnemyAt(x, y);
                //if (enemy == null)
                //{
                //    X = x;
                //    Y = y;
                //}
                //else
                //{
                //    Global.CombatManager.Attack(this, enemy);
                //}
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
