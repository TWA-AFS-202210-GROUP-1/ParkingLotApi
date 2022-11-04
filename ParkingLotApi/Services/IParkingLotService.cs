using ParkingLotApi.Dtos;
using System.Threading.Tasks;

namespace ParkingLotApi.Services;

public interface IParkingLotService
{
    Task<CreatedParkingLotDto> CreateParkingLot(ParkingLotDto parkingLot);
    Task DeleteParkingLot(int parkingLotId);
}