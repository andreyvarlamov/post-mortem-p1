using System;
using System.Collections.Generic;
using System.Text;

using PostMortem_P1.Core;
using PostMortem_P1.MapGenSchemas;

namespace PostMortem_P1.Systems
{
    public class WorldGenerator
    {
        public static WorldMap GenerateWorld(int width, int height, int xPlayer, int yPlayer)
        {
            WorldMap worldMap = GenerateChunks(width, height);

            worldMap.SpawnPlayerInWorld(xPlayer, yPlayer);

            return worldMap;
        }

        private static WorldMap GenerateChunks(int width, int height)
        {
            WorldMap worldMap = new WorldMap(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    MapGenSchema mapGenSchema;
                    int enemiesNum;

                    if ((x == 1 && y == 1) ||
                        (x == width - 2 && y == height - 2) ||
                        (x == 1 && y == height - 2) ||
                        (x == width - 2 && y == 1))
                    {
                        // Cities
                        mapGenSchema = new CityMapGen();
                        enemiesNum = 20;
                    }
                    else if ((y == 1 && x != 0 && x != width - 1) ||
                        (y == height - 2 && x != 0 && x != width - 1))
                    {
                        // Horizontal roads
                        mapGenSchema = new RoadMapGen(true);
                        enemiesNum = 5;
                    }
                    else if ((x == 1 && y != 0 && y != height - 1) ||
                        (x == width - 2 && y != 0 && y != height - 1))
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
                    worldMap[x, y] = MapGenerator.GenerateMap(mapGenSchema, Global.MapWidth, Global.MapHeight, enemiesNum);
                }
            }

            return worldMap;
        }
    }
}
