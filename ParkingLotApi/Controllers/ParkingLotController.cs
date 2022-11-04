using ParkingLotApi.Dtos;
using ParkingLotApi.Services;
using System.Threading.Tasks;

namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> CreateParkingLot([FromBody] ParkingLotDto parkingLot)
    {
        var createdParkingLot = await _service.CreateParkingLot(parkingLot);
        return Created($"api/ParkingLot/{createdParkingLot.Id}", createdParkingLot);
    }
}