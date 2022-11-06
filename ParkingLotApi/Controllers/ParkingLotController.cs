using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("parkingLots")]
    public class ParkingLotController: ControllerBase
    {
        private readonly ParkingLotService _parkingLotService;

        public ParkingLotController(ParkingLotService parkingLotService)
        {
            this._parkingLotService = parkingLotService;
        }

        [HttpPost]
        public async Task<ActionResult<ParkingLotDto>> AddParkingLot(ParkingLotDto parkingLotDto)
        {
            int parkingLotId = await this._parkingLotService.AddParkingLot(parkingLotDto);
            return this.Created($"parkingLots/{parkingLotId}", parkingLotDto);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingLotDto>>> GetAllParkingLot([FromQuery] int? pageIndex)
        {
            var parkingLotsList = await this._parkingLotService.GetAllParkingLot((int)pageIndex);
            return this.Ok(parkingLotsList);
        }

        [HttpGet("{parkingLotId}")]
        public async Task<ActionResult<ParkingLotDto>> GetParkingLotById(int parkingLotId)
        {
            var parkingLot = await this._parkingLotService.GetParkingLotById(parkingLotId);
            return this.Ok(parkingLot);
        }

        [HttpDelete("{parkingLotId}")]
        public async Task<ActionResult> DeleteParkingLotById(int parkingLotId)
        {
            await this._parkingLotService.DeleteParkingLotById(parkingLotId);
            return this.NoContent();
        }

        [HttpPost("{parkingLotId}")]
        public async Task<ActionResult<ParkingLotDto>> UpdateParkingLotById([FromRoute]int parkingLotId, [FromBody] ParkingLotDto parkingLotDto)
        {
            return await this._parkingLotService.UpdateParkingLotById(parkingLotId, parkingLotDto);
        }
    }
}
