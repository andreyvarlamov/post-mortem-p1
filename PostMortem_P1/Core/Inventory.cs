using System;
using System.Collections.Generic;
using System.Text;

namespace PostMortem_P1.Core
{
    public class Inventory
    {
        public List<Item> Items { get; set; }

        public Inventory()
        {
            Items = new List<Item>();
        }
    }
}
