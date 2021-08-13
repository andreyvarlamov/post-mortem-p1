using System.Collections.Generic;
using System.Linq;

using PostMortem_P1.Core;
using PostMortem_P1.Interfaces;

namespace PostMortem_P1.Systems
{
    public class SchedulingSystem
    {
        private long _totalTicks;
        private readonly SortedDictionary<long, List<IScheduleable>> _scheduleables;

        public SchedulingSystem()
        {
            _totalTicks = 0;
            _scheduleables = new SortedDictionary<long, List<IScheduleable>>();
        }

        public void Add(IScheduleable scheduleable)
        {
            long key = _totalTicks + scheduleable.Time;
            if (!_scheduleables.ContainsKey(key))
            {
                _scheduleables.Add(key, new List<IScheduleable>());
            }
            _scheduleables[key].Add(scheduleable);
        }

        public void AddMultipleDelayed(SortedDictionary<long, List<IScheduleable>> scheduleables)
        {
            foreach(KeyValuePair<long, List<IScheduleable>> kvp in scheduleables)
            {
                _scheduleables.Add(kvp.Key + _totalTicks, kvp.Value);
            }
        }

        /// <summary>
        /// Remove current scheduleables and return them as list with key being offset from current totalTicks
        /// </summary>
        /// <returns>dictionary</returns>
        public SortedDictionary<long, List<IScheduleable>> PopScheduleables()
        {
            SortedDictionary<long, List<IScheduleable>> scheduleables = new SortedDictionary<long, List<IScheduleable>>();
            for(int i = _scheduleables.Count - 1; i >= 0; i--)
            {
                KeyValuePair<long, List<IScheduleable>> kvp = _scheduleables.ElementAt(i);

                // If a player is in this list of scheduleables, keep only player
                IScheduleable player = kvp.Value.FirstOrDefault(s => s is Player);

                if (player != null)
                {
                    var scheduleablesNoPlayer = new List<IScheduleable>(kvp.Value);
                    scheduleablesNoPlayer.RemoveAll(s => s is Player);
                    scheduleables.Add(kvp.Key - _totalTicks, scheduleablesNoPlayer);

                    _scheduleables.Remove(kvp.Key);
                    var newList = new List<IScheduleable>();
                    newList.Add(player);
                    _scheduleables.Add(kvp.Key, newList);
                }
                else
                {
                    scheduleables.Add(kvp.Key - _totalTicks, kvp.Value);
                    _scheduleables.Remove(kvp.Key);
                }
            }
            return scheduleables;
        }

        public void Remove(IScheduleable scheduleable)
        {
            KeyValuePair<long, List<IScheduleable>> scheduleableListFound = new KeyValuePair<long, List<IScheduleable>>(-1, null);

            foreach (var scheduleablesList in _scheduleables)
            {
                if (scheduleablesList.Value.Contains(scheduleable))
                {
                    scheduleableListFound = scheduleablesList;
                    break;
                }
            }

            if (scheduleableListFound.Value != null)
            {
                scheduleableListFound.Value.Remove(scheduleable);
                if (scheduleableListFound.Value.Count <= 0)
                {
                    _scheduleables.Remove(scheduleableListFound.Key);
                }
            }
        }

        public IScheduleable Get()
        {
            var firstScheduleableGroup = _scheduleables.First();
            var firstScheduleable = firstScheduleableGroup.Value.First();
            Remove(firstScheduleable);
            _totalTicks = firstScheduleableGroup.Key;
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
