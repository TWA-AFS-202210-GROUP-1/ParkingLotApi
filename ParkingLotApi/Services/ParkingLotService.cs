using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
  public class ParkingLotService
  {
    private readonly ParkingLotDbContext parkingLotDbContext;

    public ParkingLotService(ParkingLotDbContext parkingLotDbContext)
    {
      this.parkingLotDbContext = parkingLotDbContext;
    }

    public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
    {
      var parkingLotEntity = parkingLotDto.ToEntity();
      await parkingLotDbContext.ParkingLots.AddAsync(parkingLotEntity);
      await parkingLotDbContext.SaveChangesAsync();

      return parkingLotEntity.Id;
    }

    public List<ParkingLotDto> GetAll()
    {
      var parkingLots = parkingLotDbContext.ParkingLots.ToList();

      return parkingLots
        .Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity))
        .ToList();
    }

    public async Task<ParkingLotDto> GetById(int id)
    {
      var parkingLot = await parkingLotDbContext.ParkingLots.FirstOrDefaultAsync(parkingLot => parkingLot.Id == id);

      return new ParkingLotDto(parkingLot);
    }
  }
}
