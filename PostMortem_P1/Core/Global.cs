using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp.Random;

using PostMortem_P1.Graphics;
using PostMortem_P1.Systems;
using PostMortem_P1.Input;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Core
{
    public enum GameStates
    {
        None = 0,
        PlayerTurn = 1,
        NPCTurn = 2,
        Debugging = 3
    }

    public class Global
    {
        #region Constants
        public static readonly int MapWidth = 32;
        public static readonly int MapHeight = 32;

        public static readonly int WorldWidth = 6;
        public static readonly int WorldHeight = 6;
        #endregion

        public static bool Debugging = false;

        public static readonly IRandom Random = new DotNetRandom();

        public static SpriteManager SpriteManager { get; set; }
        public static FontManager FontManager { get; set; }
        public static InputManager InputManager { get; set; }
        public static WorldMap WorldMap { get; set; }

        public static GameMode GameMode { get; set; }
        public static OverlayManager OverlayManager { get; set; }
        public static Hud Hud { get; set; }
        public static GraphicsDevice GraphicsDevice { get; set; }
    }
}
