//using System.Collections.Generic;
//using System.Diagnostics;

//using RogueSharp.DiceNotation;

//namespace PostMortem_P1
//{
//    public class CombatManager
//    {
//        private readonly Player _player;
//        private readonly List<Enemy> _enemies;

//        public CombatManager(Player player, List<Enemy> enemies)
//        {
//            _player = player;
//            _enemies = enemies;
//        }

//        public void Attack(Figure attacker, Figure defender)
//        {
//        }

//        public Figure FigureAt(int x, int y)
//        {
//            if (IsPlayerAt(x, y))
//            {
//                return _player;
//            }
//            else
//            {
//                return EnemyAt(x, y);
//            }
//        }

//        public bool IsPlayerAt(int x, int y)
//        {
//            return (_player.X == x && _player.Y == y);
//        }

//        public Enemy EnemyAt(int x, int y)
//        {
//            foreach (var enemy in _enemies)
//            {
//                if (enemy.X == x && enemy.Y == y)
//                {
//                    return enemy;
//                }
//            }
//            return null;
//        }

//        public bool IsEnemyAt(int x, int y)
//        {
//            return EnemyAt(x, y) != null;
//        }
//    }
//}
