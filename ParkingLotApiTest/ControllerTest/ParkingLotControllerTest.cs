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


        [Fact]
        public async Task Should_return_parking_lots_from_system_successfully_given_index()
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
            await client.PostAsync("/parkingLots", postBody);

            // when
            var ParkingLotsResponseIndex1 = await client.GetAsync("/parkingLots?pageIndex=1");
            var returnParkingLots1 = await DeserializeContent<List<ParkingLotDto>>(ParkingLotsResponseIndex1);
            var ParkingLotsResponseIndex2 = await client.GetAsync("/parkingLots?pageIndex=2");
            var returnParkingLots2= await DeserializeContent<List<ParkingLotDto>>(ParkingLotsResponseIndex2);

            // then
            Assert.Equal(1, returnParkingLots1.Count);
            Assert.Equal(0, returnParkingLots2.Count);
        }

        [Fact]
        public async Task Should_return_parking_lot_from_system_successfully_given_id()
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
            var response = await client.PostAsync("/parkingLots", postBody);

            // when
            var ParkingLotResponse = await client.GetAsync(response.Headers.Location);
            var returnParkingLot = await DeserializeContent<ParkingLotDto>(ParkingLotResponse);

            // then
            Assert.Equal("park1", returnParkingLot.Name);
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