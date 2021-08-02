using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionGetSelectedTile : MenuActionGet
    {
        private Tile _selectedTile;

        public MenuActionGetSelectedTile(GraphicsDeviceManager graphics, MenuAction nextAction, bool isLastGet) : base(graphics, nextAction, isLastGet)
        {
        }

        public override bool Do()
        {
            TileSelectOverlay tileSelectOverlay = new TileSelectOverlay(Global.WorldMap.Player.X, Global.WorldMap.Player.Y, graphics, this);

            Global.OverlayManager.SetCurrentOverlay(tileSelectOverlay);

            return true;
        }

        public void SetTile(Tile tile)
        {
            _selectedTile = tile;
            IsDataSet = true;

            if (this.isLastGet)
            {
                Global.OverlayManager.ReturnToGame();
            }

            this.nextAction.Do();
        }

        public Tile GetSelectedTile()
        {
            if (IsDataSet)
            {
                return _selectedTile;
            }
            else
            {
                throw new Exception("Data not set for MenuActionGetSelectedTile");
            }
        }
    }
}
