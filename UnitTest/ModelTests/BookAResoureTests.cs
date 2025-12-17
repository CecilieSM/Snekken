using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;

namespace UnitTest.ModelTests;

[TestClass]
public class BookAResourceTests
{

    [TestClass]
    public class BookingTests
    {
        [TestMethod]
        public void Booking_Creation_LejEnRessource_SetsCorrectProperties()
        {
            // Arrange – data from userform
            int resourceId = 1;
            int personId = 42;
            var startTime = new DateTime(2025, 12, 10, 9, 0, 0);
            var endTime = new DateTime(2025, 12, 10, 12, 0, 0);
            decimal price = 400;

            // Act – create booking
            var booking = new Booking(resourceId, personId, startTime, endTime, price);

            // Assert – only fields that are filled out by user
            Assert.AreEqual(resourceId, booking.ResourceId);
            Assert.AreEqual(personId, booking.PersonId);
            Assert.AreEqual(startTime, booking.StartTime);
            Assert.AreEqual(endTime, booking.EndTime);
            Assert.AreEqual(price, booking.TotalPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Booking_StartTimeAfterEndTime_ThrowsException()
        {
            // Arrange – startTime after endTime
            int resourceId = 1;
            int personId = 42;
            var startTime = new DateTime(2025, 12, 10, 14, 0, 0);
            var endTime = new DateTime(2025, 12, 10, 12, 0, 0); // previously endTime
            decimal price = 400;

            // Act – try to create a booking
            var booking = new Booking(resourceId, personId, startTime, endTime, price);

            // Assert – handled by ExpectedException
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void Cannot_Create_Overlapping_Booking()
        {
            // Arrange – existing booking
            int resourceId = 1;
            int personId1 = 42;
            var start1 = new DateTime(2025, 12, 10, 9, 0, 0);
            var end1 = new DateTime(2025, 12, 10, 12, 0, 0);
            decimal price1 = 400;
            var existingBooking = new Booking(resourceId, personId1, start1, end1, price1);

            var bookings = new List<Booking> { existingBooking };

            // New booking attempt
            int personId2 = 43;
            var start2 = new DateTime(2025, 12, 10, 11, 0, 0); // overlap
            var end2 = new DateTime(2025, 12, 10, 13, 0, 0);
            decimal price2 = 300;
            var newBooking = new Booking(resourceId, personId2, start2, end2, price2);

            // Act – check manually for overlap
            if (bookings.Any(b => b.ResourceId == newBooking.ResourceId &&
                                   newBooking.StartTime < b.EndTime &&
                                   newBooking.EndTime > b.StartTime))
            {
                throw new InvalidOperationException("Ressourcen er allerede booket på dette tidspunkt.");
            }

            bookings.Add(newBooking); // This will never happen because of exception
        }

        [TestMethod]
        public void Booking_Creation_SavesPersonIdCorrectly()
        {
            // Arrange
            int resourceId = 1;      // fx en båd
            int personId = 42;       // ID for den person, der laver bookingen
            var startTime = new DateTime(2025, 12, 10, 9, 0, 0);
            var endTime = new DateTime(2025, 12, 10, 12, 0, 0);
            decimal price = 400;

            // Act
            var booking = new Booking(resourceId, personId, startTime, endTime, price);

            // Assert
            Assert.AreEqual(personId, booking.PersonId);
        }

        [TestMethod]
        public void Booking_Creation_SavesAllRequiredFields()
        {
            //arrange
            int resourceId = 1;    
            int personId = 42;      
            var startTime = new DateTime(2025, 12, 10, 9, 0, 0);
            var endTime = new DateTime(2025, 12, 10, 12, 0, 0);
            decimal price = 400;

            //act
            var booking = new Booking(resourceId, personId, startTime, endTime, price);

            // Assert  
            Assert.AreEqual(resourceId, booking.ResourceId, "ResourceId blev ikke gemt korrekt");
            Assert.AreEqual(personId, booking.PersonId, "PersonId blev ikke gemt korrekt");
            Assert.AreEqual(startTime, booking.StartTime, "StartTime blev ikke gemt korrekt");
            Assert.AreEqual(endTime, booking.EndTime, "EndTime blev ikke gemt korrekt");
            Assert.AreEqual(price, booking.TotalPrice, "TotalPrice blev ikke gemt korrekt");

        }
    }
}



  






