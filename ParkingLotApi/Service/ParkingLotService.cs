using ParkingLotApi.Model;
using ParkingLotApi.Repository;
using ParkingLotApiTest.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkingLotApi.Service
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
            ParkingLotEntity parkingLotEntity = parkingLotDto.ToEntity();
            await this.parkingLotContext.AddAsync(parkingLotEntity);
            await this.parkingLotContext.SaveChangesAsync();
            return parkingLotEntity.Id;
        }

        public async Task deleteParkingLot(int id)
        {
            var targetParkingLot = this.parkingLotContext.ParkingLots.FirstOrDefault(parkingLot => parkingLot.Id == id);
            parkingLotContext.Remove(targetParkingLot);
            await this.parkingLotContext.SaveChangesAsync();
        }

        public async Task<List<ParkingLotDto>> GetAll()
        {
            var parkingLots = this.parkingLotContext.ParkingLots.ToList();
            return parkingLots.Select(parkingLot => new ParkingLotDto(parkingLot)).ToList();
        }
    }
}