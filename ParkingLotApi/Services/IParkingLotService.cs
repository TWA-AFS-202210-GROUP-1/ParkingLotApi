using System.Collections.Generic;
using System.Threading.Tasks;
using ParkingLotApi.Dtos;

namespace ParkingLotApi.Services;

public interface IParkingLotService
{
    Task<int> AddParkingLot(ParkingLotDto parkingLotDto);

    Task<List<ParkingLotDto>> GetAllParkingLot();

    Task DeleteParkingLot(int id);

    Task<IEnumerable<ParkingLotDto>> GetParkingLotByRange(int pageIndex);

    Task<ParkingLotDto?> GetById(int id);

    Task<ParkingLotDto> UpdateParkingLot(int id, ParkingLotDto parkingLotDto);
}