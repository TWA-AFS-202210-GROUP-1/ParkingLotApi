using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        public async Task<ActionResult<ParkingOrderDto>> Add(ParkingOrderDto parkingOrderDto)
        {
            var id = await this.parkingOrderService.AddParkingOrder(parkingOrderDto);
            return Created($"/orders/{id}", id);
        }

        [HttpGet]
        public async Task<ActionResult<List<ParkingOrderDto>>> GetAll()
        {
            var parkingOrderDtos = await this.parkingOrderService.GetAllParkingOrder();
            return Ok(parkingOrderDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetParkingOrderById([FromRoute] int id)
        {
            var parkingOrderDto = await this.parkingOrderService.GetById(id);
            return Ok(parkingOrderDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ParkingOrderDto>> UpdateParkingOrder(
            [FromRoute] int id,
            [FromBody]ParkingOrderDto parkingOrderDto)
        {
            var updateParkingOrderDto = await this.parkingOrderService.UpdateParkingLot(id, parkingOrderDto);
            return Ok(updateParkingOrderDto);
        }
    }
}
