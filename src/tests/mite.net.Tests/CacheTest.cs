using System;
using System.Threading;
using FluentAssertions;
using Mite.Data;
using NUnit.Framework;

namespace mite.Tests
{
    [TestFixture]
    public class CacheTest
    {
        [SetUp]
        public void Setup()
        {
            _sut = new Cache<int>(TimeSpan.FromMilliseconds(CacheTimeoutInMillis));
        }

        private Cache<int> _sut;

        private const int Id1 = 0;
        private const int Id2 = 1;

        private const int CacheTimeoutInMillis = 10;

        [Test]
        public void AddorUpdateShouldReplaceValue()
        {
            int value;
            _sut.AddorUpdate(Id1, 1);
            _sut.AddorUpdate(Id1, 2);
            var existsValue = _sut.TryGet(Id1, out value);

            existsValue.ShouldBeEquivalentTo(true, "it was added and updated");
            value.ShouldBeEquivalentTo(2, "second call to AddOrUpdate set this value to 2");
        }

        [Test]
        public void ClearShouldRemoveAllValues()
        {
            int value;
            _sut.AddorUpdate(Id1, 1);
            _sut.Clear();
            var existsValue = _sut.TryGet(Id1, out value);

            existsValue.ShouldBeEquivalentTo(false, "clear deleted all values from cache.");
        }

        [Test]
        public void RemoveShouldRemoveASingleValue()
        {
            int value;
            _sut.AddorUpdate(Id1, 1);
            _sut.AddorUpdate(Id2, 2);
            _sut.Remove(Id1);
            var existsValueWithId1 = _sut.TryGet(Id1, out value);
            var existsValueWithId2 = _sut.TryGet(Id2, out value);

            existsValueWithId1.ShouldBeEquivalentTo(false, "it was deleted");
            existsValueWithId2.ShouldBeEquivalentTo(true, "it wasn't deleted");
        }

        [Test]
        public void TryGetShouldReturnFalseAfterCacheTimeout()
        {
            int value;
            _sut.AddorUpdate(Id1, 1);
            //I currently dont know a better way. Using Scheduler from ReactiveExtension whould solve this ugly hack.
            Thread.Sleep(CacheTimeoutInMillis + 1);
                
            var existsValue = _sut.TryGet(Id1, out value);

            existsValue.ShouldBeEquivalentTo(false, "it is to old");
        }

        [Test]
        public void TryGetShouldReturnFalseWhileCacheIsEmpty()
        {
            int value;
            var existsValue = _sut.TryGet(0, out value);

            existsValue.ShouldBeEquivalentTo(false, "there is no item in cache");
        }

        [Test]
        public void TryGetShouldReturnTrueWhenValueisInCache()
        {
            int value;
            _sut.AddorUpdate(Id1, 1);
            var existsValue = _sut.TryGet(Id1, out value);

            existsValue.ShouldBeEquivalentTo(true, "item was added in call before");
            value.ShouldBeEquivalentTo(1, "because the was the value added to cache");
            Assert.IsTrue(existsValue);
            Assert.AreEqual(1, value);
        }
    }
}