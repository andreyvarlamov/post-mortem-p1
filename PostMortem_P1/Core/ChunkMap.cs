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
    public class ChunkMap
    {
        private readonly List<Enemy> _enemies;

        private FieldOfView<Tile> _playerFov;

        private MapGenSchema _mapGenSchema;

        public SchedulingSystem SchedulingSystem;

        public LayerMap Floors { get; private set; }
        public LayerMap Blocks { get; private set; }

        public int Width { get; set; }
        public int Height { get; set; }

        public ChunkMap(MapGenSchema mapGenSchema)
        {
            _enemies = new List<Enemy>();

            _mapGenSchema = mapGenSchema;
            SchedulingSystem = new SchedulingSystem();

            Floors = new LayerMap(LayerDepth.Floors);
            Blocks = new LayerMap(LayerDepth.Blocks);

            _playerFov = new FieldOfView<Tile>(Blocks);
        }

        public void Draw(SpriteBatch spriteBatch)
        {

            Floors.Draw(spriteBatch, _playerFov);
            Blocks.Draw(spriteBatch, _playerFov);
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

        public void Initialize(int width, int height)
        {
            Width = width;
            Height = height;

            Floors.Initialize(width, height);
            Blocks.Initialize(width, height);
        }

        public void InitializeCells(Texture2D floorSprite, Texture2D blockSprite, bool isWalkable)
        {
            // Initialize all Floor tiles
            Floors.InitializeTiles(floorSprite, true, true);

            // If not walkable, initialize all Block tiles as well
            if (!isWalkable)
            {
                Blocks.InitializeTiles(blockSprite, false, false);
            }
            else
            {
                Blocks.InitializeAsAir();
            }
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
            foreach (Tile tile in Floors.GetAllCells())
            {
                if (_playerFov.IsInFov(tile.X, tile.Y))
                {
                    tile.SetExplored(true);
                    Blocks[tile.X, tile.Y].SetExplored(true);
                }

            }
        }

        public Tile SetBlock(int x, int y, Texture2D sprite, bool isTransparent, bool isWalkable)
        {
            Tile block = Blocks[x, y];
            block.IsTransparent = isTransparent;
            block.IsWalkable = isWalkable;
            block.SetAir(false);
            block.SetSprite(sprite);
            return block;
        }

        public Tile RemoveBlockAndSetFloor(int x, int y, Texture2D sprite, bool isTransparent, bool isWalkable)
        {
            Blocks[x, y].SetAir(true);
            Tile floor = Floors[x, y];
            floor.IsTransparent = isTransparent;
            floor.IsWalkable = isWalkable;
            floor.SetSprite(sprite);
            return floor;
        }

        public bool IsWalkable(int x, int y)
        {
            return Floors.IsWalkable(x, y) && Blocks.IsWalkable(x, y);
        }

        public bool IsExplored(int x, int y)
        {
            return Floors[x, y].IsExplored;
        }

        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Tile tile = Blocks[x, y];
            Blocks.SetCellProperties(tile.X, tile.Y, tile.IsTransparent, isWalkable);
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

                    if (IsWalkable(x, y))
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
                    if (IsWalkable(x + rect.X, y + rect.Y))
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
