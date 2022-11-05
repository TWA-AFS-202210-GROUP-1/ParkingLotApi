using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("parkinglots")]
    public class ParkingLotController : ControllerBase
    {
        private readonly ParkingLotService parkinglotService;

        public ParkingLotController(ParkingLotService parkinglotService)
        {
            this.parkinglotService = parkinglotService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingLotDto>>> List()
        {
            var parkinglotDtos = await this.parkinglotService.GetAll();

            return Ok(parkinglotDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetById(int id)
        {
            var companyDto = await this.parkinglotService.GetById(id);
            return Ok(companyDto);
        }

        [HttpPost]
        public async Task<ActionResult<ParkingLotDto>> Add(ParkingLotDto parkinglotDto)
        {
            var id = await this.parkinglotService.AddParkinglot(parkinglotDto);

            return CreatedAtAction(nameof(GetById), new { id = id }, parkinglotDto);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await parkinglotService.DeleteParkingLot(id);

            return this.NoContent();
        }

    }
}
