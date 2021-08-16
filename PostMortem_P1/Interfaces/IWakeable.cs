using System;
using System.Collections.Generic;
using System.Text;

namespace PostMortem_P1.Interfaces
{
    public interface IWakeable : IScheduleable
    {
        bool WillWakeUp { get; }
        long TicksUnloadedAt { get; set; }
        void WakeUp(long ticksReloadedAt);
    }
}
