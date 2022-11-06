using ParkingLotApiTest.Dtos;
using System.Threading.Tasks;

namespace ParkingLotApi.Service
{
    public interface IParkingOrderService
    {
        Task<int> AddParkingOrder(ParkingOrderDto parkingOrderDto);
        Task<ParkingOrderDto> UpdateParkingOrderStatus(int id, ParkingOrderDto parkingOrderDto);
    }
}