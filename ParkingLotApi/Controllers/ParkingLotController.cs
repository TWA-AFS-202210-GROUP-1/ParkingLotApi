using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ParkingLotApi.Controllers
{
  [ApiController]
  [Route("parking-lots")]
  public class ParkingLotController : ControllerBase
  {
    private readonly ParkingLotService parkingLotService;

    public ParkingLotController(ParkingLotService parkingLotService)
    {
      this.parkingLotService = parkingLotService;
    }

    [HttpPost]
    public async Task<IActionResult> AddParkingLot(ParkingLotDto parkingLotDto)
    {
      var id = await parkingLotService.AddParkingLot(parkingLotDto);

      return Created("/parking-lots", id);
    }

    [HttpGet]
    public IActionResult GetParkingLots([FromQuery] int? pageIndex)
    {
      if (pageIndex == null)
      {
        var parkingLotDtos = parkingLotService.GetAll();

        return Ok(parkingLotDtos);
      }
      else
      {
        var parkingLotDtos = parkingLotService.GetByPageIndex(pageIndex.Value);

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
    public IActionResult UpdateCapacity([FromRoute] int parkingLotId, int newCapacity)
    {
      return Ok();
    }

    [HttpDelete("{parkingLotId}")]
    public async Task<IActionResult> DeleteById([FromRoute] int parkingLotId)
    {
      await parkingLotService.RemoveParkingLot(parkingLotId);

      return NoContent();
    }
  }
}