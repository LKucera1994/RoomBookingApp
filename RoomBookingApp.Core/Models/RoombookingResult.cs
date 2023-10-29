using RoomBookingApp.Core.Enums;

namespace RoomBookingApp.Core.Models
{
    public class RoomBookingResult : RoombookingBase
    {
        public BookingResultFlag Flag { get; set; }
        public int? RoomBookingId { get; set; }
    }
}