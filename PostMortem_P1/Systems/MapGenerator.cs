using System.Collections.Generic;
using System.Linq;

using RogueSharp;

using RSRectangle = RogueSharp.Rectangle;

using PostMortem_P1.Core;

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
    }
}
