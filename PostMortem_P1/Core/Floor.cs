using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace PostMortem_P1.Core
{
    public class Floor
    {
        public int FloorID { get; private set; }
        public string Name { get; private set; }
        public Texture2D Sprite { get; set; }

        public Floor(int floorID, string name, Texture2D sprite)
        {
            FloorID = floorID;
            Name = name;
            Sprite = sprite;
        }
    }
}
