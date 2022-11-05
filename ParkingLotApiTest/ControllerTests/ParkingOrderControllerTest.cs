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
using ParkingLotApi.Models;

namespace ParkingLotApiTest.ControllerTest
{
  [Collection("SequenceAlpha")]
  public class ParkingOrderControllerTest : TestBase
  {
    public ParkingOrderControllerTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async void Should_create_a_parking_order_with_id_when_post()
    {
      // given
      var httpClient = GetHttpClient();
      var parkingOrderDtos = TestService.PrepareParkingOrderDtos();
      var requestBody = TestService.SerializeDto(parkingOrderDtos[0]);

      // when
      var response = await httpClient.PostAsync("/orders", requestBody);

      // then
      var idString = await response.Content.ReadAsStringAsync();
      Assert.Equal(HttpStatusCode.Created, response.StatusCode);
      Assert.NotNull(idString);
    }

    [Fact]
    public async void Should_update_parking_order_status_when_put_given_id()
    {
      // given
      var httpClient = GetHttpClient();
      var parkingOrderDtos = TestService.PrepareParkingOrderDtos();
      var idList = await TestService.PostDtoList(httpClient, "/orders", parkingOrderDtos);
      parkingOrderDtos[0].Status = OrderStatus.Close;
      var requestBody = TestService.SerializeDto(parkingOrderDtos[0]);

      // when
      var putResponse = await httpClient.PutAsync($"/orders/{idList[0]}", requestBody);

      // then
      var response = await httpClient.GetAsync($"/orders/{idList[0]}");
      var returnedDto = await TestService.GetResponseContents<ParkingOrderDto>(response);
      Assert.Equal(HttpStatusCode.OK, putResponse.StatusCode);
      Assert.Equal(OrderStatus.Close, returnedDto?.Status);
    }
  }
}