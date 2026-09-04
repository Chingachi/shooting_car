using System;
using System.Collections.Generic;
namespace Core.EventSystemComponents
{
    public class EventManager
    {
        private readonly Dictionary<Type, Delegate> _events = new Dictionary<Type, Delegate>();

        public void Subscribe<T> (Action<T> action)
            where T : struct, IEvent
        {
            Type type = typeof(T);

            if (!_events.ContainsKey(type))
            {
                _events[type] = null;
            }

            _events[type] = Delegate.Combine(_events[type], action);
        }

        public void Unsubscribe<T> (Action<T> action)
            where T : struct, IEvent
        {
            Type type = typeof(T);

            if (_events.TryGetValue(type, out Delegate eventValue))
            {
                eventValue = Delegate.Remove(eventValue, action);

                if (eventValue == null)
                {
                    _events.Remove(type);
                } else
                {
                    _events[type] = eventValue;
                }
            }
        }

        public void Fire<T> (T eventData)
            where T : struct, IEvent
        {
            if (_events.TryGetValue(typeof(T), out Delegate action))
            {
                ((Action<T>)action)(eventData);
            }
        }
    }
}