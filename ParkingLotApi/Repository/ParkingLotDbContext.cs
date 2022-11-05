using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Model;

namespace ParkingLotApi.Repository
{
    public class ParkingLotDbContext : DbContext
    {
        public ParkingLotDbContext(DbContextOptions<ParkingLotDbContext> options)
            : base(options)
        {
        }

        public DbSet<ParkingLotEntity> Parkinglots
        {
            get;
            set;
        }
    }
}