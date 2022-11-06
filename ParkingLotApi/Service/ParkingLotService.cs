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
        private readonly int pageSize = 15;

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

        public async Task<ParkingLotDto> GetById(int id)
        {
            var targetParkingLot = this.parkingLotContext.ParkingLots.FirstOrDefault(parkingLot => parkingLot.Id == id);
            return new ParkingLotDto(targetParkingLot);
        }

        public async Task<List<ParkingLotDto>> GetByPageIndex(int? pageIndex)
        {
            var parkingLotsDtos = this.parkingLotContext.ParkingLots.ToList().Select(parkingLotEntity => new ParkingLotDto(parkingLotEntity));
            return parkingLotsDtos.Skip((pageIndex.Value - 1) * pageSize).Take(pageSize).ToList();
        }
    }
}