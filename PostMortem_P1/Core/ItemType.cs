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
            int itemID = 1;
            string name = "Dirt";
            Texture2D sprite = Global.SpriteManager.Dirt;

            return new Item(itemID, name, sprite);
        }

        public static Item Apple()
        {
            int itemID = 2;
            string name = "Apple";
            Texture2D sprite = Global.SpriteManager.Apple;

            return new Item(itemID, name, sprite);
        }
    }
}
