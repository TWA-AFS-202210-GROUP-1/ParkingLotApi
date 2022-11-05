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
    public IActionResult GetParkingLots([FromQuery] int? page)
    {
      if (page == null)
      {
        var parkingLotDtos = parkingLotService.GetAll();

        return Ok(parkingLotDtos);
      }
      else
      {
        var parkingLotDtos = parkingLotService.GetPage(page.Value);

        return Ok(parkingLotDtos);
      }
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id)
    {
      var parkingLotDto = parkingLotService.GetById(id);

      return Ok(parkingLotDto);
    }
  }
}