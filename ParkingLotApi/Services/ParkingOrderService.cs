using System;
using System.Collections.Generic;
using System.IO.Enumeration;
using System.Linq;
using System.Net;
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
                return parkingOrderEntity.Id;
            }

            throw new Exception(Const.OrderStatus.FailMessage);
        }

        public async Task<List<ParkingOrderDto>> GetAllParkingOrder()
        {
            var parkingOrders = this.parkingLotContext.ParkingOrders.ToList();
            return parkingOrders.Select(parkingOrderEntity => new ParkingOrderDto(parkingOrderEntity)).ToList();
        }

        public async Task<ParkingOrderDto?> GetById(int id)
        {
            var findParkingOrder = await this.parkingLotContext.ParkingOrders
                .FirstOrDefaultAsync(parkingOrder => parkingOrder.Id == id);
            return new ParkingOrderDto(findParkingOrder);
        }

        public async Task<ParkingOrderDto> UpdateParkingLot(int id, ParkingOrderDto parkingOrderDto)
        {
            var findParkingOrder = await this.parkingLotContext.ParkingOrders
                .FirstOrDefaultAsync(parkingOrder => parkingOrder.Id == id);
            findParkingOrder.OrderStatus = Const.OrderStatus.Close;
            findParkingOrder.CloseTime = parkingOrderDto.CloseTime;
            this.parkingLotContext.ParkingOrders.Update(findParkingOrder);
            await this.parkingLotContext.SaveChangesAsync();
            return new ParkingOrderDto(findParkingOrder);
        }
    }
}
