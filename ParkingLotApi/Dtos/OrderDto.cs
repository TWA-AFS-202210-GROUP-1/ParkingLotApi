using ParkingLotApi.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Dtos
{
    public class OrderDto
    {
        public OrderDto()
        {
        }

        public OrderDto(OrderEntity orderEntity)
        {
            Ordernumber = orderEntity.Ordernumber;
            NameofParkinglot = orderEntity.NameofParkinglot;
            PlateNumber = orderEntity.PlateNumber;
            CreationTime = orderEntity.CreationTime;
            CloseTime = orderEntity.CloseTime;
            Status = orderEntity.Status;
        }

        public int Ordernumber { get; set; }

        public int NameofParkinglot { get; set; }

        public string PlateNumber { get; set; }

        public string CreationTime { get; set; }

        public string CloseTime { get; set; }

        public bool Status { get; set; }

        public OrderEntity ToEntity()
        {
            return new OrderEntity()
            {
                Ordernumber = this.Ordernumber,
                NameofParkinglot = this.NameofParkinglot,
                PlateNumber = this.PlateNumber,
                CreationTime = this.CreationTime,
                CloseTime = this.CloseTime,
                Status = this.Status,
            };
        }
    }
}
