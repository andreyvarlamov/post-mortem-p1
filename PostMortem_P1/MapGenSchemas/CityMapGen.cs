using System;
using System.Collections.Generic;
using System.Text;

using PostMortem_P1.Core;

namespace PostMortem_P1.MapGenSchemas
{
    public class CityMapGen : MapGenSchema
    {
        public override ChunkMap CreateMap(int width, int height)
        {
            var chunkMap = base.CreateMap(width, height);
            return chunkMap;
        }
    }
}
