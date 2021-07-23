using System;
using System.Collections.Generic;
using System.Text;

namespace PostMortem_P1.Core
{
    public class ItemType
    {
        public static Item Dirt()
        {
            int itemID = 1;
            string name = "Dirt";

            return new Item(itemID, name);
        }

        public static Item Apple()
        {
            int itemID = 2;
            string name = "Apple";

            return new Item(itemID, name);
        }
    }
}
