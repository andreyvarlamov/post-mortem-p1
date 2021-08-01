using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;
namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionDropItem : MenuAction
    {
        private MenuActionGetSelectedTile _tileAction;
        private MenuActionGetItemFromInventory _itemAction;

        public MenuActionDropItem(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public void SetActions(MenuActionGetSelectedTile tileAction, MenuActionGetItemFromInventory itemAction)
        {
            _tileAction = tileAction;
            _itemAction = itemAction;
        }

        public override bool Do()
        {
            if (!_tileAction.IsDataSet || !_itemAction.IsDataSet)
            {
                throw new Exception("tile or item action is not ready yet.");
            }

            Tile tile = _tileAction.SelectedTile;
            Item item = _itemAction.SelectedItem;

            if (tile.Block is ItemPickup)
            {
                ((ItemPickup)tile.Block).AddItem(item);
            }
            else if (tile.Block.IsAir)
            {
                var itemPickup = BlockType.ItemPickup();

                itemPickup.AddItem(item);

                var currentChunkMap = Global.WorldMap.CurrentChunkMap;
                currentChunkMap.SetBlock(tile, itemPickup);
            }
            else
            {
                return false;
            }

            Global.WorldMap.Player.RemoveFromInventory(item);

            return true;
        }
    }
}
