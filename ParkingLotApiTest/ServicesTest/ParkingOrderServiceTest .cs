using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi.Const;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using ParkingLotApiTest;

namespace ParkingLotApi.ControllerTest
{
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;

    [Collection("SameCollection")]
    public class ParkingOrderServiceTest : ServiceTestBase
    {
        public ParkingOrderServiceTest(WebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_parking_order_success_via_parking_order_service()
        {
            // given
            var context = this.GetParkingLotContext();
            var parkingLotService = new ParkingLotService(context);
            await parkingLotService.AddParkingLot(this.ParkingLotDtos()[0]);

            var parkingOrderService = new ParkingOrderService(context);

            // when
            await parkingOrderService.AddParkingOrder(this.ParkingOrderDtos()[0]);

            // then
            Assert.Equal(1, context.ParkingOrders.Count());
        }

        [Fact]
        public async Task Should_get_all_parking_orders_success_via_parking_order_service()
        {
            // given
            var context = this.GetParkingLotContext();
            var parkingLotService = new ParkingLotService(context);
            await parkingLotService.AddParkingLot(this.ParkingLotDtos()[0]);
            var parkingOrderService = new ParkingOrderService(context);
            foreach (var parkingOrderDto in this.ParkingOrderDtos())
            {
                await parkingOrderService.AddParkingOrder(parkingOrderDto);
            }

            // when
            var parkingOrderDtos = await parkingOrderService.GetAllParkingOrder();

            // then
            Assert.Equal(this.ParkingOrderDtos().Count, parkingOrderDtos.Count());
        }

        [Fact]
        public async Task Should_get_parking_order_by_id_via_parking_lot_service()
        {
            // given
            var context = this.GetParkingLotContext();
            var parkingLotService = new ParkingLotService(context);
            await parkingLotService.AddParkingLot(this.ParkingLotDtos()[0]);
            var parkingOrderService = new ParkingOrderService(context);
            var id = await parkingOrderService.AddParkingOrder(this.ParkingOrderDtos()[0]);
            await parkingOrderService.AddParkingOrder(this.ParkingOrderDtos()[1]);

            // when
            var parkingOrderDto = await parkingOrderService.GetById(id);

            // then
            Assert.Equal(this.ParkingOrderDtos()[0].PlateNumber, parkingOrderDto.PlateNumber);
        }

        [Fact]
        public async Task Should_update_parking_lot_capacity_via_parking_lot_service()
        {
            // given
            var context = GetParkingLotContext();
            var parkingLotService = new ParkingLotService(context);
            await parkingLotService.AddParkingLot(this.ParkingLotDtos()[0]);
            var parkingOrderService = new ParkingOrderService(context);
            var id = await parkingOrderService.AddParkingOrder(this.ParkingOrderDtos()[0]);
            var parkingOrderDto = this.ParkingOrderDtos()[0];
            parkingOrderDto.OrderStatus = OrderStatus.Close;

            // when
            var modifiedParkingLotDto = await parkingOrderService.UpdateParkingLot(id, parkingOrderDto);

            // then
            Assert.Equal(OrderStatus.Close, modifiedParkingLotDto.OrderStatus);
        }

        [Fact]
        public async Task Should_throw_exception_when_parking_lot_is_full_given_extra_order()
        {
            // given
            var context = GetParkingLotContext();
            var parkingLotService = new ParkingLotService(context);

            var parkingLotDto = new ParkingLotDto("PL1", 1, "Beijing");
            await parkingLotService.AddParkingLot(parkingLotDto);

            var parkingOrderService = new ParkingOrderService(context);
            await parkingOrderService.AddParkingOrder(this.ParkingOrderDtos()[0]);

            // when // then
            var exception = await Assert.ThrowsAsync<Exception>(async ()
                => await parkingOrderService.AddParkingOrder(this.ParkingOrderDtos()[1]));

            Assert.Equal(OrderStatus.FailMessage, exception.Message);
        }
    }
}