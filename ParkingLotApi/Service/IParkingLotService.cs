using ParkingLotApiTest.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkingLotApi.Service
{
    public interface IParkingLotService
    {
        List<ParkingLotDto> GetAll();
        Task<int> AddParkingLot(ParkingLotDto parkingLotDto);
        Task deleteParkingLot(int id);
        List<ParkingLotDto> GetByPageIndex(int? pageIndex);
    }
}