using System;
using System.Diagnostics;
using System.Linq;

using RogueSharp.DiceNotation;

using RSRectangle = RogueSharp.Rectangle;
using RSPoint = RogueSharp.Point;

using PostMortem_P1.Core;

namespace PostMortem_P1.Systems
{
    public class MapGenerator
    {
        public static ChunkMap GenerateMap(MapGenSchema mapGenSchema, int width, int height, int npcsNum)
        {
            var chunkMap = mapGenSchema.CreateMap(width, height);

            chunkMap.PlaceNPCs(npcsNum);

            return chunkMap;
        }

    }
}
