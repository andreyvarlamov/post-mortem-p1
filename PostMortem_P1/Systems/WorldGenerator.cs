using System;
using System.Collections.Generic;
using System.Text;

using PostMortem_P1.Core;
using PostMortem_P1.MapGenSchemas;

namespace PostMortem_P1.Systems
{
    public class WorldGenerator
    {

        public static WorldMap GenerateWorld(int width, int height, Camera camera)
        {
            WorldMap worldMap = new WorldMap(width, height, camera);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    MapGenSchema mapGenSchema;
                    int npcsNum;

                    if ((x == 1 && y == 1) ||
                        (x == width - 2 && y == height - 2) ||
                        (x == 1 && y == height - 2) ||
                        (x == width - 2 && y == 1))
                    {
                        // Cities
                        mapGenSchema = new CityMapGen();
                        npcsNum = 20;
                    }
                    else if ((y == 1 && x != 0 && x != width - 1) ||
                        (y == height - 2 && x != 0 && x != width - 1))
                    {
                        // Horizontal roads
                        mapGenSchema = new RoadMapGen(true);
                        npcsNum = 5;
                    }
                    else if ((x == 1 && y != 0 && y != height - 1) ||
                        (x == width - 2 && y != 0 && y != height - 1))
                    {
                        // Vertical roads
                        mapGenSchema = new RoadMapGen(false);
                        npcsNum = 5;
                    }
                    else
                    {
                        mapGenSchema = new WildernessMapGen(5, 6, 4);
                        npcsNum = 2;
                    }
                    worldMap[x, y] = MapGenerator.GenerateMap(mapGenSchema, Global.MapWidth, Global.MapHeight, npcsNum);
                }
            }

            return worldMap;
        }
    }
}
