using System;
using ParkingLotApi.Consts;

namespace ParkingLotApi.Models
{
    public class ParkingOrderEntity
    {
        public int Id { get; set; }
        public string PlateNumber { get; set; } 
        public DateTime CreateTime { get; set; }
        public DateTime CloseTime { get; set; }
        public OrderStatus Status { get; set; }
    }
}