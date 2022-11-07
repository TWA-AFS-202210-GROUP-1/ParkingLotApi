using System.Threading.Tasks;
using ParkingLotApi;
using Xunit;

namespace ParkingLotApiTest.ControllerTest
{
    using Microsoft.AspNetCore.Mvc.Testing;
    using Newtonsoft.Json;
    using ParkingLotApiTest.Dtos;
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Net.Mime;
    using System.Text;

    public class ParkingOrderControllerTest : TestBase
    {
        public ParkingOrderControllerTest(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task Should_add_parking_order_to_system_successfully()
        {
            // given
            var client = GetClient();
            await AddAparkingLotToDb(client, TestData.ParkingLotDtos[0]);
            //when
            var response = await AddAparkingOrderToDb(client, TestData.ParkingOrderDtos[0]);

            // then

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task Should_throw_exception_when_add_parking_order_to_system_give_lot_is_full()
        {
            // given
            var client = GetClient();
            await AddAparkingLotToDb(client, TestData.ParkingLotDtos[3]);
            await AddAparkingOrderToDb(client, TestData.ParkingOrderDtos[1]);
            //when
            var response = await AddAparkingOrderToDb(client, TestData.ParkingOrderDtos[2]);

            // then

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }


        [Fact]
        public async Task Should_update_parking_Order_status_from_system_successfully_given_id()
        {
           //given
            var client = GetClient();
            await AddAparkingLotToDb(client, TestData.ParkingLotDtos[0]);
            var response = await AddAparkingOrderToDb(client, TestData.ParkingOrderDtos[0]);
            TestData.ParkingOrderDtos[0].CloseTime = DateTime.Now;
            TestData.ParkingOrderDtos[0].OrderStatus = false;

            //when
            var ParkingOrderResponse = await client.PutAsync(response.Headers.Location, SerializeContent(TestData.ParkingOrderDtos[0]));
            var returnParkingOrder = await DeserializeContent<ParkingOrderDto>(ParkingOrderResponse);

            // then
            Assert.Equal(false, returnParkingOrder.OrderStatus);
        }

        private static async Task<HttpResponseMessage> AddAparkingLotToDb(HttpClient client, ParkingLotDto parkingLotDto)
        {
            StringContent postBody = SerializeContent(parkingLotDto);
            var response = await client.PostAsync("/parkingLots", postBody);
            return response;
        }
        private static async Task<HttpResponseMessage> AddAparkingOrderToDb(HttpClient client, ParkingOrderDto parkingOrderDto)
        {
            StringContent postBody = SerializeContent(parkingOrderDto);
            var response = await client.PostAsync("/parkingOrders", postBody);
            return response;
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