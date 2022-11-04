using ParkingLotApi.Dtos;

namespace ParkingLotApi.Models;

public class ParkingLotEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; }

    public ParkingLotEntity()
    {
    }

    public void UpdateEntityByDto(CreateOrUpdateParkingLotDto newParkingLot)
    {
        Name = newParkingLot.Name;
        Capacity = newParkingLot.Capacity;
        Location = newParkingLot.Location;
    }
}