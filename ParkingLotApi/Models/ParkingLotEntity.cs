using System.Collections.Generic;
using ParkingLotApi.Dtos;

namespace ParkingLotApi.Models;

public class ParkingLotEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Capacity { get; set; }
    public string Location { get; set; }
    public List<ParkingOrderEntity> Orders { get; set; }

    public ParkingLotEntity()
    {
    }

    public void UpdateEntityByDto(CreateOrUpdateParkingLotDto newParkingLot)
    {
        Name = newParkingLot.Name;
        Capacity = newParkingLot.Capacity;
        Location = newParkingLot.Location;
    }

    public bool IsFull()
    {
        return Orders.Count >= Capacity;
    }
}