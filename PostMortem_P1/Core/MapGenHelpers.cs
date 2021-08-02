using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Graphics;

using RSRectangle = RogueSharp.Rectangle;

using Structure = System.Collections.Generic.HashSet<PostMortem_P1.Core.Tile>;

namespace PostMortem_P1.Core
{
    public class MapGenHelpers
    {
        public static List<RSRectangle> GenerateNonIntersectingRects(int mapWidth, int mapHeight, int maxRooms, int roomMaxSize, int roomMinSize)
        {
            List<RSRectangle> rooms = new List<RSRectangle>();

            for (int r = 0; r < maxRooms; r++)
            {
                int roomWidth = Global.Random.Next(roomMinSize, roomMaxSize);
                int roomHeight = Global.Random.Next(roomMinSize, roomMaxSize);
                int roomXPosition = Global.Random.Next(0, mapWidth - roomWidth - 1);
                int roomYPosition = Global.Random.Next(0, mapHeight - roomHeight - 1);

                var newRoom = new RSRectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);

                bool newRoomIntersects = rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects)
                {
                    rooms.Add(newRoom);
                }
            }

            return rooms;
        }
        /// <summary>
        /// Carve out a room
        /// </summary>
        public static void CreateRoom(ChunkMap chunkMap, RSRectangle room, Floor floor)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    chunkMap.RemoveBlockAndSetFloor(x, y, floor);
                }
            }
        }

        /// <summary>
        /// Carve out a tunnel
        /// </summary>
        public static void CreateHorizontalTunnel(ChunkMap chunkMap, int xStart, int xEnd, int yPosition)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                chunkMap.RemoveBlockAndSetFloor(x, yPosition, FloorType.Floor());
            }
        }

        /// <summary>
        /// Carve out a tunnel
        /// </summary>
        public static void CreateVerticalTunnel(ChunkMap chunkMap, int yStart, int yEnd, int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                chunkMap.RemoveBlockAndSetFloor(xPosition, y, FloorType.Floor());
            }
        }

        public static void CreateRoad(ChunkMap chunkMap, List<Structure> roads, Structure allRoadTiles, int position, int width, bool isHorizontal)
        {
            Structure road = new Structure();

            for (int i = 0; i < (isHorizontal ? chunkMap.Width : chunkMap.Height); i++)
            {
                if (isHorizontal)
                {
                    for (int y = position - width / 2; y < position + width / 2 + 1; y++)
                    {
                        var floor = chunkMap.RemoveBlockAndSetFloor(i, y, FloorType.Road());
                        road.Add(floor);
                        allRoadTiles.Add(floor);
                    }

                    if (!allRoadTiles.Any(tile => tile.X == i && tile.Y == position - width / 2 - 1))
                    {
                        chunkMap.RemoveBlockAndSetFloor(i, position - width / 2 - 1, FloorType.Sidewalk());
                    }

                    if (!allRoadTiles.Any(tile => tile.X == i && tile.Y == position + width / 2 + 1))
                    {
                        chunkMap.RemoveBlockAndSetFloor(i, position + width / 2 + 1, FloorType.Sidewalk());
                    }
                }
                else
                {
                    for (int x = position - width / 2; x < position + width / 2 + 1; x++)
                    {
                        var floor = chunkMap.RemoveBlockAndSetFloor(x, i, FloorType.Road());
                        road.Add(floor);
                        allRoadTiles.Add(floor);
                    }
                    
                    if (!allRoadTiles.Any(tile => tile.X == position - width / 2 - 1 && tile.Y == i))
                    {
                        chunkMap.RemoveBlockAndSetFloor(position - width / 2 - 1, i, FloorType.Sidewalk());
                    }

                    if (!allRoadTiles.Any(tile => tile.X == position + width / 2 + 1 && tile.Y == i))
                    {
                        chunkMap.RemoveBlockAndSetFloor(position + width / 2 + 1, i, FloorType.Sidewalk());
                    }
                }
            }

            roads.Add(road);
        }
    }
}
