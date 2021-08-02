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
        private MenuActionGetSelectedTile _tileAction;
        private MenuActionGetBlockFromAllBlocks _blockAction;

        public MenuActionSetBlock(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public void SetActions(MenuActionGetSelectedTile tileAction, MenuActionGetBlockFromAllBlocks blockAction)
        {
            _tileAction = tileAction;
            _blockAction = blockAction;
        }

        public override bool Do()
        {
            Tile tile = _tileAction.GetSelectedTile();
            Block block = _blockAction.GetSelectedBlock();

            Global.WorldMap.CurrentChunkMap.SetBlockAndUpdateFov(tile, block);

            Global.OverlayManager.ReturnToGame();

            return true;
        }
    }
}
