using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System;

namespace ParkingLotApiTest.ServiceTest
{
    public class ServiceTestBase : IDisposable
    {
        protected readonly ParkingLotService _parkingLotService;
        protected readonly ParkingLotContext _parkingLotContext;
        public ServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<ParkingLotContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            _parkingLotContext = new ParkingLotContext(options);
            _parkingLotService = new ParkingLotService(_parkingLotContext);
        }

        public void Dispose()
        {
            _parkingLotContext.ParkingLots.RemoveRange(_parkingLotContext.ParkingLots);
            _parkingLotContext.SaveChanges();
        }
    }
}
