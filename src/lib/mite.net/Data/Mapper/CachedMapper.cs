namespace Mite
{
    using System;
    using System.Collections.Generic;
    using Data;

    internal class CachedMapper<T> : IDataMapper<T> where T : class, new()
    {
        private readonly IDataMapper<T> _realMapper;
        private readonly Func<T,int> _idSelector;
        private readonly Cache<T> _cache;

        public CachedMapper(IDataMapper<T> realMapper, Func<T,int> idSelector, TimeSpan timeItemsShouldBeCached)
        {
            _realMapper = realMapper;
            _idSelector = idSelector;
            _cache = new Cache<T>(timeItemsShouldBeCached);
        }

        public T Create(T item)
        {
            var itemFromServer = _realMapper.Create(item);
            _cache.AddorUpdate(_idSelector(item), itemFromServer);

            return itemFromServer;
        }

        public T Update(T item)
        {
            var itemFromServer = _realMapper.Update(item);
            _cache.AddorUpdate(_idSelector(itemFromServer), itemFromServer);

            return itemFromServer;
        }

        public void Delete(T item)
        {
            _cache.Remove(_idSelector(item));
            _realMapper.Delete(item);
        }

        public T GetById(object id)
        {
            T itemFromServer;
            if (_cache.TryGet(Int32.Parse(id.ToString()), out itemFromServer))
            {
                return itemFromServer;
            }
            itemFromServer = _realMapper.GetById(id);
            _cache.AddorUpdate(_idSelector(itemFromServer), itemFromServer);

            return itemFromServer;
        }

        public IList<T> GetAll()
        {
            _cache.Clear();
            var itemsFromServer = _realMapper.GetAll();
            foreach (var item in itemsFromServer)
            {
                _cache.AddorUpdate(_idSelector(item), item);
            }

            return itemsFromServer;
        }

        public IList<string> CriteriaProperties { get { return _realMapper.CriteriaProperties; } }
        public IList<T> GetByCriteria(QueryExpression queryExpression)
        {
            var itemsFromServer = _realMapper.GetByCriteria(queryExpression);
            foreach (var item in itemsFromServer)
            {
                _cache.AddorUpdate(_idSelector(item), item);
            }

            return itemsFromServer;
        }
    }
}