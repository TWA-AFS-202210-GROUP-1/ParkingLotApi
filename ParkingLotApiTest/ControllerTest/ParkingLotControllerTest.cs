using System.Collections.Generic;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
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
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;

    [Collection("SameCollection")]
    public class ParkingLotControllerTest : TestBase
    {
        public ParkingLotControllerTest(WebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        private List<ParkingLotDto> ParkingLotDtos()
        {
            var parkingLotDtos = new List<ParkingLotDto>()
            {
                new ParkingLotDto()
                {
                    Name = "ParkingLot1",
                    Capacity = 10,
                    Location = "Beijing",
                },
                new ParkingLotDto()
                {
                    Name = "ParkingLot2",
                    Capacity = 20,
                    Location = "Shanghai",
                },
            };
            return parkingLotDtos;
        }

        [Fact]
        public async Task Should_create_parking_lot_success()
        {
            // given
            var client = GetClient();

            // when
            var httpContent = JsonConvert.SerializeObject(ParkingLotDtos()[0]);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkingLots", content);

            // then
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");
            var body = await allParkingLotsResponse.Content.ReadAsStringAsync();
            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);
            Assert.Single(returnParkingLots);
        }

        [Fact]
        public async Task Should_return_all_parking_lots_success()
        {
            // given
            var client = GetClient();

            // when
            var httpContent = JsonConvert.SerializeObject(ParkingLotDtos()[0]);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkingLots", content);

            var httpContent1 = JsonConvert.SerializeObject(ParkingLotDtos()[1]);
            var content1 = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkingLots", content1);

            // then
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");
            var body = await allParkingLotsResponse.Content.ReadAsStringAsync();
            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);
            Assert.Equal(2, returnParkingLots.Count);
        }

        [Fact]
        public async Task Should_remove_parking_lot_success()
        {
            // given
            var client = GetClient();

            // when
            var httpContent = JsonConvert.SerializeObject(ParkingLotDtos()[0]);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parkingLots", content);
            await client.DeleteAsync(response.Headers.Location);

            // then
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");
            var body = await allParkingLotsResponse.Content.ReadAsStringAsync();
            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);
            Assert.Empty(returnParkingLots);
        }
    }
}