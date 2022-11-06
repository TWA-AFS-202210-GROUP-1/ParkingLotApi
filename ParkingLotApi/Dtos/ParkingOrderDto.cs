using System;
using System.Collections.Generic;
using ParkingLotApi.Models;

namespace ParkingLotApi.Dtos
{
    public class ParkingOrderDto
    {
        public ParkingOrderDto()
        {
        }

        public ParkingOrderDto(string parkingLotName, string plateNumber,
            DateTime createTime, DateTime closeTime)
        {
            this.ParkingLotName = parkingLotName;
            this.PlateNumber = plateNumber;
            this.CreateTime = createTime;
            this.CloseTime = closeTime;
            this.OrderStatus = false;
        }

        public ParkingOrderDto(ParkingOrderEntity parkingOrderEntity)
        {
            this.ParkingLotName = parkingOrderEntity.ParkingLotName;
            this.PlateNumber = parkingOrderEntity.PlateNumber;
            this.CreateTime = parkingOrderEntity.CreateTime;
            this.CloseTime = parkingOrderEntity.CloseTime;
            this.OrderStatus = parkingOrderEntity.OrderStatus;
        }

        public string ParkingLotName { get; set; }

        public string PlateNumber { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime CloseTime { get; set; }

        public bool OrderStatus { get; set; }

        public ParkingOrderEntity ToEntity()
        {
            return new ParkingOrderEntity()
            {
                ParkingLotName = this.ParkingLotName,
                PlateNumber = this.PlateNumber,
                CreateTime = this.CreateTime,
                CloseTime = this.CloseTime,
                OrderStatus = this.OrderStatus,
            };
        }
    }
}
