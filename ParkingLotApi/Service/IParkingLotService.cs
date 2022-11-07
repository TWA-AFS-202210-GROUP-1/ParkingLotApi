using ParkingLotApiTest.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkingLotApi.Service
{
    public interface IParkingLotService
    {
        Task<List<ParkingLotDto>> GetAll();
        Task<int> AddParkingLot(ParkingLotDto parkingLotDto);
        Task deleteParkingLot(int id);
        Task<List<ParkingLotDto>> GetByPageIndex(int? pageIndex);
        Task<ParkingLotDto> GetById(int id);
        Task<ParkingLotDto> UpdateParkingLotCapacity(int id, ParkingLotDto parkingLotDto);
    }
}