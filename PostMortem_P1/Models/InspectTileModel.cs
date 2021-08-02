using System;
using System.Collections.Generic;
using System.Text;

using PostMortem_P1.Core;

namespace PostMortem_P1.Models
{
    public class InspectTileModel
    {
        public TileModel Tile { get; set; }
        public ActorModel Actor { get; set; }

        public static InspectTileModel Get(Tile tile, Actor actor)
        {
            return new InspectTileModel
            {
                Tile = TileModel.Get(tile),
                Actor = ActorModel.Get(actor)
            };
        }
    }

    public class TileModel
    {
        public bool IsAir { get; set; }
        public bool IsWalkable { get; set; }
        public bool IsTransparent { get; set; }
        public bool IsExplored { get; set; }
        public FloorModel Floor { get; set; }
        public BlockModel Block { get; set; }
        public ItemPickupModel ItemPickup { get; set; }

        public static TileModel Get(Tile tile)
        {
            BlockModel block = null;
            ItemPickupModel itemPickup = null;

            if (tile.Block is ItemPickup)
            {
                itemPickup = ItemPickupModel.Get(tile.Block as ItemPickup); 
            }
            else
            {
                block = BlockModel.Get(tile.Block); 
            }

            return new TileModel
            {
                IsAir = tile.IsAir,
                IsWalkable = tile.IsTileWalkable,
                IsTransparent = tile.IsTransparent,
                IsExplored = tile.IsExplored,
                Floor = FloorModel.Get(tile.Floor),
                Block = block,
                ItemPickup = itemPickup
            };
        }
    }

    public class FloorModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public static FloorModel Get(Floor floor)
        {
            return new FloorModel
            {
                ID = floor.FloorID,
                Name = floor.Name
            };
        }
    }

    public class BlockModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool IsAir { get; set; }
        public bool IsWalkable { get; set; }
        public bool IsTransparent { get; set; }

        public static BlockModel Get(Block block)
        {
            return new BlockModel
            {
                ID = block.BlockID,
                Name = block.Name,
                IsAir = block.IsAir,
                IsWalkable = block.IsWalkable,
                IsTransparent = block.IsTransparent
            };
        }
    }

    public class ItemPickupModel
    {
        public InventoryModel Inventory { get; set; }

        public static ItemPickupModel Get(ItemPickup itemPickup)
        {
            return new ItemPickupModel
            {
                Inventory = InventoryModel.Get(itemPickup.Inventory)
            };
        }
    }

    public class ActorModel
    {
        public int X { get; set; }
        public int Y { get; set; }

        public int Awareness { get; set; }
        public string Name { get; set; }
        public int Speed { get; set; }

        public int ArmorClass { get; set; }
        public int AttackBonus { get; set; }
        public string Damage { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; set; }

        public int? Disposition { get; set; }

        public InventoryModel Inventory { get; set; }

        public static ActorModel Get(Actor actor)
        {
            if (actor == null)
            {
                return null;
            }

            int? disposition = null;

            if (actor is NPC)
            {
                disposition = (actor as NPC).Disposition;
            }

            return new ActorModel
            {
                X = actor.X,
                Y = actor.Y,

                Awareness = actor.Awareness,
                Name = actor.Name,
                Speed = actor.Speed,

                ArmorClass = actor.ArmorClass,
                AttackBonus = actor.AttackBonus,
                Damage = actor.Damage.ToString(),
                Health = actor.Health,
                MaxHealth = actor.MaxHealth,

                Disposition = disposition,

                Inventory = InventoryModel.Get(actor.Inventory)
            };
        }
    }

    public class InventoryModel
    {
        public List<string> Items { get; set; }

        public static InventoryModel Get(Inventory inventory)
        {
            if (inventory == null)
            {
                return null;
            }

            List<string> itemStrings = new List<string>();
            foreach (Item item in inventory.Items)
            {
                itemStrings.Add(item.Name);
            }

            return new InventoryModel
            {
                Items = itemStrings
            };
        }
    }
}
