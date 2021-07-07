using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

namespace PostMortem_P1
{
    public class PathToPlayer
    {
        private readonly Player _player;
        private readonly IMap _map;
        private readonly Texture2D _sprite;
        private readonly PathFinder _pathFinder;
        private IEnumerable<ICell> _cells;

        public PathToPlayer(Player player, IMap map, Texture2D sprite)
        {
            _player = player;
            _map = map;
            _sprite = sprite;
            _pathFinder = new PathFinder(map);
        }

        public Cell FirstCell
        {
            get
            {
                return _cells.ElementAt(1) as Cell;
            }
        }

        public void CreateFrom(int x, int y)
        {
            _cells = _pathFinder.ShortestPath(_map.GetCell(x, y), _map.GetCell(_player.X, _player.Y)).Steps;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (_cells != null && Global.GameState == GameStates.Debugging)
            {
                foreach (Cell cell in _cells)
                {
                    if (cell != null)
                    {
                        spriteBatch.Draw(_sprite, new Vector2(cell.X * _sprite.Width, cell.Y * _sprite.Width), null, Color.Blue * 0.2f, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Paths);
                    }
                }
            }

        }
    }
}
