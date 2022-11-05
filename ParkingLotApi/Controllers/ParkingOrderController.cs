using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ParkingLotApi.Controllers
{
  [ApiController]
  [Route("orders")]
  public class ParkingOrderController : ControllerBase
  {
    private readonly ParkingOrderService parkingOrderService;

    public ParkingOrderController(ParkingOrderService parkingOrderService)
    {
      this.parkingOrderService = parkingOrderService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(ParkingOrderDto parkingOrderDto)
    {
      var id = await parkingOrderService.CreateOrder(parkingOrderDto);

      return Created("/orders", id);
    }

    [HttpGet("{orderId}")]
    public IActionResult GetById([FromRoute] int orderId)
    {
      var parkingOrderDto = parkingOrderService.GetById(orderId);

      return Ok(parkingOrderDto);
    }

    [HttpPut("{orderId}")]
    public async Task<IActionResult> UpdateOrderStatus([FromRoute] int orderId, ParkingOrderDto parkingOrderDto)
    {
      var updatedParkingOrderDto = await parkingOrderService.UpdateStatus(orderId, parkingOrderDto);

      return Ok(updatedParkingOrderDto);
    }
  }
}