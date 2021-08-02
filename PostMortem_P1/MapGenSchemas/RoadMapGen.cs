using System;
using System.Collections.Generic;
using System.Text;

using Structure = System.Collections.Generic.HashSet<PostMortem_P1.Core.Tile>;

using PostMortem_P1.Core;

namespace PostMortem_P1.MapGenSchemas
{
    public class RoadMapGen : MapGenSchema
    {
        private bool _isHorizontal;

        private List<Structure> _roads = new List<Structure>();

        private Structure _allRoadsTiles = new Structure();

        public RoadMapGen(bool isHorizontal)
        {
            _isHorizontal = isHorizontal;
        }

        public override ChunkMap CreateMap(int width, int height)
        {
            var chunkMap = base.CreateMap(width, height);
            chunkMap.InitializeDefaultTiles(FloorType.Grass(), null, false);

            MapGenHelpers.CreateRoad(chunkMap, _roads, _allRoadsTiles, (_isHorizontal ? height : width) / 2, 5, _isHorizontal);

            return chunkMap;
        }
    }
}
