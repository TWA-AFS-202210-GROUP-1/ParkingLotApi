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
            ParkingLotEntity parkingLotEntity = parkingLotDto.ToParkingLotEntity();

            await _parkingLotDbContext.ParkingLots.AddAsync(parkingLotEntity);
            await _parkingLotDbContext.SaveChangesAsync();

            return parkingLotEntity.ParkingLotId;
        }

        public async Task<List<ParkingLotDto>> GetAllParkingLot()
        {
            return this._parkingLotDbContext.ParkingLots.Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();
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
    }
}
