using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<ParkingLotDto>> GetAll()
        {
            var parkingLots = parkingLotContext.ParkingLots.ToList();
            return parkingLots.Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity)).ToList();
        }
    }
}
