using Microsoft.EntityFrameworkCore;
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
            var targetParkingLot =  this.parkingLotContext.ParkingLots.Include(_ => _.ParkingOrders).FirstOrDefault(parkingLot => parkingLot.Name == parkingOrderDto.ParkingLotName);
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

        public async Task<ParkingOrderDto> UpdateParkingOrderStatus(int id, ParkingOrderDto parkingOrderDto)
        {
            var targetOrder = this.parkingLotContext.ParkingOrders.FirstOrDefault(parkingOrder => parkingOrder.Id == id);
            targetOrder.OrderStatus = parkingOrderDto.OrderStatus;
            targetOrder.CloseTime = parkingOrderDto.CloseTime;
            this.parkingLotContext.ParkingOrders.Update(targetOrder);
            await this.parkingLotContext.SaveChangesAsync();
            return new ParkingOrderDto(targetOrder);
        }

        private bool isAvailable(ParkingLotEntity parkingLotEntity)
        {
            return parkingLotEntity.ParkingOrders.Where(parkingOrderEntity => parkingOrderEntity.OrderStatus == true).Count()<parkingLotEntity.Capacity;
        }
    }
}