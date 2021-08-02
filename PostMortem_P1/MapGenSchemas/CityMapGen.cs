using System;
using System.Collections.Generic;
using System.Linq;

using RogueSharp.DiceNotation;

using RSPoint = RogueSharp.Point;
using RSRectangle = RogueSharp.Rectangle;

using Structure = System.Collections.Generic.HashSet<PostMortem_P1.Core.Tile>;

using PostMortem_P1.Core;

namespace PostMortem_P1.MapGenSchemas
{
    public class CityMapGen : MapGenSchema
    {
        private List<Structure> _roads = new List<Structure>();

        private List<Structure> _buildingFloors = new List<Structure>();

        private Structure _allRoadTiles = new Structure();

        public override ChunkMap CreateMap(int width, int height)
        {
            var chunkMap = base.CreateMap(width, height);
            chunkMap.InitializeDefaultTiles(FloorType.Grass(), null, false);
            MapGenHelpers.CreateRoad(chunkMap, _roads, _allRoadTiles, Height / 2, 5, true);
            MapGenHelpers.CreateRoad(chunkMap, _roads, _allRoadTiles, Width / 2, 5, false);

            CreateBuilding(chunkMap, GetBuildingRect(Direction.NW), Direction.S);
            CreateBuilding(chunkMap, GetBuildingRect(Direction.NE), Direction.W);
            CreateBuilding(chunkMap, GetBuildingRect(Direction.SW), Direction.E);
            CreateBuilding(chunkMap, GetBuildingRect(Direction.SE), Direction.N);

            return chunkMap;
        }


        public override List<RSPoint> GetSuitableNPCPositionList(ChunkMap chunkMap, int num)
        {
            List<RSPoint> positions = new List<RSPoint>();

            while (positions.Count < num)
            {
                foreach(Structure building in _buildingFloors)
                {
                    if (Dice.Roll("1d10") > 8)
                    {
                        int tileNum = Global.Random.Next(building.Count - 1);
                        var tile = building.ElementAt(tileNum);
                        if (chunkMap.IsTileWalkable(tile.X, tile.Y))
                        {
                            positions.Add(new RSPoint(tile.X, tile.Y));
                        }
                    }
                }
            }

            return positions;
        }

        private RSRectangle GetBuildingRect(Direction corner)
        {
            int x, y, width, height;

            switch(corner)
            {
                case Direction.NW:
                    x = 1;
                    y = 1;
                    width = _roads[1].ElementAt(0).X - 3 - x;
                    height = _roads[0].ElementAt(0).Y - 3 - y;
                    break;
                case Direction.NE:
                    x = _roads[1].ElementAt(4).X + 3;
                    y = 1;
                    width = Width - 2 - x;
                    height = _roads[0].ElementAt(0).Y - 3 - y;
                    break;
                case Direction.SW:
                    x = 1;
                    y = _roads[0].ElementAt(4).Y + 3;
                    width = _roads[1].ElementAt(0).X - 3 - x;
                    height = Height - 2 - y;
                    break;
                case Direction.SE:
                    x = _roads[1].ElementAt(4).X + 3;
                    y = _roads[0].ElementAt(4).Y + 3;
                    width = Width - 2 - x;
                    height = Height - 2 - y;
                    break;
                default:
                    throw new Exception("Invalid building rectangle corner");
            }

            return new RSRectangle(x, y, width, height);
        }

        private void CreateBuilding(ChunkMap chunkMap, RSRectangle rect, Direction entranceSide)
        {
            Structure buildingFloor = new Structure();

            for (int xPos = rect.X; xPos < rect.X + rect.Width + 1; xPos++)
            {
                chunkMap.SetBlock(xPos, rect.Y, BlockType.BuildingWall());
                chunkMap.SetBlock(xPos, rect.Y + rect.Height, BlockType.BuildingWall());
            }

            for (int yPos = rect.Y; yPos < rect.Y + rect.Height + 1; yPos++)
            {
                chunkMap.SetBlock(rect.X, yPos, BlockType.BuildingWall());
                chunkMap.SetBlock(rect.X + rect.Width, yPos, BlockType.BuildingWall());
            }

            int x, y;
            switch (entranceSide)
            {
                case Direction.N:
                    x = Global.Random.Next(rect.X + 1, rect.X + rect.Width - 1);
                    y = rect.Y;
                    break;
                case Direction.W:
                    x = rect.X;
                    y = Global.Random.Next(rect.Y + 1, rect.Y + rect.Height - 1);
                    break;
                case Direction.E:
                    x = rect.X + rect.Width;
                    y = Global.Random.Next(rect.Y + 1, rect.Y + rect.Height - 1);
                    break;
                case Direction.S:
                    x = Global.Random.Next(rect.X + 1, rect.X + rect.Width - 1);
                    y = rect.Y + rect.Height;
                    break;
                default:
                    throw new Exception("Invalid direction for building entrance");
            }

            buildingFloor.Add(chunkMap.RemoveBlockAndSetFloor(x, y, FloorType.Floor()));

            for (int xPos = rect.X + 1; xPos < rect.X + rect.Width; xPos++)
            {
                for (int yPos = rect.Y + 1; yPos < rect.Y + rect.Height; yPos++)
                {
                    buildingFloor.Add(chunkMap.RemoveBlockAndSetFloor(xPos, yPos, FloorType.Floor()));
                }
            }

            _buildingFloors.Add(buildingFloor);
        }
    }
}
