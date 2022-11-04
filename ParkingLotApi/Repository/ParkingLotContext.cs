using Microsoft.EntityFrameworkCore;

namespace ParkingLotApi.Repository
{
  public class ParkingLotContext : DbContext
  {
    public ParkingLotContext(DbContextOptions<ParkingLotContext> options)
        : base(options)
    {
    }
  }
}