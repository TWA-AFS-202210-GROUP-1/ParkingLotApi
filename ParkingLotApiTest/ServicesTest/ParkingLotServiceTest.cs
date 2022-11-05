using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
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
    public class ParkingLotServiceTest : ServiceTestBase
    {
        public ParkingLotServiceTest(WebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_parking_lot_success_via_parking_lot_service()
        {
            // given
            var context = this.GetParkingLotContext();
            var parkingLotService = new ParkingLotService(context);

            // when
            await parkingLotService.AddParkingLot(this.ParkingLotDtos()[0]);

            // then
            Assert.Equal(1, context.ParkingLots.Count());
        }

        [Fact]
        public async Task Should_get_all_parking_lots_success_via_parking_lot_service()
        {
            // given
            var context = this.GetParkingLotContext();
            var parkingLotService = new ParkingLotService(context);
            foreach (var parkingLotDto in this.ParkingLotDtos())
            {
                await parkingLotService.AddParkingLot(parkingLotDto);
            }

            // when
            var parkingLotDtos = await parkingLotService.GetAllParkingLot();

            // then
            Assert.Equal(this.ParkingLotDtos().Count, parkingLotDtos.Count());
        }

        [Fact]
        public async Task Should_delete_parking_lot_success_via_parking_lot_service()
        {
            // given
            var context = this.GetParkingLotContext();
            var parkingLotService = new ParkingLotService(context);
            var id = await parkingLotService.AddParkingLot(this.ParkingLotDtos()[0]);
            await parkingLotService.AddParkingLot(this.ParkingLotDtos()[1]);

            // when
            await parkingLotService.DeleteParkingLot(id);

            // then
            Assert.Equal(1, context.ParkingLots.Count());
            Assert.Equal(this.ParkingLotDtos()[1].Name, context.ParkingLots.ToList()[0].Name);
        }

        [Fact]
        public async Task Should_get_parking_lots_by_range_via_parking_lot_service()
        {
            // given
            var context = this.GetParkingLotContext();
            var parkingLotService = new ParkingLotService(context);
            foreach (var parkingLotDto in this.ParkingLotDtos())
            {
                await parkingLotService.AddParkingLot(parkingLotDto);
            }

            // when
            var parkingLotByRangePage1 = await parkingLotService.GetParkingLotByRange(pageIndex: 1);
            var parkingLotByRangePage2 = await parkingLotService.GetParkingLotByRange(pageIndex: 2);

            // then
            Assert.Equal(6, parkingLotByRangePage1.Count());
            Assert.Empty(parkingLotByRangePage2);
        }

        [Fact]
        public async Task Should_get_parking_lot_by_id_via_parking_lot_service()
        {
            // given
            var context = this.GetParkingLotContext();
            var parkingLotService = new ParkingLotService(context);
            var id = await parkingLotService.AddParkingLot(this.ParkingLotDtos()[0]);
            await parkingLotService.AddParkingLot(this.ParkingLotDtos()[1]);

            // when
            var parkingLotDto = await parkingLotService.GetById(id);

            // then
            Assert.Equal(this.ParkingLotDtos()[0].Name, parkingLotDto.Name);
        }

        [Fact]
        public async Task Should_update_parking_lot_capacity_via_parking_lot_service()
        {
            // given
            var context = GetParkingLotContext();
            var parkingLotService = new ParkingLotService(context);
            var id = await parkingLotService.AddParkingLot(this.ParkingLotDtos()[0]);
            var parkingLotDto = this.ParkingLotDtos()[0];
            parkingLotDto.Capacity = 100;

            // when
            var modifiedParkingLotDto = await parkingLotService.UpdateParkingLot(id, parkingLotDto);

            // then
            Assert.Equal(100, modifiedParkingLotDto.Capacity);
        }
    }
}