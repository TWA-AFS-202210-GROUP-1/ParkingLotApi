using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Model;
using ParkingLotApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
    public class ParkingLotService
    {
        private readonly ParkingLotDbContext parkinglotDbContext;

        public ParkingLotService(ParkingLotDbContext parkinglotDbContext)
        {
            this.parkinglotDbContext = parkinglotDbContext;
        }

        public async Task<List<ParkingLotDto>> GetAll()
        {
            var parkinglots = parkinglotDbContext.Parkinglots;
            return parkinglots.Select(parkinglotEntity => new ParkingLotDto(parkinglotEntity)).ToList();
        }

        public async Task<ParkingLotDto> GetById(long id)
        {
            var foundParkinglot = await parkinglotDbContext.Parkinglots.FirstOrDefaultAsync(parkinglot => parkinglot.Id == id);

            return new ParkingLotDto(foundParkinglot);
        }

        public async Task<int> AddParkinglot(ParkingLotDto parkinglotDto)
        {
            //convert dto to entity
            ParkingLotEntity parkinglotEntity = parkinglotDto.ToEntity();
            //save entity to db
            await parkinglotDbContext.Parkinglots.AddAsync(parkinglotEntity);
            await parkinglotDbContext.SaveChangesAsync();

            return parkinglotEntity.Id;
        }

        public async Task DeleteParkingLot(int id)
        {
            var foundParkinglot = await parkinglotDbContext.Parkinglots.FirstOrDefaultAsync(parkinglot => parkinglot.Id == id);
            parkinglotDbContext.Parkinglots.Remove(foundParkinglot);
            await parkinglotDbContext.SaveChangesAsync();
        }
    }
}
