using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Domain;
using RoomBookingApp.Persistence;
using RoomBookingApp.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace roomBookingApp.Persistence.Tests
{
    public class RoomBookingServiceTest
    {
        [Fact]
        public void ShouldReturnAvailableRooms()
        {
            //Arrange
            var date = new DateTime(2023, 10, 23);
            var dbOptions = new DbContextOptionsBuilder<RoomBookingDbContext>()
                .UseInMemoryDatabase("AvailableRoomTest")
                .Options;

            using var context = new RoomBookingDbContext(dbOptions);

            context.Add(new Room { Id = 1, Name = "Room 1" });
            context.Add(new Room { Id = 2, Name = "Room 2" });
            context.Add(new Room { Id = 3, Name = "Room 3" });

            context.Add(new RoomBooking { RoomId = 1, Date = date });
            context.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1) });


            context.SaveChanges();


            var roomBookingService = new RoomBookingService(context);

            //Act

            var availableRooms = roomBookingService.GetAvailableRooms(date);

            //Assert

            Assert.Equal(2, availableRooms.Count());
            Assert.Contains(availableRooms, q => q.Id == 2);
            Assert.Contains(availableRooms, q => q.Id == 3);
            Assert.DoesNotContain(availableRooms, q => q.Id == 1);




        }

        [Fact]
        public void ShouldSaveRoomBooking()
        {
            //Arrange
            var date = new DateTime(2023, 10, 23);
            var dbOptions = new DbContextOptionsBuilder<RoomBookingDbContext>()
                .UseInMemoryDatabase("ShouldSaveTest")
                .Options;

            
            using var context = new RoomBookingDbContext(dbOptions);
            var roomBookingService = new RoomBookingService(context);

            var roomBooking = new RoomBooking
            {
                RoomId = 1,
                Date = new DateTime(2022, 10, 2)
            };

            roomBookingService.Save(roomBooking);
            var bookings = context.RoomBookings.ToList();


            var booking = Assert.Single(bookings);
            Assert.Equal(roomBooking.Date, booking.Date);
            Assert.Equal(roomBooking.RoomId,booking.RoomId);
            

        }

    }
}
