using Mono.Cecil;
using Moq;
using System.Security.AccessControl;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models.Repository;
using Snekken.ViewModel;


namespace UnitTest.ViewModelTests
{
    [TestClass]
    public class ResourceViewModelTests
    {
        [TestMethod]
        public void Constructor_LoadResourceTypes()
        {
            // Arrange
            var mockResourceRepo = new Mock<IRepository<Models.Resource>>();
            var mockTypeRepo = new Mock<IRepository<Models.ResourceType>>();

            mockTypeRepo
                .Setup(r => r.GetAll())
                .Returns(new List<Models.ResourceType> {
                new Models.ResourceType("Hammer", Models.TimeUnit.Hour, "Wear helmet")
                });

            // Act
            var vm = new ResourceViewModel(mockResourceRepo.Object, mockTypeRepo.Object);

            // Assert
            Assert.AreEqual(1, vm.ResourceTypes.Count);
            Assert.AreEqual("Hammer", vm.ResourceTypes[0].Title);
        }

        //[TestMethod]
        //public void Constructor_ShowsMessage_WhenResourceTypeLoadFails()
        //{
        //    // Arrange
        //    var mockResourceRepo = new Mock<IResourceRepository>();
        //    var mockTypeRepo = new Mock<IResourceTypeRepository>();

        //    mockTypeRepo
        //        .Setup(r => r.GetAll())
        //        .Throws(new Exception());

        //    // Act
        //    var vm = new ResourceViewModel(mockResourceRepo.Object, mockTypeRepo.Object);

        //    // Assert: messageService.Show() was called once
        //    mockMessage.Verify(m => m.Show(It.IsAny<string>()), Times.Once);
        //}




        [TestMethod]
        public void AddResource_CallsRepositoryAdd_WithCorrectResource()
        {
            // Arrange
            var mockResourceRepo = new Mock<IRepository<Models.Resource>>();
            var mockTypeRepo = new Mock<IRepository<Models.ResourceType>>();

            mockTypeRepo.Setup(r => r.GetAll()).Returns(new List<Models.ResourceType>());

            var vm = new ResourceViewModel(mockResourceRepo.Object, mockTypeRepo.Object);

            vm.ResourceFormTitle = "Laptop";
            vm.ResourceFormUnitPrice = 2500;
            vm.ResourceFormType = new Models.ResourceType("Electronics", Models.TimeUnit.Hour, "Handle with care") { Id = 5 };
            vm.ResourceFormDescription = "Dell XPS";

            // Act
            vm.AddResourceCommand.Execute(null);

            // Assert
            mockResourceRepo.Verify(r => r.Add(It.Is<Models.Resource>(res =>
                res.Title == "Laptop" &&
                res.Price == 2500 &&
                res.ResourceTypeId == 5 &&
                res.Description == "Dell XPS"
            )), Times.Once);
        }


        //[TestMethod]
        //public void AddResource_ShowsMessage_WhenRepositoryThrows()
        //{
        //    // Arrange
        //    var mockResourceRepo = new Mock<IResourceRepository>();
        //    var mockTypeRepo = new Mock<IResourceTypeRepository>();
        //    var mockMessage = new Mock<IMessageService>();

        //    mockTypeRepo.Setup(r => r.GetAll()).Returns(new List<Models.ResourceType>());
        //    mockResourceRepo.Setup(r => r.Add(It.IsAny<Models.Resource>())).Throws(new Exception());

        //    var vm = new ResourceViewModel(mockResourceRepo.Object, mockTypeRepo.Object, mockMessage.Object);

        //    // Act
        //    vm.AddResourceCommand.Execute(null);

        //    // Assert
        //    mockMessage.Verify(m => m.Show(It.IsAny<string>()), Times.Once);
        //}

        [TestMethod]
        public void CanAddResource_AlwaysReturnsTrue()
        {
            var mockResourceRepo = new Mock<IRepository<Models.Resource>>();
            var mockTypeRepo = new Mock<IRepository<Models.ResourceType>>();

            mockTypeRepo.Setup(r => r.GetAll()).Returns(new List<Models.ResourceType>());

            var vm = new ResourceViewModel(mockResourceRepo.Object, mockTypeRepo.Object);

            Assert.IsTrue(vm.AddResourceCommand.CanExecute(null));
        }

        [TestMethod]
        public void ResourceFormProperties_CanBeSetAndGet()
        {
            //Arrange
            var mockResourceRepo = new Mock<IRepository<Models.Resource>>();
            var mockTypeRepo = new Mock<IRepository<Models.ResourceType>>();
            mockTypeRepo.Setup(r => r.GetAll()).Returns(new List<Models.ResourceType>());

            //Act
            var vm = new ResourceViewModel(mockResourceRepo.Object, mockTypeRepo.Object);
            vm.ResourceFormTitle = "Drill";
            vm.ResourceFormUnitPrice = 150;
            var resourceType = new Models.ResourceType("Tool", Models.TimeUnit.Hour, "Wear gloves") { Id = 3 };
            vm.ResourceFormType = resourceType;
            vm.ResourceFormDescription = "Electric drill";
            vm.TypeFormTitle = "Tool";
            vm.TypeFormUnit = Models.TimeUnit.Hour;
            vm.TypeFormRequirement = "Wear gloves";

            //Assert
            Assert.AreEqual("Drill", vm.ResourceFormTitle);
            Assert.AreEqual(150, vm.ResourceFormUnitPrice);
            Assert.AreEqual(resourceType, vm.ResourceFormType);
            Assert.AreEqual("Electric drill", vm.ResourceFormDescription);
            Assert.AreEqual("Tool", vm.TypeFormTitle);
            Assert.AreEqual(Models.TimeUnit.Hour, vm.TypeFormUnit);
            Assert.AreEqual("Wear gloves", vm.TypeFormRequirement);
        }
    }
}

