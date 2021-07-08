using System;
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
        private readonly int _width;
        private readonly int _height;
        private readonly int _maxRooms;
        private readonly int _roomMaxSize;
        private readonly int _roomMinSize;

        private readonly WorldCellMap _map;

        public MapGenerator(int width, int height, int maxRooms, int roomMaxSize, int roomMinSize)
        {
            _width = width;
            _height = height;
            _maxRooms = maxRooms;
            _roomMaxSize = roomMaxSize;
            _roomMinSize = roomMinSize;

            _map = new WorldCellMap();
        }

        public WorldCellMap CreateMap()
        {
            // Set the properties of all cells to false
            _map.Initialize(_width, _height);

            for (int r = 0; r < _maxRooms; r++)
            {
                int roomWidth = Global.Random.Next(_roomMinSize, _roomMaxSize);
                int roomHeight = Global.Random.Next(_roomMinSize, _roomMaxSize);
                int roomXPosition = Global.Random.Next(0, _width - roomWidth - 1);
                int roomYPosition = Global.Random.Next(0, _height - roomHeight - 1);

                var newRoom = new RSRectangle(roomXPosition, roomYPosition, roomWidth, roomHeight);

                bool newRoomIntersects = _map.Rooms.Any(room => newRoom.Intersects(room));

                if (!newRoomIntersects)
                {
                    _map.Rooms.Add(newRoom);
                }
            }

            foreach(RSRectangle room in _map.Rooms)
            {
                CreateRoom(room);
            }

            for (int r = 1; r < _map.Rooms.Count; r++)
            {
                int previousRoomCenterX = _map.Rooms[r - 1].Center.X;
                int previousRoomCenterY = _map.Rooms[r - 1].Center.Y;
                int currentRoomCenterX = _map.Rooms[r].Center.X;
                int currentRoomCenterY = _map.Rooms[r].Center.Y;

                if (Global.Random.Next(1, 2) == 1)
                {
                    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, previousRoomCenterY);
                    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, currentRoomCenterX);
                }
                else
                {
                    CreateVerticalTunnel(previousRoomCenterY, currentRoomCenterY, previousRoomCenterX);
                    CreateHorizontalTunnel(previousRoomCenterX, currentRoomCenterX, currentRoomCenterY);
                }
            }

            PlacePlayer();
            PlaceEnemies();

            return _map;
        }

        public void CreateRoom(RSRectangle room)
        {
            for (int x = room.Left + 1; x < room.Right; x++)
            {
                for (int y = room.Top + 1; y < room.Bottom; y++)
                {
                    _map.SetCellProperties(x, y, true, true, true);
                }
            }
        }

        private void CreateHorizontalTunnel(int xStart, int xEnd, int yPosition)
        {
            for (int x = Math.Min(xStart, xEnd); x <= Math.Max(xStart, xEnd); x++)
            {
                _map.SetCellProperties(x, yPosition, true, true);
            }
        }

        private void CreateVerticalTunnel(int yStart, int yEnd, int xPosition)
        {
            for (int y = Math.Min(yStart, yEnd); y <= Math.Max(yStart, yEnd); y++)
            {
                _map.SetCellProperties(xPosition, y, true, true);
            }
        }
        
        private void PlacePlayer()
        {
            Player player = Global.Player;

            if (player == null)
            {
                player = new Player(_map.Rooms[0].Center.X, _map.Rooms[0].Center.Y); ;
            }

            _map.AddPlayer(player);
        }

        private void PlaceEnemies()
        {
            foreach (var room in _map.Rooms)
            {
                // 60% chance of spawning an enemy in a room
                if (Dice.Roll("1d10") > 0)
                {
                    var enemyNum = Dice.Roll("1d4");
                    for (int i = 0; i < enemyNum; i++)
                    {
                        RSPoint? randomRoomLoc = _map.GetRandomWalkableLocationInRoom(room);

                        if (randomRoomLoc.HasValue)
                        {
                            var enemy = Bandit.Create(1, randomRoomLoc.Value.X, randomRoomLoc.Value.Y);

                            _map.AddEnemy(enemy);

                        }
                    }
                }
            }
        }
    }
}
