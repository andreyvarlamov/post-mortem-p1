using System.Diagnostics;
using System.Collections.Generic;

using RogueSharp.DiceNotation;

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

            Enemy enemy = Global.WorldCellMap.GetEnemyAt(x, y);
            if (enemy != null)
            {
                Attack(Global.Player, enemy);
                return true;
            }

            return false;
        }

        public void Attack(Actor attacker, Actor defender)
        {
            if (Dice.Roll("1d20") + attacker.AttackBonus >= defender.ArmorClass)
            {
                int damage = attacker.Damage.Roll().Value;
                defender.Health -= damage;

                Debug.WriteLine($"{attacker.Name} hit {defender.Name} for {damage} and he has {defender.Health} remaining.");

                if (defender.Health <= 0)
                {
                    if (defender is Enemy)
                    {
                        Global.WorldCellMap.RemoveEnemy(defender as Enemy);
                    }

                    Debug.WriteLine($"{attacker.Name} killed {defender.Name}.");
                }
            }
            else
            {
                Debug.WriteLine($"{attacker.Name} missed {defender.Name}.");
            }
        }
    }
}
