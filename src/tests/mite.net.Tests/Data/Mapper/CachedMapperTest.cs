using System;
using FakeItEasy;
using FluentAssertions;
using Mite;
using NUnit.Framework;

namespace mite.Tests.Data.Mapper
{
    [TestFixture]
    public class CachedMapperTest
    {
        [SetUp]
        public void Setup()
        {
            _mapper = A.Fake<IDataMapper<MockWithId>>();
            _sut = new CachedMapper<MockWithId>(_mapper, mock => mock.Id, TimeSpan.FromMilliseconds(10000));
        }

        private CachedMapper<MockWithId> _sut;
        private IDataMapper<MockWithId> _mapper;

        public class MockWithId
        {
            public MockWithId() {}

            public MockWithId(int id)
            {
                Id = id;
            }

            public int Id { get; }
        }

        [Test]
        public void CreateShouldCallCreateInMapper()
        {
            var value = new MockWithId(1);
            var wasInvoked = false;
            A.CallTo(() => _mapper.Create(value))
                .Invokes(p => wasInvoked = true)
                .Returns(value);

            _sut.Create(value);

            wasInvoked.ShouldBeEquivalentTo(true, "calls to Create() should relay call to mapper");
        }

        [Test]
        public void DeleteShouldCallDeleteInMapper()
        {
            var value = new MockWithId(1);
            var wasInvoked = false;
            A.CallTo(() => _mapper.Delete(value)).Invokes(p => wasInvoked = true);

            _sut.Delete(value);

            wasInvoked.ShouldBeEquivalentTo(true, "calls to Delete() should relay call to mapper");
        }

        [Test]
        public void GetAllShouldCallGetAllInMapper()
        {
            var items = new[] {new MockWithId(1), new MockWithId(2)};
            var wasInvoked = false;
            A.CallTo(() => _mapper.GetAll())
                .Invokes(p => wasInvoked = true)
                .Returns(items);

            var retrievedItems = _sut.GetAll();

            wasInvoked.ShouldBeEquivalentTo(true, "calls to GetAll() should relay call to mapper");
            retrievedItems.Count.ShouldBeEquivalentTo(2, "because mapper return two items");
        }

        [Test]
        public void GetByIdShouldCallGetByIdInMapperIfValueIsNotPresentInCache()
        {
            var wasInvoked = false;
            A.CallTo(() => _mapper.GetById(1))
                .Invokes(p => wasInvoked = true)
                .Returns(new MockWithId(1));

            _sut.GetById(1);

            wasInvoked.ShouldBeEquivalentTo(true, "Id is not present in cache");
        }

        [Test]
        public void GetByIdShouldNotCallGetByIdInMapperWhenValueIsPresentInCache()
        {
            var mock = new MockWithId(1);
            var mapperWasCalled = false;
            A.CallTo(() => _mapper.Create(mock)).Returns(mock);
            A.CallTo(() => _mapper.GetById(mock.Id)).Invokes(p => mapperWasCalled = true);
            var createdValue = _sut.Create(mock); // now value should be in cache

            var retrievedValue = _sut.GetById(mock.Id); //should not call mapper

            mapperWasCalled.ShouldBeEquivalentTo(false, "Id is present in cache");
            createdValue.ShouldBeEquivalentTo(mock);
            retrievedValue.ShouldBeEquivalentTo(mock, "this was the value added via Create");
        }

        [Test]
        public void UpdateShouldCallUpdateInMapper()
        {
            var mock = new MockWithId(1);
            var wasInvoked = false;
            A.CallTo(() => _mapper.Update(mock))
                .Invokes(p => wasInvoked = true)
                .Returns(mock);

            _sut.Update(mock);

            wasInvoked.ShouldBeEquivalentTo(true, "calls to Update() should relay call to mapper");
        }
    }
}