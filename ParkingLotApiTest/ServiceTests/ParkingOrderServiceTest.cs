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
  public class ParkingOrderServiceTest : TestBase
  {
    public ParkingOrderServiceTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async void Should_create_parking_order_with_id_when_add_one_given_valid_dto()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      var parkingOrderService = new ParkingOrderService(dbContext);
      var parkingOrderDtos = TestService.PrepareParkingOrderDtos();

      // when
      var id = await parkingOrderService.CreateOrder(parkingOrderDtos[0]);

      // then
      Assert.True(id > 0);
      Assert.Equal(parkingLotDtos[0].Name, parkingOrderDtos[0].ParkingLot);
    }

    [Fact]
    public async void Should_throw_exception_when_create_parking_order_given_unavailable_parking_lot()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      var parkingOrderService = new ParkingOrderService(dbContext);
      var parkingOrderDtos = TestService.PrepareParkingOrderDtos();
      await parkingOrderService.CreateOrder(parkingOrderDtos[0]);

      // when
      var action = async () => { await parkingOrderService.CreateOrder(parkingOrderDtos[0]); };

      // then
      var exception = await Assert.ThrowsAsync<ParkingLotFullException>(action);
      Assert.Equal($"The parking lot is full.", exception.Message);
      Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
    }
  }
}
