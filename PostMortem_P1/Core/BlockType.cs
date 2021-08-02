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
            int blockID = (int)eBlockIDs.Air;
            string name = "Air";
            Texture2D sprite = null;
            bool isAir = true;
            bool isWalkable = true;
            bool isTransparent = true;
            int? itemVersionID = null;

            return new Block(blockID, name, sprite, isAir, isWalkable, isTransparent, itemVersionID);
        }

        public static Block Dirt()
        {
            int blockID = (int)eBlockIDs.Dirt;
            string name = "Dirt";
            Texture2D sprite = Global.SpriteManager.Dirt;
            bool isAir = false;
            bool isWalkable = false;
            bool isTransparent = false;
            int? itemVersionID = (int)ItemType.eItemIDs.Dirt;

            return new Block(blockID, name, sprite, isAir, isWalkable, isTransparent, itemVersionID);
        }

        public static Block BuildingWall()
        {
            int blockID = (int)eBlockIDs.BuildingWall;
            string name = "Building Wall";
            Texture2D sprite = Global.SpriteManager.BuildingWall;
            bool isAir = false;
            bool isWalkable = false;
            bool isTransparent = false;
            int? itemVersionID = (int)ItemType.eItemIDs.BuildingWall;

            return new Block(blockID, name, sprite, isAir, isWalkable, isTransparent, itemVersionID);
        }

        public static Block Wall()
        {
            int blockID = (int)eBlockIDs.Wall;
            string name = "Wall";
            Texture2D sprite = Global.SpriteManager.Wall;
            bool isAir = false;
            bool isWalkable = false;
            bool isTransparent = false;
            int? itemVersionID = (int)ItemType.eItemIDs.Wall;

            return new Block(blockID, name, sprite, isAir, isWalkable, isTransparent, itemVersionID);
        }

        public static ItemPickup ItemPickup()
        {
            int blockID = (int)eBlockIDs.ItemPickup;
            bool isAir = true;
            bool isWalkable = true;
            bool isTransparent = true;

            return new ItemPickup(blockID, isAir, isWalkable, isTransparent);
        }

        public static Block GetByID(int blockIDInt)
        {
            eBlockIDs blockID = (eBlockIDs)blockIDInt;
            switch(blockID)
            {
                case eBlockIDs.ItemPickup:
                    return ItemPickup();
                case eBlockIDs.Air:
                    return Air();
                case eBlockIDs.Dirt:
                    return Dirt();
                case eBlockIDs.BuildingWall:
                    return BuildingWall();
                case eBlockIDs.Wall:
                    return Wall();
                default:
                    return null;
            }
        }

        public static List<int> GetAllBlockIDs()
        {
            var values =  (int[])Enum.GetValues(typeof(BlockType.eBlockIDs));
            List<int> blockIDs = new List<int>();
            foreach(int value in values)
            {
                if (value > 0)
                {
                    blockIDs.Add(value);
                }
            }

            return blockIDs;
        }

        public enum eBlockIDs
        {
            ItemPickup = -1,
            Air = 0,
            Dirt = 1,
            BuildingWall = 2,
            Wall = 3,
        }
    }
}
