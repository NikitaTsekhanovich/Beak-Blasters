using System;
using System.Collections.Generic;

namespace GameControllers.PoolObjects
{
    public class PoolBase<T>
    {
        private readonly Func<T> _preloadFunc;
        private readonly Action<T> _getAction;
        private readonly Action<T> _returnAction;
        private readonly int _preloadCount;
        private readonly Queue<T> _pool = new();

        public PoolBase(Func<T> preloadFunc, Action<T> getAction, Action<T> returnAction, int preloadCount)
        {
            _preloadFunc = preloadFunc;
            _getAction = getAction;
            _returnAction = returnAction;
            _preloadCount = preloadCount;

            Spawn(preloadFunc, preloadCount);
        }

        private void Spawn(Func<T> preloadFunc, int preloadCount)
        {
            for (var i = 0; i < preloadCount; i++)
                Return(preloadFunc());
        }

        public T Get()
        {
            var item = _pool.Count > 0 ? _pool.Dequeue() : _preloadFunc();
            _getAction(item);
            
            return item;
        }

        public void Return(T item)
        {
            _returnAction(item);
            _pool.Enqueue(item);
        }
    }
}
