using System.Threading.Tasks;
using ParkingLotApi;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ParkingLotApi.Dtos;
using ParkingLotApiTest.Services;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Collections.Generic;

namespace ParkingLotApiTest.ControllerTest
{
  public class ParkingLotControllerTest : TestBase
  {
    public ParkingLotControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async void Should_create_parking_lot_when_post()
    {
      // given
      var httpClient = GetHttpClient();
      var parkingLotDtos = TestService.PrepareTestParkingLots();
      var requestBody = TestService.SerializeDto(parkingLotDtos[0]);

      // when
      var response = await httpClient.PostAsync("/parking-lots", requestBody);

      // then
      Assert.Equal(HttpStatusCode.Created, response.StatusCode);
      var idString = await response.Content.ReadAsStringAsync();
      Assert.Equal(1, int.Parse(idString));
    }

    [Fact]
    public async void Should_get_all_parking_lots_when_get_given_multiple_parking_lots()
    {
      // given
      var httpClient = GetHttpClient();
      var parkingLotDtos = TestService.PrepareTestParkingLots();
      await TestService.PostDtoList(httpClient, "/parking-lots", parkingLotDtos);

      // when
      var response = await httpClient.GetAsync("/parking-lots");

      // then
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      var responseBody = await response.Content.ReadAsStringAsync();
      var deserializeObject = JsonConvert.DeserializeObject<List<ParkingLotDto>>(responseBody);
      Assert.Equal(parkingLotDtos.Count, deserializeObject.Count);
      Assert.Equal(parkingLotDtos[0].ToString(), deserializeObject[0].ToString());
    }
  }
}