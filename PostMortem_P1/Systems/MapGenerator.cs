using System;
using System.Collections.Generic;
using System.Text;

using RogueSharp;

using PostMortem_P1.Core;

namespace PostMortem_P1.Systems
{
    public class MapGenerator
    {
        private readonly int _width;
        private readonly int _height;
        private readonly WorldCellMap _map;

        public MapGenerator(int width, int height)
        {
            _width = width;
            _height = height;
            _map = new WorldCellMap();
        }

        public WorldCellMap CreateMap()
        {
            // Initialize every cell in the map by setitng walkable, transparency and explored to true
            _map.Initialize(_width, _height);
            foreach(Cell cell in _map.GetAllCells())
            {
                _map.SetCellProperties(cell.X, cell.Y, true, true, true);
            }

            // Set the first and last rows in the map to not be transparent or walkable
            foreach (Cell cell in _map.GetCellsInRows(0, _height - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            // Set the first and last columns int he map to not be transparent or walkable
            foreach (Cell cell in _map.GetCellsInColumns(0, _width - 1))
            {
                _map.SetCellProperties(cell.X, cell.Y, false, false, true);
            }

            return _map;
        }
    }
}
