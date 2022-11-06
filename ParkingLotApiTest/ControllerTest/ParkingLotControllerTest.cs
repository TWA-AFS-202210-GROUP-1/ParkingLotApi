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
    using ParkingLotApi.Models;
    using ParkingLotApi.Repository;
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
            var createParkingLotResponse = await PrepareNewData(client);
            var createParkingLotResponseBody = await createParkingLotResponse.Content.ReadAsStringAsync();
            //string parkingLotName = "NO.new";
            //int parkingLotCapacity = 50;
            //string parkingLotLocation = "somewhere new";
            //ParkingLotDto parkingLotDto = new ParkingLotDto()
            //{
            //    ParkingLotName = parkingLotName,
            //    ParkingLotCapacity = parkingLotCapacity,
            //    ParkingLotLocation = parkingLotLocation,
            //};
            //var parkingLotHttpContent = JsonConvert.SerializeObject(parkingLotDto);
            //StringContent parkingLotContent = new StringContent(parkingLotHttpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            //// when
            //var createParkingLotResponse = await client.PostAsync("/parkingLots", parkingLotContent);
            var getParkingLotByIdResponse = await client.GetAsync(createParkingLotResponse.Headers.Location);
            var responseBody = await getParkingLotByIdResponse.Content.ReadAsStringAsync();

            // then
            Assert.Equal(HttpStatusCode.Created, createParkingLotResponse.StatusCode);
            Assert.Equal(createParkingLotResponseBody, responseBody);

        }

        [Fact]
        public async Task Should_delete_a_parking_lot_when_call_delete_given_parking_lot_name()
        {
            // given
            var client = GetClient();

            var createParkingLotResponse = await PrepareNewData(client);
            //// prepare data
            //string parkingLotName = "NO.new";
            //int parkingLotCapacity = 50;
            //string parkingLotLocation = "somewhere new";
            //ParkingLotDto parkingLotDto = new ParkingLotDto()
            //{
            //    ParkingLotName = parkingLotName,
            //    ParkingLotCapacity = parkingLotCapacity,
            //    ParkingLotLocation = parkingLotLocation,
            //};
            //var parkingLotHttpContent = JsonConvert.SerializeObject(parkingLotDto);
            //StringContent parkingLotContent = new StringContent(parkingLotHttpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            //var createParkingLotResponse = await client.PostAsync("/parkingLots", parkingLotContent);

            // when
            var deleteParkingLotResponse = await client.DeleteAsync(createParkingLotResponse.Headers.Location);
            var getParkingLotByIdResponse = await client.GetAsync(createParkingLotResponse.Headers.Location);

            // then
            Assert.Equal(HttpStatusCode.NoContent, deleteParkingLotResponse.StatusCode);
            Assert.Equal(HttpStatusCode.InternalServerError, getParkingLotByIdResponse.StatusCode);
        }

        [Fact]
        public async Task Should_return_parking_lot_List_when_call_getAll_given_parking_lot_pageIndex()
        {
            // given
            var client = GetClient();
            await PrepareMultiData(client);
            int pageIndex = 2;
            int maxPageSize = 15;

            // when
            var getParkingLotsByPageIndexResponse = await client.GetAsync($"parkingLots?pageIndex={pageIndex}");
            var responseBody = await getParkingLotsByPageIndexResponse.Content.ReadAsStringAsync();
            var returnParkingLotDtoList = JsonConvert.DeserializeObject<List<ParkingLotDto>>(responseBody);

            // then
            Assert.Equal(HttpStatusCode.OK, getParkingLotsByPageIndexResponse.StatusCode);
            Assert.Equal(maxPageSize, returnParkingLotDtoList.Count);
        }

        [Fact]
        public async Task Should_return_one_parking_lot_loction_when_call_get_given_parking_lot_id()
        {
            // given
            var client = GetClient();
            var createParkingLotResponse = await PrepareNewData(client);

            // when
            var getParkingLotsByIdResponse = await client.GetAsync(createParkingLotResponse.Headers.Location);
            var responseBody = await getParkingLotsByIdResponse.Content.ReadAsStringAsync();
            var returnParkingLotDto = JsonConvert.DeserializeObject<ParkingLotDto>(responseBody);

            // then
            Assert.Equal(HttpStatusCode.OK, getParkingLotsByIdResponse.StatusCode);
            Assert.Equal("Somewhere New", returnParkingLotDto.ParkingLotLocation);
        }

        [Fact]
        public async Task Should_update_one_parking_lot_capacity_when_call_update_given_parking_lot_new_capacity()
        {
            // given
            var client = GetClient();
            var createParkingLotResponse = await PrepareNewData(client);
            string parkingLotName = "NO.New";
            int parkingLotCapacity = 100;
            string parkingLotLocation = "Somewhere New";
            ParkingLotDto parkingLotDto = new ParkingLotDto()
            {
                ParkingLotName = parkingLotName,
                ParkingLotCapacity = parkingLotCapacity,
                ParkingLotLocation = parkingLotLocation,
            };
            var parkingLotHttpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent parkingLotContent = new StringContent(parkingLotHttpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            // when
            var getParkingLotsByIdResponse = await client.PatchAsync(createParkingLotResponse.Headers.Location, parkingLotContent);
            var responseBody = await getParkingLotsByIdResponse.Content.ReadAsStringAsync();
            var returnParkingLotDto = JsonConvert.DeserializeObject<ParkingLotDto>(responseBody);

            // then
            Assert.Equal(HttpStatusCode.OK, getParkingLotsByIdResponse.StatusCode);
            Assert.Equal(100, returnParkingLotDto.ParkingLotCapacity);
        }

        [Fact]
        public async Task Should_create_one_order_capacity_when_car_in_given_car_plate_and_time()
        {
            // given
            var client = GetClient();
            var createParkingLotResponse = await PrepareNewData(client);
            Car newCar = new Car(carPlateNumber: "666");

            // when
            HttpResponseMessage createOrderResponse = await newCar.Parking(client, createParkingLotResponse);
            var createOrderResponseBody = await createOrderResponse.Content.ReadAsStringAsync();
            // then
            var getParkingLotByIdResponse = await client.GetAsync(createParkingLotResponse.Headers.Location);
            var getParkingLotByIdResponseBody = await getParkingLotByIdResponse.Content.ReadAsStringAsync();
            Assert.Equal(HttpStatusCode.OK, createOrderResponse.StatusCode);
            Assert.Equal(createOrderResponseBody, getParkingLotByIdResponseBody);

        }


        static async Task PrepareMultiData(HttpClient client)
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

        static async Task<HttpResponseMessage> PrepareNewData(HttpClient client)
        {
            string parkingLotName = "NO.New";
            int parkingLotCapacity = 50;
            string parkingLotLocation = "Somewhere New";
            ParkingLotDto parkingLotDto = new ParkingLotDto()
            {
                ParkingLotName = parkingLotName,
                ParkingLotCapacity = parkingLotCapacity,
                ParkingLotLocation = parkingLotLocation,
            };
            var parkingLotHttpContent = JsonConvert.SerializeObject(parkingLotDto);
            StringContent parkingLotContent = new StringContent(parkingLotHttpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            return await client.PostAsync("/parkingLots", parkingLotContent);
        }
    }
}
