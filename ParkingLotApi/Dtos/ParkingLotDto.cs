using ParkingLotApi.Models;
using System.Collections.Generic;

namespace ParkingLotApi.Dtos
{
  public class ParkingLotDto
  {
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

    public string Name { get; set; } = string.Empty;

    public int Capacity { get; set; }

    public string Location { get; set; } = string.Empty;

    public ParkingLotEntity ToEntity()
    {
      return new ParkingLotEntity
      {
        Name = Name,
        Capacity = Capacity,
        Location = Location,
        ParkingOrders = new List<ParkingOrderEntity>(),
      };
    }

    public override string ToString()
    {
      return $"Name: {Name}\r\nCapacity: {Capacity}\r\nLocation: {Location}";
    }
  }
}
