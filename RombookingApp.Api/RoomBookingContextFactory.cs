using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Persistence;

namespace RombookingApp.Api
{
    public class RoomBookingContextFactory : IDesignTimeDbContextFactory<RoomBookingDbContext>
    {
        public RoomBookingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<RoomBookingDbContext>();
            optionsBuilder.UseSqlite("Filename=:memory:");
            return new RoomBookingDbContext(optionsBuilder.Options);
        }
    }
}
}
