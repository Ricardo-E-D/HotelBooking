using HotelBooking.Core;
using Moq;
using System;
using System.Collections.Generic;
using TechTalk.SpecFlow;
using FluentAssertions;

namespace HotelBooking.SpecFlow.Steps
{
    [Binding]
    public sealed class CreateBookingStepDefinitions
    {

        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext _scenarioContext;
        private IBookingManager bookingManager;
        private Mock<IRepository<Booking>> fakeBookRepository;
        private Mock<IRepository<Room>> fakeRoomRepository;
        private Booking booking = new Booking();
        private bool _result = false;

        public CreateBookingStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
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
            bookingManager = new BookingManager(fakeBookRepository.Object, fakeRoomRepository.Object);
        }

        [Given("the first date is (.*)")]
        public void GivenTheFirstDate(int daysFromNow)
        {
            booking.StartDate = DateTime.Today.AddDays(daysFromNow);
        }

        [Given("the second date is (.*)")]
        public void GivenTheSecondDate(int daysFromNow)
        {
            booking.EndDate = DateTime.Today.AddDays(daysFromNow);
        }

        [When("a booking is created")]
        public void WhenABookingIsCreatedFromTheTwoDates()
        {
            _result = bookingManager.CreateBooking(booking);
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(bool result)
        {
            _result.Should().Be(result);
        }
    }
}