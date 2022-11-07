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
        try
        {
          var parkingLotDtos = parkingLotService.GetAll();

          return Ok(parkingLotDtos);
        }
        catch (ParkingLotNotFoundException exception)
        {
          return NotFound(exception.Message);
        }
      }
      else
      {
        try
        {
          var parkingLotDtos = parkingLotService.GetByPageIndex(page.Value);

          return Ok(parkingLotDtos);
        }
        catch (ParkingLotNotFoundException exception)
        {
          return NotFound(exception.Message);
        }
        catch (ParkingLotPageIndexOutOfRangeException exception)
        {
          return NotFound(exception.Message);
        }
      }
    }

    [HttpGet("{parkingLotId}")]
    public IActionResult GetById([FromRoute] int parkingLotId)
    {
      try
      {
        var parkingLotDto = parkingLotService.GetById(parkingLotId);

        return Ok(parkingLotDto);
      }
      catch (ParkingLotNotFoundException exception)
      {
        return NotFound(exception.Message);
      }
    }

    [HttpPut("{parkingLotId}")]
    public async Task<IActionResult> UpdateParkingLot([FromRoute] int parkingLotId, ParkingLotDto parkingLotDto)
    {
      try
      {
        var updatedParkingLotDto = await parkingLotService.UpdateCapacity(parkingLotId, parkingLotDto);

        return Ok(updatedParkingLotDto);
      }
      catch (ParkingLotNotFoundException exception)
      {
        return NotFound(exception.Message);
      }
      catch (InvalidParkingLotCapacityException exception)
      {
        return Conflict(exception.Message);
      }
    }

    [HttpDelete("{parkingLotId}")]
    public async Task<IActionResult> DeleteById([FromRoute] int parkingLotId)
    {
      try
      {
        await parkingLotService.RemoveParkingLot(parkingLotId);

        return NoContent();
      }
      catch (ParkingLotNotFoundException exception)
      {
        return NotFound(exception.Message);
      }
    }
  }
}