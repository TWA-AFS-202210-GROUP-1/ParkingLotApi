using ParkingLotApi.Dtos;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
    public interface IParkingOrderService
    {
        Task<ParkingOrderDto> CreateParkingOrder(CreateParkingOrderDto createParkingOrderDto);
        Task<ParkingOrderDto> UpdateOrderStatus(int orderId, UpdateParkingOrderDto updateParkingOrderDto);
    }
}
