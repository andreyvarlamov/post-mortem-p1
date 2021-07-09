using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;

using RogueSharp;
using RogueSharp.DiceNotation;

using RSRectangle = RogueSharp.Rectangle;
using RSPoint = RogueSharp.Point;

using PostMortem_P1.Core;

namespace PostMortem_P1.MapGenSchemas
{
    public class RoomsMapGen : MapGenSchema
    {
        private int _maxRooms;
        private int _roomMaxSize;
        private int _roomMinSize;

        private List<RSRectangle> _rooms;

        public RoomsMapGen(int maxRooms, int roomMaxSize, int roomMinSize) : base()
        {
            _maxRooms = maxRooms;
            _roomMaxSize = roomMaxSize;
            _roomMinSize = roomMinSize;
            _rooms = new List<RSRectangle>();
        }

        public override ChunkMap CreateMap(int width, int height)
        {
            var chunkMap = base.CreateMap(width, height);

            chunkMap.InitializeCells(Global.SpriteManager.Wall, false, false);

            for (int r = 0; r < _maxRooms; r++)
            {
                int roomWidth = Global.Random.Next(_roomMinSize, _roomMaxSize);
                int roomHeight = Global.Random.Next(_roomMinSize, _roomMaxSize);
                int roomXPosition = Global.Random.Next(0, Width - roomWidth - 1);
                int roomYPosition = Global.Random.Next(0, Height - roomHeight - 1);

                var newRoom = new RSRectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);

                bool newRoomIntersects = _rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects)
                {
                    _rooms.Add(newRoom);
                }
            }

            foreach(RSRectangle room in _rooms)
            {
                CreateRoom(chunkMap, room);
            }

            for (int r = 1; r < _rooms.Count; r++)
            {
                int previousRoomCenterX = _rooms[r - 1].Center.X;
                int previousRoomCenterY = _rooms[r - 1].Center.Y;
                int currentRoomCenterX = _rooms[r].Center.X;
                int currentRoomCenterY = _rooms[r].Center.Y;

                if (Global.Random.Next(1, 2) == 1)
                {
                    CreateHorizontalTunnel(chunkMap, previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    CreateVerticalTunnel(chunkMap, previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
                }
                else
                {
                    CreateVerticalTunnel(chunkMap, previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    CreateHorizontalTunnel(chunkMap, previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
                }
            }

            return chunkMap;
        }

        public override RSPoint GetSuitablePlayerPosition(ChunkMap chunkMap)
        {
            return new RSPoint(_rooms[0].Center.X, _rooms[0].Center.Y);
        }

        public override List<RSPoint> GetSuitableEnemyPositionList(ChunkMap chunkMap, int num)
        {
            var positionList = new List<RSPoint>();

            // TODO: what if no suitable locations to fit all
            while (positionList.Count < num)
            {
                foreach (var room in _rooms)
                {
                    if (Dice.Roll("1d10") > 8)
                    {
                        RSPoint? randomRoomLoc = chunkMap.GetRandomWalkableLocationInRect(room);
                        if (randomRoomLoc.HasValue)
                        {
                            positionList.Add(randomRoomLoc.Value);
                        }
                    }
                }
            }
            return positionList;
        }

        private void CreateRoom(ChunkMap chunkMap, RSRectangle room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    chunkMap.SetCellProperties(x, y, true, true);
                    chunkMap.GetCell(x, y).SetSprite(Global.SpriteManager.Floor);
                }
            }
        }

        private void CreateHorizontalTunnel(ChunkMap chunkMap, int xStart, int xEnd, int yPosition)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                chunkMap.SetCellProperties(x, yPosition, true, true);
                chunkMap.GetCell(x, yPosition).SetSprite(Global.SpriteManager.Floor);
            }
        }

        private void CreateVerticalTunnel(ChunkMap chunkMap, int yStart, int yEnd, int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                chunkMap.SetCellProperties(xPosition, y, true, true);
                chunkMap.GetCell(xPosition, y).SetSprite(Global.SpriteManager.Floor);
            }
        }
    }
}
