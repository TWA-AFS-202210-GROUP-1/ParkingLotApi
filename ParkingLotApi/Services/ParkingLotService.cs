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
    public class ParkingLotService
    {
        private readonly ParkingLotContext parkingLotContext;

        public ParkingLotService(ParkingLotContext parkingLotContext)
        {
            this.parkingLotContext = parkingLotContext;
        }

        public async Task<int> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            var parkingLotEntity = parkingLotDto.ToEntity();
            await parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
            await parkingLotContext.SaveChangesAsync();
            return parkingLotEntity.Id;
        }

        public async Task<List<ParkingLotDto>> GetAllParkingLot()
        {
            var parkingLots = parkingLotContext.ParkingLots.ToList();
            return parkingLots.Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();
        }

        public async Task DeleteParkingLot(int id)
        {
            var findParkingLot = await parkingLotContext.ParkingLots
                .FirstOrDefaultAsync(parkingLot => parkingLot.Id == id);
            parkingLotContext.ParkingLots.Remove(findParkingLot);
            await parkingLotContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<ParkingLotDto>> GetParkingLotByRange(int pageIndex)
        {
            const int pageSize = 15;
            var parkingLots = parkingLotContext.ParkingLots.ToList();
            var parkingLotDtos = parkingLots
                .Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();

            return parkingLotDtos.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }
    }
}
