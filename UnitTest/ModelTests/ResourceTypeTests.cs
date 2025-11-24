using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ModelTests
{
    [TestClass]
    public class ResourceTypeTests
    {
        [TestMethod]


        public void ResourceType_Creation_WorksCorrectly()
        {
            // Arrange & Act
            var resourceType = new Models.ResourceType("Projector", Models.TimeUnit.Hour, "Handle with care");
            // Assert
            Assert.AreEqual("Projector", resourceType.Title);
            Assert.AreEqual(Models.TimeUnit.Hour, resourceType.Unit);
            Assert.AreEqual("Handle with care", resourceType.Requirement);
        }

        [TestMethod]
        public void ResourceType_Creation_WithId_WorksCorrectly()
        {
            // Arrange & Act
            var resourceType = new Models.ResourceType("Camera", Models.TimeUnit.Day, 5, "Use tripod");
            // Assert
            Assert.AreEqual(5, resourceType.Id);
            Assert.AreEqual("Camera", resourceType.Title);
            Assert.AreEqual(Models.TimeUnit.Day, resourceType.Unit);
            Assert.AreEqual("Use tripod", resourceType.Requirement);
        }
    }
}
