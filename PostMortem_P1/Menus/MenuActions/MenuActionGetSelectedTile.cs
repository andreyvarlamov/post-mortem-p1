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
        public bool IsDataSet { get; private set; }
        public Tile SelectedTile { get; private set; }

        private MenuAction _nextAction;

        private bool _isLastGet;

        public MenuActionGetSelectedTile(GraphicsDeviceManager graphics, MenuAction nextAction, bool isLastGet) : base(graphics)
        {
            _nextAction = nextAction;
            _isLastGet = isLastGet;
        }

        public override bool Do()
        {
            TileSelectOverlay tileSelectOverlay = new TileSelectOverlay(Global.WorldMap.Player.X, Global.WorldMap.Player.Y, graphics, this);

            Global.OverlayManager.SetCurrentOverlay(tileSelectOverlay);

            return true;
        }

        public void SetTile(Tile tile)
        {
            SelectedTile = tile;
            IsDataSet = true;

            if (_isLastGet)
            {
                Global.OverlayManager.ReturnToGame();
            }

            _nextAction.Do();
        }
    }
}
