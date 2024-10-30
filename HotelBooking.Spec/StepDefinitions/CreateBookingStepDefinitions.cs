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
    private Exception bookingException;
    
    [BeforeScenario]
    public void Setup()
    {
        bookingRepositoryMock = new Mock<IRepository<Booking>>();
        roomRepositoryMock = new Mock<IRepository<Room>>();
        bookingManager = new BookingManager(bookingRepositoryMock.Object, roomRepositoryMock.Object);
        booking = new Booking();
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
    
    [Given(@"the start date is (\d+) days in the past from today")]
    public void GivenTheStartDateIsDaysInThePast(int days)
    {
        booking.StartDate = DateTime.Today.AddDays(-days);
    }
    
    [Given(@"the end date is (\d+) days in the past from today")]
    public void GivenTheEndDateIsDaysInThePast(int days)
    {
        booking.EndDate = DateTime.Today.AddDays(-days);
    }

    [Given(@"there are (\d+) rooms available during period")]
    public void GivenThereAreRoomsAvailableDuringPeriod(int availableRooms)
    {
        // Create a list of rooms based on the availableRooms count
        var rooms = new List<Room>();
        for (int i = 1; i <= availableRooms; i++)
        {
            rooms.Add(new Room { Id = i });
        }

        // Mock the room repository to return the list of rooms
        roomRepositoryMock.Setup(r => r.GetAll()).Returns(rooms);
    }
    
    [When(@"the customer creates a booking")]
    public void WhenTheCustomerCreatesABooking()
    {
        try
        {
            bookingResult = bookingManager.CreateBooking(booking);
        }
        catch (Exception e)
        {
            bookingException = e;
        }
    }

    [Then(@"the booking should be created successfully")]
    public void ThenTheBookingShouldBe()
    {
        Assert.True(bookingResult);
    }

    [Then(@"the booking should be not created")]
    public void ThenTheBookingShouldNotBeCreated()
    {
        Assert.False(bookingResult);
    }

    [Then(@"the booking should create an exception")]
    public void ThenTheBookingShouldCreateAnException()
    {
        Assert.NotNull(bookingException);
    }
}

