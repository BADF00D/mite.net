using NUnit.Framework;

namespace Mite.Tests
{
    using System;
    using Data.Factory;

    [TestFixture]
    public class ProjectProxyTests
    {
        private MiteDataMapperFactory _factory;
        [SetUp]
        public void SetUp()
        {
            _factory = new MiteDataMapperFactory(new MiteConfiguration(new Uri("https://test"), string.Empty));
        }


        [Test]
        public void ProjectShouldBeEqualToProxy()
        {
            Project project = new Project
            {
                Id = 3,
                Name = "Test"
            };

            Project projectProxy = new ProjectProxy(_factory)
            {
                Name = "Test",
                Id = 3
            };

            Assert.IsTrue(projectProxy.Equals(project));
        }

        [Test]
        public void ProxyShouldBeEqualToProject()
        {
            Project project = new Project
            {
                Id = 3,
                Name = "Test"
            };

            Project projectProxy = new ProjectProxy(_factory)
            {
                Name = "Test",
                Id = 3
            };

            Assert.IsTrue(project.Equals(projectProxy));
        }

        [Test]
        public void ShouldBeEqualForSameInstance()
        {
            Project project = new Project();
            project.Name = "Test";

            Assert.IsTrue(project.Equals(project));
        }

        [Test]
        public void GetHashCodeShouldReturnSameValueForIdenticalObject()
        {
            Project p1 = new ProjectProxy(_factory);
            p1.Id = 1;

            Project p2 = new ProjectProxy(_factory);
            p2.Id = 1;

            int p1Hash = p1.GetHashCode();
            int p2Hash = p2.GetHashCode();
            
            Assert.AreEqual(p1Hash,p2Hash);
        }
    }
}