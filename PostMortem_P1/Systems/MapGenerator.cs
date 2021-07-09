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
        private MapGenSchema _mapGenSchema;

        private int _width;
        private int _height;
        private int _enemiesNum;

        public MapGenerator(MapGenSchema mapGenSchema, int width, int height, int enemiesNum)
        {
            _mapGenSchema = mapGenSchema;
            _width = width;
            _height = height;
            _enemiesNum = enemiesNum;
        }

        public ChunkMap GenerateMap()
        {
            var chunkMap = _mapGenSchema.CreateMap(_width, _height);

            PlacePlayer(chunkMap);

            PlaceEnemies(chunkMap);

            return chunkMap;
        }

        
        private void PlacePlayer(ChunkMap chunkMap)
        {
            Player player = Global.Player;

            if (player == null)
            {
                player = new Player(_mapGenSchema.GetSuitablePlayerPosition(chunkMap)); ;
            }

            Debug.WriteLine($"Initial player location: x = {player.X} y = {player.Y}");

            chunkMap.AddPlayer(player);
        }

        private void PlaceEnemies(ChunkMap chunkMap)
        {
            var positionList = _mapGenSchema.GetSuitableEnemyPositionList(chunkMap, _enemiesNum);
            foreach (RSPoint position in positionList)
            {
                chunkMap.AddEnemy(Bandit.Create(position, 1));
            }

        }
    }
}
