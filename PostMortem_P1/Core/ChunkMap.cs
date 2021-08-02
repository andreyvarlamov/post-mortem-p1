using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using RogueSharp;
using RogueSharp.DiceNotation;

using RSRectangle = RogueSharp.Rectangle;
using RSPoint = RogueSharp.Point;

using PostMortem_P1.NPCs;
using PostMortem_P1.Graphics;
using PostMortem_P1.Systems;
using PostMortem_P1.Models;

namespace PostMortem_P1.Core
{
    public class ChunkMap : Map<Tile>
    {
        private readonly List<NPC> _npcs;

        private FieldOfView<Tile> _playerFov;

        private MapGenSchema _mapGenSchema;

        public SchedulingSystem SchedulingSystem;

        public ChunkMap(MapGenSchema mapGenSchema)
        {
            _npcs = new List<NPC>();

            _mapGenSchema = mapGenSchema;
            SchedulingSystem = new SchedulingSystem();

            _playerFov = new FieldOfView<Tile>(this);
        }

        public void InitializeDefaultTiles(Floor floor, Block block, bool isBlocks)
        {
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (isBlocks)
                    {
                        if (block == null)
                        {
                            throw new Exception("No block isntance has been passed, while isBlocks=true.");
                        }
                        this[x, y] = new Tile(x, y, block);

                    }
                    else
                    {
                        if (floor == null)
                        {
                            throw new Exception("No floor instance has been passed, while isBlocks=false.");
                        }
                        this[x, y] = new Tile(x, y, floor);
                    }
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Tile tile in GetAllCells())
            {
                // If not explored yet, don't render
                if (!tile.IsExplored && !Global.Debugging)
                {
                    continue;
                }


                // If explored, but not in fov - gray tint, if in fov - no tint
                Color tint = Color.Gray;
                if (_playerFov.IsInFov(tile.X, tile.Y) || Global.Debugging)
                {
                    tint = Color.White;
                }

                var position = new Vector2(tile.X * SpriteManager.SpriteSize, tile.Y * SpriteManager.SpriteSize);

                spriteBatch.Draw(tile.Floor.Sprite, position, null, tint, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Floors);

                if (tile.Block.Sprite != null)
                {
                    if (tile.Block is ItemPickup)
                    {
                        ((ItemPickup)tile.Block).UpdateCanvas(spriteBatch);
                    }

                    spriteBatch.Draw(tile.Block.Sprite, position, null, tint, 0.0f, Vector2.Zero, Vector2.One, SpriteEffects.None, LayerDepth.Blocks);
                }
            }

            foreach (NPC npc in _npcs)
            {
                npc.Draw(spriteBatch);
            }
        }

        public void Update()
        {
            //_npcs.ForEach(npc =>
            //{
            //    npc.Update();
            //});
        }

               public bool IsInPlayerFov(int x, int y)
        {
            return _playerFov.IsInFov(x, y);
        }

