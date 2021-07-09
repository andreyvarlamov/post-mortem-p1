using System;
using System.Collections.Generic;
using System.Text;

using PostMortem_P1.Core;
using PostMortem_P1.MapGenSchemas;

namespace PostMortem_P1.Systems
{
    public class WorldGenerator
    {
        private int _width;
        private int _height;
        private int _xPlayer;
        private int _yPlayer;

        private MapGenerator[,] _mapGenerators;

        public WorldGenerator(int width, int height, int xPlayer, int yPlayer)
        {
            _width = width;
            _height = height;
            _xPlayer = xPlayer;
            _yPlayer = yPlayer;

            _mapGenerators = new MapGenerator[width, height];
        }

        public WorldMap GenerateWorld()
        {
            WorldMap worldMap = new WorldMap(_width, _height);

            GenerateChunks(worldMap);

            _mapGenerators[_xPlayer, _yPlayer].PlacePlayer(worldMap[_xPlayer, _yPlayer]);
            worldMap.SetPlayerWorldPosition(_xPlayer, _yPlayer);

            return worldMap;
        }

        private void GenerateChunks(WorldMap worldMap)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int y = 0; y < _height; y++)
                {
                    MapGenSchema mapGenSchema;
                    int enemiesNum;

                    if ((x == 1 && y == 1) ||
                        (x == _width - 2 && y == _height - 2) ||
                        (x == 1 && y == _height - 2) ||
                        (x == _width - 2 && y == 1))
                    {
                        // Cities
                        mapGenSchema = new CityMapGen();
                        enemiesNum = 20;
                    }
                    else if ((y == 1 && x != 0 && x != _width - 1) ||
                        (y == _height - 2 && x != 0 && x != _width - 1))
                    {
                        // Horizontal roads
                        mapGenSchema = new RoadMapGen(true);
                        enemiesNum = 5;
                    }
                    else if ((x == 1 && y != 0 && y != _height - 1) ||
                        (x == _width - 2 && y != 0 && y != _height - 1))
                    {
                        // Vertical roads
                        mapGenSchema = new RoadMapGen(false);
                        enemiesNum = 5;
                    }
                    else
                    {
                        mapGenSchema = new WildernessMapGen(5, 6, 4);
                        enemiesNum = 2;
                    }
                    enemiesNum = 0;
                    MapGenerator mapGenerator = new MapGenerator(mapGenSchema, Global.MapWidth, Global.MapHeight, enemiesNum);
                    worldMap[x, y] = CreateChunk(mapGenerator);
                    _mapGenerators[x, y] = mapGenerator;
                }
            }
        }

        private ChunkMap CreateChunk(MapGenerator mapGen)
        {
            return mapGen.GenerateMap();
        }
    }
}
