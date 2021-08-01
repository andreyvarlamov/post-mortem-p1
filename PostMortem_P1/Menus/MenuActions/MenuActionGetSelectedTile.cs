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
        public Tile SelectedTile { get; private set; }

        public MenuActionGetSelectedTile(GraphicsDeviceManager graphics, MenuAction nextAction, bool isLastGet) : base(graphics, nextAction, isLastGet)
        {
        }

        public override bool Do()
        {
            TileSelectOverlay tileSelectOverlay = new TileSelectOverlay(Global.WorldMap.Player.X, Global.WorldMap.Player.Y, graphics, this);

            Global.OverlayManager.SetCurrentOverlay(tileSelectOverlay);

            return true;
        }

        public override void SetData(object data)
        {
            base.SetData(data);
        }

        public void SetTile(Tile tile)
        {
            SelectedTile = tile;
            IsDataSet = true;

            if (this.isLastGet)
            {
                Global.OverlayManager.ReturnToGame();
            }

            this.nextAction.Do();
        }
    }
}
