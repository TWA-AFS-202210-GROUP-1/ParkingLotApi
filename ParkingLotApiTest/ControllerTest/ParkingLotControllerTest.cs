using System.Collections.Generic;
using System.Net;
using ParkingLotApi.Dtos;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ParkingLotApi.Consts;

namespace ParkingLotApiTest.ControllerTest
{
    [Collection("ControllerTest")]
    public class ParkingLotControllerTest : ControllerTestBase
    {
        [Fact]
        public async Task Should_return_created_parking_lot_when_post_new_parking_lot()
        {
            // given
            var parkingLot = new CreateOrUpdateParkingLotDto("Best ParkingLot", 100, "11th Street");
            
            // when
            var createdParkingLotResponse = await _httpClient.PostAsJsonAsync("/api/ParkingLots", parkingLot);
            var createdParkingLot = await GetObjectFromHttpResponse<ParkingLotDto>(createdParkingLotResponse);

            // then
            Assert.Equal(parkingLot.Name, createdParkingLot.Name);
            Assert.Equal(parkingLot.Capacity, parkingLot.Capacity);
            Assert.Equal(parkingLot.Location, parkingLot.Location);
        }

        [Fact]
        public async Task Should_delete_parking_lot_when_delete_parkingLot_given_a_exist_id()
        {
            // given
            var parkingLot = new CreateOrUpdateParkingLotDto("Best ParkingLot", 100, "11th Street");
            var createdParkingLotResponse = await _httpClient.PostAsJsonAsync("/api/ParkingLots", parkingLot);
            var createdParkingLot = await GetObjectFromHttpResponse<ParkingLotDto>(createdParkingLotResponse);

            // when
            var deletedParkingLotResponse = await _httpClient.DeleteAsync($"/api/ParkingLots/{createdParkingLot.Id}");

            // then
            Assert.Equal(HttpStatusCode.NoContent, deletedParkingLotResponse.StatusCode);
        }

        [Fact]
        public async Task Should_return_a_page_of_parking_lots_when_get_a_page_number()
        {
            // given
            var parkingLot = new CreateOrUpdateParkingLotDto("Best ParkingLot", 100, "11th Street");
            for (int i = 0; i < 20; i++)
            {
                await _httpClient.PostAsJsonAsync("/api/ParkingLots", parkingLot);
            }

            // when
            var parkingLotsInPageResponse = await _httpClient.GetAsync("/api/ParkingLots?pageNumber=1");
            var parkingLotsInPage = await GetObjectFromHttpResponse<List<ParkingLotDto>>(parkingLotsInPageResponse);

            // then
            Assert.Equal(15, parkingLotsInPage.Count);
        }

        [Fact]
        public async Task Should_return_a_parking_lot_when_get_by_id_give_a_existed_id()
        {
            // given
            var parkingLot = new CreateOrUpdateParkingLotDto("Best ParkingLot", 100, "11th Street");
            var createdResponse = await _httpClient.PostAsJsonAsync("/api/ParkingLots", parkingLot);
            var createdParkingLot = await GetObjectFromHttpResponse<ParkingLotDto>(createdResponse);

            // when
            var foundparkingLotResponse = await _httpClient.GetAsync($"/api/ParkingLots/{createdParkingLot.Id}");
            var foundParkingLot = await GetObjectFromHttpResponse<ParkingLotDto>(foundparkingLotResponse);

            // then
            Assert.Equal(createdParkingLot.Id, foundParkingLot.Id);
            Assert.Equal(createdParkingLot.Name, foundParkingLot.Name);
            Assert.Equal(createdParkingLot.Capacity, foundParkingLot.Capacity);
            Assert.Equal(createdParkingLot.Location, foundParkingLot.Location);
        }

        [Fact]
        public async Task Should_return_a_updated_parking_lot_when_update_parking_lot_given_a_valid_dto()
        {
            // given
            var parkingLot = new CreateOrUpdateParkingLotDto("Best ParkingLot", 100, "11th Street");
            var createdResponse = await _httpClient.PostAsJsonAsync("/api/ParkingLots", parkingLot);
            var createdParkingLot = await GetObjectFromHttpResponse<ParkingLotDto>(createdResponse);

            var newParkingLot = new CreateOrUpdateParkingLotDto("Best ParkingLot", 200, "12th Street");

            // when
            var updatedResponse = await _httpClient.PutAsJsonAsync($"/api/ParkingLots/{createdParkingLot.Id}", newParkingLot);
            var updatedParkingLot = await GetObjectFromHttpResponse<ParkingLotDto>(updatedResponse);

            // then
            Assert.Equal(createdParkingLot.Id, updatedParkingLot.Id);
            Assert.Equal(createdParkingLot.Name, updatedParkingLot.Name);
            Assert.Equal(200, updatedParkingLot.Capacity);
            Assert.Equal("12th Street", updatedParkingLot.Location);
        }

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