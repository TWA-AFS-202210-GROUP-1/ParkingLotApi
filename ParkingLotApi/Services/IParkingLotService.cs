using System.Collections.Generic;
using ParkingLotApi.Dtos;
using System.Threading.Tasks;

namespace ParkingLotApi.Services;

public interface IParkingLotService
{
    Task<ParkingLotDto> CreateParkingLot(CreateParkingLotDto parkingLot);
    Task DeleteParkingLot(int parkingLotId);
    Task<List<ParkingLotDto>> GetParkingLotsByPageNumber(int pageNumber);
    Task<ParkingLotDto> GetParkingLotById(int parkingLotId);
}