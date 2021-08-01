using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionGetItemFromInventory : MenuActionGet
    {
        public Item SelectedItem { get; private set; }

        public MenuActionGetItemFromInventory(GraphicsDeviceManager graphics, MenuAction nextAction, bool isLastGet) : base(graphics, nextAction, isLastGet)
        {
        }

        public override bool Do()
        {
            var menuItems = new List<MenuItem>();

            Global.WorldMap.Player.Inventory.Items.ForEach(item =>
            {
                menuItems.Add(new MenuItem(item.Name, null));
            });

            MenuOverlay menuOverlay = new MenuOverlay(250, 350, menuItems, false, this, this.graphics);

            Global.OverlayManager.SetCurrentOverlay(menuOverlay);

            return true;
        }

        public void SetItem(int selection)
        {
            SelectedItem = Global.WorldMap.Player.Inventory.Items[selection];
            IsDataSet = true;

            if (this.isLastGet)
            {
                Global.OverlayManager.ReturnToGame();
            }

            this.nextAction.Do();
        }
    }
}
