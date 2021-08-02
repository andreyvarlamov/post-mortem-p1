using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace PostMortem_P1.Core
{
    public class Block
    {
        public int BlockID { get; private set; }
        public string Name { get; private set; }
        public virtual Texture2D Sprite { get; set; }
        public bool IsAir { get; private set; }
        public bool IsWalkable { get; set; }
        public bool IsTransparent { get; set; }

        public int? ItemVersionID { get; private set; }

        public Block(int blockID, string name, Texture2D sprite, bool isAir, bool isWalkable, bool isTransparent, int? itemVersionID)
        {
            BlockID = blockID;
            Name = name;
            Sprite = sprite;
            IsAir = isAir;
            IsWalkable = isWalkable;
            IsTransparent = isTransparent;
            ItemVersionID = itemVersionID;
        }

        //public void SetAir(bool isAir)
        //{
        //    IsAir = isAir;
        //    if (IsAir)
        //    {
        //        Sprite = null;
        //    }
        //}
    }
}
