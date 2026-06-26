using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task03
{
    public class CustomCollection<T> : IEnumerable<T>
    {
        private readonly List<T> _items = new();

        public void Add(T item) => _items.Add(item);
        public void Remove(T item) => _items.Remove(item);
        public IEnumerator<T> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerable<T> GetReverseEnumerator()
        {
            for (int i = _items.Count - 1; i >= 0; i--)
            {
                yield return _items[i];
            }
        }

        public static IEnumerable<int> GenerateSequence(int start, int count)
        {
            int current = start;
            for (int i = 0; i < count; i++)
            {
                yield return current++;
            }
        }

        public IEnumerable<T> FilterAndSort<TKey>(Func<T, bool> predicate, Func<T, TKey> keySelector) where TKey : IComparable
        {
            return _items.Where(predicate).OrderBy(keySelector);
        }
    }
}
