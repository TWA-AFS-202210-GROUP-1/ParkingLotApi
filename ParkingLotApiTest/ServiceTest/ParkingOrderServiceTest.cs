using System.Threading.Tasks;
using ParkingLotApi;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.Extensions.DependencyInjection;
    using Newtonsoft.Json;
    using ParkingLotApi.Repository;
    using ParkingLotApi.Service;
    using ParkingLotApiTest.Dtos;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;

    public class ParkingOrderServiceTest : TestBase
    {
        public ParkingOrderServiceTest(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_parkingOrder_success_via_parkingOrder_service()
        {
            // given
            var context = GetParkingLotDbContext();
            IParkingLotService parkingLotService = new ParkingLotService(context);
            await parkingLotService.AddParkingLot(TestData.ParkingLotDtos[0]);
            IParkingOrderService parkingOrderService = new ParkingOrderService(context);
            // when
            await parkingOrderService.AddParkingOrder(TestData.ParkingOrderDtos[0]);

            // then
            Assert.Equal(1, context.ParkingOrders.Count());
        }

        [Fact]
        public async Task Should_throw_exception_when_create_parkingOrder_success_via_parkingOrder_service_given_lot_isfull()
        {
            // given
            var context = GetParkingLotDbContext();
            IParkingLotService parkingLotService = new ParkingLotService(context);
            await parkingLotService.AddParkingLot(TestData.ParkingLotDtos[3]);
            IParkingOrderService parkingOrderService = new ParkingOrderService(context);
            // when
            await parkingOrderService.AddParkingOrder(TestData.ParkingOrderDtos[1]);
            var err = async() => await parkingOrderService.AddParkingOrder(TestData.ParkingOrderDtos[2]);
            var res = await Assert.ThrowsAsync<Exception>(err);

            // then
            Assert.Equal("The parking lot is full", res.Message);
        }


        [Fact]
        public async Task Should_update_a_parkingOrder_Status_by_Id_success_via_parkingOrder_service()
        {
            // given
            var context = GetParkingLotDbContext();
            IParkingLotService parkingLotService = new ParkingLotService(context);
            var id = await parkingLotService.AddParkingLot(TestData.ParkingLotDtos[0]);
            IParkingOrderService parkingOrderService = new ParkingOrderService(context);
            await parkingOrderService.AddParkingOrder(TestData.ParkingOrderDtos[0]);
            TestData.ParkingOrderDtos[0].OrderStatus = false;
            TestData.ParkingOrderDtos[0].CloseTime = DateTime.Now;
            //when
            var targetParkingOrder = await parkingOrderService.UpdateParkingOrderStatus(id, TestData.ParkingOrderDtos[0]);
            //then
            Assert.Equal(false, targetParkingOrder.OrderStatus);
        }

        private ParkingLotContext GetParkingLotDbContext()
        {
            var scope = Factory.Services.CreateScope();
            var scopedService = scope.ServiceProvider;
            ParkingLotContext context = scopedService.GetRequiredService<ParkingLotContext>();
            return context;

        }
    }
}