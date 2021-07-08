﻿using System;
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

        public MapGenerator(MapGenSchema mapGenSchema, int width, int height)
        {
            _mapGenSchema = mapGenSchema;
            _width = width;
            _height = height;
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
                player = new Player(_mapGenSchema.GetSuitablePlayerPosition()); ;
            }

            Debug.WriteLine($"Initial player location: x = {player.X} y = {player.Y}");

            chunkMap.AddPlayer(player);
        }

        private void PlaceEnemies(ChunkMap chunkMap)
        {
            var positionList = _mapGenSchema.GetSuitableEnemyPositionList(chunkMap, 10);
            foreach (RSPoint position in positionList)
            {
                chunkMap.AddEnemy(Bandit.Create(position, 1));
            }

        }
    }
}
