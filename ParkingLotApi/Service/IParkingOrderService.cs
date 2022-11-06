using ParkingLotApiTest.Dtos;
using System.Threading.Tasks;

namespace ParkingLotApi.Service
{
    public interface IParkingOrderService
    {
        Task<int> AddParkingOrder(ParkingOrderDto parkingOrderDto);
    }
}