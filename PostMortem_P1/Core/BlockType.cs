using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace PostMortem_P1.Core
{
    public class BlockType
    {
        public static Block Air()
        {
            int blockID = 0;
            Texture2D sprite = null;
            bool isAir = true;
            bool isWalkable = true;
            bool isTransparent = true;

            return new Block(blockID, sprite, isAir, isWalkable, isTransparent);
        }

        public static Block Dirt()
        {
            int blockID = 1;
            Texture2D sprite = Global.SpriteManager.Dirt;
            bool isAir = false;
            bool isWalkable = false;
            bool isTransparent = false;

            return new Block(blockID, sprite, isAir, isWalkable, isTransparent);
        }

        public static Block BuildingWall()
        {
            int blockID = 2;
            Texture2D sprite = Global.SpriteManager.BuildingWall;
            bool isAir = false;
            bool isWalkable = false;
            bool isTransparent = false;

            return new Block(blockID, sprite, isAir, isWalkable, isTransparent);
        }

        public static Block Wall()
        {
            int blockID = 3;
            Texture2D sprite = Global.SpriteManager.Wall;
            bool isAir = false;
            bool isWalkable = false;
            bool isTransparent = false;

            return new Block(blockID, sprite, isAir, isWalkable, isTransparent);
        }

        public static ItemPickup ItemPickup()
        {
            int blockID = 4;
            bool isAir = true;
            bool isWalkable = true;
            bool isTransparent = true;

            return new ItemPickup(blockID, isAir, isWalkable, isTransparent);
        }
    }
}
