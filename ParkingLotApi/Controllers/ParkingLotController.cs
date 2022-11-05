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
    private readonly IParkingLotService parkingLotService;

    public ParkingLotController(IParkingLotService parkingLotService)
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