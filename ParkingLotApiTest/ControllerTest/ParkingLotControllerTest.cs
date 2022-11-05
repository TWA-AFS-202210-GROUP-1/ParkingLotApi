using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using ParkingLotApi.Dtos;
using ParkingLotApi.Models;
using ParkingLotApiTest;
using Xunit.Sdk;

namespace ParkingLotApi.ControllerTest
{
    using System.Net;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;
    using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

    [Collection("SameCollection")]
    public class ParkingLotControllerTest : ControllerTestBase
    {
        public ParkingLotControllerTest(WebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_create_parking_lot_success()
        {
            // given
            var client = this.GetClient();
            var content = this.ConvertDtoToStringContent(ParkingLotDtos()[0]).Result;

            // when
            await client.PostAsync("/parkingLots", content);

            // then
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");
            var returnParkingLots = await ConvertResponseToParkingLotDtos(allParkingLotsResponse);
            Assert.Single(returnParkingLots);
        }

        [Fact]
        public async Task Should_return_all_parking_lots_success()
        {
            // given
            var client = this.GetClient();
            await this.PostAsyncParkingLotDtoList(client, this.ParkingLotDtos());

            // when
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");

            // then
            var body = await allParkingLotsResponse.Content.ReadAsStringAsync();
            var returnParkingLots = await ConvertResponseToParkingLotDtos(allParkingLotsResponse);
            Assert.Equal(4, returnParkingLots.Count);
        }

        [Fact]
        public async Task Should_remove_parking_lot_success()
        {
            // given
            var client = this.GetClient();
            var response = await this.PostAsyncParkingLotDto(client, this.ParkingLotDtos()[0]);

            // when
            await client.DeleteAsync(response.Headers.Location);

            // then
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");
            var returnParkingLots = await ConvertResponseToParkingLotDtos(allParkingLotsResponse);
            Assert.Empty(returnParkingLots);
        }
    }
}