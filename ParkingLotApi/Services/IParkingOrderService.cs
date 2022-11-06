using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingLotApi.Dtos;

namespace ParkingLotApi.Services;

public interface IParkingOrderService
{
    Task<int> AddParkingOrder(ParkingOrderDto parkingOrderDto);

    Task<List<ParkingOrderDto>> GetAllParkingOrder();

    Task<ParkingOrderDto?> GetById(int id);

    Task<ParkingOrderDto> UpdateParkingLot(int id, ParkingOrderDto parkingOrderDto);
}