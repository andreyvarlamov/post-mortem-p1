using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework.Graphics;

using PostMortem_P1.Input;
using PostMortem_P1.Systems;
using PostMortem_P1.Menus;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Core
{
    public class WorldMap
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Player Player { get; set; }
        public int PlayerWorldPosX;
        public int PlayerWorldPosY;

        public CommandSystem CommandSystem { get; set; }
        public Camera Camera;

        private ChunkMap[,] _chunkMaps;

        public ChunkMap CurrentChunkMap
        {
            get
            {
                return _chunkMaps[PlayerWorldPosX, PlayerWorldPosY];
            }
            private set
            {
            }
        }

        public WorldMap(int width, int height, Camera camera)
        {
            Width = width;
            Height = height;
            Camera = camera;

            _chunkMaps = new ChunkMap[Width, Height];

            CommandSystem = new CommandSystem();
        }

        public ChunkMap this[int x, int y]
        {
            get => _chunkMaps[x, y];
            set => _chunkMaps[x, y] = value;
        }

        public void SpawnPlayerInWorld(int xWorld, int yWorld)
        {
            PlayerWorldPosX = xWorld;
            PlayerWorldPosY = yWorld;

            if (Player == null)
            {
                Player = new Player(CurrentChunkMap.GetSuitablePlayerPosition());
            }

            Camera.CenterOn(CurrentChunkMap[Player.X, Player.Y]);

            Global.Hud.SetChunkX(Player.X);
            Global.Hud.SetChunkY(Player.Y);

            Global.Hud.SetWorldX(PlayerWorldPosX);
            Global.Hud.SetWorldY(PlayerWorldPosY);

            Global.Hud.SetMaxHealth(Player.MaxHealth);
            Global.Hud.SetHealth(Player.Health);

            Debug.WriteLine($"Initial player location: /n" +
                $"WORLD: x = {PlayerWorldPosX} y = {PlayerWorldPosY}" +
                $"CHUNK: x = {Player.X} y = {Player.Y}");

            CurrentChunkMap.SetMapForPlayer(Player);
        }

        public bool SetPlayerWorldPosition(int xWorld, int yWorld)
        {
            if (xWorld >= 0 && xWorld < Width && yWorld >= 0 && yWorld < Height)
            {
                // Move player to the correct chunk position
                int xChunk = Global.WorldMap.Player.X;
                int yChunk =  Global.WorldMap.Player.Y;

                if (xWorld > PlayerWorldPosX)
                {
                    // Moved east; came from west
                    xChunk = 0;
                }
                else if (xWorld < PlayerWorldPosX)
                {
                    // Moved west; came from east
                    xChunk = this[xWorld, yWorld].Width - 1;
                }
                else if (yWorld > PlayerWorldPosY)
                {
                    // Moved south; came from north
                    yChunk = 0;
                }
                else if (yWorld < PlayerWorldPosY)
                {
                    // Moved north; came from south
                    yChunk = this[xWorld, yWorld].Height - 1;
                }

                if (Global.WorldMap.Player.CheckIfCanMoveTo(xChunk, yChunk, this[xWorld, yWorld]))
                {
                    ChunkMap prevChunkMap = CurrentChunkMap;

                    PlayerWorldPosX = xWorld;
                    PlayerWorldPosY = yWorld;

                    prevChunkMap.RemoveActorFromSchedulingSystem(Player);
                    CurrentChunkMap.AddActorToSchedulingSystem(Player);
                    return Global.WorldMap.Player.SetPosition(xChunk, yChunk, prevChunkMap);
                }
            }

            return false;
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            CurrentChunkMap.Draw(spriteBatch);
            Player.Draw(spriteBatch);
        }

        public void Update()
        {
            CurrentChunkMap.Update();
            CommandSystem.Update();
        }
    }
}
