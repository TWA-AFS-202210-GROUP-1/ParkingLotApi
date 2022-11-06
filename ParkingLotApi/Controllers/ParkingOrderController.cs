namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Service;
using ParkingLotApiTest.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("parkingOrders")]
public class ParkingOrderController : ControllerBase
{
    private readonly IParkingOrderService parkingOrderService;
    public ParkingOrderController(IParkingOrderService parkingOrderService)
    {
        this.parkingOrderService = parkingOrderService;
    }


    [HttpPost]
    public async Task<ActionResult<int>> AddParkingOrder(ParkingOrderDto parkingOrderDto)
    {
        var id = await this.parkingOrderService.AddParkingOrder(parkingOrderDto);

        return Created($"/parkingOrders/{id}", id);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ParkingOrderDto>> updateParkingOrderByIdAsync([FromRoute] int id, [FromBody] ParkingOrderDto parkingOrderDto)
    {
        var UpdatedParkingOrder = await this.parkingOrderService.UpdateParkingOrderStatus(id, parkingOrderDto);
        return Ok(UpdatedParkingOrder);
    }

}