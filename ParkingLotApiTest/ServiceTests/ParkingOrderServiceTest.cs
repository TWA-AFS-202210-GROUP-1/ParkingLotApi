using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Models;
using ParkingLotApi.Services;
using ParkingLotApiTest.Services;
using System;
using System.Net;

namespace ParkingLotApiTest.ServiceTests
{
  [Collection("SequenceAlpha")]
  public class ParkingOrderServiceTest : TestBase
  {
    public ParkingOrderServiceTest(CustomWebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact]
    public async void Should_create_order_with_id_when_add_one_given_valid_dto()
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
    public async void Should_throw_exception_when_create_order_given_unavailable_parking_lot()
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

    [Fact]
    public async void Should_throw_exception_when_create_order_given_unexisting_parking_lot()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(2, 1);
      await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      var parkingOrderService = new ParkingOrderService(dbContext);
      var parkingOrderDtos = TestService.PrepareParkingOrderDtos();

      // when
      var action = async () => { await parkingOrderService.CreateOrder(parkingOrderDtos[1]); };

      // then
      var exception = await Assert.ThrowsAsync<ParkingLotNotFoundException>(action);
      Assert.Equal($"Found no parking lot named {parkingOrderDtos[1].ParkingLot}.", exception.Message);
      Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async void Should_return_right_order_when_get_by_id_given_at_least_one_order_in_database()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      var parkingOrderService = new ParkingOrderService(dbContext);
      var parkingOrderDtos = TestService.PrepareParkingOrderDtos();
      var id = await parkingOrderService.CreateOrder(parkingOrderDtos[0]);

      // when
      var returnedDto = parkingOrderService.GetById(id);

      // then
      Assert.Equal(parkingOrderDtos[0].ToString(), returnedDto.ToString());
    }

    [Fact]
    public async void Should_throw_exception_when_get_by_id_given_wrong_id()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      var parkingOrderService = new ParkingOrderService(dbContext);
      var parkingOrderDtos = TestService.PrepareParkingOrderDtos();
      var id = await parkingOrderService.CreateOrder(parkingOrderDtos[0]);

      // when
      var action = () => { parkingOrderService.GetById(id + 1); };

      // then
      var exception = Assert.Throws<ParkingOrderNotFoundException>(action);
      Assert.Equal($"Found no parking order with id: {id + 1}.", exception.Message);
      Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async void Should_return_right_order_when_update_given_at_least_one_order_in_database()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      var parkingOrderService = new ParkingOrderService(dbContext);
      var parkingOrderDtos = TestService.PrepareParkingOrderDtos();
      var id = await parkingOrderService.CreateOrder(parkingOrderDtos[0]);
      var newOrderDto = new ParkingOrderDto
      {
        ParkingLot = parkingOrderDtos[0].ParkingLot,
        PlateNumber = parkingOrderDtos[0].PlateNumber,
        CreationTime = parkingOrderDtos[0].CreationTime,
        CloseTime = DateTime.Now,
        Status = OrderStatus.Close,
      };

      // when
      var returnedDto = await parkingOrderService.UpdateStatus(id, newOrderDto);

      // then
      Assert.Equal(newOrderDto.ToString(), returnedDto.ToString());
    }

    [Fact]
    public async void Should_throw_exception_when_update_given_wrong_id()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      var parkingOrderService = new ParkingOrderService(dbContext);
      var parkingOrderDtos = TestService.PrepareParkingOrderDtos();
      var id = await parkingOrderService.CreateOrder(parkingOrderDtos[0]);
      var newOrderDto = new ParkingOrderDto
      {
        ParkingLot = parkingOrderDtos[0].ParkingLot,
        PlateNumber = parkingOrderDtos[0].PlateNumber,
        CreationTime = parkingOrderDtos[0].CreationTime,
        CloseTime = DateTime.Now,
        Status = OrderStatus.Close,
      };

      // when
      var action = async () => { await parkingOrderService.UpdateStatus(id + 1, newOrderDto); };

      // then
      var exception = await Assert.ThrowsAsync<ParkingOrderNotFoundException>(action);
      Assert.Equal($"Found no parking order with id: {id + 1}.", exception.Message);
      Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }
  }
}
