using System.Threading.Tasks;
using ParkingLotApi;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;
    using ParkingLotApiTest.Dtos;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;

    public class ParkingLotControllerTest : TestBase
    {
        public ParkingLotControllerTest(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_add_parking_lot_to_system_successfully()
        {
            // given
            var client = GetClient();
            ParkingLotDto companyDto = new ParkingLotDto
            {
                Name = "park1",
                Capacity = 10,
                Location = "Chaoyang",
            };

            // when
            StringContent postBody = SerializeContent(companyDto);
            await client.PostAsync("/parkingLots", postBody);

            // then
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");
            var returnParkingLots = await DeserializeContent<List<ParkingLotDto>>(allParkingLotsResponse);

            Assert.Equal(1, returnParkingLots.Count);
        }

        [Fact]
        public async Task Should_delete_parking_lot_from_system_successfully()
        {
            // given
            var client = GetClient();
            ParkingLotDto companyDto = new ParkingLotDto
            {
                Name = "park1",
                Capacity = 10,
                Location = "Chaoyang",
            };
            StringContent postBody = SerializeContent(companyDto);
            var response  = await client.PostAsync("/parkingLots", postBody);

            // when
            await client.DeleteAsync(response.Headers.Location);

            // then
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");
            var returnParkingLots = await DeserializeContent<List<ParkingLotDto>>(allParkingLotsResponse);

            Assert.Equal(0, returnParkingLots.Count);
        }

        private static async Task<T> DeserializeContent<T>(HttpResponseMessage response)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            var res = JsonConvert.DeserializeObject<T>(responseBody);
            return res;
        }

        private static StringContent SerializeContent<T>(T target)
        {
            var targetJson = JsonConvert.SerializeObject(target);
            var postBody = new StringContent(targetJson, Encoding.UTF8, mediaType: "application/json");
            return postBody;
        }
    }
}