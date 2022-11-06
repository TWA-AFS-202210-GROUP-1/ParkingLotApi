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
            var httpContent = JsonConvert.SerializeObject(companyDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkingLots", content);

            // then
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");
            var body = await allParkingLotsResponse.Content.ReadAsStringAsync();
            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);

            Assert.Single(returnParkingLots);
        }
    }
}