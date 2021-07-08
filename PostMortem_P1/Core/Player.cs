using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp.DiceNotation;

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

            ArmorClass = 15;
            AttackBonus = 1;
            Damage = Dice.Parse("2d4");
            Health = 50;
            MaxHealth = 50;
        }
    }
}
