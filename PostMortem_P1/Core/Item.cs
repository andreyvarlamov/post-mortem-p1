using System;
using System.Collections.Generic;
using System.Text;

namespace PostMortem_P1.Core
{
    public class Item
    {
        public int ItemID { get; set; }
        public string Name { get; set; }

        public Item(int itemID, string name)
        {
            ItemID = itemID;
            Name = name;
        }
    }
}
