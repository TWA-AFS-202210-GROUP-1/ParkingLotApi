using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
    public class ParkingLotService
    {
        private readonly ParkingLotContext _parkingLotDbContext;

        public ParkingLotService(ParkingLotContext parkingLotDbContext)
        {
            this._parkingLotDbContext = parkingLotDbContext;
        }

        public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            if (parkingLotDto.ParkingLotCapacity < 0)
            {
                throw new ArgumentException("Capacity Invalid");
            }

            ParkingLot parkingLotEntity = parkingLotDto.ToParkingLotEntity();

            await _parkingLotDbContext.ParkingLots.AddAsync(parkingLotEntity);
            await _parkingLotDbContext.SaveChangesAsync();

            return parkingLotEntity.ParkingLotId;
        }

        public async Task<IEnumerable<ParkingLotDto>> GetAllParkingLot(int pageIndex)
        {
            var allParkingLots = this._parkingLotDbContext.ParkingLots.Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();
            if (pageIndex != null)
            {
                return allParkingLots.Skip((pageIndex - 1) * 15).Take(15);
            }
            else
            {
                return allParkingLots;
            }
        }

        public async Task<ParkingLotDto> GetParkingLotById(int parkingLotId)
        {
            var parkingLotEntity = await this._parkingLotDbContext.ParkingLots
                .Include(parkingLot => parkingLot.OrdersListEntity)
                .FirstOrDefaultAsync(parkingLot => parkingLot.ParkingLotId.Equals(parkingLotId));
            return new ParkingLotDto(parkingLotEntity);
        }

        public async Task DeleteParkingLotById(int parkingLotId)
        {
            var parkingLotEntity = await this._parkingLotDbContext.ParkingLots
                .FirstOrDefaultAsync(parkingLot => parkingLot.ParkingLotId.Equals(parkingLotId));
            //_parkingLotDbContext.Orders.Remove(parkingLotEntity.OrdersListEntity);
            _parkingLotDbContext.ParkingLots.Remove(parkingLotEntity);
            await _parkingLotDbContext.SaveChangesAsync();
        }

        public async Task<ActionResult<ParkingLotDto>> UpdateParkingLotById(int parkingLotId, ParkingLotDto parkingLotDto)
        {
            var parkingLotEntity = await this._parkingLotDbContext.ParkingLots
                .Include(parkingLot => parkingLot.OrdersListEntity)
                .FirstOrDefaultAsync(parkingLot => parkingLot.ParkingLotId.Equals(parkingLotId));
            if (parkingLotEntity != null)
            {
                if (parkingLotEntity.ParkingLotCapacity != parkingLotDto.ParkingLotCapacity)
                {
                    parkingLotEntity.ParkingLotCapacity = parkingLotDto.ParkingLotCapacity;
                    return new ParkingLotDto(parkingLotEntity);
                }
                int parkingCarNumber = parkingLotEntity.OrdersListEntity.Select(orderEntity => orderEntity.OrderStatus.Equals("Open")).ToList().Count();
                if ((parkingLotEntity.ParkingLotCapacity - parkingCarNumber) > 0)
                {
                    if (string.IsNullOrEmpty(parkingLotDto.OrdersList[0].CloseTime))
                    {

                        parkingLotEntity.OrdersListEntity.Add(
                            parkingLotDto.OrdersList.Select(orderDto => orderDto.ToOrderEntity()).ToList()[0]);
                        //parkingLotEntity.OrdersListEntity.Clear();
                        //parkingLotEntity.OrdersListEntity.AddRange(parkingLotDto.OrdersList
                        //    .Select(orderDto => orderDto.ToOrderEntity()).ToList());
                        _parkingLotDbContext.SaveChanges();
                        return new ParkingLotDto(parkingLotEntity);
                    }
                    else
                    {
                        var orderEntity = parkingLotEntity.OrdersListEntity.Find(orderEntity => orderEntity.CarPlateNumber.Equals(parkingLotDto.OrdersList[0].CarPlateNumber));
                        orderEntity.CloseTime = parkingLotDto.OrdersList[0].CloseTime;
                        orderEntity.OrderStatus = parkingLotDto.OrdersList[0].OrderStatus;
                        _parkingLotDbContext.SaveChanges();
                        return new ParkingLotDto(parkingLotEntity);
                    }
                }
                else
                {
                    throw new ArgumentException("The parking lot is full");
                }
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}
