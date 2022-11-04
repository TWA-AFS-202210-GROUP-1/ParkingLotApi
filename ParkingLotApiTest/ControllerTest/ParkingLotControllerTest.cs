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
            var parkingLot = new ParkingLotDto("Best ParkingLot", 100, "11th Street");
            
            // when
            var createdParkingLotResponse = await _httpClient.PostAsJsonAsync("/api/ParkingLots", parkingLot);
            var createdParkingLot = await GetObjectFromHttpResponse<CreatedParkingLotDto>(createdParkingLotResponse);

            // then
            Assert.Equal(parkingLot.Name, createdParkingLot.Name);
            Assert.Equal(parkingLot.Capacity, parkingLot.Capacity);
            Assert.Equal(parkingLot.Location, parkingLot.Location);
        }

        [Fact]
        public async Task Should_delete_parking_lot_when_delete_parkingLot_given_a_exitst_id()
        {
            // given
            var parkingLot = new ParkingLotDto("Best ParkingLot", 100, "11th Street");
            var createdParkingLotResponse = await _httpClient.PostAsJsonAsync("/api/ParkingLots", parkingLot);
            var createdParkingLot = await GetObjectFromHttpResponse<CreatedParkingLotDto>(createdParkingLotResponse);

            // when
            var deletedParkingLotResponse = await _httpClient.DeleteAsync($"/api/ParkingLots/{createdParkingLot.Id}");

            // then
            Assert.Equal(HttpStatusCode.NoContent, deletedParkingLotResponse.StatusCode);
        }
    }
}