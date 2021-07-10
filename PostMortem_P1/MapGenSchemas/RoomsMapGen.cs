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
        }

        public override ChunkMap CreateMap(int width, int height)
        {
            var chunkMap = base.CreateMap(width, height);

            chunkMap.InitializeCells(Global.SpriteManager.Wall, false, false);

            _rooms = MapGenHelpers.GenerateNonIntersectingRects(Width, Height, _maxRooms, _roomMaxSize, _roomMinSize);

            foreach(RSRectangle room in _rooms)
            {
                MapGenHelpers.CreateRoom(chunkMap, room, Global.SpriteManager.Floor);
            }

            for (int r = 1; r < _rooms.Count; r++)
            {
                int previousRoomCenterX = _rooms[r - 1].Center.X;
                int previousRoomCenterY = _rooms[r - 1].Center.Y;
                int currentRoomCenterX = _rooms[r].Center.X;
                int currentRoomCenterY = _rooms[r].Center.Y;

                if (Global.Random.Next(1, 2) == 1)
                {
                    MapGenHelpers.CreateHorizontalTunnel(chunkMap, previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    MapGenHelpers.CreateVerticalTunnel(chunkMap, previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
                }
                else
                {
                    MapGenHelpers.CreateVerticalTunnel(chunkMap, previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    MapGenHelpers.CreateHorizontalTunnel(chunkMap, previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
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
    }
}
