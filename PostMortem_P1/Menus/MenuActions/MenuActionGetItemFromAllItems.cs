using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionGetItemFromAllItems : MenuActionGet
    {
        private List<int> _allItemIDs;

        public MenuActionGetItemFromAllItems(GraphicsDeviceManager graphics, MenuAction nextAction, bool isLastGet) : base(graphics, nextAction, isLastGet)
        {
            _allItemIDs = ItemType.GetAllItemIDs();
        }

        public override bool Do()
        {
            var menuItems = new List<MenuItem>();
            foreach(int id in _allItemIDs)
            {
                menuItems.Add(new MenuItem(ItemType.GetByID(id).Name, null));
            }

            MenuOverlay menuOverlay = new MenuOverlay(250, 350, menuItems, false, this, this.graphics);

            Global.OverlayManager.SetCurrentOverlay(menuOverlay);

            return true;
        }

        public Item GetSelectedItem()
        {
            if (IsDataSet)
            {
                return ItemType.GetByID(_allItemIDs[SelectedIndex]);
            }
            else
            {
                throw new Exception("Data not set for MenuActionGetItemFromAllItems");
            }
        }
    }
}
