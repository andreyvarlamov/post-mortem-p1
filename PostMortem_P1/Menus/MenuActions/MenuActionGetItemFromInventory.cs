using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionGetItemFromInventory : MenuActionGet
    {
        private Inventory _inventory;

        public MenuActionGetItemFromInventory(GraphicsDeviceManager graphics, MenuAction nextAction, bool isLastGet, Inventory inventory) : base(graphics, nextAction, isLastGet)
        {
            _inventory = inventory;
        }

        public override bool Do()
        {
            var menuItems = new List<MenuItem>();

            if (_inventory != null)
            {
                _inventory.Items.ForEach(item =>
                {
                    menuItems.Add(new MenuItem(item.Name, null));
                });
            }

            MenuOverlay menuOverlay = new MenuOverlay(250, 350, menuItems, false, this, this.graphics);

            Global.OverlayManager.SetCurrentOverlay(menuOverlay);

            return true;
        }

        /// <summary>
        /// Set inventory dynamically (if it was not available at the time of creation of the main menu)
        /// </summary>
        /// <param name="inventory"></param>
        public void SetInventory(Inventory inventory)
        {
            _inventory = inventory;
        }

        public Item GetSelectedItem()
        {
            if (IsDataSet)
            {
                return _inventory.Items[SelectedIndex];
            }
            else
            {
                return null;
            }
        }
    }
}
