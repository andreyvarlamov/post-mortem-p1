using RogueSharp.Random;

using PostMortem_P1.Graphics;
using PostMortem_P1.Systems;

namespace PostMortem_P1.Core
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

        public static bool Debugging = false;
        
        public static readonly IRandom Random = new DotNetRandom();
        public static readonly Camera Camera = new Camera();

        public static SpriteManager SpriteManager { get; set; }

        public static WorldCellMap WorldCellMap { get; set; }

        public static Player Player { get; set; }

        public static CommandSystem CommandSystem { get; set; }

        public static SchedulingSystem SchedulingSystem { get; set; }
    }
}
