using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using ParkingLotApiTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ParkingLotApiTest.ServiceTests
{
  [Collection("SequenceAlpha")]
  public class ParkingLotServiceTest : TestBase
  {
    public ParkingLotServiceTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async void Should_create_parking_lot_with_id_when_add_one_given_valid_dto()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);

      // when
      var id = await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      // then
      Assert.True(id > 0);
      Assert.Equal(parkingLotDtos[0].Name, dbContext.ParkingLots.First().Name);
    }

    [Fact]
    public async void Should_throw_exception_when_add_parking_lot_given_existing_name()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      // when
      var action = async () => { await parkingLotService.AddParkingLot(parkingLotDtos[0]); };

      // then
      var exception = await Assert.ThrowsAsync<DuplicateParkingLotNameException>(action);
      Assert.Equal($"Parking lot name: {parkingLotDtos[0].Name} already exists.", exception.Message);
      Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
    }
  }
}
