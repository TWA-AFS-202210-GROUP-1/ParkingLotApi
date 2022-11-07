using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
  public class ParkingLotService : IParkingLotService
  {
    private readonly ParkingLotDbContext parkingLotDbContext;

    public ParkingLotService(ParkingLotDbContext parkingLotDbContext)
    {
      this.parkingLotDbContext = parkingLotDbContext;
    }

    public static int PageSize { get; private set; } = 15;

    public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
    {
      if (HasParkingLot(parkingLotDto))
      {
        throw new DuplicateParkingLotNameException($"Parking lot name: {parkingLotDto.Name} already exists.", HttpStatusCode.Conflict);
      }

      var parkingLotEntity = parkingLotDto.ToEntity();
      await parkingLotDbContext.ParkingLots.AddAsync(parkingLotEntity);
      await parkingLotDbContext.SaveChangesAsync();

      return parkingLotEntity.Id;
    }

    public List<ParkingLotDto> GetAll()
    {
      var parkingLots = FindAllParkingLotEntities();

      return parkingLots
        .Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity))
        .ToList();
    }

    public ParkingLotDto GetById(int id)
    {
      var parkingLot = FindParkingLotEntityById(id);

      return new ParkingLotDto(parkingLot);
    }

    public List<ParkingLotDto> GetByPageIndex(int pageIndex)
    {
      var parkingLots = FindAllParkingLotEntities();
      var pageOfParkingLots = parkingLots
        .Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity))
        .Skip((pageIndex - 1) * PageSize)
        .Take(PageSize)
        .ToList();

      if (pageOfParkingLots.Count == 0)
      {
        throw new ParkingLotPageIndexOutOfRangeException($"There is(are) only {pageIndex - 1} page(s).", HttpStatusCode.NotFound);
      }

      return pageOfParkingLots;
    }

    public async Task<ParkingLotDto> UpdateCapacity(int id, ParkingLotDto newParkingLotDto)
    {
      var parkingLot = FindParkingLotEntityById(id);

      if (newParkingLotDto.Capacity < parkingLot.Capacity)
      {
        throw new InvalidParkingLotCapacityException($"New capacity must not be smaller than the original, which is {parkingLot.Capacity}.", HttpStatusCode.Conflict);
      }

      parkingLot.Capacity = newParkingLotDto.Capacity;
      parkingLotDbContext.ParkingLots.Update(parkingLot);
      await parkingLotDbContext.SaveChangesAsync();

      return new ParkingLotDto(parkingLot);
    }

    public async Task RemoveParkingLot(int id)
    {
      var parkingLot = FindParkingLotEntityById(id);
      parkingLotDbContext.ParkingLots.Remove(parkingLot);
      await parkingLotDbContext.SaveChangesAsync();
    }

    private bool HasParkingLot(ParkingLotDto parkingLotDto)
    {
      var existingParkingLot = parkingLotDbContext.ParkingLots.FirstOrDefault(parkingLot => parkingLot.Name.Equals(parkingLotDto.Name));

      return existingParkingLot != null;
    }

    private ParkingLotEntity FindParkingLotEntityById(int id)
    {
      var parkingLot = parkingLotDbContext.ParkingLots
        .Include(parkingLot => parkingLot.ParkingOrders)
        .FirstOrDefault(parkingLot => parkingLot.Id == id);

      if (parkingLot == null)
      {
        throw new ParkingLotNotFoundException($"Found no parking lot with id: {id}.", HttpStatusCode.NotFound);
      }

      return parkingLot;
    }

    private List<ParkingLotEntity> FindAllParkingLotEntities()
    {
      var parkingLots = parkingLotDbContext.ParkingLots
        .Include(parkingLot => parkingLot.ParkingOrders)
        .ToList();

      if (parkingLots.Count == 0)
      {
        throw new ParkingLotNotFoundException("Found no parking lot.", HttpStatusCode.NotFound);
      }

      return parkingLots;
    }
  }
}
