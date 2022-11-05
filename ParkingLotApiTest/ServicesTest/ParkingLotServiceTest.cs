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
    public class ParkingLotServiceTest : TestBase
    {
        public ParkingLotServiceTest(WebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        private ParkingLotContext GetParkingLotContext()
        {
            var scope = Factory.Services.CreateScope();
            var scopedService = scope.ServiceProvider;
            var context = scopedService.GetRequiredService<ParkingLotContext>();
            return context;
        }

        private ParkingLotDto ParkingLotDto()
        {
            var parkingLotDto = new ParkingLotDto
            {
                Name = "ParkingLot1",
                Capacity = 10,
                Location = "Beijing",
            };
            return parkingLotDto;
        }

        [Fact]
        public async Task Should_create_company_success_via_company_service()
        {
            // given
            var context = GetParkingLotContext();
            var parkingLotService= new ParkingLotService(context);

            // when
            await parkingLotService.AddParkingLot(ParkingLotDto());

            // then
            Assert.Equal(1, context.ParkingLots.Count());
        }
    }
}