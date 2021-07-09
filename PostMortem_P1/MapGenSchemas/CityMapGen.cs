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
            chunkMap.InitializeCells(Global.SpriteManager.Dirt, true, true);
            CreateRoad(chunkMap, chunkMap.Height / 2, 5, true);
            CreateRoad(chunkMap, chunkMap.Width / 2, 5, false);

            CreateBuilding(chunkMap, GetBuildingRect(chunkMap, eDirection.NW), eDirection.S);
            CreateBuilding(chunkMap, GetBuildingRect(chunkMap, eDirection.NE), eDirection.W);
            CreateBuilding(chunkMap, GetBuildingRect(chunkMap, eDirection.SW), eDirection.E);
            CreateBuilding(chunkMap, GetBuildingRect(chunkMap, eDirection.SE), eDirection.N);

            return chunkMap;
        }

        private void CreateRoad(ChunkMap chunkMap, int position, int width, bool isHorizontal)
        {
            Structure road = new Structure();

            for (int i = 0; i < (isHorizontal ? chunkMap.Width : chunkMap.Height); i++)
            {
                if (isHorizontal)
                {
                    for (int y = position - width / 2; y < position + width / 2 + 1; y++)
                    {
                        chunkMap.SetCellProperties(i, y, true, true);
                        var tile = chunkMap.GetCell(i, y);
                        tile.SetSprite(Global.SpriteManager.Road);
                        road.Add(tile);
                        _allRoadTiles.Add(tile);
                    }

                    if (!_allRoadTiles.Any(tile => tile.X == i && tile.Y == position - width / 2 - 1))
                    {
                        chunkMap.GetCell(i, position - width / 2 - 1).SetSprite(Global.SpriteManager.Sidewalk);
                    }

                    if (!_allRoadTiles.Any(tile => tile.X == i && tile.Y == position + width / 2 + 1))
                    {
                        chunkMap.GetCell(i, position + width / 2 + 1).SetSprite(Global.SpriteManager.Sidewalk);
                    }
                }
                else
                {
                    for (int x = position - width / 2; x < position + width / 2 + 1; x++)
                    {
                        chunkMap.SetCellProperties(x, i, true, true);
                        var tile = chunkMap.GetCell(x, i);
                        tile.SetSprite(Global.SpriteManager.Road);
                        road.Add(tile);
                        _allRoadTiles.Add(tile);
                    }
                    
                    if (!_allRoadTiles.Any(tile => tile.X == position - width / 2 - 1 && tile.Y == i))
                    {
                        chunkMap.GetCell(position - width / 2 - 1, i).SetSprite(Global.SpriteManager.Sidewalk);
                    }

                    if (!_allRoadTiles.Any(tile => tile.X == position + width / 2 + 1 && tile.Y == i))
                    {
                        chunkMap.GetCell(position + width / 2 + 1, i).SetSprite(Global.SpriteManager.Sidewalk);
                    }
                }
            }

            _roads.Add(road);
        }

        public override List<RSPoint> GetSuitableEnemyPositionList(ChunkMap chunkMap, int num)
        {
            List<RSPoint> positions = new List<RSPoint>();

            while (positions.Count < num)
            {
                foreach(Structure building in _buildingFloors)
                {
                    if (Dice.Roll("1d10") > 8)
                    {
                        int tileNum = Global.Random.Next(building.Count);
                        var tile = building.ElementAt(tileNum);
                        if (chunkMap.IsWalkable(tile.X, tile.Y))
                        {
                            positions.Add(new RSPoint(tile.X, tile.Y));
                        }
                    }
                }
            }

            return positions;
        }

        private RSRectangle GetBuildingRect(ChunkMap chunkMap, eDirection corner)
        {
            int x, y, width, height;

            switch(corner)
            {
                case eDirection.NW:
                    x = 1;
                    y = 1;
                    width = _roads[1].ElementAt(0).X - 3 - x;
                    height = _roads[0].ElementAt(0).Y - 3 - y;
                    break;
                case eDirection.NE:
                    x = _roads[1].ElementAt(4).X + 3;
                    y = 1;
                    width = chunkMap.Width - 2 - x;
                    height = _roads[0].ElementAt(0).Y - 3 - y;
                    break;
                case eDirection.SW:
                    x = 1;
                    y = _roads[0].ElementAt(4).Y + 3;
                    width = _roads[1].ElementAt(0).X - 3 - x;
                    height = chunkMap.Height - 2 - y;
                    break;
                case eDirection.SE:
                    x = _roads[1].ElementAt(4).X + 3;
                    y = _roads[0].ElementAt(4).Y + 3;
                    width = chunkMap.Width - 2 - x;
                    height = chunkMap.Height - 2 - y;
                    break;
                default:
                    throw new Exception("Invalid building rectangle corner");
            }

            return new RSRectangle(x, y, width, height);
        }

        private void CreateBuilding(ChunkMap chunkMap, RSRectangle rect, eDirection entranceSide)
        {
            Structure buildingFloor = new Structure();

            for (int xPos = rect.X; xPos < rect.X + rect.Width + 1; xPos++)
            {
                chunkMap.SetCellProperties(xPos, rect.Y, false, false);
                chunkMap.GetCell(xPos, rect.Y).SetSprite(Global.SpriteManager.BuildingWall);
                
                chunkMap.SetCellProperties(xPos, rect.Y + rect.Height, false, false);
                chunkMap.GetCell(xPos, rect.Y + rect.Height).SetSprite(Global.SpriteManager.BuildingWall);
            }

            for (int yPos = rect.Y; yPos < rect.Y + rect.Height + 1; yPos++)
            {
                chunkMap.SetCellProperties(rect.X, yPos, false, false);
                chunkMap.GetCell(rect.X, yPos).SetSprite(Global.SpriteManager.BuildingWall);
                
                chunkMap.SetCellProperties(rect.X + rect.Width, yPos, false, false);
                chunkMap.GetCell(rect.X + rect.Width, yPos).SetSprite(Global.SpriteManager.BuildingWall);
            }

            int x, y;
            switch (entranceSide)
            {
                case eDirection.N:
                    x = Global.Random.Next(rect.X + 1, rect.X + rect.Width - 1);
                    y = rect.Y;
                    break;
                case eDirection.W:
                    x = rect.X;
                    y = Global.Random.Next(rect.Y + 1, rect.Y + rect.Height - 1);
                    break;
                case eDirection.E:
                    x = rect.X + rect.Width;
                    y = Global.Random.Next(rect.Y + 1, rect.Y + rect.Height - 1);
                    break;
                case eDirection.S:
                    x = Global.Random.Next(rect.X + 1, rect.X + rect.Width - 1);
                    y = rect.Y + rect.Height;
                    break;
                default:
                    throw new Exception("Invalid direction for building entrance");
            }

            chunkMap.SetCellProperties(x, y, true, true);
            var tile = chunkMap.GetCell(x, y);
            tile.SetSprite(Global.SpriteManager.Floor);
            buildingFloor.Add(tile);

            for (int xPos = rect.X + 1; xPos < rect.X + rect.Width; xPos++)
            {
                for (int yPos = rect.Y + 1; yPos < rect.Y + rect.Height; yPos++)
                {
                    chunkMap.SetCellProperties(xPos, yPos, true, true);
                    tile = chunkMap.GetCell(xPos, yPos);
                    tile.SetSprite(Global.SpriteManager.Floor);
                    buildingFloor.Add(tile);
                }
            }

            _buildingFloors.Add(buildingFloor);
        }
    }
}
