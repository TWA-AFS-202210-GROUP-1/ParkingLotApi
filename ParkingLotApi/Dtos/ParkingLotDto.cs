using ParkingLotApi.Models;

namespace ParkingLotApi.Dtos;

public class ParkingLotDto
{
    public string Name { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; }

    public ParkingLotDto()
    {

    }

    public ParkingLotDto(string name, int capacity, string location)
    {
        Name = name;
        Capacity = capacity;
        Location = location;
    }

    public ParkingLotDto(ParkingLotEntity parkingLotEntity)
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