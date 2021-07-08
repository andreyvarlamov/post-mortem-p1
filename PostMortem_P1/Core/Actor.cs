using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp.DiceNotation;

using PostMortem_P1.Interfaces;

using IDrawable = PostMortem_P1.Interfaces.IDrawable;

namespace PostMortem_P1.Core
{
    public class Actor : IActor, IDrawable
    {
        #region IActor
        // General stats
        private int _awareness;
        private string _name;
        private int _speed;

        public int Awareness
        {
            get
            {
                return _awareness;
            }
            set
            {
                _awareness = value;
            }
        }
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public int Speed
        {
            get
            { 
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        // Combat stats
        private int _attackBonus;
        private int _armorClass;
        private DiceExpression _damage;
        private int _health;
        private int _maxHealth;

        public int AttackBonus
        {
            get
            {
                return _attackBonus;
            }
            set
            {
                _attackBonus = value;
            }
        }
        public int ArmorClass
        {
            get
            {
                return _armorClass;
            }
            set
            {
                _armorClass = value;
            }
        }
        public DiceExpression Damage
        {
            get
            {
                return _damage;
            }
            set
            {
                _damage = value;
            }
        }
        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }
        public int MaxHealth
        {
            get
            {
                return _maxHealth;
            }
            set
            {
                _maxHealth = value;
            }
        }
        #endregion

        #region IDrawable
        public int X { get; set; }
        public int Y { get; set; }
        public Texture2D Sprite { get; set; }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (!Global.WorldCellMap.GetCell(X, Y).IsExplored && !Global.Debugging)
            {
                return;
            }

            if (Global.WorldCellMap.IsInFov(X, Y) || Global.Debugging)
            {
                spriteBatch.Draw(Sprite, new Vector2(X * Sprite.Width, Y * Sprite.Width), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Actors);
            }
        }
        #endregion

        public bool SetPosition(int x, int y)
        {
            if (Global.WorldCellMap.GetCell(x, y).IsWalkable)
            {
                // Set the previous cell to walkable
                Global.WorldCellMap.SetIsWalkable(X, Y, true);

                X = x;
                Y = y;

                // Set the current cell to not walkable
                Global.WorldCellMap.SetIsWalkable(X, Y, false);

                if (this is Player)
                {
                    Global.WorldCellMap.UpdatePlayerFieldOfView();
                }

                return true;
            }

            return false;
        }
    }
}
