using ParkingLotApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ParkingLotApi.Consts;
using ParkingLotApi.Models;
using ParkingLotApi.Services;

namespace ParkingLotApiTest.ServiceTest
{
    [Collection("ServiceTest")]
    public class ParkingOrderServiceTest : ServiceTestBase
    {
        private readonly ParkingOrderService _parkingOrderService;
        public ParkingOrderServiceTest()
        {
            _parkingOrderService = new ParkingOrderService(ParkingLotContext);
        }

        [Fact]
        public async void Should_create_parking_order_when_give_a_valid_dto()
        {
            // given
            var parkingLotEntity = new ParkingLotEntity()
            {
                Capacity = 10,
                Name = "Random",
                Location = "Random",
                Orders = new List<ParkingOrderEntity>()
            };
            await ParkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
            await ParkingLotContext.SaveChangesAsync();

            var createParkingOrderDto = new CreateParkingOrderDto(parkingLotEntity.Id, "TestPlate");

            // when
            var createdParkingLotDto = await _parkingOrderService.CreateParkingOrder(createParkingOrderDto);
            var parkingOrderEntities = ParkingLotContext.ParkingOrders.ToList();

            // then

            Assert.Single(parkingOrderEntities);
            Assert.Equal("TestPlate", parkingOrderEntities[0].PlateNumber);
            Assert.Equal(OrderStatus.Open, parkingOrderEntities[0].Status);
        }
    }
}
