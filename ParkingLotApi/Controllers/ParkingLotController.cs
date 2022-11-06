namespace ParkingLotApi.Controllers;

using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Service;
using ParkingLotApiTest.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

[ApiController]
[Route("parkingLots")]
public class ParkingLotController : ControllerBase
{
    private readonly IParkingLotService parkingLotService;
    public ParkingLotController(IParkingLotService parkingLotService)
    {
        this.parkingLotService = parkingLotService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ParkingLotDto>>> GetAllParkingLots()
    {
        var companyDtos = await this.parkingLotService.GetAll();

        return Ok(companyDtos);
    }

    [HttpPost]
    public async Task<ActionResult<int>> AddParkingLot(ParkingLotDto parkingLotDto)
    {
        var id = await this.parkingLotService.AddParkingLot(parkingLotDto);

        return Created($"/parkinglots/{id}", id);
    }
}