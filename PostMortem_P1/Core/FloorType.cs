using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Graphics;

namespace PostMortem_P1.Core
{
    public class FloorType
    {
        public static Floor Dirt()
        {
            int floorID = (int)eFloorIDs.Dirt;
            string name = "Dirt";
            Texture2D sprite = Global.SpriteManager.Dirt;

            return new Floor(floorID, name, sprite);
        }

        public static Floor Grass()
        {
            int floorID = (int)eFloorIDs.Grass;
            string name = "Grass";
            Texture2D sprite = Global.SpriteManager.Grass;

            return new Floor(floorID, name, sprite);
        }

        public static Floor Road()
        {
            int floorID = (int)eFloorIDs.Road;
            string name = "Road";
            Texture2D sprite = Global.SpriteManager.Road;

            return new Floor(floorID, name, sprite);
        }

        public static Floor Sidewalk()
        {
            int floorID = (int)eFloorIDs.Sidewalk;
            string name = "Sidewalk";
            Texture2D sprite = Global.SpriteManager.Sidewalk;

            return new Floor(floorID, name, sprite);
        }

        public static Floor Floor()
        {
            int floorID = (int)eFloorIDs.Floor;
            string name = "Floor";
            Texture2D sprite = Global.SpriteManager.Floor;

            return new Floor(floorID, name, sprite);
        }

        public enum eFloorIDs
        {
            Dirt = 1,
            Grass = 2,
            Road = 3,
            Sidewalk = 4,
            Floor = 5
        }
    }
}
