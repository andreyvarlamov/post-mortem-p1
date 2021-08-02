using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionGetBlockFromAllBlocks : MenuActionGet
    {
        private List<int> _allBlockIDs;

        public MenuActionGetBlockFromAllBlocks(GraphicsDeviceManager graphics, MenuAction nextAction, bool isLastGet) : base(graphics, nextAction, isLastGet)
        {
            _allBlockIDs = BlockType.GetAllBlockIDs();
        }

        public override bool Do()
        {
            var menuItems = new List<MenuItem>();
            foreach(int id in _allBlockIDs)
            {
                menuItems.Add(new MenuItem(BlockType.GetByID(id).Name, null));
            }

            MenuOverlay menuOverlay = new MenuOverlay(250, 350, menuItems, false, this, this.graphics);

            Global.OverlayManager.SetCurrentOverlay(menuOverlay);

            return true;
        }

        public Block GetSelectedBlock()
        {
            if (IsDataSet)
            {
                return BlockType.GetByID(_allBlockIDs[SelectedIndex]);
            }
            else
            {
                throw new Exception("Data not set for MenuActionGetBlockFromAllBlocks");
            }
        }
    }
}
