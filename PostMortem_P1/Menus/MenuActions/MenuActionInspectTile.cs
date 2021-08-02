using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;
using PostMortem_P1.Models;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionInspectTile : MenuAction
    {
        private MenuActionGetSelectedTile _tileAction;

        public MenuActionInspectTile(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public void SetTileAction(MenuActionGetSelectedTile tileAction)
        {
            _tileAction = tileAction;
        }

        public override bool Do()
        {
            Tile tile = _tileAction.GetSelectedTile();

            InspectTileModel inspectTileModel = Global.WorldMap.CurrentChunkMap.InspectTile(tile);

            MenuInspectTile menuInspectTile = new MenuInspectTile(300, 500, inspectTileModel, this.graphics);

            Global.OverlayManager.SetCurrentOverlay(menuInspectTile);

            return true;
        }
    }
}
