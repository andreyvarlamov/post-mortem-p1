using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;
namespace PostMortem_P1.Core
{
    public class Block
    {
        public int BlockID { get; private set; }
        public virtual Texture2D Sprite { get; set; }
        public bool IsAir { get; private set; }
        public bool IsWalkable { get; set; }
        public bool IsTransparent { get; set; }

        public bool HasItemVersion { get; private set; }
        public Item ItemVersion { get; private set; }

        public Block(int blockID, Texture2D sprite, bool isAir, bool isWalkable, bool isTransparent, Item itemVersion)
        {
            BlockID = blockID;
            Sprite = sprite;
            IsAir = isAir;
            IsWalkable = isWalkable;
            IsTransparent = isTransparent;
            ItemVersion = itemVersion;
            HasItemVersion = itemVersion != null;
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
