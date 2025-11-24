using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest.ModelTests
{
    [TestClass]
    public class ResourceTests
    {
    [TestMethod]
    public void Resource_Creation_WorksCorrectly()
        {
            // Arrange
            var resourceType = new Models.ResourceType("Laptop", Models.TimeUnit.Day, "Handle with care") { Id = 1 };
            // Act
            var resource = new Models.Resource("Work Laptop", 2000, resourceType.Id, "High-end laptop for work tasks");
            // Assert
            Assert.AreEqual("Work Laptop", resource.Title);
            Assert.AreEqual(2000, resource.Price);
            Assert.AreEqual(resourceType.Id, resource.ResourceTypeId);
            Assert.AreEqual("High-end laptop for work tasks", resource.Description);
        }
    }
}
