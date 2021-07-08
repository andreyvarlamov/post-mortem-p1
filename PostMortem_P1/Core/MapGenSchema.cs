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

        public virtual RSPoint GetSuitablePlayerPosition()
        {
            return RSPoint.Zero;
        }

        public virtual List<RSPoint> GetSuitableEnemyPositionList(ChunkMap chunkMap, int num)
        {
            return new List<RSPoint>();
        }
    }
}
