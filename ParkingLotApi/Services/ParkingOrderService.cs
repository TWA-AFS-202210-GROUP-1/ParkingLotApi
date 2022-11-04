using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;

namespace ParkingLotApi.Services;

public class ParkingOrderService :IParkingOrderService
{
    private readonly ParkingLotContext _context;
    public ParkingOrderService(ParkingLotContext context)
    {
        _context = context;
    }
    public async Task<ParkingOrderDto> CreateParkingOrder(CreateParkingOrderDto createParkingOrderDto)
    {
        var parkingOrderEntity = createParkingOrderDto.ToEntity();
        var foundParkingLotEntity = await _context.ParkingLots.Include(_ => _.Orders).FirstOrDefaultAsync(_ => _.Id.Equals(createParkingOrderDto.ParkingLotId));
        if (foundParkingLotEntity == null)
        {
            throw new NotFoundEntityException(
                $"Can not find parking lot with id: {createParkingOrderDto.ParkingLotId}");

        }

        if (foundParkingLotEntity.IsFull())
        {
            throw new FullParkingLotException($"Parking Lot {createParkingOrderDto.ParkingLotId} is full.");

        }
        foundParkingLotEntity.Orders.Add(parkingOrderEntity);
        await _context.SaveChangesAsync();
        return new ParkingOrderDto(parkingOrderEntity, foundParkingLotEntity.Name);
    }
}