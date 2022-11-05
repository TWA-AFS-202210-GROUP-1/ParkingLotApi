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
            var id = await this.parkingLotService.AddParkingLot(parkingLotDto);
            return Created($"/parkingLots/{id}", id);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingLotDto>>> List()
        {
            var companyDtos = await this.parkingLotService.GetAll();
            return Ok(companyDtos);
        }
    }
}
