using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("parkinglots")]
    public class ParkingLotController: ControllerBase
    {
        private readonly IParkingLotService parkingLotService;

        public ParkingLotController(IParkingLotService parkingLotService)
        {
            this.parkingLotService = parkingLotService;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingLotDto>> Add([FromBody] ParkingLotDto parkingLotDto)
        {
            var id = await this.parkingLotService.AddParkingLot(parkingLotDto);
            return Created($"/parkinglots/{id}", id);
        }

        [HttpGet]
        public async Task<ActionResult<List<ParkingLotDto>>> GetAllParkingLots([FromQuery] int? pageIndex)
        {
            if (pageIndex == null)
            {
                var parkingLotDto = await this.parkingLotService.GetAllParkingLot();
                return Ok(parkingLotDto);
            }
            else
            {
                var parkingLotDtos = await this.parkingLotService.GetParkingLotByRange(pageIndex.Value);
                return Ok(parkingLotDtos);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetParkingLotById([FromRoute] int id)
        {
            var parkingLotDto = await this.parkingLotService.GetById(id);
            return Ok(parkingLotDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromRoute] int id)
        {
            await this.parkingLotService.DeleteParkingLot(id);
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ParkingLotDto>> Update([FromRoute] int id, [FromBody] ParkingLotDto parkingLotDto)
        {
            var modifiedParkingLot = await this.parkingLotService.UpdateParkingLot(id, parkingLotDto);
            return Ok(modifiedParkingLot);
        }
    }
}
