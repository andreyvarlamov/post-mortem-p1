using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionBuildBlock : MenuAction
    {
        private MenuActionGetSelectedTile _tileAction;
        private MenuActionGetItemFromInventory _itemAction;

        public MenuActionBuildBlock(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public void SetActions(MenuActionGetSelectedTile tileAction, MenuActionGetItemFromInventory itemAction)
        {
            _tileAction = tileAction;
            _itemAction = itemAction;
        }

        public override bool Do()
        {
            Tile tile = _tileAction.GetSelectedTile();
            Item item = _itemAction.GetSelectedItem();

            if (item.BlockVersionID.HasValue)
            {
                if (Global.WorldMap.CurrentChunkMap.BuildBlock(tile, BlockType.GetByID(item.BlockVersionID.Value)))
                {
                    Global.WorldMap.Player.RemoveFromInventory(item);
                }
                else
                {
                    return false;
                }

            }
            else
            {
                return false;
            }

            return true;
        }
    }
}
