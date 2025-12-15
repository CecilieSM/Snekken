using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Models;
using Models.Repository;
using Snekken.ViewModel;

namespace UnitTest.ViewModelTests;

[TestClass]
public class BookingViewModelUC3Tests
{
    [TestMethod]
    public void UC3_Checkout_WhenAdminSetsCheckedOut_ThenRepositoryUpdateIsCalled()
    {
        // ARRANGE (forbered testdata)

        // 1) Lav en booking som om den allerede findes i systemet
        var booking = new Booking(
            id: 1,
            resourceId: 10,
            personId: 100,
            startTime: new DateTime(2025, 12, 10, 9, 0, 0),
            endTime: new DateTime(2025, 12, 10, 12, 0, 0)
        );

        // 2) Lav en person (BookingViewModel forventer at kunne finde personen)
        var person = new Person(id: 100, name: "Test", email: "test@mail.dk", phone: "12345678");

        // 3) Mock repositories (fake databaser)
        var bookingRepoMock = new Mock<IRepository<Booking>>();
        var personRepoMock = new Mock<IRepository<Person>>();
        var resourceRepoMock = new Mock<IRepository<Resource>>();
        var resourceTypeRepoMock = new Mock<IRepository<ResourceType>>();

        // 4) Fortæl mocks hvad de skal returnere når ViewModel kalder GetAll()
        bookingRepoMock.Setup(r => r.GetAll()).Returns(new List<Booking> { booking });
        personRepoMock.Setup(r => r.GetAll()).Returns(new List<Person> { person });
        resourceRepoMock.Setup(r => r.GetAll()).Returns(new List<Resource>());          
        resourceTypeRepoMock.Setup(r => r.GetAll()).Returns(new List<ResourceType>());  

        // 5) Lav ViewModel med mocks
        var vm = new BookingViewModel(
            bookingRepoMock.Object,
            personRepoMock.Object,
            resourceRepoMock.Object,
            resourceTypeRepoMock.Object
        );

        // 6) Vælg booking (som admin gør i UI)
        vm.SelectedBooking = vm.Bookings[0];

        // 7) Simuler "UC3: udlevering"
        vm.IsCheckedOut = true; // admin sætter checkbox “Udleveret”
    

        // ACT (gør det vi tester)

        // Admin trykker "Gem" (UpdateBookingCommand)
        vm.UpdateBookingCommand.Execute(null);


        // ASSERT (tjek resultat)

        // Det vigtigste: at vi prøvede at gemme booking i repository via Update(...)
        bookingRepoMock.Verify(r => r.Update(It.IsAny<Booking>()), Times.Once);

        // Bonus: tjek at booking faktisk blev markeret som udleveret (HandedOutAt sat)
        Assert.IsTrue(vm.SelectedBooking.HandedOutAt.HasValue,
            "HandedOutAt bør være sat når IsCheckedOut = true og man trykker Gem.");
    }
}
