using System;
using System.Collections.Generic;
using System.Text;

namespace PostMortem_P1.Interfaces
{
    public interface IActor
    {
        string Name { get; set; }
        int Awareness { get; set; }
    }
}
