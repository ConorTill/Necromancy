using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Inventory
{
    public interface IObservableArray<T>
    {
        event Action<T[]> AnyValueChanged;

        int Size { get; }
        T Get(int index);
        
        void ReplaceAt(int index, T item);
        void ReplaceAll(T[] items);
        bool TryAdd(T item);
        IEnumerable<T> GetAll(Func<T, bool> predicate = null);
    }

    [Serializable]
    public class ObservableArray<T> : IObservableArray<T>
    {
        private T[] _items;

        public event Action<T[]> AnyValueChanged = delegate { };

        public ObservableArray(int size, IList<T> initialList = null)
        {
            _items = new T[size];
            if (initialList == null)
                return;

            initialList.Take(size).ToArray().CopyTo(_items, 0);
            Invoke();
        }

        private void Invoke() => AnyValueChanged.Invoke(_items);

        public int Count => _items.Count(i => i != null);
        public int Size => _items.Length;
        public int IndexOf(T item) => Array.IndexOf(_items, item);
        public T this[int index] => _items[index];

        public void Swap(int index1, int index2)
        {
            (_items[index1], _items[index2]) = (_items[index2], _items[index1]);
            Invoke();
        }

        public T Get(int index) => _items[index];

        public void ReplaceAt(int index, T item)
        {
            _items[index] = item;
            Invoke();
        }

        public void ReplaceAll(T[] items)
        {
            _items = items;
            Invoke();
        }

        public bool Contains(T item) => _items.Contains(item);

        public IEnumerable<T> GetAll(Func<T, bool> predicate = null) =>
            predicate is null ? _items : _items.Where(predicate);

        public bool TryAdd(T item)
        {
            var itemAdded = false;
            var i = 0;
            while (!itemAdded && i < _items.Length)
            {
                if (_items[i] == null)
                {
                    _items[i] = item;
                    itemAdded = true;
                }

                i++;
            }

            if (!itemAdded)
                return false;

            Invoke();
            return true;
        }

        public bool TryAdd(IEnumerable<T> itemsToAdd)
        {
            var tempItems = _items.ToArray();
            foreach (var item in itemsToAdd)
            {
                var itemAdded = false;
                var i = 0;
                while (!itemAdded && i < tempItems.Length)
                {
                    if (_items[i] == null)
                    {
                        tempItems[i] = item;
                        itemAdded = true;
                    }

                    i++;
                }

                if (!itemAdded)
                    return false;
            }

            _items = tempItems;

            Invoke();
            return true;
        }

        public bool TryRemove(T item)
        {
            var itemRemoved = false;
            var i = 0;
            while (!itemRemoved && i < _items.Length)
            {
                if (EqualityComparer<T>.Default.Equals(_items[i], item))
                {
                    _items[i] = default;
                    itemRemoved = true;
                }

                i++;
            }

            if (!itemRemoved)
                return false;

            Invoke();
            return true;
        }

        public bool TryRemove(IEnumerable<T> itemsToRemove)
        {
            var tempItems = _items.ToArray();
            foreach (var item in itemsToRemove)
            {
                var itemRemoved = false;
                var i = 0;
                while (!itemRemoved && i < tempItems.Length)
                {
                    if (EqualityComparer<T>.Default.Equals(tempItems[i], item))
                    {
                        tempItems[i] = default;
                        itemRemoved = true;
                    }

                    i++;
                }

                if (!itemRemoved)
                    return false;
            }

            _items = tempItems;

            Invoke();
            return true;
        }

        public void Clear()
        {
            _items = new T[_items.Length];
            Invoke();
        }
    }
}