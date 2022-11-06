﻿using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
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
        .Skip((pageIndex - 1) * PageSize)
        .Take(PageSize)
        .ToList();
    }

    public async Task<ParkingLotDto> UpdateCapacity(int id, ParkingLotDto newParkingLotDto)
    {
      var parkingLot = parkingLotDbContext.ParkingLots.FirstOrDefault(parkingLot => parkingLot.Id == id);
      parkingLot.Capacity = newParkingLotDto.Capacity;
      parkingLotDbContext.ParkingLots.Update(parkingLot);
      await parkingLotDbContext.SaveChangesAsync();

      return new ParkingLotDto(parkingLot);
    }

    public async Task RemoveParkingLot(int id)
    {
      var parkingLot = parkingLotDbContext.ParkingLots.FirstOrDefault(parkingLot => parkingLot.Id == id);
      parkingLotDbContext.ParkingLots.Remove(parkingLot);
      await parkingLotDbContext.SaveChangesAsync();
    }

    private bool HasParkingLot(ParkingLotDto parkingLotDto)
    {
      var existingParkingLot = parkingLotDbContext.ParkingLots.FirstOrDefault(parkingLot => parkingLot.Name.Equals(parkingLotDto.Name));

      return existingParkingLot != null;
    }
  }
}
