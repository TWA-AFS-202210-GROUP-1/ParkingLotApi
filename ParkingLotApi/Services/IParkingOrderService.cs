using ParkingLotApi.Dtos;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
  public interface IParkingOrderService
  {
    Task<int> CreateOrder(ParkingOrderDto parkingOrderDto);
    ParkingOrderDto GetById(int id);
    Task<ParkingOrderDto> UpdateStatus(int id, ParkingOrderDto newParkingOrderDto);
  }
}