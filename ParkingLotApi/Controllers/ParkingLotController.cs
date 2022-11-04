using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace ParkingLotApi.Controllers
{
  [ApiController]
  [Route("parking-lots")]
  public class ParkingLotController : ControllerBase
  {
    private readonly ParkingLotDbContext parkingLotDbContext;

    public ParkingLotController(ParkingLotDbContext parkingLotDbContext)
    {
      this.parkingLotDbContext = parkingLotDbContext;
    }

    [HttpPost]
    public async Task<IActionResult> AddParkingLot(ParkingLotDto parkingLotDto)
    {
      var parkingLotEntity = parkingLotDto.ToEntity();
      var s = await parkingLotDbContext.ParkingLots.AddAsync(parkingLotEntity);
      await parkingLotDbContext.SaveChangesAsync();

      return Created($"/parking-lots", parkingLotEntity.Id);
    }

    [HttpGet]
    public string Get()
    {
      return "Hello World";
    }
  }
}