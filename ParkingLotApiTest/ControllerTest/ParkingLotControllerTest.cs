using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotApiTest.ControllerTest
{
    using DeepEqual.Syntax;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;
    using ParkingLotApi.Dtos;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mime;
    using Xunit;

    public class ParkingLotControllerTest : TestBase
    {
        public ParkingLotControllerTest(ParkingLotWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Should__create_a_parking_lot_when_call_post_given_parking_lot_infromation()
        {
            // given
            var client = GetClient();
            PrepareData(client);

            string parkingLotName = "NO.new";
            int parkingLotCapacity = 50;
            string parkingLotLocation = "somewhere new";
            ParkingLotDto parkingLotDto = new ParkingLotDto()
            {
                ParkingLotName = parkingLotName,
                ParkingLotCapacity = parkingLotCapacity,
                ParkingLotLocation = parkingLotLocation,
            };
            var parkingLotHttpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent parkingLotContent = new StringContent(parkingLotHttpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            // when
            var createParkingLotResponse = await client.PostAsync("/parkingLots", parkingLotContent);
            var allParkingLotsResponse = await client.GetAsync("/parkingLots");
            var responseBody = await allParkingLotsResponse.Content.ReadAsStringAsync();
            var returnParkingLotDtoList = JsonConvert.DeserializeObject<List<ParkingLotDto>>(responseBody);

            // then
            Assert.Equal(HttpStatusCode.Created, createParkingLotResponse.StatusCode);
            returnParkingLotDtoList.Find(x => x.ParkingLotName.Equals(parkingLotName)).ShouldDeepEqual(parkingLotDto);
        }

        [Fact]
        public async Task Should_delete_a_parking_lot_when_call_delete_given_parking_lot_name()
        {
            // given
            var client = GetClient();
            //PrepareData(client);

            // prepare data
            string parkingLotName = "NO.new";
            int parkingLotCapacity = 50;
            string parkingLotLocation = "somewhere new";
            ParkingLotDto parkingLotDto = new ParkingLotDto()
            {
                ParkingLotName = parkingLotName,
                ParkingLotCapacity = parkingLotCapacity,
                ParkingLotLocation = parkingLotLocation,
            };
            var parkingLotHttpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent parkingLotContent = new StringContent(parkingLotHttpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var createParkingLotResponse = await client.PostAsync("/parkingLots", parkingLotContent);

            // when
            var deleteParkingLotResponse = await client.DeleteAsync(createParkingLotResponse.Headers.Location);
            var getParkingLotByIdResponse = await client.GetAsync(createParkingLotResponse.Headers.Location);

            // then
            Assert.Equal(HttpStatusCode.NoContent, deleteParkingLotResponse.StatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, getParkingLotByIdResponse.StatusCode);
        }

        static async void PrepareData(HttpClient client)
        {
            int dataNumberToprepare = 50;
            for (int i = 0; i < dataNumberToprepare; i++)
            {
                ParkingLotDto dataToPost = new ParkingLotDto()
                {
                    ParkingLotName = $"NO.{i}",
                    ParkingLotCapacity = 50,
                    ParkingLotLocation = $"somewhere {i}",
                };
                var parkingLotHttpContent = JsonConvert.SerializeObject(dataToPost);
                StringContent parkingLotContent = new StringContent(parkingLotHttpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
                await client.PostAsync("/parkingLots", parkingLotContent);
            }
        }
    }
}
