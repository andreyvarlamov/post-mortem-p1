using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

namespace PostMortem_P1.Core
{
    public class Tile : Cell
    {
        public bool IsExplored { get; private set; }

        public Block Block { get; private set; }

        public Texture2D Floor { get; private set; }

        public bool IsAir
        {
            get
            {
                return Block.IsAir;
            }
        }

        // TODO I think hidingthis is the issue
        public bool IsTileWalkable
        {
            get
            {
                return Block.IsWalkable;
            }
            set
            {
                Block.IsWalkable = value;
            }
        }

        public Tile() : base()
        {

        }

        public Tile(int x, int y) : base(x, y, false, false)
        {
            IsExplored = false;
            SetFloor(Global.SpriteManager.Dirt);
            SetBlock(BlockType.Air());
        }

        public Tile(int x, int y, Texture2D floorSprite, Block block) : base(x, y, false, false)
        {
            IsExplored = false;
            SetFloor(floorSprite);
            SetBlock(block);
        }

        public Tile(int x, int y, Block block) : base (x, y, false, false)
        {
            IsExplored = false;
            SetFloor(Global.SpriteManager.Dirt);
            SetBlock(block);
        }

        public Tile(int x, int y, Texture2D floorSprite) : base(x, y, true, true)
        {
            IsExplored = false;
            SetFloor(floorSprite);
            SetBlock(BlockType.Air());
        }

        public void SetExplored(bool isExplored)
        {
            IsExplored = isExplored;
        }

        public void SetBlock(Block block)
        {
            Block = block;
            IsTransparent = Block.IsTransparent;
        }

        public void SetItemPickup(ItemPickup itemPickup)
        {
            Block = itemPickup;
        }

        public void SetFloor(Texture2D floorSprite)
        {
            Floor = floorSprite;
        }
    }
}
