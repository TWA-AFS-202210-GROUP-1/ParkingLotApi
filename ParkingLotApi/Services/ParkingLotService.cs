﻿using System.Collections.Generic;
using System.Linq;
using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Repository;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Models;

namespace ParkingLotApi.Services
{
    public class ParkingLotService : IParkingLotService
    {
        private const int MaxParkingLotNumberInOnePage = 15;
        private readonly ParkingLotContext _context;
        public ParkingLotService(ParkingLotContext context)
        {
            _context = context;
        }
        public async Task<CreatedParkingLotDto> CreateParkingLot(ParkingLotDto parkingLot)
        {
            if (parkingLot.Capacity < 0)
            {
                throw new InvalidParkingLotDtoException("Capacity cannot be minus.");
            }
            var parkingLotEntity = parkingLot.ToEntity();
            await _context.ParkingLots.AddAsync(parkingLotEntity);
            await _context.SaveChangesAsync();

            return new CreatedParkingLotDto(parkingLotEntity);
        }

        public async Task DeleteParkingLot(int parkingLotId)
        {
            var foundEntity = await _context.ParkingLots.FirstOrDefaultAsync(_ => _.Id.Equals(parkingLotId));
            if (foundEntity == null)
            {
                throw new NotFoundParkingLotException($"Can not find a parking lot with id : {parkingLotId}");
            }
            _context.ParkingLots.Remove(foundEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ParkingLotDto>> GetParkingLotsByPageNumber(int pageNumber)
        {
            var allEntities = await _context.ParkingLots.ToListAsync();
            if (IsPageOutOfIndex(pageNumber, allEntities))
            {
                return new List<ParkingLotDto>();
            }

            return allEntities
                .Skip((pageNumber - 1) * MaxParkingLotNumberInOnePage)
                .Take(MaxParkingLotNumberInOnePage)
                .OrderBy(_ => _.Id)
                .Select(_ => new ParkingLotDto(_))
                .ToList();
        }

        private bool IsPageOutOfIndex(int pageNumber, List<ParkingLotEntity> allEntities)
        {
            return allEntities.Count < MaxParkingLotNumberInOnePage * (pageNumber - 1);
        }
    }
}
