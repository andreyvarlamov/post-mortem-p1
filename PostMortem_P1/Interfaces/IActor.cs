using RogueSharp.DiceNotation;

namespace PostMortem_P1.Interfaces
{
    public interface IActor
    {
        // General stats
        string Name { get; set; }
        int Awareness { get; set; }
        int Speed { get; set; }

        // Combat stats
        int AttackBonus { get; set; }
        int ArmorClass { get; set; }
        DiceExpression Damage { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }
    }
}
