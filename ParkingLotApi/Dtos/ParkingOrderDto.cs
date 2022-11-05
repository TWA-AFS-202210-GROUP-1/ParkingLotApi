using System;
using ParkingLotApi.Consts;
using ParkingLotApi.Models;

namespace ParkingLotApi.Dtos
{
    public class ParkingOrderDto
    {
        public int OrderNumber { get; set; }
        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime CloseTime { get; set; }
        public OrderStatus OrderStatus { get; set; }

        public ParkingOrderDto()
        {
            
        }

        public ParkingOrderDto(ParkingOrderEntity entity, string parkingLotName)
        {
            ParkingLotName = parkingLotName;
            OrderNumber = entity.Id;
            PlateNumber = entity.PlateNumber;
            CreateTime = entity.CreateTime;
            CloseTime =  entity.CloseTime;
            OrderStatus = entity.Status;
        }
    }
}
