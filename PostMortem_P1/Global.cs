using RogueSharp.Random;

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
        public static readonly int SpriteSize = 64;
        #endregion

        public static readonly IRandom Random = new DotNetRandom();
        public static GameStates GameState { get; set; }
        public static readonly Camera Camera = new Camera();
        //public static CombatManager CombatManager;

        public static bool Debugging = false;
    }
}
