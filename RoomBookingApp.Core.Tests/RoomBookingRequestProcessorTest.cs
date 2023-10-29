using Moq;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Core
{
    public class RoomBookingRequestProcessorTest 
    {
        private RoomBookingRequestProcessor _processor;
        private RoomBookingRequest _request;
        private Mock<IRoomBookingService> _roomBookingServiceMock;
        private List<Room> _availableRooms;

        public RoomBookingRequestProcessorTest()
        {

            _request = new RoomBookingRequest
            {
                FullName = "Test Name",
                Email = "test@request.com",
                Date = new DateTime(2021, 10, 21),
            };

            _availableRooms = new List<Room>() { new Room() { Id=1} };

            _roomBookingServiceMock = new Mock<IRoomBookingService>();
            _roomBookingServiceMock.Setup(q => q.GetAvailableRooms(_request.Date))
                    .Returns(_availableRooms);            
            _processor = new RoomBookingRequestProcessor(_roomBookingServiceMock.Object);
            
        }

        [Fact]
        public void ShouldReturnRoomBookResponseWithRequestValues()
        {

            // Act

            RoomBookingResult result = _processor.BookRoom(_request);

            //Assert

            Assert.NotNull(result);
            Assert.Equal(_request.FullName, result.FullName);
            Assert.Equal(_request.Email, result.Email);
            Assert.Equal(_request.Date, result.Date);


            //Shouldly
            //result.ShouldNotBeNull();
            //result.FullName.ShouldBe(request.FullName);
            //result.Email.ShouldBe(request.Email);
            //result.Date.ShouldBe(request.Date);




        }

        [Fact]
        public void ShouldThrowExceptionForNullRequest()
        {
            
            //Shouldly
            var exception = Should.Throw<ArgumentNullException>(() => _processor.BookRoom(null));
            exception.ParamName.ShouldBe("bookingRequest");

            Assert.Throws<ArgumentNullException>(() => _processor.BookRoom(null));
        }
        [Fact]
        public void ShouldSaveRoomBookingRequest()
        {
            RoomBooking roomBooking = null;
            _roomBookingServiceMock.Setup(q => q.Save(It.IsAny<RoomBooking>()))
                .Callback<RoomBooking>(q => roomBooking = q);
            _processor.BookRoom(_request);


            _roomBookingServiceMock.Verify(q=> q.Save(It.IsAny<RoomBooking>()), Times.Once);

            Assert.NotNull(roomBooking);
            Assert.Equal(roomBooking.FullName, _request.FullName);
            Assert.Equal(roomBooking.Email, _request.Email);
            Assert.Equal(roomBooking.Date, _request.Date);
            Assert.Equal(roomBooking.RoomId, _availableRooms.FirstOrDefault().Id);

        }

        [Fact]
        public void ShouldNotSaveRoomBookingRequestIfNoneAvailable()
        {
            _availableRooms.Clear();
            _processor.BookRoom(_request);
            _roomBookingServiceMock.Verify(q => q.Save(It.IsAny<RoomBooking>()), Times.Never);
        }

        [Theory]
        [InlineData(BookingResultFlag.Failure,false)]
        [InlineData(BookingResultFlag.Success,true)]
        public void ShouldReturnSuccessFailureFlagInResult(BookingResultFlag bookingSuccessFlag, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableRooms.Clear();
            }
            var result = _processor.BookRoom(_request);
            Assert.Equal(bookingSuccessFlag, result.Flag);

        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(null, false)]
        public void ShouldReturnRoomBookingIdInResult(int? roomBookingId, bool isAvailable)
        {
            if (!isAvailable)
            {
                _availableRooms.Clear();
            }
            else
            {
                _roomBookingServiceMock.Setup(q => q.Save(It.IsAny<RoomBooking>()))
               .Callback<RoomBooking>(q =>q.Id = roomBookingId.Value);
            }

            var result = _processor.BookRoom(_request);

            Assert.Equal(result.RoomBookingId, roomBookingId);
            

        }

        
    }
}
