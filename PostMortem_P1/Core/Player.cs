using RogueSharp.DiceNotation;

using RSPoint = RogueSharp.Point;

namespace PostMortem_P1.Core
{
    public class Player : Actor
    {
        public Player(RSPoint position) : base()
        {
            int health = 50;
            int speed = 100;
            var damage = Dice.Parse("2d4");

            Sprite = Global.SpriteManager.Player;
            X = position.X;
            Y = position.Y;

            Awareness = 15;
            Name = "Player";
            Speed = speed;

            ArmorClass = 15;
            AttackBonus = 1;
            Damage = damage;
            Health = health;
            MaxHealth = health;
        }
    }
}
