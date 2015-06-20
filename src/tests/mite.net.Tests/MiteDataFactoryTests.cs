using System;
using NUnit.Framework;

namespace Mite.Tests
{
    using Data.Factory;

    [TestFixture]
    [Category("MapperFactory")]
    public class MiteDataFactoryTests
    {

        private MiteDataMapperFactory _factory;
        [SetUp]
        public void Setup()
        {
            _factory = new MiteDataMapperFactory(new MiteConfiguration(new Uri("https://test"), string.Empty));
        }
        
        [Test]
        [ExpectedException(typeof(NotImplementedException))]
        public void ThrowExceptionForUnkownType()
        {
            IDataMapper<object> mapper = _factory.GetMapper<object>();
        }

        [Test]
        public void GetMapperForUser()
        {
            IDataMapper<User> mapper = _factory.GetMapper<User>();

            Assert.IsInstanceOfType(typeof(IDataMapper<User>),mapper);
        }

        [Test]
        public void GetMapperForTimeEntry()
        {
            IDataMapper<TimeEntry> mapper = _factory.GetMapper<TimeEntry>();

            Assert.IsInstanceOfType(typeof(IDataMapper<TimeEntry>), mapper);
        }

        [Test]
        public void GetMapperForService()
        {
            IDataMapper<Service> mapper = _factory.GetMapper<Service>();

            Assert.IsInstanceOfType(typeof(IDataMapper<Service>), mapper);
        }

        [Test]
        public void GetMapperForProject()
        {
            IDataMapper<Project> mapper = _factory.GetMapper<Project>();

            Assert.IsInstanceOfType(typeof(IDataMapper<Project>), mapper);
        }

        [Test]
        public void GetMapperForCustomer()
        {
            IDataMapper<Customer> mapper = _factory.GetMapper<Customer>();

            Assert.IsInstanceOfType(typeof(IDataMapper<Customer>), mapper);
        }

        [Test]
        public void GetMapperForTimer()
        {
            IDataMapper<Timer> mapper = _factory.GetMapper<Timer>();

            Assert.IsInstanceOfType(typeof(IDataMapper<Timer>), mapper);
        }
    }
}