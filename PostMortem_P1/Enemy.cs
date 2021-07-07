using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

namespace PostMortem_P1
{
    public class Enemy : Figure
    {
        private readonly PathToPlayer _path;

        private readonly IMap _map;
        private bool _isAwareOfPlayer;

        public Enemy(IMap map, PathToPlayer path)
        {
            _map = map;
            _path = path;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Sprite, new Vector2(X * Sprite.Width, Y * Sprite.Width), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Figures);
            _path.Draw(spriteBatch);
        }

        public void Update()
        {
            if (!_isAwareOfPlayer)
            {
                //When the enemy is not aware of the player
                // Check the map to see if they are in fov
                if (_map.IsInFov(X, Y))
                {
                    _isAwareOfPlayer = true;
                }
            }
            if (_isAwareOfPlayer)
            {
                _path.CreateFrom(X, Y);
                // If the player is located in the next cell, attack them; else just move
                if (Global.CombatManager.IsPlayerAt(_path.FirstCell.X, _path.FirstCell.Y))
                {
                    Global.CombatManager.Attack(this, Global.CombatManager.FigureAt(_path.FirstCell.X, _path.FirstCell.Y));
                }
                else
                {
                    X = _path.FirstCell.X;
                    Y = _path.FirstCell.Y;
                }
            }
        }
    }
}
