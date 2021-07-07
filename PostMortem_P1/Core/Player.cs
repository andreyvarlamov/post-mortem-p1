using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

namespace PostMortem_P1.Core
{
    public class Player : Actor
    {
        public Player(Texture2D sprite, int x, int y)
        {
            Awareness = 15;
            Name = "Player";
            Sprite = sprite;
            X = x;
            Y = y;
        }
    }
}
