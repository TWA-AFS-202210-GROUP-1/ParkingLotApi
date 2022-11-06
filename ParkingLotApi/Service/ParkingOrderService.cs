using ParkingLotApi.Model;
using ParkingLotApi.Repository;
using ParkingLotApiTest.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Service
{
    public class ParkingOrderService : IParkingOrderService
    {
        private readonly ParkingLotContext parkingLotContext;

        public ParkingOrderService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<int> AddParkingOrder(ParkingOrderDto parkingOrderDto)
        {
            ParkingOrderEntity parkingOrderEntity = parkingOrderDto.ToEntity();
            var targetParkingLot =  this.parkingLotContext.ParkingLots.FirstOrDefault(parkingLot => parkingLot.Name == parkingOrderDto.ParkingLotName);
            if (isAvailable(targetParkingLot))
            {
                await this.parkingLotContext.ParkingOrders.AddAsync(parkingOrderEntity);
                targetParkingLot.ParkingOrders.Add(parkingOrderEntity);
                await this.parkingLotContext.SaveChangesAsync();
                return parkingOrderEntity.Id;
            }
            else
            {
                throw new Exception("The parking lot is full");
            }
        }

        private bool isAvailable(ParkingLotEntity parkingLotEntity)
        {
            return parkingLotEntity.ParkingOrders.Where(parkingOrderEntity => parkingOrderEntity.OrderStatus == true).Count()<parkingLotEntity.Capacity;
        }
    }
}