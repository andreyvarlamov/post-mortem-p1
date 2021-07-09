using System.Diagnostics;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp.DiceNotation;

using PostMortem_P1.Interfaces;

using IDrawable = PostMortem_P1.Interfaces.IDrawable;

namespace PostMortem_P1.Core
{
    public class Actor : IActor, IDrawable, IScheduleable
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
            if (!Global.CurrentChunkMap.GetCell(X, Y).IsExplored && !Global.Debugging)
            {
                return;
            }

            if (Global.CurrentChunkMap.IsInPlayerFov(X, Y) || Global.Debugging)
            {
                spriteBatch.Draw(Sprite, new Vector2(X * Sprite.Width, Y * Sprite.Width), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Actors);
            }
        }
        #endregion

        #region IScheduleable
        public int Time
        {
            get
            {
                return Speed;
            }
        }

        #endregion

        public bool SetPosition(int x, int y)
        {
            if (x < 0)
            {
                // Move to the left chunk
                int playerCurrentPosX = Global.WorldMap.PlayerX;
                int playerCurrentPosY = Global.WorldMap.PlayerY;

                if (playerCurrentPosX > 0)
                {
                    Global.WorldMap.SetPlayerWorldPosition(playerCurrentPosX - 1, playerCurrentPosY);
                    Global.CurrentChunkMap = Global.WorldMap[playerCurrentPosX - 1, playerCurrentPosY];

                    x = Global.CurrentChunkMap.Width - 1;
                }
            }
            else if (x >= Global.CurrentChunkMap.Width)
            {
                // Move to the right chunk
                int playerCurrentPosX = Global.WorldMap.PlayerX;
                int playerCurrentPosY = Global.WorldMap.PlayerY;

                if (playerCurrentPosX < Global.CurrentChunkMap.Width - 1)
                {
                    Global.WorldMap.SetPlayerWorldPosition(playerCurrentPosX + 1, playerCurrentPosY);
                    Global.CurrentChunkMap = Global.WorldMap[playerCurrentPosX + 1, playerCurrentPosY];

                    x = 0;
                }
            }
            else if (y < 0)
            {
                // Move to the upper chunk
                int playerCurrentPosX = Global.WorldMap.PlayerX;
                int playerCurrentPosY = Global.WorldMap.PlayerY;

                if (playerCurrentPosY > 0)
                {
                    Global.WorldMap.SetPlayerWorldPosition(playerCurrentPosX, playerCurrentPosY - 1);
                    Global.CurrentChunkMap = Global.WorldMap[playerCurrentPosX, playerCurrentPosY - 1];

                    y = Global.CurrentChunkMap.Height - 1;
                }
            }
            else if (y >= Global.CurrentChunkMap.Height)
            {
                // Move to the bottom chunk
                int playerCurrentPosX = Global.WorldMap.PlayerX;
                int playerCurrentPosY = Global.WorldMap.PlayerY;

                if (playerCurrentPosY < Global.CurrentChunkMap.Height - 1)
                {
                    Global.WorldMap.SetPlayerWorldPosition(playerCurrentPosX, playerCurrentPosY + 1);
                    Global.CurrentChunkMap = Global.WorldMap[playerCurrentPosX, playerCurrentPosY + 1];

                    y = 0;
                }
            }

            if (x >= 0 && x < Global.CurrentChunkMap.Width &&
                y >= 0 &&  y < Global.CurrentChunkMap.Height &&
                Global.CurrentChunkMap.GetCell(x, y).IsWalkable)
            {
                //Debug.WriteLine($"x = {x} y = {y} IsWalkable={Global.WorldCellMap.GetCell(x, y).IsWalkable}");

                // Set the previous cell to walkable
                Global.CurrentChunkMap.SetIsWalkable(X, Y, true);

                X = x;
                Y = y;

                // Set the current cell to not walkable
                Global.CurrentChunkMap.SetIsWalkable(X, Y, false);

                if (this is Player)
                {
                    Global.CurrentChunkMap.UpdatePlayerFieldOfView();
                    Debug.WriteLine($"Player position: x = {X}; y = {Y}");
                }

                return true;
            }

            return false;
        }
    }
}
