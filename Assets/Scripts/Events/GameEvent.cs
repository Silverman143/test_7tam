using System;

namespace com.Test_7tam
{
    public class GameEvent
    {
        private event Action action = delegate { };

        public void Publish()
        {
            action?.Invoke();
        }

        public void Add(Action subscriber)
        {
            action += subscriber;
        }

        public void Remove(Action subscriber)
        {
            action -= subscriber;
        }
    }

    public class GameEvent<T>
    {
        private event Action<T> action;

        public void Publish(T param)
        {
            action?.Invoke(param);
        }

        public void Add(Action<T> subscriber)
        {
            action += subscriber;
        }

        public void Remove(Action<T> subscriber)
        {
            action -= subscriber;
        }
    }

    public class GameEvent<T, Q>
    {
        private event Action<T, Q> action;

        public void Publish(T param0, Q param1)
        {
            action?.Invoke(param0, param1);
        }

        public void Add(Action<T, Q> subscriber)
        {
            action += subscriber;
        }

        public void Remove(Action<T, Q> subscriber)
        {
            action -= subscriber;
        }
    }
}
