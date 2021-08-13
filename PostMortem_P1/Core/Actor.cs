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
        public Actor()
        {
            Inventory = new Inventory();
        }

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
            if (!Global.WorldMap.CurrentChunkMap.IsExplored(X, Y) && !Global.Debugging)
            {
                return;
            }

            if (Global.WorldMap.CurrentChunkMap.IsInPlayerFov(X, Y) || Global.Debugging)
            {
                spriteBatch.Draw(Sprite, new Vector2(X * Sprite.Width, Y * Sprite.Width), null, Color.White, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Actors);
            }
        }
        #endregion

        #region IScheduleable
        public long Time
        {
            get
            {
                return Speed;
            }
        }

        #endregion

        public Inventory Inventory;

        public void AddToInventory(Item item)
        {
            Inventory.Items.Add(item);

            if (this is Player)
            {
                Global.Hud.SetItems(Inventory.Items);
            }
        }

        public void RemoveFromInventory(Item item)
        {
            Inventory.Items.Remove(item);

            if (this is Player)
            {
                Global.Hud.SetItems(Inventory.Items);
            }
        }

        public bool SetPosition(int setX, int setY, ChunkMap movingFromChunk)
        {
            WorldMap worldMap = Global.WorldMap;

            // Player can move between world chunks
            if (this is Player)
            {
                bool attemptedMoveChunks = false;

                int playerCurrentWorldPosX = worldMap.PlayerWorldPosX;
                int playerCurrentWorldPosY = worldMap.PlayerWorldPosY;

                if (setX < 0)
                {
                    // Move to the west chunk
                    playerCurrentWorldPosX--;
                    attemptedMoveChunks = true;
                }
                else if (setX >= worldMap.CurrentChunkMap.Width)
                {
                    // Move to the east chunk
                    playerCurrentWorldPosX++;
                    attemptedMoveChunks = true;
                }
                else if (setY < 0)
                {
                    // Move to the north chunk
                    playerCurrentWorldPosY--;
                    attemptedMoveChunks = true;
                }
                else if (setY >= worldMap.CurrentChunkMap.Height)
                {
                    // Move to the south chunk
                    playerCurrentWorldPosY++;
                    attemptedMoveChunks = true;
                }

                if (attemptedMoveChunks)
                {
                    return worldMap.SetPlayerWorldPosition(playerCurrentWorldPosX, playerCurrentWorldPosY);
                }
            }

            if (CheckIfCanMoveTo(setX, setY, worldMap.CurrentChunkMap))
            {
                // If coming from previos chunk, set that tile to walkable
                if (movingFromChunk != null)
                {
                    movingFromChunk.SetIsWalkable(X, Y, true);

                }
                else
                {
                    worldMap.CurrentChunkMap.SetIsWalkable(X, Y, true);
                }

                X = setX;
                Y = setY;

                // Set the current cell to not walkable
                worldMap.CurrentChunkMap.SetIsWalkable(X, Y, false);

                if (this is Player)
                {
                    worldMap.CurrentChunkMap.UpdatePlayerFieldOfView();
                    worldMap.Camera.CenterOn(worldMap.CurrentChunkMap[X, Y]);

                    Global.Hud.SetChunkX(X);
                    Global.Hud.SetChunkY(Y);

                    Global.Hud.SetWorldX(worldMap.PlayerWorldPosX);
                    Global.Hud.SetWorldY(worldMap.PlayerWorldPosY);
                }

                return true;
            }

            return false;
        }

        public bool CheckIfCanMoveTo(int x, int y, ChunkMap chunkMap)
        {
            bool result = (x >= 0 && x < chunkMap.Width &&
                y >= 0 && y < chunkMap.Height &&
                chunkMap.IsTileWalkable(x, y));

            return result;
        }
    }
}
