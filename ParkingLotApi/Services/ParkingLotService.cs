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

            ParkingLotEntity parkingLotEntity = parkingLotDto.ToParkingLotEntity();

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
            var parkingLotEntity = await this._parkingLotDbContext.ParkingLots.FirstOrDefaultAsync(parkingLot => parkingLot.ParkingLotId.Equals(parkingLotId));
            return new ParkingLotDto(parkingLotEntity);
        }

        public async Task DeleteParkingLotById(int parkingLotId)
        {
            var parkingLotEntity = await this._parkingLotDbContext.ParkingLots.FirstOrDefaultAsync(parkingLot => parkingLot.ParkingLotId.Equals(parkingLotId));
            _parkingLotDbContext.ParkingLots.Remove(parkingLotEntity);
            await _parkingLotDbContext.SaveChangesAsync();
        }

        public async Task<ActionResult<ParkingLotDto>> UpdateParkingLotById(int parkingLotId, ParkingLotDto parkingLotDto)
        {
            var parkingLotEntity = await this._parkingLotDbContext.ParkingLots.FirstOrDefaultAsync(parkingLot => parkingLot.ParkingLotId.Equals(parkingLotId));
            if (parkingLotEntity != null)
            {
                parkingLotEntity.ParkingLotCapacity = parkingLotDto.ParkingLotCapacity;
                _parkingLotDbContext.SaveChangesAsync();
                return new ParkingLotDto(parkingLotEntity);
            }
            else
            {
                return new NotFoundResult();
            }
        }
    }
}
