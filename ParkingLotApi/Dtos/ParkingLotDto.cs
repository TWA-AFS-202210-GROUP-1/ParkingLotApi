using ParkingLotApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace ParkingLotApi.Dtos
{
    public class ParkingLotDto
    {
        public ParkingLotDto(ParkingLot parkingLotEntity)
        {
            this.ParkingLotName = parkingLotEntity.ParkingLotName;
            this.ParkingLotCapacity = parkingLotEntity.ParkingLotCapacity;
            this.ParkingLotLocation = parkingLotEntity.ParkingLotLocation;
            this.OrdersList = parkingLotEntity.OrdersListEntity.Select(orderEntity => new OrderDto(orderEntity)).ToList();
        }

        public ParkingLotDto()
        {
        }

        public string ParkingLotName { get; set; }

        public int ParkingLotCapacity { get; set; }

        public string ParkingLotLocation { get; set; }

        public List<OrderDto>? OrdersList { get; set; }

        public ParkingLot ToParkingLotEntity()
        {
            return new ParkingLot()
            {
                ParkingLotName = this.ParkingLotName,
                ParkingLotCapacity = this.ParkingLotCapacity,
                ParkingLotLocation = this.ParkingLotLocation,
                OrdersListEntity = OrdersList?.Select(OrderDto => OrderDto.ToOrderEntity()).ToList(),
            };
        }
    }
}