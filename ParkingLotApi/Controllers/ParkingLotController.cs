using System;
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
            var resultparkinglotDtos = new List<ParkingLotDto>();
            foreach (var parkinglot in parkinglotDtos)
            {
                resultparkinglotDtos.Add(new ParkingLotDto(parkinglot.Name, parkinglot.Capacity));
            }

            return Ok(parkinglotDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetById(int id, int? size, string? details)
        {
            if (size != null)
            {
                var resultParkinglots = new List<ParkingLotDto>();
                var parkinglotDtos = await this.parkinglotService.GetAll();
                if (id < parkinglotDtos.Count)
                {
                    for (int i = id; i < Math.Min(parkinglotDtos.Count, id + 15); i++)
                    {
                        resultParkinglots.Add(new ParkingLotDto(parkinglotDtos[i - 1].Name, parkinglotDtos[i - 1].Capacity));
                    }

                    return Ok(resultParkinglots);
                }

                return NoContent();
            }

            if (details != null)
            {
                var detailedparkinglotDto = await this.parkinglotService.GetById(id);
                return Ok(detailedparkinglotDto);
            }

            var parkinglotDto = await this.parkinglotService.GetById(id);
            parkinglotDto = new ParkingLotDto(parkinglotDto.Name, parkinglotDto.Capacity);
            return Ok(parkinglotDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetById([FromRoute] int id, ParkingLotDto parkinglot)
        {
            var parkinglotDto = await this.parkinglotService.GetById(id);
            if (parkinglot.Capacity > parkinglotDto.Capacity)
            {
                parkinglotDto.Capacity = parkinglot.Capacity;
            }

            return Ok(parkinglotDto);
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
