using ParkingLotApiTest.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkingLotApi.Service
{
    public interface IParkingLotService
    {
        Task<List<ParkingLotDto>> GetAll();
        Task<int> AddParkingLot(ParkingLotDto parkingLotDto);
    }
}