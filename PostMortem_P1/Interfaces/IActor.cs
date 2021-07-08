using System;
using System.Collections.Generic;
using System.Text;

namespace PostMortem_P1.Interfaces
{
    public interface IActor
    {
        // General stats
        string Name { get; set; }
        int Awareness { get; set; }
        int Speed { get; set; }

        // Combat stats
        int Attack { get; set; }
        int AttackChance { get; set; }
        int DefenseChance { get; set; }
        int Health { get; set; }
        int MaxHealth { get; set; }
    }
}
