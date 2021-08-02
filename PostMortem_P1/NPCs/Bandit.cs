using RogueSharp.DiceNotation;

using PostMortem_P1.Core;

using RSPoint = RogueSharp.Point;

namespace PostMortem_P1.NPCs
{
    public class Bandit : NPC
    {
        public Bandit() : base()
        {
        }

        public static Bandit Create(RSPoint position, int level)
        {
            int health = Dice.Roll("10d5");
            int speed = Dice.Roll("1d4") + 8;
            var damage = Dice.Parse("1d3");

            return new Bandit
            {
                Sprite = Global.SpriteManager.Bandit,
                X = position.X,
                Y = position.Y,

                Awareness = 10,
                Name = "Bandit",
                Speed = speed,

                ArmorClass = 10,
                AttackBonus = 0,
                Damage = damage,
                Health = health,
                MaxHealth = health,

                Disposition = -100
            };
        }
    }
}
