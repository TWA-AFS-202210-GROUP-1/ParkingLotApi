using ParkingLotApi.Models;

namespace ParkingLotApi.Dtos;

public class CreatedParkingLotDto : ParkingLotDto
{
    public CreatedParkingLotDto(ParkingLotEntity parkingLotEntity)
    {
        Id = parkingLotEntity.Id;
        Name = parkingLotEntity.Name;
        Capacity = parkingLotEntity.Capacity;
        Location = parkingLotEntity.Location;
    }

    public int Id { get; set; }
}