        public void UpdatePlayerFieldOfView()
        {
            Player player = Global.WorldMap.Player;
            _playerFov.ComputeFov(player.X, player.Y, player.Awareness, true);

            // Setting tiles in fov to explored (that information is stored in floors layer)
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    if (_playerFov.IsInFov(x, y))
                    {
                        SetExplored(x, y, true);
                    }
                }
            }
        }

        public void SetExplored(int x, int y, bool isExplored)
        {
            this[x, y].SetExplored(isExplored);
        }

        #region Map editing for map gen
        public Tile SetBlock(int x, int y, Block block)
        {
            Tile tile = this[x, y];
            tile.SetBlock(block);
            return tile;
        }

        public Tile RemoveBlockAndSetFloor(int x, int y, Floor floor)
        {

            Tile tile = this[x, y];
            tile.SetFloor(floor);
            tile.SetBlock(BlockType.Air());

            return tile;
        }
        #endregion

        public Tile SetBlockAndUpdateFov(Tile tile, Block block)
        {
            tile.SetBlock(block);
            UpdatePlayerFieldOfView();
            return tile;
        }

        public bool BuildBlock(Tile tile, Block block)
        {
            if (tile.Block.IsAir && !(tile.Block is ItemPickup))
            {
                SetBlockAndUpdateFov(tile, block);
            }
            else
            {
                return false;
            }

            return true;
        }

        public Tile RemoveBlock(Tile tile)
        {
            SetBlockAndUpdateFov(tile, BlockType.Air());
            return tile;
        }

        public Tile RemoveAndDropBlock(Tile tile)
        {
            Block block = tile.Block;

            if (block is ItemPickup || block.IsAir)
            {
                return null;
            }
            else
            {
                if (block.ItemVersionID.HasValue)
                {
                    var itemPickup = BlockType.ItemPickup();
                    itemPickup.AddItem(ItemType.GetByID(block.ItemVersionID.Value));
                    SetBlockAndUpdateFov(tile, itemPickup);
                }
                else
                {
                    RemoveBlock(tile);
                }

                return tile;
            }
        }

        public bool DropItemOnTile(Tile tile, Item item)
        {
            if (tile.Block is ItemPickup)
            {
                ((ItemPickup)tile.Block).AddItem(item);
            }
            else if (tile.Block.IsAir)
            {
                var itemPickup = BlockType.ItemPickup();

                itemPickup.AddItem(item);

                SetBlockAndUpdateFov(tile, itemPickup);
            }
            else
            {
                return false;
            }

            return true;
        }

        public Tile RemoveItemFromItemPickup(Tile tile, Item item)
        {
            bool isLastItem = ((ItemPickup)tile.Block).RemoveItem(item);

            if (isLastItem)
            {
                RemoveBlock(tile);
            }

            return tile;
        }

        public Inventory GetItemPickupInventory(Tile tile)
        {
            if (tile.Block is ItemPickup)
            {
                return ((ItemPickup)tile.Block).Inventory;
            }
            else
            {
                return null;
            }
        }

        public Tile ReplaceFloor(Tile tile, Floor floor)
        {
            tile.SetFloor(floor);
            return tile;
        }

        public InspectTileModel InspectTile(Tile tile)
        {
            Actor actor;
            Player player = Global.WorldMap.Player;
            if (tile.X == player.X && tile.Y == player.Y)
            {
                actor = player;
            }
            else
            {
                actor = GetNPCAt(tile.X, tile.Y);
            }

            return InspectTileModel.Get(tile, actor);
        }

        public bool IsExplored(int x, int y)
        {
            return this[x, y].IsExplored;
        }

        public bool IsTileWalkable(int x, int y)
        {
            return this[x, y].IsTileWalkable;
        }

        public void SetIsWalkable(int x, int y, bool isWalkable)
        {
            Tile tile = this[x, y];
            tile.IsTileWalkable = isWalkable;
        }

        public void SetMapForPlayer(Player player)
        {
            SetIsWalkable(player.X, player.Y, false);
            UpdatePlayerFieldOfView();
            AddActorToSchedulingSystem(player);
        }

        public void AddActorToSchedulingSystem(Actor actor)
        {
            SchedulingSystem.Add(actor);
        }

        public void RemoveActorFromSchedulingSystem(Actor actor)
        {
            SchedulingSystem.Remove(actor);
        }

        public void AddNPC(NPC npc)
        {
            _npcs.Add(npc);
            SetIsWalkable(npc.X, npc.Y, false);
            AddActorToSchedulingSystem(npc);
        }

        public void RemoveNPC(NPC npc)
        {
            _npcs.Remove(npc);
            SetIsWalkable(npc.X, npc.Y, true);
            RemoveActorFromSchedulingSystem(npc);
        }

        public NPC GetNPCAt(int x, int y)
        {
            return _npcs.FirstOrDefault(npc => npc.X == x && npc.Y == y);
        }

        public RSPoint? GetRandomWalkableLocationInRect(RSRectangle rect)
        {
            if (DoesRectHaveWalkableSpace(rect))
            {
                for (int i = 0; i < 100; i++)
                {
                    int x = Global.Random.Next(1, rect.Width - 2) + rect.X;
                    int y = Global.Random.Next(1, rect.Height - 2) + rect.Y;

                    if (IsTileWalkable(x, y))
                    {
                        return new RSPoint(x, y);
                    }
                }
            }

            return null;
        }

        private bool DoesRectHaveWalkableSpace(RSRectangle rect)
        {
            for (int x = 1; x <= rect.Width - 2; x++)
            {
                for (int y = 1; y <= rect.Height - 2; y++)
                {
                    if (IsTileWalkable(x + rect.X, y + rect.Y))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public RSPoint GetSuitablePlayerPosition()
        {
            return _mapGenSchema.GetSuitablePlayerPosition(this);
        }

        public void PlaceNPCs(int npcsNum)
        {
            var positionList = _mapGenSchema.GetSuitableNPCPositionList(this, npcsNum);
            foreach (RSPoint position in positionList)
            {
                NPC npc;

                bool isStranger = Dice.Roll("1d2") == 1;
                //bool isStranger = true;

                if (isStranger)
                {
                    npc = Stranger.Create(position, 1);
                }
                else
                {
                    npc = Bandit.Create(position, 1); 
                }

                AddNPC(npc);
            }

        }
    }
}
