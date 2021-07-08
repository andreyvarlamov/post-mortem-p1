using RogueSharp.DiceNotation;

namespace PostMortem_P1.Enemies
{
    public class Bandit : Enemy
    {
        public static Bandit Create(int level, int x, int y)
        {
            int health = Dice.Roll("20d5");
            return new Bandit
            {
                Attack = Dice.Roll("1d3") + level / 3,
                AttackChance = Dice.Roll("25d3"),
                Awareness = 10,
                Defense = Dice.Roll("1d3") + level / 3,
                DefenseChance = Dice.Roll("10d4"),
                Health = health,
                MaxHealth = health,
                Name = "Bandit",
                Speed = Dice.Roll("1d4") + 8,
                Sprite = Global.SpriteManager.Enemy,

                X = x,
                Y = y
            };
        }
    }
}
