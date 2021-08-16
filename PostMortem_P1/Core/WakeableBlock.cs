using Microsoft.Xna.Framework.Graphics;
using PostMortem_P1.Interfaces;

namespace PostMortem_P1.Core
{
    public class WakeableBlock : Block, IWakeable
    {
        #region IWakeable
        public virtual long Time { get; }

        public virtual bool WillWakeUp
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
        public virtual bool WakeUp(long ticksReloadedAt)
        {
            return false;
        }
        #endregion

        #region WakeableBlock
        public int X { get; set; }
        public int Y { get; set; }

        public WakeableBlock(int blockID, string name, Texture2D sprite, bool isAir,
            bool isWalkable, bool isTransparent, int? itemVersionID, int? buildTime, int x, int y) :
            base(blockID, name, sprite, isAir, isWalkable, isTransparent, itemVersionID, buildTime)
        {
            X = x;
            Y = y;
        }

        public virtual void PerformAction()
        {

        }
        #endregion
    }
}
