using RogueSharp.DiceNotation;

using RSPoint = RogueSharp.Point;

namespace PostMortem_P1.Core
{
    public class Player : Actor
    {
        public Player(RSPoint position) : base()
        {
            Sprite = Global.SpriteManager.Player;
            X = position.X;
            Y = position.Y;

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
