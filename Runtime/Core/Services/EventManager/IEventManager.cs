using System;

namespace Framework
{
    public interface IEventManager
    {
        void Subscribe<T>(Action<T> action) where T : IEvent;
        void Unsubscribe<T>(Action<T> action) where T : IEvent;
        void Invoke<T>(T e) where T : IEvent;
    }
}
