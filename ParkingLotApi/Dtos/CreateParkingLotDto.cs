using ParkingLotApi.Models;

namespace ParkingLotApi.Dtos;

public class CreateParkingLotDto
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; }

    public CreateParkingLotDto()
    {

    }

    public CreateParkingLotDto(string name, int capacity, string location)
    {
        Name = name;
        Capacity = capacity;
        Location = location;
    }

    public CreateParkingLotDto(ParkingLotEntity parkingLotEntity)
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