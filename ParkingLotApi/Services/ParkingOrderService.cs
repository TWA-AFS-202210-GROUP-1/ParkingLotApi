using System.Linq;
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
        var foundParkingLotEntity = await _context.ParkingLots.Include(_ => _.Orders).FirstOrDefaultAsync(_ => _.Id.Equals(createParkingOrderDto.ParkingLotId));
        if (foundParkingLotEntity == null)
        {
            throw new NotFoundEntityException(
                $"Can not find parking lot with id: {createParkingOrderDto.ParkingLotId}");

        }

        if (foundParkingLotEntity.IsFull())
        {
            throw new FullParkingLotException($"The parking lot is full.");

        }
        var parkingOrderEntity = createParkingOrderDto.ToEntity();
        foundParkingLotEntity.Orders.Add(parkingOrderEntity);
        await _context.SaveChangesAsync();
        return new ParkingOrderDto(parkingOrderEntity, foundParkingLotEntity.Name);
    }

    public async Task<ParkingOrderDto> UpdateOrderStatus(int orderId, UpdateParkingOrderDto updateParkingOrderDto)
    {
        var parkingOrderEntity = await _context.ParkingOrders.FirstOrDefaultAsync(_ => _.Id.Equals(orderId));
        if (parkingOrderEntity == null)
        {
            throw new NotFoundEntityException(
                $"Can not find parking order with id: {orderId}");
        }
        updateParkingOrderDto.UpdateEntity(parkingOrderEntity);
        await _context.SaveChangesAsync();
        var correlatedLot = await _context.ParkingLots
            .Include(_=>_.Orders)
            .FirstOrDefaultAsync(parkingLot => parkingLot.Orders.Any(order => order.Id.Equals(orderId)));
        return new ParkingOrderDto(parkingOrderEntity, correlatedLot.Name);
    }
}