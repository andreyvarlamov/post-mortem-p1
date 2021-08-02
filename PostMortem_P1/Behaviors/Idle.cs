using System.Diagnostics;

using RogueSharp;

using PostMortem_P1.Core;
using PostMortem_P1.Interfaces;
using PostMortem_P1.Systems;

namespace PostMortem_P1.Behaviors
{
    public class Idle : IBehavior
    {
        public bool Act(NPC npc, CommandSystem commandSystem)
        {
            ChunkMap chunkMap = Global.WorldMap.CurrentChunkMap;

            int randomDir = Global.Random.Next(9) + 1;

            int moveToX = npc.X;
            int moveToY = npc.Y;
            switch ((Direction) randomDir)
            {
                case Direction.SW:
                    moveToX--;
                    moveToY++;
                    break;
                case Direction.S:
                    moveToY++;
                    break;
                case Direction.SE:
                    moveToX++;
                    moveToY++;
                    break;
                case Direction.W:
                    moveToX--;
                    break;
                case Direction.E:
                    moveToX++;
                    break;
                case Direction.NW:
                    moveToX--;
                    moveToY--;
                    break;
                case Direction.N:
                    moveToY--;
                    break;
                case Direction.NE:
                    moveToX++;
                    moveToY--;
                    break;
                default:
                    break;
            }

            if (moveToX < 0)
            {
                moveToX = 0;
            }
            else if (moveToX >= chunkMap.Width)
            {
                moveToX = chunkMap.Width - 1;
            }

            if (moveToY < 0)
            {
                moveToY = 0;
            }
            else if (moveToY >= chunkMap.Height)
            {
                moveToY = chunkMap.Height - 1;
            }

            commandSystem.MoveNPC(npc, chunkMap[moveToX, moveToY]);

            return true;
        }
    }
}
