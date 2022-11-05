using ParkingLotApi.Models;
using System;

namespace ParkingLotApi.Dtos
{
  public class ParkingOrderDto
  {
    public ParkingOrderDto()
    {
    }

    public ParkingOrderDto(ParkingOrderEntity parkingOrderEntity)
    {
      ParkingLot = parkingOrderEntity.ParkingLot;
      PlateNumber = parkingOrderEntity.PlateNumber;
      CreationTime = parkingOrderEntity.CreationTime;
      CloseTime = parkingOrderEntity.CloseTime;
      Status = parkingOrderEntity.Status;
    }

    public string ParkingLot { get; set; } = string.Empty;

    public string PlateNumber { get; set; } = string.Empty;

    public DateTime CreationTime { get; set; }

    public DateTime CloseTime { get; set; }

    public OrderStatus Status { get; set; }

    public ParkingOrderEntity ToEntity()
    {
      return new ParkingOrderEntity
      {
        ParkingLot = ParkingLot,
        PlateNumber = PlateNumber,
        CreationTime = CreationTime,
        CloseTime = CloseTime,
        Status = Status,
      };
    }
  }
}
