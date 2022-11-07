using Microsoft.Extensions.DependencyInjection;
using ParkingLotApi.Repository;
using System;
using System.Net.Http;

namespace ParkingLotApiTest
{
  public class TestBase : IClassFixture<CustomWebApplicationFactory<Program>>, IDisposable
  {
    public TestBase(CustomWebApplicationFactory<Program> factory)
    {
      Factory = factory;
    }

    protected CustomWebApplicationFactory<Program> Factory { get; private set; }

    public void Dispose()
    {
      var scope = Factory.Services.CreateScope();
      var scopedServices = scope.ServiceProvider;
      var context = scopedServices.GetRequiredService<ParkingLotDbContext>();

      context.ParkingOrders.RemoveRange(context.ParkingOrders);
      context.ParkingLots.RemoveRange(context.ParkingLots);

      context.SaveChanges();
    }

    protected HttpClient GetHttpClient()
    {
      return Factory.CreateClient();
    }

    protected ParkingLotDbContext GetParkingLotDbContext()
    {
      var scope = Factory.Services.CreateScope();
      var scopedService = scope.ServiceProvider;
      return scopedService.GetRequiredService<ParkingLotDbContext>();
    }
  }
}