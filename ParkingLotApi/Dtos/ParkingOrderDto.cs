using ParkingLotApi.Model;
using System;

namespace ParkingLotApiTest.Dtos
{
    public class ParkingOrderDto
    {
        public ParkingOrderDto()
        {
        }
        public ParkingOrderDto(ParkingOrderEntity parkingOrderEntity)
        {
            ParkingLotName = parkingOrderEntity.ParkingLotName;
            PlateNumber = parkingOrderEntity.PlateNumber;
            CreationTime = parkingOrderEntity.CreationTime;
            CloseTime = parkingOrderEntity.CloseTime;
            OrderStatus = parkingOrderEntity.OrderStatus;
        }

        public string ParkingLotName { get; set; }
        public string PlateNumber { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime CloseTime { get; set; }
        public bool OrderStatus { get; set; }

        public ParkingOrderEntity ToEntity()
        {
            return new ParkingOrderEntity()
            {
                ParkingLotName = ParkingLotName,
                PlateNumber = PlateNumber,
                CreationTime = CreationTime,
                CloseTime = CloseTime,
                OrderStatus = OrderStatus,
            };
        }
    }
}