using System;
using System.Collections.Generic;
using System.Text;

namespace PostMortem_P1.Core
{
    public class WorldMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public ChunkMap[,] _chunkMaps { get; private set; }
        public int PlayerX { get; private set; }
        public int PlayerY { get; private set; }

        public WorldMap(int width, int height)
        {
            Width = width;
            Height = height;

            _chunkMaps = new ChunkMap[Width, Height];
        }

        public ChunkMap this[int x, int y]
        {
            get => _chunkMaps[x, y];
            set => _chunkMaps[x, y] = value;
        }

        public void SetPlayerWorldPosition(int x, int y)
        {
            PlayerX = x;
            PlayerY = y;
        }
    }
}
