using Microsoft.EntityFrameworkCore;
using ParkingLotApi.Repository;
using ParkingLotApi.Services;
using System;

namespace ParkingLotApiTest.ServiceTest
{
    public class ServiceTestBase : IDisposable
    {
        protected readonly ParkingLotContext ParkingLotContext;
        protected readonly ParkingLotService ParkingLotService;
        protected readonly ParkingOrderService ParkingOrderService;

        public ServiceTestBase()
        {
            var options = new DbContextOptionsBuilder<ParkingLotContext>()
                .UseInMemoryDatabase(databaseName: $"{GetType()}")
                .Options;

            ParkingLotContext = new ParkingLotContext(options);
            ParkingLotService = new ParkingLotService(ParkingLotContext);
            ParkingOrderService = new ParkingOrderService(ParkingLotContext);

        }

        public async void Dispose()
        {
            ParkingLotContext.ParkingOrders.RemoveRange(ParkingLotContext.ParkingOrders);
            ParkingLotContext.ParkingLots.RemoveRange(ParkingLotContext.ParkingLots);
            await ParkingLotContext.SaveChangesAsync();
        }
    }
}
