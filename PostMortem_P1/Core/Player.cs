using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;

namespace PostMortem_P1.Core
{
    public class Player : Actor
    {
        public Player(int x, int y)
        {
            Sprite = Global.SpriteManager.Player;
            X = x;
            Y = y;

            Awareness = 15;
            Name = "Player";
            Speed = 10;

            Attack = 2;
            AttackChance = 50;
            Defense = 2;
            DefenseChance = 40;
            Health = 100;
            MaxHealth = 100;
        }
    }
}
