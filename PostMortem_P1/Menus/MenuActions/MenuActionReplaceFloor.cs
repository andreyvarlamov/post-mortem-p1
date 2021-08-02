using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionReplaceFloor : MenuAction
    {
        private MenuActionGetSelectedTile _tileAction;

        public MenuActionReplaceFloor(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public void SetTileAction(MenuActionGetSelectedTile tileAction)
        {
            _tileAction = tileAction;
        }

        public override bool Do()
        {
            Tile tile = _tileAction.GetSelectedTile();

            Global.WorldMap.CurrentChunkMap.ReplaceFloor(tile, FloorType.Floor());

            return true;
        }
    }
}
