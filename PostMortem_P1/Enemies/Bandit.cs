using RogueSharp.DiceNotation;

using PostMortem_P1.Core;

using RSPoint = RogueSharp.Point;

namespace PostMortem_P1.Enemies
{
    public class Bandit : Enemy
    {
        public static Bandit Create(RSPoint position, int level)
        {
            int health = Dice.Roll("10d5");
            return new Bandit
            {
                Sprite = Global.SpriteManager.Bandit,
                X = position.X,
                Y = position.Y,

                Awareness = 10,
                Name = "Bandit",
                Speed = Dice.Roll("1d4") + 8,

                ArmorClass = 10,
                AttackBonus = 0,
                Damage = Dice.Parse("1d3"),
                Health = health,
                MaxHealth = health
            };
        }
    }
}
