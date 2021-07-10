using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Graphics;

using PostMortem_P1.Systems;

namespace PostMortem_P1.Core
{
    public class WorldMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public ChunkMap[,] _chunkMaps { get; private set; }
        public int PlayerPosX;
        public int PlayerPosY;

        public ChunkMap CurrentChunkMap
        {
            get
            {
                return _chunkMaps[PlayerPosX, PlayerPosY];
            }
            private set
            {

            }
        }

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

        public void SpawnPlayerInWorld(int xWorld, int yWorld)
        {
            PlayerPosX = xWorld;
            PlayerPosY = yWorld;

            CurrentChunkMap.PlacePlayer();
        }

        public bool SetPlayerWorldPosition(int xWorld, int yWorld)
        {
            if (xWorld >= 0 && xWorld < Width && yWorld >= 0 && yWorld < Height)
            {
                // Move player to the correct chunk position
                int xChunk = Global.Player.X;
                int yChunk =  Global.Player.Y;

                if (xWorld > PlayerPosX)
                {
                    // Moved east; came from west
                    xChunk = 0;
                }
                else if (xWorld < PlayerPosX)
                {
                    // Moved west; came from east
                    xChunk = this[xWorld, yWorld].Width - 1;
                }
                else if (yWorld > PlayerPosY)
                {
                    // Moved south; came from north
                    yChunk = 0;
                }
                else if (yWorld < PlayerPosY)
                {
                    // Moved north; came from south
                    yChunk = this[xWorld, yWorld].Height - 1;
                }

                if (Global.Player.CheckIfCanMoveTo(xChunk, yChunk, this[xWorld, yWorld]))
                {
                    ChunkMap prevChunkMap = CurrentChunkMap;

                    PlayerPosX = xWorld;
                    PlayerPosY = yWorld;

                    Debug.WriteLine($"SETTING WORLD POS: X = {Global.WorldMap.PlayerPosX}; Y = {Global.WorldMap.PlayerPosY}");

                    return Global.Player.SetPosition(xChunk, yChunk, prevChunkMap);
                }
            }

            return false;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentChunkMap.Draw(spriteBatch);
        }

        private void Update()
        {

        }
    }
}
