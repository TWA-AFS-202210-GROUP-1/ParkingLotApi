namespace ParkingLotApiTest.ControllerTest
{
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;
    using ParkingLotApi.Dtos;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;
    using System.Threading.Tasks;
    using ParkingLotApi;
    using Xunit;
    using System;

    [Collection("test")]
    public class ParkingLotControllerTest : TestBase
    {
        public ParkingLotControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
        {
        }

        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }

        [Fact]
        public async Task Should_create_company_success()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkinglotDto = new ParkingLotDto
            {
                Name = "IBM",
                Capacity = 10,
                Location = "NYC",
            };

            // when
            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkinglots", content);

            // then
            var allParkinglotsResponse = await client.GetAsync("/parkinglots");
            var body = await allParkinglotsResponse.Content.ReadAsStringAsync();

            var returnParkingLots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);

            Assert.Single(returnParkingLots);
        }

        [Fact]
        public async Task Should_delete_parkinglot_success()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkinglotDto = new ParkingLotDto
            {
                Name = "IBM",
                Capacity = 10,
                Location = "NYC",
            };

            // when
            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parkinglots", content);

            // then
            await client.DeleteAsync(response.Headers.Location);
            var allParkinglotsResponse = await client.GetAsync("/parkinglots");
            var body = await allParkinglotsResponse.Content.ReadAsStringAsync();

            var returnParkinglots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);

            Assert.Empty(returnParkinglots);
        }

        [Fact]
        public async Task Should_get_15_parkinglots_after_the_second_one()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkinglotDto = new ParkingLotDto
            {
                Name = "IBM",
                Capacity = 10,
                Location = "NYC",
            };

            // when
            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkinglots", content);

            parkinglotDto = new ParkingLotDto
            {
                Name = "CBA",
                Capacity = 10,
                Location = "NYC",
            };
            httpContent = JsonConvert.SerializeObject(parkinglotDto);
            content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            for (int i = 0; i < 17; i++)
            {
                await client.PostAsync("/parkinglots", content);
            }

            // then
            var allParkinglotsResponse = await client.GetAsync("/parkinglots/2?&size=15");
            var body = await allParkinglotsResponse.Content.ReadAsStringAsync();
            var returnParkinglots = JsonConvert.DeserializeObject<List<ParkingLotDto>>(body);

            Assert.Equal(15, returnParkinglots.Count);
        }

        [Fact]
        public async Task Should_get_details_of_parkinglot()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkinglotDto = new ParkingLotDto
            {
                Name = "IBM",
                Capacity = 10,
                Location = "NYC",
            };

            // when
            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkinglots", content);

            // then
            var allParkinglotsResponse = await client.GetAsync("/parkinglots/1?&details=1");
            var body = await allParkinglotsResponse.Content.ReadAsStringAsync();

            var returnParkinglot = JsonConvert.DeserializeObject<ParkingLotDto>(body);

            Assert.Equal(10, returnParkinglot.Capacity);
        }

        [Fact]
        public async Task Should_get_updated_capacity_of_parkinglot()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkinglotDto = new ParkingLotDto
            {
                Name = "IBM",
                Capacity = 10,
                Location = "NYC",
            };

            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkinglots", content);

            // when
            parkinglotDto = new ParkingLotDto
            {
                Name = "IBM",
                Capacity = 15,
                Location = "NYC",
            };
            httpContent = JsonConvert.SerializeObject(parkinglotDto);
            content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            // then
            var allParkinglotsResponse = await client.PutAsync("/parkinglots/1", content);
            var body = await allParkinglotsResponse.Content.ReadAsStringAsync();

            var returnParkinglot = JsonConvert.DeserializeObject<ParkingLotDto>(body);

            Assert.Equal(15, returnParkinglot.Capacity);
        }

        [Fact]
        public async Task Should_create_order_success()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkinglotDto = new ParkingLotDto
            {
                Name = "IBM",
                Capacity = 10,
                Location = "NYC",
            };

            // when
            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parkinglots", content);
            var OrderDto = new OrderDto()
            {
                Ordernumber = 1,
                NameofParkinglot = "1",
                PlateNumber = "gb123",
                CreationTime = "20220102",
                CloseTime = "20220301",
                Status = "open",
            };
            httpContent = JsonConvert.SerializeObject(OrderDto);
            content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            // then
            var allParkinglotsResponse = await client.PostAsync("/parkinglots/1/orders", content);
            Assert.Equal("Created", allParkinglotsResponse.StatusCode.ToString());
        }


        [Fact]
        public async Task Should_update_order_when_car_leave()
        {
            // given
            var client = GetClient();
            ParkingLotDto parkinglotDto = new ParkingLotDto
            {
                Name = "IBM",
                Capacity = 10,
                Location = "NYC",
            };

            // when
            var httpContent = JsonConvert.SerializeObject(parkinglotDto);
            StringContent content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            await client.PostAsync("/parkinglots", content);
            var OrderDto = new OrderDto()
            {
                Ordernumber = 1,
                NameofParkinglot = "1",
                PlateNumber = "gb123",
                CreationTime = GetTimestamp(DateTime.Now),
                CloseTime = string.Empty,
                Status = "open",
            };
            httpContent = JsonConvert.SerializeObject(OrderDto);
            content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);
            var response = await client.PostAsync("/parkinglots/1/orders", content);

            var body = await response.Content.ReadAsStringAsync();

            var returnparkinglot = JsonConvert.DeserializeObject<OrderDto>(body);

            response = await client.GetAsync("/parkinglots/1/orders/1");
            body = await response.Content.ReadAsStringAsync();

            var returnOrder = JsonConvert.DeserializeObject<OrderDto>(body);

            OrderDto = new OrderDto()
            {
                Ordernumber = 1,
                NameofParkinglot = "1",
                PlateNumber = "gb123",
                CreationTime = returnOrder.CreationTime,
                CloseTime = GetTimestamp(DateTime.Now),
                Status = "close",
            };
            httpContent = JsonConvert.SerializeObject(OrderDto);
            content = new StringContent(httpContent, Encoding.UTF8, MediaTypeNames.Application.Json);

            // then
            response = await client.PutAsync("/parkinglots/1/orders/1", content);

            body = await response.Content.ReadAsStringAsync();

            returnOrder = JsonConvert.DeserializeObject<OrderDto>(body);
            Assert.Equal("close", returnOrder.Status);
        }
    }
}
