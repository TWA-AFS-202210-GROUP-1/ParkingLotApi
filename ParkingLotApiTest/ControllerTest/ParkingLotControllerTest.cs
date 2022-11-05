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
    public async void Should_create_a_parking_lot_with_id_when_post()
    {
      // given
      var httpClient = GetHttpClient();
      var parkingLotDtos = TestService.PrepareParkingLotDtos();
      var requestBody = TestService.SerializeDto(parkingLotDtos[0]);

      // when
      var response = await httpClient.PostAsync("/parking-lots", requestBody);

      // then
      var idString = await response.Content.ReadAsStringAsync();
      Assert.Equal(HttpStatusCode.Created, response.StatusCode);
      Assert.NotNull(idString);
    }

    [Fact]
    public async void Should_return_all_parking_lots_when_get_given_multiple_parking_lots()
    {
      // given
      var httpClient = GetHttpClient();
      var parkingLotDtos = TestService.PrepareParkingLotDtos();
      await TestService.PostDtoList(httpClient, "/parking-lots", parkingLotDtos);

      // when
      var response = await httpClient.GetAsync("/parking-lots");

      // then
      var returnedDtos = await TestService.GetResponseContents<List<ParkingLotDto>>(response);
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Assert.Equal(parkingLotDtos.Count, returnedDtos?.Count);
      for (int index = 0; index < parkingLotDtos.Count; index++)
      {
        Assert.Equal(parkingLotDtos[index].ToString(), returnedDtos?[index].ToString());
      }
    }

    [Fact]
    public async void Should_return_a_parking_lot_when_get_given_id()
    {
      // given
      var httpClient = GetHttpClient();
      var parkingLotDtos = TestService.PrepareParkingLotDtos();
      var idList = await TestService.PostDtoList(httpClient, "/parking-lots", parkingLotDtos);

      // when
      var response = await httpClient.GetAsync($"/parking-lots/{idList[1]}");

      // then
      var returnedDto = await TestService.GetResponseContents<ParkingLotDto>(response);
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Assert.Equal(parkingLotDtos[1].ToString(), returnedDto?.ToString());
    }

    [Fact]
    public async void Should_return_a_page_of_parking_lots_when_get_given_page_index()
    {
      // given
      var httpClient = GetHttpClient();
      var parkingLotDtos = TestService.PrepareParkingLotDtos();
      await TestService.PostDtoList(httpClient, "/parking-lots", parkingLotDtos);

      // when
      var response = await httpClient.GetAsync($"/parking-lots?page=1");

      // then
      var returnedDtos = await TestService.GetResponseContents<List<ParkingLotDto>>(response);
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Assert.Equal(parkingLotDtos.Count, returnedDtos?.Count);
    }
  }
}