using System;
using System.Collections.Generic;
using System.Text;

using PostMortem_P1.Core;

namespace PostMortem_P1.Systems
{
    public class CommandSystem
    {
        public bool MovePlayer(eDirection direction)
        {
            int x = Global.Player.X;
            int y = Global.Player.Y;

            switch (direction)
            {
                case eDirection.SW:
                    x--;
                    y++;
                    break;
                case eDirection.S:
                    y++;
                    break;
                case eDirection.SE:
                    x++;
                    y++;
                    break;
                case eDirection.W:
                    x--;
                    break;
                case eDirection.Center:
                    return true;
                case eDirection.E:
                    x++;
                    break;
                case eDirection.NW:
                    x--;
                    y--;
                    break;
                case eDirection.N:
                    y--;
                    break;
                case eDirection.NE:
                    x++;
                    y--;
                    break;
                default:
                    return false;
            }

            if (Global.Player.SetPosition(x, y))
            {
                return true;
            }

            return false;
        }
    }
}
