using System;
using System.Collections.Generic;

namespace Mite.Data
{
    internal class Cache<T>
    {
        private readonly TimeSpan _timeItemsShouldBeCached;
        private Dictionary<object, ItemWithDateTimeTag<T>> _cache = new Dictionary<object, ItemWithDateTimeTag<T>>();

        public Cache(TimeSpan timeItemsShouldBeCached)
        {
            _timeItemsShouldBeCached = timeItemsShouldBeCached;
        }

        public void AddorUpdate(int id, T item)
        {
            if (_cache.ContainsKey(id))
            {
                _cache.Remove(id);
            }
            _cache.Add(id,new ItemWithDateTimeTag<T>{Item = item});
        }

        public void Remove(int id)
        {
            if(_cache.ContainsKey(id))
                _cache.Remove(id);
        }

        public void Clear()
        {
            _cache.Clear();
        }

        public bool TryGet(int id, out T item)
        {
            item = default(T);
            if (!_cache.ContainsKey(id)) return false;

            var taggedItem = _cache[id];

            var elapsedTime = (DateTimeOffset.Now - taggedItem.AddedAt);
            if (elapsedTime > _timeItemsShouldBeCached)
            {
                _cache.Remove(id);
                return false;
            };
            
            item = taggedItem.Item;
            return true;
        }


        private class ItemWithDateTimeTag<T>
        {
            public DateTimeOffset AddedAt { get; private set; }

            public T Item { get; set; }

            public ItemWithDateTimeTag()
            {
                AddedAt = DateTimeOffset.Now;
            }
        }
    }
}
