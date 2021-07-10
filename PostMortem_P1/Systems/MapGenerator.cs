using System;
using System.Diagnostics;
using System.Linq;

using RogueSharp.DiceNotation;

using RSRectangle = RogueSharp.Rectangle;
using RSPoint = RogueSharp.Point;

using PostMortem_P1.Core;
using PostMortem_P1.Enemies;

namespace PostMortem_P1.Systems
{
    public class MapGenerator
    {
        public static ChunkMap GenerateMap(MapGenSchema mapGenSchema, int width, int height, int enemiesNum)
        {
            var chunkMap = mapGenSchema.CreateMap(width, height);

            chunkMap.PlaceEnemies(enemiesNum);

            return chunkMap;
        }

    }
}
