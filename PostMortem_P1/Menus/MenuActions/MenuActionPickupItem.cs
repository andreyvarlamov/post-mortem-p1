using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionPickupItem : MenuAction
    {
        private MenuActionGetSelectedTile _tileAction;
        private MenuActionGetItemFromInventory _itemAction;

        private bool _isItemSelected;

        public MenuActionPickupItem(GraphicsDeviceManager graphics) : base(graphics)
        {
            _isItemSelected = false;
        }

        public void SetActions(MenuActionGetSelectedTile tileAction, MenuActionGetItemFromInventory itemAction)
        {
            _tileAction = tileAction;
            _itemAction = itemAction;
        }

        public override bool Do()
        {
            if (!_tileAction.IsDataSet)
            {
                throw new Exception("tile action is not ready yet.");
            }

            Tile tile = _tileAction.SelectedTile;

            if (!_isItemSelected)
            {
                // Just tile was selected, set inventory for the item action, so user can select
                Inventory inventory = Global.WorldMap.CurrentChunkMap.GetItemPickupInventory(tile);
                if (inventory != null)
                {
                    _itemAction.SetInventory(inventory);
                    _itemAction.Do();
                    _isItemSelected = true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // Tile and item were selected, pick up item
                if (!_itemAction.IsDataSet)
                {
                    throw new Exception("item action is not ready yet.");
                }

                Item item = _itemAction.SelectedItem;

                Global.WorldMap.CurrentChunkMap.RemoveItemFromItemPickup(tile, item);

                Global.WorldMap.Player.AddToInventory(item);
            }

            return true;
        }
    }
}
