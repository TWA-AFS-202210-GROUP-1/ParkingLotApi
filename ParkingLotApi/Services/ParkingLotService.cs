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
    private readonly int pageSize = 15;

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

    public ParkingLotDto GetById(int id)
    {
      var parkingLot = parkingLotDbContext.ParkingLots.FirstOrDefault(parkingLot => parkingLot.Id == id);

      return new ParkingLotDto(parkingLot);
    }

    public List<ParkingLotDto> GetByPageIndex(int pageIndex)
    {
      var parkingLots = parkingLotDbContext.ParkingLots.ToList();

      return parkingLots
        .Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity))
        .Skip((pageIndex - 1) * pageSize)
        .Take(pageSize)
        .ToList();
    }

    public async Task RemoveParkingLot(int id)
    {
      var parkingLot = parkingLotDbContext.ParkingLots.FirstOrDefault(parkingLot => parkingLot.Id == id);

      parkingLotDbContext.ParkingLots.Remove(parkingLot);
      await parkingLotDbContext.SaveChangesAsync();
    }
  }
}
