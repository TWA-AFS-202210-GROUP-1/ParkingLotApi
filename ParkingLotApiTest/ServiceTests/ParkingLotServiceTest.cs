using ParkingLotApi.Dtos;
using ParkingLotApi.Exceptions;
using ParkingLotApi.Services;
using ParkingLotApiTest.Services;
using System.Linq;
using System.Net;

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

    [Fact]
    public async void Should_return_all_parking_lots_when_get_given_at_least_one_parking_lot_in_database()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      // when
      var returnedDtos = parkingLotService.GetAll();

      // then
      Assert.Single(returnedDtos);
      Assert.Equal(parkingLotDtos[0].ToString(), returnedDtos[0].ToString());
    }

    [Fact]
    public void Should_throw_exception_when_get_all_given_no_parking_lot_in_database()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);

      // when
      var action = () => { parkingLotService.GetAll(); };

      // then
      var exception = Assert.Throws<ParkingLotNotFoundException>(action);
      Assert.Equal("Found no parking lot.", exception.Message);
      Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async void Should_return_right_parking_lot_when_get_by_id_given_at_least_one_parking_lot_in_database()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      var id = await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      // when
      var returnedDto = parkingLotService.GetById(id);

      // then
      Assert.Equal(parkingLotDtos[0].ToString(), returnedDto.ToString());
    }

    [Fact]
    public async void Should_throw_exception_when_get_by_id_given_wrong_id()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      var id = await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      // when
      var action = () => { parkingLotService.GetById(id + 1); };

      // then
      var exception = Assert.Throws<ParkingLotNotFoundException>(action);
      Assert.Equal($"Found no parking lot with id: {id + 1}.", exception.Message);
      Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async void Should_return_right_parking_lots_when_get_by_page_given_20_parking_lots_in_database()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(20, 1);

      foreach (var parkingLotDto in parkingLotDtos)
      {
        await parkingLotService.AddParkingLot(parkingLotDto);
      }

      // when
      var returnedDtos = parkingLotService.GetByPageIndex(1);

      // then
      Assert.Equal(ParkingLotService.PageSize, returnedDtos.Count);
    }

    [Fact]
    public async void Should_throw_exception_when_get_by_page_given_exceeding_page_index()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(16, 1);

      foreach (var parkingLotDto in parkingLotDtos)
      {
        await parkingLotService.AddParkingLot(parkingLotDto);
      }

      // when
      var action = () => { parkingLotService.GetByPageIndex(3); };

      // then
      var exception = Assert.Throws<ParkingLotPageIndexOutOfRangeException>(action);
      Assert.Equal($"There is(are) only 2 page(s).", exception.Message);
      Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async void Should_change_parking_lot_capacity_when_update_given_different_capacity()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      var id = await parkingLotService.AddParkingLot(parkingLotDtos[0]);
      var newParkingLotDto = new ParkingLotDto(parkingLotDtos[0].Name, 20, parkingLotDtos[0].Location);

      // when
      var returnedDto = await parkingLotService.UpdateCapacity(id, newParkingLotDto);

      // then
      Assert.Equal(20, returnedDto.Capacity);
    }

    [Fact]
    public async void Should_throw_exception_when_update_given_wrong_id()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      var id = await parkingLotService.AddParkingLot(parkingLotDtos[0]);
      var newParkingLotDto = new ParkingLotDto(parkingLotDtos[0].Name, 20, parkingLotDtos[0].Location);

      // when
      var action = async () => { await parkingLotService.UpdateCapacity(id + 1, newParkingLotDto); };

      // then
      var exception = await Assert.ThrowsAsync<ParkingLotNotFoundException>(action);
      Assert.Equal($"Found no parking lot with id: {id + 1}.", exception.Message);
      Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }

    [Fact]
    public async void Should_throw_exception_when_update_given_invalid_capacity()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      var id = await parkingLotService.AddParkingLot(parkingLotDtos[0]);
      var newParkingLotDto = new ParkingLotDto(parkingLotDtos[0].Name, parkingLotDtos[0].Capacity - 1, parkingLotDtos[0].Location);

      // when
      var action = async () => { await parkingLotService.UpdateCapacity(id, newParkingLotDto); };

      // then
      var exception = await Assert.ThrowsAsync<InvalidParkingLotCapacityException>(action);
      Assert.Equal($"New capacity must not be smaller than the original, which is {parkingLotDtos[0].Capacity}.", exception.Message);
      Assert.Equal(HttpStatusCode.Conflict, exception.StatusCode);
    }

    [Fact]
    public async void Should_delete_parking_lot_when_remove_given_id()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(2, 1);
      var id = await parkingLotService.AddParkingLot(parkingLotDtos[0]);
      await parkingLotService.AddParkingLot(parkingLotDtos[1]);

      // when
      await parkingLotService.RemoveParkingLot(id);

      // then
      var returnedDtos = parkingLotService.GetAll();

      Assert.Single(returnedDtos);
    }

    [Fact]
    public async void Should_throw_exception_when_remove_given_wrong_id()
    {
      // given
      var dbContext = GetParkingLotDbContext();
      var parkingLotService = new ParkingLotService(dbContext);
      var parkingLotDtos = TestService.PrepareParkingLotDtos(1, 1);
      var id = await parkingLotService.AddParkingLot(parkingLotDtos[0]);

      // when
      var action = async () => { await parkingLotService.RemoveParkingLot(id + 1); };

      // then
      var exception = await Assert.ThrowsAsync<ParkingLotNotFoundException>(action);
      Assert.Equal($"Found no parking lot with id: {id + 1}.", exception.Message);
      Assert.Equal(HttpStatusCode.NotFound, exception.StatusCode);
    }
  }
}
