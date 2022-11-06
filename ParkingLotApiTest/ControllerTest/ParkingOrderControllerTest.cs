using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApiTest;
using Xunit.Sdk;

namespace ParkingLotApi.ControllerTest
{
    using System.Linq;
    using System.Net;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    [Collection("SameCollection")]
    public class ParkingOrderControllerTest : ControllerTestBase
    {
        public ParkingOrderControllerTest(WebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_parking_order_success()
        {
            // given
            var client = this.GetClient();
            var parkingLot = new ParkingLotDto("Parking Lot NO.1", 10, "Beijing");
            await this.PostAsyncParkingLotDto(client, parkingLot);

            var parkingOrderDto = new ParkingOrderDto(parkingLot.Name, "A1234", DateTime.Now, DateTime.Now);
            var parkingOrderContent = await this.ConvertDtoToStringContent(parkingOrderDto);

            // when
            await client.PostAsync("/orders", parkingOrderContent);

            // then
            var allParkingOrdersResponse = await client.GetAsync("/orders");
            var returnParkingOrder = await DeserializeHttpResponse<List<ParkingOrderDto>>(allParkingOrdersResponse);
            Assert.Single(returnParkingOrder);
            Assert.Equal(parkingLot.Name, returnParkingOrder[0].ParkingLotName);
            Assert.Equal("A1234", returnParkingOrder[0].PlateNumber);
        }
    }
}