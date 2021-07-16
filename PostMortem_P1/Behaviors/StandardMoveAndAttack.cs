using System.Diagnostics;

using RogueSharp;

using PostMortem_P1.Core;
using PostMortem_P1.Interfaces;
using PostMortem_P1.Systems;

namespace PostMortem_P1.Behaviors
{
    public class StandardMoveAndAttack : IBehavior
    {
        public bool Act(Enemy enemy, CommandSystem commandSystem)
        {
            ChunkMap chunkMap = Global.WorldMap.CurrentChunkMap;
            Player player = Global.WorldMap.Player;
            FieldOfView<Tile> enemyFov = new FieldOfView<Tile>(chunkMap);

            if (!enemy.TurnsAlerted.HasValue)
            {
                enemyFov.ComputeFov(enemy.X, enemy.Y, enemy.Awareness, true);
                if (enemyFov.IsInFov(player.X, player.Y))
                {
                    Debug.WriteLine($"{enemy.Name} noticed {player.Name}");
                    enemy.TurnsAlerted = 1;
                }
            }

            if (enemy.TurnsAlerted.HasValue)
            {
                // Before we find a path, make sure to make the monster and player cells walkable
                chunkMap.SetIsWalkable(enemy.X, enemy.Y, true);
                chunkMap.SetIsWalkable(player.X, player.Y, true);

                PathFinder<Tile> pathFinder = new PathFinder<Tile>(chunkMap);
                Path path = null;

                try
                {
                    path = pathFinder.ShortestPath(
                        chunkMap[enemy.X, enemy.Y],
                        chunkMap[player.X, player.Y]
                    );
                }
                catch(PathNotFoundException)
                {
                    Debug.WriteLine($"{enemy.Name} waits for a turn");
                }

                // Don't forget to set the walkable status back to false
                chunkMap.SetIsWalkable(enemy.X, enemy.Y, false);
                chunkMap.SetIsWalkable(player.X, player.Y, false);

                if (path != null)
                {
                    try
                    {
                        commandSystem.MoveEnemy(enemy, path.StepForward() as Tile);
                    }
                    catch (NoMoreStepsException)
                    {
                        Debug.WriteLine($"{enemy.Name} NoMoreStepsException");
                    }
                }

                enemy.TurnsAlerted++;

                if (enemy.TurnsAlerted > 15)
                {
                    enemy.TurnsAlerted = null;
                }
            }

            return true;
        }
    }
}
