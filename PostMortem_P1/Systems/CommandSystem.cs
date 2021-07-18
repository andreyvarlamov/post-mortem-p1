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
        public void Update()
        {
            if (IsPlayerTurn)
            {
                if (MovePlayer(Global.InputManager.IsMove()))
                {
                    EndPlayerTurn();
                }
            }
            else
            {
                ActivateEnemies(Global.WorldMap.CurrentChunkMap.SchedulingSystem);
            }
        }

        public bool IsPlayerTurn { get; set; }

        public void EndPlayerTurn()
        {
            IsPlayerTurn = false;
        }

        /// <summary>
        /// processing stops here until an input is made
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public bool MovePlayer(Direction direction)
        {
            int x = Global.WorldMap.Player.X;
            int y = Global.WorldMap.Player.Y;

            switch (direction)
            {
                case Direction.SW:
                    x--;
                    y++;
                    break;
                case Direction.S:
                    y++;
                    break;
                case Direction.SE:
                    x++;
                    y++;
                    break;
                case Direction.W:
                    x--;
                    break;
                case Direction.Center:
                    return true;
                case Direction.E:
                    x++;
                    break;
                case Direction.NW:
                    x--;
                    y--;
                    break;
                case Direction.N:
                    y--;
                    break;
                case Direction.NE:
                    x++;
                    y--;
                    break;
                default:
                    return false;
            }

            if (Global.WorldMap.Player.SetPosition(x, y, null))
            {
                return true;
            }

            Enemy enemy = Global.WorldMap.CurrentChunkMap.GetEnemyAt(x, y);
            if (enemy != null)
            {
                Attack(Global.WorldMap.Player, enemy);
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

        public void ActivateEnemies(SchedulingSystem schedulingSystem)
        {
            IScheduleable scheduleable = schedulingSystem.Get();

            if (scheduleable is Player)
            {
                IsPlayerTurn = true;
                schedulingSystem.Add(Global.WorldMap.Player);
            }
            else
            {
                if (scheduleable is Enemy enemy)
                {
                    enemy.PerformAction(this);
                    schedulingSystem.Add(enemy);
                }

                ActivateEnemies(schedulingSystem);
            }
        }

        public void MoveEnemy(Enemy enemy, Tile tile)
        {
            if (!enemy.SetPosition(tile.X, tile.Y, null))
            {
                if (Global.WorldMap.Player.X == tile.X && Global.WorldMap.Player.Y == tile.Y)
                {
                    Attack(enemy, Global.WorldMap.Player);
                }
            }
        }
    }
}
