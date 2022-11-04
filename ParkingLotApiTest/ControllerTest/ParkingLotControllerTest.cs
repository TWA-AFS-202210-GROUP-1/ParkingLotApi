using System.Collections.Generic;
using System.Net;
using ParkingLotApi.Dtos;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ParkingLotApiTest.ControllerTest
{
    public class ParkingLotControllerTest : ControllerTestBase
    {
        [Fact]
        public async Task Should_return_created_parking_lot_when_post_new_parking_lot()
        {
            // given
            var parkingLot = new CreateParkingLotDto("Best ParkingLot", 100, "11th Street");
            
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
            var parkingLot = new CreateParkingLotDto("Best ParkingLot", 100, "11th Street");
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
            var parkingLot = new CreateParkingLotDto("Best ParkingLot", 100, "11th Street");
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
            var parkingLot = new CreateParkingLotDto("Best ParkingLot", 100, "11th Street");
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
    }
}