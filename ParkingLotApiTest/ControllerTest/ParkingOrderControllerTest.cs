using ParkingLotApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ParkingLotApi.Consts;

namespace ParkingLotApiTest.ControllerTest
{
    public class ParkingOrderControllerTest : ControllerTestBase
    {
        [Fact]
        public async Task Should_return_created_parking_order_when_post_new_parking_order()
        {
            // given
            var parkingLot = new CreateOrUpdateParkingLotDto("Best ParkingLot", 100, "11th Street");
            var createdParkingLotResponse = await _httpClient.PostAsJsonAsync("/api/ParkingLots", parkingLot);
            var createdParkingLot = await GetObjectFromHttpResponse<ParkingLotDto>(createdParkingLotResponse);
            var parkingOrder = new CreateParkingOrderDto(createdParkingLot.Id, "MyPlate");

            // when
            var createdParkingOrderResponse = await _httpClient.PostAsJsonAsync("/api/ParkingOrders", parkingOrder);
            var createdParkingOrder = await GetObjectFromHttpResponse<ParkingOrderDto>(createdParkingOrderResponse);

            // then
            Assert.Equal("MyPlate", createdParkingOrder.PlateNumber);
            Assert.Equal("Best ParkingLot", createdParkingOrder.ParkingLotName);
            Assert.Equal(OrderStatus.Open, createdParkingOrder.OrderStatus);
        }
    }
}
