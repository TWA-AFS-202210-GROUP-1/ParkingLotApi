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
    public async Task<ActionResult<List<ParkingLotDto>>> GetAllParkingLots([FromQuery] int? pageIndex)
    {
        if (pageIndex == null)
        {
            var parkingLotsDtos = await this.parkingLotService.GetAll();

            return Ok(parkingLotsDtos);
        }
        else
        {
            var parkingLotsDtos = await this.parkingLotService.GetByPageIndex(pageIndex);
            return Ok(parkingLotsDtos);
        }

    }

    [HttpPost]
    public async Task<ActionResult<int>> AddParkingLot(ParkingLotDto parkingLotDto)
    {
        var id = await this.parkingLotService.AddParkingLot(parkingLotDto);

        return Created($"/parkinglots/{id}", id);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteParkingLotById(int id)
    {
        await this.parkingLotService.deleteParkingLot(id);

        return this.NoContent();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ParkingLotDto>> GetById(int id)
    {
        var companyDto = await this.parkingLotService.GetById(id);
        return Ok(companyDto);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ParkingLotDto>> updateParkingLotByIDAsync([FromRoute] int id, [FromBody] ParkingLotDto parkingLotDto)
    {
        var UpdatedParkingLot = await this.parkingLotService.UpdateParkingLotCapacity(id, parkingLotDto);
        return Ok(UpdatedParkingLot);
    }
}