using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PostMortem_P1.Graphics
{
    public class SpriteManager
    {
        public static readonly int SpriteSize = 64;

        private static class SpriteDictionary
        {
            public const string Floor = "Floor";
            public const string Wall = "Wall";

            public const string Player = "Player";
            public const string Enemy = "Hound";

            public const string White = "White";
        }

        public Texture2D Floor { get; private set; }
        public Texture2D Wall { get; private set; }

        public Texture2D Player { get; private set; }
        public Texture2D Enemy { get; private set; }

        public Texture2D White { get; private set; }

        public void LoadContent(ContentManager content)
        {
            Floor = content.Load<Texture2D>(SpriteDictionary.Floor);
            Wall = content.Load<Texture2D>(SpriteDictionary.Wall);

            Player = content.Load<Texture2D>(SpriteDictionary.Player);
            Enemy = content.Load<Texture2D>(SpriteDictionary.Enemy);

            White = content.Load<Texture2D>(SpriteDictionary.White);
        }
    }
}
