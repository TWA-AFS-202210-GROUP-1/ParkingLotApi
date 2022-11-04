using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System;

namespace ParkingLotApiTest.ServiceTest
{
    public class ServiceTestBase : IDisposable
    {
        protected readonly ParkingLotContext ParkingLotContext;
        public ServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<ParkingLotContext>()
                .UseInMemoryDatabase(databaseName: "TestDB")
                .Options;

            ParkingLotContext = new ParkingLotContext(options);
        }

        public void Dispose()
        {
            ParkingLotContext.ParkingLots.RemoveRange(ParkingLotContext.ParkingLots);
            ParkingLotContext.SaveChanges();
        }
    }
}
