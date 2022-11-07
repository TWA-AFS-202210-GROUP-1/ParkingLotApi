using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Services;
using System.Threading.Tasks;

namespace ParkingLotApi.Controllers
{
  [ApiController]
  [Route("orders")]
  public class ParkingOrderController : ControllerBase
  {
    private readonly IParkingOrderService parkingOrderService;

    public ParkingOrderController(IParkingOrderService parkingOrderService)
    {
      this.parkingOrderService = parkingOrderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(ParkingOrderDto parkingOrderDto)
    {
      try
      {
        var id = await parkingOrderService.CreateOrder(parkingOrderDto);

        return Created("/orders", id);
      }
      catch (ParkingLotNotFoundException exception)
      {
        return NotFound(exception.Message);
      }
      catch (ParkingLotFullException exception)
      {
        return Conflict(exception.Message);
      }
    }

    [HttpGet("{orderId}")]
    public IActionResult GetById([FromRoute] int orderId)
    {
      try
      {
        var parkingOrderDto = parkingOrderService.GetById(orderId);

        return Ok(parkingOrderDto);
      }
      catch (ParkingOrderNotFoundException exception)
      {
        return NotFound(exception.Message);
      }
    }

    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrderStatus([FromRoute] int orderId, ParkingOrderDto parkingOrderDto)
    {
      try
      {
        var updatedParkingOrderDto = await parkingOrderService.UpdateStatus(orderId, parkingOrderDto);

        return Ok(updatedParkingOrderDto);
      }
      catch (ParkingOrderNotFoundException exception)
      {
        return NotFound(exception.Message);
      }
    }
  }
}