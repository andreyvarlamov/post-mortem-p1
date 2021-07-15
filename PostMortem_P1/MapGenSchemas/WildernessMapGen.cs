using System;
using System.Collections.Generic;
using System.Text;

using RSRectangle = RogueSharp.Rectangle;

using PostMortem_P1.Core;

namespace PostMortem_P1.MapGenSchemas
{
    public class WildernessMapGen : MapGenSchema
    {
        private int _maxDirtPatches;
        private int _dirtPatchMaxSize;
        private int _dirtPatchMinSize;

        private List<RSRectangle> _dirtPatches;
        public WildernessMapGen(int maxDirtPatches, int dirtPatchMaxSize, int dirtPatchMinSize)
        {
            _maxDirtPatches = maxDirtPatches;
            _dirtPatchMaxSize = dirtPatchMaxSize;
            _dirtPatchMinSize = dirtPatchMinSize;
        }

        public override ChunkMap CreateMap(int width, int height)
        {
            var chunkMap = base.CreateMap(width, height);

            chunkMap.InitializeDefaultTiles(Global.SpriteManager.Grass, null, false);

            _dirtPatches = MapGenHelpers.GenerateNonIntersectingRects(Width, Height, _maxDirtPatches, _dirtPatchMaxSize, _dirtPatchMinSize);

            foreach(RSRectangle dirtPatch in _dirtPatches)
            {
                MapGenHelpers.CreateRoom(chunkMap, dirtPatch, Global.SpriteManager.Dirt);
            }

            return chunkMap;
        }
    }
}
