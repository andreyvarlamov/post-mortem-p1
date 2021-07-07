using System.Collections.Generic;
using System.Diagnostics;

using RogueSharp.DiceNotation;

namespace PostMortem_P1
{
    public class CombatManager
    {
        private readonly Player _player;
        private readonly List<Enemy> _enemies;

        public CombatManager(Player player, List<Enemy> enemies)
        {
            _player = player;
            _enemies = enemies;
        }

        public void Attack(Figure attacker, Figure defender)
        {
            if (Dice.Roll("d20") + attacker.AttackBonus >= defender.ArmorClass)
            {
                int damage = attacker.Damage.Roll().Value;
                defender.Health -= damage;

                Debug.WriteLine($"{attacker.Name} hit {defender.Name} for {damage} and he has {defender.Health} remaining.");

                if (defender.Health <= 0)
                {
                    if (defender is Enemy)
                    {
                        _enemies.Remove(defender as Enemy);
                    }

                    Debug.WriteLine($"{attacker.Name} killed {defender.Name}.");
                }
            }
            else
            {
                Debug.WriteLine($"{attacker.Name} missed {defender.Name}.");
            }
        }

        public Figure FigureAt(int x, int y)
        {
            if (IsPlayerAt(x, y))
            {
                return _player;
            }
            else
            {
                return EnemyAt(x, y);
            }
        }

        public bool IsPlayerAt(int x, int y)
        {
            return (_player.X == x && _player.Y == y);
        }

        public Enemy EnemyAt(int x, int y)
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.X == x && enemy.Y == y)
                {
                    return enemy;
                }
            }
            return null;
        }

        public bool IsEnemyAt(int x, int y)
        {
            return EnemyAt(x, y) != null;
        }
    }
}
