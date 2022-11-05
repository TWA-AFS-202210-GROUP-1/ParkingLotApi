using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;
using System.Collections.Generic;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("parkingLots")]
    public class ParkingLotController: ControllerBase
    {
        private readonly ParkingLotService parkingLotService;

        public ParkingLotController(ParkingLotService parkingLotService)
        {
            this.parkingLotService = parkingLotService;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingLotDto>> Add(ParkingLotDto parkingLotDto)
        {
            var id = await this.parkingLotService.AddParkingLot(parkingLotDto);
            return Created($"/parkingLots/{id}", id);
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

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete([FromRoute] int id)
        {
            await this.parkingLotService.DeleteParkingLot(id);
            return NoContent();
        }
    }
}
