using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;

using PostMortem_P1.Core;

namespace PostMortem_P1.Menus
{
    public class MenuActionGet : MenuAction
    {
        public bool IsDataSet { get; protected set; }
        public object data { get; protected set; }

        protected MenuAction nextAction;

        protected bool isLastGet;

        public MenuActionGet(GraphicsDeviceManager graphics, MenuAction nextAction, bool isLastGet) : base(graphics)
        {
            this.nextAction = nextAction;
            this.isLastGet = isLastGet;
        }

        public override bool Do()
        {
            return true;
        }

        public virtual void SetData(object data)
        {
            this.data = data;
            IsDataSet = true;

            if (isLastGet)
            {
                Global.OverlayManager.ReturnToGame();
            }

            nextAction.Do();
        }

    }
}
