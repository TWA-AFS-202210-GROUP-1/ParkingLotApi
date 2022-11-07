using ParkingLotApi.Dtos;
using ParkingLotApiTest.Services;
using System.Net;
using System.Collections.Generic;
using ParkingLotApi.Services;

namespace ParkingLotApiTest.ControllerTest
{
  [Collection("SequenceAlpha")]
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
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
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
      var parkingLotDtos = TestService.PrepareParkingLotDtos(3, 1);
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
      var parkingLotDtos = TestService.PrepareParkingLotDtos(3, 1);
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
      var parkingLotDtos = TestService.PrepareParkingLotDtos(20, 1);
      await TestService.PostDtoList(httpClient, "/parking-lots", parkingLotDtos);

      // when
      var responsePage1 = await httpClient.GetAsync("/parking-lots?page=1");
      var responsePage2 = await httpClient.GetAsync("/parking-lots?page=2");

      // then
      var returnedDtosPage1 = await TestService.GetResponseContents<List<ParkingLotDto>>(responsePage1);
      var returnedDtosPage2 = await TestService.GetResponseContents<List<ParkingLotDto>>(responsePage2);
      Assert.Equal(HttpStatusCode.OK, responsePage1.StatusCode);
      Assert.Equal(HttpStatusCode.OK, responsePage2.StatusCode);
      Assert.Equal(ParkingLotService.PageSize, returnedDtosPage1?.Count);
      Assert.Equal(parkingLotDtos.Count - ParkingLotService.PageSize, returnedDtosPage2?.Count);
    }

    [Fact]
    public async void Should_remove_a_parking_lot_when_delete_given_id()
    {
      // given
      var httpClient = GetHttpClient();
      var parkingLotDtos = TestService.PrepareParkingLotDtos(3, 1);
      var idList = await TestService.PostDtoList(httpClient, "/parking-lots", parkingLotDtos);

      // when
      var deleteResponse = await httpClient.DeleteAsync($"/parking-lots/{idList[0]}");

      // then
      var response = await httpClient.GetAsync("/parking-lots");
      var returnedDtos = await TestService.GetResponseContents<List<ParkingLotDto>>(response);
      Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);
      Assert.Equal(parkingLotDtos.Count - 1, returnedDtos?.Count);
    }

    [Fact]
    public async void Should_change_parking_lot_capacity_when_put_given_id()
    {
      // given
      var httpClient = GetHttpClient();
      var parkingLotDtos = TestService.PrepareParkingLotDtos(3, 10);
      var idList = await TestService.PostDtoList(httpClient, "/parking-lots", parkingLotDtos);
      parkingLotDtos[2].Capacity = 15;
      var requestBody = TestService.SerializeDto(parkingLotDtos[2]);

      // when
      var putResponse = await httpClient.PutAsync($"/parking-lots/{idList[2]}", requestBody);

      // then
      var response = await httpClient.GetAsync($"/parking-lots/{idList[2]}");
      var returnedDto = await TestService.GetResponseContents<ParkingLotDto>(response);
      Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
      Assert.Equal(15, returnedDto?.Capacity);
    }
  }
}