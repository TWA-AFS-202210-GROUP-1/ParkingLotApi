using System;
using ParkingLotApi.Dtos;
using System.Linq;
using System.Threading.Tasks;
using ParkingLotApi.Exceptions;
using System.Net.Http;
using ParkingLotApi.Models;

namespace ParkingLotApiTest.ServiceTest
{
    public class ParkingLotServiceTest : ServiceTestBase
    {
        [Fact]
        public async void Should_create_parking_lot_when_give_a_valid_dto()
        {
            // given
            var parkingLotDto = new CreateOrUpdateParkingLotDto("name", 10, "location");

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
            var parkingLotDto = new CreateOrUpdateParkingLotDto("name", -1, "location");

            // when
            // then
            await Assert.ThrowsAsync<InvalidParkingLotDtoException>(() => _parkingLotService.CreateParkingLot(parkingLotDto));
        }

        [Fact]
        public async void Should_delete_parking_lot_when_give_a_existed_id()
        {
            // given
            var parkingLotEntity = new ParkingLotEntity() { Name = "name", Capacity = 10, Location = "location" };
            await _parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
            await _parkingLotContext.SaveChangesAsync();

            // when
            await _parkingLotService.DeleteParkingLot(parkingLotEntity.Id);
            var parkingLotEntities = _parkingLotContext.ParkingLots.ToList();

            // then
            Assert.Empty(parkingLotEntities);
        }

        [Fact]
        public async void Should_throw_NotFoundParkingLotDtoException_when_delete_give_a_not_existed_id()
        {
            // given
            var parkingLotId = 1000;
            // when
            // then
            await Assert.ThrowsAsync<NotFoundParkingLotException>(() => _parkingLotService.DeleteParkingLot(parkingLotId));
        }

        [Fact]
        public async void Should_return_a_page_of_parking_lots_when_get_a_page_number()
        {
            // given
            
            for (int i = 0; i < 20; i++)
            {
                var parkingLotEntity = new ParkingLotEntity() { Name = "name", Capacity = i, Location = "location" };
                await _parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
                await _parkingLotContext.SaveChangesAsync();
            }
            
            // when
            var parkingLots = await _parkingLotService.GetParkingLotsByPageNumber(1);

            // then
            Assert.Equal(15, parkingLots.Count);
            Assert.True(parkingLots[0].Capacity < parkingLots[1].Capacity);
            Assert.True(parkingLots[0].Capacity < parkingLots[14].Capacity);
        }

        [Fact]
        public async void Should_return_all_parking_lot_when_this_page_has_no_more_than_15_parking_lot()
        {
            // given
            for (int i = 0; i < 20; i++)
            {
                var parkingLotEntity = new ParkingLotEntity() { Name = "name", Capacity = i, Location = "location" };
                await _parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
                await _parkingLotContext.SaveChangesAsync();
            }

            // when
            var parkingLots = await _parkingLotService.GetParkingLotsByPageNumber(2);

            // then
            Assert.Equal(5, parkingLots.Count);
        }

        [Fact]
        public async void Should_return_empty_when_get_a_page_number_out_of_index()
        {
            // given
            for (int i = 0; i < 20; i++)
            {
                var parkingLotEntity = new ParkingLotEntity() { Name = "name", Capacity = i, Location = "location" };
                await _parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
                await _parkingLotContext.SaveChangesAsync();
            }
            
            // when
            var parkingLots = await _parkingLotService.GetParkingLotsByPageNumber(3);

            // then
            Assert.Empty(parkingLots);
        }

        [Fact]
        public async void Should_return_parking_lot_when_get_by_id_give_a_existed_id()
        {
            // given
            var parkingLotEntity = new ParkingLotEntity() { Name = "name", Capacity = 100, Location = "location" };
            await _parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
            await _parkingLotContext.SaveChangesAsync();

            // when
            var parkingLot = await _parkingLotService.GetParkingLotById(parkingLotEntity.Id);

            // then
            Assert.Equal(parkingLotEntity.Name, parkingLot.Name);
            Assert.Equal(parkingLotEntity.Capacity, parkingLot.Capacity);
            Assert.Equal(parkingLotEntity.Location, parkingLot.Location);
        }

        [Fact]
        public async void Should_throw_NotFoundParkingLotDtoException_when_get_give_a_not_existed_id()
        {
            // given
            var parkingLotId = 1000;
            // when
            // then
            await Assert.ThrowsAsync<NotFoundParkingLotException>(() => _parkingLotService.GetParkingLotById(parkingLotId));
        }

        [Fact]
        public async void Should_return_new_parking_lot_when_update_by_id_give_a_existed_id()
        {
            // given
            var parkingLotEntity = new ParkingLotEntity() { Name = "name", Capacity = 100, Location = "location" };
            await _parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
            await _parkingLotContext.SaveChangesAsync();
            var newParkingLotDto = new CreateOrUpdateParkingLotDto("name", 200, "location 2");

            // when
            var updatedParkingLot = await _parkingLotService.UpdateParkingLotById(parkingLotEntity.Id, newParkingLotDto);

            // then
            Assert.Equal(200, updatedParkingLot.Capacity);
            Assert.Equal("location 2", updatedParkingLot.Location);
            Assert.Equal(parkingLotEntity.Name, updatedParkingLot.Name);
            Assert.Equal(parkingLotEntity.Capacity, updatedParkingLot.Capacity);
            Assert.Equal(parkingLotEntity.Location, updatedParkingLot.Location);
        }



        [Fact]
        public async void Should_throw_NotFoundParkingLotDtoException_when_update_give_a_not_existed_id()
        {
            // given
            var parkingLotId = 1000;
            var newParkingLotDto = new CreateOrUpdateParkingLotDto("name", 50, "location 2");
            // when
            // then
            await Assert.ThrowsAsync<NotFoundParkingLotException>(() => _parkingLotService.UpdateParkingLotById(parkingLotId, newParkingLotDto));
        }


        [Fact]
        public async void Should_throw_ForbidUpdateParkingLotException_when_new_capacity_less_than_not_existed_capacity()
        {
            // given
            var parkingLotEntity = new ParkingLotEntity() { Name = "name", Capacity = 100, Location = "location" };
            await _parkingLotContext.ParkingLots.AddAsync(parkingLotEntity);
            await _parkingLotContext.SaveChangesAsync();
            var newParkingLotDto = new CreateOrUpdateParkingLotDto("name", 50, "location 2");

            // when
            await Assert.ThrowsAsync<ForbidUpdateParkingLotException>(() => _parkingLotService.UpdateParkingLotById(parkingLotEntity.Id, newParkingLotDto));
        }



    }
}
