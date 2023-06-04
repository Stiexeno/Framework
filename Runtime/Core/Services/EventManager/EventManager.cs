using System;
using System.Collections.Generic;

namespace Framework
{
    public class EventManager : IEventManager
    {
        private readonly Dictionary<Type, List<Delegate>> eventHandlers = new();

        public void Subscribe<T>(Action<T> action) where T : IEvent
        {
            eventHandlers.GetOrAdd(typeof(T), new List<Delegate>()).Add(action);
        }

        public void Unsubscribe<T>(Action<T> action) where T : IEvent
        {
            if (eventHandlers.TryGetValue(typeof(T), out var handlers))
            {
                handlers.Remove(action);
            }
        }

        public void Invoke<T>(T e) where T : IEvent
        {
            if (eventHandlers.TryGetValue(typeof(T), out var handlers))
            {
                foreach (var handler in handlers)
                {
                    ((Action<T>)handler)(e);
                }
            }
        }
    }
}
