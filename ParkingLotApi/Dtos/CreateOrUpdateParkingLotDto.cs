using ParkingLotApi.Models;

namespace ParkingLotApi.Dtos;

public class CreateOrUpdateParkingLotDto
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; }

    public CreateOrUpdateParkingLotDto()
    {

    }

    public CreateOrUpdateParkingLotDto(string name, int capacity, string location)
    {
        Name = name;
        Capacity = capacity;
        Location = location;
    }

    public CreateOrUpdateParkingLotDto(ParkingLotEntity parkingLotEntity)
    {
        Name = parkingLotEntity.Name;
        Capacity = parkingLotEntity.Capacity;
        Location = parkingLotEntity.Location;
    }

    public ParkingLotEntity ToEntity()
    {
        return new ParkingLotEntity()
        {
            Name = Name,
            Capacity = Capacity,
            Location = Location,
        };
    }
}