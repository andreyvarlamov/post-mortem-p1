using System.Collections.Generic;
using System.Linq;

using PostMortem_P1.Core;
using PostMortem_P1.Interfaces;

namespace PostMortem_P1.Systems
{
    public class SchedulingSystem
    {
        private long _totalTicks;
        private ScheduleableDictionary _scheduleables;

        public SchedulingSystem()
        {
            _totalTicks = 0;
            _scheduleables = new ScheduleableDictionary();
        }

        public void AddNext(IScheduleable scheduleable)
        {
            long key = _totalTicks + scheduleable.Time;
            _scheduleables.AddScheduleable(key, scheduleable);
        }

        public void AddMultipleDelayed(ScheduleableDictionary scheduleables)
        {
            foreach(KeyValuePair<long, List<IScheduleable>> kvp in scheduleables.GetDictionary())
            {
                _scheduleables.AddList(kvp.Key + _totalTicks, kvp.Value);
            }
            scheduleables.Clear();
        }

        /// <summary>
        /// Remove current scheduleables and return them as list with key being offset from current totalTicks
        /// </summary>
        /// <returns>dictionary</returns>
        public ScheduleableDictionary PopScheduleables()
        {
            return _scheduleables.PopScheduleables(_totalTicks);
        }

        public void Remove(IScheduleable scheduleable)
        {
            _scheduleables.Remove(scheduleable);
        }

        public IScheduleable PopFirst()
        {
            var scheduleableGroupKvp = _scheduleables.GetFirst();
            var firstScheduleable = scheduleableGroupKvp.Value.First();
            Remove(firstScheduleable);
            _totalTicks = scheduleableGroupKvp.Key;
            return firstScheduleable;
        }

        public long GetTimeTicks()
        {
            return _totalTicks;
        }

        public float GetTimeTurns()
        {
            return _totalTicks / 100.0f;
        }

        public void Clear()
        {
            _totalTicks = 0;
            _scheduleables.Clear();
        }
    }
}
