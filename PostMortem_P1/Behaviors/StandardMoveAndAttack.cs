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
            WorldCellMap worldCellMap = Global.WorldCellMap;
            Player player = Global.Player;
            FieldOfView enemyFov = new FieldOfView(worldCellMap);

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
                worldCellMap.SetIsWalkable(enemy.X, enemy.Y, true);
                worldCellMap.SetIsWalkable(player.X, player.Y, true);

                PathFinder pathFinder = new PathFinder(worldCellMap);
                Path path = null;

                try
                {
                    path = pathFinder.ShortestPath(
                        worldCellMap.GetCell(enemy.X, enemy.Y),
                        worldCellMap.GetCell(player.X, player.Y)
                    );
                }
                catch(PathNotFoundException)
                {
                    Debug.WriteLine($"{enemy.Name} waits for a turn");
                }

                // Don't forget to set the walkable status back to false
                worldCellMap.SetIsWalkable(enemy.X, enemy.Y, false);
                worldCellMap.SetIsWalkable(player.X, player.Y, false);

                if (path != null)
                {
                    try
                    {
                        commandSystem.MoveEnemy(enemy, path.StepForward() as Cell);
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
