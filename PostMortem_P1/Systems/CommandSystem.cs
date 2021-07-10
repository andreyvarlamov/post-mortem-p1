using System.Diagnostics;
using System.Collections.Generic;

using RogueSharp;
using RogueSharp.DiceNotation;

using PostMortem_P1.Core;
using PostMortem_P1.Interfaces;

namespace PostMortem_P1.Systems
{
    public class CommandSystem
    {
        public bool IsPlayerTurn { get; set; }

        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }

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

            if (Global.Player.SetPosition(x, y, null))
            {
                return true;
            }

            Enemy enemy = Global.WorldMap.CurrentChunkMap.GetEnemyAt(x, y);
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
                        Global.WorldMap.CurrentChunkMap.RemoveEnemy(defender as Enemy);
                    }

                    Debug.WriteLine($"{attacker.Name} killed {defender.Name}.");
                }
            }
            else
            {
                Debug.WriteLine($"{attacker.Name} missed {defender.Name}.");
            }
        }

        public void ActivateEnemies()
        {
            IScheduleable scheduleable = Global.SchedulingSystem.Get();

            if (scheduleable is Player)
            {
                IsPlayerTurn = true;
                Global.SchedulingSystem.Add(Global.Player);
            }
            else
            {
                if (scheduleable is Enemy enemy)
                {
                    enemy.PerformAction(this);
                    Global.SchedulingSystem.Add(enemy);
                }

                ActivateEnemies();
            }
        }

        public void MoveEnemy(Enemy enemy, Cell cell)
        {
            if (!enemy.SetPosition(cell.X, cell.Y, null))
            {
                if (Global.Player.X == cell.X && Global.Player.Y == cell.Y)
                {
                    Attack(enemy, Global.Player);
                }
            }
        }
    }
}
