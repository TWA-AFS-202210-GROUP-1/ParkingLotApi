using ParkingLotApi.Dtos;
using ParkingLotApi.Repository;
using System.Linq;
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
      var parkingOrderEntity = parkingOrderDto.ToEntity();
      await parkingLotDbContext.ParkingOrders.AddAsync(parkingOrderEntity);
      await parkingLotDbContext.SaveChangesAsync();

      return parkingOrderEntity.Id;
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
      parkingLotDbContext.ParkingOrders.Update(parkingOrder);
      await parkingLotDbContext.SaveChangesAsync();

      return new ParkingOrderDto(parkingOrder);
    }
  }
}
