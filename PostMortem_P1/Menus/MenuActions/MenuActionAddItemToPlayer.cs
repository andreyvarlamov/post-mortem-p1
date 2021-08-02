using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;
using PostMortem_P1.Menus.Overlays;

namespace PostMortem_P1.Menus.MenuActions
{
    public class MenuActionAddItemToPlayer : MenuAction
    {
        private MenuActionGetItemFromAllItems _itemAction;

        public MenuActionAddItemToPlayer(GraphicsDeviceManager graphics) : base(graphics)
        {
        }

        public void SetItemAction(MenuActionGetItemFromAllItems itemAction)
        {
            _itemAction = itemAction;
        }

        public override bool Do()
        {
            Item item = _itemAction.GetSelectedItem();

            Global.WorldMap.Player.AddToInventory(item);

            return true;
        }
    }
}
