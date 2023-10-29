using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomBookingApp.Persistence.Repositories
{
    public class RoomBookingService : IRoomBookingService
    {
        private readonly RoomBookingDbContext _context;

        public RoomBookingService(RoomBookingDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Room> GetAvailableRooms(DateTime date)
        {
        
            var availableRooms = _context.Rooms.Where(q => q.RoomBookings.Any(x => x.Date == date) == false).ToList();


            return availableRooms;
        }

        public void Save(RoomBooking roomBooking)
        {
            _context.Add(roomBooking);
            _context.SaveChanges();
        }
    }
}
