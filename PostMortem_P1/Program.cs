﻿using System;

namespace PostMortem_P1
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new PostMortem())
                game.Run();
        }
    }
}
