﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace PostMortem_P1.Core
{
    public class Item
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public Texture2D Sprite { get; set; }

        public Item(int itemID, string name, Texture2D sprite)
        {
            ItemID = itemID;
            Name = name;
            Sprite = sprite;
        }
    }
}
