using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Services;
using System.Threading.Tasks;

namespace ParkingLotApi.Controllers
{
  [ApiController]
  [Route("parking-lots")]
  public class ParkingLotController : ControllerBase
  {
    private readonly IParkingLotService parkingLotService;

    public ParkingLotController(IParkingLotService parkingLotService)
    {
      this.parkingLotService = parkingLotService;
    }

    [HttpPost]
    public async Task<IActionResult> AddParkingLot(ParkingLotDto parkingLotDto)
    {
      try
      {
        var id = await parkingLotService.AddParkingLot(parkingLotDto);

        return Created("/parking-lots", id);
      }
      catch (DuplicateParkingLotNameException exception)
      {
        return Conflict(exception.Message);
      }
    }

    [HttpGet]
    public IActionResult GetParkingLots([FromQuery] int? page)
    {
      if (page == null)
      {
        var parkingLotDtos = parkingLotService.GetAll();

        return Ok(parkingLotDtos);
      }
      else
      {
        var parkingLotDtos = parkingLotService.GetByPageIndex(page.Value);

        return Ok(parkingLotDtos);
      }
    }

    [HttpGet("{parkingLotId}")]
    public IActionResult GetById([FromRoute] int parkingLotId)
    {
      var parkingLotDto = parkingLotService.GetById(parkingLotId);

      return Ok(parkingLotDto);
    }

    [HttpPut("{parkingLotId}")]
    public async Task<IActionResult> UpdateParkingLot([FromRoute] int parkingLotId, ParkingLotDto parkingLotDto)
    {
      var updatedParkingLotDto = await parkingLotService.UpdateCapacity(parkingLotId, parkingLotDto);

      return Ok(updatedParkingLotDto);
    }

    [HttpDelete("{parkingLotId}")]
    public async Task<IActionResult> DeleteById([FromRoute] int parkingLotId)
    {
      await parkingLotService.RemoveParkingLot(parkingLotId);

      return NoContent();
    }
  }
}