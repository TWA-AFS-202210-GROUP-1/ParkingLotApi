using ParkingLotApi.Models;
using System;
using System.Data;

namespace ParkingLotApi.Dtos
{
    public class OrderDto
    {
        public OrderDto(Order orderEntity)
        {
            NameOfParkingLot = orderEntity.NameOfParkingLot;
            CarPlateNumber = orderEntity.CarPlateNumber;
            CreateTime = orderEntity.CreateTime;
            CloseTime = orderEntity.CloseTime;
            OrderStatus = orderEntity.OrderStatus;
        }

        public OrderDto()
        {
        }

        public string NameOfParkingLot { get; set; }
        public string CarPlateNumber { get; set; }
        public string CreateTime { get; set; }
        public string CloseTime { get; set; } = string.Empty;
        public string OrderStatus { get; set; }

        public Order ToOrderEntity()
        {
            return new Order()
            {
                NameOfParkingLot = this.NameOfParkingLot,
                CarPlateNumber = this.CarPlateNumber,
                CreateTime = this.CreateTime,
                CloseTime = this.CloseTime,
                OrderStatus = this.OrderStatus,
            };
        }
    }
}