using System;
using ParkingLotApi.Dtos;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Exceptions;

namespace ParkingLotApiTest.ServiceTest
{
    public class ParkingLotServiceTest : ServiceTestBase
    {
        [Fact]
        public async void Should_create_parking_lot_when_give_a_valid_dto()
        {
            // given
            var parkingLotDto = new ParkingLotDto("name", 10, "location");

            // when
            var createdParkingLotDto = await _parkingLotService.CreateParkingLot(parkingLotDto);
            var parkingLotEntities = _parkingLotContext.ParkingLots.ToList();

            // then

            Assert.Single(parkingLotEntities);
            Assert.Equal(createdParkingLotDto.Id, parkingLotEntities[0].Id);
            Assert.Equal(parkingLotDto.Name, parkingLotEntities[0].Name);
            Assert.Equal(createdParkingLotDto.Name, parkingLotEntities[0].Name);
        }

        [Fact]
        public async void Should_throw_InvalidParkingLotDtoException_when_give_a_invalid_dto()
        {
            // given
            var parkingLotDto = new ParkingLotDto("name", -1, "location");

            // when
            // then
            await Assert.ThrowsAsync<InvalidParkingLotDtoException>(() => _parkingLotService.CreateParkingLot(parkingLotDto));
        }

        [Fact]
        public async void Should_delete_parking_lot_when_give_a_existed_id()
        {
            // given
            var parkingLotDto = new ParkingLotDto("name", 10, "location");

            // when
            var createdParkingLotDto = await _parkingLotService.CreateParkingLot(parkingLotDto);
            await _parkingLotService.DeleteParkingLot(createdParkingLotDto.Id);
            var parkingLotEntities = _parkingLotContext.ParkingLots.ToList();

            // then
            Assert.Empty(parkingLotEntities);
        }

        [Fact]
        public async void Should_throw_NoFoundParkingLotDtoException_when_give_a_not_existed_id()
        {
            // given
            var parkingLotId = 1000;
            // when
            // then
            await Assert.ThrowsAsync<NotFoundParkingLotException>(() => _parkingLotService.DeleteParkingLot(parkingLotId));
        }


    }
}
