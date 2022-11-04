using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Repository;
using System.Threading.Tasks;

namespace ParkingLotApi.Services
{
    public class ParkingLotService : IParkingLotService
    {
        private readonly ParkingLotContext _context;
        public ParkingLotService(ParkingLotContext context)
        {
            _context = context;
        }
        public async Task<CreatedParkingLotDto> CreateParkingLot(ParkingLotDto parkingLot)
        {
            if (parkingLot.Capacity < 0)
            {
                throw new ParkingLotDtoInvalidException("Capacity cannot be minus.");
            }
            var parkingLotEntity = parkingLot.ToEntity();
            await _context.ParkingLots.AddAsync(parkingLotEntity);
            await _context.SaveChangesAsync();

            return new CreatedParkingLotDto(parkingLotEntity);
        }
    }
}
