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
  public class ParkingOrderService : IParkingOrderService
  {
    private readonly ParkingLotDbContext parkingLotDbContext;

    public ParkingOrderService(ParkingLotDbContext parkingLotDbContext)
    {
      this.parkingLotDbContext = parkingLotDbContext;
    }

    public async Task<int> CreateOrder(ParkingOrderDto parkingOrderDto)
    {
      var parkingLot = parkingLotDbContext.ParkingLots
        .Include(parkingLot => parkingLot.ParkingOrders)
        .FirstOrDefault(parkingLot => parkingLot.Name == parkingOrderDto.ParkingLot);
      if (parkingLot != null)
      {
        if (!IsAvailable(parkingLot))
        {
          throw new ParkingLotFullException("The parking lot is full.", HttpStatusCode.Conflict);
        }

        var parkingOrderEntity = parkingOrderDto.ToEntity();
        parkingLot.ParkingOrders.Add(parkingOrderEntity);
        await parkingLotDbContext.ParkingOrders.AddAsync(parkingOrderEntity);
        await parkingLotDbContext.SaveChangesAsync();

        return parkingOrderEntity.Id;
      }
      else
      {
        throw new ParkingLotNotFoundException($"Found no parking lot named {parkingOrderDto.ParkingLot}.", HttpStatusCode.NotFound);
      }
    }

    public ParkingOrderDto GetById(int id)
    {
      var parkingOrder = parkingLotDbContext.ParkingOrders.FirstOrDefault(parkingOrder => parkingOrder.Id == id);

      return new ParkingOrderDto(parkingOrder);
    }

    public async Task<ParkingOrderDto> UpdateStatus(int id, ParkingOrderDto newParkingOrderDto)
    {
      var parkingOrder = parkingLotDbContext.ParkingOrders.FirstOrDefault(parkingOrder => parkingOrder.Id == id);
      parkingOrder.Status = newParkingOrderDto.Status;
      parkingOrder.CloseTime = newParkingOrderDto.CloseTime;
      parkingLotDbContext.ParkingOrders.Update(parkingOrder);
      await parkingLotDbContext.SaveChangesAsync();

      return new ParkingOrderDto(parkingOrder);
    }

    private static bool IsAvailable(ParkingLotEntity parkingLot)
    {
      if (parkingLot.ParkingOrders != null)
      {
        var openOrderCount = parkingLot.ParkingOrders.FindAll(parkingOrder => parkingOrder.Status == OrderStatus.Open).Count;

        return openOrderCount < parkingLot.Capacity;
      }
      else
      {
        parkingLot.ParkingOrders = new List<ParkingOrderEntity>();
        return true;
      }
    }
  }
}
