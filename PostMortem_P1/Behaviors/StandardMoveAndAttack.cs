using System.Diagnostics;

using RogueSharp;

using PostMortem_P1.Core;
using PostMortem_P1.Interfaces;
using PostMortem_P1.Systems;

namespace PostMortem_P1.Behaviors
{
    public class StandardMoveAndAttack : IBehavior
    {
        public bool Act(NPC npc, CommandSystem commandSystem)
        {
            ChunkMap chunkMap = Global.WorldMap.CurrentChunkMap;
            Player player = Global.WorldMap.Player;
            FieldOfView<Tile> npcFov = new FieldOfView<Tile>(chunkMap);

            if (!npc.TurnsAlerted.HasValue)
            {
                npcFov.ComputeFov(npc.X, npc.Y, npc.Awareness, true);
                if (npcFov.IsInFov(player.X, player.Y))
                {
                    Debug.WriteLine($"{npc.Name} noticed {player.Name}");
                    npc.TurnsAlerted = 1;
                }
            }

            if (npc.TurnsAlerted.HasValue)
            {
                // Before we find a path, make sure to make the monster and player cells walkable
                chunkMap.SetIsWalkable(npc.X, npc.Y, true);
                chunkMap.SetIsWalkable(player.X, player.Y, true);

                PathFinder<Tile> pathFinder = new PathFinder<Tile>(chunkMap);
                Path path = null;

                try
                {
                    path = pathFinder.ShortestPath(
                        chunkMap[npc.X, npc.Y],
                        chunkMap[player.X, player.Y]
                    );
                }
                catch(PathNotFoundException)
                {
                    Debug.WriteLine($"{npc.Name} waits for a turn");
                }

                // Don't forget to set the walkable status back to false
                chunkMap.SetIsWalkable(npc.X, npc.Y, false);
                chunkMap.SetIsWalkable(player.X, player.Y, false);

                if (path != null)
                {
                    try
                    {
                        commandSystem.MoveNPC(npc, path.StepForward() as Tile);
                    }
                    catch (NoMoreStepsException)
                    {
                        Debug.WriteLine($"{npc.Name} NoMoreStepsException");
                    }
                }

                npc.TurnsAlerted++;

                if (npc.TurnsAlerted > 15)
                {
                    npc.TurnsAlerted = null;
                }
            }

            return true;
        }
    }
}
