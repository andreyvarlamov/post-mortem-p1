using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionSetBlock : MenuAction
    {
        public MenuActionSetBlock(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public override bool Do()
        {
            TileSelectOverlay tileSelectOverlay = (TileSelectOverlay)Global.OverlayManager.CurrentOverlay;

            Tile tile = tileSelectOverlay.GetHighlightedTile();

            Global.WorldMap.CurrentChunkMap.SetBlock(tile, BlockType.Wall());

            Global.OverlayManager.ReturnToGame();

            return true;
        }
    }
}
