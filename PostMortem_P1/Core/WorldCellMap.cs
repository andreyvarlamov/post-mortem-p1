using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

using PostMortem_P1.Graphics;

using RSRectangle = RogueSharp.Rectangle;
using RSPoint = RogueSharp.Point;

namespace PostMortem_P1.Core
{
    public class WorldCellMap : Map
    {
        public List<RSRectangle> Rooms;
        private readonly List<Enemy> _enemies;

        public WorldCellMap()
        {
            Rooms = new List<RSRectangle>();
            _enemies = new List<Enemy>();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Cell cell in GetAllCells())
            {
                // If not explored yet, don't render
                if (!cell.IsExplored && !Global.Debugging)
                {
                    continue;
                }

                // If explored, but not in fov - gray tint, if in fov - no tint
                Color tint = Color.Gray;
                if (cell.IsInFov || Global.Debugging)
                {
                    tint = Color.White; 
                }

                var position = new Vector2(cell.X * SpriteManager.SpriteSize, cell.Y * SpriteManager.SpriteSize);

                spriteBatch.Draw(GetCellSprite(cell), position, null, tint, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Cells);

                foreach (Enemy enemy in _enemies)
                {
                    enemy.Draw(spriteBatch);
                }
            }
        }

        private Texture2D GetCellSprite(Cell cell)
        {
            if (cell.IsWalkable)
            {
                return Global.SpriteManager.Floor;
            }
            else
            {
                return Global.SpriteManager.Wall;
            }
        }

        public void UpdatePlayerFieldOfView()
        {
            Player player = Global.Player;
            ComputeFov(player.X, player.Y, Global.Player.Awareness, true);
            foreach (Cell cell in GetAllCells())
            {
                if (IsInFov(cell.X, cell.Y))
                {
                    SetCellProperties(cell.X, cell.Y, cell.IsTransparent, cell.IsWalkable, true);
                }
            }
        }

        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Cell cell = GetCell(x, y) as Cell;
            SetCellProperties(cell.X, cell.Y, cell.IsTransparent, isWalkable, cell.IsExplored);
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

        public RSPoint? GetRandomWalkableLocationInRoom(RSRectangle room)
        {
            if (DoesRoomHaveWalkableSpace(room))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = Global.Random.Next(1, room.Width - 2) + room.X;
                    int y = Global.Random.Next(1, room.Height - 2) + room.Y;

                    if (IsWalkable(x, y))
                    {
                        return new RSPoint(x, y);
                    }
                }
            }

            return null;
        }
        public bool DoesRoomHaveWalkableSpace(RSRectangle room)
        {
            for (int x = 1; x <= room.Width - 2; x++)
            {
                for (int y = 1; y <= room.Height - 2; y++)
                {
                    if (IsWalkable(x + room.X, y + room.Y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
