using RogueSharp.DiceNotation;

using PostMortem_P1.Core;

using RSPoint = RogueSharp.Point;

namespace PostMortem_P1.NPCs
{
    public class Stranger : NPC
    {
        public Stranger() : base()
        {
        }

        public static Stranger Create(RSPoint position, int level)
        {
            int health = Dice.Roll("10d6");
            int speed = Dice.Roll("1d4") + 8;
            var damage = Dice.Parse("2d4");

            return new Stranger
            {
                Sprite = Global.SpriteManager.Stranger,
                X = position.X,
                Y = position.Y,

                Awareness = 15,
                Name = "Stranger",
                Speed = speed,

                ArmorClass = 15,
                AttackBonus = 1,
                Damage = damage,
                Health = health,
                MaxHealth = health,

                Disposition = 100
            };
        }
    }
}
