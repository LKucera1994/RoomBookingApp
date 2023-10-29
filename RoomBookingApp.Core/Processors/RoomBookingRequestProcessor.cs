using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Core.Models;

namespace RoomBookingApp.Core.Processors
{
    public class RoomBookingRequestProcessor : IRoomBookingRequestProcessor
    {
        private readonly IRoomBookingService _roomBookingService;

        public RoomBookingRequestProcessor(IRoomBookingService roomBookingService)
        {
            _roomBookingService = roomBookingService;
        }

        public RoomBookingResult BookRoom(RoomBookingRequest bookingRequest)
        {

            if (bookingRequest == null)
            {
                throw new ArgumentNullException(nameof(bookingRequest));
            }

            var availableRooms = _roomBookingService.GetAvailableRooms(bookingRequest.Date);
            var result = CreateRoomBookingObject<RoomBookingResult>(bookingRequest);

            if (availableRooms.Any())
            {
                var room = availableRooms.First();
                var roomBooking = CreateRoomBookingObject<RoomBooking>(bookingRequest);
                roomBooking.RoomId = room.Id;
                _roomBookingService.Save(roomBooking);
                result.RoomBookingId = roomBooking.Id;
                result.Flag = Enums.BookingResultFlag.Success;


            }
            else
            {
                result.Flag = Enums.BookingResultFlag.Failure;
            }



            return result;
        }

        private static TRoomBooking CreateRoomBookingObject<TRoomBooking>(RoomBookingRequest bookingRequest)
            where TRoomBooking : RoombookingBase, new()
        {
            return new TRoomBooking
            {

                FullName = bookingRequest.FullName,
                Date = bookingRequest.Date,
                Email = bookingRequest.Email

            };
        }
    }
}