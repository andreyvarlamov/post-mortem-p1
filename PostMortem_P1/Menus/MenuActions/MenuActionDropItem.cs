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

            Global.WorldMap.CurrentChunkMap.DropItemOnTile(tile, item);

            Global.WorldMap.Player.RemoveFromInventory(item);

            return true;
        }
    }
}
