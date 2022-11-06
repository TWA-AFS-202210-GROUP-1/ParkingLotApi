using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Services
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
            var parkingOrderEntity = parkingOrderDto.ToEntity();
            var parkingLotName = parkingOrderDto.ParkingLotName;

            var findParkingLot = await this.parkingLotContext.ParkingLots
                .FirstOrDefaultAsync(parkingLot => parkingLot.Name == parkingLotName);

            var count = Enumerable.Count(
                this.parkingLotContext.ParkingOrders,
                parkingOrder => parkingOrder.ParkingLotName == parkingLotName);

            if (count < findParkingLot.Capacity)
            {
                await this.parkingLotContext.ParkingOrders.AddAsync(parkingOrderEntity);
                await this.parkingLotContext.SaveChangesAsync();
            }

            return parkingOrderEntity.Id;
        }

        public async Task<List<ParkingOrderDto>> GetAllParkingOrder()
        {
            var parkingOrders = this.parkingLotContext.ParkingOrders.ToList();
            return parkingOrders.Select(parkingOrderEntity => new ParkingOrderDto(parkingOrderEntity)).ToList();
        }
    }
}
