using System.Collections.Generic;
using System.Net.Http;
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

        [Fact]
        public async Task Should_create_parking_lot_success()
        {
            // given
            var client = GetClient();
            var parkingLotDto = new ParkingLotDto
            {
                Name = "ParkingLot1",
                Capacity = 10,
                Location = "Beijing",
            };

            // when
            var httpContent = JsonConvert.SerializeObject(parkingLotDto);
            var content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkingLots", content);

            // then
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");
            var body = await allParkingLotsResponse.Content.ReadAsStringAsync();
            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);
            Assert.Single(returnParkingLots);
        }
    }
}