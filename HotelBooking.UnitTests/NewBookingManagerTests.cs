using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace HotelBooking.UnitTests;

public class NewBookingManagerTests
{
    private readonly Mock<IRepository<Booking>> _bookingRepositoryMock;
    private readonly Mock<IRepository<Room>> _roomRepositoryMock;
    private readonly BookingManager _bookingManager;
    private readonly ITestOutputHelper _output;

    public NewBookingManagerTests(ITestOutputHelper output)
    {
        _bookingRepositoryMock = new Mock<IRepository<Booking>>();
        _roomRepositoryMock = new Mock<IRepository<Room>>();
        _bookingManager = new BookingManager(_bookingRepositoryMock.Object, _roomRepositoryMock.Object);
        _output = output; 
    }
    
    //Provides test data for the FindAvailableRoom_ShouldReturnCorrectRoom
    public static IEnumerable<object[]> GetRoomAvailabilityData()
    {
        yield return new object[] { DateTime.Today.AddDays(1), DateTime.Today.AddDays(2),2 }; 
        yield return new object[] { DateTime.Today.AddDays(3), DateTime.Today.AddDays(5), 2 };
        yield return new object[] { DateTime.Today.AddDays(3), DateTime.Today.AddDays(5), 2 };
    }
    
    //Purpose: Tests that the FindAvailableRoom method correctly returns the expected available room when given a date range.
    //Uses MemberData to apply different test cases.
    [Theory]
    [MemberData(nameof(GetRoomAvailabilityData))]
    public void FindAvailableRoom_ShouldReturnCorrectRoom(DateTime startDate, DateTime endDate, int expectedRoomId)
    {
      
        var bookings = new List<Booking>
        {
            new Booking { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), RoomId = 1, IsActive = true }
        };

        
        var rooms = new List<Room>
        {
            new Room { Id = 1 },
            new Room { Id = 2 }
        };

        _bookingRepositoryMock.Setup(repo => repo.GetAll()).Returns(bookings.AsQueryable());
        _roomRepositoryMock.Setup(repo => repo.GetAll()).Returns(rooms.AsQueryable());

        // Act
        var result = _bookingManager.FindAvailableRoom(startDate, endDate);

       
        _output.WriteLine("Expected Room ID:" +expectedRoomId +", Actual Room ID:" + result);


        // Assert
        Assert.Equal(expectedRoomId, result);
    }
    
   
    
    [Fact]
    public void FindAvailableRoom_WhenNoRoomsAvailable_ShouldReturnMinusOne()
    {
        // Arrange
        var bookings = new List<Booking>
        {
            new Booking { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), RoomId = 1, IsActive = true },
            new Booking { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), RoomId = 2, IsActive = true }
        };

        var rooms = new List<Room>
        {
            new Room { Id = 1 },
            new Room { Id = 2 }
        };

        _bookingRepositoryMock.Setup(repo => repo.GetAll()).Returns(bookings.AsQueryable());
        _roomRepositoryMock.Setup(repo => repo.GetAll()).Returns(rooms.AsQueryable());

        // Act
        var result = _bookingManager.FindAvailableRoom(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3));

        // Assert
        Assert.Equal(-1, result);
    }
    
    
    [Theory]
    [MemberData(nameof(GetRoomAvailabilityData))]
    public void FindAvailableRoom_WhenNoRoomIsAvailable_ShouldReturnMinusOne(DateTime startDate, DateTime endDate, int _)
    {
        // Arrange
        var bookings = new List<Booking>
        {
            new Booking { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), RoomId = 1, IsActive = true },
            new Booking { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), RoomId = 2, IsActive = true }
        };

        var rooms = new List<Room>
        {
            new Room { Id = 1 },
            new Room { Id = 2 }
        };

        _bookingRepositoryMock.Setup(repo => repo.GetAll()).Returns(bookings.AsQueryable());
        _roomRepositoryMock.Setup(repo => repo.GetAll()).Returns(rooms.AsQueryable());

        // Act
        var result = _bookingManager.FindAvailableRoom(startDate, endDate);

        // Assert
        Assert.Equal(-1, result);
    }
    
    [Fact]
    public void CreateBooking_WithAvailableRoom_ShouldReturnTrue()
    {
        // Arrange
        var booking = new Booking { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(2) };
        var availableRoomId = 1;

        _roomRepositoryMock.Setup(r => r.GetAll()).Returns(new List<Room> { new Room { Id = availableRoomId } }.AsQueryable());
        _bookingRepositoryMock.Setup(b => b.GetAll()).Returns(new List<Booking>().AsQueryable());

        // Act
        var result = _bookingManager.CreateBooking(booking);

        // Assert
        Assert.True(result);
        _bookingRepositoryMock.Verify(repo => repo.Add(It.Is<Booking>(b => b.RoomId == availableRoomId && b.IsActive)), Times.Once);
    }
    
    [Fact]
    public void GetFullyOccupiedDates_ShouldReturnCorrectDates()
    {
        // Arrange
        var bookings = new List<Booking>
        {
            new Booking { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), RoomId = 1, IsActive = true },
            new Booking { StartDate = DateTime.Today.AddDays(1), EndDate = DateTime.Today.AddDays(3), RoomId = 2, IsActive = true }
        };
        var rooms = new List<Room> { new Room { Id = 1 }, new Room { Id = 2 } };
        //The mocked bookingRepository is set up to return the predefined bookings list as a queryable data source.
        _bookingRepositoryMock.Setup(repo => repo.GetAll()).Returns(bookings.AsQueryable());
        _roomRepositoryMock.Setup(repo => repo.GetAll()).Returns(rooms.AsQueryable());

        // Act
        var result = _bookingManager.GetFullyOccupiedDates(DateTime.Today.AddDays(1), DateTime.Today.AddDays(3));

        // Assert
        Assert.Equal(new List<DateTime> { DateTime.Today.AddDays(1), DateTime.Today.AddDays(2), DateTime.Today.AddDays(3) }, result);
    }
    
    // edge tests 
    [Fact]
    public void FindAvailableRoom_WhenEndDateEarlierThanStartDate_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _bookingManager.FindAvailableRoom(DateTime.Today.AddDays(2), DateTime.Today.AddDays(1)));
    }
    
    [Fact]
    public void FindAvailableRoom_WhenNoActiveBookings_ShouldReturnFirstAvailableRoom()
    {
        // Arrange
        _bookingRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Booking>().AsQueryable());
        _roomRepositoryMock.Setup(repo => repo.GetAll()).Returns(new List<Room> { new Room { Id = 1 } }.AsQueryable());

        // Act
        var result = _bookingManager.FindAvailableRoom(DateTime.Today.AddDays(1), DateTime.Today.AddDays(2));

        // Assert
        Assert.Equal(1, result);
    }
    
    
    [Fact]
    public void FindAvailableRoom_WhenStartDateInPast_ShouldThrowArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _bookingManager.FindAvailableRoom(DateTime.Today.AddDays(-1), DateTime.Today.AddDays(1)));
    }
    
}