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

}