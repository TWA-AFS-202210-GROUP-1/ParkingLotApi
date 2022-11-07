using ParkingLotApi.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
  public interface IParkingLotService
  {
    Task<int> AddParkingLot(ParkingLotDto parkingLotDto);
    List<ParkingLotDto> GetAll();
    ParkingLotDto GetById(int id);
    List<ParkingLotDto> GetByPageIndex(int pageIndex);
    Task RemoveParkingLot(int id);
    Task<ParkingLotDto> UpdateCapacity(int id, ParkingLotDto newParkingLotDto);
  }
}