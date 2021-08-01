using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionGetItemFromInventory : MenuAction
    {
        public bool IsDataSet { get; private set; }
        public Item SelectedItem { get; private set; }

        private MenuAction _nextAction;

        private bool _isLastGet;

        public MenuActionGetItemFromInventory(GraphicsDeviceManager graphics, MenuAction nextAction, bool isLastGet) : base(graphics)
        {
            _nextAction = nextAction;
            _isLastGet = isLastGet;
        }

        public override bool Do()
        {
            var menuItems = new List<MenuItem>();

            Global.WorldMap.Player.Inventory.Items.ForEach(item =>
            {
                menuItems.Add(new MenuItem(item.Name, null));
            });

            MenuOverlay menuOverlay = new MenuOverlay(250, 350, menuItems, false, this, this.graphics);

            Global.OverlayManager.SetCurrentOverlayAndReset(menuOverlay);

            return true;
        }

        public void SetItem(int selection)
        {
            SelectedItem = Global.WorldMap.Player.Inventory.Items[selection];
            IsDataSet = true;

            if (_isLastGet)
            {
                Global.OverlayManager.ReturnToGame();
            }

            _nextAction.Do();
        }
    }
}
