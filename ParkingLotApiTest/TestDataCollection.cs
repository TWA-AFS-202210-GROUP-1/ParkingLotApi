using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using System;
using System.Collections.Generic;

namespace ParkingLotApiTest
{
  public static class TestDataCollection
  {
    public static int DefaultCapacity { get; set; } = 10;

    public static OrderStatus DefaultStatus { get; set; } = OrderStatus.Open;

    public static List<ParkingLotDto> ParkingLots
    {
      get => new List<ParkingLotDto>
      {
        new ParkingLotDto("Park Xpert", DefaultCapacity, "Aaron's Hill, Surrey"),
        new ParkingLotDto("Mountain View Parking", DefaultCapacity, "Clapton, Berkshire"),
        new ParkingLotDto("Drive On Park", DefaultCapacity, "Stocksbridge, Sheffield"),
        new ParkingLotDto("Parker Parking", DefaultCapacity, "Knighton, City of Leicester"),
        new ParkingLotDto("Parking Miles", DefaultCapacity, "Stockstreet, Essex"),
        new ParkingLotDto("Park N' Stay", DefaultCapacity, "Benville, Dorset"),
        new ParkingLotDto("Ben's Parking", DefaultCapacity, "Kitt's Green, Birmingham"),
        new ParkingLotDto("Park Hub City", DefaultCapacity, "Acaster Malbis, York"),
        new ParkingLotDto("Park My Spot", DefaultCapacity, "Preston, Lancashire"),
        new ParkingLotDto("Drop By Park", DefaultCapacity, "Bethesda, Gwynedd"),
        new ParkingLotDto("Poor Gentleman", DefaultCapacity, "Stonesfield, Oxfordshire"),
        new ParkingLotDto("Future Park", DefaultCapacity, "Aigburth, Liverpool"),
        new ParkingLotDto("Roadside Garage", DefaultCapacity, "Quarry Bank, Dudley"),
        new ParkingLotDto("16th Accessible", DefaultCapacity, "Marsh, Bradford"),
        new ParkingLotDto("The Story", DefaultCapacity, "Beverston, Gloucestershire"),
        new ParkingLotDto("Principal Park Spot", DefaultCapacity, "Queen's Park, London"),
        new ParkingLotDto("Diagonal Vehicles", DefaultCapacity, "Clifford, Leeds"),
        new ParkingLotDto("Old Dealership", DefaultCapacity, "Airlie, Angus"),
        new ParkingLotDto("Just Enough", DefaultCapacity, "St. Peter's, Kent"),
        new ParkingLotDto("Tried N' True", DefaultCapacity, "Radford, Coventry"),
      };
    }

    public static List<ParkingOrderDto> ParkingOrders
    {
      get => new List<ParkingOrderDto>
      {
        new ParkingOrderDto
        {
          ParkingLot = "Park Xpert",
          PlateNumber = "GD40 FDM",
          CreationTime = DateTime.Now,
          CloseTime = DateTime.Now,
          Status = DefaultStatus,
        },
        new ParkingOrderDto
        {
          ParkingLot = "Mountain View Parking",
          PlateNumber = "AO24 HJF",
          CreationTime = DateTime.Now,
          CloseTime = DateTime.Now,
          Status = DefaultStatus,
        },
        new ParkingOrderDto
        {
          ParkingLot = "Drive On Park",
          PlateNumber = "KM14 POW",
          CreationTime = DateTime.Now,
          CloseTime = DateTime.Now,
          Status = DefaultStatus,
        },
      };
    }
  }
}
