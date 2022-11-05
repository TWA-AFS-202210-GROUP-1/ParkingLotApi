using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;
using System.Collections.Generic;

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
            var id = await parkingLotService.AddParkingLot(parkingLotDto);
            return Created($"/parkingLots/{id}", id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingLotDto>>> GetAll()
        {
            var companyDtos = await parkingLotService.GetAllParkingLot();
            return Ok(companyDtos);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> Delete([FromRoute] int id)
        {
            await parkingLotService.DeleteParkingLot(id);
            return NoContent();
        }
    }
}
