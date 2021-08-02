using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace PostMortem_P1.Core
{
    public class ItemType
    {
        public static Item Dirt()
        {
            int itemID = (int)eItemIDs.Dirt;
            string name = "Dirt";
            Texture2D sprite = Global.SpriteManager.Dirt;
            int? blockVersionID = (int)BlockType.eBlockIDs.Dirt;

            return new Item(itemID, name, sprite, blockVersionID);
        }

        public static Item BuildingWall()
        {
            int itemID = (int)eItemIDs.BuildingWall;
            string name = "Building Wall";
            Texture2D sprite = Global.SpriteManager.BuildingWall;
            int? blockVersionID = (int)BlockType.eBlockIDs.BuildingWall;

            return new Item(itemID, name, sprite, blockVersionID);
        }

        public static Item Wall()
        {
            int itemID = (int)eItemIDs.Wall;
            string name = "Wall";
            Texture2D sprite = Global.SpriteManager.Wall;
            int? blockVersionID = (int)BlockType.eBlockIDs.Wall;

            return new Item(itemID, name, sprite, blockVersionID);
        }

        public static Item Apple()
        {
            int itemID = (int)eItemIDs.Apple;
            string name = "Apple";
            Texture2D sprite = Global.SpriteManager.Apple;
            int? blockVersionID = null;

            return new Item(itemID, name, sprite, blockVersionID);
        }

        public static Item GetByID(int itemIDInt)
        {
            eItemIDs itemID = (eItemIDs)itemIDInt;
            switch (itemID)
            {
                case eItemIDs.Dirt:
                    return Dirt();
                case eItemIDs.BuildingWall:
                    return BuildingWall();
                case eItemIDs.Wall:
                    return Wall();
                case eItemIDs.Apple:
                    return Apple();
                default:
                    return null;
            }
        }
        public static List<int> GetAllItemIDs()
        {
            var values =  (int[])Enum.GetValues(typeof(eItemIDs));
            List<int> itemIDs = new List<int>();
            foreach(int value in values)
            {
                if (value > 0)
                {
                    itemIDs.Add(value);
                }
            }

            return itemIDs;
        }

        public enum eItemIDs
        {
            None = 0,
            Dirt = 1,
            BuildingWall = 2,
            Wall = 3,
            Apple = 4
        }
    }
}
