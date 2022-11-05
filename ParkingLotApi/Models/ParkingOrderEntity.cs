using System;

namespace ParkingLotApi.Models
{
  public class ParkingOrderEntity
  {
    public int Id { get; set; }

    public string ParkingLot { get; set; } = string.Empty;

    public string PlateNumber { get; set; } = string.Empty;

    public DateTime CreationTime { get; set; }

    public DateTime CloseTime { get; set; }

    public OrderStatus Status { get; set; }
  }
}
