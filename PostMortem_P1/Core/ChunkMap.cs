using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

using RSRectangle = RogueSharp.Rectangle;
using RSPoint = RogueSharp.Point;

using PostMortem_P1.Enemies;
using PostMortem_P1.Graphics;
using PostMortem_P1.Systems;

namespace PostMortem_P1.Core
{
    public class ChunkMap : Map<Tile>
    {
        private readonly List<Enemy> _enemies;

        private FieldOfView<Tile> _playerFov;

        private MapGenSchema _mapGenSchema;

        public SchedulingSystem SchedulingSystem;

        public ChunkMap(MapGenSchema mapGenSchema)
        {
            _enemies = new List<Enemy>();

            _mapGenSchema = mapGenSchema;
            SchedulingSystem = new SchedulingSystem();

            _playerFov = new FieldOfView<Tile>(this);
        }

        public void InitializeDefaultTiles(Texture2D floorSprite, Block block, bool isBlocks)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (isBlocks)
                    {
                        if (block == null)
                        {
                            throw new Exception("No block isntance has been passed, while isBlocks=true.");
                        }
                        this[x, y] = new Tile(x, y, block);

                    }
                    else
                    {
                        if (floorSprite == null)
                        {
                            throw new Exception("No floor sprite has been passed, while isBlocks=false.");
                        }
                        this[x, y] = new Tile(x, y, floorSprite);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in GetAllCells())
            {
                // If not explored yet, don't render
                if (!tile.IsExplored && !Global.Debugging)
                {
                    continue;
                }


                // If explored, but not in fov - gray tint, if in fov - no tint
                Color tint = Color.Gray;
                if (_playerFov.IsInFov(tile.X, tile.Y) || Global.Debugging)
                {
                    tint = Color.White;
                }

                var position = new Vector2(tile.X * SpriteManager.SpriteSize, tile.Y * SpriteManager.SpriteSize);

                spriteBatch.Draw(tile.Floor, position, null, tint, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Floors);

                if (!tile.IsAir)
                {
                    spriteBatch.Draw(tile.Block.Sprite, position, null, tint, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Blocks);
                }
            }

            foreach (Enemy enemy in _enemies)
            {
                enemy.Draw(spriteBatch);
            }
        }

        public void Update()
        {
            //_enemies.ForEach(enemy =>
            //{
            //    enemy.Update();
            //});
        }

               public bool IsInPlayerFov(int x, int y)
        {
            return _playerFov.IsInFov(x, y);
        }

        public void UpdatePlayerFieldOfView()
        {
            Player player = Global.WorldMap.Player;
            _playerFov.ComputeFov(player.X, player.Y, player.Awareness, true);

            // Setting tiles in fov to explored (that information is stored in floors layer)
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (_playerFov.IsInFov(x, y))
                    {
                        SetExplored(x, y, true);
                    }
                }
            }
        }

        public void SetExplored(int x, int y, bool isExplored)
        {
            this[x, y].SetExplored(isExplored);
        }

        public Tile SetBlock(int x, int y, Block block)
        {
            Tile tile = this[x, y];
            tile.SetBlock(block);
            return tile;
        }

        public Tile RemoveBlockAndSetFloor(int x, int y, Texture2D floorSprite)
        {

            Tile tile = this[x, y];
            tile.SetBlock(BlockType.Air());
            tile.SetFloor(floorSprite);
            return tile;
        }

        public bool IsExplored(int x, int y)
        {
            return this[x, y].IsExplored;
        }

        public bool IsTileWalkable(int x, int y)
        {
            return this[x, y].IsTileWalkable;
        }

        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Tile tile = this[x, y];
            tile.IsTileWalkable = isWalkable;
        }

        public void SetMapForPlayer(Player player)
        {
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();
            AddActorToSchedulingSystem(player);
        }

        public void AddActorToSchedulingSystem(Actor actor)
        {
            SchedulingSystem.Add(actor);
        }

        public void RemoveActorFromSchedulingSystem(Actor actor)
        {
            SchedulingSystem.Remove(actor);
        }

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
            SetIsWalkable(enemy.X, enemy.Y, false);
            AddActorToSchedulingSystem(enemy);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
            SetIsWalkable(enemy.X, enemy.Y, true);
            RemoveActorFromSchedulingSystem(enemy);
        }

        public Enemy GetEnemyAt(int x, int y)
        {
            return _enemies.FirstOrDefault(enemy => enemy.X == x && enemy.Y == y);
        }

        public RSPoint? GetRandomWalkableLocationInRect(RSRectangle rect)
        {
            if (DoesRectHaveWalkableSpace(rect))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = Global.Random.Next(1, rect.Width - 2) + rect.X;
                    int y = Global.Random.Next(1, rect.Height - 2) + rect.Y;

                    if (IsTileWalkable(x, y))
                    {
                        return new RSPoint(x, y);
                    }
                }
            }

            return null;
        }

        private bool DoesRectHaveWalkableSpace(RSRectangle rect)
        {
            for (int x = 1; x <= rect.Width - 2; x++)
            {
                for (int y = 1; y <= rect.Height - 2; y++)
                {
                    if (IsTileWalkable(x + rect.X, y + rect.Y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public RSPoint GetSuitablePlayerPosition()
        {
            return _mapGenSchema.GetSuitablePlayerPosition(this);
        }

        public void PlaceEnemies(int enemiesNum)
        {
            var positionList = _mapGenSchema.GetSuitableEnemyPositionList(this, enemiesNum);
            foreach (RSPoint position in positionList)
            {
                AddEnemy(Bandit.Create(position, 1));
            }

        }
    }
}
