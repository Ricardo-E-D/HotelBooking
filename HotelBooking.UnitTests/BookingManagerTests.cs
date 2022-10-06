using System;
using System.Collections.Generic;
using System.Linq;
using HotelBooking.Core;
using HotelBooking.UnitTests.Fakes;
using Moq;
using Xunit;

namespace HotelBooking.UnitTests
{
    public class BookingManagerTests
    {
        private IBookingManager bookingManager;
        private Mock<IRepository<Booking>> fakeBookRepository;
        private Mock<IRepository<Room>> fakeRoomRepository;


        //Mock<IBookingManager> bookingMock = new Mock<IBookingManager>();


        public BookingManagerTests(){
            DateTime start = DateTime.Today.AddDays(10);
            DateTime end = DateTime.Today.AddDays(20);
            var rooms = new List<Room>
            {
                new Room { Id=1, Description="A" },
                new Room { Id=2, Description="B" }
            };

            var booking = new List<Booking>
            {
                 new Booking { Id=1, StartDate=start, EndDate=end, IsActive=true, CustomerId=1, RoomId=1 },
                new Booking { Id=2, StartDate=start, EndDate=end, IsActive=true, CustomerId=2, RoomId=2 },
            };
            
            fakeBookRepository = new Mock<IRepository<Booking>>();
            fakeRoomRepository = new Mock<IRepository<Room>>();

            fakeRoomRepository.Setup(x => x.GetAll()).Returns(rooms);
            fakeBookRepository.Setup(x => x.GetAll()).Returns(booking);
            //fakeBookRepository
            //    .Setup(b => b.Get(It.IsAny<int>()))
            //    .Returns(booking.FirstOrDefault());
            bookingManager = new BookingManager(fakeBookRepository.Object, fakeRoomRepository.Object);

        }

        #region CreateBooking-Tests
        [Theory]
        [InlineData(9, 9, true)]
        [InlineData(21, 21, true)]
        [InlineData(9, 21, false)]
        [InlineData(9, 10, false)]
        [InlineData(9, 20, false)]
        [InlineData(10, 21, false)]
        [InlineData(20, 21, false)]
        [InlineData(10, 10, false)]
        [InlineData(10, 20, false)]
        [InlineData(20, 20, false)]
        public void CreateBooking_BookingDateAvailable_ReturnsTrue_BookingDateNotAvailable_ReturnsFalse(int start, int end, bool expected)
        {
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(start),
                EndDate = DateTime.Today.AddDays(end)
            };
            bool created = bookingManager.CreateBooking(booking);
            Assert.Equal(expected, created);
        }

        [Fact]
        public void CreateBooking_BookingDateUnavailable_ReturnsFalse()
        {
            var booking = new Booking
            {
                StartDate = DateTime.Today.AddDays(1),
                EndDate = DateTime.Today.AddDays(10)
            };
            bool created = bookingManager.CreateBooking(booking);
            Assert.False(created);
        } 
        #endregion

        #region FindAvailableRoom-Tests
        [Fact]
        public void FindAvailableRoom_StartDateNotInTheFuture_ThrowsArgumentException()
        {
            DateTime date = DateTime.Today;
            Action act = () => bookingManager.FindAvailableRoom(date, date);
            Assert.Throws<ArgumentException>(act);
        }

        [Fact]
        public void FindAvailableRoom_RoomAvailable_RoomIdNotMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(1);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, date);
            // Assert
            Assert.NotEqual(-1, roomId);
        }

        [Fact]
        public void FindAvailableRoom_RoomNotAvailable_RoomIdIsMinusOne()
        {
            // Arrange
            DateTime date = DateTime.Today.AddDays(5);
            DateTime endDate = DateTime.Today.AddDays(13);
            // Act
            int roomId = bookingManager.FindAvailableRoom(date, endDate);
            // Assert
            Assert.Equal(-1, roomId);
        } 
        #endregion

        #region GetFullyOccupiedDates-Tests
        [Fact]
        public void GetFullyOccupiedDates_StartDateNotLaterThanEndDate_ThrowsArgumentException()
        {
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today;
            Action act = () => bookingManager.GetFullyOccupiedDates(startDate, endDate);
            Assert.Throws<ArgumentException>(act);
        }

        [Theory]
        [InlineData(1, 9, 0)]
        [InlineData(18, 300, 3)]
        [InlineData(19, 300, 2)]
        [InlineData(20, 300, 1)]
        [InlineData(17, 300, 4)]
        [InlineData(16, 300, 5)]
        [InlineData(15, 300, 6)]
        [InlineData(14, 300, 7)]
        [InlineData(13, 300, 8)]
        [InlineData(12, 300, 9)]
        [InlineData(2, 300, 11)]
        [InlineData(3, 300, 11)]
        [InlineData(77, 300, 0)]
        [InlineData(7, 300, 11)]
        [InlineData(64, 300, 0)]
        [InlineData(90, 300, 0)]
        [InlineData(88, 300, 0)]
        [InlineData(78, 300, 0)]
        [InlineData(65, 300, 0)]
        [InlineData(33, 300, 0)]
        [InlineData(32, 300, 0)]
        [InlineData(44, 300, 0)]
        public void GetFullyOccupiedDates_DatesRangingBetweenFullyAndNonFullyOccupiedDates_ReturnsTheRightAmountOfFullyOccupiedDates(int x, int y, int expected)
        {
            DateTime date = DateTime.Today.AddDays(x);
            DateTime endDate = DateTime.Today.AddDays(y);
            var noOfOccupiedDates = bookingManager.GetFullyOccupiedDates(date, endDate).Count;
            Assert.Equal(expected, noOfOccupiedDates);
        }


        //[Fact]
        //public void GetFullyOccupiedDates_DatesOutsideFullyOccupiedBookings_Returns0FullyOccupiedDates()
        //{
        //    DateTime date = DateTime.Today.AddDays(1);
        //    DateTime endDate = DateTime.Today.AddDays(9);
        //    var noOfOccupiedDates = bookingManager.GetFullyOccupiedDates(date, endDate).Count;
        //    Assert.Equal(0, noOfOccupiedDates);
        //}

        //[Fact]
        //public void GetFullyOccupiedDates_DatesWithinOccupiedBookings_Returns3FullyOccupiedDates()
        //{
        //    DateTime date = DateTime.Today.AddDays(18);
        //    DateTime endDate = DateTime.Today.AddDays(300);
        //    var noOfOccupiedDates = bookingManager.GetFullyOccupiedDates(date, endDate).Count;
        //    Assert.Equal(3, noOfOccupiedDates);
        //} 
        #endregion
    }
}
