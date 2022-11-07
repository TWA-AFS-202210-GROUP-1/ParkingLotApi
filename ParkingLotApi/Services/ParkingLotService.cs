using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Services
{
    public class ParkingLotService : IParkingLotService
    {
        private readonly ParkingLotContext parkingLotContext;

        public ParkingLotService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            var parkingLotEntity = parkingLotDto.ToEntity();
            await this.parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
            await this.parkingLotContext.SaveChangesAsync();
            return parkingLotEntity.Id;
        }

        public async Task<List<ParkingLotDto>> GetAllParkingLot()
        {
            var parkingLots = this.parkingLotContext.ParkingLots.ToList();
            return parkingLots.Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();
        }

        public async Task DeleteParkingLot(int id)
        {
            var findParkingLot = await this.parkingLotContext.ParkingLots
                .FirstOrDefaultAsync(parkingLot => parkingLot.Id == id);
            this.parkingLotContext.ParkingLots.Remove(findParkingLot);
            await this.parkingLotContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ParkingLotDto>> GetParkingLotByRange(int pageIndex)
        {
            const int pageSize = 15;
            var parkingLots = this.parkingLotContext.ParkingLots.ToList();
            var parkingLotDtos = parkingLots
                .Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();

            return parkingLotDtos.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public async Task<ParkingLotDto?> GetById(int id)
        {
            var findParkingLot = await this.parkingLotContext.ParkingLots
                .FirstOrDefaultAsync(parkingLot => parkingLot.Id == id);
            return new ParkingLotDto(findParkingLot);
        }

        public async Task<ParkingLotDto> UpdateParkingLot(int id, ParkingLotDto parkingLotDto)
        {
            var findParkingLot = await this.parkingLotContext.ParkingLots
                .FirstOrDefaultAsync(parkingLot => parkingLot.Id == id);
            findParkingLot.Capacity = parkingLotDto.Capacity;
            this.parkingLotContext.ParkingLots.Update(findParkingLot);
            await this.parkingLotContext.SaveChangesAsync();
            return new ParkingLotDto(findParkingLot);
        }
    }
}
