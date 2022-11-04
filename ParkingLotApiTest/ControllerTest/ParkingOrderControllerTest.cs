using ParkingLotApi.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using ParkingLotApi.Consts;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("ControllerTest")]
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

        [Fact]
        public async Task Should_return_error_message_when_post_new_parking_order_given_a_full_parking_lot()
        {
            // given
            var parkingLot = new CreateOrUpdateParkingLotDto("Best ParkingLot", 1, "11th Street");
            var createdParkingLotResponse = await _httpClient.PostAsJsonAsync("/api/ParkingLots", parkingLot);
            var createdParkingLot = await GetObjectFromHttpResponse<ParkingLotDto>(createdParkingLotResponse);
            var parkingOrder = new CreateParkingOrderDto(createdParkingLot.Id, "MyPlate");
            await _httpClient.PostAsJsonAsync("/api/ParkingOrders", parkingOrder);

            // when
            var createdParkingOrderResponse = await _httpClient.PostAsJsonAsync("/api/ParkingOrders", parkingOrder);
            var createdParkingOrder = await createdParkingOrderResponse.Content.ReadAsStringAsync();

            // then
            Assert.Equal("The parking lot is full.", createdParkingOrder);
        }

        [Fact]
        public async Task Should_return_updated_order_when_update_parking_order_status()
        {
            // given
            var parkingLot = new CreateOrUpdateParkingLotDto("Best ParkingLot", 1, "11th Street");
            var createdParkingLotResponse = await _httpClient.PostAsJsonAsync("/api/ParkingLots", parkingLot);
            var createdParkingLot = await GetObjectFromHttpResponse<ParkingLotDto>(createdParkingLotResponse);
            var parkingOrder = new CreateParkingOrderDto(createdParkingLot.Id, "MyPlate");
            var createdParkingOrderResponse = await _httpClient.PostAsJsonAsync("/api/ParkingOrders", parkingOrder);
            var createdParkingOrder = await GetObjectFromHttpResponse<ParkingOrderDto>(createdParkingOrderResponse);

            var updateOrderDto = new UpdateParkingOrderDto(OrderStatus.Closed);

            // when
            var updatedParkingOrderResponse = await _httpClient.PutAsJsonAsync($"/api/ParkingOrders/{createdParkingOrder.OrderNumber}", updateOrderDto);
            var updatedParkingOrder = await GetObjectFromHttpResponse<ParkingOrderDto>(updatedParkingOrderResponse);

            // then
            Assert.Equal(OrderStatus.Closed, updatedParkingOrder.OrderStatus);
        }
    }
}
