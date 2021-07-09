using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using RogueSharp;

using RSPoint = RogueSharp.Point;

namespace PostMortem_P1.Core
{
    public class MapGenSchema
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public virtual ChunkMap CreateMap(int width, int height)
        {
            Width = width;
            Height = height;
            var chunkMap = new ChunkMap();
            chunkMap.Initialize(width, height);

            return chunkMap;
        }

        public virtual RSPoint GetSuitablePlayerPosition(ChunkMap chunkMap)
        {
            return new RSPoint(chunkMap.Width / 2, chunkMap.Height / 2);
        }

        public virtual List<RSPoint> GetSuitableEnemyPositionList(ChunkMap chunkMap, int num)
        {
            var positions = new List<RSPoint>();

            for (int i = 0; i < num; i++)
            {
                int x = Global.Random.Next(chunkMap.Width - 1);
                int y = Global.Random.Next(chunkMap.Height - 1);

                positions.Add(new RSPoint(x, y));
            }

            return positions;
        }
    }
}
