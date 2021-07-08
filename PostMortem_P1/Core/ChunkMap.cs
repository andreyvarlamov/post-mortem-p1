using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

using RSRectangle = RogueSharp.Rectangle;
using RSPoint = RogueSharp.Point;

using PostMortem_P1.Graphics;

namespace PostMortem_P1.Core
{
    public class ChunkMap : Map<Tile>
    {
        public List<RSRectangle> Rooms;
        private readonly List<Enemy> _enemies;

        private FieldOfView<Tile> _playerFov;

        public ChunkMap()
        {
            Rooms = new List<RSRectangle>();
            _enemies = new List<Enemy>();
            _playerFov = new FieldOfView<Tile>(this);
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
                if (IsInPlayerFov(tile.X, tile.Y) || Global.Debugging)
                {
                    tint = Color.White;
                }

                var position = new Vector2(tile.X * SpriteManager.SpriteSize, tile.Y * SpriteManager.SpriteSize);

                spriteBatch.Draw(tile.Sprite, position, null, tint, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Cells);

                foreach (Enemy enemy in _enemies)
                {
                    enemy.Draw(spriteBatch);
                }
            }
        }

        public override void Initialize(int width, int height)
        {
            base.Initialize(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GetCell(x, y).SetSprite(Global.SpriteManager.Wall);
                }
            }
        }

        public bool IsInPlayerFov(int x, int y)
        {
            return _playerFov.IsInFov(x, y);
        }

        //private Texture2D GetCellSprite(Cell cell)
        //{
        //    if (cell.IsWalkable)
        //    {
        //        return Global.SpriteManager.Floor;
        //    }
        //    else
        //    {
        //        return Global.SpriteManager.Wall;
        //    }
        //}

        public void UpdatePlayerFieldOfView()
        {
            Player player = Global.Player;
            _playerFov.ComputeFov(player.X, player.Y, player.Awareness, true);
            foreach (Tile tile in GetAllCells())
            {
                if (_playerFov.IsInFov(tile.X, tile.Y))
                {
                    tile.SetExplored(true);
                }

            }
        }

        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Tile tile = GetCell(x, y) as Tile;
            SetCellProperties(tile.X, tile.Y, tile.IsTransparent, isWalkable);
        }

        public void AddPlayer(Player player)
        {
            Global.Player = player;
            SetIsWalkable(player.X, player.Y, false);
            Global.Camera.CenterOn(GetCell(player.X, player.Y) as Cell);
            UpdatePlayerFieldOfView();
            Global.SchedulingSystem.Add(player);
        }

        public void AddEnemy(Enemy enemy)
        {
            _enemies.Add(enemy);
            SetIsWalkable(enemy.X, enemy.Y, false);
            Global.SchedulingSystem.Add(enemy);
        }

        public void RemoveEnemy(Enemy enemy)
        {
            _enemies.Remove(enemy);
            SetIsWalkable(enemy.X, enemy.Y, true);
            Global.SchedulingSystem.Remove(enemy);
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
    }
}
