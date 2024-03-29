﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace PostMortem_P1.Graphics
{
    public class SpriteManager
    {
        public static readonly int SpriteSize = 64;

        private static class SpriteDictionary
        {
            public const string BuildingWall = "BuildingWall";
            public const string Dirt = "Dirt";
            public const string Grass = "Grass";
            public const string Road = "Road";
            public const string Sidewalk = "Sidewalk";
            public const string Floor = "Floor";
            public const string Wall = "Wall";

            public const string Player = "Player";
            public const string Bandit = "Bandit";
            public const string Stranger = "Stranger";

            public const string White = "White";
            public const string MapCursor = "MapCursor";
            public const string ItemPickup = "ItemPickup";
            public const string ItemPickupMultiple = "ItemPickupMultiple";

            public const string Apple = "Apple";
        }

        public Texture2D BuildingWall { get; private set; }
        public Texture2D Dirt { get; private set; }
        public Texture2D Grass { get; private set; }
        public Texture2D Road { get; private set; }
        public Texture2D Sidewalk { get; private set; }
        public Texture2D Floor { get; private set; }
        public Texture2D Wall { get; private set; }

        public Texture2D Player { get; private set; }
        public Texture2D Bandit { get; private set; }
        public Texture2D Stranger { get; private set; }

        public Texture2D White { get; private set; }
        public Texture2D MapCursor { get; private set; }
        public Texture2D ItemPickup { get; private set; }
        public Texture2D ItemPickupMultiple { get; private set; }

        public Texture2D Apple { get; private set; }

        public void LoadContent(ContentManager content)
        {
            BuildingWall = content.Load<Texture2D>(SpriteDictionary.BuildingWall);
            Dirt = content.Load<Texture2D>(SpriteDictionary.Dirt);
            Grass = content.Load<Texture2D>(SpriteDictionary.Grass);
            Road = content.Load<Texture2D>(SpriteDictionary.Road);
            Sidewalk = content.Load<Texture2D>(SpriteDictionary.Sidewalk);
            Floor = content.Load<Texture2D>(SpriteDictionary.Floor);
            Wall = content.Load<Texture2D>(SpriteDictionary.Wall);

            Player = content.Load<Texture2D>(SpriteDictionary.Player);
            Bandit = content.Load<Texture2D>(SpriteDictionary.Bandit);
            Stranger = content.Load<Texture2D>(SpriteDictionary.Stranger);

            White = content.Load<Texture2D>(SpriteDictionary.White);
            MapCursor = content.Load<Texture2D>(SpriteDictionary.MapCursor);
            ItemPickup = content.Load<Texture2D>(SpriteDictionary.ItemPickup);
            ItemPickupMultiple = content.Load<Texture2D>(SpriteDictionary.ItemPickupMultiple);

            Apple = content.Load<Texture2D>(SpriteDictionary.Apple);
        }
    }
}
