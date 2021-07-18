using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionGetSelectedTile : MenuAction
    {
        private MenuAction _selectAction;
        public MenuActionGetSelectedTile(GraphicsDeviceManager graphics, MenuAction selectAction) : base(graphics)
        {
            _selectAction = selectAction;
        }

        public override bool Do()
        {
            TileSelectOverlay tileSelectOverlay = new TileSelectOverlay(Global.WorldMap.Player.X, Global.WorldMap.Player.Y, graphics, _selectAction);

            Global.OverlayManager.SetCurrentOverlay(tileSelectOverlay);

            return true;
        }
    }
}
