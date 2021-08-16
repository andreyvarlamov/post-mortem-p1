using Microsoft.Xna.Framework.Graphics;
using PostMortem_P1.Interfaces;

namespace PostMortem_P1.Core
{
    public class WakeableBlock : Block, IWakeable
    {
        #region IWakeable
        public long Time { get; }

        public bool WillWakeUp
        {
            get
            {
                // Override and return true, for children that implement WakeUp
                return false;
            }
            private set
            {
            }
        }

        public long TicksUnloadedAt { get; set; }
        public virtual void WakeUp(long ticksReloadedAt)
        {

        }
        #endregion

        #region WakeableBlock
        public WakeableBlock(int blockID, string name, Texture2D sprite, bool isAir,
            bool isWalkable, bool isTransparent, int? itemVersionID) :
            base(blockID, name, sprite, isAir, isWalkable, isTransparent, itemVersionID)
        {

        }

        public virtual void PerformAction()
        {

        }
        #endregion
    }
}
