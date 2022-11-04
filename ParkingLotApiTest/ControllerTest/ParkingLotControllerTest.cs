using System.Threading.Tasks;
using ParkingLotApi;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using ParkingLotApi.Dtos;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;

namespace ParkingLotApiTest.ControllerTest
{
  public class ParkingLotControllerTest
  {
    public ParkingLotControllerTest()
    {
    }

    [Fact]
    public async Task Should_create_parking_lot_when_post()
    {
      // given
      var factory = new WebApplicationFactory<Program>();
      var client = factory.CreateClient();
      ParkingLotDto parkingLotDto = new ParkingLotDto
      {
        Name = "ParkXpert",
        Capacity = 10,
        Location = "Aaron's Hill, Surrey",
      };

      // when
      var httpContent = JsonConvert.SerializeObject(parkingLotDto);
      var content = new StringContent(httpContent, Encoding.UTF8, "application/json");
      var allCompaniesResponse = await client.PostAsync("/parking-lots", content);
      var responseBody = await allCompaniesResponse.Content.ReadAsStringAsync();

      // then
      Assert.Equal(HttpStatusCode.Created, allCompaniesResponse.StatusCode);
    }
  }
}