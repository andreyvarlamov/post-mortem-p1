﻿using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionRemoveBlock : MenuAction
    {
        private MenuActionGetSelectedTile _tileAction;

        public MenuActionRemoveBlock(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public void SetTileAction(MenuActionGetSelectedTile tileAction)
        {
            _tileAction = tileAction;
        }

        public override bool Do()
        {
            if (!_tileAction.IsDataSet)
            {
                throw new Exception("tile or item action is not ready yet.");
            }

            Tile tile = _tileAction.SelectedTile;

            Global.WorldMap.CurrentChunkMap.RemoveAndDropBlock(tile);

            return true;
        }
    }
}
