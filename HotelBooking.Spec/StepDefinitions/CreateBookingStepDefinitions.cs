using System;
using System.Collections.Generic;
using HotelBooking.Core;
using Moq;
using Reqnroll;
using Xunit;

namespace HotelBooking.Spec.StepDefinitions;


[Binding]
public class CreateBookingStepDefinitions
{

    private BookingManager bookingManager;
    private Mock<IRepository<Booking>> bookingRepositoryMock;
    private Mock<IRepository<Room>> roomRepositoryMock;
    private Booking booking;
    private bool bookingResult;
    
    [BeforeScenario]
    public void Setup()
    {
        bookingRepositoryMock = new Mock<IRepository<Booking>>();
        roomRepositoryMock = new Mock<IRepository<Room>>();
        bookingManager = new BookingManager(bookingRepositoryMock.Object, roomRepositoryMock.Object);
        booking = new Booking(); // Initialize the booking object here
    }

    [Given(@"the start date is (.*) days in the future from today")]
    public void GivenTheStartDateIsDaysInTheFuture(int days)
    {
        var startingDate = DateTime.Today.AddDays(days);
        booking.StartDate = startingDate;
    }
    
    [Given(@"the end date is (.*) days in the future from today")]
    public void GivenTheEndDateIsDaysInTheFuture(int days)
    {
        var endDate = DateTime.Today.AddDays(days);
        booking.EndDate = endDate;
    }

    [Given(@"there is a room available during period")] 
    public void ThereIsAnAvailableRoomDuringPeriod()
    {
        // Mock room availability here (example)
        roomRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Room> { new Room { Id = 1 } });
    }
    
    [When(@"the customer creates a booking")]
    public void WhenTheCustomerCreatesABooking()
    {
        bookingResult = bookingManager.CreateBooking(booking);
    }

    [Then(@"the booking should be created successfully")]
    public void ThenTheBookingShouldBeCreatedSuccessfully()
    {
        Assert.True(bookingResult);
    }

}