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
    public IActionResult GetAll()
    {
      var parkingLotDtos = parkingLotService.GetAll();

      return Ok(parkingLotDtos);
    }
  }
}