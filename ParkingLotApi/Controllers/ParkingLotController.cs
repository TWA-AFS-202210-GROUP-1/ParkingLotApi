using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using ParkingLotApi.Model;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("parkinglots")]
    public class ParkingLotController : ControllerBase
    {
        private readonly ParkingLotService parkinglotService;

        public ParkingLotController(ParkingLotService parkinglotService)
        {
            this.parkinglotService = parkinglotService;
        }

        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParkingLotDto>>> List()
        {
            var parkinglotDtos = await this.parkinglotService.GetAll();
            var resultparkinglotDtos = new List<ParkingLotDto>();
            foreach (var parkinglot in parkinglotDtos)
            {
                resultparkinglotDtos.Add(new ParkingLotDto(parkinglot.Name, parkinglot.Capacity));
            }

            return Ok(parkinglotDtos);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetById(int id, int? size, string? details)
        {
            if (size != null)
            {
                var resultParkinglots = new List<ParkingLotDto>();
                var parkinglotDtos = await this.parkinglotService.GetAll();
                if (id < parkinglotDtos.Count)
                {
                    for (int i = id; i < Math.Min(parkinglotDtos.Count, id + 15); i++)
                    {
                        resultParkinglots.Add(new ParkingLotDto(parkinglotDtos[i - 1].Name, parkinglotDtos[i - 1].Capacity));
                    }

                    return Ok(resultParkinglots);
                }

                return NoContent();
            }

            if (details != null)
            {
                var detailedparkinglotDto = await this.parkinglotService.GetById(id);
                return Ok(detailedparkinglotDto);
            }

            var parkinglotDto = await this.parkinglotService.GetById(id);
            parkinglotDto = new ParkingLotDto(parkinglotDto.Name, parkinglotDto.Capacity);
            return Ok(parkinglotDto);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ParkingLotDto>> GetById([FromRoute] int id, ParkingLotDto parkinglot)
        {
            var parkinglotDto = await this.parkinglotService.GetById(id);
            if (parkinglot.Capacity > parkinglotDto.Capacity)
            {
                parkinglotDto.Capacity = parkinglot.Capacity;
            }

            return Ok(parkinglotDto);
        }

        [HttpPost]
        public async Task<ActionResult<ParkingLotDto>> Add(ParkingLotDto parkinglotDto)
        {
            var id = await this.parkinglotService.AddParkinglot(parkinglotDto);

            return CreatedAtAction(nameof(GetById), new { id = id }, parkinglotDto);
        }

        [HttpPost("{id}/orders")]
        public async Task<ActionResult<OrderDto>> AddNewOrdertoCompany([FromRoute] int id, OrderDto orderDto)
        {
            var parkinglotDto = await this.parkinglotService.GetById(id);

            if (parkinglotDto.OrderDtos == null)
            {
                parkinglotDto.OrderDtos = new List<OrderDto>();
                var updatedorderDto = await this.parkinglotService.AddOrderById(id, orderDto);
                return updatedorderDto;
            }

            if (parkinglotDto.OrderDtos.Count < parkinglotDto.Capacity)
            {
                parkinglotDto.OrderDtos.Add(orderDto);
                var updatedorderDto = await this.parkinglotService.AddOrderById(id, orderDto);
                return updatedorderDto;
            }

            return NotFound();
        }

        [HttpGet("{id}/orders/{ordernumber}")]
        public async Task<ActionResult<ParkingLotDto>> GetOrder([FromRoute] int id, [FromRoute] int ordernumber)
        {
            var parkinglotDto = await this.parkinglotService.GetById(id);
            if (parkinglotDto.OrderDtos != null)
            {
                var foundOrder = parkinglotDto.OrderDtos.FirstOrDefault(order => order.Ordernumber == ordernumber);
                return Ok(foundOrder);
            }

            return NotFound();
        }

        [HttpPut("{id}/orders/{orderid}")]
        public async Task<ActionResult<OrderDto>> ChangeOrder([FromRoute] int id, [FromRoute] int orderid, OrderDto orderdto)
        {
            var parkinglotDto = await this.parkinglotService.GetById(id);
            if (parkinglotDto.OrderDtos != null)
            {
                var updatedorderDto = await this.parkinglotService.UpdateById(id, orderid);
                return updatedorderDto;
            }

            return NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await parkinglotService.DeleteParkingLot(id);

            return this.NoContent();
        }
    }
}
