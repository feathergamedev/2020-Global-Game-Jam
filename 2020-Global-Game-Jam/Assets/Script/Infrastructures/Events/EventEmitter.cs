using System;
using System.Collections.Generic;
using System.Linq;

namespace Repair.Infrastructures.Events
{
    public static class EventEmitter
    {
        private static Dictionary<GameEvent, List<Action<object>>> listeners = new Dictionary<GameEvent, List<Action<object>>>();

        public static void Reset()
        {
            listeners.Clear();
        }

        public static void Add(GameEvent eventName, Action<object> action)
        {
            if (listeners.ContainsKey(eventName))
            {
                listeners[eventName].Add(action);
            }
            else
            {
                listeners[eventName] = new List<Action<object>>
                {
                    action
                };
            } 
        } 

        public static void Remove(GameEvent eventName, Action<object> action)
        {
            if (listeners.ContainsKey(eventName))
            {
                listeners[eventName].Remove(action);
                if (!listeners[eventName].Any())
                {
                    listeners.Remove(eventName);
                }
            }
        }

        public static void Emit(GameEvent eventName, object eventParams = null)
        {
            if (listeners.ContainsKey(eventName))
            {
                foreach (var action in listeners[eventName])
                {
                    action?.Invoke(eventParams);
                }
            }
        }
    }
}
