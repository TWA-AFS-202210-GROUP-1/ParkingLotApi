


using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Models;

namespace ParkingLotApi.Repository
{
    public class ParkingLotContext : DbContext
    {
        public ParkingLotContext(DbContextOptions<ParkingLotContext> options)
            : base(options)
        {
        }

        public DbSet<ParkingLot> ParkingLots
        {
            get; set;
        }

        public DbSet<Order> Orders
        {
            get; set;
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ParkingLotContext>()
        //           .HasOptional(j => j.Orders)
        //           .WithMany()
        //           .WillCascadeOnDelete(true);
        //    base.OnModelCreating(modelBuilder);
        //}
    }
}