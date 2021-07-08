﻿using RogueSharp.Random;

using PostMortem_P1.Core;
using PostMortem_P1.Graphics;
using PostMortem_P1.Systems;

namespace PostMortem_P1
{
    public enum GameStates
    {
        None = 0,
        PlayerTurn = 1,
        EnemyTurn = 2,
        Debugging = 3
    }

    public class Global
    {
        #region Constants
        public static readonly int MapWidth = 50;
        public static readonly int MapHeight = 30;
        #endregion

        public static readonly IRandom Random = new DotNetRandom();
        public static GameStates GameState { get; set; }
        public static readonly Camera Camera = new Camera();
        //public static CombatManager CombatManager;

        public static bool Debugging = false;

        public static SpriteManager SpriteManager { get; set; }

        public static WorldCellMap WorldCellMap { get; set; }

        public static Player Player { get; set; }

        public static CommandSystem CommandSystem { get; set; }
    }
}
