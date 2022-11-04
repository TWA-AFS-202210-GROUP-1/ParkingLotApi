using System.Threading.Tasks;
using ParkingLotApi;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ParkingLotApi.Dtos;
using ParkingLotApiTest.Service;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace ParkingLotApiTest.ControllerTest
{
  public class ParkingLotControllerTest : TestBase
  {
    public ParkingLotControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async Task Should_create_parking_lot_when_post()
    {
      // given
      var httpClient = GetHttpClient();
      var parkingLotDtos = TestService.PrepareTestParkingLots();
      var requestBody = TestService.SerializeDto(parkingLotDtos[0]);

      // when
      var response = await httpClient.PostAsync("/parking-lots", requestBody);

      // then
      Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
  }
}