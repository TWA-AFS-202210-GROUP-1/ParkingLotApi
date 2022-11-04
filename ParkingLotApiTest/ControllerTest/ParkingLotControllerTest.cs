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
            var parkingLot = new ParkingLotDto("Best ParkingLot", 100, "11th Street");
            var createdParkingLotResponse = await _httpClient.PostAsJsonAsync("/api/ParkingLots", parkingLot);
            var createdParkingLot = await GetObjectFromHttpResponse<ParkingLotDto>(createdParkingLotResponse);

            Assert.Equal(parkingLot.Name, createdParkingLot.Name);
            Assert.Equal(parkingLot.Capacity, parkingLot.Capacity);
            Assert.Equal(parkingLot.Location, parkingLot.Location);
        }
    }
}