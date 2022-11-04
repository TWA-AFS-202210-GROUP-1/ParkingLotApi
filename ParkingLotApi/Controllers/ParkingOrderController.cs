﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ParkingLotApi.Dtos;
using ParkingLotApi.Services;

namespace ParkingLotApi.Controllers
{
    [ApiController]
    [Route("api/ParkingOrders")]
    public class ParkingOrderController : ControllerBase
    {
        private readonly IParkingOrderService _service;
        public ParkingOrderController(IParkingOrderService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateParkingOrderDto createParkingOrderDto)
        {
            var createdDto = await _service.CreateParkingOrder(createParkingOrderDto);
            return Created($"api/ParkingOrder/{createdDto.OrderNumber}", createdDto);
        }
    }
}