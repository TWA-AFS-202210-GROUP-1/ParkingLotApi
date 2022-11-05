using System;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;
using System.Threading.Tasks;

namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

[ApiController]
[Route("api/ParkingLots")]
public class ParkingLotController : ControllerBase
{
    private readonly IParkingLotService _service;

    public ParkingLotController(IParkingLotService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateParkingLot([FromBody] CreateOrUpdateParkingLotDto orUpdateParkingLot)
    {
        var createdParkingLot = await _service.CreateParkingLot(orUpdateParkingLot);
        return Created($"api/ParkingLot/{createdParkingLot.Id}", createdParkingLot);
    }

    [HttpDelete("{parkingLotId}")]
    public async Task<IActionResult> DeleteParkingLot([FromRoute] int parkingLotId)
    {
        await _service.DeleteParkingLot(parkingLotId);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetParkingLotsByPageNumber([FromQuery] [Range(1, int.MaxValue)] int pageNumber)
    {
        var pages = await _service.GetParkingLotsByPageNumber(pageNumber);
        return Ok(pages);
    }

    [HttpGet("{parkingLotId}")]
    public async Task<IActionResult> GetParkingLotById([FromRoute] int parkingLotId)
    {
        var foundParkingLot = await _service.GetParkingLotById(parkingLotId);
        return Ok(foundParkingLot);
    }

    [HttpPut("{parkingLotId}")]
    public async Task<IActionResult> UpdateParkingLotById([FromRoute] int parkingLotId, [FromBody] CreateOrUpdateParkingLotDto newParkingLot)
    {
        var foundParkingLot = await _service.UpdateParkingLotById(parkingLotId, newParkingLot);
        return Ok(foundParkingLot);
    }
}