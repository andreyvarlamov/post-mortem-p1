using PostMortem_P1.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace PostMortem_P1.Core
{
    public class ScheduleableDictionary
    {
        private SortedDictionary<long, List<IScheduleable>> _scheduleables;

        public int Count
        {
            get
            {
                return _scheduleables.Count;
            }
            private set
            {
            }
        }

        public ScheduleableDictionary()
        {
            _scheduleables = new SortedDictionary<long, List<IScheduleable>>();
        }

        public void AddScheduleable(long ticks, IScheduleable scheduleable)
        {
            if (!_scheduleables.ContainsKey(ticks))
            {
                _scheduleables.Add(ticks, new List<IScheduleable>());
            }
            _scheduleables[ticks].Add(scheduleable);
        }

        public void AddList(long ticks, List<IScheduleable> scheduleables)
        {
            _scheduleables.Add(ticks, scheduleables);
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
        public ScheduleableDictionary PopScheduleables(long offset)
        {
            ScheduleableDictionary scheduleables = new ScheduleableDictionary();

            for (int i = Count - 1; i >= 0; i--)
            {
                KeyValuePair<long, List<IScheduleable>> kvp = _scheduleables.ElementAt(i);

                // If a player is in this list of scheduleables, keep only player
                IScheduleable player = kvp.Value.FirstOrDefault(s => s is Player);

                if (player != null)
                {
                    var scheduleablesNoPlayer = new List<IScheduleable>(kvp.Value);
                    scheduleablesNoPlayer.RemoveAll(s => s is Player);
                    scheduleables.AddList(kvp.Key - offset, scheduleablesNoPlayer);

                    _scheduleables.Remove(kvp.Key);
                    var newList = new List<IScheduleable>();
                    newList.Add(player);
                    _scheduleables.Add(kvp.Key, newList);
                }
                else
                {
                    scheduleables.AddList(kvp.Key - offset, kvp.Value);
                    _scheduleables.Remove(kvp.Key);
                }
            }
            return scheduleables;
        }

        public void Clear()
        {
            _scheduleables.Clear();
        }

        public KeyValuePair<long, List<IScheduleable>> GetFirst()
        {
            return _scheduleables.First();
        }

        public SortedDictionary<long, List<IScheduleable>> GetDictionary()
        {
            return _scheduleables;
        }
    }
}
