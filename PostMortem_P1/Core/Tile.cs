using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

namespace PostMortem_P1.Core
{
    public class Tile : Cell
    {
        public bool IsAir { get; private set; }
        public bool IsExplored { get; private set; }
        public Texture2D Sprite { get; private set; }

        public Tile() : base()
        {
            IsExplored = false;
        }

        public Tile(int x, int y, Texture2D sprite) : base(x, y, false, false)
        {
            Sprite = sprite;
            IsExplored = false;
        }

        public void SetExplored(bool isExplored)
        {
            IsExplored = isExplored;
        }

        public void SetSprite(Texture2D sprite)
        {
            Sprite = sprite;
        }

        public void SetAir(bool isAir)
        {
            IsAir = isAir;

            if (isAir)
            {
                Sprite = null;
                IsTransparent = true;
                IsWalkable = true;
            }
        }
    }
}
